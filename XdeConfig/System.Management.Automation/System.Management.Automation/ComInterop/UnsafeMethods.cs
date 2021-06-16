using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Security.Permissions;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A6C RID: 2668
	internal static class UnsafeMethods
	{
		// Token: 0x06006A48 RID: 27208
		[DllImport("oleaut32.dll", PreserveSig = false)]
		internal static extern void VariantClear(IntPtr variant);

		// Token: 0x06006A49 RID: 27209
		[DllImport("oleaut32.dll", PreserveSig = false)]
		internal static extern ITypeLib LoadRegTypeLib(ref Guid clsid, short majorVersion, short minorVersion, int lcid);

		// Token: 0x06006A4A RID: 27210 RVA: 0x0021663C File Offset: 0x0021483C
		private static MethodInfo Create_ConvertByrefToPtr()
		{
			AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("ComSnippets"), AssemblyBuilderAccess.Run);
			ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("ComSnippets");
			TypeBuilder typeBuilder = moduleBuilder.DefineType("Type$ConvertByrefToPtr", TypeAttributes.Public);
			Type[] parameterTypes = new Type[]
			{
				typeof(Variant).MakeByRefType()
			};
			MethodBuilder methodBuilder = typeBuilder.DefineMethod("ConvertByrefToPtr", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, typeof(IntPtr), parameterTypes);
			GenericTypeParameterBuilder[] array = methodBuilder.DefineGenericParameters(new string[]
			{
				"T"
			});
			array[0].SetGenericParameterAttributes(GenericParameterAttributes.NotNullableValueTypeConstraint);
			methodBuilder.SetSignature(typeof(IntPtr), null, null, new Type[]
			{
				array[0].MakeByRefType()
			}, null, null);
			ILGenerator ilgenerator = methodBuilder.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Conv_I);
			ilgenerator.Emit(OpCodes.Ret);
			return typeBuilder.CreateType().GetMethod("ConvertByrefToPtr");
		}

		// Token: 0x06006A4B RID: 27211 RVA: 0x0021673F File Offset: 0x0021493F
		public static IntPtr ConvertSByteByrefToPtr(ref sbyte value)
		{
			return UnsafeMethods._ConvertSByteByrefToPtr(ref value);
		}

		// Token: 0x06006A4C RID: 27212 RVA: 0x0021674C File Offset: 0x0021494C
		public static IntPtr ConvertInt16ByrefToPtr(ref short value)
		{
			return UnsafeMethods._ConvertInt16ByrefToPtr(ref value);
		}

		// Token: 0x06006A4D RID: 27213 RVA: 0x00216759 File Offset: 0x00214959
		public static IntPtr ConvertInt32ByrefToPtr(ref int value)
		{
			return UnsafeMethods._ConvertInt32ByrefToPtr(ref value);
		}

		// Token: 0x06006A4E RID: 27214 RVA: 0x00216766 File Offset: 0x00214966
		public static IntPtr ConvertInt64ByrefToPtr(ref long value)
		{
			return UnsafeMethods._ConvertInt64ByrefToPtr(ref value);
		}

		// Token: 0x06006A4F RID: 27215 RVA: 0x00216773 File Offset: 0x00214973
		public static IntPtr ConvertByteByrefToPtr(ref byte value)
		{
			return UnsafeMethods._ConvertByteByrefToPtr(ref value);
		}

		// Token: 0x06006A50 RID: 27216 RVA: 0x00216780 File Offset: 0x00214980
		public static IntPtr ConvertUInt16ByrefToPtr(ref ushort value)
		{
			return UnsafeMethods._ConvertUInt16ByrefToPtr(ref value);
		}

		// Token: 0x06006A51 RID: 27217 RVA: 0x0021678D File Offset: 0x0021498D
		public static IntPtr ConvertUInt32ByrefToPtr(ref uint value)
		{
			return UnsafeMethods._ConvertUInt32ByrefToPtr(ref value);
		}

		// Token: 0x06006A52 RID: 27218 RVA: 0x0021679A File Offset: 0x0021499A
		public static IntPtr ConvertUInt64ByrefToPtr(ref ulong value)
		{
			return UnsafeMethods._ConvertUInt64ByrefToPtr(ref value);
		}

		// Token: 0x06006A53 RID: 27219 RVA: 0x002167A7 File Offset: 0x002149A7
		public static IntPtr ConvertIntPtrByrefToPtr(ref IntPtr value)
		{
			return UnsafeMethods._ConvertIntPtrByrefToPtr(ref value);
		}

		// Token: 0x06006A54 RID: 27220 RVA: 0x002167B4 File Offset: 0x002149B4
		public static IntPtr ConvertUIntPtrByrefToPtr(ref UIntPtr value)
		{
			return UnsafeMethods._ConvertUIntPtrByrefToPtr(ref value);
		}

		// Token: 0x06006A55 RID: 27221 RVA: 0x002167C1 File Offset: 0x002149C1
		public static IntPtr ConvertSingleByrefToPtr(ref float value)
		{
			return UnsafeMethods._ConvertSingleByrefToPtr(ref value);
		}

		// Token: 0x06006A56 RID: 27222 RVA: 0x002167CE File Offset: 0x002149CE
		public static IntPtr ConvertDoubleByrefToPtr(ref double value)
		{
			return UnsafeMethods._ConvertDoubleByrefToPtr(ref value);
		}

		// Token: 0x06006A57 RID: 27223 RVA: 0x002167DB File Offset: 0x002149DB
		public static IntPtr ConvertDecimalByrefToPtr(ref decimal value)
		{
			return UnsafeMethods._ConvertDecimalByrefToPtr(ref value);
		}

		// Token: 0x06006A58 RID: 27224 RVA: 0x002167E8 File Offset: 0x002149E8
		public static IntPtr ConvertVariantByrefToPtr(ref Variant value)
		{
			return UnsafeMethods._ConvertVariantByrefToPtr(ref value);
		}

		// Token: 0x06006A59 RID: 27225 RVA: 0x002167F8 File Offset: 0x002149F8
		internal static Variant GetVariantForObject(object obj)
		{
			Variant result = default(Variant);
			if (obj == null)
			{
				return result;
			}
			UnsafeMethods.InitVariantForObject(obj, ref result);
			return result;
		}

		// Token: 0x06006A5A RID: 27226 RVA: 0x0021681C File Offset: 0x00214A1C
		internal static void InitVariantForObject(object obj, ref Variant variant)
		{
			IDispatch dispatch = obj as IDispatch;
			if (dispatch != null)
			{
				variant.AsDispatch = obj;
				return;
			}
			Marshal.GetNativeVariantForObject(obj, UnsafeMethods.ConvertVariantByrefToPtr(ref variant));
		}

		// Token: 0x06006A5B RID: 27227 RVA: 0x00216848 File Offset: 0x00214A48
		[Obsolete("do not use this method", true)]
		public static object GetObjectForVariant(Variant variant)
		{
			IntPtr pSrcNativeVariant = UnsafeMethods.ConvertVariantByrefToPtr(ref variant);
			return Marshal.GetObjectForNativeVariant(pSrcNativeVariant);
		}

		// Token: 0x06006A5C RID: 27228 RVA: 0x00216863 File Offset: 0x00214A63
		[Obsolete("do not use this method", true)]
		public static int IUnknownRelease(IntPtr interfacePointer)
		{
			return UnsafeMethods._IUnknownRelease(interfacePointer);
		}

		// Token: 0x06006A5D RID: 27229 RVA: 0x00216870 File Offset: 0x00214A70
		[Obsolete("do not use this method", true)]
		public static void IUnknownReleaseNotZero(IntPtr interfacePointer)
		{
			if (interfacePointer != IntPtr.Zero)
			{
				UnsafeMethods.IUnknownRelease(interfacePointer);
			}
		}

		// Token: 0x06006A5E RID: 27230 RVA: 0x00216888 File Offset: 0x00214A88
		[Obsolete("do not use this method", true)]
		public static int IDispatchInvoke(IntPtr dispatchPointer, int memberDispId, System.Runtime.InteropServices.ComTypes.INVOKEKIND flags, ref System.Runtime.InteropServices.ComTypes.DISPPARAMS dispParams, out Variant result, out ExcepInfo excepInfo, out uint argErr)
		{
			int num = UnsafeMethods._IDispatchInvoke(dispatchPointer, memberDispId, flags, ref dispParams, out result, out excepInfo, out argErr);
			if (num == -2147352573 && (flags & System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_FUNC) != (System.Runtime.InteropServices.ComTypes.INVOKEKIND)0 && (flags & (System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYPUT | System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYPUTREF)) == (System.Runtime.InteropServices.ComTypes.INVOKEKIND)0)
			{
				num = UnsafeMethods._IDispatchInvokeNoResult(dispatchPointer, memberDispId, System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_FUNC, ref dispParams, out result, out excepInfo, out argErr);
			}
			return num;
		}

		// Token: 0x06006A5F RID: 27231 RVA: 0x002168D4 File Offset: 0x00214AD4
		[Obsolete("do not use this method", true)]
		public static IntPtr GetIdsOfNamedParameters(IDispatch dispatch, string[] names, int methodDispId, out GCHandle pinningHandle)
		{
			pinningHandle = GCHandle.Alloc(null, GCHandleType.Pinned);
			int[] array = new int[names.Length];
			Guid empty = Guid.Empty;
			int num = dispatch.TryGetIDsOfNames(ref empty, names, (uint)names.Length, 0, array);
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			if (methodDispId != array[0])
			{
				throw Error.GetIDsOfNamesInvalid(names[0]);
			}
			int[] array2 = array.RemoveFirst<int>();
			pinningHandle.Target = array2;
			return Marshal.UnsafeAddrOfPinnedArrayElement(array2, 0);
		}

		// Token: 0x06006A61 RID: 27233 RVA: 0x00216CAC File Offset: 0x00214EAC
		private static void EmitLoadArg(ILGenerator il, int index)
		{
			switch (index)
			{
			case 0:
				il.Emit(OpCodes.Ldarg_0);
				return;
			case 1:
				il.Emit(OpCodes.Ldarg_1);
				return;
			case 2:
				il.Emit(OpCodes.Ldarg_2);
				return;
			case 3:
				il.Emit(OpCodes.Ldarg_3);
				return;
			default:
				if (index <= 255)
				{
					il.Emit(OpCodes.Ldarg_S, (byte)index);
					return;
				}
				il.Emit(OpCodes.Ldarg, index);
				return;
			}
		}

		// Token: 0x06006A62 RID: 27234 RVA: 0x00216D28 File Offset: 0x00214F28
		[Conditional("DEBUG")]
		public static void AssertByrefPointsToStack(IntPtr ptr)
		{
			if (Marshal.ReadInt32(ptr) == 269488144)
			{
				return;
			}
			int num = 269488144;
			UnsafeMethods.ConvertInt32ByrefToPtr(ref num);
		}

		// Token: 0x17001DAF RID: 7599
		// (get) Token: 0x06006A63 RID: 27235 RVA: 0x00216D54 File Offset: 0x00214F54
		internal static ModuleBuilder DynamicModule
		{
			get
			{
				if (UnsafeMethods._dynamicModule != null)
				{
					return UnsafeMethods._dynamicModule;
				}
				ModuleBuilder dynamicModule;
				lock (UnsafeMethods._lock)
				{
					if (UnsafeMethods._dynamicModule == null)
					{
						CustomAttributeBuilder[] assemblyAttributes = new CustomAttributeBuilder[]
						{
							new CustomAttributeBuilder(typeof(UnverifiableCodeAttribute).GetConstructor(Type.EmptyTypes), new object[0]),
							new CustomAttributeBuilder(typeof(PermissionSetAttribute).GetConstructor(new Type[]
							{
								typeof(SecurityAction)
							}), new object[]
							{
								SecurityAction.Demand
							}, new PropertyInfo[]
							{
								typeof(PermissionSetAttribute).GetProperty("Unrestricted")
							}, new object[]
							{
								true
							})
						};
						string text = typeof(VariantArray).Namespace + ".DynamicAssembly";
						AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(text), AssemblyBuilderAccess.Run, assemblyAttributes);
						assemblyBuilder.DefineVersionInfoResource();
						UnsafeMethods._dynamicModule = assemblyBuilder.DefineDynamicModule(text);
					}
					dynamicModule = UnsafeMethods._dynamicModule;
				}
				return dynamicModule;
			}
		}

		// Token: 0x06006A64 RID: 27236 RVA: 0x00216EB0 File Offset: 0x002150B0
		private static UnsafeMethods.IUnknownReleaseDelegate Create_IUnknownRelease()
		{
			DynamicMethod dynamicMethod = new DynamicMethod("IUnknownRelease", typeof(int), new Type[]
			{
				typeof(IntPtr)
			}, UnsafeMethods.DynamicModule);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			int arg = 2 * Marshal.SizeOf(typeof(IntPtr));
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldind_I);
			ilgenerator.Emit(OpCodes.Ldc_I4, arg);
			ilgenerator.Emit(OpCodes.Add);
			ilgenerator.Emit(OpCodes.Ldind_I);
			SignatureHelper methodSigHelper = SignatureHelper.GetMethodSigHelper(CallingConvention.Winapi, typeof(int));
			methodSigHelper.AddArgument(typeof(IntPtr));
			ilgenerator.Emit(OpCodes.Calli, methodSigHelper);
			ilgenerator.Emit(OpCodes.Ret);
			return (UnsafeMethods.IUnknownReleaseDelegate)dynamicMethod.CreateDelegate(typeof(UnsafeMethods.IUnknownReleaseDelegate));
		}

		// Token: 0x06006A65 RID: 27237 RVA: 0x00216F98 File Offset: 0x00215198
		private static IntPtr GetNullInterfaceId()
		{
			int num = Marshal.SizeOf(Guid.Empty);
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			for (int i = 0; i < num; i++)
			{
				Marshal.WriteByte(intPtr, i, 0);
			}
			return intPtr;
		}

		// Token: 0x17001DB0 RID: 7600
		// (get) Token: 0x06006A66 RID: 27238 RVA: 0x00216FD4 File Offset: 0x002151D4
		private static UnsafeMethods.IDispatchInvokeDelegate _IDispatchInvokeNoResult
		{
			get
			{
				if (UnsafeMethods._IDispatchInvokeNoResultImpl == null)
				{
					lock (UnsafeMethods._IDispatchInvoke)
					{
						if (UnsafeMethods._IDispatchInvokeNoResultImpl == null)
						{
							UnsafeMethods._IDispatchInvokeNoResultImpl = UnsafeMethods.Create_IDispatchInvoke(false);
						}
					}
				}
				return UnsafeMethods._IDispatchInvokeNoResultImpl;
			}
		}

		// Token: 0x06006A67 RID: 27239 RVA: 0x0021702C File Offset: 0x0021522C
		private static UnsafeMethods.IDispatchInvokeDelegate Create_IDispatchInvoke(bool returnResult)
		{
			Type[] parameterTypes = new Type[]
			{
				typeof(IntPtr),
				typeof(int),
				typeof(System.Runtime.InteropServices.ComTypes.INVOKEKIND),
				typeof(System.Runtime.InteropServices.ComTypes.DISPPARAMS).MakeByRefType(),
				typeof(Variant).MakeByRefType(),
				typeof(ExcepInfo).MakeByRefType(),
				typeof(uint).MakeByRefType()
			};
			DynamicMethod dynamicMethod = new DynamicMethod("IDispatchInvoke", typeof(int), parameterTypes, UnsafeMethods.DynamicModule);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			UnsafeMethods.EmitLoadArg(ilgenerator, 0);
			UnsafeMethods.EmitLoadArg(ilgenerator, 1);
			if (IntPtr.Size == 4)
			{
				ilgenerator.Emit(OpCodes.Ldc_I4, UnsafeMethods.NullInterfaceId.ToInt32());
			}
			else
			{
				ilgenerator.Emit(OpCodes.Ldc_I8, UnsafeMethods.NullInterfaceId.ToInt64());
			}
			ilgenerator.Emit(OpCodes.Conv_I);
			ilgenerator.Emit(OpCodes.Ldc_I4_0);
			UnsafeMethods.EmitLoadArg(ilgenerator, 2);
			UnsafeMethods.EmitLoadArg(ilgenerator, 3);
			if (returnResult)
			{
				UnsafeMethods.EmitLoadArg(ilgenerator, 4);
			}
			else
			{
				ilgenerator.Emit(OpCodes.Ldsfld, typeof(IntPtr).GetField("Zero"));
			}
			UnsafeMethods.EmitLoadArg(ilgenerator, 5);
			UnsafeMethods.EmitLoadArg(ilgenerator, 6);
			int arg = 6 * Marshal.SizeOf(typeof(IntPtr));
			UnsafeMethods.EmitLoadArg(ilgenerator, 0);
			ilgenerator.Emit(OpCodes.Ldind_I);
			ilgenerator.Emit(OpCodes.Ldc_I4, arg);
			ilgenerator.Emit(OpCodes.Add);
			ilgenerator.Emit(OpCodes.Ldind_I);
			SignatureHelper methodSigHelper = SignatureHelper.GetMethodSigHelper(CallingConvention.Winapi, typeof(int));
			Type[] arguments = new Type[]
			{
				typeof(IntPtr),
				typeof(int),
				typeof(IntPtr),
				typeof(int),
				typeof(ushort),
				typeof(IntPtr),
				typeof(IntPtr),
				typeof(IntPtr),
				typeof(IntPtr)
			};
			methodSigHelper.AddArguments(arguments, null, null);
			ilgenerator.Emit(OpCodes.Calli, methodSigHelper);
			ilgenerator.Emit(OpCodes.Ret);
			return (UnsafeMethods.IDispatchInvokeDelegate)dynamicMethod.CreateDelegate(typeof(UnsafeMethods.IDispatchInvokeDelegate));
		}

		// Token: 0x040032F1 RID: 13041
		private const int _dummyMarker = 269488144;

		// Token: 0x040032F2 RID: 13042
		private static readonly MethodInfo _ConvertByrefToPtr = UnsafeMethods.Create_ConvertByrefToPtr();

		// Token: 0x040032F3 RID: 13043
		private static readonly UnsafeMethods.ConvertByrefToPtrDelegate<Variant> _ConvertVariantByrefToPtr = (UnsafeMethods.ConvertByrefToPtrDelegate<Variant>)Delegate.CreateDelegate(typeof(UnsafeMethods.ConvertByrefToPtrDelegate<Variant>), UnsafeMethods._ConvertByrefToPtr.MakeGenericMethod(new Type[]
		{
			typeof(Variant)
		}));

		// Token: 0x040032F4 RID: 13044
		private static readonly UnsafeMethods.ConvertByrefToPtrDelegate<sbyte> _ConvertSByteByrefToPtr = (UnsafeMethods.ConvertByrefToPtrDelegate<sbyte>)Delegate.CreateDelegate(typeof(UnsafeMethods.ConvertByrefToPtrDelegate<sbyte>), UnsafeMethods._ConvertByrefToPtr.MakeGenericMethod(new Type[]
		{
			typeof(sbyte)
		}));

		// Token: 0x040032F5 RID: 13045
		private static readonly UnsafeMethods.ConvertByrefToPtrDelegate<short> _ConvertInt16ByrefToPtr = (UnsafeMethods.ConvertByrefToPtrDelegate<short>)Delegate.CreateDelegate(typeof(UnsafeMethods.ConvertByrefToPtrDelegate<short>), UnsafeMethods._ConvertByrefToPtr.MakeGenericMethod(new Type[]
		{
			typeof(short)
		}));

		// Token: 0x040032F6 RID: 13046
		private static readonly UnsafeMethods.ConvertByrefToPtrDelegate<int> _ConvertInt32ByrefToPtr = (UnsafeMethods.ConvertByrefToPtrDelegate<int>)Delegate.CreateDelegate(typeof(UnsafeMethods.ConvertByrefToPtrDelegate<int>), UnsafeMethods._ConvertByrefToPtr.MakeGenericMethod(new Type[]
		{
			typeof(int)
		}));

		// Token: 0x040032F7 RID: 13047
		private static readonly UnsafeMethods.ConvertByrefToPtrDelegate<long> _ConvertInt64ByrefToPtr = (UnsafeMethods.ConvertByrefToPtrDelegate<long>)Delegate.CreateDelegate(typeof(UnsafeMethods.ConvertByrefToPtrDelegate<long>), UnsafeMethods._ConvertByrefToPtr.MakeGenericMethod(new Type[]
		{
			typeof(long)
		}));

		// Token: 0x040032F8 RID: 13048
		private static readonly UnsafeMethods.ConvertByrefToPtrDelegate<byte> _ConvertByteByrefToPtr = (UnsafeMethods.ConvertByrefToPtrDelegate<byte>)Delegate.CreateDelegate(typeof(UnsafeMethods.ConvertByrefToPtrDelegate<byte>), UnsafeMethods._ConvertByrefToPtr.MakeGenericMethod(new Type[]
		{
			typeof(byte)
		}));

		// Token: 0x040032F9 RID: 13049
		private static readonly UnsafeMethods.ConvertByrefToPtrDelegate<ushort> _ConvertUInt16ByrefToPtr = (UnsafeMethods.ConvertByrefToPtrDelegate<ushort>)Delegate.CreateDelegate(typeof(UnsafeMethods.ConvertByrefToPtrDelegate<ushort>), UnsafeMethods._ConvertByrefToPtr.MakeGenericMethod(new Type[]
		{
			typeof(ushort)
		}));

		// Token: 0x040032FA RID: 13050
		private static readonly UnsafeMethods.ConvertByrefToPtrDelegate<uint> _ConvertUInt32ByrefToPtr = (UnsafeMethods.ConvertByrefToPtrDelegate<uint>)Delegate.CreateDelegate(typeof(UnsafeMethods.ConvertByrefToPtrDelegate<uint>), UnsafeMethods._ConvertByrefToPtr.MakeGenericMethod(new Type[]
		{
			typeof(uint)
		}));

		// Token: 0x040032FB RID: 13051
		private static readonly UnsafeMethods.ConvertByrefToPtrDelegate<ulong> _ConvertUInt64ByrefToPtr = (UnsafeMethods.ConvertByrefToPtrDelegate<ulong>)Delegate.CreateDelegate(typeof(UnsafeMethods.ConvertByrefToPtrDelegate<ulong>), UnsafeMethods._ConvertByrefToPtr.MakeGenericMethod(new Type[]
		{
			typeof(ulong)
		}));

		// Token: 0x040032FC RID: 13052
		private static readonly UnsafeMethods.ConvertByrefToPtrDelegate<IntPtr> _ConvertIntPtrByrefToPtr = (UnsafeMethods.ConvertByrefToPtrDelegate<IntPtr>)Delegate.CreateDelegate(typeof(UnsafeMethods.ConvertByrefToPtrDelegate<IntPtr>), UnsafeMethods._ConvertByrefToPtr.MakeGenericMethod(new Type[]
		{
			typeof(IntPtr)
		}));

		// Token: 0x040032FD RID: 13053
		private static readonly UnsafeMethods.ConvertByrefToPtrDelegate<UIntPtr> _ConvertUIntPtrByrefToPtr = (UnsafeMethods.ConvertByrefToPtrDelegate<UIntPtr>)Delegate.CreateDelegate(typeof(UnsafeMethods.ConvertByrefToPtrDelegate<UIntPtr>), UnsafeMethods._ConvertByrefToPtr.MakeGenericMethod(new Type[]
		{
			typeof(UIntPtr)
		}));

		// Token: 0x040032FE RID: 13054
		private static readonly UnsafeMethods.ConvertByrefToPtrDelegate<float> _ConvertSingleByrefToPtr = (UnsafeMethods.ConvertByrefToPtrDelegate<float>)Delegate.CreateDelegate(typeof(UnsafeMethods.ConvertByrefToPtrDelegate<float>), UnsafeMethods._ConvertByrefToPtr.MakeGenericMethod(new Type[]
		{
			typeof(float)
		}));

		// Token: 0x040032FF RID: 13055
		private static readonly UnsafeMethods.ConvertByrefToPtrDelegate<double> _ConvertDoubleByrefToPtr = (UnsafeMethods.ConvertByrefToPtrDelegate<double>)Delegate.CreateDelegate(typeof(UnsafeMethods.ConvertByrefToPtrDelegate<double>), UnsafeMethods._ConvertByrefToPtr.MakeGenericMethod(new Type[]
		{
			typeof(double)
		}));

		// Token: 0x04003300 RID: 13056
		private static readonly UnsafeMethods.ConvertByrefToPtrDelegate<decimal> _ConvertDecimalByrefToPtr = (UnsafeMethods.ConvertByrefToPtrDelegate<decimal>)Delegate.CreateDelegate(typeof(UnsafeMethods.ConvertByrefToPtrDelegate<decimal>), UnsafeMethods._ConvertByrefToPtr.MakeGenericMethod(new Type[]
		{
			typeof(decimal)
		}));

		// Token: 0x04003301 RID: 13057
		private static readonly object _lock = new object();

		// Token: 0x04003302 RID: 13058
		private static ModuleBuilder _dynamicModule;

		// Token: 0x04003303 RID: 13059
		private static readonly UnsafeMethods.IUnknownReleaseDelegate _IUnknownRelease = UnsafeMethods.Create_IUnknownRelease();

		// Token: 0x04003304 RID: 13060
		internal static readonly IntPtr NullInterfaceId = UnsafeMethods.GetNullInterfaceId();

		// Token: 0x04003305 RID: 13061
		private static readonly UnsafeMethods.IDispatchInvokeDelegate _IDispatchInvoke = UnsafeMethods.Create_IDispatchInvoke(true);

		// Token: 0x04003306 RID: 13062
		private static UnsafeMethods.IDispatchInvokeDelegate _IDispatchInvokeNoResultImpl;

		// Token: 0x02000A6D RID: 2669
		// (Invoke) Token: 0x06006A69 RID: 27241
		public delegate IntPtr ConvertByrefToPtrDelegate<T>(ref T value);

		// Token: 0x02000A6E RID: 2670
		// (Invoke) Token: 0x06006A6D RID: 27245
		private delegate int IUnknownReleaseDelegate(IntPtr interfacePointer);

		// Token: 0x02000A6F RID: 2671
		// (Invoke) Token: 0x06006A71 RID: 27249
		private delegate int IDispatchInvokeDelegate(IntPtr dispatchPointer, int memberDispId, System.Runtime.InteropServices.ComTypes.INVOKEKIND flags, ref System.Runtime.InteropServices.ComTypes.DISPPARAMS dispParams, out Variant result, out ExcepInfo excepInfo, out uint argErr);
	}
}
