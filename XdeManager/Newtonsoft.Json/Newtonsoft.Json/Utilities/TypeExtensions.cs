using System;
using System.Collections.Generic;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000068 RID: 104
	internal static class TypeExtensions
	{
		// Token: 0x060005E4 RID: 1508 RVA: 0x00019962 File Offset: 0x00017B62
		public static MethodInfo Method(this Delegate d)
		{
			return d.Method;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001996A File Offset: 0x00017B6A
		public static MemberTypes MemberType(this MemberInfo memberInfo)
		{
			return memberInfo.MemberType;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00019972 File Offset: 0x00017B72
		public static bool ContainsGenericParameters(this Type type)
		{
			return type.ContainsGenericParameters;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0001997A File Offset: 0x00017B7A
		public static bool IsInterface(this Type type)
		{
			return type.IsInterface;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00019982 File Offset: 0x00017B82
		public static bool IsGenericType(this Type type)
		{
			return type.IsGenericType;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0001998A File Offset: 0x00017B8A
		public static bool IsGenericTypeDefinition(this Type type)
		{
			return type.IsGenericTypeDefinition;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00019992 File Offset: 0x00017B92
		public static Type BaseType(this Type type)
		{
			return type.BaseType;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0001999A File Offset: 0x00017B9A
		public static Assembly Assembly(this Type type)
		{
			return type.Assembly;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x000199A2 File Offset: 0x00017BA2
		public static bool IsEnum(this Type type)
		{
			return type.IsEnum;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x000199AA File Offset: 0x00017BAA
		public static bool IsClass(this Type type)
		{
			return type.IsClass;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x000199B2 File Offset: 0x00017BB2
		public static bool IsSealed(this Type type)
		{
			return type.IsSealed;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x000199BA File Offset: 0x00017BBA
		public static bool IsAbstract(this Type type)
		{
			return type.IsAbstract;
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x000199C2 File Offset: 0x00017BC2
		public static bool IsVisible(this Type type)
		{
			return type.IsVisible;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x000199CA File Offset: 0x00017BCA
		public static bool IsValueType(this Type type)
		{
			return type.IsValueType;
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x000199D2 File Offset: 0x00017BD2
		public static bool IsPrimitive(this Type type)
		{
			return type.IsPrimitive;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x000199DC File Offset: 0x00017BDC
		public static bool AssignableToTypeName(this Type type, string fullTypeName, bool searchInterfaces, out Type match)
		{
			Type type2 = type;
			while (type2 != null)
			{
				if (string.Equals(type2.FullName, fullTypeName, StringComparison.Ordinal))
				{
					match = type2;
					return true;
				}
				type2 = type2.BaseType();
			}
			if (searchInterfaces)
			{
				Type[] interfaces = type.GetInterfaces();
				for (int i = 0; i < interfaces.Length; i++)
				{
					if (string.Equals(interfaces[i].Name, fullTypeName, StringComparison.Ordinal))
					{
						match = type;
						return true;
					}
				}
			}
			match = null;
			return false;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00019A44 File Offset: 0x00017C44
		public static bool AssignableToTypeName(this Type type, string fullTypeName, bool searchInterfaces)
		{
			Type type2;
			return type.AssignableToTypeName(fullTypeName, searchInterfaces, out type2);
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00019A5C File Offset: 0x00017C5C
		public static bool ImplementInterface(this Type type, Type interfaceType)
		{
			Type type2 = type;
			while (type2 != null)
			{
				foreach (Type type3 in ((IEnumerable<Type>)type2.GetInterfaces()))
				{
					if (type3 == interfaceType || (type3 != null && type3.ImplementInterface(interfaceType)))
					{
						return true;
					}
				}
				type2 = type2.BaseType();
			}
			return false;
		}
	}
}
