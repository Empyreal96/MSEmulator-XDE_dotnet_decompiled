using System;

namespace System.Management.Automation
{
	// Token: 0x020000E9 RID: 233
	[Flags]
	public enum DebugModes
	{
		// Token: 0x040005BA RID: 1466
		None = 0,
		// Token: 0x040005BB RID: 1467
		Default = 1,
		// Token: 0x040005BC RID: 1468
		LocalScript = 2,
		// Token: 0x040005BD RID: 1469
		RemoteScript = 4
	}
}
