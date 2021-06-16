using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000016 RID: 22
	[Guid("E7E17DC4-3B71-4BA7-A8E6-281FFADCA28F")]
	[CoClass(typeof(MsRdpClient2aClass))]
	[ComImport]
	public interface MsRdpClient2a : IMsRdpClient2, IMsTscAxEvents_Event
	{
	}
}
