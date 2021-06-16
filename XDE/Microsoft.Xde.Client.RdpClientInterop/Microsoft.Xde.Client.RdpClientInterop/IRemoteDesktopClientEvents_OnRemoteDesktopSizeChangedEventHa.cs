using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200008B RID: 139
	// (Invoke) Token: 0x060024B0 RID: 9392
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	[ComVisible(false)]
	public delegate void IRemoteDesktopClientEvents_OnRemoteDesktopSizeChangedEventHandler([In] int width, [In] int height);
}
