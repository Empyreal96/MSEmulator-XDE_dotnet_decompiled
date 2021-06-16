using System;
using System.Collections.ObjectModel;
using System.Management.Automation.Internal;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x02000143 RID: 323
	public class PSScriptMethod : PSMethodInfo
	{
		// Token: 0x060010DC RID: 4316 RVA: 0x0005E338 File Offset: 0x0005C538
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.TypeNameOfValue);
			stringBuilder.Append(" ");
			stringBuilder.Append(base.Name);
			stringBuilder.Append("();");
			return stringBuilder.ToString();
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x060010DD RID: 4317 RVA: 0x0005E384 File Offset: 0x0005C584
		public ScriptBlock Script
		{
			get
			{
				if (this.shouldCloneOnAccess)
				{
					ScriptBlock scriptBlock = this.script.Clone(false);
					scriptBlock.LanguageMode = this.script.LanguageMode;
					return scriptBlock;
				}
				return this.script;
			}
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x0005E3BF File Offset: 0x0005C5BF
		public PSScriptMethod(string name, ScriptBlock script)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.name = name;
			if (script == null)
			{
				throw PSTraceSource.NewArgumentNullException("script");
			}
			this.script = script;
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x0005E3F6 File Offset: 0x0005C5F6
		internal PSScriptMethod(string name, ScriptBlock script, bool shouldCloneOnAccess) : this(name, script)
		{
			this.shouldCloneOnAccess = shouldCloneOnAccess;
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x0005E407 File Offset: 0x0005C607
		public override object Invoke(params object[] arguments)
		{
			if (arguments == null)
			{
				throw PSTraceSource.NewArgumentNullException("arguments");
			}
			return PSScriptMethod.InvokeScript(base.Name, this.script, this.instance, arguments);
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x0005E430 File Offset: 0x0005C630
		private static object InvokeScript(string methodName, ScriptBlock script, object @this, object[] arguments)
		{
			object result;
			try
			{
				result = script.DoInvokeReturnAsIs(true, ScriptBlock.ErrorHandlingBehavior.WriteToExternalErrorPipe, AutomationNull.Value, AutomationNull.Value, @this, arguments);
			}
			catch (SessionStateOverflowException ex)
			{
				throw new MethodInvocationException("ScriptMethodSessionStateOverflowException", ex, ExtendedTypeSystem.MethodInvocationException, new object[]
				{
					methodName,
					arguments.Length,
					ex.Message
				});
			}
			catch (RuntimeException ex2)
			{
				throw new MethodInvocationException("ScriptMethodRuntimeException", ex2, ExtendedTypeSystem.MethodInvocationException, new object[]
				{
					methodName,
					arguments.Length,
					ex2.Message
				});
			}
			catch (TerminateException)
			{
				throw;
			}
			catch (FlowControlException ex3)
			{
				throw new MethodInvocationException("ScriptMethodFlowControlException", ex3, ExtendedTypeSystem.MethodInvocationException, new object[]
				{
					methodName,
					arguments.Length,
					ex3.Message
				});
			}
			catch (PSInvalidOperationException ex4)
			{
				throw new MethodInvocationException("ScriptMethodInvalidOperationException", ex4, ExtendedTypeSystem.MethodInvocationException, new object[]
				{
					methodName,
					arguments.Length,
					ex4.Message
				});
			}
			return result;
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x060010E2 RID: 4322 RVA: 0x0005E57C File Offset: 0x0005C77C
		public override Collection<string> OverloadDefinitions
		{
			get
			{
				return new Collection<string>
				{
					this.ToString()
				};
			}
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x0005E59C File Offset: 0x0005C79C
		public override PSMemberInfo Copy()
		{
			PSScriptMethod psscriptMethod = new PSScriptMethod(this.name, this.script);
			psscriptMethod.shouldCloneOnAccess = this.shouldCloneOnAccess;
			base.CloneBaseProperties(psscriptMethod);
			return psscriptMethod;
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x060010E4 RID: 4324 RVA: 0x0005E5CF File Offset: 0x0005C7CF
		public override PSMemberTypes MemberType
		{
			get
			{
				return PSMemberTypes.ScriptMethod;
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x060010E5 RID: 4325 RVA: 0x0005E5D6 File Offset: 0x0005C7D6
		public override string TypeNameOfValue
		{
			get
			{
				return typeof(object).FullName;
			}
		}

		// Token: 0x04000745 RID: 1861
		private ScriptBlock script;

		// Token: 0x04000746 RID: 1862
		private bool shouldCloneOnAccess;
	}
}
