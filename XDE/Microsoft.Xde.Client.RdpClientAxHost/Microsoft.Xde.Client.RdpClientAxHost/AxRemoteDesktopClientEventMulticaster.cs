using System;
using System.Runtime.InteropServices;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200005D RID: 93
	[ClassInterface(ClassInterfaceType.None)]
	public class AxRemoteDesktopClientEventMulticaster : IRemoteDesktopClientEvents
	{
		// Token: 0x06000EB6 RID: 3766 RVA: 0x00026FD8 File Offset: 0x000251D8
		public AxRemoteDesktopClientEventMulticaster(AxRemoteDesktopClient parent)
		{
			this.parent = parent;
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x00026FE8 File Offset: 0x000251E8
		public virtual void OnConnecting()
		{
			EventArgs e = new EventArgs();
			this.parent.RaiseOnOnConnecting(this.parent, e);
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x00027010 File Offset: 0x00025210
		public virtual void OnConnected()
		{
			EventArgs e = new EventArgs();
			this.parent.RaiseOnOnConnected(this.parent, e);
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x00027038 File Offset: 0x00025238
		public virtual void OnLoginCompleted()
		{
			EventArgs e = new EventArgs();
			this.parent.RaiseOnOnLoginCompleted(this.parent, e);
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x00027060 File Offset: 0x00025260
		public virtual void OnDisconnected(int disconnectReason, int extendedDisconnectReason, string disconnectErrorMessage)
		{
			IRemoteDesktopClientEvents_OnDisconnectedEvent e = new IRemoteDesktopClientEvents_OnDisconnectedEvent(disconnectReason, extendedDisconnectReason, disconnectErrorMessage);
			this.parent.RaiseOnOnDisconnected(this.parent, e);
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x00027088 File Offset: 0x00025288
		public virtual void OnStatusChanged(int statusCode, string statusMessage)
		{
			IRemoteDesktopClientEvents_OnStatusChangedEvent e = new IRemoteDesktopClientEvents_OnStatusChangedEvent(statusCode, statusMessage);
			this.parent.RaiseOnOnStatusChanged(this.parent, e);
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x000270B0 File Offset: 0x000252B0
		public virtual void OnAutoReconnecting(int disconnectReason, int extendedDisconnectReason, string disconnectErrorMessage, bool networkAvailable, int attemptCount, int maxAttemptCount)
		{
			IRemoteDesktopClientEvents_OnAutoReconnectingEvent e = new IRemoteDesktopClientEvents_OnAutoReconnectingEvent(disconnectReason, extendedDisconnectReason, disconnectErrorMessage, networkAvailable, attemptCount, maxAttemptCount);
			this.parent.RaiseOnOnAutoReconnecting(this.parent, e);
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x000270E0 File Offset: 0x000252E0
		public virtual void OnAutoReconnected()
		{
			EventArgs e = new EventArgs();
			this.parent.RaiseOnOnAutoReconnected(this.parent, e);
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x00027108 File Offset: 0x00025308
		public virtual void OnDialogDisplaying()
		{
			EventArgs e = new EventArgs();
			this.parent.RaiseOnOnDialogDisplaying(this.parent, e);
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x00027130 File Offset: 0x00025330
		public virtual void OnDialogDismissed()
		{
			EventArgs e = new EventArgs();
			this.parent.RaiseOnOnDialogDismissed(this.parent, e);
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x00027158 File Offset: 0x00025358
		public virtual void OnNetworkBandwidthChanged(int qualityLevel, string bandwidth, string latency)
		{
			IRemoteDesktopClientEvents_OnNetworkBandwidthChangedEvent e = new IRemoteDesktopClientEvents_OnNetworkBandwidthChangedEvent(qualityLevel, bandwidth, latency);
			this.parent.RaiseOnOnNetworkBandwidthChanged(this.parent, e);
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x00027180 File Offset: 0x00025380
		public virtual void OnAdminMessageReceived(string adminMessage)
		{
			IRemoteDesktopClientEvents_OnAdminMessageReceivedEvent e = new IRemoteDesktopClientEvents_OnAdminMessageReceivedEvent(adminMessage);
			this.parent.RaiseOnOnAdminMessageReceived(this.parent, e);
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x000271A8 File Offset: 0x000253A8
		public virtual void OnKeyCombinationPressed(int keyCombination)
		{
			IRemoteDesktopClientEvents_OnKeyCombinationPressedEvent e = new IRemoteDesktopClientEvents_OnKeyCombinationPressedEvent(keyCombination);
			this.parent.RaiseOnOnKeyCombinationPressed(this.parent, e);
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x000271D0 File Offset: 0x000253D0
		public virtual void OnRemoteDesktopSizeChanged(int width, int height)
		{
			IRemoteDesktopClientEvents_OnRemoteDesktopSizeChangedEvent e = new IRemoteDesktopClientEvents_OnRemoteDesktopSizeChangedEvent(width, height);
			this.parent.RaiseOnOnRemoteDesktopSizeChanged(this.parent, e);
		}

		// Token: 0x04000308 RID: 776
		private AxRemoteDesktopClient parent;
	}
}
