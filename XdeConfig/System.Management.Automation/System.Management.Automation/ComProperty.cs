using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x02000181 RID: 385
	internal class ComProperty
	{
		// Token: 0x060012D7 RID: 4823 RVA: 0x00075290 File Offset: 0x00073490
		internal ComProperty(ITypeInfo typeinfo, string name)
		{
			this.typeInfo = typeinfo;
			this.name = name;
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x060012D8 RID: 4824 RVA: 0x000752A6 File Offset: 0x000734A6
		internal string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x060012D9 RID: 4825 RVA: 0x000752B0 File Offset: 0x000734B0
		internal Type Type
		{
			get
			{
				this.cachedType = null;
				if (this.cachedType == null)
				{
					IntPtr zero = IntPtr.Zero;
					try
					{
						this.typeInfo.GetFuncDesc(this.GetFuncDescIndex(), out zero);
						System.Runtime.InteropServices.ComTypes.FUNCDESC funcdesc = ClrFacade.PtrToStructure<System.Runtime.InteropServices.ComTypes.FUNCDESC>(zero);
						if (this.hasGetter)
						{
							this.cachedType = ComUtil.GetTypeFromTypeDesc(funcdesc.elemdescFunc.tdesc);
						}
						else
						{
							ParameterInformation[] parameterInformation = ComUtil.GetParameterInformation(funcdesc, false);
							this.cachedType = parameterInformation[0].parameterType;
						}
					}
					finally
					{
						if (zero != IntPtr.Zero)
						{
							this.typeInfo.ReleaseFuncDesc(zero);
						}
					}
				}
				return this.cachedType;
			}
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x0007535C File Offset: 0x0007355C
		private int GetFuncDescIndex()
		{
			if (this.hasGetter)
			{
				return this.getterIndex;
			}
			if (this.hasSetter)
			{
				return this.setterIndex;
			}
			return this.setterByRefIndex;
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x060012DB RID: 4827 RVA: 0x00075382 File Offset: 0x00073582
		internal bool IsParameterized
		{
			get
			{
				return this.isparameterizied;
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x060012DC RID: 4828 RVA: 0x0007538A File Offset: 0x0007358A
		internal int ParamCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x060012DD RID: 4829 RVA: 0x0007538D File Offset: 0x0007358D
		internal bool IsSettable
		{
			get
			{
				return this.hasSetter | this.hasSetterByRef;
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x060012DE RID: 4830 RVA: 0x0007539C File Offset: 0x0007359C
		internal bool IsGettable
		{
			get
			{
				return this.hasGetter;
			}
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x000753A4 File Offset: 0x000735A4
		internal object GetValue(object target)
		{
			try
			{
				return ComInvoker.Invoke(target as IDispatch, this.dispId, null, null, System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYGET);
			}
			catch (TargetInvocationException ex)
			{
				CommandProcessorBase.CheckForSevereException(ex.InnerException);
				COMException ex2 = ex.InnerException as COMException;
				if (ex2 == null || ex2.HResult != -2147352573)
				{
					throw;
				}
			}
			catch (COMException ex3)
			{
				if (ex3.HResult != -2147352570)
				{
					throw;
				}
			}
			return null;
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x00075428 File Offset: 0x00073628
		internal object GetValue(object target, object[] arguments)
		{
			try
			{
				Collection<int> methods = new Collection<int>
				{
					this.getterIndex
				};
				ComMethodInformation[] methodInformationArray = ComUtil.GetMethodInformationArray(this.typeInfo, methods, false);
				object[] array;
				ComMethodInformation comMethodInformation = (ComMethodInformation)Adapter.GetBestMethodAndArguments(this.Name, methodInformationArray, arguments, out array);
				object result = ComInvoker.Invoke(target as IDispatch, comMethodInformation.DispId, array, ComInvoker.GetByRefArray(comMethodInformation.parameters, array.Length, false), comMethodInformation.InvokeKind);
				Adapter.SetReferences(array, comMethodInformation, arguments);
				return result;
			}
			catch (TargetInvocationException ex)
			{
				CommandProcessorBase.CheckForSevereException(ex.InnerException);
				COMException ex2 = ex.InnerException as COMException;
				if (ex2 == null || ex2.HResult != -2147352573)
				{
					throw;
				}
			}
			catch (COMException ex3)
			{
				if (ex3.HResult != -2147352570)
				{
					throw;
				}
			}
			return null;
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x0007550C File Offset: 0x0007370C
		internal void SetValue(object target, object setValue)
		{
			object[] array = new object[1];
			setValue = Adapter.PropertySetAndMethodArgumentConvertTo(setValue, this.Type, CultureInfo.InvariantCulture);
			array[0] = setValue;
			try
			{
				ComInvoker.Invoke(target as IDispatch, this.dispId, array, null, System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYPUT);
			}
			catch (TargetInvocationException ex)
			{
				CommandProcessorBase.CheckForSevereException(ex.InnerException);
				COMException ex2 = ex.InnerException as COMException;
				if (ex2 == null || ex2.HResult != -2147352573)
				{
					throw;
				}
			}
			catch (COMException ex3)
			{
				if (ex3.HResult != -2147352570)
				{
					throw;
				}
			}
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x000755A8 File Offset: 0x000737A8
		internal void SetValue(object target, object setValue, object[] arguments)
		{
			Collection<int> methods = new Collection<int>
			{
				this.hasSetterByRef ? this.setterByRefIndex : this.setterIndex
			};
			ComMethodInformation[] methodInformationArray = ComUtil.GetMethodInformationArray(this.typeInfo, methods, true);
			object[] array;
			ComMethodInformation comMethodInformation = (ComMethodInformation)Adapter.GetBestMethodAndArguments(this.Name, methodInformationArray, arguments, out array);
			object[] array2 = new object[array.Length + 1];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = array[i];
			}
			array2[array.Length] = Adapter.PropertySetAndMethodArgumentConvertTo(setValue, this.Type, CultureInfo.InvariantCulture);
			try
			{
				ComInvoker.Invoke(target as IDispatch, comMethodInformation.DispId, array2, ComInvoker.GetByRefArray(comMethodInformation.parameters, array2.Length, true), comMethodInformation.InvokeKind);
				Adapter.SetReferences(array2, comMethodInformation, arguments);
			}
			catch (TargetInvocationException ex)
			{
				CommandProcessorBase.CheckForSevereException(ex.InnerException);
				COMException ex2 = ex.InnerException as COMException;
				if (ex2 == null || ex2.HResult != -2147352573)
				{
					throw;
				}
			}
			catch (COMException ex3)
			{
				if (ex3.HResult != -2147352570)
				{
					throw;
				}
			}
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x000756D4 File Offset: 0x000738D4
		internal void UpdateFuncDesc(System.Runtime.InteropServices.ComTypes.FUNCDESC desc, int index)
		{
			this.dispId = desc.memid;
			System.Runtime.InteropServices.ComTypes.INVOKEKIND invkind = desc.invkind;
			switch (invkind)
			{
			case System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYGET:
				this.hasGetter = true;
				this.getterIndex = index;
				if (desc.cParams > 0)
				{
					this.isparameterizied = true;
					return;
				}
				break;
			case System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_FUNC | System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYGET:
				break;
			case System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYPUT:
				this.hasSetter = true;
				this.setterIndex = index;
				if (desc.cParams > 1)
				{
					this.isparameterizied = true;
					return;
				}
				break;
			default:
				if (invkind != System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYPUTREF)
				{
					return;
				}
				this.setterByRefIndex = index;
				this.hasSetterByRef = true;
				if (desc.cParams > 1)
				{
					this.isparameterizied = true;
				}
				break;
			}
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x00075770 File Offset: 0x00073970
		internal string GetDefinition()
		{
			IntPtr zero = IntPtr.Zero;
			string methodSignatureFromFuncDesc;
			try
			{
				this.typeInfo.GetFuncDesc(this.GetFuncDescIndex(), out zero);
				System.Runtime.InteropServices.ComTypes.FUNCDESC funcdesc = ClrFacade.PtrToStructure<System.Runtime.InteropServices.ComTypes.FUNCDESC>(zero);
				methodSignatureFromFuncDesc = ComUtil.GetMethodSignatureFromFuncDesc(this.typeInfo, funcdesc, !this.hasGetter);
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					this.typeInfo.ReleaseFuncDesc(zero);
				}
			}
			return methodSignatureFromFuncDesc;
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x000757E0 File Offset: 0x000739E0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.GetDefinition());
			stringBuilder.Append(" ");
			if (this.hasGetter)
			{
				stringBuilder.Append("{get} ");
			}
			if (this.hasSetter)
			{
				stringBuilder.Append("{set} ");
			}
			if (this.hasSetterByRef)
			{
				stringBuilder.Append("{set by ref}");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000827 RID: 2087
		private bool hasSetter;

		// Token: 0x04000828 RID: 2088
		private bool hasSetterByRef;

		// Token: 0x04000829 RID: 2089
		private bool hasGetter;

		// Token: 0x0400082A RID: 2090
		private int dispId;

		// Token: 0x0400082B RID: 2091
		private int setterIndex;

		// Token: 0x0400082C RID: 2092
		private int setterByRefIndex;

		// Token: 0x0400082D RID: 2093
		private int getterIndex;

		// Token: 0x0400082E RID: 2094
		private ITypeInfo typeInfo;

		// Token: 0x0400082F RID: 2095
		private string name;

		// Token: 0x04000830 RID: 2096
		private bool isparameterizied;

		// Token: 0x04000831 RID: 2097
		private Type cachedType;
	}
}
