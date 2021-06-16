using System;
using System.Reflection;

namespace Microsoft.Reflection
{
	// Token: 0x02000073 RID: 115
	internal static class ReflectionExtensions
	{
		// Token: 0x060002BF RID: 703 RVA: 0x0000F3C5 File Offset: 0x0000D5C5
		public static bool IsEnum(this Type type)
		{
			return type.IsEnum;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000F3CD File Offset: 0x0000D5CD
		public static bool IsAbstract(this Type type)
		{
			return type.IsAbstract;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000F3D5 File Offset: 0x0000D5D5
		public static bool IsSealed(this Type type)
		{
			return type.IsSealed;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000F3DD File Offset: 0x0000D5DD
		public static Type BaseType(this Type type)
		{
			return type.BaseType;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000F3E5 File Offset: 0x0000D5E5
		public static Assembly Assembly(this Type type)
		{
			return type.Assembly;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000F3ED File Offset: 0x0000D5ED
		public static TypeCode GetTypeCode(this Type type)
		{
			return Type.GetTypeCode(type);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000F3F5 File Offset: 0x0000D5F5
		public static bool ReflectionOnly(this Assembly assm)
		{
			return assm.ReflectionOnly;
		}
	}
}
