using System;
using System.IO;

namespace DiscUtils.Partitions
{
	// Token: 0x0200004F RID: 79
	[PartitionTableFactory]
	internal sealed class DefaultPartitionTableFactory : PartitionTableFactory
	{
		// Token: 0x06000358 RID: 856 RVA: 0x00008250 File Offset: 0x00006450
		public override bool DetectIsPartitioned(Stream s)
		{
			return BiosPartitionTable.IsValid(s);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00008258 File Offset: 0x00006458
		public override PartitionTable DetectPartitionTable(VirtualDisk disk)
		{
			if (!BiosPartitionTable.IsValid(disk.Content))
			{
				return null;
			}
			BiosPartitionTable biosPartitionTable = new BiosPartitionTable(disk);
			if (biosPartitionTable.Count == 1 && biosPartitionTable[0].BiosType == 238)
			{
				return new GuidPartitionTable(disk);
			}
			return biosPartitionTable;
		}
	}
}
