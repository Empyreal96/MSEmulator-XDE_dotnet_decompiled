using System;
using System.ComponentModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200004C RID: 76
	public interface IXdeAutomationGuestNotificationsPipe : IXdeAutomationPipe, INotifyPropertyChanged
	{
		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000195 RID: 405
		// (remove) Token: 0x06000196 RID: 406
		event EventHandler MicrophoneStarted;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000197 RID: 407
		// (remove) Token: 0x06000198 RID: 408
		event EventHandler MicrophoneStopped;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000199 RID: 409
		// (remove) Token: 0x0600019A RID: 410
		event EventHandler<GuestUpdatedEventArgs> GuestUpdated;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x0600019B RID: 411
		// (remove) Token: 0x0600019C RID: 412
		event EventHandler<ExEventArgs> ErrorEncountered;
	}
}
