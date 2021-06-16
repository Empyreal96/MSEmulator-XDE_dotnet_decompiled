using System;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000932 RID: 2354
	internal sealed class ExpressionToken
	{
		// Token: 0x0600579E RID: 22430 RVA: 0x001C9256 File Offset: 0x001C7456
		internal ExpressionToken()
		{
		}

		// Token: 0x0600579F RID: 22431 RVA: 0x001C925E File Offset: 0x001C745E
		internal ExpressionToken(string expressionValue, bool isScriptBlock)
		{
			this.expressionValue = expressionValue;
			this.isScriptBlock = isScriptBlock;
		}

		// Token: 0x04002EC6 RID: 11974
		internal bool isScriptBlock;

		// Token: 0x04002EC7 RID: 11975
		internal string expressionValue;
	}
}
