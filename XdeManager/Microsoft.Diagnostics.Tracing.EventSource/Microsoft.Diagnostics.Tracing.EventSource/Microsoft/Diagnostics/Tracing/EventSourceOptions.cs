using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000080 RID: 128
	public struct EventSourceOptions
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600032A RID: 810 RVA: 0x0000FF77 File Offset: 0x0000E177
		// (set) Token: 0x0600032B RID: 811 RVA: 0x0000FF7F File Offset: 0x0000E17F
		public EventLevel Level
		{
			get
			{
				return (EventLevel)this.level;
			}
			set
			{
				this.level = checked((byte)value);
				this.valuesSet |= 4;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600032C RID: 812 RVA: 0x0000FF98 File Offset: 0x0000E198
		// (set) Token: 0x0600032D RID: 813 RVA: 0x0000FFA0 File Offset: 0x0000E1A0
		public EventOpcode Opcode
		{
			get
			{
				return (EventOpcode)this.opcode;
			}
			set
			{
				this.opcode = checked((byte)value);
				this.valuesSet |= 8;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600032E RID: 814 RVA: 0x0000FFB9 File Offset: 0x0000E1B9
		internal bool IsOpcodeSet
		{
			get
			{
				return (this.valuesSet & 8) != 0;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600032F RID: 815 RVA: 0x0000FFC9 File Offset: 0x0000E1C9
		// (set) Token: 0x06000330 RID: 816 RVA: 0x0000FFD1 File Offset: 0x0000E1D1
		public EventKeywords Keywords
		{
			get
			{
				return this.keywords;
			}
			set
			{
				this.keywords = value;
				this.valuesSet |= 1;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000331 RID: 817 RVA: 0x0000FFE9 File Offset: 0x0000E1E9
		// (set) Token: 0x06000332 RID: 818 RVA: 0x0000FFF1 File Offset: 0x0000E1F1
		public EventTags Tags
		{
			get
			{
				return this.tags;
			}
			set
			{
				this.tags = value;
				this.valuesSet |= 2;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000333 RID: 819 RVA: 0x00010009 File Offset: 0x0000E209
		// (set) Token: 0x06000334 RID: 820 RVA: 0x00010011 File Offset: 0x0000E211
		public EventActivityOptions ActivityOptions
		{
			get
			{
				return this.activityOptions;
			}
			set
			{
				this.activityOptions = value;
				this.valuesSet |= 16;
			}
		}

		// Token: 0x04000197 RID: 407
		internal const byte keywordsSet = 1;

		// Token: 0x04000198 RID: 408
		internal const byte tagsSet = 2;

		// Token: 0x04000199 RID: 409
		internal const byte levelSet = 4;

		// Token: 0x0400019A RID: 410
		internal const byte opcodeSet = 8;

		// Token: 0x0400019B RID: 411
		internal const byte activityOptionsSet = 16;

		// Token: 0x0400019C RID: 412
		internal EventKeywords keywords;

		// Token: 0x0400019D RID: 413
		internal EventTags tags;

		// Token: 0x0400019E RID: 414
		internal EventActivityOptions activityOptions;

		// Token: 0x0400019F RID: 415
		internal byte level;

		// Token: 0x040001A0 RID: 416
		internal byte opcode;

		// Token: 0x040001A1 RID: 417
		internal byte valuesSet;
	}
}
