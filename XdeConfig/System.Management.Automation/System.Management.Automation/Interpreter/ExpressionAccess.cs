using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000758 RID: 1880
	[Flags]
	internal enum ExpressionAccess
	{
		// Token: 0x04002441 RID: 9281
		None = 0,
		// Token: 0x04002442 RID: 9282
		Read = 1,
		// Token: 0x04002443 RID: 9283
		Write = 2,
		// Token: 0x04002444 RID: 9284
		ReadWrite = 3
	}
}
