using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000057 RID: 87
	// (Invoke) Token: 0x06001F0D RID: 7949
	[ComVisible(false)]
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	public delegate void IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler([In] int width, [In] int height);
}
