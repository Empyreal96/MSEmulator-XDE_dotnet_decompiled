using System;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000014 RID: 20
	internal class CookedDataRun
	{
		// Token: 0x06000082 RID: 130 RVA: 0x00004364 File Offset: 0x00002564
		public CookedDataRun(DataRun raw, long startVcn, long prevLcn, NonResidentAttributeRecord attributeExtent)
		{
			this.DataRun = raw;
			this.StartVcn = startVcn;
			this.StartLcn = prevLcn + raw.RunOffset;
			this.AttributeExtent = attributeExtent;
			if (startVcn < 0L)
			{
				throw new ArgumentOutOfRangeException("startVcn", startVcn, "VCN must be >= 0");
			}
			if (this.StartLcn < 0L)
			{
				throw new ArgumentOutOfRangeException("prevLcn", prevLcn, "LCN must be >= 0");
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000083 RID: 131 RVA: 0x000043D6 File Offset: 0x000025D6
		public NonResidentAttributeRecord AttributeExtent { get; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000084 RID: 132 RVA: 0x000043DE File Offset: 0x000025DE
		public DataRun DataRun { get; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000085 RID: 133 RVA: 0x000043E6 File Offset: 0x000025E6
		public bool IsSparse
		{
			get
			{
				return this.DataRun.IsSparse;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000086 RID: 134 RVA: 0x000043F3 File Offset: 0x000025F3
		// (set) Token: 0x06000087 RID: 135 RVA: 0x00004400 File Offset: 0x00002600
		public long Length
		{
			get
			{
				return this.DataRun.RunLength;
			}
			set
			{
				this.DataRun.RunLength = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000088 RID: 136 RVA: 0x0000440E File Offset: 0x0000260E
		// (set) Token: 0x06000089 RID: 137 RVA: 0x00004416 File Offset: 0x00002616
		public long StartLcn { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600008A RID: 138 RVA: 0x0000441F File Offset: 0x0000261F
		public long StartVcn { get; }
	}
}
