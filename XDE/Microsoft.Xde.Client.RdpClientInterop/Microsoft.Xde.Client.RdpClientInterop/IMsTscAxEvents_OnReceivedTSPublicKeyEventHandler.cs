using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200005B RID: 91
	// (Invoke) Token: 0x06001F15 RID: 7957
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	[ComVisible(false)]
	public delegate void IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler([MarshalAs(UnmanagedType.BStr)] [In] string publicKey, out bool pfContinueLogon);
}
