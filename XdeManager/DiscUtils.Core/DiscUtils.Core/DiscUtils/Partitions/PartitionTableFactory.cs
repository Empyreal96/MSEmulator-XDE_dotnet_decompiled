using System;
using System.IO;

namespace DiscUtils.Partitions
{
	// Token: 0x02000057 RID: 87
	internal abstract class PartitionTableFactory
	{
		// Token: 0x060003AD RID: 941
		public abstract bool DetectIsPartitioned(Stream s);

		// Token: 0x060003AE RID: 942
		public abstract PartitionTable DetectPartitionTable(VirtualDisk disk);
	}
}
