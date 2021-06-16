using System;
using System.ComponentModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200004F RID: 79
	public interface IXdeAutomationMicrophonePipe : IXdeAutomationPipe, INotifyPropertyChanged
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600019F RID: 415
		// (set) Token: 0x060001A0 RID: 416
		bool AutomationOverrideEnabled { get; set; }

		// Token: 0x060001A1 RID: 417
		void SendMicrophoneAutomationDataToGuest(byte[] data, int size);
	}
}
