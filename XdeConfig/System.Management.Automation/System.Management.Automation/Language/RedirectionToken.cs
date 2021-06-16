using System;

namespace System.Management.Automation.Language
{
	// Token: 0x020005D6 RID: 1494
	public abstract class RedirectionToken : Token
	{
		// Token: 0x06003FEE RID: 16366 RVA: 0x00151CA2 File Offset: 0x0014FEA2
		internal RedirectionToken(InternalScriptExtent scriptExtent, TokenKind kind) : base(scriptExtent, kind, TokenFlags.None)
		{
		}
	}
}
