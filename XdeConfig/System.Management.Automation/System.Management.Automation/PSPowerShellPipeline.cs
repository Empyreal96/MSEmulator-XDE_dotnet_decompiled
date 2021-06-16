using System;
using System.Runtime.InteropServices;

namespace System.Management.Automation
{
	// Token: 0x02000A48 RID: 2632
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct PSPowerShellPipeline
	{
		// Token: 0x04003289 RID: 12937
		internal bool IsNested;

		// Token: 0x0400328A RID: 12938
		internal bool NoInput;

		// Token: 0x0400328B RID: 12939
		internal bool AddToHistory;

		// Token: 0x0400328C RID: 12940
		internal uint ApartmentState;

		// Token: 0x0400328D RID: 12941
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string InstanceId;

		// Token: 0x0400328E RID: 12942
		internal IntPtr Commands;
	}
}
