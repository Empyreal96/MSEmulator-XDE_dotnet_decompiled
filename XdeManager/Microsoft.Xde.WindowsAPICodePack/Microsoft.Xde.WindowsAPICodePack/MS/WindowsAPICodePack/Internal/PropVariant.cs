using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.WindowsAPICodePack.Resources;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace MS.WindowsAPICodePack.Internal
{
	// Token: 0x02000007 RID: 7
	[CLSCompliant(false)]
	[StructLayout(LayoutKind.Explicit)]
	public sealed class PropVariant : IDisposable
	{
		// Token: 0x0600001C RID: 28 RVA: 0x00002200 File Offset: 0x00000400
		private static Dictionary<Type, Action<PropVariant, Array, uint>> GenerateVectorActions()
		{
			Dictionary<Type, Action<PropVariant, Array, uint>> dictionary = new Dictionary<Type, Action<PropVariant, Array, uint>>();
			dictionary.Add(typeof(short), delegate(PropVariant pv, Array array, uint i)
			{
				short num;
				PropVariantNativeMethods.PropVariantGetInt16Elem(pv, i, out num);
				array.SetValue(num, (long)((ulong)i));
			});
			dictionary.Add(typeof(ushort), delegate(PropVariant pv, Array array, uint i)
			{
				ushort num;
				PropVariantNativeMethods.PropVariantGetUInt16Elem(pv, i, out num);
				array.SetValue(num, (long)((ulong)i));
			});
			dictionary.Add(typeof(int), delegate(PropVariant pv, Array array, uint i)
			{
				int num;
				PropVariantNativeMethods.PropVariantGetInt32Elem(pv, i, out num);
				array.SetValue(num, (long)((ulong)i));
			});
			dictionary.Add(typeof(uint), delegate(PropVariant pv, Array array, uint i)
			{
				uint num;
				PropVariantNativeMethods.PropVariantGetUInt32Elem(pv, i, out num);
				array.SetValue(num, (long)((ulong)i));
			});
			dictionary.Add(typeof(long), delegate(PropVariant pv, Array array, uint i)
			{
				long num;
				PropVariantNativeMethods.PropVariantGetInt64Elem(pv, i, out num);
				array.SetValue(num, (long)((ulong)i));
			});
			dictionary.Add(typeof(ulong), delegate(PropVariant pv, Array array, uint i)
			{
				ulong num;
				PropVariantNativeMethods.PropVariantGetUInt64Elem(pv, i, out num);
				array.SetValue(num, (long)((ulong)i));
			});
			dictionary.Add(typeof(DateTime), delegate(PropVariant pv, Array array, uint i)
			{
				System.Runtime.InteropServices.ComTypes.FILETIME filetime;
				PropVariantNativeMethods.PropVariantGetFileTimeElem(pv, i, out filetime);
				long fileTimeAsLong = PropVariant.GetFileTimeAsLong(ref filetime);
				array.SetValue(DateTime.FromFileTime(fileTimeAsLong), (long)((ulong)i));
			});
			dictionary.Add(typeof(bool), delegate(PropVariant pv, Array array, uint i)
			{
				bool flag;
				PropVariantNativeMethods.PropVariantGetBooleanElem(pv, i, out flag);
				array.SetValue(flag, (long)((ulong)i));
			});
			dictionary.Add(typeof(double), delegate(PropVariant pv, Array array, uint i)
			{
				double num;
				PropVariantNativeMethods.PropVariantGetDoubleElem(pv, i, out num);
				array.SetValue(num, (long)((ulong)i));
			});
			dictionary.Add(typeof(float), delegate(PropVariant pv, Array array, uint i)
			{
				float[] array2 = new float[1];
				Marshal.Copy(pv._ptr2, array2, (int)i, 1);
				array.SetValue(array2[0], (int)i);
			});
			dictionary.Add(typeof(decimal), delegate(PropVariant pv, Array array, uint i)
			{
				int[] array2 = new int[4];
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j] = Marshal.ReadInt32(pv._ptr2, (int)(i * 16U + (uint)(j * 4)));
				}
				array.SetValue(new decimal(array2), (long)((ulong)i));
			});
			dictionary.Add(typeof(string), delegate(PropVariant pv, Array array, uint i)
			{
				string empty = string.Empty;
				PropVariantNativeMethods.PropVariantGetStringElem(pv, i, ref empty);
				array.SetValue(empty, (long)((ulong)i));
			});
			return dictionary;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002446 File Offset: 0x00000646
		public static PropVariant FromObject(object value)
		{
			if (value == null)
			{
				return new PropVariant();
			}
			return PropVariant.GetDynamicConstructor(value.GetType())(value);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002464 File Offset: 0x00000664
		private static Func<object, PropVariant> GetDynamicConstructor(Type type)
		{
			object padlock = PropVariant._padlock;
			Func<object, PropVariant> result;
			lock (padlock)
			{
				Func<object, PropVariant> func;
				if (!PropVariant._cache.TryGetValue(type, out func))
				{
					ConstructorInfo constructor = typeof(PropVariant).GetConstructor(new Type[]
					{
						type
					});
					if (constructor == null)
					{
						throw new ArgumentException(LocalizedMessages.PropVariantTypeNotSupported);
					}
					ParameterExpression parameterExpression;
					func = Expression.Lambda<Func<object, PropVariant>>(Expression.New(constructor, new Expression[]
					{
						Expression.Convert(parameterExpression, type)
					}), new ParameterExpression[]
					{
						parameterExpression
					}).Compile();
					PropVariant._cache.Add(type, func);
				}
				result = func;
			}
			return result;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002530 File Offset: 0x00000730
		public PropVariant()
		{
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002538 File Offset: 0x00000738
		public PropVariant(string value)
		{
			if (value == null)
			{
				throw new ArgumentException(LocalizedMessages.PropVariantNullString, "value");
			}
			this._valueType = 31;
			this._ptr = Marshal.StringToCoTaskMemUni(value);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002567 File Offset: 0x00000767
		public PropVariant(string[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			PropVariantNativeMethods.InitPropVariantFromStringVector(value, (uint)value.Length, this);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002587 File Offset: 0x00000787
		public PropVariant(bool[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			PropVariantNativeMethods.InitPropVariantFromBooleanVector(value, (uint)value.Length, this);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000025A7 File Offset: 0x000007A7
		public PropVariant(short[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			PropVariantNativeMethods.InitPropVariantFromInt16Vector(value, (uint)value.Length, this);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000025C7 File Offset: 0x000007C7
		public PropVariant(ushort[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			PropVariantNativeMethods.InitPropVariantFromUInt16Vector(value, (uint)value.Length, this);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000025E7 File Offset: 0x000007E7
		public PropVariant(int[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			PropVariantNativeMethods.InitPropVariantFromInt32Vector(value, (uint)value.Length, this);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002607 File Offset: 0x00000807
		public PropVariant(uint[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			PropVariantNativeMethods.InitPropVariantFromUInt32Vector(value, (uint)value.Length, this);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002627 File Offset: 0x00000827
		public PropVariant(long[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			PropVariantNativeMethods.InitPropVariantFromInt64Vector(value, (uint)value.Length, this);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002647 File Offset: 0x00000847
		public PropVariant(ulong[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			PropVariantNativeMethods.InitPropVariantFromUInt64Vector(value, (uint)value.Length, this);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002667 File Offset: 0x00000867
		public PropVariant(double[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			PropVariantNativeMethods.InitPropVariantFromDoubleVector(value, (uint)value.Length, this);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002688 File Offset: 0x00000888
		public PropVariant(DateTime[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			System.Runtime.InteropServices.ComTypes.FILETIME[] array = new System.Runtime.InteropServices.ComTypes.FILETIME[value.Length];
			for (int i = 0; i < value.Length; i++)
			{
				array[i] = PropVariant.DateTimeToFileTime(value[i]);
			}
			PropVariantNativeMethods.InitPropVariantFromFileTimeVector(array, (uint)array.Length, this);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000026DD File Offset: 0x000008DD
		public PropVariant(bool value)
		{
			this._valueType = 11;
			this._int32 = (value ? -1 : 0);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000026FC File Offset: 0x000008FC
		public PropVariant(DateTime value)
		{
			this._valueType = 64;
			System.Runtime.InteropServices.ComTypes.FILETIME filetime = PropVariant.DateTimeToFileTime(value);
			PropVariantNativeMethods.InitPropVariantFromFileTime(ref filetime, this);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002726 File Offset: 0x00000926
		public PropVariant(byte value)
		{
			this._valueType = 17;
			this._byte = value;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000273D File Offset: 0x0000093D
		public PropVariant(sbyte value)
		{
			this._valueType = 16;
			this._sbyte = value;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002754 File Offset: 0x00000954
		public PropVariant(short value)
		{
			this._valueType = 2;
			this._short = value;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000276A File Offset: 0x0000096A
		public PropVariant(ushort value)
		{
			this._valueType = 18;
			this._ushort = value;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002781 File Offset: 0x00000981
		public PropVariant(int value)
		{
			this._valueType = 3;
			this._int32 = value;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002797 File Offset: 0x00000997
		public PropVariant(uint value)
		{
			this._valueType = 19;
			this._uint32 = value;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000027AE File Offset: 0x000009AE
		public PropVariant(decimal value)
		{
			this._decimal = value;
			this._valueType = 14;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000027C8 File Offset: 0x000009C8
		public PropVariant(decimal[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this._valueType = 4110;
			this._int32 = value.Length;
			this._ptr2 = Marshal.AllocCoTaskMem(value.Length * 16);
			for (int i = 0; i < value.Length; i++)
			{
				int[] bits = decimal.GetBits(value[i]);
				Marshal.Copy(bits, 0, this._ptr2, bits.Length);
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002839 File Offset: 0x00000A39
		public PropVariant(float value)
		{
			this._valueType = 4;
			this._float = value;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002850 File Offset: 0x00000A50
		public PropVariant(float[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this._valueType = 4100;
			this._int32 = value.Length;
			this._ptr2 = Marshal.AllocCoTaskMem(value.Length * 4);
			Marshal.Copy(value, 0, this._ptr2, value.Length);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000028A5 File Offset: 0x00000AA5
		public PropVariant(long value)
		{
			this._long = value;
			this._valueType = 20;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000028BC File Offset: 0x00000ABC
		public PropVariant(ulong value)
		{
			this._valueType = 21;
			this._ulong = value;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000028D3 File Offset: 0x00000AD3
		public PropVariant(double value)
		{
			this._valueType = 5;
			this._double = value;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000028E9 File Offset: 0x00000AE9
		internal void SetIUnknown(object value)
		{
			this._valueType = 13;
			this._ptr = Marshal.GetIUnknownForObject(value);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002900 File Offset: 0x00000B00
		internal void SetSafeArray(Array array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			IntPtr intPtr = PropVariantNativeMethods.SafeArrayCreateVector(13, 0, (uint)array.Length);
			IntPtr ptr = PropVariantNativeMethods.SafeArrayAccessData(intPtr);
			try
			{
				for (int i = 0; i < array.Length; i++)
				{
					object value = array.GetValue(i);
					IntPtr val = (value != null) ? Marshal.GetIUnknownForObject(value) : IntPtr.Zero;
					Marshal.WriteIntPtr(ptr, i * IntPtr.Size, val);
				}
			}
			finally
			{
				PropVariantNativeMethods.SafeArrayUnaccessData(intPtr);
			}
			this._valueType = 8205;
			this._ptr = intPtr;
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002998 File Offset: 0x00000B98
		// (set) Token: 0x0600003D RID: 61 RVA: 0x000029A0 File Offset: 0x00000BA0
		public VarEnum VarType
		{
			get
			{
				return (VarEnum)this._valueType;
			}
			set
			{
				this._valueType = (ushort)value;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600003E RID: 62 RVA: 0x000029AA File Offset: 0x00000BAA
		public bool IsNullOrEmpty
		{
			get
			{
				return this._valueType == 0 || this._valueType == 1;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600003F RID: 63 RVA: 0x000029C0 File Offset: 0x00000BC0
		public object Value
		{
			get
			{
				VarEnum valueType = (VarEnum)this._valueType;
				if (valueType <= (VarEnum)4101)
				{
					if (valueType <= VarEnum.VT_FILETIME)
					{
						switch (valueType)
						{
						case VarEnum.VT_I2:
							return this._short;
						case VarEnum.VT_I4:
						case VarEnum.VT_INT:
							return this._int32;
						case VarEnum.VT_R4:
							return this._float;
						case VarEnum.VT_R8:
							return this._double;
						case VarEnum.VT_CY:
							return this._decimal;
						case VarEnum.VT_DATE:
							return DateTime.FromOADate(this._double);
						case VarEnum.VT_BSTR:
							return Marshal.PtrToStringBSTR(this._ptr);
						case VarEnum.VT_DISPATCH:
							return Marshal.GetObjectForIUnknown(this._ptr);
						case VarEnum.VT_ERROR:
							return this._long;
						case VarEnum.VT_BOOL:
							return this._int32 == -1;
						case VarEnum.VT_VARIANT:
						case (VarEnum)15:
						case VarEnum.VT_VOID:
						case VarEnum.VT_HRESULT:
						case VarEnum.VT_PTR:
						case VarEnum.VT_SAFEARRAY:
						case VarEnum.VT_CARRAY:
						case VarEnum.VT_USERDEFINED:
							break;
						case VarEnum.VT_UNKNOWN:
							return Marshal.GetObjectForIUnknown(this._ptr);
						case VarEnum.VT_DECIMAL:
							return this._decimal;
						case VarEnum.VT_I1:
							return this._sbyte;
						case VarEnum.VT_UI1:
							return this._byte;
						case VarEnum.VT_UI2:
							return this._ushort;
						case VarEnum.VT_UI4:
						case VarEnum.VT_UINT:
							return this._uint32;
						case VarEnum.VT_I8:
							return this._long;
						case VarEnum.VT_UI8:
							return this._ulong;
						case VarEnum.VT_LPSTR:
							return Marshal.PtrToStringAnsi(this._ptr);
						case VarEnum.VT_LPWSTR:
							return Marshal.PtrToStringUni(this._ptr);
						default:
							if (valueType == VarEnum.VT_FILETIME)
							{
								return DateTime.FromFileTime(this._long);
							}
							break;
						}
					}
					else
					{
						if (valueType == VarEnum.VT_BLOB)
						{
							return this.GetBlobData();
						}
						switch (valueType)
						{
						case (VarEnum)4098:
							return this.GetVector<short>();
						case (VarEnum)4099:
							return this.GetVector<int>();
						case (VarEnum)4100:
							return this.GetVector<float>();
						case (VarEnum)4101:
							return this.GetVector<double>();
						}
					}
				}
				else if (valueType <= (VarEnum)4127)
				{
					switch (valueType)
					{
					case (VarEnum)4107:
						return this.GetVector<bool>();
					case (VarEnum)4108:
					case (VarEnum)4109:
					case (VarEnum)4111:
					case (VarEnum)4112:
					case (VarEnum)4113:
						break;
					case (VarEnum)4110:
						return this.GetVector<decimal>();
					case (VarEnum)4114:
						return this.GetVector<ushort>();
					case (VarEnum)4115:
						return this.GetVector<uint>();
					case (VarEnum)4116:
						return this.GetVector<long>();
					case (VarEnum)4117:
						return this.GetVector<ulong>();
					default:
						if (valueType == (VarEnum)4127)
						{
							return this.GetVector<string>();
						}
						break;
					}
				}
				else
				{
					if (valueType == (VarEnum)4160)
					{
						return this.GetVector<DateTime>();
					}
					if (valueType == (VarEnum)8205)
					{
						return PropVariant.CrackSingleDimSafeArray(this._ptr);
					}
				}
				return null;
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002C79 File Offset: 0x00000E79
		private static long GetFileTimeAsLong(ref System.Runtime.InteropServices.ComTypes.FILETIME val)
		{
			return ((long)val.dwHighDateTime << 32) + (long)val.dwLowDateTime;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002C90 File Offset: 0x00000E90
		private static System.Runtime.InteropServices.ComTypes.FILETIME DateTimeToFileTime(DateTime value)
		{
			long num = value.ToFileTime();
			return new System.Runtime.InteropServices.ComTypes.FILETIME
			{
				dwLowDateTime = (int)(num & (long)((ulong)-1)),
				dwHighDateTime = (int)(num >> 32)
			};
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002CC8 File Offset: 0x00000EC8
		private object GetBlobData()
		{
			byte[] array = new byte[this._int32];
			Marshal.Copy(this._ptr2, array, 0, this._int32);
			return array;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002CF8 File Offset: 0x00000EF8
		private Array GetVector<T>()
		{
			int num = PropVariantNativeMethods.PropVariantGetElementCount(this);
			if (num <= 0)
			{
				return null;
			}
			object padlock = PropVariant._padlock;
			lock (padlock)
			{
				if (PropVariant._vectorActions == null)
				{
					PropVariant._vectorActions = PropVariant.GenerateVectorActions();
				}
			}
			Action<PropVariant, Array, uint> action;
			if (!PropVariant._vectorActions.TryGetValue(typeof(T), out action))
			{
				throw new InvalidCastException(LocalizedMessages.PropVariantUnsupportedType);
			}
			Array array = new T[num];
			uint num2 = 0U;
			while ((ulong)num2 < (ulong)((long)num))
			{
				action(this, array, num2);
				num2 += 1U;
			}
			return array;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002D98 File Offset: 0x00000F98
		private static Array CrackSingleDimSafeArray(IntPtr psa)
		{
			if (PropVariantNativeMethods.SafeArrayGetDim(psa) != 1U)
			{
				throw new ArgumentException(LocalizedMessages.PropVariantMultiDimArray, "psa");
			}
			int num = PropVariantNativeMethods.SafeArrayGetLBound(psa, 1U);
			int num2 = PropVariantNativeMethods.SafeArrayGetUBound(psa, 1U);
			object[] array = new object[num2 - num + 1];
			for (int i = num; i <= num2; i++)
			{
				array[i] = PropVariantNativeMethods.SafeArrayGetElement(psa, ref i);
			}
			return array;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002DF1 File Offset: 0x00000FF1
		public void Dispose()
		{
			PropVariantNativeMethods.PropVariantClear(this);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002E00 File Offset: 0x00001000
		~PropVariant()
		{
			this.Dispose();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002E2C File Offset: 0x0000102C
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", this.Value, this.VarType.ToString());
		}

		// Token: 0x040000EF RID: 239
		private static Dictionary<Type, Action<PropVariant, Array, uint>> _vectorActions = null;

		// Token: 0x040000F0 RID: 240
		private static Dictionary<Type, Func<object, PropVariant>> _cache = new Dictionary<Type, Func<object, PropVariant>>();

		// Token: 0x040000F1 RID: 241
		private static object _padlock = new object();

		// Token: 0x040000F2 RID: 242
		[FieldOffset(0)]
		private decimal _decimal;

		// Token: 0x040000F3 RID: 243
		[FieldOffset(0)]
		private ushort _valueType;

		// Token: 0x040000F4 RID: 244
		[FieldOffset(12)]
		private IntPtr _ptr2;

		// Token: 0x040000F5 RID: 245
		[FieldOffset(8)]
		private IntPtr _ptr;

		// Token: 0x040000F6 RID: 246
		[FieldOffset(8)]
		private int _int32;

		// Token: 0x040000F7 RID: 247
		[FieldOffset(8)]
		private uint _uint32;

		// Token: 0x040000F8 RID: 248
		[FieldOffset(8)]
		private byte _byte;

		// Token: 0x040000F9 RID: 249
		[FieldOffset(8)]
		private sbyte _sbyte;

		// Token: 0x040000FA RID: 250
		[FieldOffset(8)]
		private short _short;

		// Token: 0x040000FB RID: 251
		[FieldOffset(8)]
		private ushort _ushort;

		// Token: 0x040000FC RID: 252
		[FieldOffset(8)]
		private long _long;

		// Token: 0x040000FD RID: 253
		[FieldOffset(8)]
		private ulong _ulong;

		// Token: 0x040000FE RID: 254
		[FieldOffset(8)]
		private double _double;

		// Token: 0x040000FF RID: 255
		[FieldOffset(8)]
		private float _float;
	}
}
