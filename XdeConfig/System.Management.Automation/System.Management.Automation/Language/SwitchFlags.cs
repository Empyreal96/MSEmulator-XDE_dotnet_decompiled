using System;

namespace System.Management.Automation.Language
{
	// Token: 0x0200055F RID: 1375
	[Flags]
	public enum SwitchFlags
	{
		// Token: 0x04001CF6 RID: 7414
		None = 0,
		// Token: 0x04001CF7 RID: 7415
		File = 1,
		// Token: 0x04001CF8 RID: 7416
		Regex = 2,
		// Token: 0x04001CF9 RID: 7417
		Wildcard = 4,
		// Token: 0x04001CFA RID: 7418
		Exact = 8,
		// Token: 0x04001CFB RID: 7419
		CaseSensitive = 16,
		// Token: 0x04001CFC RID: 7420
		Parallel = 32
	}
}
