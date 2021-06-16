using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Hcs.Interop
{
	// Token: 0x0200000C RID: 12
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct HCS_PROCESS_INFORMATION
	{
		// Token: 0x0400002C RID: 44
		public uint ProcessId;

		// Token: 0x0400002D RID: 45
		public uint Reserved;

		// Token: 0x0400002E RID: 46
		public IntPtr StdInput;

		// Token: 0x0400002F RID: 47
		public IntPtr StdOutput;

		// Token: 0x04000030 RID: 48
		public IntPtr StdError;
	}
}
