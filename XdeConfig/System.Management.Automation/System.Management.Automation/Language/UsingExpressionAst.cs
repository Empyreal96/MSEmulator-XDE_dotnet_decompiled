using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x02000590 RID: 1424
	public class UsingExpressionAst : ExpressionAst
	{
		// Token: 0x06003B0A RID: 15114 RVA: 0x00136A56 File Offset: 0x00134C56
		public UsingExpressionAst(IScriptExtent extent, ExpressionAst expressionAst) : base(extent)
		{
			if (expressionAst == null)
			{
				throw PSTraceSource.NewArgumentNullException("expressionAst");
			}
			this.RuntimeUsingIndex = -1;
			this.SubExpression = expressionAst;
			base.SetParent(this.SubExpression);
		}

		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x06003B0B RID: 15115 RVA: 0x00136A87 File Offset: 0x00134C87
		// (set) Token: 0x06003B0C RID: 15116 RVA: 0x00136A8F File Offset: 0x00134C8F
		public ExpressionAst SubExpression { get; private set; }

		// Token: 0x17000D36 RID: 3382
		// (get) Token: 0x06003B0D RID: 15117 RVA: 0x00136A98 File Offset: 0x00134C98
		// (set) Token: 0x06003B0E RID: 15118 RVA: 0x00136AA0 File Offset: 0x00134CA0
		internal int RuntimeUsingIndex { get; set; }

		// Token: 0x06003B0F RID: 15119 RVA: 0x00136AAC File Offset: 0x00134CAC
		public override Ast Copy()
		{
			ExpressionAst expressionAst = Ast.CopyElement<ExpressionAst>(this.SubExpression);
			return new UsingExpressionAst(base.Extent, expressionAst)
			{
				RuntimeUsingIndex = this.RuntimeUsingIndex
			};
		}

		// Token: 0x06003B10 RID: 15120 RVA: 0x00136ADF File Offset: 0x00134CDF
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return this.SubExpression.GetInferredType(context);
		}

		// Token: 0x06003B11 RID: 15121 RVA: 0x00136AED File Offset: 0x00134CED
		public static VariableExpressionAst ExtractUsingVariable(UsingExpressionAst usingExpressionAst)
		{
			if (usingExpressionAst == null)
			{
				throw new ArgumentNullException("usingExpressionAst");
			}
			return UsingExpressionAst.ExtractUsingVariableImpl(usingExpressionAst);
		}

		// Token: 0x06003B12 RID: 15122 RVA: 0x00136B04 File Offset: 0x00134D04
		private static VariableExpressionAst ExtractUsingVariableImpl(ExpressionAst expression)
		{
			UsingExpressionAst usingExpressionAst = expression as UsingExpressionAst;
			if (usingExpressionAst != null)
			{
				VariableExpressionAst variableExpressionAst = usingExpressionAst.SubExpression as VariableExpressionAst;
				if (variableExpressionAst != null)
				{
					return variableExpressionAst;
				}
				return UsingExpressionAst.ExtractUsingVariableImpl(usingExpressionAst.SubExpression);
			}
			else
			{
				IndexExpressionAst indexExpressionAst = expression as IndexExpressionAst;
				if (indexExpressionAst != null)
				{
					VariableExpressionAst variableExpressionAst = indexExpressionAst.Target as VariableExpressionAst;
					if (variableExpressionAst != null)
					{
						return variableExpressionAst;
					}
					return UsingExpressionAst.ExtractUsingVariableImpl(indexExpressionAst.Target);
				}
				else
				{
					MemberExpressionAst memberExpressionAst = expression as MemberExpressionAst;
					if (memberExpressionAst == null)
					{
						return null;
					}
					VariableExpressionAst variableExpressionAst = memberExpressionAst.Expression as VariableExpressionAst;
					if (variableExpressionAst != null)
					{
						return variableExpressionAst;
					}
					return UsingExpressionAst.ExtractUsingVariableImpl(memberExpressionAst.Expression);
				}
			}
		}

		// Token: 0x06003B13 RID: 15123 RVA: 0x00136B87 File Offset: 0x00134D87
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitUsingExpression(this);
		}

		// Token: 0x06003B14 RID: 15124 RVA: 0x00136B90 File Offset: 0x00134D90
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitUsingExpression(this);
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

		// Token: 0x04001D84 RID: 7556
		internal const string UsingPrefix = "__using_";
	}
}
