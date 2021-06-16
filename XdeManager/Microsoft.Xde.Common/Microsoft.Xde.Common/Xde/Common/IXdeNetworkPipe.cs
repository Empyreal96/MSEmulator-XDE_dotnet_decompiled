using System;
using System.ComponentModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000050 RID: 80
	public interface IXdeNetworkPipe : IXdePipe, IXdeAutomationPipe, INotifyPropertyChanged, IXdeConnectionController, IDisposable
	{
	}
}
