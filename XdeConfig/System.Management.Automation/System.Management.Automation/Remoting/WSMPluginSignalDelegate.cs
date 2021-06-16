using System;
using System.Runtime.InteropServices;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020003E6 RID: 998
	// (Invoke) Token: 0x06002D39 RID: 11577
	internal delegate void WSMPluginSignalDelegate(IntPtr pluginContext, IntPtr requestDetails, int flags, IntPtr shellContext, IntPtr commandContext, [MarshalAs(UnmanagedType.LPWStr)] string code);
}
