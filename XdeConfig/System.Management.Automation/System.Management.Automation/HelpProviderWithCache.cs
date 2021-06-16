using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x020001B5 RID: 437
	internal abstract class HelpProviderWithCache : HelpProvider
	{
		// Token: 0x0600144B RID: 5195 RVA: 0x0007C5B6 File Offset: 0x0007A7B6
		internal HelpProviderWithCache(HelpSystem helpSystem) : base(helpSystem)
		{
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x0007C8A4 File Offset: 0x0007AAA4
		internal override IEnumerable<HelpInfo> ExactMatchHelp(HelpRequest helpRequest)
		{
			string target = helpRequest.Target;
			if (!this.HasCustomMatch)
			{
				if (this._helpCache.Contains(target))
				{
					yield return (HelpInfo)this._helpCache[target];
				}
			}
			else
			{
				foreach (object obj in this._helpCache.Keys)
				{
					string key = (string)obj;
					if (this.CustomMatch(target, key))
					{
						yield return (HelpInfo)this._helpCache[key];
					}
				}
			}
			if (!this.CacheFullyLoaded)
			{
				this.DoExactMatchHelp(helpRequest);
				if (this._helpCache.Contains(target))
				{
					yield return (HelpInfo)this._helpCache[target];
				}
			}
			yield break;
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x0600144D RID: 5197 RVA: 0x0007C8C8 File Offset: 0x0007AAC8
		// (set) Token: 0x0600144E RID: 5198 RVA: 0x0007C8D0 File Offset: 0x0007AAD0
		protected bool HasCustomMatch
		{
			get
			{
				return this._hasCustomMatch;
			}
			set
			{
				this._hasCustomMatch = value;
			}
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0007C8D9 File Offset: 0x0007AAD9
		protected virtual bool CustomMatch(string target, string key)
		{
			return target == key;
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x0007C8E2 File Offset: 0x0007AAE2
		internal virtual void DoExactMatchHelp(HelpRequest helpRequest)
		{
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0007CC98 File Offset: 0x0007AE98
		internal override IEnumerable<HelpInfo> SearchHelp(HelpRequest helpRequest, bool searchOnlyContent)
		{
			string target = helpRequest.Target;
			string wildcardpattern = this.GetWildCardPattern(target);
			HelpRequest searchHelpRequest = helpRequest.Clone();
			searchHelpRequest.Target = wildcardpattern;
			if (!this.CacheFullyLoaded)
			{
				IEnumerable<HelpInfo> result = this.DoSearchHelp(searchHelpRequest);
				if (result != null)
				{
					foreach (HelpInfo helpInfoToReturn in result)
					{
						yield return helpInfoToReturn;
					}
				}
			}
			else
			{
				int countOfHelpInfoObjectsFound = 0;
				WildcardPattern helpMatchter = new WildcardPattern(wildcardpattern, WildcardOptions.IgnoreCase);
				foreach (object obj in this._helpCache.Keys)
				{
					string key = (string)obj;
					if ((!searchOnlyContent && helpMatchter.IsMatch(key)) || (searchOnlyContent && ((HelpInfo)this._helpCache[key]).MatchPatternInContent(helpMatchter)))
					{
						countOfHelpInfoObjectsFound++;
						yield return (HelpInfo)this._helpCache[key];
						if (helpRequest.MaxResults > 0 && countOfHelpInfoObjectsFound >= helpRequest.MaxResults)
						{
							yield break;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x0007CCC3 File Offset: 0x0007AEC3
		internal virtual string GetWildCardPattern(string target)
		{
			if (WildcardPattern.ContainsWildcardCharacters(target))
			{
				return target;
			}
			return "*" + target + "*";
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x0007CD80 File Offset: 0x0007AF80
		internal virtual IEnumerable<HelpInfo> DoSearchHelp(HelpRequest helpRequest)
		{
			yield break;
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x0007CD9D File Offset: 0x0007AF9D
		internal void AddCache(string target, HelpInfo helpInfo)
		{
			this._helpCache[target] = helpInfo;
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x0007CDAC File Offset: 0x0007AFAC
		internal HelpInfo GetCache(string target)
		{
			return (HelpInfo)this._helpCache[target];
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06001456 RID: 5206 RVA: 0x0007CDBF File Offset: 0x0007AFBF
		// (set) Token: 0x06001457 RID: 5207 RVA: 0x0007CDC7 File Offset: 0x0007AFC7
		protected internal bool CacheFullyLoaded
		{
			get
			{
				return this._cacheFullyLoaded;
			}
			set
			{
				this._cacheFullyLoaded = value;
			}
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x0007CDD0 File Offset: 0x0007AFD0
		internal override void Reset()
		{
			base.Reset();
			this._helpCache.Clear();
			this._cacheFullyLoaded = false;
		}

		// Token: 0x040008CF RID: 2255
		private Hashtable _helpCache = new Hashtable(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040008D0 RID: 2256
		private bool _hasCustomMatch;

		// Token: 0x040008D1 RID: 2257
		private bool _cacheFullyLoaded;
	}
}
