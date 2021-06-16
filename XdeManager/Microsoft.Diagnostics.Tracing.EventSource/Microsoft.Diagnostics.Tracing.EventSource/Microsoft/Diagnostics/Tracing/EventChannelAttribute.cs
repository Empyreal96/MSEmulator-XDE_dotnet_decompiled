using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200002E RID: 46
	[AttributeUsage(AttributeTargets.Field)]
	internal class EventChannelAttribute : Attribute
	{
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x0000B2CE File Offset: 0x000094CE
		// (set) Token: 0x060001AA RID: 426 RVA: 0x0000B2D6 File Offset: 0x000094D6
		public bool Enabled { get; set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001AB RID: 427 RVA: 0x0000B2DF File Offset: 0x000094DF
		// (set) Token: 0x060001AC RID: 428 RVA: 0x0000B2E7 File Offset: 0x000094E7
		public EventChannelType EventChannelType { get; set; }
	}
}
