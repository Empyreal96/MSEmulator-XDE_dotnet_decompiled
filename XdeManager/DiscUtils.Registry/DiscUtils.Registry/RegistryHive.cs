using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils.Registry
{
	// Token: 0x02000009 RID: 9
	public sealed class RegistryHive : IDisposable
	{
		// Token: 0x06000029 RID: 41 RVA: 0x00002DC3 File Offset: 0x00000FC3
		public RegistryHive(Stream hive) : this(hive, Ownership.None)
		{
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002DD0 File Offset: 0x00000FD0
		public RegistryHive(Stream hive, Ownership ownership)
		{
			this._fileStream = hive;
			this._fileStream.Position = 0L;
			this._ownsStream = ownership;
			byte[] buffer = StreamUtilities.ReadExact(this._fileStream, 512);
			this._header = new HiveHeader();
			this._header.ReadFrom(buffer, 0);
			this._bins = new List<BinHeader>();
			BinHeader binHeader;
			for (int i = 0; i < this._header.Length; i += binHeader.BinSize)
			{
				this._fileStream.Position = 4096L + (long)i;
				byte[] buffer2 = StreamUtilities.ReadExact(this._fileStream, 32);
				binHeader = new BinHeader();
				binHeader.ReadFrom(buffer2, 0);
				this._bins.Add(binHeader);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002E8B File Offset: 0x0000108B
		public RegistryKey Root
		{
			get
			{
				return new RegistryKey(this, this.GetCell<KeyNodeCell>(this._header.RootCell));
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002EA4 File Offset: 0x000010A4
		public void Dispose()
		{
			if (this._fileStream != null && this._ownsStream == Ownership.Dispose)
			{
				this._fileStream.Dispose();
				this._fileStream = null;
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002EC9 File Offset: 0x000010C9
		public static RegistryHive Create(Stream stream)
		{
			return RegistryHive.Create(stream, Ownership.None);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002ED4 File Offset: 0x000010D4
		public static RegistryHive Create(Stream stream, Ownership ownership)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream", "Attempt to create registry hive in null stream");
			}
			BinHeader binHeader = new BinHeader();
			binHeader.FileOffset = 0;
			binHeader.BinSize = 4096;
			HiveHeader hiveHeader = new HiveHeader();
			hiveHeader.Length = binHeader.BinSize;
			stream.Position = 0L;
			byte[] array = new byte[hiveHeader.Size];
			hiveHeader.WriteTo(array, 0);
			stream.Write(array, 0, array.Length);
			array = new byte[binHeader.Size];
			binHeader.WriteTo(array, 0);
			stream.Position = 4096L;
			stream.Write(array, 0, array.Length);
			array = new byte[4];
			EndianUtilities.WriteBytesLittleEndian(binHeader.BinSize - binHeader.Size, array, 0);
			stream.Write(array, 0, array.Length);
			stream.Position = 4096L + (long)binHeader.BinSize - 1L;
			stream.WriteByte(0);
			RegistryHive registryHive = new RegistryHive(stream);
			KeyNodeCell keyNodeCell = new KeyNodeCell("root", -1);
			keyNodeCell.Flags = (RegistryKeyFlags.Root | RegistryKeyFlags.Normal);
			registryHive.UpdateCell(keyNodeCell, true);
			RegistrySecurity registrySecurity = new RegistrySecurity();
			registrySecurity.SetSecurityDescriptorSddlForm("O:BAG:BAD:PAI(A;;KA;;;SY)(A;CI;KA;;;BA)", AccessControlSections.All);
			SecurityCell securityCell = new SecurityCell(registrySecurity);
			registryHive.UpdateCell(securityCell, true);
			securityCell.NextIndex = securityCell.Index;
			securityCell.PreviousIndex = securityCell.Index;
			registryHive.UpdateCell(securityCell, false);
			keyNodeCell.SecurityIndex = securityCell.Index;
			registryHive.UpdateCell(keyNodeCell, false);
			hiveHeader.RootCell = keyNodeCell.Index;
			array = new byte[hiveHeader.Size];
			hiveHeader.WriteTo(array, 0);
			stream.Position = 0L;
			stream.Write(array, 0, array.Length);
			return new RegistryHive(stream, ownership);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003065 File Offset: 0x00001265
		public static RegistryHive Create(string path)
		{
			return RegistryHive.Create(new LocalFileLocator(string.Empty).Open(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None), Ownership.Dispose);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003080 File Offset: 0x00001280
		internal K GetCell<K>(int index) where K : Cell
		{
			Bin bin = this.GetBin(index);
			if (bin != null)
			{
				return (K)((object)bin.TryGetCell(index));
			}
			return default(K);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000030B0 File Offset: 0x000012B0
		internal void FreeCell(int index)
		{
			Bin bin = this.GetBin(index);
			if (bin != null)
			{
				bin.FreeCell(index);
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000030D0 File Offset: 0x000012D0
		internal int UpdateCell(Cell cell, bool canRelocate)
		{
			if (cell.Index == -1 && canRelocate)
			{
				cell.Index = this.AllocateRawCell(cell.Size);
			}
			Bin bin = this.GetBin(cell.Index);
			if (bin == null)
			{
				throw new RegistryCorruptException("No bin found containing index: " + cell.Index);
			}
			if (bin.UpdateCell(cell))
			{
				return cell.Index;
			}
			if (!canRelocate)
			{
				throw new ArgumentException("Can't update cell, needs relocation but relocation disabled", "canRelocate");
			}
			int index = cell.Index;
			cell.Index = this.AllocateRawCell(cell.Size);
			bin = this.GetBin(cell.Index);
			if (!bin.UpdateCell(cell))
			{
				cell.Index = index;
				throw new RegistryCorruptException("Failed to migrate cell to new location");
			}
			this.FreeCell(index);
			return cell.Index;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003198 File Offset: 0x00001398
		internal byte[] RawCellData(int index, int maxBytes)
		{
			Bin bin = this.GetBin(index);
			if (bin != null)
			{
				return bin.ReadRawCellData(index, maxBytes);
			}
			return null;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000031BC File Offset: 0x000013BC
		internal bool WriteRawCellData(int index, byte[] data, int offset, int count)
		{
			Bin bin = this.GetBin(index);
			if (bin != null)
			{
				return bin.WriteRawCellData(index, data, offset, count);
			}
			throw new RegistryCorruptException("No bin found containing index: " + index);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000031F8 File Offset: 0x000013F8
		internal int AllocateRawCell(int capacity)
		{
			int num = MathUtilities.RoundUp(capacity + 4, 8);
			foreach (BinHeader binHeader in this._bins)
			{
				int num2 = this.LoadBin(binHeader).AllocateCell(num);
				if (num2 >= 0)
				{
					return num2;
				}
			}
			BinHeader binHeader2 = this.AllocateBin(num);
			return this.LoadBin(binHeader2).AllocateCell(num);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003280 File Offset: 0x00001480
		private BinHeader FindBin(int index)
		{
			int num = this._bins.BinarySearch(null, new RegistryHive.BinFinder(index));
			if (num >= 0)
			{
				return this._bins[num];
			}
			return null;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000032B4 File Offset: 0x000014B4
		private Bin GetBin(int cellIndex)
		{
			BinHeader binHeader = this.FindBin(cellIndex);
			if (binHeader != null)
			{
				return this.LoadBin(binHeader);
			}
			return null;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000032D5 File Offset: 0x000014D5
		private Bin LoadBin(BinHeader binHeader)
		{
			this._fileStream.Position = 4096L + (long)binHeader.FileOffset;
			return new Bin(this, this._fileStream);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000032FC File Offset: 0x000014FC
		private BinHeader AllocateBin(int minSize)
		{
			BinHeader binHeader = this._bins[this._bins.Count - 1];
			BinHeader binHeader2 = new BinHeader();
			binHeader2.FileOffset = binHeader.FileOffset + binHeader.BinSize;
			binHeader2.BinSize = MathUtilities.RoundUp(minSize + binHeader2.Size, 4096);
			byte[] array = new byte[binHeader2.Size];
			binHeader2.WriteTo(array, 0);
			this._fileStream.Position = 4096L + (long)binHeader2.FileOffset;
			this._fileStream.Write(array, 0, array.Length);
			byte[] buffer = new byte[4];
			EndianUtilities.WriteBytesLittleEndian(binHeader2.BinSize - binHeader2.Size, buffer, 0);
			this._fileStream.Write(buffer, 0, 4);
			this._header.Length = binHeader2.FileOffset + binHeader2.BinSize;
			this._header.Timestamp = DateTime.UtcNow;
			this._header.Sequence1++;
			this._header.Sequence2++;
			this._fileStream.Position = 0L;
			byte[] array2 = StreamUtilities.ReadExact(this._fileStream, this._header.Size);
			this._header.WriteTo(array2, 0);
			this._fileStream.Position = 0L;
			this._fileStream.Write(array2, 0, array2.Length);
			this._fileStream.Position = 4096L + (long)this._header.Length - 1L;
			this._fileStream.WriteByte(0);
			this._bins.Add(binHeader2);
			return binHeader2;
		}

		// Token: 0x04000028 RID: 40
		private const long BinStart = 4096L;

		// Token: 0x04000029 RID: 41
		private readonly List<BinHeader> _bins;

		// Token: 0x0400002A RID: 42
		private Stream _fileStream;

		// Token: 0x0400002B RID: 43
		private readonly HiveHeader _header;

		// Token: 0x0400002C RID: 44
		private readonly Ownership _ownsStream;

		// Token: 0x02000014 RID: 20
		private class BinFinder : IComparer<BinHeader>
		{
			// Token: 0x060000A6 RID: 166 RVA: 0x000053BC File Offset: 0x000035BC
			public BinFinder(int index)
			{
				this._index = index;
			}

			// Token: 0x060000A7 RID: 167 RVA: 0x000053CB File Offset: 0x000035CB
			public int Compare(BinHeader x, BinHeader y)
			{
				if (x.FileOffset + x.BinSize < this._index)
				{
					return -1;
				}
				if (x.FileOffset > this._index)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x04000071 RID: 113
			private readonly int _index;
		}
	}
}
