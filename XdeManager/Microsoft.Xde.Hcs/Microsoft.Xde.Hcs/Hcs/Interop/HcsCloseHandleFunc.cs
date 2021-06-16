using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Hcs.Interop
{
	// Token: 0x02000015 RID: 21
	// (Invoke) Token: 0x060000A7 RID: 167
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
	public delegate bool HcsCloseHandleFunc(IntPtr handle);
}
