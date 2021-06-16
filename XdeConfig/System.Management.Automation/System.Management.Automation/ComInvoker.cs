using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Management.Automation
{
	// Token: 0x02000179 RID: 377
	internal static class ComInvoker
	{
		// Token: 0x060012CA RID: 4810 RVA: 0x00074ADC File Offset: 0x00072CDC
		private unsafe static void MakeByRefVariant(IntPtr srcVariantPtr, IntPtr destVariantPtr)
		{
			ComInvoker.Variant* ptr = (ComInvoker.Variant*)((void*)srcVariantPtr);
			ComInvoker.Variant* ptr2 = (ComInvoker.Variant*)((void*)destVariantPtr);
			VarEnum vt = (VarEnum)ptr->_typeUnion._vt;
			switch (vt)
			{
			case VarEnum.VT_EMPTY:
			case VarEnum.VT_NULL:
				ptr2->_typeUnion._unionTypes._byref = new IntPtr((void*)ptr);
				ptr2->_typeUnion._vt = 16396;
				return;
			default:
				switch (vt)
				{
				case VarEnum.VT_VARIANT:
					ptr2->_typeUnion._unionTypes._byref = new IntPtr((void*)ptr);
					goto IL_120;
				case VarEnum.VT_UNKNOWN:
					break;
				case VarEnum.VT_DECIMAL:
					ptr2->_typeUnion._unionTypes._byref = new IntPtr((void*)(&ptr->_decimal));
					goto IL_120;
				default:
					if (vt == VarEnum.VT_RECORD)
					{
						ptr2->_typeUnion._unionTypes._record._record = ptr->_typeUnion._unionTypes._record._record;
						ptr2->_typeUnion._unionTypes._record._recordInfo = ptr->_typeUnion._unionTypes._record._recordInfo;
						goto IL_120;
					}
					break;
				}
				ptr2->_typeUnion._unionTypes._byref = new IntPtr((void*)(&ptr->_typeUnion._unionTypes._i4));
				IL_120:
				ptr2->_typeUnion._vt = (ptr->_typeUnion._vt | 16384);
				return;
			}
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x00074C28 File Offset: 0x00072E28
		private unsafe static IntPtr NewVariantArray(int length)
		{
			IntPtr intPtr = Marshal.AllocCoTaskMem(ComInvoker.VariantSize * length);
			for (int i = 0; i < length; i++)
			{
				IntPtr value = intPtr + ComInvoker.VariantSize * i;
				ComInvoker.Variant* ptr = (ComInvoker.Variant*)((void*)value);
				ptr->_typeUnion._vt = 0;
			}
			return intPtr;
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x00074C70 File Offset: 0x00072E70
		internal static bool[] GetByRefArray(ParameterInformation[] parameters, int argumentCount, bool isPropertySet)
		{
			if (parameters.Length == 0)
			{
				return null;
			}
			bool[] array = new bool[argumentCount];
			int num = argumentCount;
			if (isPropertySet)
			{
				num = argumentCount - 1;
				array[num] = false;
			}
			for (int i = 0; i < num; i++)
			{
				array[i] = parameters[i].isByRef;
			}
			return array;
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x00074CB4 File Offset: 0x00072EB4
		internal static object Invoke(IDispatch target, int dispId, object[] args, bool[] byRef, System.Runtime.InteropServices.ComTypes.INVOKEKIND invokeKind)
		{
			int num = (args != null) ? args.Length : 0;
			int num2;
			if (byRef == null)
			{
				num2 = 0;
			}
			else
			{
				num2 = byRef.Count((bool c) => c);
			}
			int num3 = num2;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			IntPtr intPtr3 = IntPtr.Zero;
			object result;
			try
			{
				if (num > 0)
				{
					intPtr = ComInvoker.NewVariantArray(num);
					int num4 = 0;
					for (int i = 0; i < num; i++)
					{
						int num5 = num - i - 1;
						IntPtr intPtr4 = intPtr + ComInvoker.VariantSize * num5;
						if (byRef != null && byRef[i])
						{
							if (intPtr3 == IntPtr.Zero)
							{
								intPtr3 = ComInvoker.NewVariantArray(num3);
							}
							IntPtr intPtr5 = intPtr3 + ComInvoker.VariantSize * num4;
							Marshal.GetNativeVariantForObject(args[i], intPtr5);
							ComInvoker.MakeByRefVariant(intPtr5, intPtr4);
							num4++;
						}
						else
						{
							Marshal.GetNativeVariantForObject(args[i], intPtr4);
						}
					}
				}
				System.Runtime.InteropServices.ComTypes.DISPPARAMS[] array = new System.Runtime.InteropServices.ComTypes.DISPPARAMS[1];
				array[0].rgvarg = intPtr;
				array[0].cArgs = num;
				if (invokeKind == System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYPUT || invokeKind == System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYPUTREF)
				{
					intPtr2 = Marshal.AllocCoTaskMem(4);
					Marshal.WriteInt32(intPtr2, -3);
					array[0].cNamedArgs = 1;
					array[0].rgdispidNamedArgs = intPtr2;
				}
				else
				{
					array[0].cNamedArgs = 0;
					array[0].rgdispidNamedArgs = IntPtr.Zero;
				}
				ComInvoker.EXCEPINFO excepinfo = default(ComInvoker.EXCEPINFO);
				object obj = null;
				try
				{
					uint num6 = 0U;
					target.Invoke(dispId, ComInvoker.IID_NULL, 1033, invokeKind, array, out obj, out excepinfo, out num6);
				}
				catch (Exception ex)
				{
					string text = null;
					if (ex.HResult == -2147352567)
					{
						int errorCode = (excepinfo.scode != 0) ? excepinfo.scode : ((int)excepinfo.wCode);
						ex = (Marshal.GetExceptionForHR(errorCode, IntPtr.Zero) ?? ex);
						if (excepinfo.bstrDescription != IntPtr.Zero)
						{
							text = Marshal.PtrToStringBSTR(excepinfo.bstrDescription);
							Marshal.FreeBSTR(excepinfo.bstrDescription);
						}
						if (excepinfo.bstrSource != IntPtr.Zero)
						{
							Marshal.FreeBSTR(excepinfo.bstrSource);
						}
						if (excepinfo.bstrHelpFile != IntPtr.Zero)
						{
							Marshal.FreeBSTR(excepinfo.bstrHelpFile);
						}
					}
					TargetInvocationException ex2 = (text == null) ? new TargetInvocationException(ex) : new TargetInvocationException(text, ex);
					throw ex2;
				}
				if (num3 > 0)
				{
					for (int j = 0; j < num; j++)
					{
						int num7 = num - j - 1;
						if (byRef != null && byRef[j])
						{
							args[j] = Marshal.GetObjectForNativeVariant(intPtr + ComInvoker.VariantSize * num7);
						}
					}
				}
				result = obj;
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					for (int k = 0; k < num; k++)
					{
						ComInvoker.VariantClear(intPtr + ComInvoker.VariantSize * k);
					}
					Marshal.FreeCoTaskMem(intPtr);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(intPtr2);
				}
				if (intPtr3 != IntPtr.Zero)
				{
					for (int l = 0; l < num3; l++)
					{
						ComInvoker.VariantClear(intPtr3 + ComInvoker.VariantSize * l);
					}
					Marshal.FreeCoTaskMem(intPtr3);
				}
			}
			return result;
		}

		// Token: 0x060012CE RID: 4814
		[DllImport("oleaut32.dll")]
		internal static extern void VariantClear(IntPtr pVariant);

		// Token: 0x040007F3 RID: 2035
		private const int DISP_E_EXCEPTION = -2147352567;

		// Token: 0x040007F4 RID: 2036
		private const int LCID_DEFAULT = 1033;

		// Token: 0x040007F5 RID: 2037
		private const int DISPID_PROPERTYPUT = -3;

		// Token: 0x040007F6 RID: 2038
		private static readonly Guid IID_NULL = default(Guid);

		// Token: 0x040007F7 RID: 2039
		private static readonly int VariantSize = ClrFacade.SizeOf<ComInvoker.Variant>();

		// Token: 0x0200017A RID: 378
		internal struct EXCEPINFO
		{
			// Token: 0x040007F9 RID: 2041
			public short wCode;

			// Token: 0x040007FA RID: 2042
			public short wReserved;

			// Token: 0x040007FB RID: 2043
			public IntPtr bstrSource;

			// Token: 0x040007FC RID: 2044
			public IntPtr bstrDescription;

			// Token: 0x040007FD RID: 2045
			public IntPtr bstrHelpFile;

			// Token: 0x040007FE RID: 2046
			public int dwHelpContext;

			// Token: 0x040007FF RID: 2047
			public IntPtr pvReserved;

			// Token: 0x04000800 RID: 2048
			public IntPtr pfnDeferredFillIn;

			// Token: 0x04000801 RID: 2049
			public int scode;
		}

		// Token: 0x0200017B RID: 379
		[StructLayout(LayoutKind.Explicit)]
		internal struct Variant
		{
			// Token: 0x04000802 RID: 2050
			[FieldOffset(0)]
			internal ComInvoker.Variant.TypeUnion _typeUnion;

			// Token: 0x04000803 RID: 2051
			[FieldOffset(0)]
			internal decimal _decimal;

			// Token: 0x0200017C RID: 380
			[StructLayout(LayoutKind.Explicit)]
			internal struct TypeUnion
			{
				// Token: 0x04000804 RID: 2052
				[FieldOffset(0)]
				internal ushort _vt;

				// Token: 0x04000805 RID: 2053
				[FieldOffset(2)]
				internal ushort _wReserved1;

				// Token: 0x04000806 RID: 2054
				[FieldOffset(4)]
				internal ushort _wReserved2;

				// Token: 0x04000807 RID: 2055
				[FieldOffset(6)]
				internal ushort _wReserved3;

				// Token: 0x04000808 RID: 2056
				[FieldOffset(8)]
				internal ComInvoker.Variant.UnionTypes _unionTypes;
			}

			// Token: 0x0200017D RID: 381
			internal struct Record
			{
				// Token: 0x04000809 RID: 2057
				internal IntPtr _record;

				// Token: 0x0400080A RID: 2058
				internal IntPtr _recordInfo;
			}

			// Token: 0x0200017E RID: 382
			[StructLayout(LayoutKind.Explicit)]
			internal struct UnionTypes
			{
				// Token: 0x0400080B RID: 2059
				[FieldOffset(0)]
				internal sbyte _i1;

				// Token: 0x0400080C RID: 2060
				[FieldOffset(0)]
				internal short _i2;

				// Token: 0x0400080D RID: 2061
				[FieldOffset(0)]
				internal int _i4;

				// Token: 0x0400080E RID: 2062
				[FieldOffset(0)]
				internal long _i8;

				// Token: 0x0400080F RID: 2063
				[FieldOffset(0)]
				internal byte _ui1;

				// Token: 0x04000810 RID: 2064
				[FieldOffset(0)]
				internal ushort _ui2;

				// Token: 0x04000811 RID: 2065
				[FieldOffset(0)]
				internal uint _ui4;

				// Token: 0x04000812 RID: 2066
				[FieldOffset(0)]
				internal ulong _ui8;

				// Token: 0x04000813 RID: 2067
				[FieldOffset(0)]
				internal int _int;

				// Token: 0x04000814 RID: 2068
				[FieldOffset(0)]
				internal uint _uint;

				// Token: 0x04000815 RID: 2069
				[FieldOffset(0)]
				internal short _bool;

				// Token: 0x04000816 RID: 2070
				[FieldOffset(0)]
				internal int _error;

				// Token: 0x04000817 RID: 2071
				[FieldOffset(0)]
				internal float _r4;

				// Token: 0x04000818 RID: 2072
				[FieldOffset(0)]
				internal double _r8;

				// Token: 0x04000819 RID: 2073
				[FieldOffset(0)]
				internal long _cy;

				// Token: 0x0400081A RID: 2074
				[FieldOffset(0)]
				internal double _date;

				// Token: 0x0400081B RID: 2075
				[FieldOffset(0)]
				internal IntPtr _bstr;

				// Token: 0x0400081C RID: 2076
				[FieldOffset(0)]
				internal IntPtr _unknown;

				// Token: 0x0400081D RID: 2077
				[FieldOffset(0)]
				internal IntPtr _dispatch;

				// Token: 0x0400081E RID: 2078
				[FieldOffset(0)]
				internal IntPtr _pvarVal;

				// Token: 0x0400081F RID: 2079
				[FieldOffset(0)]
				internal IntPtr _byref;

				// Token: 0x04000820 RID: 2080
				[FieldOffset(0)]
				internal ComInvoker.Variant.Record _record;
			}
		}
	}
}
