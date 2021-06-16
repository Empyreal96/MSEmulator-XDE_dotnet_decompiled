using System;
using System.Collections;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Runtime.CompilerServices;

namespace System.Management.Automation
{
	// Token: 0x02000083 RID: 131
	internal class ScriptParameterBinder : ParameterBinderBase
	{
		// Token: 0x060006B5 RID: 1717 RVA: 0x00020502 File Offset: 0x0001E702
		internal ScriptParameterBinder(ScriptBlock script, InvocationInfo invocationInfo, ExecutionContext context, InternalCommand command, SessionStateScope localScope) : base(invocationInfo, context, command)
		{
			this.Script = script;
			this.LocalScope = localScope;
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0002052D File Offset: 0x0001E72D
		internal object CopyMutableValues(object o)
		{
			return this._copyMutableValueSite.Target(this._copyMutableValueSite, o);
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00020548 File Offset: 0x0001E748
		internal override object GetDefaultParameterValue(string name)
		{
			RuntimeDefinedParameter parameter;
			if (this.Script.RuntimeDefinedParameters.TryGetValue(name, out parameter))
			{
				return this.GetDefaultScriptParameterValue(parameter, null);
			}
			return null;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00020574 File Offset: 0x0001E774
		internal override void BindParameter(string name, object value)
		{
			if (value == AutomationNull.Value || value == UnboundParameter.Value)
			{
				value = null;
			}
			VariablePath variablePath = new VariablePath(name, VariablePathFlags.Variable);
			if (this.LocalScope != null && variablePath.IsAnyLocal() && this.LocalScope.TrySetLocalParameterValue(variablePath.UnqualifiedPath, this.CopyMutableValues(value)))
			{
				return;
			}
			PSVariable psvariable = new PSVariable(variablePath.UnqualifiedPath, value, variablePath.IsPrivate ? ScopedItemOptions.Private : ScopedItemOptions.None);
			base.Context.EngineSessionState.SetVariable(variablePath, psvariable, false, CommandOrigin.Internal);
			RuntimeDefinedParameter runtimeDefinedParameter;
			if (this.Script.RuntimeDefinedParameters.TryGetValue(name, out runtimeDefinedParameter))
			{
				psvariable.AddParameterAttributesNoChecks(runtimeDefinedParameter.Attributes);
			}
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00020618 File Offset: 0x0001E818
		internal object GetDefaultScriptParameterValue(RuntimeDefinedParameter parameter, IDictionary implicitUsingParameters = null)
		{
			object value = parameter.Value;
			Compiler.DefaultValueExpressionWrapper defaultValueExpressionWrapper = value as Compiler.DefaultValueExpressionWrapper;
			if (defaultValueExpressionWrapper != null)
			{
				value = defaultValueExpressionWrapper.GetValue(base.Context, this.Script.SessionStateInternal, implicitUsingParameters);
			}
			return value;
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x00020650 File Offset: 0x0001E850
		// (set) Token: 0x060006BB RID: 1723 RVA: 0x00020658 File Offset: 0x0001E858
		internal ScriptBlock Script { get; private set; }

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060006BC RID: 1724 RVA: 0x00020661 File Offset: 0x0001E861
		// (set) Token: 0x060006BD RID: 1725 RVA: 0x00020669 File Offset: 0x0001E869
		internal SessionStateScope LocalScope { get; set; }

		// Token: 0x040002C1 RID: 705
		private readonly CallSite<Func<CallSite, object, object>> _copyMutableValueSite = CallSite<Func<CallSite, object, object>>.Create(PSVariableAssignmentBinder.Get());
	}
}
