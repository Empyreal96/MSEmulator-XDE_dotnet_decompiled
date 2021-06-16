using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000014 RID: 20
	[CoClass(typeof(MsRdpClient2Class))]
	[Guid("E7E17DC4-3B71-4BA7-A8E6-281FFADCA28F")]
	[ComImport]
	public interface MsRdpClient2 : IMsRdpClient2, IMsTscAxEvents_Event
	{
	}
}
