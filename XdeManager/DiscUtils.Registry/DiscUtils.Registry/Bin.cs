using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Registry
{
	// Token: 0x02000002 RID: 2
	internal sealed class Bin
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public Bin(RegistryHive hive, Stream stream)
		{
			this._hive = hive;
			this._fileStream = stream;
			this._streamPos = stream.Position;
			stream.Position = this._streamPos;
			byte[] buffer = StreamUtilities.ReadExact(stream, 32);
			this._header = new BinHeader();
			this._header.ReadFrom(buffer, 0);
			this._fileStream.Position = this._streamPos;
			this._buffer = StreamUtilities.ReadExact(this._fileStream, this._header.BinSize);
			this._freeCells = new List<Range<int, int>>();
			int num;
			for (int i = 32; i < this._buffer.Length; i += Math.Abs(num))
			{
				num = EndianUtilities.ToInt32LittleEndian(this._buffer, i);
				if (num > 0)
				{
					this._freeCells.Add(new Range<int, int>(i, num));
				}
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000211F File Offset: 0x0000031F
		public Cell TryGetCell(int index)
		{
			if (EndianUtilities.ToInt32LittleEndian(this._buffer, index - this._header.FileOffset) >= 0)
			{
				return null;
			}
			return Cell.Parse(this._hive, index, this._buffer, index + 4 - this._header.FileOffset);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002160 File Offset: 0x00000360
		public void FreeCell(int index)
		{
			int num = index - this._header.FileOffset;
			int num2 = EndianUtilities.ToInt32LittleEndian(this._buffer, num);
			if (num2 >= 0)
			{
				throw new ArgumentException("Attempt to free non-allocated cell");
			}
			num2 = Math.Abs(num2);
			int num3 = 0;
			while (num3 < this._freeCells.Count && this._freeCells[num3].Offset < num)
			{
				if (this._freeCells[num3].Offset + this._freeCells[num3].Count == num)
				{
					num = this._freeCells[num3].Offset;
					num2 += this._freeCells[num3].Count;
					this._freeCells.RemoveAt(num3);
				}
				else
				{
					num3++;
				}
			}
			if (num3 < this._freeCells.Count && this._freeCells[num3].Offset == num + num2)
			{
				num2 += this._freeCells[num3].Count;
				this._freeCells.RemoveAt(num3);
			}
			this._freeCells.Insert(num3, new Range<int, int>(num, num2));
			EndianUtilities.WriteBytesLittleEndian(num2, this._buffer, num);
			this._fileStream.Position = this._streamPos + (long)num;
			this._fileStream.Write(this._buffer, num, 4);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000022B0 File Offset: 0x000004B0
		public bool UpdateCell(Cell cell)
		{
			int num = cell.Index - this._header.FileOffset;
			int num2 = Math.Abs(EndianUtilities.ToInt32LittleEndian(this._buffer, num));
			int num3 = cell.Size + 4;
			if (num3 > num2)
			{
				return false;
			}
			cell.WriteTo(this._buffer, num + 4);
			this._fileStream.Position = this._streamPos + (long)num;
			this._fileStream.Write(this._buffer, num, num3);
			return true;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002328 File Offset: 0x00000528
		public byte[] ReadRawCellData(int cellIndex, int maxBytes)
		{
			int num = cellIndex - this._header.FileOffset;
			byte[] array = new byte[Math.Min(Math.Abs(EndianUtilities.ToInt32LittleEndian(this._buffer, num)) - 4, maxBytes)];
			Array.Copy(this._buffer, num + 4, array, 0, array.Length);
			return array;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002378 File Offset: 0x00000578
		internal bool WriteRawCellData(int cellIndex, byte[] data, int offset, int count)
		{
			int num = cellIndex - this._header.FileOffset;
			int num2 = Math.Abs(EndianUtilities.ToInt32LittleEndian(this._buffer, num));
			int num3 = count + 4;
			if (num3 > num2)
			{
				return false;
			}
			Array.Copy(data, offset, this._buffer, num + 4, count);
			this._fileStream.Position = this._streamPos + (long)num;
			this._fileStream.Write(this._buffer, num, num3);
			return true;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000023EC File Offset: 0x000005EC
		internal int AllocateCell(int size)
		{
			if (size < 8 || size % 8 != 0)
			{
				throw new ArgumentException("Invalid cell size");
			}
			for (int i = 0; i < this._freeCells.Count; i++)
			{
				int result = this._freeCells[i].Offset + this._header.FileOffset;
				if (this._freeCells[i].Count > size)
				{
					EndianUtilities.WriteBytesLittleEndian(-size, this._buffer, this._freeCells[i].Offset);
					this._fileStream.Position = this._streamPos + (long)this._freeCells[i].Offset;
					this._fileStream.Write(this._buffer, this._freeCells[i].Offset, 4);
					this._freeCells[i] = new Range<int, int>(this._freeCells[i].Offset + size, this._freeCells[i].Count - size);
					EndianUtilities.WriteBytesLittleEndian(this._freeCells[i].Count, this._buffer, this._freeCells[i].Offset);
					this._fileStream.Position = this._streamPos + (long)this._freeCells[i].Offset;
					this._fileStream.Write(this._buffer, this._freeCells[i].Offset, 4);
					return result;
				}
				if (this._freeCells[i].Count == size)
				{
					EndianUtilities.WriteBytesLittleEndian(-size, this._buffer, this._freeCells[i].Offset);
					this._fileStream.Position = this._streamPos + (long)this._freeCells[i].Offset;
					this._fileStream.Write(this._buffer, this._freeCells[i].Offset, 4);
					this._freeCells.RemoveAt(i);
					return result;
				}
			}
			return -1;
		}

		// Token: 0x04000001 RID: 1
		private readonly byte[] _buffer;

		// Token: 0x04000002 RID: 2
		private readonly Stream _fileStream;

		// Token: 0x04000003 RID: 3
		private readonly List<Range<int, int>> _freeCells;

		// Token: 0x04000004 RID: 4
		private readonly BinHeader _header;

		// Token: 0x04000005 RID: 5
		private readonly RegistryHive _hive;

		// Token: 0x04000006 RID: 6
		private readonly long _streamPos;
	}
}
