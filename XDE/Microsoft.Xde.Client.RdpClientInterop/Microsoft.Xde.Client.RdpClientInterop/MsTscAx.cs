using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200000B RID: 11
	[CoClass(typeof(MsTscAxClass))]
	[Guid("8C11EFAE-92C3-11D1-BC1E-00C04FA31489")]
	[ComImport]
	public interface MsTscAx : IMsTscAx, IMsTscAxEvents_Event
	{
	}
}
