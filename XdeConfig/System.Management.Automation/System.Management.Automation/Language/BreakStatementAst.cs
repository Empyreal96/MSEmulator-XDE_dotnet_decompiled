using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x02000564 RID: 1380
	public class BreakStatementAst : StatementAst
	{
		// Token: 0x0600394B RID: 14667 RVA: 0x0012FAA2 File Offset: 0x0012DCA2
		public BreakStatementAst(IScriptExtent extent, ExpressionAst label) : base(extent)
		{
			if (label != null)
			{
				this.Label = label;
				base.SetParent(label);
			}
		}

		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x0600394C RID: 14668 RVA: 0x0012FABC File Offset: 0x0012DCBC
		// (set) Token: 0x0600394D RID: 14669 RVA: 0x0012FAC4 File Offset: 0x0012DCC4
		public ExpressionAst Label { get; private set; }

		// Token: 0x0600394E RID: 14670 RVA: 0x0012FAD0 File Offset: 0x0012DCD0
		public override Ast Copy()
		{
			ExpressionAst label = Ast.CopyElement<ExpressionAst>(this.Label);
			return new BreakStatementAst(base.Extent, label);
		}

		// Token: 0x0600394F RID: 14671 RVA: 0x0012FAF5 File Offset: 0x0012DCF5
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return Ast.EmptyPSTypeNameArray;
		}

		// Token: 0x06003950 RID: 14672 RVA: 0x0012FAFC File Offset: 0x0012DCFC
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitBreakStatement(this);
		}

		// Token: 0x06003951 RID: 14673 RVA: 0x0012FB08 File Offset: 0x0012DD08
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitBreakStatement(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue && this.Label != null)
			{
				astVisitAction = this.Label.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
