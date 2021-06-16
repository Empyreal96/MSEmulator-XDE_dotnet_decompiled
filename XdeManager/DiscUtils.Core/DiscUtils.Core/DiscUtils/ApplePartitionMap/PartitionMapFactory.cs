using System;
using System.IO;
using DiscUtils.Partitions;
using DiscUtils.Streams;

namespace DiscUtils.ApplePartitionMap
{
	// Token: 0x02000094 RID: 148
	[PartitionTableFactory]
	internal sealed class PartitionMapFactory : PartitionTableFactory
	{
		// Token: 0x06000508 RID: 1288 RVA: 0x0000EC50 File Offset: 0x0000CE50
		public override bool DetectIsPartitioned(Stream s)
		{
			if (s.Length < 1024L)
			{
				return false;
			}
			s.Position = 0L;
			byte[] buffer = StreamUtilities.ReadExact(s, 1024);
			BlockZero blockZero = new BlockZero();
			blockZero.ReadFrom(buffer, 0);
			if (blockZero.Signature != 17746)
			{
				return false;
			}
			PartitionMapEntry partitionMapEntry = new PartitionMapEntry(s);
			partitionMapEntry.ReadFrom(buffer, 512);
			return partitionMapEntry.Signature == 20557;
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0000ECBC File Offset: 0x0000CEBC
		public override PartitionTable DetectPartitionTable(VirtualDisk disk)
		{
			if (!this.DetectIsPartitioned(disk.Content))
			{
				return null;
			}
			return new PartitionMap(disk.Content);
		}
	}
}
