using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A8E RID: 2702
	internal static class TypeUtils
	{
		// Token: 0x06006B3D RID: 27453 RVA: 0x0021A2D0 File Offset: 0x002184D0
		internal static Type GetNonNullableType(Type type)
		{
			if (type.IsNullableType())
			{
				return type.GetGenericArguments()[0];
			}
			return type;
		}

		// Token: 0x06006B3E RID: 27454 RVA: 0x0021A2E4 File Offset: 0x002184E4
		internal static bool IsNullableType(this Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		// Token: 0x06006B3F RID: 27455 RVA: 0x0021A305 File Offset: 0x00218505
		internal static bool AreReferenceAssignable(Type dest, Type src)
		{
			return dest == src || (!dest.IsValueType && !src.IsValueType && TypeUtils.AreAssignable(dest, src));
		}

		// Token: 0x06006B40 RID: 27456 RVA: 0x0021A330 File Offset: 0x00218530
		internal static bool AreAssignable(Type dest, Type src)
		{
			return dest == src || dest.IsAssignableFrom(src) || (dest.IsArray && src.IsArray && dest.GetArrayRank() == src.GetArrayRank() && TypeUtils.AreReferenceAssignable(dest.GetElementType(), src.GetElementType())) || (src.IsArray && dest.IsGenericType && (dest.GetGenericTypeDefinition() == typeof(IEnumerable<>) || dest.GetGenericTypeDefinition() == typeof(IList<>) || dest.GetGenericTypeDefinition() == typeof(ICollection<>)) && dest.GetGenericArguments()[0] == src.GetElementType());
		}

		// Token: 0x06006B41 RID: 27457 RVA: 0x0021A3F3 File Offset: 0x002185F3
		internal static bool IsImplicitlyConvertible(Type source, Type destination)
		{
			return TypeUtils.IsIdentityConversion(source, destination) || TypeUtils.IsImplicitNumericConversion(source, destination) || TypeUtils.IsImplicitReferenceConversion(source, destination) || TypeUtils.IsImplicitBoxingConversion(source, destination);
		}

		// Token: 0x06006B42 RID: 27458 RVA: 0x0021A419 File Offset: 0x00218619
		internal static bool IsImplicitlyConvertible(Type source, Type destination, bool considerUserDefined)
		{
			return TypeUtils.IsImplicitlyConvertible(source, destination) || (considerUserDefined && TypeUtils.GetUserDefinedCoercionMethod(source, destination, true) != null);
		}

		// Token: 0x06006B43 RID: 27459 RVA: 0x0021A43C File Offset: 0x0021863C
		internal static MethodInfo GetUserDefinedCoercionMethod(Type convertFrom, Type convertToType, bool implicitOnly)
		{
			Type nonNullableType = TypeUtils.GetNonNullableType(convertFrom);
			Type nonNullableType2 = TypeUtils.GetNonNullableType(convertToType);
			MethodInfo[] methods = nonNullableType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			MethodInfo methodInfo = TypeUtils.FindConversionOperator(methods, convertFrom, convertToType, implicitOnly);
			if (methodInfo != null)
			{
				return methodInfo;
			}
			MethodInfo[] methods2 = nonNullableType2.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			methodInfo = TypeUtils.FindConversionOperator(methods2, convertFrom, convertToType, implicitOnly);
			if (methodInfo != null)
			{
				return methodInfo;
			}
			if (nonNullableType != convertFrom || nonNullableType2 != convertToType)
			{
				methodInfo = TypeUtils.FindConversionOperator(methods, nonNullableType, nonNullableType2, implicitOnly);
				if (methodInfo == null)
				{
					methodInfo = TypeUtils.FindConversionOperator(methods2, nonNullableType, nonNullableType2, implicitOnly);
				}
				if (methodInfo != null)
				{
					return methodInfo;
				}
			}
			return null;
		}

		// Token: 0x06006B44 RID: 27460 RVA: 0x0021A4D4 File Offset: 0x002186D4
		internal static MethodInfo FindConversionOperator(MethodInfo[] methods, Type typeFrom, Type typeTo, bool implicitOnly)
		{
			foreach (MethodInfo methodInfo in methods)
			{
				if ((!(methodInfo.Name != "op_Implicit") || (!implicitOnly && !(methodInfo.Name != "op_Explicit"))) && !(methodInfo.ReturnType != typeTo))
				{
					ParameterInfo[] parameters = methodInfo.GetParameters();
					if (!(parameters[0].ParameterType != typeFrom))
					{
						return methodInfo;
					}
				}
			}
			return null;
		}

		// Token: 0x06006B45 RID: 27461 RVA: 0x0021A54D File Offset: 0x0021874D
		private static bool IsIdentityConversion(Type source, Type destination)
		{
			return source == destination;
		}

		// Token: 0x06006B46 RID: 27462 RVA: 0x0021A558 File Offset: 0x00218758
		private static bool IsImplicitNumericConversion(Type source, Type destination)
		{
			TypeCode typeCode = Type.GetTypeCode(source);
			TypeCode typeCode2 = Type.GetTypeCode(destination);
			switch (typeCode)
			{
			case TypeCode.Char:
				switch (typeCode2)
				{
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return true;
				default:
					return false;
				}
				break;
			case TypeCode.SByte:
				switch (typeCode2)
				{
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return true;
				}
				return false;
			case TypeCode.Byte:
				switch (typeCode2)
				{
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return true;
				default:
					return false;
				}
				break;
			case TypeCode.Int16:
				switch (typeCode2)
				{
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return true;
				}
				return false;
			case TypeCode.UInt16:
				switch (typeCode2)
				{
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return true;
				default:
					return false;
				}
				break;
			case TypeCode.Int32:
				switch (typeCode2)
				{
				case TypeCode.Int64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return true;
				}
				return false;
			case TypeCode.UInt32:
				switch (typeCode2)
				{
				case TypeCode.UInt32:
				case TypeCode.UInt64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return true;
				}
				return false;
			case TypeCode.Int64:
			case TypeCode.UInt64:
				switch (typeCode2)
				{
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return true;
				default:
					return false;
				}
				break;
			case TypeCode.Single:
				return typeCode2 == TypeCode.Double;
			default:
				return false;
			}
		}

		// Token: 0x06006B47 RID: 27463 RVA: 0x0021A71C File Offset: 0x0021891C
		private static bool IsImplicitReferenceConversion(Type source, Type destination)
		{
			return TypeUtils.AreAssignable(destination, source);
		}

		// Token: 0x06006B48 RID: 27464 RVA: 0x0021A728 File Offset: 0x00218928
		private static bool IsImplicitBoxingConversion(Type source, Type destination)
		{
			return (source.IsValueType && (destination == typeof(object) || destination == typeof(ValueType))) || (source.IsEnum && destination == typeof(Enum));
		}

		// Token: 0x0400333D RID: 13117
		private const BindingFlags AnyStatic = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		// Token: 0x0400333E RID: 13118
		internal const MethodAttributes PublicStatic = MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static;
	}
}
