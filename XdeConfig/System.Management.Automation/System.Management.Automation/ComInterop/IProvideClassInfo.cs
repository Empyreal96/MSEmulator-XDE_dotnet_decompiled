using System;
using System.Runtime.InteropServices;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A63 RID: 2659
	[Guid("B196B283-BAB4-101A-B69C-00AA00341D07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IProvideClassInfo
	{
		// Token: 0x060069F4 RID: 27124
		void GetClassInfo(out IntPtr info);
	}
}
