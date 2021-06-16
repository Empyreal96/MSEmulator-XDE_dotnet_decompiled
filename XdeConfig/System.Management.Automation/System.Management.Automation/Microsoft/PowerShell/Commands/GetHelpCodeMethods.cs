using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020001AE RID: 430
	public static class GetHelpCodeMethods
	{
		// Token: 0x0600141B RID: 5147 RVA: 0x0007AC98 File Offset: 0x00078E98
		private static bool DoesCurrentRunspaceIncludeCoreHelpCmdlet()
		{
			InitialSessionState initialSessionState = Runspace.DefaultRunspace.InitialSessionState;
			if (initialSessionState != null)
			{
				IEnumerable<SessionStateCommandEntry> enumerable = from entry in initialSessionState.Commands["Get-Help"]
				where entry.Visibility == SessionStateEntryVisibility.Public
				select entry;
				if (enumerable.Count<SessionStateCommandEntry>() != 1)
				{
					return false;
				}
				foreach (SessionStateCommandEntry sessionStateCommandEntry in enumerable)
				{
					SessionStateCmdletEntry sessionStateCmdletEntry = sessionStateCommandEntry as SessionStateCmdletEntry;
					if (sessionStateCmdletEntry != null && sessionStateCmdletEntry.ImplementingType.Equals(typeof(GetHelpCommand)))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x0007AD6C File Offset: 0x00078F6C
		public static string GetHelpUri(PSObject commandInfoPSObject)
		{
			if (commandInfoPSObject == null)
			{
				return string.Empty;
			}
			CommandInfo commandInfo = PSObject.Base(commandInfoPSObject) as CommandInfo;
			if (commandInfo == null || string.IsNullOrEmpty(commandInfo.Name))
			{
				return string.Empty;
			}
			if ((commandInfo is CmdletInfo || commandInfo is FunctionInfo || commandInfo is ExternalScriptInfo || commandInfo is ScriptInfo) && !string.IsNullOrEmpty(commandInfo.CommandMetadata.HelpUri))
			{
				return commandInfo.CommandMetadata.HelpUri;
			}
			AliasInfo aliasInfo = commandInfo as AliasInfo;
			if (aliasInfo != null && aliasInfo.ExternalCommandMetadata != null && !string.IsNullOrEmpty(aliasInfo.ExternalCommandMetadata.HelpUri))
			{
				return aliasInfo.ExternalCommandMetadata.HelpUri;
			}
			string text = commandInfo.Name;
			if (!string.IsNullOrEmpty(commandInfo.ModuleName))
			{
				text = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", new object[]
				{
					commandInfo.ModuleName,
					commandInfo.Name
				});
			}
			if (GetHelpCodeMethods.DoesCurrentRunspaceIncludeCoreHelpCmdlet())
			{
				ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
				if (executionContextFromTLS == null || executionContextFromTLS.HelpSystem == null)
				{
					goto IL_27A;
				}
				HelpRequest helpRequest = new HelpRequest(text, commandInfo.HelpCategory);
				helpRequest.ProviderContext = new ProviderContext(string.Empty, executionContextFromTLS, executionContextFromTLS.SessionState.Path);
				helpRequest.CommandOrigin = CommandOrigin.Runspace;
				using (IEnumerator<Uri> enumerator = (from helpInfo in executionContextFromTLS.HelpSystem.ExactMatchHelp(helpRequest)
				select helpInfo.GetUriForOnlineHelp() into result
				where null != result
				select result).GetEnumerator())
				{
					if (!enumerator.MoveNext())
					{
						goto IL_27A;
					}
					Uri uri = enumerator.Current;
					return uri.OriginalString;
				}
			}
			PowerShell powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace).AddCommand("get-help").AddParameter("Name", text).AddParameter("Category", commandInfo.HelpCategory.ToString());
			try
			{
				Collection<PSObject> collection = powerShell.Invoke();
				if (collection != null)
				{
					for (int i = 0; i < collection.Count; i++)
					{
						HelpInfo helpInfo2;
						if (!LanguagePrimitives.TryConvertTo<HelpInfo>(collection[i], out helpInfo2))
						{
							Uri uriFromCommandPSObject = BaseCommandHelpInfo.GetUriFromCommandPSObject(collection[i]);
							return (uriFromCommandPSObject != null) ? uriFromCommandPSObject.OriginalString : string.Empty;
						}
						Uri uriForOnlineHelp = helpInfo2.GetUriForOnlineHelp();
						if (null != uriForOnlineHelp)
						{
							return uriForOnlineHelp.OriginalString;
						}
					}
				}
			}
			finally
			{
				powerShell.Dispose();
			}
			IL_27A:
			return string.Empty;
		}
	}
}
