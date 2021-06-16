using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200005C RID: 92
	// (Invoke) Token: 0x06001F17 RID: 7959
	[ComVisible(false)]
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	public delegate void IMsTscAxEvents_OnAutoReconnectingEventHandler([In] int disconnectReason, [In] int attemptCount, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.AutoReconnectContinueState")] out AutoReconnectContinueState pArcContinueStatus);
}
