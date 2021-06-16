using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	// Token: 0x0200000E RID: 14
	internal static class PropVariantNativeMethods
	{
		// Token: 0x060000A3 RID: 163
		[DllImport("Ole32.dll", PreserveSig = false)]
		internal static extern void PropVariantClear([In] [Out] PropVariant pvar);

		// Token: 0x060000A4 RID: 164
		[DllImport("OleAut32.dll")]
		internal static extern IntPtr SafeArrayCreateVector(ushort vt, int lowerBound, uint cElems);

		// Token: 0x060000A5 RID: 165
		[DllImport("OleAut32.dll", PreserveSig = false)]
		internal static extern IntPtr SafeArrayAccessData(IntPtr psa);

		// Token: 0x060000A6 RID: 166
		[DllImport("OleAut32.dll", PreserveSig = false)]
		internal static extern void SafeArrayUnaccessData(IntPtr psa);

		// Token: 0x060000A7 RID: 167
		[DllImport("OleAut32.dll")]
		internal static extern uint SafeArrayGetDim(IntPtr psa);

		// Token: 0x060000A8 RID: 168
		[DllImport("OleAut32.dll", PreserveSig = false)]
		internal static extern int SafeArrayGetLBound(IntPtr psa, uint nDim);

		// Token: 0x060000A9 RID: 169
		[DllImport("OleAut32.dll", PreserveSig = false)]
		internal static extern int SafeArrayGetUBound(IntPtr psa, uint nDim);

		// Token: 0x060000AA RID: 170
		[DllImport("OleAut32.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		internal static extern object SafeArrayGetElement(IntPtr psa, ref int rgIndices);

		// Token: 0x060000AB RID: 171
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, PreserveSig = false, SetLastError = true)]
		internal static extern void InitPropVariantFromPropVariantVectorElem([In] PropVariant propvarIn, uint iElem, [Out] PropVariant ppropvar);

		// Token: 0x060000AC RID: 172
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, PreserveSig = false, SetLastError = true)]
		internal static extern void InitPropVariantFromFileTime([In] ref System.Runtime.InteropServices.ComTypes.FILETIME pftIn, [Out] PropVariant ppropvar);

		// Token: 0x060000AD RID: 173
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.I4)]
		internal static extern int PropVariantGetElementCount([In] PropVariant propVar);

		// Token: 0x060000AE RID: 174
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, PreserveSig = false, SetLastError = true)]
		internal static extern void PropVariantGetBooleanElem([In] PropVariant propVar, [In] uint iElem, [MarshalAs(UnmanagedType.Bool)] out bool pfVal);

		// Token: 0x060000AF RID: 175
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, PreserveSig = false, SetLastError = true)]
		internal static extern void PropVariantGetInt16Elem([In] PropVariant propVar, [In] uint iElem, out short pnVal);

		// Token: 0x060000B0 RID: 176
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, PreserveSig = false, SetLastError = true)]
		internal static extern void PropVariantGetUInt16Elem([In] PropVariant propVar, [In] uint iElem, out ushort pnVal);

		// Token: 0x060000B1 RID: 177
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, PreserveSig = false, SetLastError = true)]
		internal static extern void PropVariantGetInt32Elem([In] PropVariant propVar, [In] uint iElem, out int pnVal);

		// Token: 0x060000B2 RID: 178
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, PreserveSig = false, SetLastError = true)]
		internal static extern void PropVariantGetUInt32Elem([In] PropVariant propVar, [In] uint iElem, out uint pnVal);

		// Token: 0x060000B3 RID: 179
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, PreserveSig = false, SetLastError = true)]
		internal static extern void PropVariantGetInt64Elem([In] PropVariant propVar, [In] uint iElem, out long pnVal);

		// Token: 0x060000B4 RID: 180
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, PreserveSig = false, SetLastError = true)]
		internal static extern void PropVariantGetUInt64Elem([In] PropVariant propVar, [In] uint iElem, out ulong pnVal);

		// Token: 0x060000B5 RID: 181
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, PreserveSig = false, SetLastError = true)]
		internal static extern void PropVariantGetDoubleElem([In] PropVariant propVar, [In] uint iElem, out double pnVal);

		// Token: 0x060000B6 RID: 182
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, PreserveSig = false, SetLastError = true)]
		internal static extern void PropVariantGetFileTimeElem([In] PropVariant propVar, [In] uint iElem, [MarshalAs(UnmanagedType.Struct)] out System.Runtime.InteropServices.ComTypes.FILETIME pftVal);

		// Token: 0x060000B7 RID: 183
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, PreserveSig = false, SetLastError = true)]
		internal static extern void PropVariantGetStringElem([In] PropVariant propVar, [In] uint iElem, [MarshalAs(UnmanagedType.LPWStr)] ref string ppszVal);

		// Token: 0x060000B8 RID: 184
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, PreserveSig = false, SetLastError = true)]
		internal static extern void InitPropVariantFromBooleanVector([MarshalAs(UnmanagedType.LPArray)] [In] bool[] prgf, uint cElems, [Out] PropVariant ppropvar);

		// Token: 0x060000B9 RID: 185
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, PreserveSig = false, SetLastError = true)]
		internal static extern void InitPropVariantFromInt16Vector([In] [Out] short[] prgn, uint cElems, [Out] PropVariant ppropvar);

		// Token: 0x060000BA RID: 186
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, PreserveSig = false, SetLastError = true)]
		internal static extern void InitPropVariantFromUInt16Vector([In] [Out] ushort[] prgn, uint cElems, [Out] PropVariant ppropvar);

		// Token: 0x060000BB RID: 187
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, PreserveSig = false, SetLastError = true)]
		internal static extern void InitPropVariantFromInt32Vector([In] [Out] int[] prgn, uint cElems, [Out] PropVariant propVar);

		// Token: 0x060000BC RID: 188
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, PreserveSig = false, SetLastError = true)]
		internal static extern void InitPropVariantFromUInt32Vector([In] [Out] uint[] prgn, uint cElems, [Out] PropVariant ppropvar);

		// Token: 0x060000BD RID: 189
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, PreserveSig = false, SetLastError = true)]
		internal static extern void InitPropVariantFromInt64Vector([In] [Out] long[] prgn, uint cElems, [Out] PropVariant ppropvar);

		// Token: 0x060000BE RID: 190
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, PreserveSig = false, SetLastError = true)]
		internal static extern void InitPropVariantFromUInt64Vector([In] [Out] ulong[] prgn, uint cElems, [Out] PropVariant ppropvar);

		// Token: 0x060000BF RID: 191
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, PreserveSig = false, SetLastError = true)]
		internal static extern void InitPropVariantFromDoubleVector([In] [Out] double[] prgn, uint cElems, [Out] PropVariant propvar);

		// Token: 0x060000C0 RID: 192
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, PreserveSig = false, SetLastError = true)]
		internal static extern void InitPropVariantFromFileTimeVector([In] [Out] System.Runtime.InteropServices.ComTypes.FILETIME[] prgft, uint cElems, [Out] PropVariant ppropvar);

		// Token: 0x060000C1 RID: 193
		[DllImport("propsys.dll", CharSet = CharSet.Unicode, PreserveSig = false, SetLastError = true)]
		internal static extern void InitPropVariantFromStringVector([In] [Out] string[] prgsz, uint cElems, [Out] PropVariant ppropvar);
	}
}
