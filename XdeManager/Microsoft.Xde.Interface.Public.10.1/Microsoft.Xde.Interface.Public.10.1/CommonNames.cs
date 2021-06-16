using System;
using System.Globalization;

namespace Microsoft.Xde.Interface
{
	// Token: 0x02000002 RID: 2
	public static class CommonNames
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		public static string GetEndPointName(string vm, EndPointType type)
		{
			return string.Format(CultureInfo.InvariantCulture, "net.pipe://localhost/XDEServer/{0}_{1}", vm, type);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002060 File Offset: 0x00000260
		public static string GetEndPointName(string vm, Type type)
		{
			return string.Format(CultureInfo.InvariantCulture, "net.pipe://localhost/XDEServer/{0}_{1}", vm, type);
		}

		// Token: 0x04000001 RID: 1
		public const string EventNamePrefix = "XdeOnServerInitialize";

		// Token: 0x04000002 RID: 2
		public const string XdeOwnershipMutexPrefix = "Microsoft.XDE.Ownership.";

		// Token: 0x04000003 RID: 3
		private const string EndPointFormat = "net.pipe://localhost/XDEServer/{0}_{1}";

		// Token: 0x04000004 RID: 4
		private const string FeatureEndPointFormat = "net.pipe://localhost/XDEServer/{0}_{1}";
	}
}
