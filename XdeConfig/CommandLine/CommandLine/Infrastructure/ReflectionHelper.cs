using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommandLine.Core;
using CSharpx;

namespace CommandLine.Infrastructure
{
	// Token: 0x02000063 RID: 99
	internal static class ReflectionHelper
	{
		// Token: 0x0600027D RID: 637 RVA: 0x0000A35C File Offset: 0x0000855C
		public static void SetAttributeOverride(IEnumerable<Attribute> overrides)
		{
			if (overrides != null)
			{
				ReflectionHelper._overrides = overrides.ToDictionary((Attribute attr) => attr.GetType(), (Attribute attr) => attr);
				return;
			}
			ReflectionHelper._overrides = null;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000A3BC File Offset: 0x000085BC
		public static Maybe<TAttribute> GetAttribute<TAttribute>() where TAttribute : Attribute
		{
			if (ReflectionHelper._overrides != null)
			{
				if (!ReflectionHelper._overrides.ContainsKey(typeof(TAttribute)))
				{
					return Maybe.Nothing<TAttribute>();
				}
				return Maybe.Just<TAttribute>((TAttribute)((object)ReflectionHelper._overrides[typeof(TAttribute)]));
			}
			else
			{
				TAttribute[] array = ReflectionHelper.GetExecutingOrEntryAssembly().GetCustomAttributes<TAttribute>().ToArray<TAttribute>();
				if (array.Length == 0)
				{
					return Maybe.Nothing<TAttribute>();
				}
				return Maybe.Just<TAttribute>(array[0]);
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000A431 File Offset: 0x00008631
		public static string GetAssemblyName()
		{
			return ReflectionHelper.GetExecutingOrEntryAssembly().GetName().Name;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000A442 File Offset: 0x00008642
		public static string GetAssemblyVersion()
		{
			return ReflectionHelper.GetExecutingOrEntryAssembly().GetName().Version.ToStringInvariant<Version>();
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000A458 File Offset: 0x00008658
		public static bool IsFSharpOptionType(Type type)
		{
			return type.FullName.StartsWith("Microsoft.FSharp.Core.FSharpOption`1", StringComparison.Ordinal);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000A46B File Offset: 0x0000866B
		public static T CreateDefaultImmutableInstance<T>(Type[] constructorTypes)
		{
			return (T)((object)ReflectionHelper.CreateDefaultImmutableInstance(typeof(T), constructorTypes));
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000A484 File Offset: 0x00008684
		public static object CreateDefaultImmutableInstance(Type type, Type[] constructorTypes)
		{
			ConstructorInfo constructor = type.GetTypeInfo().GetConstructor(constructorTypes);
			object[] parameters = (from prms in constructor.GetParameters()
			select prms.ParameterType.CreateDefaultForImmutable()).ToArray<object>();
			return constructor.Invoke(parameters);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000A4D3 File Offset: 0x000086D3
		private static Assembly GetExecutingOrEntryAssembly()
		{
			return Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000A4E4 File Offset: 0x000086E4
		public static IEnumerable<string> GetNamesOfEnum(Type t)
		{
			if (t.IsEnum)
			{
				return Enum.GetNames(t);
			}
			Type underlyingType = Nullable.GetUnderlyingType(t);
			if (underlyingType != null && underlyingType.IsEnum)
			{
				return Enum.GetNames(underlyingType);
			}
			return Enumerable.Empty<string>();
		}

		// Token: 0x040000C3 RID: 195
		[ThreadStatic]
		private static IDictionary<Type, Attribute> _overrides;
	}
}
