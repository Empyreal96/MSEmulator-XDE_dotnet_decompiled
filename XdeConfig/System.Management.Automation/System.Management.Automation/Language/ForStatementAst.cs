using System;

namespace System.Management.Automation.Language
{
	// Token: 0x0200055B RID: 1371
	public class ForStatementAst : LoopStatementAst
	{
		// Token: 0x0600390B RID: 14603 RVA: 0x0012EAC6 File Offset: 0x0012CCC6
		public ForStatementAst(IScriptExtent extent, string label, PipelineBaseAst initializer, PipelineBaseAst condition, PipelineBaseAst iterator, StatementBlockAst body) : base(extent, label, condition, body)
		{
			if (initializer != null)
			{
				this.Initializer = initializer;
				base.SetParent(initializer);
			}
			if (iterator != null)
			{
				this.Iterator = iterator;
				base.SetParent(iterator);
			}
		}

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x0600390C RID: 14604 RVA: 0x0012EAF9 File Offset: 0x0012CCF9
		// (set) Token: 0x0600390D RID: 14605 RVA: 0x0012EB01 File Offset: 0x0012CD01
		public PipelineBaseAst Initializer { get; private set; }

		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x0600390E RID: 14606 RVA: 0x0012EB0A File Offset: 0x0012CD0A
		// (set) Token: 0x0600390F RID: 14607 RVA: 0x0012EB12 File Offset: 0x0012CD12
		public PipelineBaseAst Iterator { get; private set; }

		// Token: 0x06003910 RID: 14608 RVA: 0x0012EB1C File Offset: 0x0012CD1C
		public override Ast Copy()
		{
			PipelineBaseAst initializer = Ast.CopyElement<PipelineBaseAst>(this.Initializer);
			PipelineBaseAst condition = Ast.CopyElement<PipelineBaseAst>(base.Condition);
			PipelineBaseAst iterator = Ast.CopyElement<PipelineBaseAst>(this.Iterator);
			StatementBlockAst body = Ast.CopyElement<StatementBlockAst>(base.Body);
			return new ForStatementAst(base.Extent, base.Label, initializer, condition, iterator, body);
		}

		// Token: 0x06003911 RID: 14609 RVA: 0x0012EB6E File Offset: 0x0012CD6E
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitForStatement(this);
		}

		// Token: 0x06003912 RID: 14610 RVA: 0x0012EB78 File Offset: 0x0012CD78
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitForStatement(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue && this.Initializer != null)
			{
				astVisitAction = this.Initializer.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue && base.Condition != null)
			{
				astVisitAction = base.Condition.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue && this.Iterator != null)
			{
				astVisitAction = this.Iterator.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = base.Body.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
