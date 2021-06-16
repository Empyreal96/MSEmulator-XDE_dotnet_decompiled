using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200001B RID: 27
	[Guid("91B7CBC5-A72E-4FA0-9300-D647D7E897FF")]
	[CoClass(typeof(MsRdpClient3Class))]
	[ComImport]
	public interface MsRdpClient3 : IMsRdpClient3, IMsTscAxEvents_Event
	{
	}
}
