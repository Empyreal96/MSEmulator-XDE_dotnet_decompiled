using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000066 RID: 102
	// (Invoke) Token: 0x06001F2B RID: 7979
	[ComVisible(false)]
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	public delegate void IMsTscAxEvents_OnServiceMessageReceivedEventHandler([MarshalAs(UnmanagedType.BStr)] [In] string serviceMessage);
}
