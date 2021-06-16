using System;
using System.ComponentModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000058 RID: 88
	public interface IXdeSimpleCommandsPipe : IXdePipe, IXdeAutomationPipe, INotifyPropertyChanged, IXdeConnectionController, IDisposable, IXdeAutomationSimpleCommandsPipe
	{
		// Token: 0x060001B7 RID: 439
		void SetupHttpProxyServer();

		// Token: 0x060001B8 RID: 440
		void SetupSocksProxyServer();

		// Token: 0x060001B9 RID: 441
		void SetupDnsServers();
	}
}
