using System;
using System.Runtime.InteropServices;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020003E2 RID: 994
	// (Invoke) Token: 0x06002D29 RID: 11561
	internal delegate void WSMPluginCommandDelegate(IntPtr pluginContext, IntPtr requestDetails, int flags, IntPtr shellContext, [MarshalAs(UnmanagedType.LPWStr)] string commandLine, IntPtr arguments);
}
