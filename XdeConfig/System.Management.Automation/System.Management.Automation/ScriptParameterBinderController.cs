using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x02000084 RID: 132
	internal class ScriptParameterBinderController : ParameterBinderController
	{
		// Token: 0x060006BE RID: 1726 RVA: 0x00020674 File Offset: 0x0001E874
		internal ScriptParameterBinderController(ScriptBlock script, InvocationInfo invocationInfo, ExecutionContext context, InternalCommand command, SessionStateScope localScope) : base(invocationInfo, context, new ScriptParameterBinder(script, invocationInfo, context, command, localScope))
		{
			this.DollarArgs = new List<object>();
			if (script.HasDynamicParameters)
			{
				base.UnboundParameters = base.BindableParameters.ReplaceMetadata(script.ParameterMetadata);
				return;
			}
			this._bindableParameters = script.ParameterMetadata;
			base.UnboundParameters = new List<MergedCompiledCommandParameter>(this._bindableParameters.BindableParameters.Values);
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060006BF RID: 1727 RVA: 0x000206E7 File Offset: 0x0001E8E7
		// (set) Token: 0x060006C0 RID: 1728 RVA: 0x000206EF File Offset: 0x0001E8EF
		internal List<object> DollarArgs { get; private set; }

		// Token: 0x060006C1 RID: 1729 RVA: 0x000206F8 File Offset: 0x0001E8F8
		internal void BindCommandLineParameters(Collection<CommandParameterInternal> arguments)
		{
			foreach (CommandParameterInternal item in arguments)
			{
				base.UnboundArguments.Add(item);
			}
			base.ReparseUnboundArguments();
			base.UnboundArguments = this.BindParameters(base.UnboundArguments);
			ParameterBindingException ex;
			base.UnboundArguments = base.BindPositionalParameters(base.UnboundArguments, uint.MaxValue, uint.MaxValue, out ex);
			try
			{
				base.DefaultParameterBinder.RecordBoundParameters = false;
				base.BindUnboundScriptParameters();
				this.HandleRemainingArguments(base.UnboundArguments);
			}
			finally
			{
				base.DefaultParameterBinder.RecordBoundParameters = true;
			}
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x000207AC File Offset: 0x0001E9AC
		internal override bool BindParameter(CommandParameterInternal argument, ParameterBindingFlags flags)
		{
			base.DefaultParameterBinder.BindParameter(argument.ParameterName, argument.ArgumentValue);
			return true;
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x000207C8 File Offset: 0x0001E9C8
		internal override Collection<CommandParameterInternal> BindParameters(Collection<CommandParameterInternal> arguments)
		{
			Collection<CommandParameterInternal> collection = new Collection<CommandParameterInternal>();
			foreach (CommandParameterInternal commandParameterInternal in arguments)
			{
				if (!commandParameterInternal.ParameterNameSpecified)
				{
					collection.Add(commandParameterInternal);
				}
				else
				{
					MergedCompiledCommandParameter matchingParameter = base.BindableParameters.GetMatchingParameter(commandParameterInternal.ParameterName, false, true, new InvocationInfo(base.InvocationInfo.MyCommand, commandParameterInternal.ParameterExtent));
					if (matchingParameter != null)
					{
						if (base.BoundParameters.ContainsKey(matchingParameter.Parameter.Name))
						{
							ParameterBindingException ex = new ParameterBindingException(ErrorCategory.InvalidArgument, base.InvocationInfo, base.GetParameterErrorExtent(commandParameterInternal), commandParameterInternal.ParameterName, null, null, ParameterBinderStrings.ParameterAlreadyBound, "ParameterAlreadyBound", new object[0]);
							throw ex;
						}
						this.BindParameter(uint.MaxValue, commandParameterInternal, matchingParameter, ParameterBindingFlags.ShouldCoerceType);
					}
					else if (commandParameterInternal.ParameterName.Equals("-%", StringComparison.Ordinal))
					{
						base.DefaultParameterBinder.CommandLineParameters.SetImplicitUsingParameters(commandParameterInternal.ArgumentValue);
					}
					else
					{
						collection.Add(commandParameterInternal);
					}
				}
			}
			return collection;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x000208E4 File Offset: 0x0001EAE4
		private void HandleRemainingArguments(Collection<CommandParameterInternal> arguments)
		{
			List<object> list = new List<object>();
			foreach (CommandParameterInternal commandParameterInternal in arguments)
			{
				object obj = commandParameterInternal.ArgumentSpecified ? commandParameterInternal.ArgumentValue : null;
				if (commandParameterInternal.ParameterAndArgumentSpecified && commandParameterInternal.ParameterName.Equals("$args", StringComparison.OrdinalIgnoreCase))
				{
					if (obj is object[])
					{
						list.AddRange(obj as object[]);
					}
					else
					{
						list.Add(obj);
					}
				}
				else
				{
					if (commandParameterInternal.ParameterNameSpecified)
					{
						PSObject psobject = new PSObject(new string(commandParameterInternal.ParameterText.ToCharArray()));
						if (psobject.Properties["<CommandParameterName>"] == null)
						{
							PSNoteProperty member = new PSNoteProperty("<CommandParameterName>", commandParameterInternal.ParameterName)
							{
								IsHidden = true
							};
							psobject.Properties.Add(member);
						}
						list.Add(psobject);
					}
					if (commandParameterInternal.ArgumentSpecified)
					{
						list.Add(obj);
					}
				}
			}
			object[] array = list.ToArray();
			base.DefaultParameterBinder.BindParameter("args", array);
			this.DollarArgs.AddRange(array);
		}

		// Token: 0x040002C4 RID: 708
		internal const string NotePropertyNameForSplattingParametersInArgs = "<CommandParameterName>";
	}
}
