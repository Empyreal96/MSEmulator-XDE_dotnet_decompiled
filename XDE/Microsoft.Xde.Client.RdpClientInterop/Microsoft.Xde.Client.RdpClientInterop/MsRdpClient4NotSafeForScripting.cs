using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000020 RID: 32
	[CoClass(typeof(MsRdpClient4NotSafeForScriptingClass))]
	[Guid("095E0738-D97D-488B-B9F6-DD0E8D66C0DE")]
	[ComImport]
	public interface MsRdpClient4NotSafeForScripting : IMsRdpClient4, IMsTscAxEvents_Event
	{
	}
}
