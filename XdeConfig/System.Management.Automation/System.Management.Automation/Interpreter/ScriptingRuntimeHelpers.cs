using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200074F RID: 1871
	internal class ScriptingRuntimeHelpers
	{
		// Token: 0x06004AD7 RID: 19159 RVA: 0x001886CD File Offset: 0x001868CD
		internal static object Int32ToObject(int i)
		{
			return i;
		}

		// Token: 0x06004AD8 RID: 19160 RVA: 0x001886D5 File Offset: 0x001868D5
		internal static object BooleanToObject(bool b)
		{
			if (!b)
			{
				return ScriptingRuntimeHelpers.False;
			}
			return ScriptingRuntimeHelpers.True;
		}

		// Token: 0x06004AD9 RID: 19161 RVA: 0x001886E8 File Offset: 0x001868E8
		internal static object GetPrimitiveDefaultValue(Type type)
		{
			switch (type.GetTypeCode())
			{
			case TypeCode.Boolean:
				return ScriptingRuntimeHelpers.False;
			case TypeCode.Char:
				return '\0';
			case TypeCode.SByte:
				return 0;
			case TypeCode.Byte:
				return 0;
			case TypeCode.Int16:
				return 0;
			case TypeCode.UInt16:
				return 0;
			case TypeCode.Int32:
				return ScriptingRuntimeHelpers.Int32ToObject(0);
			case TypeCode.UInt32:
				return 0U;
			case TypeCode.Int64:
				return 0L;
			case TypeCode.UInt64:
				return 0UL;
			case TypeCode.Single:
				return 0f;
			case TypeCode.Double:
				return 0.0;
			case TypeCode.Decimal:
				return 0m;
			case TypeCode.DateTime:
				return default(DateTime);
			default:
				return null;
			}
		}

		// Token: 0x04002429 RID: 9257
		internal static readonly MethodInfo BooleanToObjectMethod = typeof(ScriptingRuntimeHelpers).GetMethod("BooleanToObject");

		// Token: 0x0400242A RID: 9258
		internal static readonly MethodInfo Int32ToObjectMethod = typeof(ScriptingRuntimeHelpers).GetMethod("Int32ToObject");

		// Token: 0x0400242B RID: 9259
		internal static object True = true;

		// Token: 0x0400242C RID: 9260
		internal static object False = false;
	}
}
