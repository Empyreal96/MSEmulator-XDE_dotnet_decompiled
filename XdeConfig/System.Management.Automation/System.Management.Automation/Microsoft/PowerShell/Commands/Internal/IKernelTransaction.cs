using System;
using System.Runtime.InteropServices;

namespace Microsoft.PowerShell.Commands.Internal
{
	// Token: 0x020007AD RID: 1965
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("79427A2B-F895-40e0-BE79-B57DC82ED231")]
	internal interface IKernelTransaction
	{
		// Token: 0x06004D3C RID: 19772
		int GetHandle(out IntPtr pHandle);
	}
}
