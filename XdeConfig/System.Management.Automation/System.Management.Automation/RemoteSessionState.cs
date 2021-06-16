using System;

namespace System.Management.Automation
{
	// Token: 0x020002B8 RID: 696
	internal enum RemoteSessionState
	{
		// Token: 0x04000EE6 RID: 3814
		UndefinedState,
		// Token: 0x04000EE7 RID: 3815
		Idle,
		// Token: 0x04000EE8 RID: 3816
		Connecting,
		// Token: 0x04000EE9 RID: 3817
		Connected,
		// Token: 0x04000EEA RID: 3818
		NegotiationSending,
		// Token: 0x04000EEB RID: 3819
		NegotiationSendingOnConnect,
		// Token: 0x04000EEC RID: 3820
		NegotiationSent,
		// Token: 0x04000EED RID: 3821
		NegotiationReceived,
		// Token: 0x04000EEE RID: 3822
		NegotiationPending,
		// Token: 0x04000EEF RID: 3823
		ClosingConnection,
		// Token: 0x04000EF0 RID: 3824
		Closed,
		// Token: 0x04000EF1 RID: 3825
		Established,
		// Token: 0x04000EF2 RID: 3826
		EstablishedAndKeySent,
		// Token: 0x04000EF3 RID: 3827
		EstablishedAndKeyReceived,
		// Token: 0x04000EF4 RID: 3828
		EstablishedAndKeyRequested,
		// Token: 0x04000EF5 RID: 3829
		EstablishedAndKeyExchanged,
		// Token: 0x04000EF6 RID: 3830
		Disconnecting,
		// Token: 0x04000EF7 RID: 3831
		Disconnected,
		// Token: 0x04000EF8 RID: 3832
		Reconnecting,
		// Token: 0x04000EF9 RID: 3833
		RCDisconnecting,
		// Token: 0x04000EFA RID: 3834
		MaxState
	}
}
