using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A61 RID: 2657
	[Guid("00020400-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IDispatch
	{
		// Token: 0x060069F0 RID: 27120
		[PreserveSig]
		int TryGetTypeInfoCount(out uint pctinfo);

		// Token: 0x060069F1 RID: 27121
		[PreserveSig]
		int TryGetTypeInfo(uint iTInfo, int lcid, out IntPtr info);

		// Token: 0x060069F2 RID: 27122
		[PreserveSig]
		int TryGetIDsOfNames(ref Guid iid, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 2)] string[] names, uint cNames, int lcid, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 2)] [Out] int[] rgDispId);

		// Token: 0x060069F3 RID: 27123
		[PreserveSig]
		int TryInvoke(int dispIdMember, ref Guid riid, int lcid, System.Runtime.InteropServices.ComTypes.INVOKEKIND wFlags, ref System.Runtime.InteropServices.ComTypes.DISPPARAMS pDispParams, out object VarResult, out System.Runtime.InteropServices.ComTypes.EXCEPINFO pExcepInfo, out uint puArgErr);
	}
}
