using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006D7 RID: 1751
	internal enum LabelScopeKind
	{
		// Token: 0x04002379 RID: 9081
		Statement,
		// Token: 0x0400237A RID: 9082
		Block,
		// Token: 0x0400237B RID: 9083
		Switch,
		// Token: 0x0400237C RID: 9084
		Lambda,
		// Token: 0x0400237D RID: 9085
		Try,
		// Token: 0x0400237E RID: 9086
		Catch,
		// Token: 0x0400237F RID: 9087
		Finally,
		// Token: 0x04002380 RID: 9088
		Filter,
		// Token: 0x04002381 RID: 9089
		Expression
	}
}
