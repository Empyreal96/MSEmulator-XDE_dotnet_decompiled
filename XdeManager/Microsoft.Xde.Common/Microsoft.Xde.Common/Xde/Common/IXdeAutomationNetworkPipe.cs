using System;
using System.ComponentModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000051 RID: 81
	public interface IXdeAutomationNetworkPipe : IXdeAutomationPipe, INotifyPropertyChanged
	{
		// Token: 0x14000014 RID: 20
		// (add) Token: 0x060001A2 RID: 418
		// (remove) Token: 0x060001A3 RID: 419
		event EventHandler NduEtwSessionStartAttempted;
	}
}
