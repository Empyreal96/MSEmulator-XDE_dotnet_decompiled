using System;
using System.ComponentModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200004E RID: 78
	public interface IXdeMicrophonePipe : IXdeAutomationMicrophonePipe, IXdeAutomationPipe, INotifyPropertyChanged, IXdePipe, IXdeConnectionController, IDisposable
	{
		// Token: 0x0600019E RID: 414
		void SendMicrophoneDataToGuest(byte[] data, int size);
	}
}
