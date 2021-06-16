using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A9A RID: 2714
	internal static class VariantArray
	{
		// Token: 0x06006BAB RID: 27563 RVA: 0x0021C429 File Offset: 0x0021A629
		internal static MemberExpression GetStructField(ParameterExpression variantArray, int field)
		{
			return Expression.Field(variantArray, "Element" + field);
		}

		// Token: 0x06006BAC RID: 27564 RVA: 0x0021C444 File Offset: 0x0021A644
		internal static Type GetStructType(int args)
		{
			if (args <= 1)
			{
				return typeof(VariantArray1);
			}
			if (args <= 2)
			{
				return typeof(VariantArray2);
			}
			if (args <= 4)
			{
				return typeof(VariantArray4);
			}
			if (args <= 8)
			{
				return typeof(VariantArray8);
			}
			int num = 1;
			while (args > num)
			{
				num *= 2;
			}
			Type result;
			lock (VariantArray._generatedTypes)
			{
				foreach (Type type in VariantArray._generatedTypes)
				{
					int num2 = int.Parse(type.Name.Substring("VariantArray".Length), CultureInfo.InvariantCulture);
					if (num == num2)
					{
						return type;
					}
				}
				Type type2 = VariantArray.CreateCustomType(num).MakeGenericType(new Type[]
				{
					typeof(Variant)
				});
				VariantArray._generatedTypes.Add(type2);
				result = type2;
			}
			return result;
		}

		// Token: 0x06006BAD RID: 27565 RVA: 0x0021C564 File Offset: 0x0021A764
		private static Type CreateCustomType(int size)
		{
			TypeAttributes attr = TypeAttributes.SequentialLayout;
			TypeBuilder typeBuilder = UnsafeMethods.DynamicModule.DefineType("VariantArray" + size, attr, typeof(ValueType));
			GenericTypeParameterBuilder type = typeBuilder.DefineGenericParameters(new string[]
			{
				"T"
			})[0];
			for (int i = 0; i < size; i++)
			{
				typeBuilder.DefineField("Element" + i, type, FieldAttributes.Public);
			}
			return typeBuilder.CreateType();
		}

		// Token: 0x04003372 RID: 13170
		private static readonly List<Type> _generatedTypes = new List<Type>(0);
	}
}
