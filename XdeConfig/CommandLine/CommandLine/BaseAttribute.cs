using System;
using CommandLine.Infrastructure;

namespace CommandLine
{
	// Token: 0x0200002C RID: 44
	public abstract class BaseAttribute : Attribute
	{
		// Token: 0x06000109 RID: 265 RVA: 0x00004FB9 File Offset: 0x000031B9
		protected internal BaseAttribute()
		{
			this.min = -1;
			this.max = -1;
			this.helpText = new LocalizableAttributeProperty("HelpText");
			this.metaValue = string.Empty;
			this.resourceType = null;
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00004FF1 File Offset: 0x000031F1
		// (set) Token: 0x0600010B RID: 267 RVA: 0x00004FF9 File Offset: 0x000031F9
		public bool Required { get; set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00005002 File Offset: 0x00003202
		// (set) Token: 0x0600010D RID: 269 RVA: 0x0000500A File Offset: 0x0000320A
		public int Min
		{
			get
			{
				return this.min;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentNullException("value");
				}
				this.min = value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00005022 File Offset: 0x00003222
		// (set) Token: 0x0600010F RID: 271 RVA: 0x0000502A File Offset: 0x0000322A
		public int Max
		{
			get
			{
				return this.max;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentNullException("value");
				}
				this.max = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00005042 File Offset: 0x00003242
		// (set) Token: 0x06000111 RID: 273 RVA: 0x0000504A File Offset: 0x0000324A
		public object Default
		{
			get
			{
				return this.@default;
			}
			set
			{
				this.@default = value;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00005053 File Offset: 0x00003253
		// (set) Token: 0x06000113 RID: 275 RVA: 0x00005069 File Offset: 0x00003269
		public string HelpText
		{
			get
			{
				return this.helpText.Value ?? string.Empty;
			}
			set
			{
				LocalizableAttributeProperty localizableAttributeProperty = this.helpText;
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				localizableAttributeProperty.Value = value;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00005086 File Offset: 0x00003286
		// (set) Token: 0x06000115 RID: 277 RVA: 0x0000508E File Offset: 0x0000328E
		public string MetaValue
		{
			get
			{
				return this.metaValue;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.metaValue = value;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000116 RID: 278 RVA: 0x000050A5 File Offset: 0x000032A5
		// (set) Token: 0x06000117 RID: 279 RVA: 0x000050AD File Offset: 0x000032AD
		public bool Hidden { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000118 RID: 280 RVA: 0x000050B6 File Offset: 0x000032B6
		// (set) Token: 0x06000119 RID: 281 RVA: 0x000050C0 File Offset: 0x000032C0
		public Type ResourceType
		{
			get
			{
				return this.resourceType;
			}
			set
			{
				this.helpText.ResourceType = value;
				this.resourceType = value;
			}
		}

		// Token: 0x04000043 RID: 67
		private int min;

		// Token: 0x04000044 RID: 68
		private int max;

		// Token: 0x04000045 RID: 69
		private object @default;

		// Token: 0x04000046 RID: 70
		private LocalizableAttributeProperty helpText;

		// Token: 0x04000047 RID: 71
		private string metaValue;

		// Token: 0x04000048 RID: 72
		private Type resourceType;
	}
}
