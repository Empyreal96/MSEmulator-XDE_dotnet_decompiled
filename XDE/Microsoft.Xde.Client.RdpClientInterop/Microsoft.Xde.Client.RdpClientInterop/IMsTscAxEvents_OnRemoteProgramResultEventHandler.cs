using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200005F RID: 95
	// (Invoke) Token: 0x06001F1D RID: 7965
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	[ComVisible(false)]
	public delegate void IMsTscAxEvents_OnRemoteProgramResultEventHandler([MarshalAs(UnmanagedType.BStr)] [In] string bstrRemoteProgram, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.RemoteProgramResult")] [In] RemoteProgramResult lError, [In] bool vbIsExecutable);
}
