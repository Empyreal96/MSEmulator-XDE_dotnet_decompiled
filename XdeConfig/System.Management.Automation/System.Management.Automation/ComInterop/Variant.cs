using System;
using System.Globalization;
using System.Management.Automation.Interpreter;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A91 RID: 2705
	[StructLayout(LayoutKind.Explicit)]
	internal struct Variant
	{
		// Token: 0x06006B5C RID: 27484 RVA: 0x0021B240 File Offset: 0x00219440
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "Variant ({0})", new object[]
			{
				this.VariantType
			});
		}

		// Token: 0x06006B5D RID: 27485 RVA: 0x0021B274 File Offset: 0x00219474
		internal static bool IsPrimitiveType(VarEnum varEnum)
		{
			switch (varEnum)
			{
			case VarEnum.VT_I2:
			case VarEnum.VT_I4:
			case VarEnum.VT_R4:
			case VarEnum.VT_R8:
			case VarEnum.VT_CY:
			case VarEnum.VT_DATE:
			case VarEnum.VT_BSTR:
			case VarEnum.VT_ERROR:
			case VarEnum.VT_BOOL:
			case VarEnum.VT_DECIMAL:
			case VarEnum.VT_I1:
			case VarEnum.VT_UI1:
			case VarEnum.VT_UI2:
			case VarEnum.VT_UI4:
			case VarEnum.VT_I8:
			case VarEnum.VT_UI8:
			case VarEnum.VT_INT:
			case VarEnum.VT_UINT:
				return true;
			}
			return false;
		}

		// Token: 0x06006B5E RID: 27486 RVA: 0x0021B2E8 File Offset: 0x002194E8
		public object ToObject()
		{
			if (this.IsEmpty)
			{
				return null;
			}
			switch (this.VariantType)
			{
			case VarEnum.VT_NULL:
				return DBNull.Value;
			case VarEnum.VT_I2:
				return this.AsI2;
			case VarEnum.VT_I4:
				return this.AsI4;
			case VarEnum.VT_R4:
				return this.AsR4;
			case VarEnum.VT_R8:
				return this.AsR8;
			case VarEnum.VT_CY:
				return this.AsCy;
			case VarEnum.VT_DATE:
				return this.AsDate;
			case VarEnum.VT_BSTR:
				return this.AsBstr;
			case VarEnum.VT_DISPATCH:
				return this.AsDispatch;
			case VarEnum.VT_ERROR:
				return this.AsError;
			case VarEnum.VT_BOOL:
				return this.AsBool;
			case VarEnum.VT_VARIANT:
				return this.AsVariant;
			case VarEnum.VT_UNKNOWN:
				return this.AsUnknown;
			case VarEnum.VT_DECIMAL:
				return this.AsDecimal;
			case VarEnum.VT_I1:
				return this.AsI1;
			case VarEnum.VT_UI1:
				return this.AsUi1;
			case VarEnum.VT_UI2:
				return this.AsUi2;
			case VarEnum.VT_UI4:
				return this.AsUi4;
			case VarEnum.VT_I8:
				return this.AsI8;
			case VarEnum.VT_UI8:
				return this.AsUi8;
			case VarEnum.VT_INT:
				return this.AsInt;
			case VarEnum.VT_UINT:
				return this.AsUint;
			}
			return this.AsVariant;
		}

		// Token: 0x06006B5F RID: 27487 RVA: 0x0021B464 File Offset: 0x00219664
		public void Clear()
		{
			VarEnum variantType = this.VariantType;
			if ((variantType & VarEnum.VT_BYREF) != VarEnum.VT_EMPTY)
			{
				this.VariantType = VarEnum.VT_EMPTY;
				return;
			}
			if ((variantType & VarEnum.VT_ARRAY) != VarEnum.VT_EMPTY || variantType == VarEnum.VT_BSTR || variantType == VarEnum.VT_UNKNOWN || variantType == VarEnum.VT_DISPATCH || variantType == VarEnum.VT_RECORD)
			{
				IntPtr variant = UnsafeMethods.ConvertVariantByrefToPtr(ref this);
				NativeMethods.VariantClear(variant);
				return;
			}
			this.VariantType = VarEnum.VT_EMPTY;
		}

		// Token: 0x17001DCE RID: 7630
		// (get) Token: 0x06006B60 RID: 27488 RVA: 0x0021B4BA File Offset: 0x002196BA
		// (set) Token: 0x06006B61 RID: 27489 RVA: 0x0021B4C7 File Offset: 0x002196C7
		public VarEnum VariantType
		{
			get
			{
				return (VarEnum)this._typeUnion._vt;
			}
			set
			{
				this._typeUnion._vt = (ushort)value;
			}
		}

		// Token: 0x17001DCF RID: 7631
		// (get) Token: 0x06006B62 RID: 27490 RVA: 0x0021B4D6 File Offset: 0x002196D6
		internal bool IsEmpty
		{
			get
			{
				return this._typeUnion._vt == 0;
			}
		}

		// Token: 0x06006B63 RID: 27491 RVA: 0x0021B4E6 File Offset: 0x002196E6
		public void SetAsNull()
		{
			this.VariantType = VarEnum.VT_NULL;
		}

		// Token: 0x06006B64 RID: 27492 RVA: 0x0021B4F0 File Offset: 0x002196F0
		public void SetAsIConvertible(IConvertible value)
		{
			TypeCode typeCode = value.GetTypeCode();
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			switch (typeCode)
			{
			case TypeCode.Empty:
				return;
			case TypeCode.Object:
				this.AsUnknown = value;
				return;
			case TypeCode.DBNull:
				this.SetAsNull();
				return;
			case TypeCode.Boolean:
				this.AsBool = value.ToBoolean(currentCulture);
				return;
			case TypeCode.Char:
				this.AsUi2 = (ushort)value.ToChar(currentCulture);
				return;
			case TypeCode.SByte:
				this.AsI1 = value.ToSByte(currentCulture);
				return;
			case TypeCode.Byte:
				this.AsUi1 = value.ToByte(currentCulture);
				return;
			case TypeCode.Int16:
				this.AsI2 = value.ToInt16(currentCulture);
				return;
			case TypeCode.UInt16:
				this.AsUi2 = value.ToUInt16(currentCulture);
				return;
			case TypeCode.Int32:
				this.AsI4 = value.ToInt32(currentCulture);
				return;
			case TypeCode.UInt32:
				this.AsUi4 = value.ToUInt32(currentCulture);
				return;
			case TypeCode.Int64:
				this.AsI8 = value.ToInt64(currentCulture);
				return;
			case TypeCode.UInt64:
				this.AsI8 = value.ToInt64(currentCulture);
				return;
			case TypeCode.Single:
				this.AsR4 = value.ToSingle(currentCulture);
				return;
			case TypeCode.Double:
				this.AsR8 = value.ToDouble(currentCulture);
				return;
			case TypeCode.Decimal:
				this.AsDecimal = value.ToDecimal(currentCulture);
				return;
			case TypeCode.DateTime:
				this.AsDate = value.ToDateTime(currentCulture);
				return;
			case TypeCode.String:
				this.AsBstr = value.ToString(currentCulture);
				return;
			}
			throw Assert.Unreachable;
		}

		// Token: 0x17001DD0 RID: 7632
		// (get) Token: 0x06006B65 RID: 27493 RVA: 0x0021B64A File Offset: 0x0021984A
		// (set) Token: 0x06006B66 RID: 27494 RVA: 0x0021B65C File Offset: 0x0021985C
		public sbyte AsI1
		{
			get
			{
				return this._typeUnion._unionTypes._i1;
			}
			set
			{
				this.VariantType = VarEnum.VT_I1;
				this._typeUnion._unionTypes._i1 = value;
			}
		}

		// Token: 0x06006B67 RID: 27495 RVA: 0x0021B677 File Offset: 0x00219877
		public void SetAsByrefI1(ref sbyte value)
		{
			this.VariantType = (VarEnum)16400;
			this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertSByteByrefToPtr(ref value);
		}

		// Token: 0x17001DD1 RID: 7633
		// (get) Token: 0x06006B68 RID: 27496 RVA: 0x0021B69A File Offset: 0x0021989A
		// (set) Token: 0x06006B69 RID: 27497 RVA: 0x0021B6AC File Offset: 0x002198AC
		public short AsI2
		{
			get
			{
				return this._typeUnion._unionTypes._i2;
			}
			set
			{
				this.VariantType = VarEnum.VT_I2;
				this._typeUnion._unionTypes._i2 = value;
			}
		}

		// Token: 0x06006B6A RID: 27498 RVA: 0x0021B6C6 File Offset: 0x002198C6
		public void SetAsByrefI2(ref short value)
		{
			this.VariantType = (VarEnum)16386;
			this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertInt16ByrefToPtr(ref value);
		}

		// Token: 0x17001DD2 RID: 7634
		// (get) Token: 0x06006B6B RID: 27499 RVA: 0x0021B6E9 File Offset: 0x002198E9
		// (set) Token: 0x06006B6C RID: 27500 RVA: 0x0021B6FB File Offset: 0x002198FB
		public int AsI4
		{
			get
			{
				return this._typeUnion._unionTypes._i4;
			}
			set
			{
				this.VariantType = VarEnum.VT_I4;
				this._typeUnion._unionTypes._i4 = value;
			}
		}

		// Token: 0x06006B6D RID: 27501 RVA: 0x0021B715 File Offset: 0x00219915
		public void SetAsByrefI4(ref int value)
		{
			this.VariantType = (VarEnum)16387;
			this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertInt32ByrefToPtr(ref value);
		}

		// Token: 0x17001DD3 RID: 7635
		// (get) Token: 0x06006B6E RID: 27502 RVA: 0x0021B738 File Offset: 0x00219938
		// (set) Token: 0x06006B6F RID: 27503 RVA: 0x0021B74A File Offset: 0x0021994A
		public long AsI8
		{
			get
			{
				return this._typeUnion._unionTypes._i8;
			}
			set
			{
				this.VariantType = VarEnum.VT_I8;
				this._typeUnion._unionTypes._i8 = value;
			}
		}

		// Token: 0x06006B70 RID: 27504 RVA: 0x0021B765 File Offset: 0x00219965
		public void SetAsByrefI8(ref long value)
		{
			this.VariantType = (VarEnum)16404;
			this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertInt64ByrefToPtr(ref value);
		}

		// Token: 0x17001DD4 RID: 7636
		// (get) Token: 0x06006B71 RID: 27505 RVA: 0x0021B788 File Offset: 0x00219988
		// (set) Token: 0x06006B72 RID: 27506 RVA: 0x0021B79A File Offset: 0x0021999A
		public byte AsUi1
		{
			get
			{
				return this._typeUnion._unionTypes._ui1;
			}
			set
			{
				this.VariantType = VarEnum.VT_UI1;
				this._typeUnion._unionTypes._ui1 = value;
			}
		}

		// Token: 0x06006B73 RID: 27507 RVA: 0x0021B7B5 File Offset: 0x002199B5
		public void SetAsByrefUi1(ref byte value)
		{
			this.VariantType = (VarEnum)16401;
			this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertByteByrefToPtr(ref value);
		}

		// Token: 0x17001DD5 RID: 7637
		// (get) Token: 0x06006B74 RID: 27508 RVA: 0x0021B7D8 File Offset: 0x002199D8
		// (set) Token: 0x06006B75 RID: 27509 RVA: 0x0021B7EA File Offset: 0x002199EA
		public ushort AsUi2
		{
			get
			{
				return this._typeUnion._unionTypes._ui2;
			}
			set
			{
				this.VariantType = VarEnum.VT_UI2;
				this._typeUnion._unionTypes._ui2 = value;
			}
		}

		// Token: 0x06006B76 RID: 27510 RVA: 0x0021B805 File Offset: 0x00219A05
		public void SetAsByrefUi2(ref ushort value)
		{
			this.VariantType = (VarEnum)16402;
			this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertUInt16ByrefToPtr(ref value);
		}

		// Token: 0x17001DD6 RID: 7638
		// (get) Token: 0x06006B77 RID: 27511 RVA: 0x0021B828 File Offset: 0x00219A28
		// (set) Token: 0x06006B78 RID: 27512 RVA: 0x0021B83A File Offset: 0x00219A3A
		public uint AsUi4
		{
			get
			{
				return this._typeUnion._unionTypes._ui4;
			}
			set
			{
				this.VariantType = VarEnum.VT_UI4;
				this._typeUnion._unionTypes._ui4 = value;
			}
		}

		// Token: 0x06006B79 RID: 27513 RVA: 0x0021B855 File Offset: 0x00219A55
		public void SetAsByrefUi4(ref uint value)
		{
			this.VariantType = (VarEnum)16403;
			this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertUInt32ByrefToPtr(ref value);
		}

		// Token: 0x17001DD7 RID: 7639
		// (get) Token: 0x06006B7A RID: 27514 RVA: 0x0021B878 File Offset: 0x00219A78
		// (set) Token: 0x06006B7B RID: 27515 RVA: 0x0021B88A File Offset: 0x00219A8A
		public ulong AsUi8
		{
			get
			{
				return this._typeUnion._unionTypes._ui8;
			}
			set
			{
				this.VariantType = VarEnum.VT_UI8;
				this._typeUnion._unionTypes._ui8 = value;
			}
		}

		// Token: 0x06006B7C RID: 27516 RVA: 0x0021B8A5 File Offset: 0x00219AA5
		public void SetAsByrefUi8(ref ulong value)
		{
			this.VariantType = (VarEnum)16405;
			this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertUInt64ByrefToPtr(ref value);
		}

		// Token: 0x17001DD8 RID: 7640
		// (get) Token: 0x06006B7D RID: 27517 RVA: 0x0021B8C8 File Offset: 0x00219AC8
		// (set) Token: 0x06006B7E RID: 27518 RVA: 0x0021B8DA File Offset: 0x00219ADA
		public IntPtr AsInt
		{
			get
			{
				return this._typeUnion._unionTypes._int;
			}
			set
			{
				this.VariantType = VarEnum.VT_INT;
				this._typeUnion._unionTypes._int = value;
			}
		}

		// Token: 0x06006B7F RID: 27519 RVA: 0x0021B8F5 File Offset: 0x00219AF5
		public void SetAsByrefInt(ref IntPtr value)
		{
			this.VariantType = (VarEnum)16406;
			this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertIntPtrByrefToPtr(ref value);
		}

		// Token: 0x17001DD9 RID: 7641
		// (get) Token: 0x06006B80 RID: 27520 RVA: 0x0021B918 File Offset: 0x00219B18
		// (set) Token: 0x06006B81 RID: 27521 RVA: 0x0021B92A File Offset: 0x00219B2A
		public UIntPtr AsUint
		{
			get
			{
				return this._typeUnion._unionTypes._uint;
			}
			set
			{
				this.VariantType = VarEnum.VT_UINT;
				this._typeUnion._unionTypes._uint = value;
			}
		}

		// Token: 0x06006B82 RID: 27522 RVA: 0x0021B945 File Offset: 0x00219B45
		public void SetAsByrefUint(ref UIntPtr value)
		{
			this.VariantType = (VarEnum)16407;
			this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertUIntPtrByrefToPtr(ref value);
		}

		// Token: 0x17001DDA RID: 7642
		// (get) Token: 0x06006B83 RID: 27523 RVA: 0x0021B968 File Offset: 0x00219B68
		// (set) Token: 0x06006B84 RID: 27524 RVA: 0x0021B980 File Offset: 0x00219B80
		public bool AsBool
		{
			get
			{
				return this._typeUnion._unionTypes._bool != 0;
			}
			set
			{
				this.VariantType = VarEnum.VT_BOOL;
				this._typeUnion._unionTypes._bool = (value ? -1 : 0);
			}
		}

		// Token: 0x06006B85 RID: 27525 RVA: 0x0021B9A1 File Offset: 0x00219BA1
		public void SetAsByrefBool(ref short value)
		{
			this.VariantType = (VarEnum)16395;
			this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertInt16ByrefToPtr(ref value);
		}

		// Token: 0x17001DDB RID: 7643
		// (get) Token: 0x06006B86 RID: 27526 RVA: 0x0021B9C4 File Offset: 0x00219BC4
		// (set) Token: 0x06006B87 RID: 27527 RVA: 0x0021B9D6 File Offset: 0x00219BD6
		public int AsError
		{
			get
			{
				return this._typeUnion._unionTypes._error;
			}
			set
			{
				this.VariantType = VarEnum.VT_ERROR;
				this._typeUnion._unionTypes._error = value;
			}
		}

		// Token: 0x06006B88 RID: 27528 RVA: 0x0021B9F1 File Offset: 0x00219BF1
		public void SetAsByrefError(ref int value)
		{
			this.VariantType = (VarEnum)16394;
			this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertInt32ByrefToPtr(ref value);
		}

		// Token: 0x17001DDC RID: 7644
		// (get) Token: 0x06006B89 RID: 27529 RVA: 0x0021BA14 File Offset: 0x00219C14
		// (set) Token: 0x06006B8A RID: 27530 RVA: 0x0021BA26 File Offset: 0x00219C26
		public float AsR4
		{
			get
			{
				return this._typeUnion._unionTypes._r4;
			}
			set
			{
				this.VariantType = VarEnum.VT_R4;
				this._typeUnion._unionTypes._r4 = value;
			}
		}

		// Token: 0x06006B8B RID: 27531 RVA: 0x0021BA40 File Offset: 0x00219C40
		public void SetAsByrefR4(ref float value)
		{
			this.VariantType = (VarEnum)16388;
			this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertSingleByrefToPtr(ref value);
		}

		// Token: 0x17001DDD RID: 7645
		// (get) Token: 0x06006B8C RID: 27532 RVA: 0x0021BA63 File Offset: 0x00219C63
		// (set) Token: 0x06006B8D RID: 27533 RVA: 0x0021BA75 File Offset: 0x00219C75
		public double AsR8
		{
			get
			{
				return this._typeUnion._unionTypes._r8;
			}
			set
			{
				this.VariantType = VarEnum.VT_R8;
				this._typeUnion._unionTypes._r8 = value;
			}
		}

		// Token: 0x06006B8E RID: 27534 RVA: 0x0021BA8F File Offset: 0x00219C8F
		public void SetAsByrefR8(ref double value)
		{
			this.VariantType = (VarEnum)16389;
			this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertDoubleByrefToPtr(ref value);
		}

		// Token: 0x17001DDE RID: 7646
		// (get) Token: 0x06006B8F RID: 27535 RVA: 0x0021BAB4 File Offset: 0x00219CB4
		// (set) Token: 0x06006B90 RID: 27536 RVA: 0x0021BADC File Offset: 0x00219CDC
		public decimal AsDecimal
		{
			get
			{
				Variant variant = this;
				variant._typeUnion._vt = 0;
				return variant._decimal;
			}
			set
			{
				this.VariantType = VarEnum.VT_DECIMAL;
				this._decimal = value;
				this._typeUnion._vt = 14;
			}
		}

		// Token: 0x06006B91 RID: 27537 RVA: 0x0021BAFA File Offset: 0x00219CFA
		public void SetAsByrefDecimal(ref decimal value)
		{
			this.VariantType = (VarEnum)16398;
			this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertDecimalByrefToPtr(ref value);
		}

		// Token: 0x17001DDF RID: 7647
		// (get) Token: 0x06006B92 RID: 27538 RVA: 0x0021BB1D File Offset: 0x00219D1D
		// (set) Token: 0x06006B93 RID: 27539 RVA: 0x0021BB34 File Offset: 0x00219D34
		public decimal AsCy
		{
			get
			{
				return decimal.FromOACurrency(this._typeUnion._unionTypes._cy);
			}
			set
			{
				this.VariantType = VarEnum.VT_CY;
				this._typeUnion._unionTypes._cy = decimal.ToOACurrency(value);
			}
		}

		// Token: 0x06006B94 RID: 27540 RVA: 0x0021BB53 File Offset: 0x00219D53
		public void SetAsByrefCy(ref long value)
		{
			this.VariantType = (VarEnum)16390;
			this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertInt64ByrefToPtr(ref value);
		}

		// Token: 0x17001DE0 RID: 7648
		// (get) Token: 0x06006B95 RID: 27541 RVA: 0x0021BB76 File Offset: 0x00219D76
		// (set) Token: 0x06006B96 RID: 27542 RVA: 0x0021BB8D File Offset: 0x00219D8D
		public DateTime AsDate
		{
			get
			{
				return DateTime.FromOADate(this._typeUnion._unionTypes._date);
			}
			set
			{
				this.VariantType = VarEnum.VT_DATE;
				this._typeUnion._unionTypes._date = value.ToOADate();
			}
		}

		// Token: 0x06006B97 RID: 27543 RVA: 0x0021BBAD File Offset: 0x00219DAD
		public void SetAsByrefDate(ref double value)
		{
			this.VariantType = (VarEnum)16391;
			this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertDoubleByrefToPtr(ref value);
		}

		// Token: 0x17001DE1 RID: 7649
		// (get) Token: 0x06006B98 RID: 27544 RVA: 0x0021BBD0 File Offset: 0x00219DD0
		// (set) Token: 0x06006B99 RID: 27545 RVA: 0x0021BC05 File Offset: 0x00219E05
		public string AsBstr
		{
			get
			{
				if (this._typeUnion._unionTypes._bstr != IntPtr.Zero)
				{
					return Marshal.PtrToStringBSTR(this._typeUnion._unionTypes._bstr);
				}
				return null;
			}
			set
			{
				this.VariantType = VarEnum.VT_BSTR;
				if (value != null)
				{
					Marshal.GetNativeVariantForObject(value, UnsafeMethods.ConvertVariantByrefToPtr(ref this));
				}
			}
		}

		// Token: 0x06006B9A RID: 27546 RVA: 0x0021BC1D File Offset: 0x00219E1D
		public void SetAsByrefBstr(ref IntPtr value)
		{
			this.VariantType = (VarEnum)16392;
			this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertIntPtrByrefToPtr(ref value);
		}

		// Token: 0x17001DE2 RID: 7650
		// (get) Token: 0x06006B9B RID: 27547 RVA: 0x0021BC40 File Offset: 0x00219E40
		// (set) Token: 0x06006B9C RID: 27548 RVA: 0x0021BC75 File Offset: 0x00219E75
		public object AsUnknown
		{
			get
			{
				if (this._typeUnion._unionTypes._dispatch != IntPtr.Zero)
				{
					return Marshal.GetObjectForIUnknown(this._typeUnion._unionTypes._unknown);
				}
				return null;
			}
			set
			{
				this.VariantType = VarEnum.VT_UNKNOWN;
				if (value != null)
				{
					this._typeUnion._unionTypes._unknown = Marshal.GetIUnknownForObject(value);
				}
			}
		}

		// Token: 0x06006B9D RID: 27549 RVA: 0x0021BC98 File Offset: 0x00219E98
		public void SetAsByrefUnknown(ref IntPtr value)
		{
			this.VariantType = (VarEnum)16397;
			this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertIntPtrByrefToPtr(ref value);
		}

		// Token: 0x17001DE3 RID: 7651
		// (get) Token: 0x06006B9E RID: 27550 RVA: 0x0021BCBB File Offset: 0x00219EBB
		// (set) Token: 0x06006B9F RID: 27551 RVA: 0x0021BCF0 File Offset: 0x00219EF0
		public object AsDispatch
		{
			get
			{
				if (this._typeUnion._unionTypes._dispatch != IntPtr.Zero)
				{
					return Marshal.GetObjectForIUnknown(this._typeUnion._unionTypes._dispatch);
				}
				return null;
			}
			set
			{
				this.VariantType = VarEnum.VT_DISPATCH;
				if (value != null)
				{
					this._typeUnion._unionTypes._unknown = Marshal.GetIDispatchForObject(value);
				}
			}
		}

		// Token: 0x06006BA0 RID: 27552 RVA: 0x0021BD13 File Offset: 0x00219F13
		public void SetAsByrefDispatch(ref IntPtr value)
		{
			this.VariantType = (VarEnum)16393;
			this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertIntPtrByrefToPtr(ref value);
		}

		// Token: 0x17001DE4 RID: 7652
		// (get) Token: 0x06006BA1 RID: 27553 RVA: 0x0021BD36 File Offset: 0x00219F36
		// (set) Token: 0x06006BA2 RID: 27554 RVA: 0x0021BD43 File Offset: 0x00219F43
		public object AsVariant
		{
			get
			{
				return Marshal.GetObjectForNativeVariant(UnsafeMethods.ConvertVariantByrefToPtr(ref this));
			}
			set
			{
				if (value != null)
				{
					UnsafeMethods.InitVariantForObject(value, ref this);
				}
			}
		}

		// Token: 0x06006BA3 RID: 27555 RVA: 0x0021BD4F File Offset: 0x00219F4F
		public void SetAsByrefVariant(ref Variant value)
		{
			this.VariantType = (VarEnum)16396;
			this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertVariantByrefToPtr(ref value);
		}

		// Token: 0x06006BA4 RID: 27556 RVA: 0x0021BD74 File Offset: 0x00219F74
		public void SetAsByrefVariantIndirect(ref Variant value)
		{
			VarEnum variantType = value.VariantType;
			switch (variantType)
			{
			case VarEnum.VT_EMPTY:
			case VarEnum.VT_NULL:
				this.SetAsByrefVariant(ref value);
				return;
			default:
				if (variantType != VarEnum.VT_DECIMAL)
				{
					if (variantType != VarEnum.VT_RECORD)
					{
						this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertIntPtrByrefToPtr(ref value._typeUnion._unionTypes._byref);
					}
					else
					{
						this._typeUnion._unionTypes._record = value._typeUnion._unionTypes._record;
					}
				}
				else
				{
					this._typeUnion._unionTypes._byref = UnsafeMethods.ConvertDecimalByrefToPtr(ref value._decimal);
				}
				this.VariantType = (value.VariantType | VarEnum.VT_BYREF);
				return;
			}
		}

		// Token: 0x06006BA5 RID: 27557 RVA: 0x0021BE20 File Offset: 0x0021A020
		internal static PropertyInfo GetAccessor(VarEnum varType)
		{
			switch (varType)
			{
			case VarEnum.VT_I2:
				return typeof(Variant).GetProperty("AsI2");
			case VarEnum.VT_I4:
				return typeof(Variant).GetProperty("AsI4");
			case VarEnum.VT_R4:
				return typeof(Variant).GetProperty("AsR4");
			case VarEnum.VT_R8:
				return typeof(Variant).GetProperty("AsR8");
			case VarEnum.VT_CY:
				return typeof(Variant).GetProperty("AsCy");
			case VarEnum.VT_DATE:
				return typeof(Variant).GetProperty("AsDate");
			case VarEnum.VT_BSTR:
				return typeof(Variant).GetProperty("AsBstr");
			case VarEnum.VT_DISPATCH:
				return typeof(Variant).GetProperty("AsDispatch");
			case VarEnum.VT_ERROR:
				return typeof(Variant).GetProperty("AsError");
			case VarEnum.VT_BOOL:
				return typeof(Variant).GetProperty("AsBool");
			case VarEnum.VT_VARIANT:
			case VarEnum.VT_RECORD:
				break;
			case VarEnum.VT_UNKNOWN:
				return typeof(Variant).GetProperty("AsUnknown");
			case VarEnum.VT_DECIMAL:
				return typeof(Variant).GetProperty("AsDecimal");
			case (VarEnum)15:
			case VarEnum.VT_VOID:
			case VarEnum.VT_HRESULT:
			case VarEnum.VT_PTR:
			case VarEnum.VT_SAFEARRAY:
			case VarEnum.VT_CARRAY:
			case VarEnum.VT_USERDEFINED:
			case VarEnum.VT_LPSTR:
			case VarEnum.VT_LPWSTR:
			case (VarEnum)32:
			case (VarEnum)33:
			case (VarEnum)34:
			case (VarEnum)35:
				goto IL_25F;
			case VarEnum.VT_I1:
				return typeof(Variant).GetProperty("AsI1");
			case VarEnum.VT_UI1:
				return typeof(Variant).GetProperty("AsUi1");
			case VarEnum.VT_UI2:
				return typeof(Variant).GetProperty("AsUi2");
			case VarEnum.VT_UI4:
				return typeof(Variant).GetProperty("AsUi4");
			case VarEnum.VT_I8:
				return typeof(Variant).GetProperty("AsI8");
			case VarEnum.VT_UI8:
				return typeof(Variant).GetProperty("AsUi8");
			case VarEnum.VT_INT:
				return typeof(Variant).GetProperty("AsInt");
			case VarEnum.VT_UINT:
				return typeof(Variant).GetProperty("AsUint");
			default:
				if (varType != VarEnum.VT_ARRAY)
				{
					goto IL_25F;
				}
				break;
			}
			return typeof(Variant).GetProperty("AsVariant");
			IL_25F:
			throw Error.VariantGetAccessorNYI(varType);
		}

		// Token: 0x06006BA6 RID: 27558 RVA: 0x0021C098 File Offset: 0x0021A298
		internal static MethodInfo GetByrefSetter(VarEnum varType)
		{
			switch (varType)
			{
			case VarEnum.VT_I2:
				return typeof(Variant).GetMethod("SetAsByrefI2");
			case VarEnum.VT_I4:
				return typeof(Variant).GetMethod("SetAsByrefI4");
			case VarEnum.VT_R4:
				return typeof(Variant).GetMethod("SetAsByrefR4");
			case VarEnum.VT_R8:
				return typeof(Variant).GetMethod("SetAsByrefR8");
			case VarEnum.VT_CY:
				return typeof(Variant).GetMethod("SetAsByrefCy");
			case VarEnum.VT_DATE:
				return typeof(Variant).GetMethod("SetAsByrefDate");
			case VarEnum.VT_BSTR:
				return typeof(Variant).GetMethod("SetAsByrefBstr");
			case VarEnum.VT_DISPATCH:
				return typeof(Variant).GetMethod("SetAsByrefDispatch");
			case VarEnum.VT_ERROR:
				return typeof(Variant).GetMethod("SetAsByrefError");
			case VarEnum.VT_BOOL:
				return typeof(Variant).GetMethod("SetAsByrefBool");
			case VarEnum.VT_VARIANT:
				return typeof(Variant).GetMethod("SetAsByrefVariant");
			case VarEnum.VT_UNKNOWN:
				return typeof(Variant).GetMethod("SetAsByrefUnknown");
			case VarEnum.VT_DECIMAL:
				return typeof(Variant).GetMethod("SetAsByrefDecimal");
			case (VarEnum)15:
			case VarEnum.VT_VOID:
			case VarEnum.VT_HRESULT:
			case VarEnum.VT_PTR:
			case VarEnum.VT_SAFEARRAY:
			case VarEnum.VT_CARRAY:
			case VarEnum.VT_USERDEFINED:
			case VarEnum.VT_LPSTR:
			case VarEnum.VT_LPWSTR:
			case (VarEnum)32:
			case (VarEnum)33:
			case (VarEnum)34:
			case (VarEnum)35:
				goto IL_274;
			case VarEnum.VT_I1:
				return typeof(Variant).GetMethod("SetAsByrefI1");
			case VarEnum.VT_UI1:
				return typeof(Variant).GetMethod("SetAsByrefUi1");
			case VarEnum.VT_UI2:
				return typeof(Variant).GetMethod("SetAsByrefUi2");
			case VarEnum.VT_UI4:
				return typeof(Variant).GetMethod("SetAsByrefUi4");
			case VarEnum.VT_I8:
				return typeof(Variant).GetMethod("SetAsByrefI8");
			case VarEnum.VT_UI8:
				return typeof(Variant).GetMethod("SetAsByrefUi8");
			case VarEnum.VT_INT:
				return typeof(Variant).GetMethod("SetAsByrefInt");
			case VarEnum.VT_UINT:
				return typeof(Variant).GetMethod("SetAsByrefUint");
			case VarEnum.VT_RECORD:
				break;
			default:
				if (varType != VarEnum.VT_ARRAY)
				{
					goto IL_274;
				}
				break;
			}
			return typeof(Variant).GetMethod("SetAsByrefVariantIndirect");
			IL_274:
			throw Error.VariantGetAccessorNYI(varType);
		}

		// Token: 0x04003344 RID: 13124
		[FieldOffset(0)]
		private Variant.TypeUnion _typeUnion;

		// Token: 0x04003345 RID: 13125
		[FieldOffset(0)]
		private decimal _decimal;

		// Token: 0x02000A92 RID: 2706
		private struct TypeUnion
		{
			// Token: 0x04003346 RID: 13126
			internal ushort _vt;

			// Token: 0x04003347 RID: 13127
			internal ushort _wReserved1;

			// Token: 0x04003348 RID: 13128
			internal ushort _wReserved2;

			// Token: 0x04003349 RID: 13129
			internal ushort _wReserved3;

			// Token: 0x0400334A RID: 13130
			internal Variant.UnionTypes _unionTypes;
		}

		// Token: 0x02000A93 RID: 2707
		private struct Record
		{
			// Token: 0x0400334B RID: 13131
			private IntPtr _record;

			// Token: 0x0400334C RID: 13132
			private IntPtr _recordInfo;
		}

		// Token: 0x02000A94 RID: 2708
		[StructLayout(LayoutKind.Explicit)]
		private struct UnionTypes
		{
			// Token: 0x0400334D RID: 13133
			[FieldOffset(0)]
			internal sbyte _i1;

			// Token: 0x0400334E RID: 13134
			[FieldOffset(0)]
			internal short _i2;

			// Token: 0x0400334F RID: 13135
			[FieldOffset(0)]
			internal int _i4;

			// Token: 0x04003350 RID: 13136
			[FieldOffset(0)]
			internal long _i8;

			// Token: 0x04003351 RID: 13137
			[FieldOffset(0)]
			internal byte _ui1;

			// Token: 0x04003352 RID: 13138
			[FieldOffset(0)]
			internal ushort _ui2;

			// Token: 0x04003353 RID: 13139
			[FieldOffset(0)]
			internal uint _ui4;

			// Token: 0x04003354 RID: 13140
			[FieldOffset(0)]
			internal ulong _ui8;

			// Token: 0x04003355 RID: 13141
			[FieldOffset(0)]
			internal IntPtr _int;

			// Token: 0x04003356 RID: 13142
			[FieldOffset(0)]
			internal UIntPtr _uint;

			// Token: 0x04003357 RID: 13143
			[FieldOffset(0)]
			internal short _bool;

			// Token: 0x04003358 RID: 13144
			[FieldOffset(0)]
			internal int _error;

			// Token: 0x04003359 RID: 13145
			[FieldOffset(0)]
			internal float _r4;

			// Token: 0x0400335A RID: 13146
			[FieldOffset(0)]
			internal double _r8;

			// Token: 0x0400335B RID: 13147
			[FieldOffset(0)]
			internal long _cy;

			// Token: 0x0400335C RID: 13148
			[FieldOffset(0)]
			internal double _date;

			// Token: 0x0400335D RID: 13149
			[FieldOffset(0)]
			internal IntPtr _bstr;

			// Token: 0x0400335E RID: 13150
			[FieldOffset(0)]
			internal IntPtr _unknown;

			// Token: 0x0400335F RID: 13151
			[FieldOffset(0)]
			internal IntPtr _dispatch;

			// Token: 0x04003360 RID: 13152
			[FieldOffset(0)]
			internal IntPtr _byref;

			// Token: 0x04003361 RID: 13153
			[FieldOffset(0)]
			internal Variant.Record _record;
		}
	}
}
