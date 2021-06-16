using System;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000008 RID: 8
	public class IMsTscAxEvents_OnFatalErrorEvent
	{
		// Token: 0x0600008B RID: 139 RVA: 0x0000356F File Offset: 0x0000176F
		public IMsTscAxEvents_OnFatalErrorEvent(int errorCode)
		{
			this.errorCode = errorCode;
		}

		// Token: 0x04000025 RID: 37
		public int errorCode;
	}
}
