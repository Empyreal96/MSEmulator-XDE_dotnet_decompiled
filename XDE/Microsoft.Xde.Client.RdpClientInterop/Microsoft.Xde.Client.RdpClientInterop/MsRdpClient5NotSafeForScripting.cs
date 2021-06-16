using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000028 RID: 40
	[CoClass(typeof(MsRdpClient5NotSafeForScriptingClass))]
	[Guid("4EB5335B-6429-477D-B922-E06A28ECD8BF")]
	[ComImport]
	public interface MsRdpClient5NotSafeForScripting : IMsRdpClient5, IMsTscAxEvents_Event
	{
	}
}
