using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x020000A1 RID: 161
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	[ClassInterface(ClassInterfaceType.None)]
	public sealed class IMsTscAxEvents_SinkHelper : IMsTscAxEvents
	{
		// Token: 0x060024C0 RID: 9408 RVA: 0x00002050 File Offset: 0x00000250
		public void OnConnecting()
		{
			if (this.m_OnConnectingDelegate != null)
			{
				this.m_OnConnectingDelegate();
				return;
			}
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x0000207C File Offset: 0x0000027C
		public void OnConnected()
		{
			if (this.m_OnConnectedDelegate != null)
			{
				this.m_OnConnectedDelegate();
				return;
			}
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x000020A8 File Offset: 0x000002A8
		public void OnLoginComplete()
		{
			if (this.m_OnLoginCompleteDelegate != null)
			{
				this.m_OnLoginCompleteDelegate();
				return;
			}
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x000020D4 File Offset: 0x000002D4
		public void OnDisconnected(int A_1)
		{
			if (this.m_OnDisconnectedDelegate != null)
			{
				this.m_OnDisconnectedDelegate(A_1);
				return;
			}
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x00002104 File Offset: 0x00000304
		public void OnEnterFullScreenMode()
		{
			if (this.m_OnEnterFullScreenModeDelegate != null)
			{
				this.m_OnEnterFullScreenModeDelegate();
				return;
			}
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x00002130 File Offset: 0x00000330
		public void OnLeaveFullScreenMode()
		{
			if (this.m_OnLeaveFullScreenModeDelegate != null)
			{
				this.m_OnLeaveFullScreenModeDelegate();
				return;
			}
		}

		// Token: 0x060024C6 RID: 9414 RVA: 0x0000215C File Offset: 0x0000035C
		public void OnChannelReceivedData(string A_1, string A_2)
		{
			if (this.m_OnChannelReceivedDataDelegate != null)
			{
				this.m_OnChannelReceivedDataDelegate(A_1, A_2);
				return;
			}
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x00002190 File Offset: 0x00000390
		public void OnRequestGoFullScreen()
		{
			if (this.m_OnRequestGoFullScreenDelegate != null)
			{
				this.m_OnRequestGoFullScreenDelegate();
				return;
			}
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x000021BC File Offset: 0x000003BC
		public void OnRequestLeaveFullScreen()
		{
			if (this.m_OnRequestLeaveFullScreenDelegate != null)
			{
				this.m_OnRequestLeaveFullScreenDelegate();
				return;
			}
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x000021E8 File Offset: 0x000003E8
		public void OnFatalError(int A_1)
		{
			if (this.m_OnFatalErrorDelegate != null)
			{
				this.m_OnFatalErrorDelegate(A_1);
				return;
			}
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x00002218 File Offset: 0x00000418
		public void OnWarning(int A_1)
		{
			if (this.m_OnWarningDelegate != null)
			{
				this.m_OnWarningDelegate(A_1);
				return;
			}
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x00002248 File Offset: 0x00000448
		public void OnRemoteDesktopSizeChange(int A_1, int A_2)
		{
			if (this.m_OnRemoteDesktopSizeChangeDelegate != null)
			{
				this.m_OnRemoteDesktopSizeChangeDelegate(A_1, A_2);
				return;
			}
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x0000227C File Offset: 0x0000047C
		public void OnIdleTimeoutNotification()
		{
			if (this.m_OnIdleTimeoutNotificationDelegate != null)
			{
				this.m_OnIdleTimeoutNotificationDelegate();
				return;
			}
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x000022A8 File Offset: 0x000004A8
		public void OnRequestContainerMinimize()
		{
			if (this.m_OnRequestContainerMinimizeDelegate != null)
			{
				this.m_OnRequestContainerMinimizeDelegate();
				return;
			}
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x000022D4 File Offset: 0x000004D4
		public void OnConfirmClose(ref bool A_1)
		{
			if (this.m_OnConfirmCloseDelegate != null)
			{
				this.m_OnConfirmCloseDelegate(out A_1);
				return;
			}
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x00002304 File Offset: 0x00000504
		public void OnReceivedTSPublicKey(string A_1, ref bool A_2)
		{
			if (this.m_OnReceivedTSPublicKeyDelegate != null)
			{
				this.m_OnReceivedTSPublicKeyDelegate(A_1, out A_2);
				return;
			}
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x00002338 File Offset: 0x00000538
		public void OnAutoReconnecting(int A_1, int A_2, ref AutoReconnectContinueState A_3)
		{
			if (this.m_OnAutoReconnectingDelegate != null)
			{
				this.m_OnAutoReconnectingDelegate(A_1, A_2, out A_3);
				return;
			}
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x00002370 File Offset: 0x00000570
		public void OnAuthenticationWarningDisplayed()
		{
			if (this.m_OnAuthenticationWarningDisplayedDelegate != null)
			{
				this.m_OnAuthenticationWarningDisplayedDelegate();
				return;
			}
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x0000239C File Offset: 0x0000059C
		public void OnAuthenticationWarningDismissed()
		{
			if (this.m_OnAuthenticationWarningDismissedDelegate != null)
			{
				this.m_OnAuthenticationWarningDismissedDelegate();
				return;
			}
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x000023C8 File Offset: 0x000005C8
		public void OnRemoteProgramResult(string A_1, RemoteProgramResult A_2, bool A_3)
		{
			if (this.m_OnRemoteProgramResultDelegate != null)
			{
				this.m_OnRemoteProgramResultDelegate(A_1, A_2, A_3);
				return;
			}
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x00002400 File Offset: 0x00000600
		public void OnRemoteProgramDisplayed(bool A_1, uint A_2)
		{
			if (this.m_OnRemoteProgramDisplayedDelegate != null)
			{
				this.m_OnRemoteProgramDisplayedDelegate(A_1, A_2);
				return;
			}
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x00002434 File Offset: 0x00000634
		public void OnRemoteWindowDisplayed(bool A_1, ref _RemotableHandle A_2, RemoteWindowDisplayedAttribute A_3)
		{
			if (this.m_OnRemoteWindowDisplayedDelegate != null)
			{
				this.m_OnRemoteWindowDisplayedDelegate(A_1, ref A_2, A_3);
				return;
			}
		}

		// Token: 0x060024D6 RID: 9430 RVA: 0x0000246C File Offset: 0x0000066C
		public void OnLogonError(int A_1)
		{
			if (this.m_OnLogonErrorDelegate != null)
			{
				this.m_OnLogonErrorDelegate(A_1);
				return;
			}
		}

		// Token: 0x060024D7 RID: 9431 RVA: 0x0000249C File Offset: 0x0000069C
		public void OnFocusReleased(int A_1)
		{
			if (this.m_OnFocusReleasedDelegate != null)
			{
				this.m_OnFocusReleasedDelegate(A_1);
				return;
			}
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x000024CC File Offset: 0x000006CC
		public void OnUserNameAcquired(string A_1)
		{
			if (this.m_OnUserNameAcquiredDelegate != null)
			{
				this.m_OnUserNameAcquiredDelegate(A_1);
				return;
			}
		}

		// Token: 0x060024D9 RID: 9433 RVA: 0x000024FC File Offset: 0x000006FC
		public void OnMouseInputModeChanged(bool A_1)
		{
			if (this.m_OnMouseInputModeChangedDelegate != null)
			{
				this.m_OnMouseInputModeChangedDelegate(A_1);
				return;
			}
		}

		// Token: 0x060024DA RID: 9434 RVA: 0x0000252C File Offset: 0x0000072C
		public void OnServiceMessageReceived(string A_1)
		{
			if (this.m_OnServiceMessageReceivedDelegate != null)
			{
				this.m_OnServiceMessageReceivedDelegate(A_1);
				return;
			}
		}

		// Token: 0x060024DB RID: 9435 RVA: 0x0000255C File Offset: 0x0000075C
		public void OnConnectionBarPullDown()
		{
			if (this.m_OnConnectionBarPullDownDelegate != null)
			{
				this.m_OnConnectionBarPullDownDelegate();
				return;
			}
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x00002588 File Offset: 0x00000788
		public void OnNetworkBandwidthChanged(int A_1)
		{
			if (this.m_OnNetworkBandwidthChangedDelegate != null)
			{
				this.m_OnNetworkBandwidthChangedDelegate(A_1);
				return;
			}
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x000025B8 File Offset: 0x000007B8
		public void OnAutoReconnected()
		{
			if (this.m_OnAutoReconnectedDelegate != null)
			{
				this.m_OnAutoReconnectedDelegate();
				return;
			}
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x000025E4 File Offset: 0x000007E4
		internal IMsTscAxEvents_SinkHelper()
		{
			this.m_dwCookie = 0;
			this.m_OnConnectingDelegate = null;
			this.m_OnConnectedDelegate = null;
			this.m_OnLoginCompleteDelegate = null;
			this.m_OnDisconnectedDelegate = null;
			this.m_OnEnterFullScreenModeDelegate = null;
			this.m_OnLeaveFullScreenModeDelegate = null;
			this.m_OnChannelReceivedDataDelegate = null;
			this.m_OnRequestGoFullScreenDelegate = null;
			this.m_OnRequestLeaveFullScreenDelegate = null;
			this.m_OnFatalErrorDelegate = null;
			this.m_OnWarningDelegate = null;
			this.m_OnRemoteDesktopSizeChangeDelegate = null;
			this.m_OnIdleTimeoutNotificationDelegate = null;
			this.m_OnRequestContainerMinimizeDelegate = null;
			this.m_OnConfirmCloseDelegate = null;
			this.m_OnReceivedTSPublicKeyDelegate = null;
			this.m_OnAutoReconnectingDelegate = null;
			this.m_OnAuthenticationWarningDisplayedDelegate = null;
			this.m_OnAuthenticationWarningDismissedDelegate = null;
			this.m_OnRemoteProgramResultDelegate = null;
			this.m_OnRemoteProgramDisplayedDelegate = null;
			this.m_OnRemoteWindowDisplayedDelegate = null;
			this.m_OnLogonErrorDelegate = null;
			this.m_OnFocusReleasedDelegate = null;
			this.m_OnUserNameAcquiredDelegate = null;
			this.m_OnMouseInputModeChangedDelegate = null;
			this.m_OnServiceMessageReceivedDelegate = null;
			this.m_OnConnectionBarPullDownDelegate = null;
			this.m_OnNetworkBandwidthChangedDelegate = null;
			this.m_OnAutoReconnectedDelegate = null;
		}

		// Token: 0x0400009D RID: 157
		public IMsTscAxEvents_OnConnectingEventHandler m_OnConnectingDelegate;

		// Token: 0x0400009E RID: 158
		public IMsTscAxEvents_OnConnectedEventHandler m_OnConnectedDelegate;

		// Token: 0x0400009F RID: 159
		public IMsTscAxEvents_OnLoginCompleteEventHandler m_OnLoginCompleteDelegate;

		// Token: 0x040000A0 RID: 160
		public IMsTscAxEvents_OnDisconnectedEventHandler m_OnDisconnectedDelegate;

		// Token: 0x040000A1 RID: 161
		public IMsTscAxEvents_OnEnterFullScreenModeEventHandler m_OnEnterFullScreenModeDelegate;

		// Token: 0x040000A2 RID: 162
		public IMsTscAxEvents_OnLeaveFullScreenModeEventHandler m_OnLeaveFullScreenModeDelegate;

		// Token: 0x040000A3 RID: 163
		public IMsTscAxEvents_OnChannelReceivedDataEventHandler m_OnChannelReceivedDataDelegate;

		// Token: 0x040000A4 RID: 164
		public IMsTscAxEvents_OnRequestGoFullScreenEventHandler m_OnRequestGoFullScreenDelegate;

		// Token: 0x040000A5 RID: 165
		public IMsTscAxEvents_OnRequestLeaveFullScreenEventHandler m_OnRequestLeaveFullScreenDelegate;

		// Token: 0x040000A6 RID: 166
		public IMsTscAxEvents_OnFatalErrorEventHandler m_OnFatalErrorDelegate;

		// Token: 0x040000A7 RID: 167
		public IMsTscAxEvents_OnWarningEventHandler m_OnWarningDelegate;

		// Token: 0x040000A8 RID: 168
		public IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler m_OnRemoteDesktopSizeChangeDelegate;

		// Token: 0x040000A9 RID: 169
		public IMsTscAxEvents_OnIdleTimeoutNotificationEventHandler m_OnIdleTimeoutNotificationDelegate;

		// Token: 0x040000AA RID: 170
		public IMsTscAxEvents_OnRequestContainerMinimizeEventHandler m_OnRequestContainerMinimizeDelegate;

		// Token: 0x040000AB RID: 171
		public IMsTscAxEvents_OnConfirmCloseEventHandler m_OnConfirmCloseDelegate;

		// Token: 0x040000AC RID: 172
		public IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler m_OnReceivedTSPublicKeyDelegate;

		// Token: 0x040000AD RID: 173
		public IMsTscAxEvents_OnAutoReconnectingEventHandler m_OnAutoReconnectingDelegate;

		// Token: 0x040000AE RID: 174
		public IMsTscAxEvents_OnAuthenticationWarningDisplayedEventHandler m_OnAuthenticationWarningDisplayedDelegate;

		// Token: 0x040000AF RID: 175
		public IMsTscAxEvents_OnAuthenticationWarningDismissedEventHandler m_OnAuthenticationWarningDismissedDelegate;

		// Token: 0x040000B0 RID: 176
		public IMsTscAxEvents_OnRemoteProgramResultEventHandler m_OnRemoteProgramResultDelegate;

		// Token: 0x040000B1 RID: 177
		public IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler m_OnRemoteProgramDisplayedDelegate;

		// Token: 0x040000B2 RID: 178
		public IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler m_OnRemoteWindowDisplayedDelegate;

		// Token: 0x040000B3 RID: 179
		public IMsTscAxEvents_OnLogonErrorEventHandler m_OnLogonErrorDelegate;

		// Token: 0x040000B4 RID: 180
		public IMsTscAxEvents_OnFocusReleasedEventHandler m_OnFocusReleasedDelegate;

		// Token: 0x040000B5 RID: 181
		public IMsTscAxEvents_OnUserNameAcquiredEventHandler m_OnUserNameAcquiredDelegate;

		// Token: 0x040000B6 RID: 182
		public IMsTscAxEvents_OnMouseInputModeChangedEventHandler m_OnMouseInputModeChangedDelegate;

		// Token: 0x040000B7 RID: 183
		public IMsTscAxEvents_OnServiceMessageReceivedEventHandler m_OnServiceMessageReceivedDelegate;

		// Token: 0x040000B8 RID: 184
		public IMsTscAxEvents_OnConnectionBarPullDownEventHandler m_OnConnectionBarPullDownDelegate;

		// Token: 0x040000B9 RID: 185
		public IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler m_OnNetworkBandwidthChangedDelegate;

		// Token: 0x040000BA RID: 186
		public IMsTscAxEvents_OnAutoReconnectedEventHandler m_OnAutoReconnectedDelegate;

		// Token: 0x040000BB RID: 187
		public int m_dwCookie;
	}
}
