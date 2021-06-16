using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace System.Management.Automation.Language
{
	// Token: 0x020005AA RID: 1450
	internal class InvokeMemberAssignableValue : IAssignableValue
	{
		// Token: 0x17000D5C RID: 3420
		// (get) Token: 0x06003D74 RID: 15732 RVA: 0x00141DA1 File Offset: 0x0013FFA1
		// (set) Token: 0x06003D75 RID: 15733 RVA: 0x00141DA9 File Offset: 0x0013FFA9
		internal InvokeMemberExpressionAst InvokeMemberExpressionAst { get; set; }

		// Token: 0x06003D76 RID: 15734 RVA: 0x00141DB2 File Offset: 0x0013FFB2
		private Expression GetTargetExpr(Compiler compiler)
		{
			return this._targetExprTemp ?? compiler.Compile(this.InvokeMemberExpressionAst.Expression);
		}

		// Token: 0x06003D77 RID: 15735 RVA: 0x00141DCF File Offset: 0x0013FFCF
		private Expression GetMemberNameExpr(Compiler compiler)
		{
			return this._memberNameExprTemp ?? compiler.Compile(this.InvokeMemberExpressionAst.Member);
		}

		// Token: 0x06003D78 RID: 15736 RVA: 0x00141DEC File Offset: 0x0013FFEC
		private IEnumerable<Expression> GetArgumentExprs(Compiler compiler)
		{
			if (this._argExprTemps != null)
			{
				return this._argExprTemps;
			}
			if (this.InvokeMemberExpressionAst.Arguments != null)
			{
				return this.InvokeMemberExpressionAst.Arguments.Select(new Func<ExpressionAst, Expression>(compiler.Compile)).ToArray<Expression>();
			}
			return new Expression[0];
		}

		// Token: 0x06003D79 RID: 15737 RVA: 0x00141E54 File Offset: 0x00140054
		public Expression GetValue(Compiler compiler, List<Expression> exprs, List<ParameterExpression> temps)
		{
			PSMethodInvocationConstraints invokeMemberConstraints = Compiler.GetInvokeMemberConstraints(this.InvokeMemberExpressionAst);
			Expression targetExpr = this.GetTargetExpr(compiler);
			this._targetExprTemp = Expression.Variable(targetExpr.Type);
			exprs.Add(Expression.Assign(this._targetExprTemp, targetExpr));
			int count = exprs.Count;
			IEnumerable<Expression> argumentExprs = this.GetArgumentExprs(compiler);
			this._argExprTemps = (from arg in argumentExprs
			select Expression.Variable(arg.Type)).ToArray<ParameterExpression>();
			exprs.AddRange(argumentExprs.Zip(this._argExprTemps, (Expression arg, ParameterExpression temp) => Expression.Assign(temp, arg)));
			temps.Add(this._targetExprTemp);
			int count2 = temps.Count;
			temps.AddRange(this._argExprTemps);
			StringConstantExpressionAst stringConstantExpressionAst = this.InvokeMemberExpressionAst.Member as StringConstantExpressionAst;
			if (stringConstantExpressionAst != null)
			{
				return compiler.InvokeMember(stringConstantExpressionAst.Value, invokeMemberConstraints, this._targetExprTemp, this._argExprTemps, false, false);
			}
			Expression memberNameExpr = this.GetMemberNameExpr(compiler);
			this._memberNameExprTemp = Expression.Variable(memberNameExpr.Type);
			exprs.Insert(count, Expression.Assign(this._memberNameExprTemp, memberNameExpr));
			temps.Insert(count2, this._memberNameExprTemp);
			return compiler.InvokeDynamicMember(this._memberNameExprTemp, invokeMemberConstraints, this._targetExprTemp, this._argExprTemps, false, false);
		}

		// Token: 0x06003D7A RID: 15738 RVA: 0x00141FB0 File Offset: 0x001401B0
		public Expression SetValue(Compiler compiler, Expression rhs)
		{
			PSMethodInvocationConstraints invokeMemberConstraints = Compiler.GetInvokeMemberConstraints(this.InvokeMemberExpressionAst);
			StringConstantExpressionAst stringConstantExpressionAst = this.InvokeMemberExpressionAst.Member as StringConstantExpressionAst;
			Expression targetExpr = this.GetTargetExpr(compiler);
			IEnumerable<Expression> argumentExprs = this.GetArgumentExprs(compiler);
			if (stringConstantExpressionAst != null)
			{
				return compiler.InvokeMember(stringConstantExpressionAst.Value, invokeMemberConstraints, targetExpr, argumentExprs.Append(rhs), false, true);
			}
			Expression memberNameExpr = this.GetMemberNameExpr(compiler);
			return compiler.InvokeDynamicMember(memberNameExpr, invokeMemberConstraints, targetExpr, argumentExprs.Append(rhs), false, true);
		}

		// Token: 0x04001F00 RID: 7936
		private ParameterExpression _targetExprTemp;

		// Token: 0x04001F01 RID: 7937
		private ParameterExpression _memberNameExprTemp;

		// Token: 0x04001F02 RID: 7938
		private IEnumerable<ParameterExpression> _argExprTemps;
	}
}
