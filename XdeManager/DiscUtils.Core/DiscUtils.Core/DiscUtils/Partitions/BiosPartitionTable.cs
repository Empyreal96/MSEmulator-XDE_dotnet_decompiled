using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils.Partitions
{
	// Token: 0x0200004D RID: 77
	public sealed class BiosPartitionTable : PartitionTable
	{
		// Token: 0x06000338 RID: 824 RVA: 0x00007172 File Offset: 0x00005372
		public BiosPartitionTable(VirtualDisk disk)
		{
			this.Init(disk.Content, disk.BiosGeometry);
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000718C File Offset: 0x0000538C
		public BiosPartitionTable(Stream disk, Geometry diskGeometry)
		{
			this.Init(disk, diskGeometry);
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600033A RID: 826 RVA: 0x0000719C File Offset: 0x0000539C
		public ReadOnlyCollection<BiosPartitionInfo> BiosUserPartitions
		{
			get
			{
				List<BiosPartitionInfo> list = new List<BiosPartitionInfo>();
				foreach (BiosPartitionRecord biosPartitionRecord in this.GetAllRecords())
				{
					if (biosPartitionRecord.IsValid)
					{
						list.Add(new BiosPartitionInfo(this, biosPartitionRecord));
					}
				}
				return new ReadOnlyCollection<BiosPartitionInfo>(list);
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600033B RID: 827 RVA: 0x000071E3 File Offset: 0x000053E3
		public override Guid DiskGuid
		{
			get
			{
				return Guid.Empty;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600033C RID: 828 RVA: 0x000071EC File Offset: 0x000053EC
		public override ReadOnlyCollection<PartitionInfo> Partitions
		{
			get
			{
				List<PartitionInfo> list = new List<PartitionInfo>();
				foreach (BiosPartitionRecord biosPartitionRecord in this.GetAllRecords())
				{
					if (biosPartitionRecord.IsValid)
					{
						list.Add(new BiosPartitionInfo(this, biosPartitionRecord));
					}
				}
				return new ReadOnlyCollection<PartitionInfo>(list);
			}
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00007234 File Offset: 0x00005434
		public static Geometry DetectGeometry(Stream disk)
		{
			if (disk.Length >= 512L)
			{
				disk.Position = 0L;
				byte[] array = StreamUtilities.ReadExact(disk, 512);
				if (array[510] == 85 && array[511] == 170)
				{
					byte b = 0;
					byte b2 = 0;
					foreach (BiosPartitionRecord biosPartitionRecord in BiosPartitionTable.ReadPrimaryRecords(array))
					{
						b = Math.Max(b, biosPartitionRecord.EndHead);
						b2 = Math.Max(b2, biosPartitionRecord.EndSector);
					}
					if (b > 0 && b2 > 0)
					{
						int num = (int)((b + 1) * b2) * 512;
						return new Geometry((int)MathUtilities.Ceil(disk.Length, (long)num), (int)(b + 1), (int)b2);
					}
				}
			}
			return Geometry.FromCapacity(disk.Length);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x000072F8 File Offset: 0x000054F8
		public static bool IsValid(Stream disk)
		{
			if (disk.Length < 512L)
			{
				return false;
			}
			disk.Position = 0L;
			byte[] array = StreamUtilities.ReadExact(disk, 512);
			if (array[510] != 85 || array[511] != 170)
			{
				return false;
			}
			List<StreamExtent> list = new List<StreamExtent>();
			foreach (BiosPartitionRecord biosPartitionRecord in BiosPartitionTable.ReadPrimaryRecords(array))
			{
				if (biosPartitionRecord.LBALength != 4294967295U && ((ulong)biosPartitionRecord.LBAStart + (ulong)biosPartitionRecord.LBALength) * 512UL > (ulong)disk.Length)
				{
					return false;
				}
				if (biosPartitionRecord.LBALength > 0U)
				{
					StreamExtent[] array3 = new StreamExtent[]
					{
						new StreamExtent((long)((ulong)biosPartitionRecord.LBAStart), (long)((ulong)biosPartitionRecord.LBALength))
					};
					using (IEnumerator<StreamExtent> enumerator = StreamExtent.Intersect(new IEnumerable<StreamExtent>[]
					{
						list,
						array3
					}).GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							StreamExtent streamExtent = enumerator.Current;
							return false;
						}
					}
					list = new List<StreamExtent>(StreamExtent.Union(new IEnumerable<StreamExtent>[]
					{
						list,
						array3
					}));
				}
			}
			return true;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x00007430 File Offset: 0x00005630
		public static BiosPartitionTable Initialize(VirtualDisk disk)
		{
			return BiosPartitionTable.Initialize(disk.Content, disk.BiosGeometry);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00007443 File Offset: 0x00005643
		public static BiosPartitionTable Initialize(VirtualDisk disk, WellKnownPartitionType type)
		{
			BiosPartitionTable biosPartitionTable = BiosPartitionTable.Initialize(disk.Content, disk.BiosGeometry);
			biosPartitionTable.Create(type, true);
			return biosPartitionTable;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00007460 File Offset: 0x00005660
		public static BiosPartitionTable Initialize(Stream disk, Geometry diskGeometry)
		{
			byte[] array;
			if (disk.Length >= 512L)
			{
				disk.Position = 0L;
				array = StreamUtilities.ReadExact(disk, 512);
			}
			else
			{
				array = new byte[512];
			}
			Array.Clear(array, 446, 64);
			array[510] = 85;
			array[511] = 170;
			disk.Position = 0L;
			disk.Write(array, 0, array.Length);
			return new BiosPartitionTable(disk, diskGeometry);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x000074DC File Offset: 0x000056DC
		public override int Create(WellKnownPartitionType type, bool active)
		{
			Geometry geometry = new Geometry(this._diskData.Length, this._diskGeometry.HeadsPerCylinder, this._diskGeometry.SectorsPerTrack, this._diskGeometry.BytesPerSector);
			ChsAddress chsAddress = new ChsAddress(0, 1, 1);
			ChsAddress lastSector = geometry.LastSector;
			long num = geometry.ToLogicalBlockAddress(chsAddress);
			long num2 = geometry.ToLogicalBlockAddress(lastSector);
			return this.CreatePrimaryByCylinder(0, geometry.Cylinders - 1, BiosPartitionTable.ConvertType(type, (num2 - num) * 512L), active);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00007560 File Offset: 0x00005760
		public override int Create(long size, WellKnownPartitionType type, bool active)
		{
			int num = this._diskGeometry.SectorsPerTrack * this._diskGeometry.HeadsPerCylinder * this._diskGeometry.BytesPerSector;
			int num2 = (int)(size / (long)num);
			int num3 = this.FindCylinderGap(num2);
			return this.CreatePrimaryByCylinder(num3, num3 + num2 - 1, BiosPartitionTable.ConvertType(type, size), active);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x000075B4 File Offset: 0x000057B4
		public override int CreateAligned(WellKnownPartitionType type, bool active, int alignment)
		{
			Geometry geometry = new Geometry(this._diskData.Length, this._diskGeometry.HeadsPerCylinder, this._diskGeometry.SectorsPerTrack, this._diskGeometry.BytesPerSector);
			ChsAddress chsAddress = new ChsAddress(0, 1, 1);
			long num = MathUtilities.RoundUp(geometry.ToLogicalBlockAddress(chsAddress), (long)(alignment / this._diskGeometry.BytesPerSector));
			long num2 = MathUtilities.RoundDown(this._diskData.Length / (long)this._diskGeometry.BytesPerSector, (long)(alignment / this._diskGeometry.BytesPerSector));
			return this.CreatePrimaryBySector(num, num2 - 1L, BiosPartitionTable.ConvertType(type, (num2 - num) * (long)this._diskGeometry.BytesPerSector), active);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00007664 File Offset: 0x00005864
		public override int CreateAligned(long size, WellKnownPartitionType type, bool active, int alignment)
		{
			if (size < (long)this._diskGeometry.BytesPerSector)
			{
				throw new ArgumentOutOfRangeException("size", size, "size must be at least one sector");
			}
			if (alignment % this._diskGeometry.BytesPerSector != 0)
			{
				throw new ArgumentException("Alignment is not a multiple of the sector size");
			}
			if (size % (long)alignment != 0L)
			{
				throw new ArgumentException("Size is not a multiple of the alignment");
			}
			long num = size / (long)this._diskGeometry.BytesPerSector;
			long num2 = this.FindGap(size / (long)this._diskGeometry.BytesPerSector, (long)(alignment / this._diskGeometry.BytesPerSector));
			return this.CreatePrimaryBySector(num2, num2 + num - 1L, BiosPartitionTable.ConvertType(type, num * 512L), active);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00007712 File Offset: 0x00005912
		public override void Delete(int index)
		{
			this.WriteRecord(index, new BiosPartitionRecord());
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00007720 File Offset: 0x00005920
		public int CreatePrimaryByCylinder(int first, int last, byte type, bool markActive)
		{
			if (first < 0)
			{
				throw new ArgumentOutOfRangeException("first", first, "First cylinder must be Zero or greater");
			}
			if (last <= first)
			{
				throw new ArgumentException("Last cylinder must be greater than first");
			}
			long first2 = (first == 0) ? this._diskGeometry.ToLogicalBlockAddress(0, 1, 1) : this._diskGeometry.ToLogicalBlockAddress(first, 0, 1);
			long last2 = this._diskGeometry.ToLogicalBlockAddress(last, this._diskGeometry.HeadsPerCylinder - 1, this._diskGeometry.SectorsPerTrack);
			return this.CreatePrimaryBySector(first2, last2, type, markActive);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x000077A8 File Offset: 0x000059A8
		public int CreatePrimaryBySector(long first, long last, byte type, bool markActive)
		{
			if (first >= last)
			{
				throw new ArgumentException("The first sector in a partition must be before the last");
			}
			if ((last + 1L) * (long)this._diskGeometry.BytesPerSector > this._diskData.Length)
			{
				throw new ArgumentOutOfRangeException("last", last, "The last sector extends beyond the end of the disk");
			}
			BiosPartitionRecord[] primaryRecords = this.GetPrimaryRecords();
			BiosPartitionRecord biosPartitionRecord = new BiosPartitionRecord();
			ChsAddress chsAddress = this._diskGeometry.ToChsAddress(first);
			ChsAddress chsAddress2 = this._diskGeometry.ToChsAddress(last);
			if (chsAddress.Cylinder > 1023)
			{
				chsAddress = new ChsAddress(1023, 254, 63);
			}
			if (chsAddress2.Cylinder > 1023)
			{
				chsAddress2 = new ChsAddress(1023, 254, 63);
			}
			biosPartitionRecord.StartCylinder = (ushort)chsAddress.Cylinder;
			biosPartitionRecord.StartHead = (byte)chsAddress.Head;
			biosPartitionRecord.StartSector = (byte)chsAddress.Sector;
			biosPartitionRecord.EndCylinder = (ushort)chsAddress2.Cylinder;
			biosPartitionRecord.EndHead = (byte)chsAddress2.Head;
			biosPartitionRecord.EndSector = (byte)chsAddress2.Sector;
			biosPartitionRecord.LBAStart = (uint)first;
			biosPartitionRecord.LBALength = (uint)(last - first + 1L);
			biosPartitionRecord.PartitionType = type;
			biosPartitionRecord.Status = (markActive ? 128 : 0);
			foreach (BiosPartitionRecord biosPartitionRecord2 in primaryRecords)
			{
				if (Utilities.RangesOverlap<uint>((uint)first, (uint)last + 1U, biosPartitionRecord2.LBAStartAbsolute, biosPartitionRecord2.LBAStartAbsolute + biosPartitionRecord2.LBALength))
				{
					throw new IOException("New partition overlaps with existing partition");
				}
			}
			for (int j = 0; j < 4; j++)
			{
				if (!primaryRecords[j].IsValid)
				{
					this.WriteRecord(j, biosPartitionRecord);
					return j;
				}
			}
			throw new IOException("No primary partition slots available");
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00007958 File Offset: 0x00005B58
		public void SetActivePartition(int index)
		{
			List<BiosPartitionRecord> list = new List<BiosPartitionRecord>(this.GetPrimaryRecords());
			for (int i = 0; i < list.Count; i++)
			{
				list[i].Status = ((i == index) ? 128 : 0);
				this.WriteRecord(i, list[i]);
			}
		}

		// Token: 0x0600034A RID: 842 RVA: 0x000079A8 File Offset: 0x00005BA8
		public IEnumerable<StreamExtent> GetMetadataDiskExtents()
		{
			List<StreamExtent> list = new List<StreamExtent>();
			list.Add(new StreamExtent(0L, 512L));
			foreach (BiosPartitionRecord biosPartitionRecord in this.GetPrimaryRecords())
			{
				if (biosPartitionRecord.IsValid && BiosPartitionTable.IsExtendedPartition(biosPartitionRecord))
				{
					list.AddRange(new BiosExtendedPartitionTable(this._diskData, biosPartitionRecord.LBAStart).GetMetadataDiskExtents());
				}
			}
			return list;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00007A14 File Offset: 0x00005C14
		public void UpdateBiosGeometry(Geometry geometry)
		{
			this._diskData.Position = 0L;
			BiosPartitionRecord[] array = BiosPartitionTable.ReadPrimaryRecords(StreamUtilities.ReadExact(this._diskData, 512));
			for (int i = 0; i < array.Length; i++)
			{
				BiosPartitionRecord biosPartitionRecord = array[i];
				if (biosPartitionRecord.IsValid)
				{
					ChsAddress chsAddress = geometry.ToChsAddress((long)((ulong)biosPartitionRecord.LBAStartAbsolute));
					if (chsAddress.Cylinder > 1023)
					{
						chsAddress = new ChsAddress(1023, geometry.HeadsPerCylinder - 1, geometry.SectorsPerTrack);
					}
					ChsAddress chsAddress2 = geometry.ToChsAddress((long)((ulong)(biosPartitionRecord.LBAStartAbsolute + biosPartitionRecord.LBALength - 1U)));
					if (chsAddress2.Cylinder > 1023)
					{
						chsAddress2 = new ChsAddress(1023, geometry.HeadsPerCylinder - 1, geometry.SectorsPerTrack);
					}
					biosPartitionRecord.StartCylinder = (ushort)chsAddress.Cylinder;
					biosPartitionRecord.StartHead = (byte)chsAddress.Head;
					biosPartitionRecord.StartSector = (byte)chsAddress.Sector;
					biosPartitionRecord.EndCylinder = (ushort)chsAddress2.Cylinder;
					biosPartitionRecord.EndHead = (byte)chsAddress2.Head;
					biosPartitionRecord.EndSector = (byte)chsAddress2.Sector;
					this.WriteRecord(i, biosPartitionRecord);
				}
			}
			this._diskGeometry = geometry;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00007B3B File Offset: 0x00005D3B
		internal SparseStream Open(BiosPartitionRecord record)
		{
			return new SubStream(this._diskData, Ownership.None, (long)((ulong)record.LBAStartAbsolute * (ulong)((long)this._diskGeometry.BytesPerSector)), (long)((ulong)record.LBALength * (ulong)((long)this._diskGeometry.BytesPerSector)));
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00007B74 File Offset: 0x00005D74
		private static BiosPartitionRecord[] ReadPrimaryRecords(byte[] bootSector)
		{
			BiosPartitionRecord[] array = new BiosPartitionRecord[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = new BiosPartitionRecord(bootSector, 446 + i * 16, 0U, i);
			}
			return array;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00007BAA File Offset: 0x00005DAA
		private static bool IsExtendedPartition(BiosPartitionRecord r)
		{
			return r.PartitionType == 5 || r.PartitionType == 15;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00007BC4 File Offset: 0x00005DC4
		private static byte ConvertType(WellKnownPartitionType type, long size)
		{
			switch (type)
			{
			case WellKnownPartitionType.WindowsFat:
				if (size < 536870912L)
				{
					return 6;
				}
				if (size < 8381463552L)
				{
					return 11;
				}
				return 12;
			case WellKnownPartitionType.WindowsNtfs:
				return 7;
			case WellKnownPartitionType.Linux:
				return 131;
			case WellKnownPartitionType.LinuxSwap:
				return 130;
			case WellKnownPartitionType.LinuxLvm:
				return 142;
			default:
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unrecognized partition type: '{0}'", new object[]
				{
					type
				}), "type");
			}
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00007C48 File Offset: 0x00005E48
		private BiosPartitionRecord[] GetAllRecords()
		{
			List<BiosPartitionRecord> list = new List<BiosPartitionRecord>();
			foreach (BiosPartitionRecord biosPartitionRecord in this.GetPrimaryRecords())
			{
				if (biosPartitionRecord.IsValid)
				{
					if (BiosPartitionTable.IsExtendedPartition(biosPartitionRecord))
					{
						list.AddRange(this.GetExtendedRecords(biosPartitionRecord));
					}
					else
					{
						list.Add(biosPartitionRecord);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00007CA0 File Offset: 0x00005EA0
		private BiosPartitionRecord[] GetPrimaryRecords()
		{
			this._diskData.Position = 0L;
			return BiosPartitionTable.ReadPrimaryRecords(StreamUtilities.ReadExact(this._diskData, 512));
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00007CC4 File Offset: 0x00005EC4
		private BiosPartitionRecord[] GetExtendedRecords(BiosPartitionRecord r)
		{
			return new BiosExtendedPartitionTable(this._diskData, r.LBAStart).GetPartitions();
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00007CDC File Offset: 0x00005EDC
		private void WriteRecord(int i, BiosPartitionRecord newRecord)
		{
			this._diskData.Position = 0L;
			byte[] array = StreamUtilities.ReadExact(this._diskData, 512);
			newRecord.WriteTo(array, 446 + i * 16);
			this._diskData.Position = 0L;
			this._diskData.Write(array, 0, array.Length);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00007D38 File Offset: 0x00005F38
		private int FindCylinderGap(int numCylinders)
		{
			List<BiosPartitionRecord> list = Utilities.Filter<List<BiosPartitionRecord>, BiosPartitionRecord>(this.GetPrimaryRecords(), (BiosPartitionRecord r) => r.IsValid);
			list.Sort();
			int num = 0;
			foreach (BiosPartitionRecord biosPartitionRecord in list)
			{
				int yFirst = (int)biosPartitionRecord.StartCylinder;
				int num2 = (int)biosPartitionRecord.EndCylinder;
				if ((ulong)biosPartitionRecord.LBAStart > (ulong)this._diskGeometry.ToLogicalBlockAddress((int)biosPartitionRecord.StartCylinder, (int)biosPartitionRecord.StartHead, (int)biosPartitionRecord.StartSector))
				{
					yFirst = this._diskGeometry.ToChsAddress((long)biosPartitionRecord.LBAStart).Cylinder;
				}
				if ((ulong)(biosPartitionRecord.LBAStart + biosPartitionRecord.LBALength) > (ulong)this._diskGeometry.ToLogicalBlockAddress((int)biosPartitionRecord.EndCylinder, (int)biosPartitionRecord.EndHead, (int)biosPartitionRecord.EndSector))
				{
					num2 = this._diskGeometry.ToChsAddress((long)(biosPartitionRecord.LBAStart + biosPartitionRecord.LBALength)).Cylinder;
				}
				if (!Utilities.RangesOverlap<int>(num, num + numCylinders - 1, yFirst, num2))
				{
					break;
				}
				num = num2 + 1;
			}
			return num;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00007E68 File Offset: 0x00006068
		private long FindGap(long numSectors, long alignmentSectors)
		{
			List<BiosPartitionRecord> list = Utilities.Filter<List<BiosPartitionRecord>, BiosPartitionRecord>(this.GetPrimaryRecords(), (BiosPartitionRecord r) => r.IsValid);
			list.Sort();
			long num = MathUtilities.RoundUp(this._diskGeometry.ToLogicalBlockAddress(0, 1, 1), alignmentSectors);
			for (int i = 0; i < list.Count; i++)
			{
				BiosPartitionRecord biosPartitionRecord = list[i];
				while (i < list.Count && num >= (long)((ulong)(biosPartitionRecord.LBAStartAbsolute + biosPartitionRecord.LBALength)))
				{
					i++;
					biosPartitionRecord = list[i];
				}
				if (Utilities.RangesOverlap<long>(num, num + numSectors, (long)((ulong)biosPartitionRecord.LBAStartAbsolute), (long)((ulong)(biosPartitionRecord.LBAStartAbsolute + biosPartitionRecord.LBALength))))
				{
					num = MathUtilities.RoundUp((long)((ulong)(biosPartitionRecord.LBAStartAbsolute + biosPartitionRecord.LBALength)), alignmentSectors);
				}
			}
			if (this._diskGeometry.TotalSectorsLong - num < numSectors)
			{
				throw new IOException(string.Format(CultureInfo.InvariantCulture, "Unable to find free space of {0} sectors", new object[]
				{
					numSectors
				}));
			}
			return num;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00007F68 File Offset: 0x00006168
		private void Init(Stream disk, Geometry diskGeometry)
		{
			this._diskData = disk;
			this._diskGeometry = diskGeometry;
			this._diskData.Position = 0L;
			byte[] array = StreamUtilities.ReadExact(this._diskData, 512);
			if (array[510] != 85 || array[511] != 170)
			{
				throw new IOException("Invalid boot sector - no magic number 0xAA55");
			}
		}

		// Token: 0x040000BB RID: 187
		private Stream _diskData;

		// Token: 0x040000BC RID: 188
		private Geometry _diskGeometry;
	}
}
