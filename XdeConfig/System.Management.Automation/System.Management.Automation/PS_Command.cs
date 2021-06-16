using System;
using System.Runtime.InteropServices;

namespace System.Management.Automation
{
	// Token: 0x02000A49 RID: 2633
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct PS_Command
	{
		// Token: 0x0400328F RID: 12943
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string CommandText;

		// Token: 0x04003290 RID: 12944
		internal bool IsScript;

		// Token: 0x04003291 RID: 12945
		internal IntPtr Parameters;
	}
}
