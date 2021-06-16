using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000012 RID: 18
	[CoClass(typeof(MsRdpClient2NotSafeForScriptingClass))]
	[Guid("E7E17DC4-3B71-4BA7-A8E6-281FFADCA28F")]
	[ComImport]
	public interface MsRdpClient2NotSafeForScripting : IMsRdpClient2, IMsTscAxEvents_Event
	{
	}
}
