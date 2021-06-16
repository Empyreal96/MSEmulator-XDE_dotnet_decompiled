using System;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000759 RID: 1881
	internal static class Utils
	{
		// Token: 0x06004B0A RID: 19210 RVA: 0x0018912A File Offset: 0x0018732A
		internal static Expression Constant(object value)
		{
			return Expression.Constant(value);
		}

		// Token: 0x06004B0B RID: 19211 RVA: 0x00189132 File Offset: 0x00187332
		public static DefaultExpression Empty()
		{
			return Utils.VoidInstance;
		}

		// Token: 0x06004B0C RID: 19212 RVA: 0x00189139 File Offset: 0x00187339
		public static Expression Void(Expression expression)
		{
			if (expression.Type == typeof(void))
			{
				return expression;
			}
			return Expression.Block(expression, Utils.Empty());
		}

		// Token: 0x06004B0D RID: 19213 RVA: 0x0018915F File Offset: 0x0018735F
		public static DefaultExpression Default(Type type)
		{
			if (type == typeof(void))
			{
				return Utils.Empty();
			}
			return Expression.Default(type);
		}

		// Token: 0x06004B0E RID: 19214 RVA: 0x00189180 File Offset: 0x00187380
		public static Expression Convert(Expression expression, Type type)
		{
			if (expression.Type == type)
			{
				return expression;
			}
			if (expression.Type == typeof(void))
			{
				return Expression.Block(expression, Utils.Default(type));
			}
			if (type == typeof(void))
			{
				return Utils.Void(expression);
			}
			if (type == typeof(object))
			{
				return Utils.Box(expression);
			}
			return Expression.Convert(expression, type);
		}

		// Token: 0x06004B0F RID: 19215 RVA: 0x001891FC File Offset: 0x001873FC
		public static Expression Box(Expression expression)
		{
			MethodInfo method;
			if (expression.Type == typeof(int))
			{
				method = ScriptingRuntimeHelpers.Int32ToObjectMethod;
			}
			else if (expression.Type == typeof(bool))
			{
				method = ScriptingRuntimeHelpers.BooleanToObjectMethod;
			}
			else
			{
				method = null;
			}
			return Expression.Convert(expression, typeof(object), method);
		}

		// Token: 0x06004B10 RID: 19216 RVA: 0x0018925C File Offset: 0x0018745C
		public static bool IsReadWriteAssignment(this ExpressionType type)
		{
			switch (type)
			{
			case ExpressionType.AddAssign:
			case ExpressionType.AndAssign:
			case ExpressionType.DivideAssign:
			case ExpressionType.ExclusiveOrAssign:
			case ExpressionType.LeftShiftAssign:
			case ExpressionType.ModuloAssign:
			case ExpressionType.MultiplyAssign:
			case ExpressionType.OrAssign:
			case ExpressionType.PowerAssign:
			case ExpressionType.RightShiftAssign:
			case ExpressionType.SubtractAssign:
			case ExpressionType.AddAssignChecked:
			case ExpressionType.MultiplyAssignChecked:
			case ExpressionType.SubtractAssignChecked:
			case ExpressionType.PreIncrementAssign:
			case ExpressionType.PreDecrementAssign:
			case ExpressionType.PostIncrementAssign:
			case ExpressionType.PostDecrementAssign:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x04002445 RID: 9285
		private static readonly DefaultExpression VoidInstance = Expression.Empty();
	}
}
