using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Partitions
{
	// Token: 0x02000049 RID: 73
	internal class BiosExtendedPartitionTable
	{
		// Token: 0x06000305 RID: 773 RVA: 0x00006938 File Offset: 0x00004B38
		public BiosExtendedPartitionTable(Stream disk, uint firstSector)
		{
			this._disk = disk;
			this._firstSector = firstSector;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00006950 File Offset: 0x00004B50
		public BiosPartitionRecord[] GetPartitions()
		{
			List<BiosPartitionRecord> list = new List<BiosPartitionRecord>();
			uint num2;
			for (uint num = this._firstSector; num != 0U; num = num2)
			{
				this._disk.Position = (long)((ulong)num * 512UL);
				byte[] array = StreamUtilities.ReadExact(this._disk, 512);
				if (array[510] != 85 || array[511] != 170)
				{
					throw new IOException("Invalid extended partition sector");
				}
				num2 = 0U;
				for (int i = 446; i <= 494; i += 16)
				{
					BiosPartitionRecord biosPartitionRecord = new BiosPartitionRecord(array, i, num, -1);
					if (biosPartitionRecord.StartCylinder != 0 || biosPartitionRecord.StartHead != 0 || biosPartitionRecord.StartSector != 0 || (biosPartitionRecord.LBAStart != 0U && biosPartitionRecord.LBALength != 0U))
					{
						if (biosPartitionRecord.PartitionType != 5 && biosPartitionRecord.PartitionType != 15)
						{
							list.Add(biosPartitionRecord);
						}
						else
						{
							num2 = this._firstSector + biosPartitionRecord.LBAStart;
						}
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00006A48 File Offset: 0x00004C48
		public IEnumerable<StreamExtent> GetMetadataDiskExtents()
		{
			List<StreamExtent> list = new List<StreamExtent>();
			uint num2;
			for (uint num = this._firstSector; num != 0U; num = num2)
			{
				list.Add(new StreamExtent((long)((ulong)num * 512UL), 512L));
				this._disk.Position = (long)((ulong)num * 512UL);
				byte[] array = StreamUtilities.ReadExact(this._disk, 512);
				if (array[510] != 85 || array[511] != 170)
				{
					throw new IOException("Invalid extended partition sector");
				}
				num2 = 0U;
				for (int i = 446; i <= 494; i += 16)
				{
					BiosPartitionRecord biosPartitionRecord = new BiosPartitionRecord(array, i, num, -1);
					if ((biosPartitionRecord.StartCylinder != 0 || biosPartitionRecord.StartHead != 0 || biosPartitionRecord.StartSector != 0) && (biosPartitionRecord.PartitionType == 5 || biosPartitionRecord.PartitionType == 15))
					{
						num2 = this._firstSector + biosPartitionRecord.LBAStart;
					}
				}
			}
			return list;
		}

		// Token: 0x040000A6 RID: 166
		private readonly Stream _disk;

		// Token: 0x040000A7 RID: 167
		private readonly uint _firstSector;
	}
}
