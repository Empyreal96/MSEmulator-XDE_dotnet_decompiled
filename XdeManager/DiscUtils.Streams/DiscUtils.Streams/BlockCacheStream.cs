using System;
using System.Collections.Generic;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x02000007 RID: 7
	public sealed class BlockCacheStream : SparseStream
	{
		// Token: 0x06000037 RID: 55 RVA: 0x0000280D File Offset: 0x00000A0D
		public BlockCacheStream(SparseStream toWrap, Ownership ownership) : this(toWrap, ownership, new BlockCacheSettings())
		{
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000281C File Offset: 0x00000A1C
		public BlockCacheStream(SparseStream toWrap, Ownership ownership, BlockCacheSettings settings)
		{
			if (!toWrap.CanRead)
			{
				throw new ArgumentException("The wrapped stream does not support reading", "toWrap");
			}
			if (!toWrap.CanSeek)
			{
				throw new ArgumentException("The wrapped stream does not support seeking", "toWrap");
			}
			this._wrappedStream = toWrap;
			this._ownWrapped = ownership;
			this._settings = new BlockCacheSettings(settings);
			if (this._settings.OptimumReadSize % this._settings.BlockSize != 0)
			{
				throw new ArgumentException("Invalid settings, OptimumReadSize must be a multiple of BlockSize", "settings");
			}
			this._readBuffer = new byte[this._settings.OptimumReadSize];
			this._blocksInReadBuffer = this._settings.OptimumReadSize / this._settings.BlockSize;
			int num = (int)(this._settings.ReadCacheSize / (long)this._settings.BlockSize);
			this._cache = new BlockCache<Block>(this._settings.BlockSize, num);
			this._stats = new BlockCacheStatistics();
			this._stats.FreeReadBlocks = num;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000039 RID: 57 RVA: 0x0000291D File Offset: 0x00000B1D
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002920 File Offset: 0x00000B20
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002923 File Offset: 0x00000B23
		public override bool CanWrite
		{
			get
			{
				return this._wrappedStream.CanWrite;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002930 File Offset: 0x00000B30
		public override IEnumerable<StreamExtent> Extents
		{
			get
			{
				this.CheckDisposed();
				return this._wrappedStream.Extents;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002943 File Offset: 0x00000B43
		public override long Length
		{
			get
			{
				this.CheckDisposed();
				return this._wrappedStream.Length;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002956 File Offset: 0x00000B56
		// (set) Token: 0x0600003F RID: 63 RVA: 0x00002964 File Offset: 0x00000B64
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
				this._position = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002973 File Offset: 0x00000B73
		public BlockCacheStatistics Statistics
		{
			get
			{
				this._stats.FreeReadBlocks = this._cache.FreeBlockCount;
				return this._stats;
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002991 File Offset: 0x00000B91
		public override IEnumerable<StreamExtent> GetExtentsInRange(long start, long count)
		{
			this.CheckDisposed();
			return this._wrappedStream.GetExtentsInRange(start, count);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000029A8 File Offset: 0x00000BA8
		public override int Read(byte[] buffer, int offset, int count)
		{
			this.CheckDisposed();
			if (this._position >= this.Length)
			{
				if (this._atEof)
				{
					throw new IOException("Attempt to read beyond end of stream");
				}
				this._atEof = true;
				return 0;
			}
			else
			{
				BlockCacheStatistics stats = this._stats;
				long num = stats.TotalReadsIn;
				stats.TotalReadsIn = num + 1L;
				if ((long)count > this._settings.LargeReadSize)
				{
					BlockCacheStatistics stats2 = this._stats;
					num = stats2.LargeReadsIn;
					stats2.LargeReadsIn = num + 1L;
					BlockCacheStatistics stats3 = this._stats;
					num = stats3.TotalReadsOut;
					stats3.TotalReadsOut = num + 1L;
					this._wrappedStream.Position = this._position;
					int result = this._wrappedStream.Read(buffer, offset, count);
					this._position = this._wrappedStream.Position;
					if (this._position >= this.Length)
					{
						this._atEof = true;
					}
					return result;
				}
				int num2 = 0;
				bool flag = false;
				bool flag2 = false;
				int blockSize = this._settings.BlockSize;
				long num3 = this._position / (long)blockSize;
				int num4 = (int)(this._position % (long)blockSize);
				int num5 = (int)(MathUtilities.Ceil(Math.Min(this._position + (long)count, this.Length), (long)blockSize) - num3);
				if (num4 != 0)
				{
					BlockCacheStatistics stats4 = this._stats;
					num = stats4.UnalignedReadsIn;
					stats4.UnalignedReadsIn = num + 1L;
				}
				int i = 0;
				while (i < num5)
				{
					Block block;
					while (i < num5 && this._cache.TryGetBlock(num3 + (long)i, out block))
					{
						int num6 = Math.Min(count - num2, block.Available - num4);
						Array.Copy(block.Data, num4, buffer, offset + num2, num6);
						num4 = 0;
						num2 += num6;
						this._position += (long)num6;
						i++;
						flag = true;
					}
					if (i < num5 && !this._cache.ContainsBlock(num3 + (long)i))
					{
						flag2 = true;
						int num7 = 0;
						while (i + num7 < num5 && num7 < this._blocksInReadBuffer && !this._cache.ContainsBlock(num3 + (long)i + (long)num7))
						{
							num7++;
						}
						long num8 = (num3 + (long)i) * (long)blockSize;
						int num9 = (int)Math.Min((long)num7 * (long)blockSize, this.Length - num8);
						BlockCacheStatistics stats5 = this._stats;
						num = stats5.TotalReadsOut;
						stats5.TotalReadsOut = num + 1L;
						this._wrappedStream.Position = num8;
						StreamUtilities.ReadExact(this._wrappedStream, this._readBuffer, 0, num9);
						for (int j = 0; j < num7; j++)
						{
							int num10 = Math.Min(blockSize, num9 - j * blockSize);
							block = this._cache.GetBlock(num3 + (long)i + (long)j);
							Array.Copy(this._readBuffer, j * blockSize, block.Data, 0, num10);
							block.Available = num10;
							if (num10 < blockSize)
							{
								Array.Clear(this._readBuffer, j * blockSize + num10, blockSize - num10);
							}
						}
						i += num7;
						int num11 = Math.Min(count - num2, num9 - num4);
						Array.Copy(this._readBuffer, num4, buffer, offset + num2, num11);
						num2 += num11;
						this._position += (long)num11;
						num4 = 0;
					}
				}
				if (this._position >= this.Length && num2 == 0)
				{
					this._atEof = true;
				}
				if (flag)
				{
					BlockCacheStatistics stats6 = this._stats;
					num = stats6.ReadCacheHits;
					stats6.ReadCacheHits = num + 1L;
				}
				if (flag2)
				{
					BlockCacheStatistics stats7 = this._stats;
					num = stats7.ReadCacheMisses;
					stats7.ReadCacheMisses = num + 1L;
				}
				return num2;
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002D11 File Offset: 0x00000F11
		public override void Flush()
		{
			this.CheckDisposed();
			this._wrappedStream.Flush();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002D24 File Offset: 0x00000F24
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
				num += this.Length;
			}
			this._atEof = false;
			if (num < 0L)
			{
				throw new IOException("Attempt to move before beginning of disk");
			}
			this._position = num;
			return this._position;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002D79 File Offset: 0x00000F79
		public override void SetLength(long value)
		{
			this.CheckDisposed();
			this._wrappedStream.SetLength(value);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002D90 File Offset: 0x00000F90
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.CheckDisposed();
			BlockCacheStatistics stats = this._stats;
			long num = stats.TotalWritesIn;
			stats.TotalWritesIn = num + 1L;
			int blockSize = this._settings.BlockSize;
			long num2 = this._position / (long)blockSize;
			int num3 = (int)(MathUtilities.Ceil(Math.Min(this._position + (long)count, this.Length), (long)blockSize) - num2);
			try
			{
				this._wrappedStream.Position = this._position;
				this._wrappedStream.Write(buffer, offset, count);
			}
			catch
			{
				this.InvalidateBlocks(num2, num3);
				throw;
			}
			int num4 = (int)(this._position % (long)blockSize);
			if (num4 != 0)
			{
				BlockCacheStatistics stats2 = this._stats;
				num = stats2.UnalignedWritesIn;
				stats2.UnalignedWritesIn = num + 1L;
			}
			int num5 = 0;
			for (int i = 0; i < num3; i++)
			{
				int sourceIndex = offset + num5;
				int num6 = Math.Min(count - num5, blockSize - num4);
				Block block;
				if (this._cache.TryGetBlock(num2 + (long)i, out block))
				{
					Array.Copy(buffer, sourceIndex, block.Data, num4, num6);
					block.Available = Math.Max(block.Available, num4 + num6);
				}
				num4 = 0;
				num5 += num6;
			}
			this._position += (long)count;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002ED0 File Offset: 0x000010D0
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this._wrappedStream != null && this._ownWrapped == Ownership.Dispose)
				{
					this._wrappedStream.Dispose();
				}
				this._wrappedStream = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002EFF File Offset: 0x000010FF
		private void CheckDisposed()
		{
			if (this._wrappedStream == null)
			{
				throw new ObjectDisposedException("BlockCacheStream");
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002F14 File Offset: 0x00001114
		private void InvalidateBlocks(long firstBlock, int numBlocks)
		{
			for (long num = firstBlock; num < firstBlock + (long)numBlocks; num += 1L)
			{
				this._cache.ReleaseBlock(num);
			}
		}

		// Token: 0x0400001B RID: 27
		private bool _atEof;

		// Token: 0x0400001C RID: 28
		private readonly int _blocksInReadBuffer;

		// Token: 0x0400001D RID: 29
		private readonly BlockCache<Block> _cache;

		// Token: 0x0400001E RID: 30
		private readonly Ownership _ownWrapped;

		// Token: 0x0400001F RID: 31
		private long _position;

		// Token: 0x04000020 RID: 32
		private readonly byte[] _readBuffer;

		// Token: 0x04000021 RID: 33
		private readonly BlockCacheSettings _settings;

		// Token: 0x04000022 RID: 34
		private readonly BlockCacheStatistics _stats;

		// Token: 0x04000023 RID: 35
		private SparseStream _wrappedStream;
	}
}
