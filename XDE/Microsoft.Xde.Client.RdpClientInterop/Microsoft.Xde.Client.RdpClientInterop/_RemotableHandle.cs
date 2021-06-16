using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200004A RID: 74
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct _RemotableHandle
	{
		// Token: 0x0400000E RID: 14
		public int fContext;

		// Token: 0x0400000F RID: 15
		public __MIDL_IWinTypes_0009 u;
	}
}
