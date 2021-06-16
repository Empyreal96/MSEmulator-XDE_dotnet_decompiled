using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Vhd
{
	// Token: 0x02000008 RID: 8
	internal sealed class DynamicDiskBuilder : StreamBuilder
	{
		// Token: 0x0600006F RID: 111 RVA: 0x0000378F File Offset: 0x0000198F
		public DynamicDiskBuilder(SparseStream content, Footer footer, uint blockSize)
		{
			this._content = content;
			this._footer = footer;
			this._blockSize = blockSize;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000037AC File Offset: 0x000019AC
		protected override List<BuilderExtent> FixExtents(out long totalLength)
		{
			List<BuilderExtent> list = new List<BuilderExtent>();
			this._footer.DataOffset = 512L;
			DynamicHeader dynamicHeader = new DynamicHeader(-1L, 1536L, this._blockSize, this._footer.CurrentSize);
			DynamicDiskBuilder.BlockAllocationTableExtent blockAllocationTableExtent = new DynamicDiskBuilder.BlockAllocationTableExtent(1536L, dynamicHeader.MaxTableEntries);
			long num = blockAllocationTableExtent.Start + blockAllocationTableExtent.Length;
			foreach (Range<long, long> range in StreamExtent.Blocks(this._content.Extents, (long)((ulong)this._blockSize)))
			{
				int num2 = 0;
				while ((long)num2 < range.Count)
				{
					long num3 = range.Offset + (long)num2;
					long num4 = num3 * (long)((ulong)this._blockSize);
					DynamicDiskBuilder.DataBlockExtent dataBlockExtent = new DynamicDiskBuilder.DataBlockExtent(num, new SubStream(this._content, num4, Math.Min((long)((ulong)this._blockSize), this._content.Length - num4)));
					list.Add(dataBlockExtent);
					blockAllocationTableExtent.SetEntry((int)num3, (uint)(num / 512L));
					num += dataBlockExtent.Length;
					num2++;
				}
			}
			this._footer.UpdateChecksum();
			dynamicHeader.UpdateChecksum();
			byte[] buffer = new byte[512];
			this._footer.ToBytes(buffer, 0);
			byte[] array = new byte[1024];
			dynamicHeader.ToBytes(array, 0);
			list.Add(new BuilderBufferExtent(num, buffer));
			totalLength = num + 512L;
			list.Insert(0, blockAllocationTableExtent);
			list.Insert(0, new BuilderBufferExtent(512L, array));
			list.Insert(0, new BuilderBufferExtent(0L, buffer));
			return list;
		}

		// Token: 0x0400000E RID: 14
		private readonly uint _blockSize;

		// Token: 0x0400000F RID: 15
		private readonly SparseStream _content;

		// Token: 0x04000010 RID: 16
		private readonly Footer _footer;

		// Token: 0x02000012 RID: 18
		private class BlockAllocationTableExtent : BuilderExtent
		{
			// Token: 0x060000BF RID: 191 RVA: 0x00005B90 File Offset: 0x00003D90
			public BlockAllocationTableExtent(long start, int maxEntries) : base(start, (long)MathUtilities.RoundUp(maxEntries * 4, 512))
			{
				this._entries = new uint[base.Length / 4L];
				for (int i = 0; i < this._entries.Length; i++)
				{
					this._entries[i] = uint.MaxValue;
				}
			}

			// Token: 0x060000C0 RID: 192 RVA: 0x00005BE3 File Offset: 0x00003DE3
			public override void Dispose()
			{
				if (this._dataStream != null)
				{
					this._dataStream.Dispose();
					this._dataStream = null;
				}
			}

			// Token: 0x060000C1 RID: 193 RVA: 0x00005BFF File Offset: 0x00003DFF
			public void SetEntry(int index, uint fileSector)
			{
				this._entries[index] = fileSector;
			}

			// Token: 0x060000C2 RID: 194 RVA: 0x00005C0C File Offset: 0x00003E0C
			public override void PrepareForRead()
			{
				byte[] buffer = new byte[base.Length];
				for (int i = 0; i < this._entries.Length; i++)
				{
					EndianUtilities.WriteBytesBigEndian(this._entries[i], buffer, i * 4);
				}
				this._dataStream = new MemoryStream(buffer, false);
			}

			// Token: 0x060000C3 RID: 195 RVA: 0x00005C57 File Offset: 0x00003E57
			public override int Read(long diskOffset, byte[] block, int offset, int count)
			{
				this._dataStream.Position = diskOffset - base.Start;
				return this._dataStream.Read(block, offset, count);
			}

			// Token: 0x060000C4 RID: 196 RVA: 0x00005C7B File Offset: 0x00003E7B
			public override void DisposeReadState()
			{
				if (this._dataStream != null)
				{
					this._dataStream.Dispose();
					this._dataStream = null;
				}
			}

			// Token: 0x04000067 RID: 103
			private MemoryStream _dataStream;

			// Token: 0x04000068 RID: 104
			private readonly uint[] _entries;
		}

		// Token: 0x02000013 RID: 19
		private class DataBlockExtent : BuilderExtent
		{
			// Token: 0x060000C5 RID: 197 RVA: 0x00005C97 File Offset: 0x00003E97
			public DataBlockExtent(long start, SparseStream content) : this(start, content, Ownership.None)
			{
			}

			// Token: 0x060000C6 RID: 198 RVA: 0x00005CA4 File Offset: 0x00003EA4
			public DataBlockExtent(long start, SparseStream content, Ownership ownership) : base(start, MathUtilities.RoundUp(MathUtilities.Ceil(content.Length, 512L) / 8L, 512L) + MathUtilities.RoundUp(content.Length, 512L))
			{
				this._content = content;
				this._ownership = ownership;
			}

			// Token: 0x060000C7 RID: 199 RVA: 0x00005CF7 File Offset: 0x00003EF7
			public override void Dispose()
			{
				if (this._content != null && this._ownership == Ownership.Dispose)
				{
					this._content.Dispose();
					this._content = null;
				}
				if (this._bitmapStream != null)
				{
					this._bitmapStream.Dispose();
					this._bitmapStream = null;
				}
			}

			// Token: 0x060000C8 RID: 200 RVA: 0x00005D38 File Offset: 0x00003F38
			public override void PrepareForRead()
			{
				byte[] array = new byte[MathUtilities.RoundUp(MathUtilities.Ceil(this._content.Length, 512L) / 8L, 512L)];
				foreach (Range<long, long> range in StreamExtent.Blocks(this._content.Extents, 512L))
				{
					int num = 0;
					while ((long)num < range.Count)
					{
						byte b = (byte)(1 << 7 - (int)(range.Offset + (long)num) % 8);
						byte[] array2 = array;
						IntPtr intPtr = checked((IntPtr)(unchecked(range.Offset + (long)num) / 8L));
						array2[(int)intPtr] = (array2[(int)intPtr] | b);
						num++;
					}
				}
				this._bitmapStream = new MemoryStream(array, false);
			}

			// Token: 0x060000C9 RID: 201 RVA: 0x00005E08 File Offset: 0x00004008
			public override int Read(long diskOffset, byte[] block, int offset, int count)
			{
				long num = diskOffset - base.Start;
				if (num < this._bitmapStream.Length)
				{
					this._bitmapStream.Position = num;
					return this._bitmapStream.Read(block, offset, count);
				}
				this._content.Position = num - this._bitmapStream.Length;
				return this._content.Read(block, offset, count);
			}

			// Token: 0x060000CA RID: 202 RVA: 0x00005E6F File Offset: 0x0000406F
			public override void DisposeReadState()
			{
				if (this._bitmapStream != null)
				{
					this._bitmapStream.Dispose();
					this._bitmapStream = null;
				}
			}

			// Token: 0x04000069 RID: 105
			private MemoryStream _bitmapStream;

			// Token: 0x0400006A RID: 106
			private SparseStream _content;

			// Token: 0x0400006B RID: 107
			private readonly Ownership _ownership;
		}
	}
}
