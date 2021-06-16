using System;
using System.Collections.ObjectModel;
using System.Management.Automation.Internal;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Management.Automation
{
	// Token: 0x02000180 RID: 384
	internal class ComMethod
	{
		// Token: 0x060012D2 RID: 4818 RVA: 0x00075046 File Offset: 0x00073246
		internal ComMethod(ITypeInfo typeinfo, string name)
		{
			this.typeInfo = typeinfo;
			this.name = name;
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x060012D3 RID: 4819 RVA: 0x00075067 File Offset: 0x00073267
		internal string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x0007506F File Offset: 0x0007326F
		internal void AddFuncDesc(int index)
		{
			this.methods.Add(index);
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x00075080 File Offset: 0x00073280
		internal Collection<string> MethodDefinitions()
		{
			Collection<string> collection = new Collection<string>();
			foreach (int index in this.methods)
			{
				IntPtr intPtr;
				this.typeInfo.GetFuncDesc(index, out intPtr);
				System.Runtime.InteropServices.ComTypes.FUNCDESC funcdesc = ClrFacade.PtrToStructure<System.Runtime.InteropServices.ComTypes.FUNCDESC>(intPtr);
				string methodSignatureFromFuncDesc = ComUtil.GetMethodSignatureFromFuncDesc(this.typeInfo, funcdesc, false);
				collection.Add(methodSignatureFromFuncDesc);
				this.typeInfo.ReleaseFuncDesc(intPtr);
			}
			return collection;
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x0007510C File Offset: 0x0007330C
		internal object InvokeMethod(PSMethod method, object[] arguments)
		{
			try
			{
				ComMethodInformation[] methodInformationArray = ComUtil.GetMethodInformationArray(this.typeInfo, this.methods, false);
				object[] array;
				ComMethodInformation comMethodInformation = (ComMethodInformation)Adapter.GetBestMethodAndArguments(this.Name, methodInformationArray, arguments, out array);
				object obj = ComInvoker.Invoke(method.baseObject as IDispatch, comMethodInformation.DispId, array, ComInvoker.GetByRefArray(comMethodInformation.parameters, array.Length, false), System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_FUNC);
				Adapter.SetReferences(array, comMethodInformation, arguments);
				return (comMethodInformation.ReturnType != typeof(void)) ? obj : AutomationNull.Value;
			}
			catch (TargetInvocationException ex)
			{
				CommandProcessorBase.CheckForSevereException(ex.InnerException);
				COMException ex2 = ex.InnerException as COMException;
				if (ex2 == null || ex2.HResult != -2147352573)
				{
					string text = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;
					throw new MethodInvocationException("ComMethodTargetInvocation", ex, ExtendedTypeSystem.MethodInvocationException, new object[]
					{
						method.Name,
						arguments.Length,
						text
					});
				}
			}
			catch (COMException ex3)
			{
				if (ex3.HResult != -2147352570)
				{
					throw new MethodInvocationException("ComMethodCOMException", ex3, ExtendedTypeSystem.MethodInvocationException, new object[]
					{
						method.Name,
						arguments.Length,
						ex3.Message
					});
				}
			}
			return null;
		}

		// Token: 0x04000824 RID: 2084
		private Collection<int> methods = new Collection<int>();

		// Token: 0x04000825 RID: 2085
		private ITypeInfo typeInfo;

		// Token: 0x04000826 RID: 2086
		private string name;
	}
}
