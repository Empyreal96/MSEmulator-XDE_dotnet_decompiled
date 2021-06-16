using System;

namespace System.Management.Automation.Language
{
	// Token: 0x020005D3 RID: 1491
	public class StringLiteralToken : StringToken
	{
		// Token: 0x06003FE5 RID: 16357 RVA: 0x00151B8F File Offset: 0x0014FD8F
		internal StringLiteralToken(InternalScriptExtent scriptExtent, TokenFlags flags, TokenKind tokenKind, string value) : base(scriptExtent, tokenKind, flags, value)
		{
		}
	}
}
