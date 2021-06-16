using System;
using System.Linq;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation.Language
{
	// Token: 0x02000996 RID: 2454
	public static class StaticParameterBinder
	{
		// Token: 0x06005A83 RID: 23171 RVA: 0x001E63A0 File Offset: 0x001E45A0
		public static StaticBindingResult BindCommand(CommandAst commandAst)
		{
			bool resolve = true;
			return StaticParameterBinder.BindCommand(commandAst, resolve);
		}

		// Token: 0x06005A84 RID: 23172 RVA: 0x001E63B8 File Offset: 0x001E45B8
		public static StaticBindingResult BindCommand(CommandAst commandAst, bool resolve)
		{
			string[] desiredParameters = null;
			return StaticParameterBinder.BindCommand(commandAst, resolve, desiredParameters);
		}

		// Token: 0x06005A85 RID: 23173 RVA: 0x001E63D0 File Offset: 0x001E45D0
		public static StaticBindingResult BindCommand(CommandAst commandAst, bool resolve, string[] desiredParameters)
		{
			if (desiredParameters != null && desiredParameters.Length > 0)
			{
				bool flag = false;
				foreach (CommandParameterAst commandParameterAst in commandAst.CommandElements.OfType<CommandParameterAst>())
				{
					string parameterName = commandParameterAst.ParameterName;
					foreach (string text in desiredParameters)
					{
						if (text.StartsWith(parameterName, StringComparison.OrdinalIgnoreCase))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (!flag)
				{
					return null;
				}
			}
			if (!resolve)
			{
				return new StaticBindingResult(commandAst, null);
			}
			PseudoBindingInfo bindingInfo = null;
			if (Runspace.DefaultRunspace == null)
			{
				lock (StaticParameterBinder.bindCommandlLock)
				{
					if (StaticParameterBinder.bindCommandPowerShell == null)
					{
						InitialSessionState initialSessionState = InitialSessionState.CreateDefault2();
						initialSessionState.Types.Clear();
						initialSessionState.Formats.Clear();
						StaticParameterBinder.bindCommandPowerShell = PowerShell.Create(initialSessionState);
					}
					Runspace.DefaultRunspace = StaticParameterBinder.bindCommandPowerShell.Runspace;
					bindingInfo = new PseudoParameterBinder().DoPseudoParameterBinding(commandAst, null, null, PseudoParameterBinder.BindingType.ArgumentBinding);
					Runspace.DefaultRunspace = null;
					goto IL_113;
				}
			}
			bindingInfo = new PseudoParameterBinder().DoPseudoParameterBinding(commandAst, null, null, PseudoParameterBinder.BindingType.ArgumentBinding);
			IL_113:
			return new StaticBindingResult(commandAst, bindingInfo);
		}

		// Token: 0x04003062 RID: 12386
		private static object bindCommandlLock = new object();

		// Token: 0x04003063 RID: 12387
		private static PowerShell bindCommandPowerShell = null;
	}
}
