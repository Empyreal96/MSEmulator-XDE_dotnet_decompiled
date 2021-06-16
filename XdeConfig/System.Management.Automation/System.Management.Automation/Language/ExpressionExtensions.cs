using System;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Management.Automation.Language
{
	// Token: 0x020005A1 RID: 1441
	internal static class ExpressionExtensions
	{
		// Token: 0x06003C9F RID: 15519 RVA: 0x0013A23C File Offset: 0x0013843C
		internal static Expression Convert(this Expression expr, Type type)
		{
			if (expr.Type == type)
			{
				return expr;
			}
			if (expr.Type == typeof(void))
			{
				expr = ExpressionCache.NullConstant;
			}
			ConversionRank conversionRank = LanguagePrimitives.GetConversionRank(expr.Type, type);
			if (conversionRank == ConversionRank.Assignable)
			{
				return Expression.Convert(expr, type);
			}
			if (type.GetTypeInfo().ContainsGenericParameters)
			{
				return Expression.Call(CachedReflectionInfo.LanguagePrimitives_ThrowInvalidCastException, expr.Cast(typeof(object)), Expression.Constant(type, typeof(Type)));
			}
			return DynamicExpression.Dynamic(PSConvertBinder.Get(type), type, expr);
		}

		// Token: 0x06003CA0 RID: 15520 RVA: 0x0013A2DC File Offset: 0x001384DC
		internal static Expression Cast(this Expression expr, Type type)
		{
			if (expr.Type == type)
			{
				return expr;
			}
			if ((expr.Type.IsFloating() || expr.Type == typeof(decimal)) && type.GetTypeInfo().IsPrimitive)
			{
				expr = Expression.Call(CachedReflectionInfo.Convert_ChangeType, Expression.Convert(expr, typeof(object)), Expression.Constant(type, typeof(Type)));
			}
			return Expression.Convert(expr, type);
		}
	}
}
