using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CommandLine.Infrastructure;
using CommandLine.Text;
using CSharpx;

namespace CommandLine.Core
{
	// Token: 0x02000071 RID: 113
	internal static class ReflectionExtensions
	{
		// Token: 0x060002BD RID: 701 RVA: 0x0000B1D4 File Offset: 0x000093D4
		public static IEnumerable<T> GetSpecifications<T>(this Type type, Func<PropertyInfo, T> selector)
		{
			return from pi in type.FlattenHierarchy().SelectMany((Type x) => x.GetTypeInfo().GetProperties())
			let attrs = pi.GetCustomAttributes(true)
			where attrs.OfType<OptionAttribute>().Any<OptionAttribute>() || attrs.OfType<ValueAttribute>().Any<ValueAttribute>()
			group pi by pi.Name into g
			select selector(g.First<PropertyInfo>());
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000B2B4 File Offset: 0x000094B4
		public static Maybe<VerbAttribute> GetVerbSpecification(this Type type)
		{
			return (from attr in type.FlattenHierarchy().SelectMany((Type x) => x.GetTypeInfo().GetCustomAttributes(typeof(VerbAttribute), true))
			let vattr = (VerbAttribute)attr
			select vattr).SingleOrDefault<VerbAttribute>().ToMaybe<VerbAttribute>();
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000B340 File Offset: 0x00009540
		public static Maybe<Tuple<PropertyInfo, UsageAttribute>> GetUsageData(this Type type)
		{
			return (from pi in type.FlattenHierarchy().SelectMany((Type x) => x.GetTypeInfo().GetProperties())
			let attrs = pi.GetCustomAttributes(true)
			where attrs.OfType<UsageAttribute>().Any<UsageAttribute>()
			select Tuple.Create<PropertyInfo, UsageAttribute>(pi, (UsageAttribute)attrs.First<object>())).SingleOrDefault<Tuple<PropertyInfo, UsageAttribute>>().ToMaybe<Tuple<PropertyInfo, UsageAttribute>>();
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000B3ED File Offset: 0x000095ED
		private static IEnumerable<Type> FlattenHierarchy(this Type type)
		{
			if (type == null)
			{
				yield break;
			}
			yield return type;
			foreach (Type type2 in type.SafeGetInterfaces())
			{
				yield return type2;
			}
			IEnumerator<Type> enumerator = null;
			foreach (Type type3 in type.GetTypeInfo().BaseType.FlattenHierarchy())
			{
				yield return type3;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000B400 File Offset: 0x00009600
		private static IEnumerable<Type> SafeGetInterfaces(this Type type)
		{
			if (!(type == null))
			{
				return type.GetTypeInfo().GetInterfaces();
			}
			return Enumerable.Empty<Type>();
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000B42C File Offset: 0x0000962C
		public static TargetType ToTargetType(this Type type)
		{
			if (type == typeof(bool))
			{
				return TargetType.Switch;
			}
			if (type == typeof(string))
			{
				return TargetType.Scalar;
			}
			if (!type.IsArray && !typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(type))
			{
				return TargetType.Scalar;
			}
			return TargetType.Sequence;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000B484 File Offset: 0x00009684
		public static IEnumerable<Error> SetProperties<T>(this T instance, IEnumerable<SpecificationProperty> specProps, Func<SpecificationProperty, bool> predicate, Func<SpecificationProperty, object> selector)
		{
			return specProps.Where(predicate).SelectMany((SpecificationProperty specProp) => specProp.SetValue(instance, selector(specProp)));
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000B4C0 File Offset: 0x000096C0
		private static IEnumerable<Error> SetValue<T>(this SpecificationProperty specProp, T instance, object value)
		{
			IEnumerable<Error> result;
			try
			{
				specProp.Property.SetValue(instance, value, null);
				result = Enumerable.Empty<Error>();
			}
			catch (TargetInvocationException ex)
			{
				result = new SetValueExceptionError[]
				{
					new SetValueExceptionError(specProp.Specification.FromSpecification(), ex.InnerException, value)
				};
			}
			catch (ArgumentException innerException)
			{
				ArgumentException exception = new ArgumentException("Check if Option or Value attribute values are set properly for the given type.", innerException);
				result = new SetValueExceptionError[]
				{
					new SetValueExceptionError(specProp.Specification.FromSpecification(), exception, value)
				};
			}
			catch (Exception exception2)
			{
				result = new SetValueExceptionError[]
				{
					new SetValueExceptionError(specProp.Specification.FromSpecification(), exception2, value)
				};
			}
			return result;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000B584 File Offset: 0x00009784
		public static object CreateEmptyArray(this Type type)
		{
			return Array.CreateInstance(type, 0);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000B58D File Offset: 0x0000978D
		public static object GetDefaultValue(this Type type)
		{
			return Expression.Lambda<Func<object>>(Expression.Convert(Expression.Default(type), typeof(object)), Array.Empty<ParameterExpression>()).Compile()();
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000B5B8 File Offset: 0x000097B8
		public static bool IsMutable(this Type type)
		{
			if (type == typeof(object))
			{
				return true;
			}
			bool flag = type.GetTypeInfo().GetProperties(BindingFlags.Instance | BindingFlags.Public).Any((PropertyInfo p) => p.CanWrite);
			bool flag2 = type.GetTypeInfo().GetFields(BindingFlags.Instance | BindingFlags.Public).Any<FieldInfo>();
			return flag || flag2;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000B620 File Offset: 0x00009820
		public static object CreateDefaultForImmutable(this Type type)
		{
			if (type.GetTypeInfo().IsGenericType && type.GetTypeInfo().GetGenericTypeDefinition() == typeof(IEnumerable<>))
			{
				return type.GetTypeInfo().GetGenericArguments()[0].CreateEmptyArray();
			}
			return type.GetDefaultValue();
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000B670 File Offset: 0x00009870
		public static object AutoDefault(this Type type)
		{
			if (type.IsMutable())
			{
				return Activator.CreateInstance(type);
			}
			Type[] constructorTypes = type.GetSpecifications((PropertyInfo pi) => pi.PropertyType).ToArray<Type>();
			return ReflectionHelper.CreateDefaultImmutableInstance(type, constructorTypes);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000B6BE File Offset: 0x000098BE
		public static TypeInfo ToTypeInfo(this Type type)
		{
			return TypeInfo.Create(type);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000B6C6 File Offset: 0x000098C6
		public static object StaticMethod(this Type type, string name, params object[] args)
		{
			return type.GetTypeInfo().InvokeMember(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, null, null, args);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000B6DC File Offset: 0x000098DC
		public static object StaticProperty(this Type type, string name)
		{
			return type.GetTypeInfo().InvokeMember(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty, null, null, new object[0]);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000B6F7 File Offset: 0x000098F7
		public static object InstanceProperty(this Type type, string name, object target)
		{
			return type.GetTypeInfo().InvokeMember(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty, null, target, new object[0]);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000B714 File Offset: 0x00009914
		public static bool IsPrimitiveEx(this Type type)
		{
			return (type.GetTypeInfo().IsValueType && type != typeof(Guid)) || type.GetTypeInfo().IsPrimitive || new Type[]
			{
				typeof(string),
				typeof(decimal),
				typeof(DateTime),
				typeof(DateTimeOffset),
				typeof(TimeSpan)
			}.Contains(type) || Convert.GetTypeCode(type) != TypeCode.Object;
		}
	}
}
