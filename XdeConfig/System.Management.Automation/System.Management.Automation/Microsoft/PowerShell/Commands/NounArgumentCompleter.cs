using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000070 RID: 112
	public class NounArgumentCompleter : IArgumentCompleter
	{
		// Token: 0x06000611 RID: 1553 RVA: 0x0001C610 File Offset: 0x0001A810
		public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst, IDictionary fakeBoundParameters)
		{
			if (fakeBoundParameters == null)
			{
				throw PSTraceSource.NewArgumentNullException("fakeBoundParameters");
			}
			CmdletInfo commandInfo = new CmdletInfo("Get-Command", typeof(GetCommandCommand));
			PowerShell powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace).AddCommand(commandInfo).AddParameter("Noun", wordToComplete + "*");
			if (fakeBoundParameters.Contains("Module"))
			{
				powerShell.AddParameter("Module", fakeBoundParameters["Module"]);
			}
			HashSet<string> hashSet = new HashSet<string>();
			Collection<CommandInfo> collection = powerShell.Invoke<CommandInfo>();
			foreach (CommandInfo commandInfo2 in collection)
			{
				int num = commandInfo2.Name.IndexOf('-');
				if (num != -1)
				{
					hashSet.Add(commandInfo2.Name.Substring(num + 1));
				}
			}
			return from noun in hashSet
			orderby noun
			select new CompletionResult(noun, noun, CompletionResultType.Text, noun);
		}
	}
}
