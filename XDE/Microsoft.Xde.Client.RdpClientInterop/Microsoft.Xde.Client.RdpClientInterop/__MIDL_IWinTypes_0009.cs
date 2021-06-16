using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200008E RID: 142
	[StructLayout(LayoutKind.Explicit, Pack = 4, Size = 4)]
	public struct __MIDL_IWinTypes_0009
	{
		// Token: 0x04000045 RID: 69
		[FieldOffset(0)]
		public int hInproc;

		// Token: 0x04000046 RID: 70
		[FieldOffset(0)]
		public int hRemote;
	}
}
