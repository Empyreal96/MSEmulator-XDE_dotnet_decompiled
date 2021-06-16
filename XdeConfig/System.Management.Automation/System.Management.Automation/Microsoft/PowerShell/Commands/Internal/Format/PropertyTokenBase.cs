using System;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000933 RID: 2355
	internal abstract class PropertyTokenBase : FormatToken
	{
		// Token: 0x04002EC8 RID: 11976
		internal ExpressionToken conditionToken;

		// Token: 0x04002EC9 RID: 11977
		internal ExpressionToken expression = new ExpressionToken();

		// Token: 0x04002ECA RID: 11978
		internal bool enumerateCollection;
	}
}
