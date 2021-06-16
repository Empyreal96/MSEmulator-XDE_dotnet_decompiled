using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000023 RID: 35
	[Guid("095E0738-D97D-488B-B9F6-DD0E8D66C0DE")]
	[CoClass(typeof(MsRdpClient4Class))]
	[ComImport]
	public interface MsRdpClient4 : IMsRdpClient4, IMsTscAxEvents_Event
	{
	}
}
