using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000084 RID: 132
	// (Invoke) Token: 0x060024A2 RID: 9378
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	[ComVisible(false)]
	public delegate void IRemoteDesktopClientEvents_OnAutoReconnectingEventHandler([In] int disconnectReason, [In] int ExtendedDisconnectReason, [MarshalAs(UnmanagedType.BStr)] [In] string disconnectErrorMessage, [In] bool networkAvailable, [In] int attemptCount, [In] int maxAttemptCount);
}
