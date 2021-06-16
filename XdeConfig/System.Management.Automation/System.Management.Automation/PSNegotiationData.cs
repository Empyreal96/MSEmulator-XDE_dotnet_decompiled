using System;
using System.Runtime.InteropServices;

namespace System.Management.Automation
{
	// Token: 0x02000A4B RID: 2635
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct PSNegotiationData
	{
		// Token: 0x04003294 RID: 12948
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string PSVersion;
	}
}
