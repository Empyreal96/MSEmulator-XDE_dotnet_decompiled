using System;

namespace DiscUtils.Streams
{
	// Token: 0x02000006 RID: 6
	public sealed class BlockCacheStatistics
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000024 RID: 36 RVA: 0x0000276C File Offset: 0x0000096C
		// (set) Token: 0x06000025 RID: 37 RVA: 0x00002774 File Offset: 0x00000974
		public int FreeReadBlocks { get; internal set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000026 RID: 38 RVA: 0x0000277D File Offset: 0x0000097D
		// (set) Token: 0x06000027 RID: 39 RVA: 0x00002785 File Offset: 0x00000985
		public long LargeReadsIn { get; internal set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000028 RID: 40 RVA: 0x0000278E File Offset: 0x0000098E
		// (set) Token: 0x06000029 RID: 41 RVA: 0x00002796 File Offset: 0x00000996
		public long ReadCacheHits { get; internal set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002A RID: 42 RVA: 0x0000279F File Offset: 0x0000099F
		// (set) Token: 0x0600002B RID: 43 RVA: 0x000027A7 File Offset: 0x000009A7
		public long ReadCacheMisses { get; internal set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002C RID: 44 RVA: 0x000027B0 File Offset: 0x000009B0
		// (set) Token: 0x0600002D RID: 45 RVA: 0x000027B8 File Offset: 0x000009B8
		public long TotalReadsIn { get; internal set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002E RID: 46 RVA: 0x000027C1 File Offset: 0x000009C1
		// (set) Token: 0x0600002F RID: 47 RVA: 0x000027C9 File Offset: 0x000009C9
		public long TotalReadsOut { get; internal set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000027D2 File Offset: 0x000009D2
		// (set) Token: 0x06000031 RID: 49 RVA: 0x000027DA File Offset: 0x000009DA
		public long TotalWritesIn { get; internal set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000027E3 File Offset: 0x000009E3
		// (set) Token: 0x06000033 RID: 51 RVA: 0x000027EB File Offset: 0x000009EB
		public long UnalignedReadsIn { get; internal set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000034 RID: 52 RVA: 0x000027F4 File Offset: 0x000009F4
		// (set) Token: 0x06000035 RID: 53 RVA: 0x000027FC File Offset: 0x000009FC
		public long UnalignedWritesIn { get; internal set; }
	}
}
