using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x020000A3 RID: 163
	[ClassInterface(ClassInterfaceType.None)]
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	public sealed class IRemoteDesktopClientEvents_SinkHelper : IRemoteDesktopClientEvents
	{
		// Token: 0x0600251F RID: 9503 RVA: 0x00005B90 File Offset: 0x00003D90
		public void OnConnecting()
		{
			if (this.m_OnConnectingDelegate != null)
			{
				this.m_OnConnectingDelegate();
				return;
			}
		}

		// Token: 0x06002520 RID: 9504 RVA: 0x00005BBC File Offset: 0x00003DBC
		public void OnConnected()
		{
			if (this.m_OnConnectedDelegate != null)
			{
				this.m_OnConnectedDelegate();
				return;
			}
		}

		// Token: 0x06002521 RID: 9505 RVA: 0x00005BE8 File Offset: 0x00003DE8
		public void OnLoginCompleted()
		{
			if (this.m_OnLoginCompletedDelegate != null)
			{
				this.m_OnLoginCompletedDelegate();
				return;
			}
		}

		// Token: 0x06002522 RID: 9506 RVA: 0x00005C14 File Offset: 0x00003E14
		public void OnDisconnected(int A_1, int A_2, string A_3)
		{
			if (this.m_OnDisconnectedDelegate != null)
			{
				this.m_OnDisconnectedDelegate(A_1, A_2, A_3);
				return;
			}
		}

		// Token: 0x06002523 RID: 9507 RVA: 0x00005C4C File Offset: 0x00003E4C
		public void OnStatusChanged(int A_1, string A_2)
		{
			if (this.m_OnStatusChangedDelegate != null)
			{
				this.m_OnStatusChangedDelegate(A_1, A_2);
				return;
			}
		}

		// Token: 0x06002524 RID: 9508 RVA: 0x00005C80 File Offset: 0x00003E80
		public void OnAutoReconnecting(int A_1, int A_2, string A_3, bool A_4, int A_5, int A_6)
		{
			if (this.m_OnAutoReconnectingDelegate != null)
			{
				this.m_OnAutoReconnectingDelegate(A_1, A_2, A_3, A_4, A_5, A_6);
				return;
			}
		}

		// Token: 0x06002525 RID: 9509 RVA: 0x00005CC4 File Offset: 0x00003EC4
		public void OnAutoReconnected()
		{
			if (this.m_OnAutoReconnectedDelegate != null)
			{
				this.m_OnAutoReconnectedDelegate();
				return;
			}
		}

		// Token: 0x06002526 RID: 9510 RVA: 0x00005CF0 File Offset: 0x00003EF0
		public void OnDialogDisplaying()
		{
			if (this.m_OnDialogDisplayingDelegate != null)
			{
				this.m_OnDialogDisplayingDelegate();
				return;
			}
		}

		// Token: 0x06002527 RID: 9511 RVA: 0x00005D1C File Offset: 0x00003F1C
		public void OnDialogDismissed()
		{
			if (this.m_OnDialogDismissedDelegate != null)
			{
				this.m_OnDialogDismissedDelegate();
				return;
			}
		}

		// Token: 0x06002528 RID: 9512 RVA: 0x00005D48 File Offset: 0x00003F48
		public void OnNetworkBandwidthChanged(int A_1, string A_2, string A_3)
		{
			if (this.m_OnNetworkBandwidthChangedDelegate != null)
			{
				this.m_OnNetworkBandwidthChangedDelegate(A_1, A_2, A_3);
				return;
			}
		}

		// Token: 0x06002529 RID: 9513 RVA: 0x00005D80 File Offset: 0x00003F80
		public void OnAdminMessageReceived(string A_1)
		{
			if (this.m_OnAdminMessageReceivedDelegate != null)
			{
				this.m_OnAdminMessageReceivedDelegate(A_1);
				return;
			}
		}

		// Token: 0x0600252A RID: 9514 RVA: 0x00005DB0 File Offset: 0x00003FB0
		public void OnKeyCombinationPressed(int A_1)
		{
			if (this.m_OnKeyCombinationPressedDelegate != null)
			{
				this.m_OnKeyCombinationPressedDelegate(A_1);
				return;
			}
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x00005DE0 File Offset: 0x00003FE0
		public void OnRemoteDesktopSizeChanged(int A_1, int A_2)
		{
			if (this.m_OnRemoteDesktopSizeChangedDelegate != null)
			{
				this.m_OnRemoteDesktopSizeChangedDelegate(A_1, A_2);
				return;
			}
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x00005E14 File Offset: 0x00004014
		internal IRemoteDesktopClientEvents_SinkHelper()
		{
			this.m_dwCookie = 0;
			this.m_OnConnectingDelegate = null;
			this.m_OnConnectedDelegate = null;
			this.m_OnLoginCompletedDelegate = null;
			this.m_OnDisconnectedDelegate = null;
			this.m_OnStatusChangedDelegate = null;
			this.m_OnAutoReconnectingDelegate = null;
			this.m_OnAutoReconnectedDelegate = null;
			this.m_OnDialogDisplayingDelegate = null;
			this.m_OnDialogDismissedDelegate = null;
			this.m_OnNetworkBandwidthChangedDelegate = null;
			this.m_OnAdminMessageReceivedDelegate = null;
			this.m_OnKeyCombinationPressedDelegate = null;
			this.m_OnRemoteDesktopSizeChangedDelegate = null;
		}

		// Token: 0x040000BF RID: 191
		public IRemoteDesktopClientEvents_OnConnectingEventHandler m_OnConnectingDelegate;

		// Token: 0x040000C0 RID: 192
		public IRemoteDesktopClientEvents_OnConnectedEventHandler m_OnConnectedDelegate;

		// Token: 0x040000C1 RID: 193
		public IRemoteDesktopClientEvents_OnLoginCompletedEventHandler m_OnLoginCompletedDelegate;

		// Token: 0x040000C2 RID: 194
		public IRemoteDesktopClientEvents_OnDisconnectedEventHandler m_OnDisconnectedDelegate;

		// Token: 0x040000C3 RID: 195
		public IRemoteDesktopClientEvents_OnStatusChangedEventHandler m_OnStatusChangedDelegate;

		// Token: 0x040000C4 RID: 196
		public IRemoteDesktopClientEvents_OnAutoReconnectingEventHandler m_OnAutoReconnectingDelegate;

		// Token: 0x040000C5 RID: 197
		public IRemoteDesktopClientEvents_OnAutoReconnectedEventHandler m_OnAutoReconnectedDelegate;

		// Token: 0x040000C6 RID: 198
		public IRemoteDesktopClientEvents_OnDialogDisplayingEventHandler m_OnDialogDisplayingDelegate;

		// Token: 0x040000C7 RID: 199
		public IRemoteDesktopClientEvents_OnDialogDismissedEventHandler m_OnDialogDismissedDelegate;

		// Token: 0x040000C8 RID: 200
		public IRemoteDesktopClientEvents_OnNetworkBandwidthChangedEventHandler m_OnNetworkBandwidthChangedDelegate;

		// Token: 0x040000C9 RID: 201
		public IRemoteDesktopClientEvents_OnAdminMessageReceivedEventHandler m_OnAdminMessageReceivedDelegate;

		// Token: 0x040000CA RID: 202
		public IRemoteDesktopClientEvents_OnKeyCombinationPressedEventHandler m_OnKeyCombinationPressedDelegate;

		// Token: 0x040000CB RID: 203
		public IRemoteDesktopClientEvents_OnRemoteDesktopSizeChangedEventHandler m_OnRemoteDesktopSizeChangedDelegate;

		// Token: 0x040000CC RID: 204
		public int m_dwCookie;
	}
}
