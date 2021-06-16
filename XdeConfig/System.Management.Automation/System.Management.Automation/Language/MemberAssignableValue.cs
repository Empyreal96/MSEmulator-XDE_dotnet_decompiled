using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Management.Automation.Language
{
	// Token: 0x020005A9 RID: 1449
	internal class MemberAssignableValue : IAssignableValue
	{
		// Token: 0x17000D59 RID: 3417
		// (get) Token: 0x06003D69 RID: 15721 RVA: 0x00141BAB File Offset: 0x0013FDAB
		// (set) Token: 0x06003D6A RID: 15722 RVA: 0x00141BB3 File Offset: 0x0013FDB3
		internal MemberExpressionAst MemberExpression { get; set; }

		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x06003D6B RID: 15723 RVA: 0x00141BBC File Offset: 0x0013FDBC
		// (set) Token: 0x06003D6C RID: 15724 RVA: 0x00141BC4 File Offset: 0x0013FDC4
		private Expression CachedTarget { get; set; }

		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x06003D6D RID: 15725 RVA: 0x00141BCD File Offset: 0x0013FDCD
		// (set) Token: 0x06003D6E RID: 15726 RVA: 0x00141BD5 File Offset: 0x0013FDD5
		private Expression CachedPropertyExpr { get; set; }

		// Token: 0x06003D6F RID: 15727 RVA: 0x00141BDE File Offset: 0x0013FDDE
		private Expression GetTargetExpr(Compiler compiler)
		{
			return compiler.Compile(this.MemberExpression.Expression);
		}

		// Token: 0x06003D70 RID: 15728 RVA: 0x00141BF1 File Offset: 0x0013FDF1
		private Expression GetPropertyExpr(Compiler compiler)
		{
			return compiler.Compile(this.MemberExpression.Member);
		}

		// Token: 0x06003D71 RID: 15729 RVA: 0x00141C04 File Offset: 0x0013FE04
		public Expression GetValue(Compiler compiler, List<Expression> exprs, List<ParameterExpression> temps)
		{
			Expression targetExpr = this.GetTargetExpr(compiler);
			ParameterExpression parameterExpression = Expression.Parameter(targetExpr.Type);
			temps.Add(parameterExpression);
			this.CachedTarget = parameterExpression;
			exprs.Add(Expression.Assign(parameterExpression, targetExpr));
			StringConstantExpressionAst stringConstantExpressionAst = this.MemberExpression.Member as StringConstantExpressionAst;
			if (stringConstantExpressionAst != null)
			{
				string value = stringConstantExpressionAst.Value;
				return DynamicExpression.Dynamic(PSGetMemberBinder.Get(value, compiler._memberFunctionType, this.MemberExpression.Static), typeof(object), parameterExpression);
			}
			Expression propertyExpr = this.GetPropertyExpr(compiler);
			ParameterExpression parameterExpression2 = Expression.Parameter(propertyExpr.Type);
			temps.Add(parameterExpression2);
			exprs.Add(Expression.Assign(parameterExpression2, compiler.Compile(this.MemberExpression.Member)));
			this.CachedPropertyExpr = parameterExpression2;
			return DynamicExpression.Dynamic(PSGetDynamicMemberBinder.Get(compiler._memberFunctionType, this.MemberExpression.Static), typeof(object), parameterExpression, parameterExpression2);
		}

		// Token: 0x06003D72 RID: 15730 RVA: 0x00141CF0 File Offset: 0x0013FEF0
		public Expression SetValue(Compiler compiler, Expression rhs)
		{
			StringConstantExpressionAst stringConstantExpressionAst = this.MemberExpression.Member as StringConstantExpressionAst;
			if (stringConstantExpressionAst != null)
			{
				string value = stringConstantExpressionAst.Value;
				return DynamicExpression.Dynamic(PSSetMemberBinder.Get(value, compiler._memberFunctionType, this.MemberExpression.Static), typeof(object), this.CachedTarget ?? this.GetTargetExpr(compiler), rhs);
			}
			return DynamicExpression.Dynamic(PSSetDynamicMemberBinder.Get(compiler._memberFunctionType, this.MemberExpression.Static), typeof(object), this.CachedTarget ?? this.GetTargetExpr(compiler), this.CachedPropertyExpr ?? this.GetPropertyExpr(compiler), rhs);
		}
	}
}
