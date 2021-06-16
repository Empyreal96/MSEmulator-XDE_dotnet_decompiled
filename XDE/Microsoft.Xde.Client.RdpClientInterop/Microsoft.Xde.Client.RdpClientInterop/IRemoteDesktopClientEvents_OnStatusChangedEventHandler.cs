using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000083 RID: 131
	// (Invoke) Token: 0x060024A0 RID: 9376
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	[ComVisible(false)]
	public delegate void IRemoteDesktopClientEvents_OnStatusChangedEventHandler([In] int statusCode, [MarshalAs(UnmanagedType.BStr)] [In] string statusMessage);
}
