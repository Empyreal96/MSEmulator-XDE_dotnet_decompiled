using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x020001C4 RID: 452
	internal abstract class HelpProviderWithFullCache : HelpProviderWithCache
	{
		// Token: 0x060014FB RID: 5371 RVA: 0x000831A2 File Offset: 0x000813A2
		internal HelpProviderWithFullCache(HelpSystem helpSystem) : base(helpSystem)
		{
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x000831AB File Offset: 0x000813AB
		internal sealed override IEnumerable<HelpInfo> ExactMatchHelp(HelpRequest helpRequest)
		{
			if (!base.CacheFullyLoaded || base.AreSnapInsSupported())
			{
				this.LoadCache();
			}
			base.CacheFullyLoaded = true;
			return base.ExactMatchHelp(helpRequest);
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x000831D1 File Offset: 0x000813D1
		internal sealed override void DoExactMatchHelp(HelpRequest helpRequest)
		{
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x000831D3 File Offset: 0x000813D3
		internal sealed override IEnumerable<HelpInfo> SearchHelp(HelpRequest helpRequest, bool searchOnlyContent)
		{
			if (!base.CacheFullyLoaded || base.AreSnapInsSupported())
			{
				this.LoadCache();
			}
			base.CacheFullyLoaded = true;
			return base.SearchHelp(helpRequest, searchOnlyContent);
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x000831FA File Offset: 0x000813FA
		internal sealed override IEnumerable<HelpInfo> DoSearchHelp(HelpRequest helpRequest)
		{
			return null;
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x000831FD File Offset: 0x000813FD
		internal virtual void LoadCache()
		{
		}
	}
}
