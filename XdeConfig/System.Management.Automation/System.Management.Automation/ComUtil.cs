using System;
using System.Collections.ObjectModel;
using System.Management.Automation.ComInterop;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x02000183 RID: 387
	internal class ComUtil
	{
		// Token: 0x060012F2 RID: 4850 RVA: 0x00075B7C File Offset: 0x00073D7C
		internal static string GetMethodSignatureFromFuncDesc(ITypeInfo typeinfo, System.Runtime.InteropServices.ComTypes.FUNCDESC funcdesc, bool isPropertyPut)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string nameFromFuncDesc = ComUtil.GetNameFromFuncDesc(typeinfo, funcdesc);
			if (!isPropertyPut)
			{
				string stringFromTypeDesc = ComUtil.GetStringFromTypeDesc(typeinfo, funcdesc.elemdescFunc.tdesc);
				stringBuilder.Append(stringFromTypeDesc + " ");
			}
			stringBuilder.Append(nameFromFuncDesc);
			stringBuilder.Append(" (");
			IntPtr lprgelemdescParam = funcdesc.lprgelemdescParam;
			int num = ClrFacade.SizeOf<System.Runtime.InteropServices.ComTypes.ELEMDESC>();
			for (int i = 0; i < (int)funcdesc.cParams; i++)
			{
				System.Runtime.InteropServices.ComTypes.ELEMDESC elemdesc = default(System.Runtime.InteropServices.ComTypes.ELEMDESC);
				int num2 = i * num;
				IntPtr ptr;
				if (IntPtr.Size == 4)
				{
					ptr = (IntPtr)(lprgelemdescParam.ToInt32() + num2);
				}
				else
				{
					ptr = (IntPtr)(lprgelemdescParam.ToInt64() + (long)num2);
				}
				string stringFromTypeDesc2 = ComUtil.GetStringFromTypeDesc(typeinfo, ClrFacade.PtrToStructure<System.Runtime.InteropServices.ComTypes.ELEMDESC>(ptr).tdesc);
				if (i == 0 && isPropertyPut)
				{
					stringBuilder.Insert(0, stringFromTypeDesc2 + " ");
				}
				else
				{
					stringBuilder.Append(stringFromTypeDesc2);
					if (i < (int)(funcdesc.cParams - 1))
					{
						stringBuilder.Append(", ");
					}
				}
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x00075CA4 File Offset: 0x00073EA4
		internal static string GetNameFromFuncDesc(ITypeInfo typeinfo, System.Runtime.InteropServices.ComTypes.FUNCDESC funcdesc)
		{
			string result;
			string text;
			int num;
			string text2;
			typeinfo.GetDocumentation(funcdesc.memid, out result, out text, out num, out text2);
			return result;
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x00075CC8 File Offset: 0x00073EC8
		private static string GetStringFromCustomType(ITypeInfo typeinfo, IntPtr refptr)
		{
			int hRef = (int)((long)refptr);
			ITypeInfo typeInfo;
			typeinfo.GetRefTypeInfo(hRef, out typeInfo);
			if (typeInfo != null)
			{
				string result;
				string text;
				int num;
				string text2;
				typeInfo.GetDocumentation(-1, out result, out text, out num, out text2);
				return result;
			}
			return "UnknownCustomtype";
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x00075D00 File Offset: 0x00073F00
		private static string GetStringFromTypeDesc(ITypeInfo typeinfo, System.Runtime.InteropServices.ComTypes.TYPEDESC typedesc)
		{
			if (typedesc.vt == 26)
			{
				System.Runtime.InteropServices.ComTypes.TYPEDESC typedesc2 = ClrFacade.PtrToStructure<System.Runtime.InteropServices.ComTypes.TYPEDESC>(typedesc.lpValue);
				return ComUtil.GetStringFromTypeDesc(typeinfo, typedesc2);
			}
			if (typedesc.vt == 27)
			{
				System.Runtime.InteropServices.ComTypes.TYPEDESC typedesc3 = ClrFacade.PtrToStructure<System.Runtime.InteropServices.ComTypes.TYPEDESC>(typedesc.lpValue);
				return "SAFEARRAY(" + ComUtil.GetStringFromTypeDesc(typeinfo, typedesc3) + ")";
			}
			if (typedesc.vt == 29)
			{
				return ComUtil.GetStringFromCustomType(typeinfo, typedesc.lpValue);
			}
			VarEnum vt = (VarEnum)typedesc.vt;
			switch (vt)
			{
			case VarEnum.VT_EMPTY:
				return "";
			case VarEnum.VT_NULL:
			case VarEnum.VT_ERROR:
			case (VarEnum)15:
			case VarEnum.VT_PTR:
			case VarEnum.VT_SAFEARRAY:
			case VarEnum.VT_CARRAY:
			case VarEnum.VT_USERDEFINED:
				break;
			case VarEnum.VT_I2:
				return "short";
			case VarEnum.VT_I4:
			case VarEnum.VT_INT:
			case VarEnum.VT_HRESULT:
				return "int";
			case VarEnum.VT_R4:
				return "float";
			case VarEnum.VT_R8:
				return "double";
			case VarEnum.VT_CY:
				return "currency";
			case VarEnum.VT_DATE:
				return "Date";
			case VarEnum.VT_BSTR:
			case VarEnum.VT_LPSTR:
			case VarEnum.VT_LPWSTR:
				return "string";
			case VarEnum.VT_DISPATCH:
				return "IDispatch";
			case VarEnum.VT_BOOL:
				return "bool";
			case VarEnum.VT_VARIANT:
				return "Variant";
			case VarEnum.VT_UNKNOWN:
				return "IUnknown";
			case VarEnum.VT_DECIMAL:
				return "decimal";
			case VarEnum.VT_I1:
				return "char";
			case VarEnum.VT_UI1:
				return "byte";
			case VarEnum.VT_UI2:
				return "ushort";
			case VarEnum.VT_UI4:
			case VarEnum.VT_UINT:
				return "uint";
			case VarEnum.VT_I8:
				return "int64";
			case VarEnum.VT_UI8:
				return "uint64";
			case VarEnum.VT_VOID:
				return "void";
			default:
				if (vt == VarEnum.VT_CLSID)
				{
					return "clsid";
				}
				if (vt == VarEnum.VT_ARRAY)
				{
					return "object[]";
				}
				break;
			}
			return "Unknown!";
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x00075EA0 File Offset: 0x000740A0
		internal static Type GetTypeFromTypeDesc(System.Runtime.InteropServices.ComTypes.TYPEDESC typedesc)
		{
			VarEnum vt = (VarEnum)typedesc.vt;
			return VarEnumSelector.GetTypeForVarEnum(vt);
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x00075EBC File Offset: 0x000740BC
		private static ComMethodInformation GetMethodInformation(System.Runtime.InteropServices.ComTypes.FUNCDESC funcdesc, bool skipLastParameter)
		{
			Type typeFromTypeDesc = ComUtil.GetTypeFromTypeDesc(funcdesc.elemdescFunc.tdesc);
			ParameterInformation[] parameterInformation = ComUtil.GetParameterInformation(funcdesc, skipLastParameter);
			bool hasoptional = false;
			foreach (ParameterInformation parameterInformation2 in parameterInformation)
			{
				if (parameterInformation2.isOptional)
				{
					hasoptional = true;
					break;
				}
			}
			return new ComMethodInformation(false, hasoptional, parameterInformation, typeFromTypeDesc, funcdesc.memid, funcdesc.invkind);
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x00075F24 File Offset: 0x00074124
		internal static ParameterInformation[] GetParameterInformation(System.Runtime.InteropServices.ComTypes.FUNCDESC funcdesc, bool skipLastParameter)
		{
			int num = (int)funcdesc.cParams;
			if (skipLastParameter)
			{
				num--;
			}
			ParameterInformation[] array = new ParameterInformation[num];
			IntPtr lprgelemdescParam = funcdesc.lprgelemdescParam;
			int num2 = ClrFacade.SizeOf<System.Runtime.InteropServices.ComTypes.ELEMDESC>();
			for (int i = 0; i < num; i++)
			{
				bool isOptional = false;
				System.Runtime.InteropServices.ComTypes.ELEMDESC elemdesc = default(System.Runtime.InteropServices.ComTypes.ELEMDESC);
				int num3 = i * num2;
				IntPtr ptr;
				if (IntPtr.Size == 4)
				{
					ptr = (IntPtr)(lprgelemdescParam.ToInt32() + num3);
				}
				else
				{
					ptr = (IntPtr)(lprgelemdescParam.ToInt64() + (long)num3);
				}
				elemdesc = ClrFacade.PtrToStructure<System.Runtime.InteropServices.ComTypes.ELEMDESC>(ptr);
				Type typeFromTypeDesc = ComUtil.GetTypeFromTypeDesc(elemdesc.tdesc);
				object defaultValue = null;
				if ((short)(elemdesc.desc.paramdesc.wParamFlags & System.Runtime.InteropServices.ComTypes.PARAMFLAG.PARAMFLAG_FOPT) != 0)
				{
					isOptional = true;
					defaultValue = Type.Missing;
				}
				bool isByRef = (short)(elemdesc.desc.paramdesc.wParamFlags & System.Runtime.InteropServices.ComTypes.PARAMFLAG.PARAMFLAG_FOUT) != 0;
				array[i] = new ParameterInformation(typeFromTypeDesc, isOptional, defaultValue, isByRef);
			}
			return array;
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x00076014 File Offset: 0x00074214
		internal static ComMethodInformation[] GetMethodInformationArray(ITypeInfo typeInfo, Collection<int> methods, bool skipLastParameters)
		{
			int count = methods.Count;
			int num = 0;
			ComMethodInformation[] array = new ComMethodInformation[count];
			foreach (int index in methods)
			{
				IntPtr intPtr;
				typeInfo.GetFuncDesc(index, out intPtr);
				System.Runtime.InteropServices.ComTypes.FUNCDESC funcdesc = ClrFacade.PtrToStructure<System.Runtime.InteropServices.ComTypes.FUNCDESC>(intPtr);
				array[num++] = ComUtil.GetMethodInformation(funcdesc, skipLastParameters);
				typeInfo.ReleaseFuncDesc(intPtr);
			}
			return array;
		}

		// Token: 0x04000836 RID: 2102
		internal const int DISP_E_MEMBERNOTFOUND = -2147352573;

		// Token: 0x04000837 RID: 2103
		internal const int DISP_E_UNKNOWNNAME = -2147352570;

		// Token: 0x04000838 RID: 2104
		internal const int TYPE_E_ELEMENTNOTFOUND = -2147319765;
	}
}
