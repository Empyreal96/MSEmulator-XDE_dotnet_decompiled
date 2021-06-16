using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000052 RID: 82
	// (Invoke) Token: 0x06001F03 RID: 7939
	[ComVisible(false)]
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	public delegate void IMsTscAxEvents_OnChannelReceivedDataEventHandler([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string data);
}
