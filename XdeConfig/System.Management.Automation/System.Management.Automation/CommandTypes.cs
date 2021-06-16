using System;

namespace System.Management.Automation
{
	// Token: 0x02000057 RID: 87
	[Flags]
	public enum CommandTypes
	{
		// Token: 0x040001C6 RID: 454
		Alias = 1,
		// Token: 0x040001C7 RID: 455
		Function = 2,
		// Token: 0x040001C8 RID: 456
		Filter = 4,
		// Token: 0x040001C9 RID: 457
		Cmdlet = 8,
		// Token: 0x040001CA RID: 458
		ExternalScript = 16,
		// Token: 0x040001CB RID: 459
		Application = 32,
		// Token: 0x040001CC RID: 460
		Script = 64,
		// Token: 0x040001CD RID: 461
		Workflow = 128,
		// Token: 0x040001CE RID: 462
		Configuration = 256,
		// Token: 0x040001CF RID: 463
		All = 511
	}
}
