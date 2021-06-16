using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000064 RID: 100
	// (Invoke) Token: 0x06001F27 RID: 7975
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	[ComVisible(false)]
	public delegate void IMsTscAxEvents_OnUserNameAcquiredEventHandler([MarshalAs(UnmanagedType.BStr)] [In] string bstrUserName);
}
