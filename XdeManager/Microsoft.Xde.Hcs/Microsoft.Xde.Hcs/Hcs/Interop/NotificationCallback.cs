using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Hcs.Interop
{
	// Token: 0x0200000E RID: 14
	// (Invoke) Token: 0x06000076 RID: 118
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
	public delegate void NotificationCallback(uint notificationType, IntPtr context, int notificationStatus, [MarshalAs(UnmanagedType.LPWStr)] string notificationData);
}
