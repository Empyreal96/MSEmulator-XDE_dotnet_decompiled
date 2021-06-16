using System;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200005A RID: 90
	public class IRemoteDesktopClientEvents_OnKeyCombinationPressedEvent
	{
		// Token: 0x06000EB0 RID: 3760 RVA: 0x00026FB3 File Offset: 0x000251B3
		public IRemoteDesktopClientEvents_OnKeyCombinationPressedEvent(int keyCombination)
		{
			this.keyCombination = keyCombination;
		}

		// Token: 0x04000305 RID: 773
		public int keyCombination;
	}
}
