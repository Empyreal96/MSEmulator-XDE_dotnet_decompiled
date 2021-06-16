using System;
using System.Collections.Generic;
using DiscUtils.Internal;

namespace DiscUtils.LogicalDiskManager
{
	// Token: 0x02000063 RID: 99
	[LogicalVolumeFactory]
	internal class DynamicDiskManagerFactory : LogicalVolumeFactory
	{
		// Token: 0x060003EF RID: 1007 RVA: 0x0000BB04 File Offset: 0x00009D04
		public override bool HandlesPhysicalVolume(PhysicalVolumeInfo volume)
		{
			return DynamicDiskManager.HandlesPhysicalVolume(volume);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000BB0C File Offset: 0x00009D0C
		public override void MapDisks(IEnumerable<VirtualDisk> disks, Dictionary<string, LogicalVolumeInfo> result)
		{
			DynamicDiskManager dynamicDiskManager = new DynamicDiskManager(new VirtualDisk[0]);
			foreach (VirtualDisk disk in disks)
			{
				if (DynamicDiskManager.IsDynamicDisk(disk))
				{
					dynamicDiskManager.Add(disk);
				}
			}
			foreach (LogicalVolumeInfo logicalVolumeInfo in dynamicDiskManager.GetLogicalVolumes())
			{
				result.Add(logicalVolumeInfo.Identity, logicalVolumeInfo);
			}
		}
	}
}
