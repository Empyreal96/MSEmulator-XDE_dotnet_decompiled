using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x02000565 RID: 1381
	public class ContinueStatementAst : StatementAst
	{
		// Token: 0x06003952 RID: 14674 RVA: 0x0012FB4A File Offset: 0x0012DD4A
		public ContinueStatementAst(IScriptExtent extent, ExpressionAst label) : base(extent)
		{
			if (label != null)
			{
				this.Label = label;
				base.SetParent(label);
			}
		}

		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x06003953 RID: 14675 RVA: 0x0012FB64 File Offset: 0x0012DD64
		// (set) Token: 0x06003954 RID: 14676 RVA: 0x0012FB6C File Offset: 0x0012DD6C
		public ExpressionAst Label { get; private set; }

		// Token: 0x06003955 RID: 14677 RVA: 0x0012FB78 File Offset: 0x0012DD78
		public override Ast Copy()
		{
			ExpressionAst label = Ast.CopyElement<ExpressionAst>(this.Label);
			return new ContinueStatementAst(base.Extent, label);
		}

		// Token: 0x06003956 RID: 14678 RVA: 0x0012FB9D File Offset: 0x0012DD9D
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return Ast.EmptyPSTypeNameArray;
		}

		// Token: 0x06003957 RID: 14679 RVA: 0x0012FBA4 File Offset: 0x0012DDA4
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitContinueStatement(this);
		}

		// Token: 0x06003958 RID: 14680 RVA: 0x0012FBB0 File Offset: 0x0012DDB0
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitContinueStatement(this);
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
