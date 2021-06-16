using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200003C RID: 60
	[CoClass(typeof(MsRdpClient8NotSafeForScriptingClass))]
	[Guid("4247E044-9271-43A9-BC49-E2AD9E855D62")]
	[ComImport]
	public interface MsRdpClient8NotSafeForScripting : IMsRdpClient8, IMsTscAxEvents_Event
	{
	}
}
