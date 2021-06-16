using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.PowerShell;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A6B RID: 2667
	internal static class ComRuntimeHelpers
	{
		// Token: 0x06006A3C RID: 27196 RVA: 0x00216258 File Offset: 0x00214458
		public static void CheckThrowException(int hresult, ref ExcepInfo excepInfo, ComMethodDesc method, object[] args, uint argErr)
		{
			if (Utils.Succeeded(hresult))
			{
				return;
			}
			switch (hresult)
			{
			case -2147352573:
				throw Error.DispMemberNotFound(method.Name);
			case -2147352571:
			{
				argErr = (uint)(args.Length - (int)argErr - 2);
				Type type;
				if ((ulong)argErr >= (ulong)((long)method.ParameterInformation.Length))
				{
					type = method.InputType;
				}
				else
				{
					type = method.ParameterInformation[(int)((UIntPtr)argErr)].parameterType;
				}
				object obj = args[(int)((UIntPtr)(argErr + 1U))];
				if (method.IsPropertyPut || method.IsPropertyPutRef)
				{
					type = method.InputType;
					obj = args[args.Length - 1];
				}
				string value = obj.ToString();
				string originalTypeName = ToStringCodeMethods.Type(obj.GetType(), true);
				if (type == typeof(object) && method.ParameterInformation[(int)((UIntPtr)argErr)].isByRef)
				{
					type = typeof(PSReference);
				}
				string destinationTypeName = ToStringCodeMethods.Type(type, true);
				Exception parameterException = Error.DispTypeMismatch(method.Name, value, originalTypeName, destinationTypeName);
				ComRuntimeHelpers.ThrowWrappedInvocationException(method, parameterException);
				break;
			}
			case -2147352569:
				throw Error.DispNoNamedArgs(method.Name);
			case -2147352567:
				throw excepInfo.GetException();
			case -2147352566:
				throw Error.DispOverflow(method.Name);
			case -2147352562:
			{
				Exception parameterException = Error.DispBadParamCount(method.Name, args.Length - 1);
				ComRuntimeHelpers.ThrowWrappedInvocationException(method, parameterException);
				break;
			}
			case -2147352561:
				throw Error.DispParamNotOptional(method.Name);
			}
			Marshal.ThrowExceptionForHR(hresult);
		}

		// Token: 0x06006A3D RID: 27197 RVA: 0x002163DC File Offset: 0x002145DC
		private static void ThrowWrappedInvocationException(ComMethodDesc method, Exception parameterException)
		{
			if ((method.InvokeKind & System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_FUNC) == System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_FUNC)
			{
				throw new MethodException(parameterException.Message, parameterException);
			}
			if (method.IsPropertyGet)
			{
				throw new GetValueInvocationException(parameterException.Message, parameterException);
			}
			if (method.IsPropertyPut || method.IsPropertyPutRef)
			{
				throw new SetValueInvocationException(parameterException.Message, parameterException);
			}
			throw parameterException;
		}

		// Token: 0x06006A3E RID: 27198 RVA: 0x00216434 File Offset: 0x00214634
		internal static void GetInfoFromType(ITypeInfo typeInfo, out string name, out string documentation)
		{
			int num;
			string text;
			typeInfo.GetDocumentation(-1, out name, out documentation, out num, out text);
		}

		// Token: 0x06006A3F RID: 27199 RVA: 0x00216450 File Offset: 0x00214650
		internal static string GetNameOfMethod(ITypeInfo typeInfo, int memid)
		{
			string[] array = new string[1];
			int num;
			typeInfo.GetNames(memid, array, 1, out num);
			return array[0];
		}

		// Token: 0x06006A40 RID: 27200 RVA: 0x00216474 File Offset: 0x00214674
		internal static string GetNameOfLib(ITypeLib typeLib)
		{
			string result;
			string text;
			int num;
			string text2;
			typeLib.GetDocumentation(-1, out result, out text, out num, out text2);
			return result;
		}

		// Token: 0x06006A41 RID: 27201 RVA: 0x00216494 File Offset: 0x00214694
		internal static string GetNameOfType(ITypeInfo typeInfo)
		{
			string result;
			string text;
			ComRuntimeHelpers.GetInfoFromType(typeInfo, out result, out text);
			return result;
		}

		// Token: 0x06006A42 RID: 27202 RVA: 0x002164AC File Offset: 0x002146AC
		internal static ITypeInfo GetITypeInfoFromIDispatch(IDispatch dispatch, bool throwIfMissingExpectedTypeInfo)
		{
			uint num2;
			int num = dispatch.TryGetTypeInfoCount(out num2);
			if (num == -2147467263 || num == -2147467262)
			{
				return null;
			}
			Marshal.ThrowExceptionForHR(num);
			if (num2 == 0U)
			{
				return null;
			}
			IntPtr zero = IntPtr.Zero;
			num = dispatch.TryGetTypeInfo(0U, 0, out zero);
			if (!Utils.Succeeded(num))
			{
				ComRuntimeHelpers.CheckIfMissingTypeInfoIsExpected(num, throwIfMissingExpectedTypeInfo);
				return null;
			}
			if (zero == IntPtr.Zero)
			{
				if (throwIfMissingExpectedTypeInfo)
				{
					Marshal.ThrowExceptionForHR(-2147467259);
				}
				return null;
			}
			ITypeInfo result = null;
			try
			{
				result = (Marshal.GetObjectForIUnknown(zero) as ITypeInfo);
			}
			finally
			{
				Marshal.Release(zero);
			}
			return result;
		}

		// Token: 0x06006A43 RID: 27203 RVA: 0x00216548 File Offset: 0x00214748
		private static void CheckIfMissingTypeInfoIsExpected(int hresult, bool throwIfMissingExpectedTypeInfo)
		{
			if (hresult == -2147467262)
			{
				return;
			}
			if (throwIfMissingExpectedTypeInfo)
			{
				Marshal.ThrowExceptionForHR(hresult);
			}
		}

		// Token: 0x06006A44 RID: 27204 RVA: 0x0021655C File Offset: 0x0021475C
		internal static System.Runtime.InteropServices.ComTypes.TYPEATTR GetTypeAttrForTypeInfo(ITypeInfo typeInfo)
		{
			IntPtr zero = IntPtr.Zero;
			typeInfo.GetTypeAttr(out zero);
			if (zero == IntPtr.Zero)
			{
				throw Error.CannotRetrieveTypeInformation();
			}
			System.Runtime.InteropServices.ComTypes.TYPEATTR result;
			try
			{
				result = (System.Runtime.InteropServices.ComTypes.TYPEATTR)Marshal.PtrToStructure(zero, typeof(System.Runtime.InteropServices.ComTypes.TYPEATTR));
			}
			finally
			{
				typeInfo.ReleaseTypeAttr(zero);
			}
			return result;
		}

		// Token: 0x06006A45 RID: 27205 RVA: 0x002165BC File Offset: 0x002147BC
		internal static System.Runtime.InteropServices.ComTypes.TYPELIBATTR GetTypeAttrForTypeLib(ITypeLib typeLib)
		{
			IntPtr zero = IntPtr.Zero;
			typeLib.GetLibAttr(out zero);
			if (zero == IntPtr.Zero)
			{
				throw Error.CannotRetrieveTypeInformation();
			}
			System.Runtime.InteropServices.ComTypes.TYPELIBATTR result;
			try
			{
				result = (System.Runtime.InteropServices.ComTypes.TYPELIBATTR)Marshal.PtrToStructure(zero, typeof(System.Runtime.InteropServices.ComTypes.TYPELIBATTR));
			}
			finally
			{
				typeLib.ReleaseTLibAttr(zero);
			}
			return result;
		}

		// Token: 0x06006A46 RID: 27206 RVA: 0x0021661C File Offset: 0x0021481C
		public static BoundDispEvent CreateComEvent(object rcw, Guid sourceIid, int dispid)
		{
			return new BoundDispEvent(rcw, sourceIid, dispid);
		}

		// Token: 0x06006A47 RID: 27207 RVA: 0x00216626 File Offset: 0x00214826
		public static DispCallable CreateDispCallable(IDispatchComObject dispatch, ComMethodDesc method)
		{
			return new DispCallable(dispatch, method.Name, method.DispId);
		}
	}
}
