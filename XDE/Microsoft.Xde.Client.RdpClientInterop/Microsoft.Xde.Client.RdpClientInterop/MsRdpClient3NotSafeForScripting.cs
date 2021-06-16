using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000019 RID: 25
	[Guid("91B7CBC5-A72E-4FA0-9300-D647D7E897FF")]
	[CoClass(typeof(MsRdpClient3NotSafeForScriptingClass))]
	[ComImport]
	public interface MsRdpClient3NotSafeForScripting : IMsRdpClient3, IMsTscAxEvents_Event
	{
	}
}
