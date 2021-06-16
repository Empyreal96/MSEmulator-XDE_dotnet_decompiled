using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000039 RID: 57
	[CoClass(typeof(MsRdpClient7Class))]
	[Guid("B2A5B5CE-3461-444A-91D4-ADD26D070638")]
	[ComImport]
	public interface MsRdpClient7 : IMsRdpClient7, IMsTscAxEvents_Event
	{
	}
}
