using System;
using System.Runtime.InteropServices;

namespace System.Management.Automation
{
	// Token: 0x02000A4A RID: 2634
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct PS_Parameter
	{
		// Token: 0x04003292 RID: 12946
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string Name;

		// Token: 0x04003293 RID: 12947
		internal IntPtr Value;
	}
}
