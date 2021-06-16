using System;
using System.Linq.Expressions;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200004E RID: 78
	internal class NoThrowExpressionVisitor : ExpressionVisitor
	{
		// Token: 0x060004FC RID: 1276 RVA: 0x00015763 File Offset: 0x00013963
		protected override Expression VisitConditional(ConditionalExpression node)
		{
			if (node.IfFalse.NodeType == ExpressionType.Throw)
			{
				return Expression.Condition(node.Test, node.IfTrue, Expression.Constant(NoThrowExpressionVisitor.ErrorResult));
			}
			return base.VisitConditional(node);
		}

		// Token: 0x040001BC RID: 444
		internal static readonly object ErrorResult = new object();
	}
}
