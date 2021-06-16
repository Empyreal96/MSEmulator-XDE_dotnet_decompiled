using System;

namespace System.Management.Automation.Language
{
	// Token: 0x0200055E RID: 1374
	public class WhileStatementAst : LoopStatementAst
	{
		// Token: 0x0600391B RID: 14619 RVA: 0x0012ED52 File Offset: 0x0012CF52
		public WhileStatementAst(IScriptExtent extent, string label, PipelineBaseAst condition, StatementBlockAst body) : base(extent, label, condition, body)
		{
			if (condition == null)
			{
				throw PSTraceSource.NewArgumentNullException("condition");
			}
		}

		// Token: 0x0600391C RID: 14620 RVA: 0x0012ED70 File Offset: 0x0012CF70
		public override Ast Copy()
		{
			PipelineBaseAst condition = Ast.CopyElement<PipelineBaseAst>(base.Condition);
			StatementBlockAst body = Ast.CopyElement<StatementBlockAst>(base.Body);
			return new WhileStatementAst(base.Extent, base.Label, condition, body);
		}

		// Token: 0x0600391D RID: 14621 RVA: 0x0012EDA8 File Offset: 0x0012CFA8
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitWhileStatement(this);
		}

		// Token: 0x0600391E RID: 14622 RVA: 0x0012EDB4 File Offset: 0x0012CFB4
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitWhileStatement(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = base.Condition.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = base.Body.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
