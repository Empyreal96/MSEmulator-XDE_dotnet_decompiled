using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200000F RID: 15
	[Guid("92B4A539-7115-4B7C-A5A9-E5D9EFC2780A")]
	[CoClass(typeof(MsRdpClientClass))]
	[ComImport]
	public interface MsRdpClient : IMsRdpClient, IMsTscAxEvents_Event
	{
	}
}
