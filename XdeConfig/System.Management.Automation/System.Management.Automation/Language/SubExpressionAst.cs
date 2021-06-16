using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x0200058F RID: 1423
	public class SubExpressionAst : ExpressionAst
	{
		// Token: 0x06003B03 RID: 15107 RVA: 0x001369A9 File Offset: 0x00134BA9
		public SubExpressionAst(IScriptExtent extent, StatementBlockAst statementBlock) : base(extent)
		{
			if (statementBlock == null)
			{
				throw PSTraceSource.NewArgumentNullException("statementBlock");
			}
			this.SubExpression = statementBlock;
			base.SetParent(statementBlock);
		}

		// Token: 0x17000D34 RID: 3380
		// (get) Token: 0x06003B04 RID: 15108 RVA: 0x001369CE File Offset: 0x00134BCE
		// (set) Token: 0x06003B05 RID: 15109 RVA: 0x001369D6 File Offset: 0x00134BD6
		public StatementBlockAst SubExpression { get; private set; }

		// Token: 0x06003B06 RID: 15110 RVA: 0x001369E0 File Offset: 0x00134BE0
		public override Ast Copy()
		{
			StatementBlockAst statementBlock = Ast.CopyElement<StatementBlockAst>(this.SubExpression);
			return new SubExpressionAst(base.Extent, statementBlock);
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x00136A05 File Offset: 0x00134C05
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return this.SubExpression.GetInferredType(context);
		}

		// Token: 0x06003B08 RID: 15112 RVA: 0x00136A13 File Offset: 0x00134C13
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitSubExpression(this);
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x00136A1C File Offset: 0x00134C1C
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitSubExpression(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.SubExpression.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
