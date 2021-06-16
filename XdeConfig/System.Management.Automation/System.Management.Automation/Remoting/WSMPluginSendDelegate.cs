using System;
using System.Runtime.InteropServices;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020003E4 RID: 996
	// (Invoke) Token: 0x06002D31 RID: 11569
	internal delegate void WSMPluginSendDelegate(IntPtr pluginContext, IntPtr requestDetails, int flags, IntPtr shellContext, IntPtr commandContext, [MarshalAs(UnmanagedType.LPWStr)] string stream, IntPtr inboundData);
}
