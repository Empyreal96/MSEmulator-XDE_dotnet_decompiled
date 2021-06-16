using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Hcs.Interop
{
	// Token: 0x0200000F RID: 15
	// (Invoke) Token: 0x0600007A RID: 122
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
	public delegate void RegisterHcsNotificationCallback<THandle>(THandle handle, NotificationCallback callback, IntPtr context, out IntPtr callbackHandle);
}
