using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A90 RID: 2704
	internal class VarEnumSelector
	{
		// Token: 0x06006B4D RID: 27469 RVA: 0x0021A8DC File Offset: 0x00218ADC
		internal VarEnumSelector(Type[] explicitArgTypes)
		{
			this._variantBuilders = new VariantBuilder[explicitArgTypes.Length];
			for (int i = 0; i < explicitArgTypes.Length; i++)
			{
				this._variantBuilders[i] = this.GetVariantBuilder(explicitArgTypes[i]);
			}
		}

		// Token: 0x17001DCD RID: 7629
		// (get) Token: 0x06006B4E RID: 27470 RVA: 0x0021A91C File Offset: 0x00218B1C
		internal VariantBuilder[] VariantBuilders
		{
			get
			{
				return this._variantBuilders;
			}
		}

		// Token: 0x06006B4F RID: 27471 RVA: 0x0021A924 File Offset: 0x00218B24
		internal static Type GetTypeForVarEnum(VarEnum vt)
		{
			switch (vt)
			{
			case VarEnum.VT_EMPTY:
			case VarEnum.VT_NULL:
				break;
			default:
				switch (vt)
				{
				case VarEnum.VT_VOID:
					return typeof(void);
				case VarEnum.VT_HRESULT:
					return typeof(int);
				case VarEnum.VT_PTR:
				case VarEnum.VT_USERDEFINED:
					return typeof(object);
				case VarEnum.VT_SAFEARRAY:
				case VarEnum.VT_CARRAY:
					return typeof(Array);
				case VarEnum.VT_LPSTR:
				case VarEnum.VT_LPWSTR:
					return typeof(string);
				case VarEnum.VT_RECORD:
					goto IL_57;
				case (VarEnum)37:
					return typeof(IntPtr);
				case (VarEnum)38:
					return typeof(UIntPtr);
				}
				return VarEnumSelector.GetManagedMarshalType(vt);
			}
			IL_57:
			return typeof(void);
		}

		// Token: 0x06006B50 RID: 27472 RVA: 0x0021A9F8 File Offset: 0x00218BF8
		internal static Type GetManagedMarshalType(VarEnum varEnum)
		{
			if (varEnum == VarEnum.VT_CY)
			{
				return typeof(CurrencyWrapper);
			}
			if (Variant.IsPrimitiveType(varEnum))
			{
				return VarEnumSelector._ComToManagedPrimitiveTypes[varEnum];
			}
			switch (varEnum)
			{
			case VarEnum.VT_EMPTY:
			case VarEnum.VT_NULL:
				break;
			default:
				switch (varEnum)
				{
				case VarEnum.VT_DISPATCH:
				case VarEnum.VT_VARIANT:
				case VarEnum.VT_UNKNOWN:
					goto IL_52;
				case VarEnum.VT_ERROR:
					return typeof(ErrorWrapper);
				}
				throw Error.UnexpectedVarEnum(varEnum);
			}
			IL_52:
			return typeof(object);
		}

		// Token: 0x06006B51 RID: 27473 RVA: 0x0021AA78 File Offset: 0x00218C78
		private static Dictionary<VarEnum, Type> CreateComToManagedPrimitiveTypes()
		{
			Dictionary<VarEnum, Type> dictionary = new Dictionary<VarEnum, Type>();
			dictionary[VarEnum.VT_I1] = typeof(sbyte);
			dictionary[VarEnum.VT_I2] = typeof(short);
			dictionary[VarEnum.VT_I4] = typeof(int);
			dictionary[VarEnum.VT_I8] = typeof(long);
			dictionary[VarEnum.VT_UI1] = typeof(byte);
			dictionary[VarEnum.VT_UI2] = typeof(ushort);
			dictionary[VarEnum.VT_UI4] = typeof(uint);
			dictionary[VarEnum.VT_UI8] = typeof(ulong);
			dictionary[VarEnum.VT_INT] = typeof(int);
			dictionary[VarEnum.VT_UINT] = typeof(uint);
			dictionary[VarEnum.VT_PTR] = typeof(IntPtr);
			dictionary[VarEnum.VT_BOOL] = typeof(bool);
			dictionary[VarEnum.VT_R4] = typeof(float);
			dictionary[VarEnum.VT_R8] = typeof(double);
			dictionary[VarEnum.VT_DECIMAL] = typeof(decimal);
			dictionary[VarEnum.VT_DATE] = typeof(DateTime);
			dictionary[VarEnum.VT_BSTR] = typeof(string);
			dictionary[VarEnum.VT_CLSID] = typeof(Guid);
			dictionary[VarEnum.VT_CY] = typeof(CurrencyWrapper);
			dictionary[VarEnum.VT_ERROR] = typeof(ErrorWrapper);
			return dictionary;
		}

		// Token: 0x06006B52 RID: 27474 RVA: 0x0021ABF0 File Offset: 0x00218DF0
		private static IList<IList<VarEnum>> CreateComPrimitiveTypeFamilies()
		{
			return new VarEnum[][]
			{
				new VarEnum[]
				{
					VarEnum.VT_I8,
					VarEnum.VT_I4,
					VarEnum.VT_I2,
					VarEnum.VT_I1
				},
				new VarEnum[]
				{
					VarEnum.VT_UI8,
					VarEnum.VT_UI4,
					VarEnum.VT_UI2,
					VarEnum.VT_UI1
				},
				new VarEnum[]
				{
					VarEnum.VT_INT
				},
				new VarEnum[]
				{
					VarEnum.VT_UINT
				},
				new VarEnum[]
				{
					VarEnum.VT_BOOL
				},
				new VarEnum[]
				{
					VarEnum.VT_DATE
				},
				new VarEnum[]
				{
					VarEnum.VT_R8,
					VarEnum.VT_R4
				},
				new VarEnum[]
				{
					VarEnum.VT_DECIMAL
				},
				new VarEnum[]
				{
					VarEnum.VT_BSTR
				},
				new VarEnum[]
				{
					VarEnum.VT_CY
				},
				new VarEnum[]
				{
					VarEnum.VT_ERROR
				}
			};
		}

		// Token: 0x06006B53 RID: 27475 RVA: 0x0021ACF4 File Offset: 0x00218EF4
		private static List<VarEnum> GetConversionsToComPrimitiveTypeFamilies(Type argumentType)
		{
			List<VarEnum> list = new List<VarEnum>();
			foreach (IList<VarEnum> list2 in VarEnumSelector._ComPrimitiveTypeFamilies)
			{
				foreach (VarEnum varEnum in list2)
				{
					Type destination = VarEnumSelector._ComToManagedPrimitiveTypes[varEnum];
					if (TypeUtils.IsImplicitlyConvertible(argumentType, destination, true))
					{
						list.Add(varEnum);
						break;
					}
				}
			}
			return list;
		}

		// Token: 0x06006B54 RID: 27476 RVA: 0x0021AD9C File Offset: 0x00218F9C
		private static void CheckForAmbiguousMatch(Type argumentType, List<VarEnum> compatibleComTypes)
		{
			if (compatibleComTypes.Count <= 1)
			{
				return;
			}
			string text = "";
			for (int i = 0; i < compatibleComTypes.Count; i++)
			{
				string name = VarEnumSelector._ComToManagedPrimitiveTypes[compatibleComTypes[i]].Name;
				if (i == compatibleComTypes.Count - 1)
				{
					text += " and ";
				}
				else if (i != 0)
				{
					text += ", ";
				}
				text += name;
			}
			throw Error.AmbiguousConversion(argumentType.Name, text);
		}

		// Token: 0x06006B55 RID: 27477 RVA: 0x0021AE20 File Offset: 0x00219020
		private static bool TryGetPrimitiveComType(Type argumentType, out VarEnum primitiveVarEnum)
		{
			switch (Type.GetTypeCode(argumentType))
			{
			case TypeCode.Boolean:
				primitiveVarEnum = VarEnum.VT_BOOL;
				return true;
			case TypeCode.Char:
				primitiveVarEnum = VarEnum.VT_UI2;
				return true;
			case TypeCode.SByte:
				primitiveVarEnum = VarEnum.VT_I1;
				return true;
			case TypeCode.Byte:
				primitiveVarEnum = VarEnum.VT_UI1;
				return true;
			case TypeCode.Int16:
				primitiveVarEnum = VarEnum.VT_I2;
				return true;
			case TypeCode.UInt16:
				primitiveVarEnum = VarEnum.VT_UI2;
				return true;
			case TypeCode.Int32:
				primitiveVarEnum = VarEnum.VT_I4;
				return true;
			case TypeCode.UInt32:
				primitiveVarEnum = VarEnum.VT_UI4;
				return true;
			case TypeCode.Int64:
				primitiveVarEnum = VarEnum.VT_I8;
				return true;
			case TypeCode.UInt64:
				primitiveVarEnum = VarEnum.VT_UI8;
				return true;
			case TypeCode.Single:
				primitiveVarEnum = VarEnum.VT_R4;
				return true;
			case TypeCode.Double:
				primitiveVarEnum = VarEnum.VT_R8;
				return true;
			case TypeCode.Decimal:
				primitiveVarEnum = VarEnum.VT_DECIMAL;
				return true;
			case TypeCode.DateTime:
				primitiveVarEnum = VarEnum.VT_DATE;
				return true;
			case TypeCode.String:
				primitiveVarEnum = VarEnum.VT_BSTR;
				return true;
			}
			if (argumentType == typeof(CurrencyWrapper))
			{
				primitiveVarEnum = VarEnum.VT_CY;
				return true;
			}
			if (argumentType == typeof(ErrorWrapper))
			{
				primitiveVarEnum = VarEnum.VT_ERROR;
				return true;
			}
			if (argumentType == typeof(IntPtr))
			{
				primitiveVarEnum = VarEnum.VT_PTR;
				return true;
			}
			if (argumentType == typeof(UIntPtr))
			{
				primitiveVarEnum = VarEnum.VT_PTR;
				return true;
			}
			primitiveVarEnum = VarEnum.VT_VOID;
			return false;
		}

		// Token: 0x06006B56 RID: 27478 RVA: 0x0021AF38 File Offset: 0x00219138
		private static bool TryGetPrimitiveComTypeViaConversion(Type argumentType, out VarEnum primitiveVarEnum)
		{
			List<VarEnum> conversionsToComPrimitiveTypeFamilies = VarEnumSelector.GetConversionsToComPrimitiveTypeFamilies(argumentType);
			VarEnumSelector.CheckForAmbiguousMatch(argumentType, conversionsToComPrimitiveTypeFamilies);
			if (conversionsToComPrimitiveTypeFamilies.Count == 1)
			{
				primitiveVarEnum = conversionsToComPrimitiveTypeFamilies[0];
				return true;
			}
			primitiveVarEnum = VarEnum.VT_VOID;
			return false;
		}

		// Token: 0x06006B57 RID: 27479 RVA: 0x0021AF6C File Offset: 0x0021916C
		private VarEnum GetComType(ref Type argumentType)
		{
			if (argumentType == typeof(Missing))
			{
				return VarEnum.VT_RECORD;
			}
			if (argumentType.IsArray)
			{
				return VarEnum.VT_ARRAY;
			}
			if (argumentType == typeof(UnknownWrapper))
			{
				return VarEnum.VT_UNKNOWN;
			}
			if (argumentType == typeof(DispatchWrapper))
			{
				return VarEnum.VT_DISPATCH;
			}
			if (argumentType == typeof(VariantWrapper))
			{
				return VarEnum.VT_VARIANT;
			}
			if (argumentType == typeof(BStrWrapper))
			{
				return VarEnum.VT_BSTR;
			}
			if (argumentType == typeof(ErrorWrapper))
			{
				return VarEnum.VT_ERROR;
			}
			if (argumentType == typeof(CurrencyWrapper))
			{
				return VarEnum.VT_CY;
			}
			if (argumentType.IsEnum)
			{
				argumentType = Enum.GetUnderlyingType(argumentType);
				return this.GetComType(ref argumentType);
			}
			if (argumentType.IsNullableType())
			{
				argumentType = TypeUtils.GetNonNullableType(argumentType);
				return this.GetComType(ref argumentType);
			}
			if (argumentType.IsGenericType)
			{
				return VarEnum.VT_UNKNOWN;
			}
			VarEnum result;
			if (VarEnumSelector.TryGetPrimitiveComType(argumentType, out result))
			{
				return result;
			}
			return VarEnum.VT_RECORD;
		}

		// Token: 0x06006B58 RID: 27480 RVA: 0x0021B070 File Offset: 0x00219270
		private VariantBuilder GetVariantBuilder(Type argumentType)
		{
			if (argumentType == null)
			{
				return new VariantBuilder(VarEnum.VT_EMPTY, new NullArgBuilder());
			}
			if (argumentType == typeof(DBNull))
			{
				return new VariantBuilder(VarEnum.VT_NULL, new NullArgBuilder());
			}
			ArgBuilder builder;
			if (argumentType.IsByRef)
			{
				Type elementType = argumentType.GetElementType();
				VarEnum varEnum;
				if (elementType == typeof(object) || elementType == typeof(DBNull))
				{
					varEnum = VarEnum.VT_VARIANT;
				}
				else
				{
					varEnum = this.GetComType(ref elementType);
				}
				builder = VarEnumSelector.GetSimpleArgBuilder(elementType, varEnum);
				return new VariantBuilder(varEnum | VarEnum.VT_BYREF, builder);
			}
			VarEnum comType = this.GetComType(ref argumentType);
			builder = VarEnumSelector.GetByValArgBuilder(argumentType, ref comType);
			return new VariantBuilder(comType, builder);
		}

		// Token: 0x06006B59 RID: 27481 RVA: 0x0021B120 File Offset: 0x00219320
		private static ArgBuilder GetByValArgBuilder(Type elementType, ref VarEnum elementVarEnum)
		{
			if (elementVarEnum == VarEnum.VT_RECORD)
			{
				VarEnum varEnum;
				if (VarEnumSelector.TryGetPrimitiveComTypeViaConversion(elementType, out varEnum))
				{
					elementVarEnum = varEnum;
					Type managedMarshalType = VarEnumSelector.GetManagedMarshalType(elementVarEnum);
					return new ConversionArgBuilder(elementType, VarEnumSelector.GetSimpleArgBuilder(managedMarshalType, elementVarEnum));
				}
				if (typeof(IConvertible).IsAssignableFrom(elementType))
				{
					return new ConvertibleArgBuilder();
				}
			}
			return VarEnumSelector.GetSimpleArgBuilder(elementType, elementVarEnum);
		}

		// Token: 0x06006B5A RID: 27482 RVA: 0x0021B178 File Offset: 0x00219378
		private static SimpleArgBuilder GetSimpleArgBuilder(Type elementType, VarEnum elementVarEnum)
		{
			switch (elementVarEnum)
			{
			case VarEnum.VT_CY:
				return new CurrencyArgBuilder(elementType);
			case VarEnum.VT_DATE:
				return new DateTimeArgBuilder(elementType);
			case VarEnum.VT_BSTR:
				return new StringArgBuilder(elementType);
			case VarEnum.VT_DISPATCH:
				return new DispatchArgBuilder(elementType);
			case VarEnum.VT_ERROR:
				return new ErrorArgBuilder(elementType);
			case VarEnum.VT_BOOL:
				return new BoolArgBuilder(elementType);
			case VarEnum.VT_VARIANT:
				break;
			case VarEnum.VT_UNKNOWN:
				return new UnknownArgBuilder(elementType);
			default:
				if (elementVarEnum != VarEnum.VT_RECORD && elementVarEnum != VarEnum.VT_ARRAY)
				{
					Type managedMarshalType = VarEnumSelector.GetManagedMarshalType(elementVarEnum);
					if (elementType == managedMarshalType)
					{
						return new SimpleArgBuilder(elementType);
					}
					return new ConvertArgBuilder(elementType, managedMarshalType);
				}
				break;
			}
			return new VariantArgBuilder(elementType);
		}

		// Token: 0x04003340 RID: 13120
		private const VarEnum VT_DEFAULT = VarEnum.VT_RECORD;

		// Token: 0x04003341 RID: 13121
		private readonly VariantBuilder[] _variantBuilders;

		// Token: 0x04003342 RID: 13122
		private static readonly Dictionary<VarEnum, Type> _ComToManagedPrimitiveTypes = VarEnumSelector.CreateComToManagedPrimitiveTypes();

		// Token: 0x04003343 RID: 13123
		private static readonly IList<IList<VarEnum>> _ComPrimitiveTypeFamilies = VarEnumSelector.CreateComPrimitiveTypeFamilies();
	}
}
