using System;

namespace DiscUtils.Streams
{
	// Token: 0x02000005 RID: 5
	public sealed class BlockCacheSettings
	{
		// Token: 0x0600001A RID: 26 RVA: 0x000026BA File Offset: 0x000008BA
		public BlockCacheSettings()
		{
			this.BlockSize = 4096;
			this.ReadCacheSize = 4194304L;
			this.LargeReadSize = 65536L;
			this.OptimumReadSize = 65536;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000026F0 File Offset: 0x000008F0
		internal BlockCacheSettings(BlockCacheSettings settings)
		{
			this.BlockSize = settings.BlockSize;
			this.ReadCacheSize = settings.ReadCacheSize;
			this.LargeReadSize = settings.LargeReadSize;
			this.OptimumReadSize = settings.OptimumReadSize;
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002728 File Offset: 0x00000928
		// (set) Token: 0x0600001D RID: 29 RVA: 0x00002730 File Offset: 0x00000930
		public int BlockSize { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002739 File Offset: 0x00000939
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002741 File Offset: 0x00000941
		public long LargeReadSize { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000020 RID: 32 RVA: 0x0000274A File Offset: 0x0000094A
		// (set) Token: 0x06000021 RID: 33 RVA: 0x00002752 File Offset: 0x00000952
		public int OptimumReadSize { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000022 RID: 34 RVA: 0x0000275B File Offset: 0x0000095B
		// (set) Token: 0x06000023 RID: 35 RVA: 0x00002763 File Offset: 0x00000963
		public long ReadCacheSize { get; set; }
	}
}
