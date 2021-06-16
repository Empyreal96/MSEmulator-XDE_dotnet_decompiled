using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200000D RID: 13
	[CoClass(typeof(MsRdpClientNotSafeForScriptingClass))]
	[Guid("92B4A539-7115-4B7C-A5A9-E5D9EFC2780A")]
	[ComImport]
	public interface MsRdpClientNotSafeForScripting : IMsRdpClient, IMsTscAxEvents_Event
	{
	}
}
