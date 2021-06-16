using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils.Partitions
{
	// Token: 0x02000053 RID: 83
	public sealed class GuidPartitionTable : PartitionTable
	{
		// Token: 0x06000370 RID: 880 RVA: 0x00008DC6 File Offset: 0x00006FC6
		public GuidPartitionTable(VirtualDisk disk)
		{
			this.Init(disk.Content, disk.Geometry);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00008DE0 File Offset: 0x00006FE0
		public GuidPartitionTable(Stream disk, Geometry diskGeometry)
		{
			this.Init(disk, diskGeometry);
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000372 RID: 882 RVA: 0x00008DF0 File Offset: 0x00006FF0
		public override Guid DiskGuid
		{
			get
			{
				return this._primaryHeader.DiskGuid;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000373 RID: 883 RVA: 0x00008DFD File Offset: 0x00006FFD
		public long FirstUsableSector
		{
			get
			{
				return this._primaryHeader.FirstUsable;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000374 RID: 884 RVA: 0x00008E0A File Offset: 0x0000700A
		public long LastUsableSector
		{
			get
			{
				return this._primaryHeader.LastUsable;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000375 RID: 885 RVA: 0x00008E17 File Offset: 0x00007017
		public override ReadOnlyCollection<PartitionInfo> Partitions
		{
			get
			{
				return new ReadOnlyCollection<PartitionInfo>(Utilities.Map<GptEntry, GuidPartitionInfo>(this.GetAllEntries(), (GptEntry e) => new GuidPartitionInfo(this, e)));
			}
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00008E35 File Offset: 0x00007035
		public static GuidPartitionTable Initialize(VirtualDisk disk)
		{
			return GuidPartitionTable.Initialize(disk.Content, disk.Geometry);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00008E48 File Offset: 0x00007048
		public static GuidPartitionTable Initialize(Stream disk, Geometry diskGeometry)
		{
			BiosPartitionTable.Initialize(disk, diskGeometry).CreatePrimaryByCylinder(0, diskGeometry.Cylinders - 1, 238, false);
			int num = (16384 + diskGeometry.BytesPerSector - 1) / diskGeometry.BytesPerSector;
			byte[] buffer = new byte[16384];
			GptHeader gptHeader = new GptHeader(diskGeometry.BytesPerSector);
			gptHeader.HeaderLba = 1L;
			gptHeader.AlternateHeaderLba = disk.Length / (long)diskGeometry.BytesPerSector - 1L;
			gptHeader.FirstUsable = gptHeader.HeaderLba + (long)num + 1L;
			gptHeader.LastUsable = gptHeader.AlternateHeaderLba - (long)num - 1L;
			gptHeader.DiskGuid = Guid.NewGuid();
			gptHeader.PartitionEntriesLba = 2L;
			gptHeader.PartitionEntryCount = 128U;
			gptHeader.PartitionEntrySize = 128;
			gptHeader.EntriesCrc = GuidPartitionTable.CalcEntriesCrc(buffer);
			byte[] array = new byte[diskGeometry.BytesPerSector];
			gptHeader.WriteTo(array, 0);
			disk.Position = gptHeader.HeaderLba * (long)diskGeometry.BytesPerSector;
			disk.Write(array, 0, array.Length);
			gptHeader.HeaderLba = gptHeader.AlternateHeaderLba;
			gptHeader.AlternateHeaderLba = 1L;
			gptHeader.PartitionEntriesLba = gptHeader.HeaderLba - (long)num;
			gptHeader.WriteTo(array, 0);
			disk.Position = gptHeader.HeaderLba * (long)diskGeometry.BytesPerSector;
			disk.Write(array, 0, array.Length);
			return new GuidPartitionTable(disk, diskGeometry);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00008F9A File Offset: 0x0000719A
		public static GuidPartitionTable Initialize(VirtualDisk disk, WellKnownPartitionType type)
		{
			GuidPartitionTable guidPartitionTable = GuidPartitionTable.Initialize(disk);
			guidPartitionTable.Create(type, true);
			return guidPartitionTable;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00008FAC File Offset: 0x000071AC
		public override int Create(WellKnownPartitionType type, bool active)
		{
			List<GptEntry> allEntries = new List<GptEntry>(this.GetAllEntries());
			this.EstablishReservedPartition(allEntries);
			long num = this.FirstAvailableSector(allEntries);
			long endSector = this.FindLastFreeSector(num, allEntries);
			return this.Create(num, endSector, GuidPartitionTypes.Convert(type), 0L, "Data Partition");
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00008FF4 File Offset: 0x000071F4
		public override int Create(long size, WellKnownPartitionType type, bool active)
		{
			if (size < (long)this._diskGeometry.BytesPerSector)
			{
				throw new ArgumentOutOfRangeException("size", size, "size must be at least one sector");
			}
			long num = size / (long)this._diskGeometry.BytesPerSector;
			long num2 = this.FindGap(size / (long)this._diskGeometry.BytesPerSector, 1L);
			return this.Create(num2, num2 + num - 1L, GuidPartitionTypes.Convert(type), 0L, "Data Partition");
		}

		// Token: 0x0600037B RID: 891 RVA: 0x00009068 File Offset: 0x00007268
		public override int CreateAligned(WellKnownPartitionType type, bool active, int alignment)
		{
			if (alignment % this._diskGeometry.BytesPerSector != 0)
			{
				throw new ArgumentException("Alignment is not a multiple of the sector size");
			}
			List<GptEntry> allEntries = new List<GptEntry>(this.GetAllEntries());
			this.EstablishReservedPartition(allEntries);
			long num = MathUtilities.RoundUp(this.FirstAvailableSector(allEntries), (long)(alignment / this._diskGeometry.BytesPerSector));
			long num2 = MathUtilities.RoundDown(this.FindLastFreeSector(num, allEntries) + 1L, (long)(alignment / this._diskGeometry.BytesPerSector));
			if (num2 <= num)
			{
				throw new IOException("No available space");
			}
			return this.Create(num, num2 - 1L, GuidPartitionTypes.Convert(type), 0L, "Data Partition");
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00009104 File Offset: 0x00007304
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
			return this.Create(num2, num2 + num - 1L, GuidPartitionTypes.Convert(type), 0L, "Data Partition");
		}

		// Token: 0x0600037D RID: 893 RVA: 0x000091B0 File Offset: 0x000073B0
		public int Create(long startSector, long endSector, Guid type, long attributes, string name)
		{
			GptEntry gptEntry = this.CreateEntry(startSector, endSector, type, attributes, name);
			return this.GetEntryIndex(gptEntry.Identity);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x000091D8 File Offset: 0x000073D8
		public override void Delete(int index)
		{
			int partitionOffset = this.GetPartitionOffset(index);
			Array.Clear(this._entryBuffer, partitionOffset, this._primaryHeader.PartitionEntrySize);
			this.Write();
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000920C File Offset: 0x0000740C
		internal SparseStream Open(GptEntry entry)
		{
			long num = entry.FirstUsedLogicalBlock * (long)this._diskGeometry.BytesPerSector;
			long num2 = (entry.LastUsedLogicalBlock + 1L) * (long)this._diskGeometry.BytesPerSector;
			return new SubStream(this._diskData, num, num2 - num);
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00009253 File Offset: 0x00007453
		private static uint CalcEntriesCrc(byte[] buffer)
		{
			return Crc32LittleEndian.Compute(Crc32Algorithm.Common, buffer, 0, buffer.Length);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00009260 File Offset: 0x00007460
		private static int CountEntries<T>(ICollection<T> values, Func<T, bool> pred)
		{
			int num = 0;
			foreach (T arg in values)
			{
				if (pred(arg))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x000092B4 File Offset: 0x000074B4
		private void Init(Stream disk, Geometry diskGeometry)
		{
			BiosPartitionTable biosPartitionTable;
			try
			{
				biosPartitionTable = new BiosPartitionTable(disk, diskGeometry);
			}
			catch (IOException innerException)
			{
				throw new IOException("Invalid GPT disk, protective MBR table not present or invalid", innerException);
			}
			if (biosPartitionTable.Count != 1 || biosPartitionTable[0].BiosType != 238)
			{
				throw new IOException("Invalid GPT disk, protective MBR table is not valid");
			}
			this._diskData = disk;
			this._diskGeometry = diskGeometry;
			disk.Position = (long)diskGeometry.BytesPerSector;
			byte[] array = StreamUtilities.ReadExact(disk, diskGeometry.BytesPerSector);
			this._primaryHeader = new GptHeader(diskGeometry.BytesPerSector);
			if (!this._primaryHeader.ReadFrom(array, 0) || !this.ReadEntries(this._primaryHeader))
			{
				disk.Position = disk.Length - (long)diskGeometry.BytesPerSector;
				disk.Read(array, 0, array.Length);
				this._secondaryHeader = new GptHeader(diskGeometry.BytesPerSector);
				if (!this._secondaryHeader.ReadFrom(array, 0) || !this.ReadEntries(this._secondaryHeader))
				{
					throw new IOException("No valid GUID Partition Table found");
				}
				this._primaryHeader = new GptHeader(this._secondaryHeader);
				this._primaryHeader.HeaderLba = this._secondaryHeader.AlternateHeaderLba;
				this._primaryHeader.AlternateHeaderLba = this._secondaryHeader.HeaderLba;
				this._primaryHeader.PartitionEntriesLba = 2L;
				if (disk.CanWrite)
				{
					this.WritePrimaryHeader();
				}
			}
			if (this._secondaryHeader == null)
			{
				this._secondaryHeader = new GptHeader(diskGeometry.BytesPerSector);
				disk.Position = disk.Length - (long)diskGeometry.BytesPerSector;
				disk.Read(array, 0, array.Length);
				if (!this._secondaryHeader.ReadFrom(array, 0) || !this.ReadEntries(this._secondaryHeader))
				{
					this._secondaryHeader = new GptHeader(this._primaryHeader);
					this._secondaryHeader.HeaderLba = this._secondaryHeader.AlternateHeaderLba;
					this._secondaryHeader.AlternateHeaderLba = this._secondaryHeader.HeaderLba;
					this._secondaryHeader.PartitionEntriesLba = this._secondaryHeader.HeaderLba - MathUtilities.RoundUp((long)((ulong)this._secondaryHeader.PartitionEntryCount * (ulong)((long)this._secondaryHeader.PartitionEntrySize)), (long)diskGeometry.BytesPerSector);
					if (disk.CanWrite)
					{
						this.WriteSecondaryHeader();
					}
				}
			}
		}

		// Token: 0x06000383 RID: 899 RVA: 0x000094FC File Offset: 0x000076FC
		private void EstablishReservedPartition(List<GptEntry> allEntries)
		{
			if (GuidPartitionTable.CountEntries<GptEntry>(allEntries, (GptEntry e) => e.PartitionType == GuidPartitionTypes.MicrosoftReserved) == 0)
			{
				if (GuidPartitionTable.CountEntries<GptEntry>(allEntries, (GptEntry e) => e.PartitionType == GuidPartitionTypes.WindowsBasicData) == 0 && this._diskGeometry.Capacity > 536870912L)
				{
					long num = this.FirstAvailableSector(allEntries);
					long num2 = this.FindLastFreeSector(num, allEntries);
					if ((num2 - num + 1L) * (long)this._diskGeometry.BytesPerSector > 536870912L)
					{
						long num3 = (long)(((this._diskGeometry.Capacity < 17179869184L) ? 32 : 128) * 1024 * 1024);
						num2 = num + num3 / (long)this._diskGeometry.BytesPerSector - 1L;
						int freeEntryOffset = this.GetFreeEntryOffset();
						GptEntry gptEntry = new GptEntry();
						gptEntry.PartitionType = GuidPartitionTypes.MicrosoftReserved;
						gptEntry.Identity = Guid.NewGuid();
						gptEntry.FirstUsedLogicalBlock = num;
						gptEntry.LastUsedLogicalBlock = num2;
						gptEntry.Attributes = 0UL;
						gptEntry.Name = "Microsoft reserved partition";
						gptEntry.WriteTo(this._entryBuffer, freeEntryOffset);
						allEntries.Add(gptEntry);
					}
				}
			}
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00009644 File Offset: 0x00007844
		private GptEntry CreateEntry(long startSector, long endSector, Guid type, long attributes, string name)
		{
			if (endSector < startSector)
			{
				throw new ArgumentException("The end sector is before the start sector");
			}
			int freeEntryOffset = this.GetFreeEntryOffset();
			GptEntry gptEntry = new GptEntry();
			gptEntry.PartitionType = type;
			gptEntry.Identity = Guid.NewGuid();
			gptEntry.FirstUsedLogicalBlock = startSector;
			gptEntry.LastUsedLogicalBlock = endSector;
			gptEntry.Attributes = (ulong)attributes;
			gptEntry.Name = name;
			gptEntry.WriteTo(this._entryBuffer, freeEntryOffset);
			this.Write();
			return gptEntry;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x000096B0 File Offset: 0x000078B0
		private long FindGap(long numSectors, long alignmentSectors)
		{
			List<GptEntry> list = new List<GptEntry>(this.GetAllEntries());
			list.Sort();
			long num = MathUtilities.RoundUp(this._primaryHeader.FirstUsable, alignmentSectors);
			foreach (GptEntry gptEntry in list)
			{
				if (!Utilities.RangesOverlap<long>(num, num + numSectors - 1L, gptEntry.FirstUsedLogicalBlock, gptEntry.LastUsedLogicalBlock))
				{
					break;
				}
				num = MathUtilities.RoundUp(gptEntry.LastUsedLogicalBlock + 1L, alignmentSectors);
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

		// Token: 0x06000386 RID: 902 RVA: 0x0000977C File Offset: 0x0000797C
		private long FirstAvailableSector(List<GptEntry> allEntries)
		{
			long num = this._primaryHeader.FirstUsable;
			foreach (GptEntry gptEntry in allEntries)
			{
				if (gptEntry.LastUsedLogicalBlock >= num)
				{
					num = gptEntry.LastUsedLogicalBlock + 1L;
				}
			}
			return num;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x000097E4 File Offset: 0x000079E4
		private long FindLastFreeSector(long start, List<GptEntry> allEntries)
		{
			long num = this._primaryHeader.LastUsable;
			foreach (GptEntry gptEntry in allEntries)
			{
				if (gptEntry.LastUsedLogicalBlock > start && gptEntry.FirstUsedLogicalBlock <= num)
				{
					num = gptEntry.FirstUsedLogicalBlock - 1L;
				}
			}
			return num;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00009854 File Offset: 0x00007A54
		private void Write()
		{
			this.WritePrimaryHeader();
			this.WriteSecondaryHeader();
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00009864 File Offset: 0x00007A64
		private void WritePrimaryHeader()
		{
			byte[] array = new byte[this._diskGeometry.BytesPerSector];
			this._primaryHeader.EntriesCrc = this.CalcEntriesCrc();
			this._primaryHeader.WriteTo(array, 0);
			this._diskData.Position = (long)this._diskGeometry.BytesPerSector;
			this._diskData.Write(array, 0, array.Length);
			this._diskData.Position = (long)(2 * this._diskGeometry.BytesPerSector);
			this._diskData.Write(this._entryBuffer, 0, this._entryBuffer.Length);
		}

		// Token: 0x0600038A RID: 906 RVA: 0x000098FC File Offset: 0x00007AFC
		private void WriteSecondaryHeader()
		{
			byte[] array = new byte[this._diskGeometry.BytesPerSector];
			this._secondaryHeader.EntriesCrc = this.CalcEntriesCrc();
			this._secondaryHeader.WriteTo(array, 0);
			this._diskData.Position = this._diskData.Length - (long)this._diskGeometry.BytesPerSector;
			this._diskData.Write(array, 0, array.Length);
			this._diskData.Position = this._secondaryHeader.PartitionEntriesLba * (long)this._diskGeometry.BytesPerSector;
			this._diskData.Write(this._entryBuffer, 0, this._entryBuffer.Length);
		}

		// Token: 0x0600038B RID: 907 RVA: 0x000099A8 File Offset: 0x00007BA8
		private bool ReadEntries(GptHeader header)
		{
			this._diskData.Position = header.PartitionEntriesLba * (long)this._diskGeometry.BytesPerSector;
			this._entryBuffer = StreamUtilities.ReadExact(this._diskData, (int)((long)header.PartitionEntrySize * (long)((ulong)header.PartitionEntryCount)));
			return header.EntriesCrc == this.CalcEntriesCrc();
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00009A05 File Offset: 0x00007C05
		private uint CalcEntriesCrc()
		{
			return Crc32LittleEndian.Compute(Crc32Algorithm.Common, this._entryBuffer, 0, this._entryBuffer.Length);
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00009A1C File Offset: 0x00007C1C
		private IEnumerable<GptEntry> GetAllEntries()
		{
			int i = 0;
			while ((long)i < (long)((ulong)this._primaryHeader.PartitionEntryCount))
			{
				GptEntry gptEntry = new GptEntry();
				gptEntry.ReadFrom(this._entryBuffer, i * this._primaryHeader.PartitionEntrySize);
				if (gptEntry.PartitionType != Guid.Empty)
				{
					yield return gptEntry;
				}
				int num = i + 1;
				i = num;
			}
			yield break;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00009A2C File Offset: 0x00007C2C
		private int GetPartitionOffset(int index)
		{
			bool flag = false;
			int num = 0;
			int num2 = 0;
			while (!flag && (long)num2 < (long)((ulong)this._primaryHeader.PartitionEntryCount))
			{
				GptEntry gptEntry = new GptEntry();
				gptEntry.ReadFrom(this._entryBuffer, num2 * this._primaryHeader.PartitionEntrySize);
				if (gptEntry.PartitionType != Guid.Empty)
				{
					if (index == num)
					{
						flag = true;
						break;
					}
					num++;
				}
				num2++;
			}
			if (flag)
			{
				return num2 * this._primaryHeader.PartitionEntrySize;
			}
			throw new IOException(string.Format(CultureInfo.InvariantCulture, "No such partition: {0}", new object[]
			{
				index
			}));
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00009AC8 File Offset: 0x00007CC8
		private int GetEntryIndex(Guid identity)
		{
			int num = 0;
			int num2 = 0;
			while ((long)num2 < (long)((ulong)this._primaryHeader.PartitionEntryCount))
			{
				GptEntry gptEntry = new GptEntry();
				gptEntry.ReadFrom(this._entryBuffer, num2 * this._primaryHeader.PartitionEntrySize);
				if (gptEntry.Identity == identity)
				{
					return num;
				}
				if (gptEntry.PartitionType != Guid.Empty)
				{
					num++;
				}
				num2++;
			}
			throw new IOException("No such partition");
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00009B40 File Offset: 0x00007D40
		private int GetFreeEntryOffset()
		{
			int num = 0;
			while ((long)num < (long)((ulong)this._primaryHeader.PartitionEntryCount))
			{
				GptEntry gptEntry = new GptEntry();
				gptEntry.ReadFrom(this._entryBuffer, num * this._primaryHeader.PartitionEntrySize);
				if (gptEntry.PartitionType == Guid.Empty)
				{
					return num * this._primaryHeader.PartitionEntrySize;
				}
				num++;
			}
			throw new IOException("No free partition entries available");
		}

		// Token: 0x040000E3 RID: 227
		private Stream _diskData;

		// Token: 0x040000E4 RID: 228
		private Geometry _diskGeometry;

		// Token: 0x040000E5 RID: 229
		private byte[] _entryBuffer;

		// Token: 0x040000E6 RID: 230
		private GptHeader _primaryHeader;

		// Token: 0x040000E7 RID: 231
		private GptHeader _secondaryHeader;
	}
}
