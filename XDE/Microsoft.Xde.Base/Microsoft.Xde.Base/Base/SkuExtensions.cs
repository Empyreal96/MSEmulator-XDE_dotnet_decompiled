using System;
using System.Linq;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Base
{
	// Token: 0x02000008 RID: 8
	public static class SkuExtensions
	{
		// Token: 0x0600008D RID: 141 RVA: 0x000042A8 File Offset: 0x000024A8
		public static bool ContainsFeature(this IXdeSku sku, string featureName)
		{
			return sku.Features.FirstOrDefault((IXdeFeature f) => f.Name == featureName) != null;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000042DC File Offset: 0x000024DC
		public static bool ContainsTab(this IXdeSku sku, string tabName)
		{
			return sku.Tabs.FirstOrDefault((IXdeTab t) => t.Name == tabName) != null;
		}
	}
}
