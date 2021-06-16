using System;

namespace System.Management.Automation.Host
{
	// Token: 0x02000220 RID: 544
	[Flags]
	public enum ReadKeyOptions
	{
		// Token: 0x04000A89 RID: 2697
		AllowCtrlC = 1,
		// Token: 0x04000A8A RID: 2698
		NoEcho = 2,
		// Token: 0x04000A8B RID: 2699
		IncludeKeyDown = 4,
		// Token: 0x04000A8C RID: 2700
		IncludeKeyUp = 8
	}
}
