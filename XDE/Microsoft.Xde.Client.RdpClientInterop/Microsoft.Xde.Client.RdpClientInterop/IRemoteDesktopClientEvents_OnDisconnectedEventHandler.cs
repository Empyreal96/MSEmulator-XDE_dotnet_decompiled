using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000082 RID: 130
	// (Invoke) Token: 0x0600249E RID: 9374
	[ComVisible(false)]
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	public delegate void IRemoteDesktopClientEvents_OnDisconnectedEventHandler([In] int disconnectReason, [In] int ExtendedDisconnectReason, [MarshalAs(UnmanagedType.BStr)] [In] string disconnectErrorMessage);
}
