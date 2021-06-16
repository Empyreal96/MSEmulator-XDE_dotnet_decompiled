using System;
using System.Reflection;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x0200013A RID: 314
	public class PSCodeProperty : PSPropertyInfo
	{
		// Token: 0x0600107B RID: 4219 RVA: 0x0005CC38 File Offset: 0x0005AE38
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.TypeNameOfValue);
			stringBuilder.Append(" ");
			stringBuilder.Append(base.Name);
			stringBuilder.Append("{");
			if (this.IsGettable)
			{
				stringBuilder.Append("get=");
				stringBuilder.Append(this.getterCodeReference.Name);
				stringBuilder.Append(";");
			}
			if (this.IsSettable)
			{
				stringBuilder.Append("set=");
				stringBuilder.Append(this.setterCodeReference.Name);
				stringBuilder.Append(";");
			}
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x0005CCF4 File Offset: 0x0005AEF4
		internal void SetGetterFromTypeTable(Type type, string methodName)
		{
			MethodInfo methodInfo = null;
			try
			{
				methodInfo = type.GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);
			}
			catch (AmbiguousMatchException)
			{
			}
			if (methodInfo == null)
			{
				throw new ExtendedTypeSystemException("GetterFormatFromTypeTable", null, ExtendedTypeSystem.CodePropertyGetterFormat, new object[0]);
			}
			this.SetGetter(methodInfo);
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x0005CD4C File Offset: 0x0005AF4C
		internal void SetSetterFromTypeTable(Type type, string methodName)
		{
			MethodInfo methodInfo = null;
			try
			{
				methodInfo = type.GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);
			}
			catch (AmbiguousMatchException)
			{
			}
			if (methodInfo == null)
			{
				throw new ExtendedTypeSystemException("SetterFormatFromTypeTable", null, ExtendedTypeSystem.CodePropertySetterFormat, new object[0]);
			}
			this.SetSetter(methodInfo, this.getterCodeReference);
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x0005CDA8 File Offset: 0x0005AFA8
		internal void SetGetter(MethodInfo methodForGet)
		{
			if (methodForGet == null)
			{
				this.getterCodeReference = null;
				return;
			}
			if (!PSCodeProperty.CheckGetterMethodInfo(methodForGet))
			{
				throw new ExtendedTypeSystemException("GetterFormat", null, ExtendedTypeSystem.CodePropertyGetterFormat, new object[0]);
			}
			this.getterCodeReference = methodForGet;
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x0005CDE4 File Offset: 0x0005AFE4
		internal static bool CheckGetterMethodInfo(MethodInfo methodForGet)
		{
			ParameterInfo[] parameters = methodForGet.GetParameters();
			return methodForGet.IsPublic && methodForGet.IsStatic && methodForGet.ReturnType != typeof(void) && parameters.Length == 1 && parameters[0].ParameterType == typeof(PSObject);
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x0005CE40 File Offset: 0x0005B040
		private void SetSetter(MethodInfo methodForSet, MethodInfo methodForGet)
		{
			if (methodForSet == null)
			{
				if (methodForGet == null)
				{
					throw new ExtendedTypeSystemException("SetterAndGetterNullFormat", null, ExtendedTypeSystem.CodePropertyGetterAndSetterNull, new object[0]);
				}
				this.setterCodeReference = null;
				return;
			}
			else
			{
				if (!PSCodeProperty.CheckSetterMethodInfo(methodForSet, methodForGet))
				{
					throw new ExtendedTypeSystemException("SetterFormat", null, ExtendedTypeSystem.CodePropertySetterFormat, new object[0]);
				}
				this.setterCodeReference = methodForSet;
				return;
			}
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x0005CEA8 File Offset: 0x0005B0A8
		internal static bool CheckSetterMethodInfo(MethodInfo methodForSet, MethodInfo methodForGet)
		{
			ParameterInfo[] parameters = methodForSet.GetParameters();
			return methodForSet.IsPublic && methodForSet.IsStatic && methodForSet.ReturnType == typeof(void) && parameters.Length == 2 && parameters[0].ParameterType == typeof(PSObject) && (methodForGet == null || methodForGet.ReturnType == parameters[1].ParameterType);
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x0005CF22 File Offset: 0x0005B122
		internal PSCodeProperty(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.name = name;
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x0005CF44 File Offset: 0x0005B144
		public PSCodeProperty(string name, MethodInfo getterCodeReference)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.name = name;
			if (getterCodeReference == null)
			{
				throw PSTraceSource.NewArgumentNullException("getterCodeReference");
			}
			this.SetGetter(getterCodeReference);
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x0005CF84 File Offset: 0x0005B184
		public PSCodeProperty(string name, MethodInfo getterCodeReference, MethodInfo setterCodeReference)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.name = name;
			if (getterCodeReference == null && setterCodeReference == null)
			{
				throw PSTraceSource.NewArgumentNullException("getterCodeReference setterCodeReference");
			}
			this.SetGetter(getterCodeReference);
			this.SetSetter(setterCodeReference, getterCodeReference);
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06001085 RID: 4229 RVA: 0x0005CFDD File Offset: 0x0005B1DD
		public MethodInfo GetterCodeReference
		{
			get
			{
				return this.getterCodeReference;
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06001086 RID: 4230 RVA: 0x0005CFE5 File Offset: 0x0005B1E5
		public MethodInfo SetterCodeReference
		{
			get
			{
				return this.setterCodeReference;
			}
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x0005CFF0 File Offset: 0x0005B1F0
		public override PSMemberInfo Copy()
		{
			PSCodeProperty pscodeProperty = new PSCodeProperty(this.name, this.getterCodeReference, this.setterCodeReference);
			base.CloneBaseProperties(pscodeProperty);
			return pscodeProperty;
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06001088 RID: 4232 RVA: 0x0005D01D File Offset: 0x0005B21D
		public override PSMemberTypes MemberType
		{
			get
			{
				return PSMemberTypes.CodeProperty;
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06001089 RID: 4233 RVA: 0x0005D020 File Offset: 0x0005B220
		public override bool IsSettable
		{
			get
			{
				return this.SetterCodeReference != null;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x0600108A RID: 4234 RVA: 0x0005D02E File Offset: 0x0005B22E
		public override bool IsGettable
		{
			get
			{
				return this.getterCodeReference != null;
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x0600108B RID: 4235 RVA: 0x0005D03C File Offset: 0x0005B23C
		// (set) Token: 0x0600108C RID: 4236 RVA: 0x0005D144 File Offset: 0x0005B344
		public override object Value
		{
			get
			{
				if (this.getterCodeReference == null)
				{
					throw new GetValueException("GetWithoutGetterFromCodePropertyValue", null, ExtendedTypeSystem.GetWithoutGetterException, new object[]
					{
						base.Name
					});
				}
				object result;
				try
				{
					result = this.getterCodeReference.Invoke(null, new object[]
					{
						this.instance
					});
				}
				catch (TargetInvocationException ex)
				{
					Exception ex2 = (ex.InnerException == null) ? ex : ex.InnerException;
					throw new GetValueInvocationException("CatchFromCodePropertyGetTI", ex2, ExtendedTypeSystem.ExceptionWhenGetting, new object[]
					{
						this.name,
						ex2.Message
					});
				}
				catch (Exception ex3)
				{
					if (ex3 is GetValueException)
					{
						throw;
					}
					CommandProcessorBase.CheckForSevereException(ex3);
					throw new GetValueInvocationException("CatchFromCodePropertyGet", ex3, ExtendedTypeSystem.ExceptionWhenGetting, new object[]
					{
						this.name,
						ex3.Message
					});
				}
				return result;
			}
			set
			{
				if (this.setterCodeReference == null)
				{
					throw new SetValueException("SetWithoutSetterFromCodeProperty", null, ExtendedTypeSystem.SetWithoutSetterException, new object[]
					{
						base.Name
					});
				}
				try
				{
					this.setterCodeReference.Invoke(null, new object[]
					{
						this.instance,
						value
					});
				}
				catch (TargetInvocationException ex)
				{
					Exception ex2 = (ex.InnerException == null) ? ex : ex.InnerException;
					throw new SetValueInvocationException("CatchFromCodePropertySetTI", ex2, ExtendedTypeSystem.ExceptionWhenSetting, new object[]
					{
						this.name,
						ex2.Message
					});
				}
				catch (Exception ex3)
				{
					if (ex3 is SetValueException)
					{
						throw;
					}
					CommandProcessorBase.CheckForSevereException(ex3);
					throw new SetValueInvocationException("CatchFromCodePropertySet", ex3, ExtendedTypeSystem.ExceptionWhenSetting, new object[]
					{
						this.name,
						ex3.Message
					});
				}
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x0600108D RID: 4237 RVA: 0x0005D24C File Offset: 0x0005B44C
		public override string TypeNameOfValue
		{
			get
			{
				if (this.getterCodeReference == null)
				{
					throw new GetValueException("GetWithoutGetterFromCodePropertyTypeOfValue", null, ExtendedTypeSystem.GetWithoutGetterException, new object[]
					{
						base.Name
					});
				}
				return this.getterCodeReference.ReturnType.FullName;
			}
		}

		// Token: 0x04000730 RID: 1840
		private MethodInfo getterCodeReference;

		// Token: 0x04000731 RID: 1841
		private MethodInfo setterCodeReference;
	}
}
