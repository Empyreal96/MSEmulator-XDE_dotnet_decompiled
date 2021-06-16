using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000061 RID: 97
	// (Invoke) Token: 0x06001F21 RID: 7969
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	[ComVisible(false)]
	public delegate void IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler([In] bool vbDisplayed, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")] [In] ref _RemotableHandle hwnd, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.RemoteWindowDisplayedAttribute")] [In] RemoteWindowDisplayedAttribute windowAttribute);
}
