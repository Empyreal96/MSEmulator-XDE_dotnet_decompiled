using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200003E RID: 62
	[CoClass(typeof(MsRdpClient8Class))]
	[Guid("4247E044-9271-43A9-BC49-E2AD9E855D62")]
	[ComImport]
	public interface MsRdpClient8 : IMsRdpClient8, IMsTscAxEvents_Event
	{
	}
}
