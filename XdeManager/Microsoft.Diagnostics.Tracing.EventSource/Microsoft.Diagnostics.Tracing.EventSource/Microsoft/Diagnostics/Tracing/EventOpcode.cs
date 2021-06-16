using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200008B RID: 139
	public enum EventOpcode
	{
		// Token: 0x040001CE RID: 462
		Info,
		// Token: 0x040001CF RID: 463
		Start,
		// Token: 0x040001D0 RID: 464
		Stop,
		// Token: 0x040001D1 RID: 465
		DataCollectionStart,
		// Token: 0x040001D2 RID: 466
		DataCollectionStop,
		// Token: 0x040001D3 RID: 467
		Extension,
		// Token: 0x040001D4 RID: 468
		Reply,
		// Token: 0x040001D5 RID: 469
		Resume,
		// Token: 0x040001D6 RID: 470
		Suspend,
		// Token: 0x040001D7 RID: 471
		Send,
		// Token: 0x040001D8 RID: 472
		Receive = 240
	}
}
