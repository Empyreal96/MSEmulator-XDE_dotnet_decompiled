using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000019 RID: 25
	public interface IXdeSkuFactory
	{
		// Token: 0x0600008D RID: 141
		IXdeSku LoadSkuFromName(string skuName, params object[] objs);
	}
}
