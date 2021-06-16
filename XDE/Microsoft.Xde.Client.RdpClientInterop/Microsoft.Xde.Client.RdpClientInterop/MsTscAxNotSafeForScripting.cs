using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000006 RID: 6
	[CoClass(typeof(MsTscAxNotSafeForScriptingClass))]
	[Guid("8C11EFAE-92C3-11D1-BC1E-00C04FA31489")]
	[ComImport]
	public interface MsTscAxNotSafeForScripting : IMsTscAx, IMsTscAxEvents_Event
	{
	}
}
