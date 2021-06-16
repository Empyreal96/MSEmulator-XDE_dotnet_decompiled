using System;
using DiscUtils.Streams;

namespace DiscUtils
{
	// Token: 0x02000019 RID: 25
	public interface IClusterBasedFileSystem : IFileSystem
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000EE RID: 238
		long ClusterSize { get; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000EF RID: 239
		long TotalClusters { get; }

		// Token: 0x060000F0 RID: 240
		long ClusterToOffset(long cluster);

		// Token: 0x060000F1 RID: 241
		long OffsetToCluster(long offset);

		// Token: 0x060000F2 RID: 242
		Range<long, long>[] PathToClusters(string path);

		// Token: 0x060000F3 RID: 243
		StreamExtent[] PathToExtents(string path);

		// Token: 0x060000F4 RID: 244
		ClusterMap BuildClusterMap();
	}
}
