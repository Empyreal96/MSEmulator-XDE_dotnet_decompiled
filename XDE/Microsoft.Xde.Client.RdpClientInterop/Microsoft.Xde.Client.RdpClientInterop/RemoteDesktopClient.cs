using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000043 RID: 67
	[Guid("57D25668-625A-4905-BE4E-304CAA13F89C")]
	[CoClass(typeof(RemoteDesktopClientClass))]
	[ComImport]
	public interface RemoteDesktopClient : IRemoteDesktopClient, IRemoteDesktopClientEvents_Event
	{
	}
}
