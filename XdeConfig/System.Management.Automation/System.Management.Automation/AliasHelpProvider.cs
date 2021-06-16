using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x020001BB RID: 443
	internal class AliasHelpProvider : HelpProvider
	{
		// Token: 0x0600149C RID: 5276 RVA: 0x0007FAC1 File Offset: 0x0007DCC1
		internal AliasHelpProvider(HelpSystem helpSystem) : base(helpSystem)
		{
			this._sessionState = helpSystem.ExecutionContext.SessionState;
			this._commandDiscovery = helpSystem.ExecutionContext.CommandDiscovery;
			this._context = helpSystem.ExecutionContext;
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x0600149D RID: 5277 RVA: 0x0007FAF8 File Offset: 0x0007DCF8
		internal override string Name
		{
			get
			{
				return "Alias Help Provider";
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x0600149E RID: 5278 RVA: 0x0007FAFF File Offset: 0x0007DCFF
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.Alias;
			}
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x0007FC5C File Offset: 0x0007DE5C
		internal override IEnumerable<HelpInfo> ExactMatchHelp(HelpRequest helpRequest)
		{
			CommandInfo commandInfo = null;
			try
			{
				commandInfo = this._commandDiscovery.LookupCommandInfo(helpRequest.Target);
			}
			catch (CommandNotFoundException)
			{
			}
			if (commandInfo != null && commandInfo.CommandType == CommandTypes.Alias)
			{
				AliasInfo aliasInfo = (AliasInfo)commandInfo;
				HelpInfo helpInfo = AliasHelpInfo.GetHelpInfo(aliasInfo);
				if (helpInfo != null)
				{
					yield return helpInfo;
				}
			}
			yield break;
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x000802B0 File Offset: 0x0007E4B0
		internal override IEnumerable<HelpInfo> SearchHelp(HelpRequest helpRequest, bool searchOnlyContent)
		{
			if (!searchOnlyContent)
			{
				string target = helpRequest.Target;
				string pattern = target;
				Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
				if (!WildcardPattern.ContainsWildcardCharacters(target))
				{
					pattern += "*";
				}
				WildcardPattern matcher = new WildcardPattern(pattern, WildcardOptions.IgnoreCase);
				IDictionary<string, AliasInfo> aliasTable = this._sessionState.Internal.GetAliasTable();
				foreach (string name in aliasTable.Keys)
				{
					if (matcher.IsMatch(name))
					{
						HelpRequest exactMatchHelpRequest = helpRequest.Clone();
						exactMatchHelpRequest.Target = name;
						foreach (HelpInfo helpInfo in this.ExactMatchHelp(exactMatchHelpRequest))
						{
							if (AliasHelpProvider.Match(helpInfo, helpRequest) && !hashtable.ContainsKey(name))
							{
								hashtable.Add(name, null);
								yield return helpInfo;
							}
						}
					}
				}
				CommandSearcher searcher = new CommandSearcher(pattern, SearchResolutionOptions.ResolveAliasPatterns, CommandTypes.Alias, this._context);
				while (searcher.MoveNext())
				{
					CommandInfo current = ((IEnumerator<CommandInfo>)searcher).Current;
					if (this._context.CurrentPipelineStopping)
					{
						goto IL_424;
					}
					AliasInfo alias = current as AliasInfo;
					if (alias != null)
					{
						string name2 = alias.Name;
						HelpRequest exactMatchHelpRequest2 = helpRequest.Clone();
						exactMatchHelpRequest2.Target = name2;
						foreach (HelpInfo helpInfo2 in this.ExactMatchHelp(exactMatchHelpRequest2))
						{
							if (AliasHelpProvider.Match(helpInfo2, helpRequest) && !hashtable.ContainsKey(name2))
							{
								hashtable.Add(name2, null);
								yield return helpInfo2;
							}
						}
					}
				}
				foreach (CommandInfo current2 in ModuleUtils.GetMatchingCommands(pattern, this._context, helpRequest.CommandOrigin, false, false))
				{
					if (this._context.CurrentPipelineStopping)
					{
						yield break;
					}
					AliasInfo alias2 = current2 as AliasInfo;
					if (alias2 != null)
					{
						string name3 = alias2.Name;
						HelpInfo helpInfo3 = AliasHelpInfo.GetHelpInfo(alias2);
						if (!hashtable.ContainsKey(name3))
						{
							hashtable.Add(name3, null);
							yield return helpInfo3;
						}
					}
				}
			}
			IL_424:
			yield break;
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x000802DC File Offset: 0x0007E4DC
		private static bool Match(HelpInfo helpInfo, HelpRequest helpRequest)
		{
			return helpRequest == null || ((helpRequest.HelpCategory & helpInfo.HelpCategory) != HelpCategory.None && AliasHelpProvider.Match(helpInfo.Component, helpRequest.Component) && AliasHelpProvider.Match(helpInfo.Role, helpRequest.Role) && AliasHelpProvider.Match(helpInfo.Functionality, helpRequest.Functionality));
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x00080340 File Offset: 0x0007E540
		private static bool Match(string target, string[] patterns)
		{
			if (patterns == null || patterns.Length == 0)
			{
				return true;
			}
			foreach (string pattern in patterns)
			{
				if (AliasHelpProvider.Match(target, pattern))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x0008037C File Offset: 0x0007E57C
		private static bool Match(string target, string pattern)
		{
			if (string.IsNullOrEmpty(pattern))
			{
				return true;
			}
			if (string.IsNullOrEmpty(target))
			{
				target = "";
			}
			WildcardPattern wildcardPattern = new WildcardPattern(pattern, WildcardOptions.IgnoreCase);
			return wildcardPattern.IsMatch(target);
		}

		// Token: 0x040008E0 RID: 2272
		private readonly ExecutionContext _context;

		// Token: 0x040008E1 RID: 2273
		private SessionState _sessionState;

		// Token: 0x040008E2 RID: 2274
		private CommandDiscovery _commandDiscovery;
	}
}
