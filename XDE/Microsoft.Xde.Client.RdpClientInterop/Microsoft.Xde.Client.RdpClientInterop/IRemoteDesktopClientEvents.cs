using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000041 RID: 65
	[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	[Guid("079863B7-6D47-4105-8BFE-0CDCB360E67D")]
	[TypeLibType(TypeLibTypeFlags.FDispatchable)]
	[ComImport]
	public interface IRemoteDesktopClientEvents
	{
		// Token: 0x06001E79 RID: 7801
		[DispId(740)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnConnecting();

		// Token: 0x06001E7A RID: 7802
		[DispId(741)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnConnected();

		// Token: 0x06001E7B RID: 7803
		[DispId(742)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnLoginCompleted();

		// Token: 0x06001E7C RID: 7804
		[DispId(743)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnDisconnected([In] int disconnectReason, [In] int ExtendedDisconnectReason, [MarshalAs(UnmanagedType.BStr)] [In] string disconnectErrorMessage);

		// Token: 0x06001E7D RID: 7805
		[DispId(744)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnStatusChanged([In] int statusCode, [MarshalAs(UnmanagedType.BStr)] [In] string statusMessage);

		// Token: 0x06001E7E RID: 7806
		[DispId(745)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnAutoReconnecting([In] int disconnectReason, [In] int ExtendedDisconnectReason, [MarshalAs(UnmanagedType.BStr)] [In] string disconnectErrorMessage, [In] bool networkAvailable, [In] int attemptCount, [In] int maxAttemptCount);

		// Token: 0x06001E7F RID: 7807
		[DispId(746)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnAutoReconnected();

		// Token: 0x06001E80 RID: 7808
		[DispId(747)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnDialogDisplaying();

		// Token: 0x06001E81 RID: 7809
		[DispId(748)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnDialogDismissed();

		// Token: 0x06001E82 RID: 7810
		[DispId(749)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnNetworkBandwidthChanged([In] int qualityLevel, [MarshalAs(UnmanagedType.BStr)] [In] string bandwidth, [MarshalAs(UnmanagedType.BStr)] [In] string latency);

		// Token: 0x06001E83 RID: 7811
		[DispId(750)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnAdminMessageReceived([MarshalAs(UnmanagedType.BStr)] [In] string adminMessage);

		// Token: 0x06001E84 RID: 7812
		[DispId(751)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnKeyCombinationPressed([In] int keyCombination);

		// Token: 0x06001E85 RID: 7813
		[DispId(752)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnRemoteDesktopSizeChanged([In] int width, [In] int height);
	}
}
