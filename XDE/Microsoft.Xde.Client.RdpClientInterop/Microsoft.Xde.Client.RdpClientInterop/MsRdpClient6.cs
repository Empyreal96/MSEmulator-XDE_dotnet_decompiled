using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000031 RID: 49
	[Guid("D43B7D80-8517-4B6D-9EAC-96AD6800D7F2")]
	[CoClass(typeof(MsRdpClient6Class))]
	[ComImport]
	public interface MsRdpClient6 : IMsRdpClient6, IMsTscAxEvents_Event
	{
	}
}
