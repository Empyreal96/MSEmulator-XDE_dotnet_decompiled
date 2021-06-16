using System;

namespace System.Management.Automation
{
	// Token: 0x020001D2 RID: 466
	internal class ScriptCommandHelpProvider : CommandHelpProvider
	{
		// Token: 0x0600157D RID: 5501 RVA: 0x000870EE File Offset: 0x000852EE
		internal ScriptCommandHelpProvider(HelpSystem helpSystem) : base(helpSystem)
		{
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x0600157E RID: 5502 RVA: 0x000870F7 File Offset: 0x000852F7
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.ScriptCommand | HelpCategory.Function | HelpCategory.Filter | HelpCategory.ExternalScript | HelpCategory.Workflow | HelpCategory.Configuration;
			}
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x00087100 File Offset: 0x00085300
		internal override CommandSearcher GetCommandSearcherForExactMatch(string commandName, ExecutionContext context)
		{
			return new CommandSearcher(commandName, SearchResolutionOptions.None, CommandTypes.Function | CommandTypes.Filter | CommandTypes.ExternalScript | CommandTypes.Configuration, context);
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x0008711C File Offset: 0x0008531C
		internal override CommandSearcher GetCommandSearcherForSearch(string pattern, ExecutionContext context)
		{
			return new CommandSearcher(pattern, SearchResolutionOptions.ResolveFunctionPatterns | SearchResolutionOptions.CommandNameIsPattern, CommandTypes.Function | CommandTypes.Filter | CommandTypes.ExternalScript | CommandTypes.Configuration, context);
		}
	}
}
