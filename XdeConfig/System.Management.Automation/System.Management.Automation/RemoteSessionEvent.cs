using System;

namespace System.Management.Automation
{
	// Token: 0x020002B9 RID: 697
	internal enum RemoteSessionEvent
	{
		// Token: 0x04000EFC RID: 3836
		InvalidEvent,
		// Token: 0x04000EFD RID: 3837
		CreateSession,
		// Token: 0x04000EFE RID: 3838
		ConnectSession,
		// Token: 0x04000EFF RID: 3839
		NegotiationSending,
		// Token: 0x04000F00 RID: 3840
		NegotiationSendingOnConnect,
		// Token: 0x04000F01 RID: 3841
		NegotiationSendCompleted,
		// Token: 0x04000F02 RID: 3842
		NegotiationReceived,
		// Token: 0x04000F03 RID: 3843
		NegotiationCompleted,
		// Token: 0x04000F04 RID: 3844
		NegotiationPending,
		// Token: 0x04000F05 RID: 3845
		Close,
		// Token: 0x04000F06 RID: 3846
		CloseCompleted,
		// Token: 0x04000F07 RID: 3847
		CloseFailed,
		// Token: 0x04000F08 RID: 3848
		ConnectFailed,
		// Token: 0x04000F09 RID: 3849
		NegotiationFailed,
		// Token: 0x04000F0A RID: 3850
		NegotiationTimeout,
		// Token: 0x04000F0B RID: 3851
		SendFailed,
		// Token: 0x04000F0C RID: 3852
		ReceiveFailed,
		// Token: 0x04000F0D RID: 3853
		FatalError,
		// Token: 0x04000F0E RID: 3854
		MessageReceived,
		// Token: 0x04000F0F RID: 3855
		KeySent,
		// Token: 0x04000F10 RID: 3856
		KeySendFailed,
		// Token: 0x04000F11 RID: 3857
		KeyReceived,
		// Token: 0x04000F12 RID: 3858
		KeyReceiveFailed,
		// Token: 0x04000F13 RID: 3859
		KeyRequested,
		// Token: 0x04000F14 RID: 3860
		KeyRequestFailed,
		// Token: 0x04000F15 RID: 3861
		DisconnectStart,
		// Token: 0x04000F16 RID: 3862
		DisconnectCompleted,
		// Token: 0x04000F17 RID: 3863
		DisconnectFailed,
		// Token: 0x04000F18 RID: 3864
		ReconnectStart,
		// Token: 0x04000F19 RID: 3865
		ReconnectCompleted,
		// Token: 0x04000F1A RID: 3866
		ReconnectFailed,
		// Token: 0x04000F1B RID: 3867
		RCDisconnectStarted,
		// Token: 0x04000F1C RID: 3868
		MaxEvent
	}
}
