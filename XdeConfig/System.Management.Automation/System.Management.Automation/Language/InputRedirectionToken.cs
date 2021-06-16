using System;

namespace System.Management.Automation.Language
{
	// Token: 0x020005D7 RID: 1495
	public class InputRedirectionToken : RedirectionToken
	{
		// Token: 0x06003FEF RID: 16367 RVA: 0x00151CAD File Offset: 0x0014FEAD
		internal InputRedirectionToken(InternalScriptExtent scriptExtent) : base(scriptExtent, TokenKind.RedirectInStd)
		{
		}
	}
}
