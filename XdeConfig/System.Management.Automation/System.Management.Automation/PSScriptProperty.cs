using System;
using System.Management.Automation.Internal;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x0200013F RID: 319
	public class PSScriptProperty : PSPropertyInfo
	{
		// Token: 0x060010B2 RID: 4274 RVA: 0x0005D85C File Offset: 0x0005BA5C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.TypeNameOfValue);
			stringBuilder.Append(" ");
			stringBuilder.Append(base.Name);
			stringBuilder.Append(" {");
			if (this.IsGettable)
			{
				stringBuilder.Append("get=");
				stringBuilder.Append(this.GetterScript.ToString());
				stringBuilder.Append(";");
			}
			if (this.IsSettable)
			{
				stringBuilder.Append("set=");
				stringBuilder.Append(this.SetterScript.ToString());
				stringBuilder.Append(";");
			}
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x060010B3 RID: 4275 RVA: 0x0005D918 File Offset: 0x0005BB18
		public ScriptBlock GetterScript
		{
			get
			{
				if (this.getterScript == null && this.getterScriptText != null)
				{
					this.getterScript = ScriptBlock.Create(this.getterScriptText);
					if (this.languageMode != null)
					{
						this.getterScript.LanguageMode = this.languageMode;
					}
					this.getterScript.DebuggerStepThrough = true;
				}
				if (this.getterScript == null)
				{
					return null;
				}
				if (this.shouldCloneOnAccess)
				{
					ScriptBlock scriptBlock = this.getterScript.Clone(false);
					scriptBlock.LanguageMode = this.getterScript.LanguageMode;
					return scriptBlock;
				}
				return this.getterScript;
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x060010B4 RID: 4276 RVA: 0x0005D9A8 File Offset: 0x0005BBA8
		public ScriptBlock SetterScript
		{
			get
			{
				if (this.setterScript == null && this.setterScriptText != null)
				{
					this.setterScript = ScriptBlock.Create(this.setterScriptText);
					if (this.languageMode != null)
					{
						this.setterScript.LanguageMode = this.languageMode;
					}
					this.setterScript.DebuggerStepThrough = true;
				}
				if (this.setterScript == null)
				{
					return null;
				}
				if (this.shouldCloneOnAccess)
				{
					ScriptBlock scriptBlock = this.setterScript.Clone(false);
					scriptBlock.LanguageMode = this.setterScript.LanguageMode;
					return scriptBlock;
				}
				return this.setterScript;
			}
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x0005DA38 File Offset: 0x0005BC38
		public PSScriptProperty(string name, ScriptBlock getterScript)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.name = name;
			if (getterScript == null)
			{
				throw PSTraceSource.NewArgumentNullException("getterScript");
			}
			this.getterScript = getterScript;
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x0005DA70 File Offset: 0x0005BC70
		public PSScriptProperty(string name, ScriptBlock getterScript, ScriptBlock setterScript)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.name = name;
			if (getterScript == null && setterScript == null)
			{
				throw PSTraceSource.NewArgumentException("getterScript setterScript");
			}
			if (getterScript != null)
			{
				getterScript.DebuggerStepThrough = true;
			}
			if (setterScript != null)
			{
				setterScript.DebuggerStepThrough = true;
			}
			this.getterScript = getterScript;
			this.setterScript = setterScript;
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x0005DAD0 File Offset: 0x0005BCD0
		internal PSScriptProperty(string name, string getterScript, string setterScript, PSLanguageMode? languageMode)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.name = name;
			if (getterScript == null && setterScript == null)
			{
				throw PSTraceSource.NewArgumentException("getterScript setterScript");
			}
			this.getterScriptText = getterScript;
			this.setterScriptText = setterScript;
			this.languageMode = languageMode;
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x0005DB24 File Offset: 0x0005BD24
		internal PSScriptProperty(string name, ScriptBlock getterScript, ScriptBlock setterScript, bool shouldCloneOnAccess) : this(name, getterScript, setterScript)
		{
			this.shouldCloneOnAccess = shouldCloneOnAccess;
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x0005DB37 File Offset: 0x0005BD37
		internal PSScriptProperty(string name, string getterScript, string setterScript, PSLanguageMode? languageMode, bool shouldCloneOnAccess) : this(name, getterScript, setterScript, languageMode)
		{
			this.shouldCloneOnAccess = shouldCloneOnAccess;
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x0005DB4C File Offset: 0x0005BD4C
		public override PSMemberInfo Copy()
		{
			PSScriptProperty psscriptProperty = new PSScriptProperty(this.name, this.GetterScript, this.SetterScript);
			psscriptProperty.shouldCloneOnAccess = this.shouldCloneOnAccess;
			base.CloneBaseProperties(psscriptProperty);
			return psscriptProperty;
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x060010BB RID: 4283 RVA: 0x0005DB85 File Offset: 0x0005BD85
		public override PSMemberTypes MemberType
		{
			get
			{
				return PSMemberTypes.ScriptProperty;
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x060010BC RID: 4284 RVA: 0x0005DB89 File Offset: 0x0005BD89
		public override bool IsSettable
		{
			get
			{
				return this.SetterScript != null;
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x060010BD RID: 4285 RVA: 0x0005DB97 File Offset: 0x0005BD97
		public override bool IsGettable
		{
			get
			{
				return this.GetterScript != null;
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x060010BE RID: 4286 RVA: 0x0005DBA8 File Offset: 0x0005BDA8
		// (set) Token: 0x060010BF RID: 4287 RVA: 0x0005DBEC File Offset: 0x0005BDEC
		public override object Value
		{
			get
			{
				if (this.GetterScript == null)
				{
					throw new GetValueException("GetWithoutGetterFromScriptPropertyValue", null, ExtendedTypeSystem.GetWithoutGetterException, new object[]
					{
						base.Name
					});
				}
				return this.InvokeGetter(this.instance);
			}
			set
			{
				if (this.SetterScript == null)
				{
					throw new SetValueException("SetWithoutSetterFromScriptProperty", null, ExtendedTypeSystem.SetWithoutSetterException, new object[]
					{
						base.Name
					});
				}
				this.InvokeSetter(this.instance, value);
			}
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x0005DC34 File Offset: 0x0005BE34
		internal object InvokeSetter(object scriptThis, object value)
		{
			try
			{
				this.SetterScript.DoInvokeReturnAsIs(true, ScriptBlock.ErrorHandlingBehavior.WriteToExternalErrorPipe, AutomationNull.Value, AutomationNull.Value, scriptThis, new object[]
				{
					value
				});
			}
			catch (SessionStateOverflowException e)
			{
				throw base.NewSetValueException(e, "ScriptSetValueSessionStateOverflowException");
			}
			catch (RuntimeException e2)
			{
				throw base.NewSetValueException(e2, "ScriptSetValueRuntimeException");
			}
			catch (TerminateException)
			{
				throw;
			}
			catch (FlowControlException e3)
			{
				throw base.NewSetValueException(e3, "ScriptSetValueFlowControlException");
			}
			catch (PSInvalidOperationException e4)
			{
				throw base.NewSetValueException(e4, "ScriptSetValueInvalidOperationException");
			}
			return value;
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x0005DCEC File Offset: 0x0005BEEC
		internal object InvokeGetter(object scriptThis)
		{
			object result;
			try
			{
				result = this.GetterScript.DoInvokeReturnAsIs(true, ScriptBlock.ErrorHandlingBehavior.SwallowErrors, AutomationNull.Value, AutomationNull.Value, scriptThis, new object[0]);
			}
			catch (SessionStateOverflowException e)
			{
				throw base.NewGetValueException(e, "ScriptGetValueSessionStateOverflowException");
			}
			catch (RuntimeException e2)
			{
				throw base.NewGetValueException(e2, "ScriptGetValueRuntimeException");
			}
			catch (TerminateException)
			{
				throw;
			}
			catch (FlowControlException e3)
			{
				throw base.NewGetValueException(e3, "ScriptGetValueFlowControlException");
			}
			catch (PSInvalidOperationException e4)
			{
				throw base.NewGetValueException(e4, "ScriptgetValueInvalidOperationException");
			}
			return result;
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x060010C2 RID: 4290 RVA: 0x0005DD98 File Offset: 0x0005BF98
		public override string TypeNameOfValue
		{
			get
			{
				if (this.GetterScript != null && this.GetterScript.OutputType.Count > 0)
				{
					return this.GetterScript.OutputType[0].Name;
				}
				return typeof(object).FullName;
			}
		}

		// Token: 0x0400073A RID: 1850
		private PSLanguageMode? languageMode;

		// Token: 0x0400073B RID: 1851
		private string getterScriptText;

		// Token: 0x0400073C RID: 1852
		private ScriptBlock getterScript;

		// Token: 0x0400073D RID: 1853
		private string setterScriptText;

		// Token: 0x0400073E RID: 1854
		private ScriptBlock setterScript;

		// Token: 0x0400073F RID: 1855
		private bool shouldCloneOnAccess;
	}
}
