using System;
using System.Runtime.InteropServices;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020003DF RID: 991
	// (Invoke) Token: 0x06002D1D RID: 11549
	internal delegate void WSMPluginShellDelegate(IntPtr pluginContext, IntPtr requestDetails, int flags, [MarshalAs(UnmanagedType.LPWStr)] string extraInfo, IntPtr startupInfo, IntPtr inboundShellInformation);
}
