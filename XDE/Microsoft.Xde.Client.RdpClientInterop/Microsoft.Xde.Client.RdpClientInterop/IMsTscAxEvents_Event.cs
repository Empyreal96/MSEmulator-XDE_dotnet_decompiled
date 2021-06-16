using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000005 RID: 5
	[ComEventInterface(typeof(IMsTscAxEvents), typeof(IMsTscAxEvents_EventProvider))]
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	[ComVisible(false)]
	public interface IMsTscAxEvents_Event
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600003D RID: 61
		// (remove) Token: 0x0600003E RID: 62
		event IMsTscAxEvents_OnConnectingEventHandler OnConnecting;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600003F RID: 63
		// (remove) Token: 0x06000040 RID: 64
		event IMsTscAxEvents_OnConnectedEventHandler OnConnected;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000041 RID: 65
		// (remove) Token: 0x06000042 RID: 66
		event IMsTscAxEvents_OnLoginCompleteEventHandler OnLoginComplete;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000043 RID: 67
		// (remove) Token: 0x06000044 RID: 68
		event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000045 RID: 69
		// (remove) Token: 0x06000046 RID: 70
		event IMsTscAxEvents_OnEnterFullScreenModeEventHandler OnEnterFullScreenMode;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000047 RID: 71
		// (remove) Token: 0x06000048 RID: 72
		event IMsTscAxEvents_OnLeaveFullScreenModeEventHandler OnLeaveFullScreenMode;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000049 RID: 73
		// (remove) Token: 0x0600004A RID: 74
		event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600004B RID: 75
		// (remove) Token: 0x0600004C RID: 76
		event IMsTscAxEvents_OnRequestGoFullScreenEventHandler OnRequestGoFullScreen;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600004D RID: 77
		// (remove) Token: 0x0600004E RID: 78
		event IMsTscAxEvents_OnRequestLeaveFullScreenEventHandler OnRequestLeaveFullScreen;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600004F RID: 79
		// (remove) Token: 0x06000050 RID: 80
		event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000051 RID: 81
		// (remove) Token: 0x06000052 RID: 82
		event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000053 RID: 83
		// (remove) Token: 0x06000054 RID: 84
		event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000055 RID: 85
		// (remove) Token: 0x06000056 RID: 86
		event IMsTscAxEvents_OnIdleTimeoutNotificationEventHandler OnIdleTimeoutNotification;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000057 RID: 87
		// (remove) Token: 0x06000058 RID: 88
		event IMsTscAxEvents_OnRequestContainerMinimizeEventHandler OnRequestContainerMinimize;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000059 RID: 89
		// (remove) Token: 0x0600005A RID: 90
		event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x0600005B RID: 91
		// (remove) Token: 0x0600005C RID: 92
		event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600005D RID: 93
		// (remove) Token: 0x0600005E RID: 94
		event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x0600005F RID: 95
		// (remove) Token: 0x06000060 RID: 96
		event IMsTscAxEvents_OnAuthenticationWarningDisplayedEventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000061 RID: 97
		// (remove) Token: 0x06000062 RID: 98
		event IMsTscAxEvents_OnAuthenticationWarningDismissedEventHandler OnAuthenticationWarningDismissed;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000063 RID: 99
		// (remove) Token: 0x06000064 RID: 100
		event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000065 RID: 101
		// (remove) Token: 0x06000066 RID: 102
		event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000067 RID: 103
		// (remove) Token: 0x06000068 RID: 104
		event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000069 RID: 105
		// (remove) Token: 0x0600006A RID: 106
		event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x0600006B RID: 107
		// (remove) Token: 0x0600006C RID: 108
		event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x0600006D RID: 109
		// (remove) Token: 0x0600006E RID: 110
		event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x0600006F RID: 111
		// (remove) Token: 0x06000070 RID: 112
		event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06000071 RID: 113
		// (remove) Token: 0x06000072 RID: 114
		event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000073 RID: 115
		// (remove) Token: 0x06000074 RID: 116
		event IMsTscAxEvents_OnConnectionBarPullDownEventHandler OnConnectionBarPullDown;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06000075 RID: 117
		// (remove) Token: 0x06000076 RID: 118
		event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06000077 RID: 119
		// (remove) Token: 0x06000078 RID: 120
		event IMsTscAxEvents_OnAutoReconnectedEventHandler OnAutoReconnected;
	}
}
