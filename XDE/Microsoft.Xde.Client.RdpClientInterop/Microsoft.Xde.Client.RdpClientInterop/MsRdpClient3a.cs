using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200001D RID: 29
	[CoClass(typeof(MsRdpClient3aClass))]
	[Guid("91B7CBC5-A72E-4FA0-9300-D647D7E897FF")]
	[ComImport]
	public interface MsRdpClient3a : IMsRdpClient3, IMsTscAxEvents_Event
	{
	}
}
