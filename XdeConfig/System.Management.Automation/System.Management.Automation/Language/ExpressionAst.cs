using System;

namespace System.Management.Automation.Language
{
	// Token: 0x0200053D RID: 1341
	public abstract class ExpressionAst : CommandElementAst
	{
		// Token: 0x060037B0 RID: 14256 RVA: 0x0012AE75 File Offset: 0x00129075
		protected ExpressionAst(IScriptExtent extent) : base(extent)
		{
		}

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x060037B1 RID: 14257 RVA: 0x0012AE7E File Offset: 0x0012907E
		public virtual Type StaticType
		{
			get
			{
				return typeof(object);
			}
		}

		// Token: 0x060037B2 RID: 14258 RVA: 0x0012AE8C File Offset: 0x0012908C
		internal virtual bool ShouldPreserveOutputInCaseOfException()
		{
			ParenExpressionAst parenExpressionAst = this as ParenExpressionAst;
			SubExpressionAst subExpressionAst = this as SubExpressionAst;
			if (parenExpressionAst == null && subExpressionAst == null)
			{
				PSTraceSource.NewInvalidOperationException();
			}
			CommandExpressionAst commandExpressionAst = base.Parent as CommandExpressionAst;
			if (commandExpressionAst == null)
			{
				return false;
			}
			PipelineAst pipelineAst = commandExpressionAst.Parent as PipelineAst;
			if (pipelineAst == null || pipelineAst.PipelineElements.Count > 1)
			{
				return false;
			}
			ParenExpressionAst parenExpressionAst2 = pipelineAst.Parent as ParenExpressionAst;
			if (parenExpressionAst2 != null)
			{
				return parenExpressionAst2.ShouldPreserveOutputInCaseOfException();
			}
			return pipelineAst.Parent is StatementBlockAst || pipelineAst.Parent is NamedBlockAst;
		}
	}
}
