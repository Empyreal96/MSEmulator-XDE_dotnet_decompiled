using System;

namespace System.Management.Automation.Language
{
	// Token: 0x0200055D RID: 1373
	public class DoUntilStatementAst : LoopStatementAst
	{
		// Token: 0x06003917 RID: 14615 RVA: 0x0012ECA6 File Offset: 0x0012CEA6
		public DoUntilStatementAst(IScriptExtent extent, string label, PipelineBaseAst condition, StatementBlockAst body) : base(extent, label, condition, body)
		{
			if (condition == null)
			{
				throw PSTraceSource.NewArgumentNullException("condition");
			}
		}

		// Token: 0x06003918 RID: 14616 RVA: 0x0012ECC4 File Offset: 0x0012CEC4
		public override Ast Copy()
		{
			PipelineBaseAst condition = Ast.CopyElement<PipelineBaseAst>(base.Condition);
			StatementBlockAst body = Ast.CopyElement<StatementBlockAst>(base.Body);
			return new DoUntilStatementAst(base.Extent, base.Label, condition, body);
		}

		// Token: 0x06003919 RID: 14617 RVA: 0x0012ECFC File Offset: 0x0012CEFC
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitDoUntilStatement(this);
		}

		// Token: 0x0600391A RID: 14618 RVA: 0x0012ED08 File Offset: 0x0012CF08
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitDoUntilStatement(this);
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
