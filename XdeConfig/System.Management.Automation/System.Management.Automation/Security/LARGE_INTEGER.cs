using System;
using System.Runtime.InteropServices;

namespace System.Management.Automation.Security
{
	// Token: 0x020007F6 RID: 2038
	[StructLayout(LayoutKind.Explicit)]
	internal struct LARGE_INTEGER
	{
		// Token: 0x0400282A RID: 10282
		[FieldOffset(0)]
		public Anonymous_9320654f_2227_43bf_a385_74cc8c562686 Struct1;

		// Token: 0x0400282B RID: 10283
		[FieldOffset(0)]
		public Anonymous_947eb392_1446_4e25_bbd4_10e98165f3a9 u;

		// Token: 0x0400282C RID: 10284
		[FieldOffset(0)]
		public long QuadPart;
	}
}
