using System;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000020 RID: 32
	public class IMsTscAxEvents_OnMouseInputModeChangedEvent
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x0000364D File Offset: 0x0000184D
		public IMsTscAxEvents_OnMouseInputModeChangedEvent(bool fMouseModeRelative)
		{
			this.fMouseModeRelative = fMouseModeRelative;
		}

		// Token: 0x0400003A RID: 58
		public bool fMouseModeRelative;
	}
}
