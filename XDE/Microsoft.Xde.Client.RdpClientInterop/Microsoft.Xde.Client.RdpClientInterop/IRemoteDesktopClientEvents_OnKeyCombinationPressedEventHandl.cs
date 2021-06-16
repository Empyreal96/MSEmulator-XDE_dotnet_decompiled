using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200008A RID: 138
	// (Invoke) Token: 0x060024AE RID: 9390
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	[ComVisible(false)]
	public delegate void IRemoteDesktopClientEvents_OnKeyCombinationPressedEventHandler([In] int keyCombination);
}
