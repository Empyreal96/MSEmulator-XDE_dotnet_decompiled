using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x02000572 RID: 1394
	public class AssignmentStatementAst : PipelineBaseAst
	{
		// Token: 0x060039AD RID: 14765 RVA: 0x00131058 File Offset: 0x0012F258
		public AssignmentStatementAst(IScriptExtent extent, ExpressionAst left, TokenKind @operator, StatementAst right, IScriptExtent errorPosition) : base(extent)
		{
			if (left == null || right == null || errorPosition == null)
			{
				throw PSTraceSource.NewArgumentNullException((left == null) ? "left" : ((right == null) ? "right" : "errorPosition"));
			}
			if ((@operator.GetTraits() & TokenFlags.AssignmentOperator) == TokenFlags.None)
			{
				throw PSTraceSource.NewArgumentException("operator");
			}
			PipelineAst pipelineAst = right as PipelineAst;
			if (pipelineAst != null && pipelineAst.PipelineElements.Count == 1)
			{
				CommandExpressionAst commandExpressionAst = pipelineAst.PipelineElements[0] as CommandExpressionAst;
				if (commandExpressionAst != null)
				{
					right = commandExpressionAst;
					right.ClearParent();
				}
			}
			this.Operator = @operator;
			this.Left = left;
			base.SetParent(left);
			this.Right = right;
			base.SetParent(right);
			this.ErrorPosition = errorPosition;
		}

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x060039AE RID: 14766 RVA: 0x00131115 File Offset: 0x0012F315
		// (set) Token: 0x060039AF RID: 14767 RVA: 0x0013111D File Offset: 0x0012F31D
		public ExpressionAst Left { get; private set; }

		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x060039B0 RID: 14768 RVA: 0x00131126 File Offset: 0x0012F326
		// (set) Token: 0x060039B1 RID: 14769 RVA: 0x0013112E File Offset: 0x0012F32E
		public TokenKind Operator { get; private set; }

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x060039B2 RID: 14770 RVA: 0x00131137 File Offset: 0x0012F337
		// (set) Token: 0x060039B3 RID: 14771 RVA: 0x0013113F File Offset: 0x0012F33F
		public StatementAst Right { get; private set; }

		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x060039B4 RID: 14772 RVA: 0x00131148 File Offset: 0x0012F348
		// (set) Token: 0x060039B5 RID: 14773 RVA: 0x00131150 File Offset: 0x0012F350
		public IScriptExtent ErrorPosition { get; private set; }

		// Token: 0x060039B6 RID: 14774 RVA: 0x0013115C File Offset: 0x0012F35C
		public override Ast Copy()
		{
			ExpressionAst left = Ast.CopyElement<ExpressionAst>(this.Left);
			StatementAst right = Ast.CopyElement<StatementAst>(this.Right);
			return new AssignmentStatementAst(base.Extent, left, this.Operator, right, this.ErrorPosition);
		}

		// Token: 0x060039B7 RID: 14775 RVA: 0x0013119A File Offset: 0x0012F39A
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return this.Left.GetInferredType(context);
		}

		// Token: 0x060039B8 RID: 14776 RVA: 0x0013137C File Offset: 0x0012F57C
		public IEnumerable<ExpressionAst> GetAssignmentTargets()
		{
			ArrayLiteralAst arrayExpression = this.Left as ArrayLiteralAst;
			if (arrayExpression != null)
			{
				foreach (ExpressionAst element in arrayExpression.Elements)
				{
					yield return element;
				}
			}
			else
			{
				yield return this.Left;
			}
			yield break;
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x00131399 File Offset: 0x0012F599
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitAssignmentStatement(this);
		}

		// Token: 0x060039BA RID: 14778 RVA: 0x001313A4 File Offset: 0x0012F5A4
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitAssignmentStatement(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Left.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Right.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
