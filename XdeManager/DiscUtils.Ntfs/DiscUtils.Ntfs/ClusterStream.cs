using System;
using System.Collections.Generic;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000012 RID: 18
	internal abstract class ClusterStream
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600006C RID: 108
		public abstract long AllocatedClusterCount { get; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600006D RID: 109
		public abstract IEnumerable<Range<long, long>> StoredClusters { get; }

		// Token: 0x0600006E RID: 110
		public abstract bool IsClusterStored(long vcn);

		// Token: 0x0600006F RID: 111
		public abstract void ExpandToClusters(long numVirtualClusters, NonResidentAttributeRecord extent, bool allocate);

		// Token: 0x06000070 RID: 112
		public abstract void TruncateToClusters(long numVirtualClusters);

		// Token: 0x06000071 RID: 113
		public abstract void ReadClusters(long startVcn, int count, byte[] buffer, int offset);

		// Token: 0x06000072 RID: 114
		public abstract int WriteClusters(long startVcn, int count, byte[] buffer, int offset);

		// Token: 0x06000073 RID: 115
		public abstract int ClearClusters(long startVcn, int count);
	}
}
