using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200002E RID: 46
	[Guid("D43B7D80-8517-4B6D-9EAC-96AD6800D7F2")]
	[CoClass(typeof(MsRdpClient6NotSafeForScriptingClass))]
	[ComImport]
	public interface MsRdpClient6NotSafeForScripting : IMsRdpClient6, IMsTscAxEvents_Event
	{
	}
}
