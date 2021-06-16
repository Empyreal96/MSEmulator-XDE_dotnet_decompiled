using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Management.Automation.Language
{
	// Token: 0x020005AC RID: 1452
	internal class ArrayAssignableValue : IAssignableValue
	{
		// Token: 0x17000D5E RID: 3422
		// (get) Token: 0x06003D86 RID: 15750 RVA: 0x00142295 File Offset: 0x00140495
		// (set) Token: 0x06003D87 RID: 15751 RVA: 0x0014229D File Offset: 0x0014049D
		internal ArrayLiteralAst ArrayLiteral { get; set; }

		// Token: 0x06003D88 RID: 15752 RVA: 0x001422A6 File Offset: 0x001404A6
		public Expression GetValue(Compiler compiler, List<Expression> exprs, List<ParameterExpression> temps)
		{
			return null;
		}

		// Token: 0x06003D89 RID: 15753 RVA: 0x001422AC File Offset: 0x001404AC
		public Expression SetValue(Compiler compiler, Expression rhs)
		{
			ParameterExpression parameterExpression = Expression.Variable(rhs.Type);
			int count = this.ArrayLiteral.Elements.Count;
			List<Expression> list = new List<Expression>();
			list.Add(Expression.Assign(parameterExpression, rhs));
			for (int i = 0; i < count; i++)
			{
				Expression expression = Expression.Call(parameterExpression, CachedReflectionInfo.IList_get_Item, new Expression[]
				{
					ExpressionCache.Constant(i)
				});
				ExpressionAst expressionAst = this.ArrayLiteral.Elements[i];
				ArrayLiteralAst arrayLiteralAst = expressionAst as ArrayLiteralAst;
				ParenExpressionAst parenExpressionAst = expressionAst as ParenExpressionAst;
				if (parenExpressionAst != null)
				{
					arrayLiteralAst = (parenExpressionAst.Pipeline.GetPureExpression() as ArrayLiteralAst);
				}
				if (arrayLiteralAst != null)
				{
					expression = DynamicExpression.Dynamic(PSArrayAssignmentRHSBinder.Get(arrayLiteralAst.Elements.Count), typeof(IList), expression);
				}
				list.Add(compiler.ReduceAssignment((ISupportsAssignment)expressionAst, TokenKind.Equals, expression));
			}
			list.Add(parameterExpression);
			return Expression.Block(new ParameterExpression[]
			{
				parameterExpression
			}, list);
		}
	}
}
