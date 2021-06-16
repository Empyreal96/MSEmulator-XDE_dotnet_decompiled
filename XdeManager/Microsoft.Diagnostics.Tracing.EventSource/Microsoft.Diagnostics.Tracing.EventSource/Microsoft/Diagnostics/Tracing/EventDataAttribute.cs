using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000078 RID: 120
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
	public class EventDataAttribute : Attribute
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000300 RID: 768 RVA: 0x0000F9D2 File Offset: 0x0000DBD2
		// (set) Token: 0x06000301 RID: 769 RVA: 0x0000F9DA File Offset: 0x0000DBDA
		public string Name { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000302 RID: 770 RVA: 0x0000F9E3 File Offset: 0x0000DBE3
		// (set) Token: 0x06000303 RID: 771 RVA: 0x0000F9EB File Offset: 0x0000DBEB
		internal EventLevel Level
		{
			get
			{
				return this.level;
			}
			set
			{
				this.level = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0000F9F4 File Offset: 0x0000DBF4
		// (set) Token: 0x06000305 RID: 773 RVA: 0x0000F9FC File Offset: 0x0000DBFC
		internal EventOpcode Opcode
		{
			get
			{
				return this.opcode;
			}
			set
			{
				this.opcode = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000306 RID: 774 RVA: 0x0000FA05 File Offset: 0x0000DC05
		// (set) Token: 0x06000307 RID: 775 RVA: 0x0000FA0D File Offset: 0x0000DC0D
		internal EventKeywords Keywords { get; set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000308 RID: 776 RVA: 0x0000FA16 File Offset: 0x0000DC16
		// (set) Token: 0x06000309 RID: 777 RVA: 0x0000FA1E File Offset: 0x0000DC1E
		internal EventTags Tags { get; set; }

		// Token: 0x04000179 RID: 377
		private EventLevel level = (EventLevel)(-1);

		// Token: 0x0400017A RID: 378
		private EventOpcode opcode = (EventOpcode)(-1);
	}
}
