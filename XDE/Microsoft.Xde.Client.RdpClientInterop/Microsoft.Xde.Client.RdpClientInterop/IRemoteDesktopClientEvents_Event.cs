using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000042 RID: 66
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	[ComVisible(false)]
	[ComEventInterface(typeof(IRemoteDesktopClientEvents), typeof(IRemoteDesktopClientEvents_EventProvider))]
	public interface IRemoteDesktopClientEvents_Event
	{
		// Token: 0x14000295 RID: 661
		// (add) Token: 0x06001E86 RID: 7814
		// (remove) Token: 0x06001E87 RID: 7815
		event IRemoteDesktopClientEvents_OnConnectingEventHandler OnConnecting;

		// Token: 0x14000296 RID: 662
		// (add) Token: 0x06001E88 RID: 7816
		// (remove) Token: 0x06001E89 RID: 7817
		event IRemoteDesktopClientEvents_OnConnectedEventHandler OnConnected;

		// Token: 0x14000297 RID: 663
		// (add) Token: 0x06001E8A RID: 7818
		// (remove) Token: 0x06001E8B RID: 7819
		event IRemoteDesktopClientEvents_OnLoginCompletedEventHandler OnLoginCompleted;

		// Token: 0x14000298 RID: 664
		// (add) Token: 0x06001E8C RID: 7820
		// (remove) Token: 0x06001E8D RID: 7821
		event IRemoteDesktopClientEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x14000299 RID: 665
		// (add) Token: 0x06001E8E RID: 7822
		// (remove) Token: 0x06001E8F RID: 7823
		event IRemoteDesktopClientEvents_OnStatusChangedEventHandler OnStatusChanged;

		// Token: 0x1400029A RID: 666
		// (add) Token: 0x06001E90 RID: 7824
		// (remove) Token: 0x06001E91 RID: 7825
		event IRemoteDesktopClientEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x1400029B RID: 667
		// (add) Token: 0x06001E92 RID: 7826
		// (remove) Token: 0x06001E93 RID: 7827
		event IRemoteDesktopClientEvents_OnAutoReconnectedEventHandler OnAutoReconnected;

		// Token: 0x1400029C RID: 668
		// (add) Token: 0x06001E94 RID: 7828
		// (remove) Token: 0x06001E95 RID: 7829
		event IRemoteDesktopClientEvents_OnDialogDisplayingEventHandler OnDialogDisplaying;

		// Token: 0x1400029D RID: 669
		// (add) Token: 0x06001E96 RID: 7830
		// (remove) Token: 0x06001E97 RID: 7831
		event IRemoteDesktopClientEvents_OnDialogDismissedEventHandler OnDialogDismissed;

		// Token: 0x1400029E RID: 670
		// (add) Token: 0x06001E98 RID: 7832
		// (remove) Token: 0x06001E99 RID: 7833
		event IRemoteDesktopClientEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x1400029F RID: 671
		// (add) Token: 0x06001E9A RID: 7834
		// (remove) Token: 0x06001E9B RID: 7835
		event IRemoteDesktopClientEvents_OnAdminMessageReceivedEventHandler OnAdminMessageReceived;

		// Token: 0x140002A0 RID: 672
		// (add) Token: 0x06001E9C RID: 7836
		// (remove) Token: 0x06001E9D RID: 7837
		event IRemoteDesktopClientEvents_OnKeyCombinationPressedEventHandler OnKeyCombinationPressed;

		// Token: 0x140002A1 RID: 673
		// (add) Token: 0x06001E9E RID: 7838
		// (remove) Token: 0x06001E9F RID: 7839
		event IRemoteDesktopClientEvents_OnRemoteDesktopSizeChangedEventHandler OnRemoteDesktopSizeChanged;
	}
}
