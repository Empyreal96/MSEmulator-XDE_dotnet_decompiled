using System;
using System.ComponentModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200004B RID: 75
	public interface IXdeGuestNotificationsPipe : IXdePipe, IXdeAutomationPipe, INotifyPropertyChanged, IXdeConnectionController, IDisposable, IXdeAutomationGuestNotificationsPipe
	{
		// Token: 0x06000193 RID: 403
		void StartListening();

		// Token: 0x06000194 RID: 404
		void StopListening();
	}
}
