using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200002B RID: 43
	[CoClass(typeof(MsRdpClient5Class))]
	[Guid("4EB5335B-6429-477D-B922-E06A28ECD8BF")]
	[ComImport]
	public interface MsRdpClient5 : IMsRdpClient5, IMsTscAxEvents_Event
	{
	}
}
