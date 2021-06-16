using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x02000005 RID: 5
	internal sealed class ContentStream : MappedStream
	{
		// Token: 0x06000019 RID: 25 RVA: 0x000025E0 File Offset: 0x000007E0
		public ContentStream(SparseStream fileStream, bool? canWrite, Stream batStream, FreeSpaceTable freeSpaceTable, Metadata metadata, long length, SparseStream parentStream, Ownership ownsParent)
		{
			this._fileStream = fileStream;
			this._canWrite = canWrite;
			this._batStream = batStream;
			this._freeSpaceTable = freeSpaceTable;
			this._metadata = metadata;
			this._fileParameters = this._metadata.FileParameters;
			this._length = length;
			this._parentStream = parentStream;
			this._ownsParent = ownsParent;
			this._chunks = new ObjectCache<int, Chunk>();
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001A RID: 26 RVA: 0x0000264C File Offset: 0x0000084C
		public override bool CanRead
		{
			get
			{
				this.CheckDisposed();
				return true;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002655 File Offset: 0x00000855
		public override bool CanSeek
		{
			get
			{
				this.CheckDisposed();
				return true;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002660 File Offset: 0x00000860
		public override bool CanWrite
		{
			get
			{
				this.CheckDisposed();
				bool? canWrite = this._canWrite;
				if (canWrite == null)
				{
					return this._fileStream.CanWrite;
				}
				return canWrite.GetValueOrDefault();
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002696 File Offset: 0x00000896
		public override IEnumerable<StreamExtent> Extents
		{
			get
			{
				this.CheckDisposed();
				return this.GetExtentsInRange(0L, this.Length);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000026AC File Offset: 0x000008AC
		public override long Length
		{
			get
			{
				this.CheckDisposed();
				return this._length;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000026BA File Offset: 0x000008BA
		// (set) Token: 0x06000020 RID: 32 RVA: 0x000026C8 File Offset: 0x000008C8
		public override long Position
		{
			get
			{
				this.CheckDisposed();
				return this._position;
			}
			set
			{
				this.CheckDisposed();
				this._atEof = false;
				this._position = value;
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000026DE File Offset: 0x000008DE
		public override void Flush()
		{
			this.CheckDisposed();
			throw new NotImplementedException();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000026EB File Offset: 0x000008EB
		public override IEnumerable<StreamExtent> MapContent(long start, long length)
		{
			this.CheckDisposed();
			throw new NotImplementedException();
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000026F8 File Offset: 0x000008F8
		public override IEnumerable<StreamExtent> GetExtentsInRange(long start, long count)
		{
			this.CheckDisposed();
			return StreamExtent.Intersect(StreamExtent.Union(new IEnumerable<StreamExtent>[]
			{
				this.GetExtentsRaw(start, count),
				this._parentStream.GetExtentsInRange(start, count)
			}), new StreamExtent(start, count));
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002734 File Offset: 0x00000934
		public override int Read(byte[] buffer, int offset, int count)
		{
			this.CheckDisposed();
			if (this._atEof || this._position > this._length)
			{
				this._atEof = true;
				throw new IOException("Attempt to read beyond end of file");
			}
			if (this._position == this._length)
			{
				this._atEof = true;
				return 0;
			}
			if (this._position % (long)((ulong)this._metadata.LogicalSectorSize) != 0L || (long)count % (long)((ulong)this._metadata.LogicalSectorSize) != 0L)
			{
				throw new IOException("Unaligned read");
			}
			int num = (int)Math.Min(this._length - this._position, (long)count);
			int i = 0;
			while (i < num)
			{
				int num2;
				int block;
				int num3;
				Chunk chunk = this.GetChunk(this._position + (long)i, out num2, out block, out num3);
				int num4 = (int)((long)num3 * (long)((ulong)this._metadata.LogicalSectorSize));
				int val = (int)((ulong)this._fileParameters.BlockSize - (ulong)((long)num4));
				PayloadBlockStatus blockStatus = chunk.GetBlockStatus(block);
				if (blockStatus == PayloadBlockStatus.FullyPresent)
				{
					this._fileStream.Position = chunk.GetBlockPosition(block) + (long)num4;
					int num5 = StreamUtilities.ReadMaximum(this._fileStream, buffer, offset + i, Math.Min(val, num - i));
					i += num5;
				}
				else if (blockStatus == PayloadBlockStatus.PartiallyPresent)
				{
					bool flag;
					int count2 = (int)Math.Min((long)chunk.GetBlockBitmap(block).ContiguousSectors(num3, out flag) * (long)((ulong)this._metadata.LogicalSectorSize), (long)(num - i));
					int num6;
					if (flag)
					{
						this._fileStream.Position = chunk.GetBlockPosition(block) + (long)num4;
						num6 = StreamUtilities.ReadMaximum(this._fileStream, buffer, offset + i, count2);
					}
					else
					{
						this._parentStream.Position = this._position + (long)i;
						num6 = StreamUtilities.ReadMaximum(this._parentStream, buffer, offset + i, count2);
					}
					i += num6;
				}
				else if (blockStatus == PayloadBlockStatus.NotPresent)
				{
					this._parentStream.Position = this._position + (long)i;
					int num7 = StreamUtilities.ReadMaximum(this._parentStream, buffer, offset + i, Math.Min(val, num - i));
					i += num7;
				}
				else
				{
					int num8 = Math.Min(val, num - i);
					Array.Clear(buffer, offset + i, num8);
					i += num8;
				}
			}
			this._position += (long)i;
			return i;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000295C File Offset: 0x00000B5C
		public override long Seek(long offset, SeekOrigin origin)
		{
			this.CheckDisposed();
			long num = offset;
			if (origin == SeekOrigin.Current)
			{
				num += this._position;
			}
			else if (origin == SeekOrigin.End)
			{
				num += this._length;
			}
			this._atEof = false;
			if (num < 0L)
			{
				throw new IOException("Attempt to move before beginning of disk");
			}
			this._position = num;
			return this._position;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000029B1 File Offset: 0x00000BB1
		public override void SetLength(long value)
		{
			this.CheckDisposed();
			throw new NotSupportedException();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000029C0 File Offset: 0x00000BC0
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.CheckDisposed();
			if (!this.CanWrite)
			{
				throw new InvalidOperationException("Attempt to write to read-only VHDX");
			}
			if (this._position % (long)((ulong)this._metadata.LogicalSectorSize) != 0L || (long)count % (long)((ulong)this._metadata.LogicalSectorSize) != 0L)
			{
				throw new IOException("Unaligned read");
			}
			int i;
			int num4;
			for (i = 0; i < count; i += num4)
			{
				int num;
				int block;
				int num2;
				Chunk chunk = this.GetChunk(this._position + (long)i, out num, out block, out num2);
				int num3 = (int)((long)num2 * (long)((ulong)this._metadata.LogicalSectorSize));
				int val = (int)((ulong)this._fileParameters.BlockSize - (ulong)((long)num3));
				PayloadBlockStatus payloadBlockStatus = chunk.GetBlockStatus(block);
				if (payloadBlockStatus != PayloadBlockStatus.FullyPresent && payloadBlockStatus != PayloadBlockStatus.PartiallyPresent)
				{
					payloadBlockStatus = chunk.AllocateSpaceForBlock(block);
				}
				num4 = Math.Min(val, count - i);
				this._fileStream.Position = chunk.GetBlockPosition(block) + (long)num3;
				this._fileStream.Write(buffer, offset + i, num4);
				if (payloadBlockStatus == PayloadBlockStatus.PartiallyPresent && chunk.GetBlockBitmap(block).MarkSectorsPresent(num2, (int)((long)num4 / (long)((ulong)this._metadata.LogicalSectorSize))))
				{
					chunk.WriteBlockBitmap(block);
				}
			}
			this._position += (long)i;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002AF4 File Offset: 0x00000CF4
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (this._parentStream != null)
				{
					if (this._ownsParent == Ownership.Dispose)
					{
						this._parentStream.Dispose();
					}
					this._parentStream = null;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002B40 File Offset: 0x00000D40
		private IEnumerable<StreamExtent> GetExtentsRaw(long start, long count)
		{
			long chunkSize = (long)(8388608UL * (ulong)this._metadata.LogicalSectorSize);
			int chunkRatio = (int)(chunkSize / (long)((ulong)this._metadata.FileParameters.BlockSize));
			long pos = MathUtilities.RoundDown(start, chunkSize);
			while (pos < start + count)
			{
				int num;
				int num2;
				int num3;
				Chunk chunk = this.GetChunk(pos, out num, out num2, out num3);
				int num4;
				for (int i = 0; i < chunkRatio; i = num4)
				{
					PayloadBlockStatus blockStatus = chunk.GetBlockStatus(i);
					if (blockStatus > PayloadBlockStatus.Unmapped)
					{
						yield return new StreamExtent(pos + (long)i * (long)((ulong)this._metadata.FileParameters.BlockSize), (long)((ulong)this._metadata.FileParameters.BlockSize));
					}
					num4 = i + 1;
				}
				pos += chunkSize;
				chunk = null;
			}
			yield break;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002B60 File Offset: 0x00000D60
		private Chunk GetChunk(long position, out int chunk, out int block, out int sector)
		{
			long num = (long)(8388608UL * (ulong)this._metadata.LogicalSectorSize);
			int blocksPerChunk = (int)(num / (long)((ulong)this._metadata.FileParameters.BlockSize));
			chunk = (int)(position / num);
			long num2 = position % num;
			block = (int)(num2 / (long)((ulong)this._fileParameters.BlockSize));
			int num3 = (int)(num2 % (long)((ulong)this._fileParameters.BlockSize));
			sector = (int)((long)num3 / (long)((ulong)this._metadata.LogicalSectorSize));
			Chunk chunk2 = this._chunks[chunk];
			if (chunk2 == null)
			{
				chunk2 = new Chunk(this._batStream, this._fileStream, this._freeSpaceTable, this._fileParameters, chunk, blocksPerChunk);
				this._chunks[chunk] = chunk2;
			}
			return chunk2;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002C1A File Offset: 0x00000E1A
		private void CheckDisposed()
		{
			if (this._parentStream == null)
			{
				throw new ObjectDisposedException("ContentStream", "Attempt to use closed stream");
			}
		}

		// Token: 0x0400000E RID: 14
		private bool _atEof;

		// Token: 0x0400000F RID: 15
		private readonly Stream _batStream;

		// Token: 0x04000010 RID: 16
		private readonly bool? _canWrite;

		// Token: 0x04000011 RID: 17
		private readonly ObjectCache<int, Chunk> _chunks;

		// Token: 0x04000012 RID: 18
		private readonly FileParameters _fileParameters;

		// Token: 0x04000013 RID: 19
		private readonly SparseStream _fileStream;

		// Token: 0x04000014 RID: 20
		private readonly FreeSpaceTable _freeSpaceTable;

		// Token: 0x04000015 RID: 21
		private readonly long _length;

		// Token: 0x04000016 RID: 22
		private readonly Metadata _metadata;

		// Token: 0x04000017 RID: 23
		private readonly Ownership _ownsParent;

		// Token: 0x04000018 RID: 24
		private SparseStream _parentStream;

		// Token: 0x04000019 RID: 25
		private long _position;
	}
}
