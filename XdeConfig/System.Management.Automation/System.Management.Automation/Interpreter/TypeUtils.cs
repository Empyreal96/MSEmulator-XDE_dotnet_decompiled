using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200074C RID: 1868
	internal static class TypeUtils
	{
		// Token: 0x06004ACD RID: 19149 RVA: 0x00188199 File Offset: 0x00186399
		internal static Type GetNonNullableType(this Type type)
		{
			if (TypeUtils.IsNullableType(type))
			{
				return type.GetGenericArguments()[0];
			}
			return type;
		}

		// Token: 0x06004ACE RID: 19150 RVA: 0x001881B0 File Offset: 0x001863B0
		internal static Type GetNullableType(Type type)
		{
			if (type.GetTypeInfo().IsValueType && !TypeUtils.IsNullableType(type))
			{
				return typeof(Nullable<>).MakeGenericType(new Type[]
				{
					type
				});
			}
			return type;
		}

		// Token: 0x06004ACF RID: 19151 RVA: 0x001881F0 File Offset: 0x001863F0
		internal static bool IsNullableType(Type type)
		{
			TypeInfo typeInfo = type.GetTypeInfo();
			return typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		// Token: 0x06004AD0 RID: 19152 RVA: 0x00188223 File Offset: 0x00186423
		internal static bool IsBool(Type type)
		{
			return type.GetNonNullableType() == typeof(bool);
		}

		// Token: 0x06004AD1 RID: 19153 RVA: 0x0018823C File Offset: 0x0018643C
		internal static bool IsNumeric(Type type)
		{
			type = type.GetNonNullableType();
			if (!type.GetTypeInfo().IsEnum)
			{
				switch (type.GetTypeCode())
				{
				case TypeCode.Char:
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
				case TypeCode.Single:
				case TypeCode.Double:
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004AD2 RID: 19154 RVA: 0x001882A0 File Offset: 0x001864A0
		internal static bool IsNumeric(TypeCode typeCode)
		{
			switch (typeCode)
			{
			case TypeCode.Char:
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Single:
			case TypeCode.Double:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06004AD3 RID: 19155 RVA: 0x001882E8 File Offset: 0x001864E8
		internal static bool IsArithmetic(Type type)
		{
			type = type.GetNonNullableType();
			if (!type.GetTypeInfo().IsEnum)
			{
				switch (type.GetTypeCode())
				{
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
				case TypeCode.Single:
				case TypeCode.Double:
					return true;
				}
			}
			return false;
		}
	}
}
