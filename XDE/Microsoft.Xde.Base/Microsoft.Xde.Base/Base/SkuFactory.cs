using System;
using System.IO;
using System.Reflection;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Base
{
	// Token: 0x02000009 RID: 9
	public class SkuFactory : IXdeSkuFactory
	{
		// Token: 0x0600008F RID: 143 RVA: 0x00004310 File Offset: 0x00002510
		public IXdeSku LoadSkuFromName(string skuName, params object[] objs)
		{
			return Sku.LoadSkuFromName(this.GetFullSkuFileName(skuName), objs);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000431F File Offset: 0x0000251F
		private static string GetSkuFolder()
		{
			return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "SKUs");
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000433A File Offset: 0x0000253A
		private string GetFullSkuFileName(string skuName)
		{
			return Path.Combine(Path.IsPathRooted(skuName) ? skuName : SkuFactory.GetSkuFolder(), skuName, "xdesku.xml");
		}
	}
}
