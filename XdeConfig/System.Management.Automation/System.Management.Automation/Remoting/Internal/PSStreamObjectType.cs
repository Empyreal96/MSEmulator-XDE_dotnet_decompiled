using System;

namespace System.Management.Automation.Remoting.Internal
{
	// Token: 0x020002AD RID: 685
	public enum PSStreamObjectType
	{
		// Token: 0x04000EA6 RID: 3750
		Output = 1,
		// Token: 0x04000EA7 RID: 3751
		Error,
		// Token: 0x04000EA8 RID: 3752
		MethodExecutor,
		// Token: 0x04000EA9 RID: 3753
		Warning,
		// Token: 0x04000EAA RID: 3754
		BlockingError,
		// Token: 0x04000EAB RID: 3755
		ShouldMethod,
		// Token: 0x04000EAC RID: 3756
		WarningRecord,
		// Token: 0x04000EAD RID: 3757
		Debug,
		// Token: 0x04000EAE RID: 3758
		Progress,
		// Token: 0x04000EAF RID: 3759
		Verbose,
		// Token: 0x04000EB0 RID: 3760
		Information,
		// Token: 0x04000EB1 RID: 3761
		Exception
	}
}
