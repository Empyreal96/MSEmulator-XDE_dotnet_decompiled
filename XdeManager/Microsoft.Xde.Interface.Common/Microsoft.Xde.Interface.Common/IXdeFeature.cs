using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200000C RID: 12
	public interface IXdeFeature : IXdePluginComponent, IDisposable
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600002A RID: 42
		IXdeConnectionController Connection { get; }
	}
}
