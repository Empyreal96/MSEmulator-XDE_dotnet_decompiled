using System;

namespace System.Management.Automation.Language
{
	// Token: 0x0200055C RID: 1372
	public class DoWhileStatementAst : LoopStatementAst
	{
		// Token: 0x06003913 RID: 14611 RVA: 0x0012EBFA File Offset: 0x0012CDFA
		public DoWhileStatementAst(IScriptExtent extent, string label, PipelineBaseAst condition, StatementBlockAst body) : base(extent, label, condition, body)
		{
			if (condition == null)
			{
				throw PSTraceSource.NewArgumentNullException("condition");
			}
		}

		// Token: 0x06003914 RID: 14612 RVA: 0x0012EC18 File Offset: 0x0012CE18
		public override Ast Copy()
		{
			PipelineBaseAst condition = Ast.CopyElement<PipelineBaseAst>(base.Condition);
			StatementBlockAst body = Ast.CopyElement<StatementBlockAst>(base.Body);
			return new DoWhileStatementAst(base.Extent, base.Label, condition, body);
		}

		// Token: 0x06003915 RID: 14613 RVA: 0x0012EC50 File Offset: 0x0012CE50
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitDoWhileStatement(this);
		}

		// Token: 0x06003916 RID: 14614 RVA: 0x0012EC5C File Offset: 0x0012CE5C
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitDoWhileStatement(this);
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
