using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Vhd
{
	// Token: 0x0200000A RID: 10
	internal class DynamicStream : MappedStream
	{
		// Token: 0x0600007B RID: 123 RVA: 0x00003DAC File Offset: 0x00001FAC
		public DynamicStream(Stream fileStream, DynamicHeader dynamicHeader, long length, SparseStream parentStream, Ownership ownsParentStream)
		{
			if (fileStream == null)
			{
				throw new ArgumentNullException("fileStream");
			}
			if (dynamicHeader == null)
			{
				throw new ArgumentNullException("dynamicHeader");
			}
			if (parentStream == null)
			{
				throw new ArgumentNullException("parentStream");
			}
			if (length < 0L)
			{
				throw new ArgumentOutOfRangeException("length", length, "Negative lengths not allowed");
			}
			this._fileStream = fileStream;
			this._dynamicHeader = dynamicHeader;
			this._length = length;
			this._parentStream = parentStream;
			this._ownsParentStream = ownsParentStream;
			this._blockBitmaps = new byte[this._dynamicHeader.MaxTableEntries][];
			this._blockBitmapSize = (int)MathUtilities.RoundUp((long)((ulong)MathUtilities.Ceil(this._dynamicHeader.BlockSize, 4096U)), 512L);
			this.ReadBlockAllocationTable();
			this._fileStream.Position = MathUtilities.RoundDown(this._fileStream.Length, 512L) - 512L;
			Footer footer = Footer.FromBytes(StreamUtilities.ReadExact(this._fileStream, 512), 0);
			this._nextBlockStart = this._fileStream.Position - (footer.IsValid() ? 512L : 0L);
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00003ED7 File Offset: 0x000020D7
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00003EDF File Offset: 0x000020DF
		public bool AutoCommitFooter
		{
			get
			{
				return this._autoCommitFooter;
			}
			set
			{
				this._autoCommitFooter = value;
				if (this._autoCommitFooter)
				{
					this.UpdateFooter();
				}
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00003EF6 File Offset: 0x000020F6
		public override bool CanRead
		{
			get
			{
				this.CheckDisposed();
				return true;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00003EFF File Offset: 0x000020FF
		public override bool CanSeek
		{
			get
			{
				this.CheckDisposed();
				return true;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00003F08 File Offset: 0x00002108
		public override bool CanWrite
		{
			get
			{
				this.CheckDisposed();
				return this._fileStream.CanWrite;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00003F1B File Offset: 0x0000211B
		public override IEnumerable<StreamExtent> Extents
		{
			get
			{
				return this.GetExtentsInRange(0L, this.Length);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00003F2B File Offset: 0x0000212B
		public override long Length
		{
			get
			{
				this.CheckDisposed();
				return this._length;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00003F39 File Offset: 0x00002139
		// (set) Token: 0x06000084 RID: 132 RVA: 0x00003F47 File Offset: 0x00002147
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

		// Token: 0x06000085 RID: 133 RVA: 0x00003F5D File Offset: 0x0000215D
		public override void Flush()
		{
			this.CheckDisposed();
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003F65 File Offset: 0x00002165
		public override IEnumerable<StreamExtent> MapContent(long start, long length)
		{
			long position = start;
			int maxToRead = (int)Math.Min(length, this._length - position);
			int numRead = 0;
			while (numRead < maxToRead)
			{
				long num = position / (long)((ulong)this._dynamicHeader.BlockSize);
				uint num2 = (uint)(position % (long)((ulong)this._dynamicHeader.BlockSize));
				if (this.PopulateBlockBitmap(num))
				{
					int num3 = (int)(num2 / 512U);
					int num4 = (int)(num2 % 512U);
					int toRead = (int)Math.Min((long)(maxToRead - numRead), (long)((ulong)(this._dynamicHeader.BlockSize - num2)));
					if (num4 != 0 || toRead < 512)
					{
						byte b = (byte)(1 << 7 - num3 % 8);
						if ((this._blockBitmaps[(int)(checked((IntPtr)num))][num3 / 8] & b) != 0)
						{
							long start2 = (long)(((ulong)this._blockAllocationTable[(int)(checked((IntPtr)num))] + (ulong)((long)num3)) * 512UL + (ulong)((long)this._blockBitmapSize) + (ulong)((long)num4));
							yield return new StreamExtent(start2, (long)toRead);
						}
						numRead += toRead;
						position += (long)toRead;
					}
					else
					{
						int num5 = toRead / 512;
						byte b2 = (byte)(1 << 7 - num3 % 8);
						bool flag = (this._blockBitmaps[(int)(checked((IntPtr)num))][num3 / 8] & b2) == 0;
						int i;
						for (i = 1; i < num5; i++)
						{
							b2 = (byte)(1 << 7 - (num3 + i) % 8);
							if ((this._blockBitmaps[(int)(checked((IntPtr)num))][(num3 + i) / 8] & b2) == 0 != flag)
							{
								break;
							}
						}
						toRead = i * 512;
						if (!flag)
						{
							long start3 = (long)(((ulong)this._blockAllocationTable[(int)(checked((IntPtr)num))] + (ulong)((long)num3)) * 512UL + (ulong)((long)this._blockBitmapSize));
							yield return new StreamExtent(start3, (long)toRead);
						}
						numRead += toRead;
						position += (long)toRead;
					}
				}
				else
				{
					int num6 = Math.Min(maxToRead - numRead, (int)(this._dynamicHeader.BlockSize - num2));
					numRead += num6;
					position += (long)num6;
				}
			}
			yield break;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003F84 File Offset: 0x00002184
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
			int num = (int)Math.Min((long)count, this._length - this._position);
			int i = 0;
			while (i < num)
			{
				long num2 = this._position / (long)((ulong)this._dynamicHeader.BlockSize);
				uint num3 = (uint)(this._position % (long)((ulong)this._dynamicHeader.BlockSize));
				if (this.PopulateBlockBitmap(num2))
				{
					int num4 = (int)(num3 / 512U);
					int num5 = (int)(num3 % 512U);
					int num6 = (int)Math.Min((long)(num - i), (long)((ulong)(this._dynamicHeader.BlockSize - num3)));
					if (num5 != 0 || num6 < 512)
					{
						byte b = (byte)(1 << 7 - num4 % 8);
						if ((this._blockBitmaps[(int)(checked((IntPtr)num2))][num4 / 8] & b) != 0)
						{
							this._fileStream.Position = (long)(((ulong)this._blockAllocationTable[(int)(checked((IntPtr)num2))] + (ulong)((long)num4)) * 512UL + (ulong)((long)this._blockBitmapSize) + (ulong)((long)num5));
							StreamUtilities.ReadExact(this._fileStream, buffer, offset + i, num6);
						}
						else
						{
							this._parentStream.Position = this._position;
							StreamUtilities.ReadExact(this._parentStream, buffer, offset + i, num6);
						}
						i += num6;
						this._position += (long)num6;
					}
					else
					{
						int num7 = num6 / 512;
						byte b2 = (byte)(1 << 7 - num4 % 8);
						bool flag = (this._blockBitmaps[(int)(checked((IntPtr)num2))][num4 / 8] & b2) == 0;
						int j;
						for (j = 1; j < num7; j++)
						{
							b2 = (byte)(1 << 7 - (num4 + j) % 8);
							if ((this._blockBitmaps[(int)(checked((IntPtr)num2))][(num4 + j) / 8] & b2) == 0 != flag)
							{
								break;
							}
						}
						num6 = j * 512;
						if (flag)
						{
							this._parentStream.Position = this._position;
							StreamUtilities.ReadExact(this._parentStream, buffer, offset + i, num6);
						}
						else
						{
							this._fileStream.Position = (long)(((ulong)this._blockAllocationTable[(int)(checked((IntPtr)num2))] + (ulong)((long)num4)) * 512UL + (ulong)((long)this._blockBitmapSize));
							StreamUtilities.ReadExact(this._fileStream, buffer, offset + i, num6);
						}
						i += num6;
						this._position += (long)num6;
					}
				}
				else
				{
					int num8 = Math.Min(num - i, (int)(this._dynamicHeader.BlockSize - num3));
					this._parentStream.Position = this._position;
					StreamUtilities.ReadExact(this._parentStream, buffer, offset + i, num8);
					i += num8;
					this._position += (long)num8;
				}
			}
			return i;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004244 File Offset: 0x00002444
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

		// Token: 0x06000089 RID: 137 RVA: 0x00004299 File Offset: 0x00002499
		public override void SetLength(long value)
		{
			this.CheckDisposed();
			throw new NotSupportedException();
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000042A8 File Offset: 0x000024A8
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.CheckDisposed();
			if (!this.CanWrite)
			{
				throw new IOException("Attempt to write to read-only stream");
			}
			if (this._position + (long)count > this._length)
			{
				throw new IOException("Attempt to write beyond end of the stream");
			}
			int i = 0;
			while (i < count)
			{
				long num = this._position / (long)((ulong)this._dynamicHeader.BlockSize);
				uint num2 = (uint)(this._position % (long)((ulong)this._dynamicHeader.BlockSize));
				if (!this.PopulateBlockBitmap(num))
				{
					this.AllocateBlock(num);
				}
				int num3 = (int)(num2 / 512U);
				int num4 = (int)(num2 % 512U);
				int num5 = (int)Math.Min((long)(count - i), (long)((ulong)(this._dynamicHeader.BlockSize - num2)));
				bool flag = false;
				if (num4 != 0 || num5 < 512)
				{
					num5 = Math.Min(count - i, 512 - num4);
					byte b = (byte)(1 << 7 - num3 % 8);
					long position = (long)(((ulong)this._blockAllocationTable[(int)(checked((IntPtr)num))] + (ulong)((long)num3)) * 512UL + (ulong)((long)this._blockBitmapSize));
					byte[] array;
					if ((this._blockBitmaps[(int)(checked((IntPtr)num))][num3 / 8] & b) != 0)
					{
						this._fileStream.Position = position;
						array = StreamUtilities.ReadExact(this._fileStream, 512);
					}
					else
					{
						this._parentStream.Position = this._position / 512L * 512L;
						array = StreamUtilities.ReadExact(this._parentStream, 512);
					}
					Array.Copy(buffer, offset + i, array, num4, num5);
					this._fileStream.Position = position;
					this._fileStream.Write(array, 0, 512);
					checked
					{
						if ((this._blockBitmaps[(int)((IntPtr)num)][num3 / 8] & b) == 0)
						{
							byte[] array2 = this._blockBitmaps[(int)((IntPtr)num)];
							int num6 = num3 / 8;
							array2[num6] |= b;
							flag = true;
						}
					}
				}
				else
				{
					num5 = num5 / 512 * 512;
					this._fileStream.Position = (long)(((ulong)this._blockAllocationTable[(int)(checked((IntPtr)num))] + (ulong)((long)num3)) * 512UL + (ulong)((long)this._blockBitmapSize));
					this._fileStream.Write(buffer, offset + i, num5);
					for (int j = offset; j < offset + num5; j += 512)
					{
						byte b2 = (byte)(1 << 7 - num3 % 8);
						checked
						{
							if ((this._blockBitmaps[(int)((IntPtr)num)][num3 / 8] & b2) == 0)
							{
								byte[] array3 = this._blockBitmaps[(int)((IntPtr)num)];
								int num7 = num3 / 8;
								array3[num7] |= b2;
								flag = true;
							}
						}
						num3++;
					}
				}
				if (flag)
				{
					this.WriteBlockBitmap(num);
				}
				i += num5;
				this._position += (long)num5;
			}
			this._atEof = false;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004538 File Offset: 0x00002738
		public override IEnumerable<StreamExtent> GetExtentsInRange(long start, long count)
		{
			this.CheckDisposed();
			long num = Math.Min(this.Length, start + count) - start;
			if (num < 0L)
			{
				return new StreamExtent[0];
			}
			IEnumerable<StreamExtent> extentsInRange = this._parentStream.GetExtentsInRange(start, num);
			IEnumerable<StreamExtent> enumerable = StreamExtent.Union(new IEnumerable<StreamExtent>[]
			{
				this.LayerExtents(start, num),
				extentsInRange
			});
			return StreamExtent.Intersect(new IEnumerable<StreamExtent>[]
			{
				enumerable,
				new StreamExtent[]
				{
					new StreamExtent(start, num)
				}
			});
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000045B8 File Offset: 0x000027B8
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this.UpdateFooter();
					if (this._ownsParentStream == Ownership.Dispose && this._parentStream != null)
					{
						this._parentStream.Dispose();
						this._parentStream = null;
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000460C File Offset: 0x0000280C
		private IEnumerable<StreamExtent> LayerExtents(long start, long count)
		{
			long maxPos = start + count;
			long end;
			for (long num = this.FindNextPresentSector(MathUtilities.RoundDown(start, 512L), maxPos); num < maxPos; num = this.FindNextPresentSector(end, maxPos))
			{
				end = this.FindNextAbsentSector(num, maxPos);
				yield return new StreamExtent(num, end - num);
			}
			yield break;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000462C File Offset: 0x0000282C
		private long FindNextPresentSector(long pos, long maxPos)
		{
			bool flag = false;
			while (pos < maxPos && !flag)
			{
				long num = pos / (long)((ulong)this._dynamicHeader.BlockSize);
				if (!this.PopulateBlockBitmap(num))
				{
					pos += (long)((ulong)this._dynamicHeader.BlockSize);
				}
				else
				{
					int num2 = (int)((uint)(pos % (long)((ulong)this._dynamicHeader.BlockSize)) / 512U);
					if (this._blockBitmaps[(int)(checked((IntPtr)num))][num2 / 8] == 0)
					{
						pos += (long)((8 - num2 % 8) * 512);
					}
					else
					{
						byte b = (byte)(1 << 7 - num2 % 8);
						if ((this._blockBitmaps[(int)(checked((IntPtr)num))][num2 / 8] & b) != 0)
						{
							flag = true;
						}
						else
						{
							pos += 512L;
						}
					}
				}
			}
			return Math.Min(pos, maxPos);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000046E0 File Offset: 0x000028E0
		private long FindNextAbsentSector(long pos, long maxPos)
		{
			bool flag = false;
			while (pos < maxPos && !flag)
			{
				long num = pos / (long)((ulong)this._dynamicHeader.BlockSize);
				if (!this.PopulateBlockBitmap(num))
				{
					flag = true;
				}
				else
				{
					int num2 = (int)((uint)(pos % (long)((ulong)this._dynamicHeader.BlockSize)) / 512U);
					if (this._blockBitmaps[(int)(checked((IntPtr)num))][num2 / 8] == 255)
					{
						pos += (long)((8 - num2 % 8) * 512);
					}
					else
					{
						byte b = (byte)(1 << 7 - num2 % 8);
						if ((this._blockBitmaps[(int)(checked((IntPtr)num))][num2 / 8] & b) == 0)
						{
							flag = true;
						}
						else
						{
							pos += 512L;
						}
					}
				}
			}
			return Math.Min(pos, maxPos);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004788 File Offset: 0x00002988
		private void ReadBlockAllocationTable()
		{
			this._fileStream.Position = this._dynamicHeader.TableOffset;
			byte[] buffer = StreamUtilities.ReadExact(this._fileStream, this._dynamicHeader.MaxTableEntries * 4);
			uint[] array = new uint[this._dynamicHeader.MaxTableEntries];
			for (int i = 0; i < this._dynamicHeader.MaxTableEntries; i++)
			{
				array[i] = EndianUtilities.ToUInt32BigEndian(buffer, i * 4);
			}
			this._blockAllocationTable = array;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00004800 File Offset: 0x00002A00
		private bool PopulateBlockBitmap(long block)
		{
			checked
			{
				if (this._blockBitmaps[(int)((IntPtr)block)] != null)
				{
					return true;
				}
				if (this._blockAllocationTable[(int)((IntPtr)block)] == 4294967295U)
				{
					return false;
				}
				this._fileStream.Position = (long)(unchecked((ulong)this._blockAllocationTable[(int)(checked((IntPtr)block))] * 512UL));
				this._blockBitmaps[(int)((IntPtr)block)] = StreamUtilities.ReadExact(this._fileStream, this._blockBitmapSize);
				return true;
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004860 File Offset: 0x00002A60
		private void AllocateBlock(long block)
		{
			long nextBlockStart;
			checked
			{
				if (this._blockAllocationTable[(int)((IntPtr)block)] != 4294967295U)
				{
					throw new ArgumentException("Attempt to allocate existing block");
				}
				this._newBlocksAllocated = true;
				nextBlockStart = this._nextBlockStart;
				byte[] array = new byte[this._blockBitmapSize];
				this._fileStream.Position = nextBlockStart;
				this._fileStream.Write(array, 0, this._blockBitmapSize);
				this._blockBitmaps[(int)((IntPtr)block)] = array;
			}
			this._nextBlockStart += (long)this._blockBitmapSize + (long)((ulong)this._dynamicHeader.BlockSize);
			if (this._fileStream.Length < this._nextBlockStart)
			{
				this._fileStream.SetLength(this._nextBlockStart);
			}
			byte[] buffer = new byte[4];
			EndianUtilities.WriteBytesBigEndian((uint)(nextBlockStart / 512L), buffer, 0);
			this._fileStream.Position = this._dynamicHeader.TableOffset + block * 4L;
			this._fileStream.Write(buffer, 0, 4);
			this._blockAllocationTable[(int)(checked((IntPtr)block))] = (uint)(nextBlockStart / 512L);
			if (this._autoCommitFooter)
			{
				this.UpdateFooter();
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000496C File Offset: 0x00002B6C
		private void WriteBlockBitmap(long block)
		{
			this._fileStream.Position = (long)((ulong)this._blockAllocationTable[(int)(checked((IntPtr)block))] * 512UL);
			this._fileStream.Write(this._blockBitmaps[(int)(checked((IntPtr)block))], 0, this._blockBitmapSize);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000049A5 File Offset: 0x00002BA5
		private void CheckDisposed()
		{
			if (this._parentStream == null)
			{
				throw new ObjectDisposedException("DynamicStream", "Attempt to use closed stream");
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000049C0 File Offset: 0x00002BC0
		private void UpdateFooter()
		{
			if (this._newBlocksAllocated)
			{
				if (this._footerCache == null)
				{
					this._fileStream.Position = 0L;
					this._footerCache = StreamUtilities.ReadExact(this._fileStream, 512);
				}
				this._fileStream.Position = this._nextBlockStart;
				this._fileStream.Write(this._footerCache, 0, this._footerCache.Length);
			}
		}

		// Token: 0x0400001F RID: 31
		private bool _atEof;

		// Token: 0x04000020 RID: 32
		private bool _autoCommitFooter = true;

		// Token: 0x04000021 RID: 33
		private uint[] _blockAllocationTable;

		// Token: 0x04000022 RID: 34
		private readonly byte[][] _blockBitmaps;

		// Token: 0x04000023 RID: 35
		private readonly int _blockBitmapSize;

		// Token: 0x04000024 RID: 36
		private readonly DynamicHeader _dynamicHeader;

		// Token: 0x04000025 RID: 37
		private readonly Stream _fileStream;

		// Token: 0x04000026 RID: 38
		private byte[] _footerCache;

		// Token: 0x04000027 RID: 39
		private readonly long _length;

		// Token: 0x04000028 RID: 40
		private bool _newBlocksAllocated;

		// Token: 0x04000029 RID: 41
		private long _nextBlockStart;

		// Token: 0x0400002A RID: 42
		private readonly Ownership _ownsParentStream;

		// Token: 0x0400002B RID: 43
		private SparseStream _parentStream;

		// Token: 0x0400002C RID: 44
		private long _position;
	}
}
