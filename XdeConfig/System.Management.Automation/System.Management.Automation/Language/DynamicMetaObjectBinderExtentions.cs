using System;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Management.Automation.Language
{
	// Token: 0x02000603 RID: 1539
	internal static class DynamicMetaObjectBinderExtentions
	{
		// Token: 0x06004333 RID: 17203 RVA: 0x00161790 File Offset: 0x0015F990
		internal static DynamicMetaObject DeferForPSObject(this DynamicMetaObjectBinder binder, params DynamicMetaObject[] args)
		{
			Expression[] array = new Expression[args.Length];
			BindingRestrictions bindingRestrictions = BindingRestrictions.Empty;
			for (int i = 0; i < args.Length; i++)
			{
				object obj = PSObject.Base(args[i].Value);
				if (obj != args[i].Value)
				{
					array[i] = Expression.Call(CachedReflectionInfo.PSObject_Base, args[i].Expression.Cast(typeof(object)));
					bindingRestrictions = bindingRestrictions.Merge(args[i].GetSimpleTypeRestriction()).Merge(BindingRestrictions.GetExpressionRestriction(Expression.NotEqual(array[i], args[i].Expression)));
				}
				else
				{
					array[i] = args[i].Expression;
					bindingRestrictions = bindingRestrictions.Merge(args[i].PSGetTypeRestriction());
				}
			}
			return new DynamicMetaObject(DynamicExpression.Dynamic(binder, binder.ReturnType, array), bindingRestrictions);
		}

		// Token: 0x06004334 RID: 17204 RVA: 0x001618AC File Offset: 0x0015FAAC
		internal static DynamicMetaObject UpdateComRestrictionsForPsObject(this DynamicMetaObject binder, DynamicMetaObject[] args)
		{
			BindingRestrictions bindingRestrictions = binder.Restrictions;
			bindingRestrictions = args.Aggregate(bindingRestrictions, delegate(BindingRestrictions current, DynamicMetaObject arg)
			{
				if (arg.LimitType.GetTypeInfo().IsValueType)
				{
					return current.Merge(arg.GetSimpleTypeRestriction());
				}
				return current.Merge(BindingRestrictions.GetExpressionRestriction(Expression.Equal(Expression.Call(CachedReflectionInfo.PSObject_Base, arg.Expression), arg.Expression)));
			});
			return new DynamicMetaObject(binder.Expression, bindingRestrictions);
		}
	}
}
