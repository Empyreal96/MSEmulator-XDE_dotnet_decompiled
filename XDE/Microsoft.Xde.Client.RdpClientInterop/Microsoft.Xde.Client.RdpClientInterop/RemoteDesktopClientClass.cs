using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000044 RID: 68
	[TypeLibType(TypeLibTypeFlags.FCanCreate)]
	[ComSourceInterfaces("Microsoft.Xde.Client.RdpClientInterop.IRemoteDesktopClientEvents\0\0")]
	[ClassInterface(ClassInterfaceType.None)]
	[Guid("EAB16C5D-EED1-4E95-868B-0FBA1B42C092")]
	[ComImport]
	public class RemoteDesktopClientClass : IRemoteDesktopClient, RemoteDesktopClient, IRemoteDesktopClientEvents_Event
	{
		// Token: 0x06001EA0 RID: 7840
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern RemoteDesktopClientClass();

		// Token: 0x06001EA1 RID: 7841
		[DispId(701)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void Connect();

		// Token: 0x06001EA2 RID: 7842
		[DispId(702)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void Disconnect();

		// Token: 0x06001EA3 RID: 7843
		[DispId(703)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void DeleteSavedCredentials([MarshalAs(UnmanagedType.BStr)] [In] string serverName);

		// Token: 0x17000E52 RID: 3666
		// (get) Token: 0x06001EA4 RID: 7844
		[DispId(710)]
		public virtual extern IRemoteDesktopClientSettings Settings { [DispId(710)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000E53 RID: 3667
		// (get) Token: 0x06001EA5 RID: 7845
		[DispId(711)]
		public virtual extern IRemoteDesktopClientActions Actions { [DispId(711)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x140002A2 RID: 674
		// (add) Token: 0x06001EA6 RID: 7846
		// (remove) Token: 0x06001EA7 RID: 7847
		public virtual extern event IRemoteDesktopClientEvents_OnConnectingEventHandler OnConnecting;

		// Token: 0x140002A3 RID: 675
		// (add) Token: 0x06001EA8 RID: 7848
		// (remove) Token: 0x06001EA9 RID: 7849
		public virtual extern event IRemoteDesktopClientEvents_OnConnectedEventHandler OnConnected;

		// Token: 0x140002A4 RID: 676
		// (add) Token: 0x06001EAA RID: 7850
		// (remove) Token: 0x06001EAB RID: 7851
		public virtual extern event IRemoteDesktopClientEvents_OnLoginCompletedEventHandler OnLoginCompleted;

		// Token: 0x140002A5 RID: 677
		// (add) Token: 0x06001EAC RID: 7852
		// (remove) Token: 0x06001EAD RID: 7853
		public virtual extern event IRemoteDesktopClientEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x140002A6 RID: 678
		// (add) Token: 0x06001EAE RID: 7854
		// (remove) Token: 0x06001EAF RID: 7855
		public virtual extern event IRemoteDesktopClientEvents_OnStatusChangedEventHandler OnStatusChanged;

		// Token: 0x140002A7 RID: 679
		// (add) Token: 0x06001EB0 RID: 7856
		// (remove) Token: 0x06001EB1 RID: 7857
		public virtual extern event IRemoteDesktopClientEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x140002A8 RID: 680
		// (add) Token: 0x06001EB2 RID: 7858
		// (remove) Token: 0x06001EB3 RID: 7859
		public virtual extern event IRemoteDesktopClientEvents_OnAutoReconnectedEventHandler OnAutoReconnected;

		// Token: 0x140002A9 RID: 681
		// (add) Token: 0x06001EB4 RID: 7860
		// (remove) Token: 0x06001EB5 RID: 7861
		public virtual extern event IRemoteDesktopClientEvents_OnDialogDisplayingEventHandler OnDialogDisplaying;

		// Token: 0x140002AA RID: 682
		// (add) Token: 0x06001EB6 RID: 7862
		// (remove) Token: 0x06001EB7 RID: 7863
		public virtual extern event IRemoteDesktopClientEvents_OnDialogDismissedEventHandler OnDialogDismissed;

		// Token: 0x140002AB RID: 683
		// (add) Token: 0x06001EB8 RID: 7864
		// (remove) Token: 0x06001EB9 RID: 7865
		public virtual extern event IRemoteDesktopClientEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x140002AC RID: 684
		// (add) Token: 0x06001EBA RID: 7866
		// (remove) Token: 0x06001EBB RID: 7867
		public virtual extern event IRemoteDesktopClientEvents_OnAdminMessageReceivedEventHandler OnAdminMessageReceived;

		// Token: 0x140002AD RID: 685
		// (add) Token: 0x06001EBC RID: 7868
		// (remove) Token: 0x06001EBD RID: 7869
		public virtual extern event IRemoteDesktopClientEvents_OnKeyCombinationPressedEventHandler OnKeyCombinationPressed;

		// Token: 0x140002AE RID: 686
		// (add) Token: 0x06001EBE RID: 7870
		// (remove) Token: 0x06001EBF RID: 7871
		public virtual extern event IRemoteDesktopClientEvents_OnRemoteDesktopSizeChangedEventHandler OnRemoteDesktopSizeChanged;
	}
}
