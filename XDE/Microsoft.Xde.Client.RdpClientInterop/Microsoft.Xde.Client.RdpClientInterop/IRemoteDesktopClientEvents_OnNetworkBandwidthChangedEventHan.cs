using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000088 RID: 136
	// (Invoke) Token: 0x060024AA RID: 9386
	[ComVisible(false)]
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	public delegate void IRemoteDesktopClientEvents_OnNetworkBandwidthChangedEventHandler([In] int qualityLevel, [MarshalAs(UnmanagedType.BStr)] [In] string bandwidth, [MarshalAs(UnmanagedType.BStr)] [In] string latency);
}
