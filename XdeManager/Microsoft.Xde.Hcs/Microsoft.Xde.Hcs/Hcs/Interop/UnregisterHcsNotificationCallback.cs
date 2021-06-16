using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Hcs.Interop
{
	// Token: 0x02000010 RID: 16
	// (Invoke) Token: 0x0600007E RID: 126
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
	public delegate void UnregisterHcsNotificationCallback(IntPtr handle);
}
