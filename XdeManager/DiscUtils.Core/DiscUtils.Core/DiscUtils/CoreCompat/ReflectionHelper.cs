using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DiscUtils.CoreCompat
{
	// Token: 0x02000079 RID: 121
	internal static class ReflectionHelper
	{
		// Token: 0x06000456 RID: 1110 RVA: 0x0000CE8D File Offset: 0x0000B08D
		public static bool IsEnum(Type type)
		{
			return type.IsEnum;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0000CE95 File Offset: 0x0000B095
		public static Attribute GetCustomAttribute(PropertyInfo property, Type attributeType)
		{
			return Attribute.GetCustomAttribute(property, attributeType);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0000CE9E File Offset: 0x0000B09E
		public static Attribute GetCustomAttribute(PropertyInfo property, Type attributeType, bool inherit)
		{
			return Attribute.GetCustomAttribute(property, attributeType, inherit);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0000CEA8 File Offset: 0x0000B0A8
		public static Attribute GetCustomAttribute(FieldInfo field, Type attributeType)
		{
			return Attribute.GetCustomAttribute(field, attributeType);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0000CEB1 File Offset: 0x0000B0B1
		public static Attribute GetCustomAttribute(Type type, Type attributeType)
		{
			return Attribute.GetCustomAttribute(type, attributeType);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0000CEBA File Offset: 0x0000B0BA
		public static Attribute GetCustomAttribute(Type type, Type attributeType, bool inherit)
		{
			return Attribute.GetCustomAttribute(type, attributeType);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0000CEC3 File Offset: 0x0000B0C3
		public static IEnumerable<Attribute> GetCustomAttributes(Type type, Type attributeType, bool inherit)
		{
			return Attribute.GetCustomAttributes(type, attributeType);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0000CECC File Offset: 0x0000B0CC
		public static Assembly GetAssembly(Type type)
		{
			return type.Assembly;
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0000CED4 File Offset: 0x0000B0D4
		public static int SizeOf<T>()
		{
			return Marshal.SizeOf(typeof(T));
		}
	}
}
