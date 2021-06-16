using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Management.Automation
{
	// Token: 0x02000877 RID: 2167
	internal static class PSTypeExtensions
	{
		// Token: 0x060052F8 RID: 21240 RVA: 0x001B9CFC File Offset: 0x001B7EFC
		internal static bool HasDefaultCtor(this Type type)
		{
			ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
			return constructor != null && (constructor.IsPublic || constructor.IsFamily || constructor.IsFamilyOrAssembly);
		}

		// Token: 0x060052F9 RID: 21241 RVA: 0x001B9D3D File Offset: 0x001B7F3D
		internal static bool IsNumeric(this Type type)
		{
			return LanguagePrimitives.IsNumeric(LanguagePrimitives.GetTypeCode(type));
		}

		// Token: 0x060052FA RID: 21242 RVA: 0x001B9D4A File Offset: 0x001B7F4A
		internal static bool IsNumericOrPrimitive(this Type type)
		{
			return type.GetTypeInfo().IsPrimitive || LanguagePrimitives.IsNumeric(LanguagePrimitives.GetTypeCode(type));
		}

		// Token: 0x060052FB RID: 21243 RVA: 0x001B9D66 File Offset: 0x001B7F66
		internal static bool IsSafePrimitive(this Type type)
		{
			return type.GetTypeInfo().IsPrimitive && type != typeof(IntPtr) && type != typeof(UIntPtr);
		}

		// Token: 0x060052FC RID: 21244 RVA: 0x001B9D99 File Offset: 0x001B7F99
		internal static bool IsFloating(this Type type)
		{
			return LanguagePrimitives.IsFloating(LanguagePrimitives.GetTypeCode(type));
		}

		// Token: 0x060052FD RID: 21245 RVA: 0x001B9DA6 File Offset: 0x001B7FA6
		internal static bool IsInteger(this Type type)
		{
			return LanguagePrimitives.IsInteger(LanguagePrimitives.GetTypeCode(type));
		}

		// Token: 0x060052FE RID: 21246 RVA: 0x001B9DB3 File Offset: 0x001B7FB3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool IsComObject(this Type type)
		{
			return type.IsCOMObject;
		}

		// Token: 0x060052FF RID: 21247 RVA: 0x001B9DBB File Offset: 0x001B7FBB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static TypeCode GetTypeCode(this Type type)
		{
			return Type.GetTypeCode(type);
		}

		// Token: 0x06005300 RID: 21248 RVA: 0x001B9DD6 File Offset: 0x001B7FD6
		internal static IEnumerable<T> GetCustomAttributes<T>(this Type type, bool inherit) where T : Attribute
		{
			return from attr in type.GetTypeInfo().GetCustomAttributes(typeof(T), inherit)
			where attr is T
			select (T)((object)attr);
		}

		// Token: 0x04002AB4 RID: 10932
		internal static Type[] EmptyTypes = new Type[0];

		// Token: 0x04002AB5 RID: 10933
		private static readonly Type ComObjectType = Type.GetType("System.__ComObject");
	}
}
