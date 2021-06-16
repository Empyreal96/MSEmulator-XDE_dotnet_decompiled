using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace System.Management.Automation.Language
{
	// Token: 0x020005AB RID: 1451
	internal class IndexAssignableValue : IAssignableValue
	{
		// Token: 0x17000D5D RID: 3421
		// (get) Token: 0x06003D7E RID: 15742 RVA: 0x00142029 File Offset: 0x00140229
		// (set) Token: 0x06003D7F RID: 15743 RVA: 0x00142031 File Offset: 0x00140231
		internal IndexExpressionAst IndexExpressionAst { get; set; }

		// Token: 0x06003D80 RID: 15744 RVA: 0x0014203A File Offset: 0x0014023A
		private PSMethodInvocationConstraints GetInvocationConstraints()
		{
			return Compiler.CombineTypeConstraintForMethodResolution(Compiler.GetTypeConstraintForMethodResolution(this.IndexExpressionAst.Target), Compiler.GetTypeConstraintForMethodResolution(this.IndexExpressionAst.Index));
		}

		// Token: 0x06003D81 RID: 15745 RVA: 0x00142061 File Offset: 0x00140261
		private Expression GetTargetExpr(Compiler compiler)
		{
			return this._targetExprTemp ?? compiler.Compile(this.IndexExpressionAst.Target);
		}

		// Token: 0x06003D82 RID: 15746 RVA: 0x0014207E File Offset: 0x0014027E
		private Expression GetIndexExpr(Compiler compiler)
		{
			return this._indexExprTemp ?? compiler.Compile(this.IndexExpressionAst.Index);
		}

		// Token: 0x06003D83 RID: 15747 RVA: 0x0014209C File Offset: 0x0014029C
		public Expression GetValue(Compiler compiler, List<Expression> exprs, List<ParameterExpression> temps)
		{
			Expression expression = compiler.Compile(this.IndexExpressionAst.Target);
			this._targetExprTemp = Expression.Variable(expression.Type);
			temps.Add(this._targetExprTemp);
			exprs.Add(Expression.Assign(this._targetExprTemp, expression));
			ExpressionAst index = this.IndexExpressionAst.Index;
			ArrayLiteralAst arrayLiteralAst = index as ArrayLiteralAst;
			PSMethodInvocationConstraints invocationConstraints = this.GetInvocationConstraints();
			Expression result;
			if (arrayLiteralAst != null)
			{
				result = DynamicExpression.Dynamic(PSGetIndexBinder.Get(arrayLiteralAst.Elements.Count, invocationConstraints, true), typeof(object), arrayLiteralAst.Elements.Select(new Func<ExpressionAst, Expression>(compiler.Compile)).Prepend(this._targetExprTemp));
			}
			else
			{
				Expression expression2 = compiler.Compile(index);
				this._indexExprTemp = Expression.Variable(expression2.Type);
				temps.Add(this._indexExprTemp);
				exprs.Add(Expression.Assign(this._indexExprTemp, expression2));
				result = DynamicExpression.Dynamic(PSGetIndexBinder.Get(1, invocationConstraints, true), typeof(object), this._targetExprTemp, this._indexExprTemp);
			}
			return result;
		}

		// Token: 0x06003D84 RID: 15748 RVA: 0x001421B0 File Offset: 0x001403B0
		public Expression SetValue(Compiler compiler, Expression rhs)
		{
			ParameterExpression parameterExpression = Expression.Variable(rhs.Type);
			ExpressionAst index = this.IndexExpressionAst.Index;
			ArrayLiteralAst arrayLiteralAst = index as ArrayLiteralAst;
			PSMethodInvocationConstraints invocationConstraints = this.GetInvocationConstraints();
			Expression targetExpr = this.GetTargetExpr(compiler);
			Expression expression;
			if (arrayLiteralAst != null)
			{
				expression = DynamicExpression.Dynamic(PSSetIndexBinder.Get(arrayLiteralAst.Elements.Count, invocationConstraints), typeof(object), arrayLiteralAst.Elements.Select(new Func<ExpressionAst, Expression>(compiler.Compile)).Prepend(targetExpr).Append(parameterExpression));
			}
			else
			{
				expression = DynamicExpression.Dynamic(PSSetIndexBinder.Get(1, invocationConstraints), typeof(object), targetExpr, this.GetIndexExpr(compiler), parameterExpression);
			}
			return Expression.Block(new ParameterExpression[]
			{
				parameterExpression
			}, new Expression[]
			{
				Expression.Assign(parameterExpression, rhs),
				expression,
				parameterExpression
			});
		}

		// Token: 0x04001F06 RID: 7942
		private ParameterExpression _targetExprTemp;

		// Token: 0x04001F07 RID: 7943
		private ParameterExpression _indexExprTemp;
	}
}
