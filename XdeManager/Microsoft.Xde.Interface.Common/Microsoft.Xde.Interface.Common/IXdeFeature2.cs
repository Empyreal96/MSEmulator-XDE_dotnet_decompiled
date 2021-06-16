using System;
using System.Collections.Generic;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000002 RID: 2
	public interface IXdeFeature2 : IXdeFeature, IXdePluginComponent, IDisposable
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1
		IEnumerable<IXdeConnectionController> Connections { get; }
	}
}
