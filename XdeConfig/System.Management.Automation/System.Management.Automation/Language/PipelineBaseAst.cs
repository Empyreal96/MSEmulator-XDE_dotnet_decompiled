using System;

namespace System.Management.Automation.Language
{
	// Token: 0x0200053A RID: 1338
	public abstract class PipelineBaseAst : StatementAst
	{
		// Token: 0x0600379C RID: 14236 RVA: 0x0012A98A File Offset: 0x00128B8A
		protected PipelineBaseAst(IScriptExtent extent) : base(extent)
		{
		}

		// Token: 0x0600379D RID: 14237 RVA: 0x0012A993 File Offset: 0x00128B93
		public virtual ExpressionAst GetPureExpression()
		{
			return null;
		}
	}
}
