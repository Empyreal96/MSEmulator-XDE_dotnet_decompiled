using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Management.Automation
{
	// Token: 0x02000178 RID: 376
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00020400-0000-0000-c000-000000000046")]
	[ComImport]
	internal interface IDispatch
	{
		// Token: 0x060012C6 RID: 4806
		[PreserveSig]
		int GetTypeInfoCount(out int info);

		// Token: 0x060012C7 RID: 4807
		[PreserveSig]
		int GetTypeInfo(int iTInfo, int lcid, out ITypeInfo ppTInfo);

		// Token: 0x060012C8 RID: 4808
		void GetIDsOfNames([MarshalAs(UnmanagedType.LPStruct)] Guid iid, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] string[] rgszNames, int cNames, int lcid, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] [Out] int[] rgDispId);

		// Token: 0x060012C9 RID: 4809
		void Invoke(int dispIdMember, [MarshalAs(UnmanagedType.LPStruct)] Guid iid, int lcid, System.Runtime.InteropServices.ComTypes.INVOKEKIND wFlags, [MarshalAs(UnmanagedType.LPArray)] [In] [Out] System.Runtime.InteropServices.ComTypes.DISPPARAMS[] paramArray, out object pVarResult, out ComInvoker.EXCEPINFO pExcepInfo, out uint puArgErr);
	}
}
