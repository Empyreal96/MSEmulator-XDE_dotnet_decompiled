using System;

namespace System.Management.Automation
{
	// Token: 0x020007B3 RID: 1971
	[Flags]
	public enum PSCredentialUIOptions
	{
		// Token: 0x04002676 RID: 9846
		Default = 1,
		// Token: 0x04002677 RID: 9847
		None = 0,
		// Token: 0x04002678 RID: 9848
		ValidateUserNameSyntax = 1,
		// Token: 0x04002679 RID: 9849
		AlwaysPrompt = 2,
		// Token: 0x0400267A RID: 9850
		ReadOnlyUserName = 3
	}
}
