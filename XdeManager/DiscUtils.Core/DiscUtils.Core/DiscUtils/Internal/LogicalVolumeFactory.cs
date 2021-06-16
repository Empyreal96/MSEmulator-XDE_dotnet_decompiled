using System;
using System.Collections.Generic;

namespace DiscUtils.Internal
{
	// Token: 0x02000070 RID: 112
	internal abstract class LogicalVolumeFactory
	{
		// Token: 0x0600041A RID: 1050
		public abstract bool HandlesPhysicalVolume(PhysicalVolumeInfo volume);

		// Token: 0x0600041B RID: 1051
		public abstract void MapDisks(IEnumerable<VirtualDisk> disks, Dictionary<string, LogicalVolumeInfo> result);
	}
}
