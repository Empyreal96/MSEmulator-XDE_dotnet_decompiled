using System;

namespace System.Management.Automation.Language
{
	// Token: 0x02000539 RID: 1337
	public abstract class StatementAst : Ast
	{
		// Token: 0x0600379B RID: 14235 RVA: 0x0012A981 File Offset: 0x00128B81
		protected StatementAst(IScriptExtent extent) : base(extent)
		{
		}
	}
}
