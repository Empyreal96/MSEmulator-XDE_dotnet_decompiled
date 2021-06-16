using System;
using System.Collections.Generic;
using System.Management.Automation.Internal;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Management.Automation
{
	// Token: 0x02000182 RID: 386
	internal class ComTypeInfo
	{
		// Token: 0x060012E6 RID: 4838 RVA: 0x00075850 File Offset: 0x00073A50
		internal ComTypeInfo(ITypeInfo info)
		{
			this.typeinfo = info;
			this.properties = new Dictionary<string, ComProperty>(StringComparer.OrdinalIgnoreCase);
			this.methods = new Dictionary<string, ComMethod>(StringComparer.OrdinalIgnoreCase);
			if (this.typeinfo != null)
			{
				this.Initialize();
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x060012E7 RID: 4839 RVA: 0x000758A3 File Offset: 0x00073AA3
		public Dictionary<string, ComProperty> Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x060012E8 RID: 4840 RVA: 0x000758AB File Offset: 0x00073AAB
		public Dictionary<string, ComMethod> Methods
		{
			get
			{
				return this.methods;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x060012E9 RID: 4841 RVA: 0x000758B3 File Offset: 0x00073AB3
		public string Clsid
		{
			get
			{
				return this.guid.ToString();
			}
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x000758C8 File Offset: 0x00073AC8
		private void Initialize()
		{
			if (this.typeinfo != null)
			{
				System.Runtime.InteropServices.ComTypes.TYPEATTR typeAttr = ComTypeInfo.GetTypeAttr(this.typeinfo);
				this.guid = typeAttr.guid;
				for (int i = 0; i < (int)typeAttr.cFuncs; i++)
				{
					System.Runtime.InteropServices.ComTypes.FUNCDESC funcDesc = ComTypeInfo.GetFuncDesc(this.typeinfo, i);
					if ((funcDesc.wFuncFlags & 1) != 1)
					{
						string nameFromFuncDesc = ComUtil.GetNameFromFuncDesc(this.typeinfo, funcDesc);
						System.Runtime.InteropServices.ComTypes.INVOKEKIND invkind = funcDesc.invkind;
						switch (invkind)
						{
						case System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_FUNC:
							this.AddMethod(nameFromFuncDesc, i);
							goto IL_88;
						case System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYGET:
						case System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYPUT:
							break;
						case System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_FUNC | System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYGET:
							goto IL_88;
						default:
							if (invkind != System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYPUTREF)
							{
								goto IL_88;
							}
							break;
						}
						this.AddProperty(nameFromFuncDesc, funcDesc, i);
					}
					IL_88:;
				}
			}
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x0007596C File Offset: 0x00073B6C
		internal static ComTypeInfo GetDispatchTypeInfo(object comObject)
		{
			ComTypeInfo result = null;
			IDispatch dispatch = comObject as IDispatch;
			if (dispatch != null)
			{
				ITypeInfo typeInfo = null;
				dispatch.GetTypeInfo(0, 0, out typeInfo);
				if (typeInfo != null)
				{
					System.Runtime.InteropServices.ComTypes.TYPEATTR typeAttr = ComTypeInfo.GetTypeAttr(typeInfo);
					if (typeAttr.typekind == System.Runtime.InteropServices.ComTypes.TYPEKIND.TKIND_INTERFACE)
					{
						typeInfo = ComTypeInfo.GetDispatchTypeInfoFromCustomInterfaceTypeInfo(typeInfo);
					}
					if (typeAttr.typekind == System.Runtime.InteropServices.ComTypes.TYPEKIND.TKIND_COCLASS)
					{
						typeInfo = ComTypeInfo.GetDispatchTypeInfoFromCoClassTypeInfo(typeInfo);
					}
					result = new ComTypeInfo(typeInfo);
				}
			}
			return result;
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x000759C8 File Offset: 0x00073BC8
		private void AddProperty(string strName, System.Runtime.InteropServices.ComTypes.FUNCDESC funcdesc, int index)
		{
			ComProperty comProperty;
			if (this.properties.ContainsKey(strName))
			{
				comProperty = this.properties[strName];
			}
			else
			{
				comProperty = new ComProperty(this.typeinfo, strName);
				this.properties[strName] = comProperty;
			}
			if (comProperty != null)
			{
				comProperty.UpdateFuncDesc(funcdesc, index);
			}
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x00075A1C File Offset: 0x00073C1C
		private void AddMethod(string strName, int index)
		{
			ComMethod comMethod;
			if (this.methods.ContainsKey(strName))
			{
				comMethod = this.methods[strName];
			}
			else
			{
				comMethod = new ComMethod(this.typeinfo, strName);
				this.methods[strName] = comMethod;
			}
			if (comMethod != null)
			{
				comMethod.AddFuncDesc(index);
			}
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x00075A6C File Offset: 0x00073C6C
		[ArchitectureSensitive]
		internal static System.Runtime.InteropServices.ComTypes.TYPEATTR GetTypeAttr(ITypeInfo typeinfo)
		{
			IntPtr intPtr;
			typeinfo.GetTypeAttr(out intPtr);
			System.Runtime.InteropServices.ComTypes.TYPEATTR result = ClrFacade.PtrToStructure<System.Runtime.InteropServices.ComTypes.TYPEATTR>(intPtr);
			typeinfo.ReleaseTypeAttr(intPtr);
			return result;
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x00075A90 File Offset: 0x00073C90
		[ArchitectureSensitive]
		internal static System.Runtime.InteropServices.ComTypes.FUNCDESC GetFuncDesc(ITypeInfo typeinfo, int index)
		{
			IntPtr intPtr;
			typeinfo.GetFuncDesc(index, out intPtr);
			System.Runtime.InteropServices.ComTypes.FUNCDESC result = ClrFacade.PtrToStructure<System.Runtime.InteropServices.ComTypes.FUNCDESC>(intPtr);
			typeinfo.ReleaseFuncDesc(intPtr);
			return result;
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x00075AB8 File Offset: 0x00073CB8
		internal static ITypeInfo GetDispatchTypeInfoFromCustomInterfaceTypeInfo(ITypeInfo typeinfo)
		{
			ITypeInfo result = null;
			try
			{
				int hRef;
				typeinfo.GetRefTypeOfImplType(-1, out hRef);
				typeinfo.GetRefTypeInfo(hRef, out result);
			}
			catch (COMException ex)
			{
				if (ex.HResult != -2147319765)
				{
					throw;
				}
			}
			return result;
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x00075B00 File Offset: 0x00073D00
		internal static ITypeInfo GetDispatchTypeInfoFromCoClassTypeInfo(ITypeInfo typeinfo)
		{
			int cImplTypes = (int)ComTypeInfo.GetTypeAttr(typeinfo).cImplTypes;
			ITypeInfo result = null;
			for (int i = 0; i < cImplTypes; i++)
			{
				int hRef;
				typeinfo.GetRefTypeOfImplType(i, out hRef);
				typeinfo.GetRefTypeInfo(hRef, out result);
				System.Runtime.InteropServices.ComTypes.TYPEATTR typeAttr = ComTypeInfo.GetTypeAttr(result);
				if (typeAttr.typekind == System.Runtime.InteropServices.ComTypes.TYPEKIND.TKIND_DISPATCH)
				{
					return result;
				}
				if ((short)(typeAttr.wTypeFlags & System.Runtime.InteropServices.ComTypes.TYPEFLAGS.TYPEFLAG_FDUAL) != 0)
				{
					result = ComTypeInfo.GetDispatchTypeInfoFromCustomInterfaceTypeInfo(result);
					if (ComTypeInfo.GetTypeAttr(result).typekind == System.Runtime.InteropServices.ComTypes.TYPEKIND.TKIND_DISPATCH)
					{
						return result;
					}
				}
			}
			return null;
		}

		// Token: 0x04000832 RID: 2098
		private Dictionary<string, ComProperty> properties;

		// Token: 0x04000833 RID: 2099
		private Dictionary<string, ComMethod> methods;

		// Token: 0x04000834 RID: 2100
		private ITypeInfo typeinfo;

		// Token: 0x04000835 RID: 2101
		private Guid guid = Guid.Empty;
	}
}
