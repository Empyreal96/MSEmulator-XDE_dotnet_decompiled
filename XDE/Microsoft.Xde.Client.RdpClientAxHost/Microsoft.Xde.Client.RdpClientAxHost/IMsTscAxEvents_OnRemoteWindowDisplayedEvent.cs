using System;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000018 RID: 24
	public class IMsTscAxEvents_OnRemoteWindowDisplayedEvent
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x00003603 File Offset: 0x00001803
		public IMsTscAxEvents_OnRemoteWindowDisplayedEvent(bool vbDisplayed, _RemotableHandle hwnd, RemoteWindowDisplayedAttribute windowAttribute)
		{
			this.vbDisplayed = vbDisplayed;
			this.hwnd = hwnd;
			this.windowAttribute = windowAttribute;
		}

		// Token: 0x04000034 RID: 52
		public bool vbDisplayed;

		// Token: 0x04000035 RID: 53
		public _RemotableHandle hwnd;

		// Token: 0x04000036 RID: 54
		public RemoteWindowDisplayedAttribute windowAttribute;
	}
}
