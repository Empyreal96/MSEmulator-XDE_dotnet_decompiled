using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;
using System.Management.Automation.Language;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A55 RID: 2645
	internal static class ComBinderHelpers
	{
		// Token: 0x060069BE RID: 27070 RVA: 0x00214624 File Offset: 0x00212824
		internal static bool PreferPut(Type type, bool holdsNull)
		{
			return type.IsValueType || type.IsArray || (type == typeof(string) || type == typeof(DBNull) || holdsNull || type == typeof(Missing) || type == typeof(CurrencyWrapper));
		}

		// Token: 0x060069BF RID: 27071 RVA: 0x00214694 File Offset: 0x00212894
		internal static bool IsByRef(DynamicMetaObject mo)
		{
			ParameterExpression parameterExpression = mo.Expression as ParameterExpression;
			return parameterExpression != null && parameterExpression.IsByRef;
		}

		// Token: 0x060069C0 RID: 27072 RVA: 0x002146B8 File Offset: 0x002128B8
		internal static bool IsPSReferenceArg(DynamicMetaObject o)
		{
			Type limitType = o.LimitType;
			return limitType.IsGenericType && limitType.GetGenericTypeDefinition() == typeof(PSReference<>);
		}

		// Token: 0x060069C1 RID: 27073 RVA: 0x002146EC File Offset: 0x002128EC
		internal static bool[] ProcessArgumentsForCom(ComMethodDesc method, ref DynamicMetaObject[] args, List<ParameterExpression> temps, List<Expression> initTemps)
		{
			DynamicMetaObject[] array = new DynamicMetaObject[args.Length];
			bool[] array2 = new bool[args.Length];
			for (int i = 0; i < args.Length; i++)
			{
				DynamicMetaObject dynamicMetaObject = args[i];
				if (ComBinderHelpers.IsByRef(dynamicMetaObject))
				{
					array[i] = dynamicMetaObject;
					array2[i] = true;
				}
				else if (ComBinderHelpers.IsPSReferenceArg(dynamicMetaObject))
				{
					BindingRestrictions restrictions = dynamicMetaObject.Restrictions.Merge(ComBinderHelpers.GetTypeRestrictionForDynamicMetaObject(dynamicMetaObject));
					Expression expression = Expression.Property(Helpers.Convert(dynamicMetaObject.Expression, dynamicMetaObject.LimitType), dynamicMetaObject.LimitType.GetProperty("Value"));
					PSReference psreference = dynamicMetaObject.Value as PSReference;
					object value = (psreference != null) ? psreference.Value : null;
					array[i] = new DynamicMetaObject(expression, restrictions, value);
					array2[i] = true;
				}
				else
				{
					if (method.ParameterInformation != null && i < method.ParameterInformation.Length)
					{
						array[i] = new DynamicMetaObject(dynamicMetaObject.CastOrConvertMethodArgument(method.ParameterInformation[i].parameterType, i.ToString(CultureInfo.InvariantCulture), method.Name, temps, initTemps), dynamicMetaObject.Restrictions, dynamicMetaObject.Value);
					}
					else
					{
						array[i] = dynamicMetaObject;
					}
					array2[i] = false;
				}
			}
			args = array;
			return array2;
		}

		// Token: 0x060069C2 RID: 27074 RVA: 0x0021480C File Offset: 0x00212A0C
		internal static BindingRestrictions GetTypeRestrictionForDynamicMetaObject(DynamicMetaObject obj)
		{
			if (obj.Value == null && obj.HasValue)
			{
				return BindingRestrictions.GetInstanceRestriction(obj.Expression, null);
			}
			return BindingRestrictions.GetTypeRestriction(obj.Expression, obj.LimitType);
		}
	}
}
