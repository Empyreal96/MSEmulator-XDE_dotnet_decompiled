using System;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x02000004 RID: 4
	internal sealed class Chunk
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002264 File Offset: 0x00000464
		public Chunk(Stream bat, SparseStream file, FreeSpaceTable freeSpace, FileParameters fileParameters, int chunk, int blocksPerChunk)
		{
			this._bat = bat;
			this._file = file;
			this._freeSpace = freeSpace;
			this._fileParameters = fileParameters;
			this._chunk = chunk;
			this._blocksPerChunk = blocksPerChunk;
			this._bat.Position = (long)(this._chunk * (this._blocksPerChunk + 1) * 8);
			this._batData = StreamUtilities.ReadExact(bat, (this._blocksPerChunk + 1) * 8);
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000022D8 File Offset: 0x000004D8
		private bool HasSectorBitmap
		{
			get
			{
				return new BatEntry(this._batData, this._blocksPerChunk * 8).BitmapBlockPresent;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002300 File Offset: 0x00000500
		// (set) Token: 0x06000011 RID: 17 RVA: 0x00002330 File Offset: 0x00000530
		private long SectorBitmapPos
		{
			get
			{
				return new BatEntry(this._batData, this._blocksPerChunk * 8).FileOffsetMB * 1048576L;
			}
			set
			{
				BatEntry batEntry = default(BatEntry);
				batEntry.BitmapBlockPresent = (value != 0L);
				batEntry.FileOffsetMB = value / 1048576L;
				batEntry.WriteTo(this._batData, this._blocksPerChunk * 8);
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002378 File Offset: 0x00000578
		public long GetBlockPosition(int block)
		{
			return new BatEntry(this._batData, block * 8).FileOffsetMB * 1048576L;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000023A4 File Offset: 0x000005A4
		public PayloadBlockStatus GetBlockStatus(int block)
		{
			return new BatEntry(this._batData, block * 8).PayloadBlockStatus;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000023C8 File Offset: 0x000005C8
		public BlockBitmap GetBlockBitmap(int block)
		{
			int num = (int)(1048576L / (long)this._blocksPerChunk);
			int offset = num * block;
			return new BlockBitmap(this.LoadSectorBitmap(), offset, num);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000023F8 File Offset: 0x000005F8
		public void WriteBlockBitmap(int block)
		{
			int num = (int)(1048576L / (long)this._blocksPerChunk);
			int num2 = num * block;
			this._file.Position = this.SectorBitmapPos + (long)num2;
			this._file.Write(this._sectorBitmap, num2, num);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002440 File Offset: 0x00000640
		public PayloadBlockStatus AllocateSpaceForBlock(int block)
		{
			bool flag = false;
			BatEntry batEntry = new BatEntry(this._batData, block * 8);
			if (batEntry.FileOffsetMB == 0L)
			{
				batEntry.FileOffsetMB = this.AllocateSpace((int)this._fileParameters.BlockSize, false) / 1048576L;
				flag = true;
			}
			if (batEntry.PayloadBlockStatus != PayloadBlockStatus.FullyPresent && batEntry.PayloadBlockStatus != PayloadBlockStatus.PartiallyPresent)
			{
				if ((this._fileParameters.Flags & FileParametersFlags.HasParent) != FileParametersFlags.None)
				{
					if (!this.HasSectorBitmap)
					{
						this.SectorBitmapPos = this.AllocateSpace(1048576, true);
					}
					batEntry.PayloadBlockStatus = PayloadBlockStatus.PartiallyPresent;
				}
				else
				{
					batEntry.PayloadBlockStatus = PayloadBlockStatus.FullyPresent;
				}
				flag = true;
			}
			if (flag)
			{
				batEntry.WriteTo(this._batData, block * 8);
				this._bat.Position = (long)(this._chunk * (this._blocksPerChunk + 1) * 8);
				this._bat.Write(this._batData, 0, (this._blocksPerChunk + 1) * 8);
			}
			return batEntry.PayloadBlockStatus;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002532 File Offset: 0x00000732
		private byte[] LoadSectorBitmap()
		{
			if (this._sectorBitmap == null)
			{
				this._file.Position = this.SectorBitmapPos;
				this._sectorBitmap = StreamUtilities.ReadExact(this._file, 1048576);
			}
			return this._sectorBitmap;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000256C File Offset: 0x0000076C
		private long AllocateSpace(int sizeBytes, bool zero)
		{
			long num;
			if (!this._freeSpace.TryAllocate((long)sizeBytes, out num))
			{
				num = MathUtilities.RoundUp(this._file.Length, 1048576L);
				this._file.SetLength(num + (long)sizeBytes);
				this._freeSpace.ExtendTo(num + (long)sizeBytes, false);
			}
			else if (zero)
			{
				this._file.Position = num;
				this._file.Clear(sizeBytes);
			}
			return num;
		}

		// Token: 0x04000005 RID: 5
		private const ulong SectorBitmapPresent = 6UL;

		// Token: 0x04000006 RID: 6
		private readonly Stream _bat;

		// Token: 0x04000007 RID: 7
		private readonly byte[] _batData;

		// Token: 0x04000008 RID: 8
		private readonly int _blocksPerChunk;

		// Token: 0x04000009 RID: 9
		private readonly int _chunk;

		// Token: 0x0400000A RID: 10
		private readonly SparseStream _file;

		// Token: 0x0400000B RID: 11
		private readonly FileParameters _fileParameters;

		// Token: 0x0400000C RID: 12
		private readonly FreeSpaceTable _freeSpace;

		// Token: 0x0400000D RID: 13
		private byte[] _sectorBitmap;
	}
}
