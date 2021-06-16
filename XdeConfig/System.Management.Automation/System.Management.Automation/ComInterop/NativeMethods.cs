using System;
using System.Runtime.InteropServices;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A70 RID: 2672
	internal static class NativeMethods
	{
		// Token: 0x06006A74 RID: 27252
		[DllImport("oleaut32.dll", PreserveSig = false)]
		internal static extern void VariantClear(IntPtr variant);
	}
}
