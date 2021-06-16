using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Partitions;
using DiscUtils.Streams;

namespace DiscUtils.LogicalDiskManager
{
	// Token: 0x02000062 RID: 98
	public class DynamicDiskManager : IDiagnosticTraceable
	{
		// Token: 0x060003E8 RID: 1000 RVA: 0x0000B8A8 File Offset: 0x00009AA8
		public DynamicDiskManager(params VirtualDisk[] disks)
		{
			this._groups = new Dictionary<string, DynamicDiskGroup>();
			foreach (VirtualDisk disk in disks)
			{
				this.Add(disk);
			}
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000B8E4 File Offset: 0x00009AE4
		public void Dump(TextWriter writer, string linePrefix)
		{
			writer.WriteLine(linePrefix + "DISK GROUPS");
			foreach (DynamicDiskGroup dynamicDiskGroup in this._groups.Values)
			{
				dynamicDiskGroup.Dump(writer, linePrefix + "  ");
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000B958 File Offset: 0x00009B58
		public static bool HandlesPhysicalVolume(PhysicalVolumeInfo volumeInfo)
		{
			PartitionInfo partition = volumeInfo.Partition;
			return partition != null && DynamicDiskManager.IsLdmPartition(partition);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000B978 File Offset: 0x00009B78
		public static bool IsDynamicDisk(VirtualDisk disk)
		{
			if (disk.IsPartitioned)
			{
				using (IEnumerator<PartitionInfo> enumerator = disk.Partitions.Partitions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (DynamicDiskManager.IsLdmPartition(enumerator.Current))
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000B9D8 File Offset: 0x00009BD8
		public void Add(VirtualDisk disk)
		{
			PrivateHeader privateHeader = DynamicDisk.GetPrivateHeader(disk);
			DynamicDiskGroup dynamicDiskGroup;
			if (this._groups.TryGetValue(privateHeader.DiskGroupId, out dynamicDiskGroup))
			{
				dynamicDiskGroup.Add(disk);
				return;
			}
			dynamicDiskGroup = new DynamicDiskGroup(disk);
			this._groups.Add(privateHeader.DiskGroupId, dynamicDiskGroup);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000BA24 File Offset: 0x00009C24
		public LogicalVolumeInfo[] GetLogicalVolumes()
		{
			List<LogicalVolumeInfo> list = new List<LogicalVolumeInfo>();
			foreach (DynamicDiskGroup dynamicDiskGroup in this._groups.Values)
			{
				foreach (DynamicVolume dynamicVolume in dynamicDiskGroup.GetVolumes())
				{
					LogicalVolumeInfo item = new LogicalVolumeInfo(dynamicVolume.Identity, null, new SparseStreamOpenDelegate(dynamicVolume.Open), dynamicVolume.Length, dynamicVolume.BiosType, dynamicVolume.Status);
					list.Add(item);
				}
			}
			return list.ToArray();
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000BAD4 File Offset: 0x00009CD4
		private static bool IsLdmPartition(PartitionInfo partition)
		{
			return partition.BiosType == 66 || partition.GuidType == GuidPartitionTypes.WindowsLdmMetadata || partition.GuidType == GuidPartitionTypes.WindowsLdmData;
		}

		// Token: 0x0400012D RID: 301
		private readonly Dictionary<string, DynamicDiskGroup> _groups;
	}
}
