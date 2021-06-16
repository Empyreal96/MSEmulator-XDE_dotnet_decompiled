using System;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x0200007E RID: 126
	internal class RuntimeDefinedParameterBinder : ParameterBinderBase
	{
		// Token: 0x0600068C RID: 1676 RVA: 0x0001FCF0 File Offset: 0x0001DEF0
		internal RuntimeDefinedParameterBinder(RuntimeDefinedParameterDictionary target, InternalCommand command, CommandLineParameters commandLineParameters) : base(target, command.MyInvocation, command.Context, command)
		{
			foreach (string text in target.Keys)
			{
				RuntimeDefinedParameter runtimeDefinedParameter = target[text];
				string text2 = (runtimeDefinedParameter == null) ? null : runtimeDefinedParameter.Name;
				if (runtimeDefinedParameter == null || text != text2)
				{
					ParameterBindingException ex = new ParameterBindingException(ErrorCategory.InvalidArgument, command.MyInvocation, null, text2, null, null, ParameterBinderStrings.RuntimeDefinedParameterNameMismatch, "RuntimeDefinedParameterNameMismatch", new object[]
					{
						text
					});
					throw ex;
				}
			}
			base.CommandLineParameters = commandLineParameters;
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x0001FDA8 File Offset: 0x0001DFA8
		// (set) Token: 0x0600068E RID: 1678 RVA: 0x0001FDB5 File Offset: 0x0001DFB5
		internal new RuntimeDefinedParameterDictionary Target
		{
			get
			{
				return base.Target as RuntimeDefinedParameterDictionary;
			}
			set
			{
				base.Target = value;
			}
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0001FDC0 File Offset: 0x0001DFC0
		internal override object GetDefaultParameterValue(string name)
		{
			object result = null;
			if (this.Target.ContainsKey(name))
			{
				RuntimeDefinedParameter runtimeDefinedParameter = this.Target[name];
				if (runtimeDefinedParameter != null)
				{
					result = runtimeDefinedParameter.Value;
				}
			}
			return result;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0001FDF5 File Offset: 0x0001DFF5
		internal override void BindParameter(string name, object value)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.Target[name].Value = value;
			base.CommandLineParameters.Add(name, value);
		}
	}
}
