using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000089 RID: 137
	// (Invoke) Token: 0x060024AC RID: 9388
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	[ComVisible(false)]
	public delegate void IRemoteDesktopClientEvents_OnAdminMessageReceivedEventHandler([MarshalAs(UnmanagedType.BStr)] [In] string adminMessage);
}
