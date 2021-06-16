using System;
using System.Runtime.InteropServices;

namespace System.Management.Automation.Security
{
	// Token: 0x020007F5 RID: 2037
	internal struct SAFER_CODE_PROPERTIES
	{
		// Token: 0x0400281E RID: 10270
		public uint cbSize;

		// Token: 0x0400281F RID: 10271
		public uint dwCheckFlags;

		// Token: 0x04002820 RID: 10272
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ImagePath;

		// Token: 0x04002821 RID: 10273
		public IntPtr hImageFileHandle;

		// Token: 0x04002822 RID: 10274
		public uint UrlZoneId;

		// Token: 0x04002823 RID: 10275
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] ImageHash;

		// Token: 0x04002824 RID: 10276
		public uint dwImageHashSize;

		// Token: 0x04002825 RID: 10277
		public LARGE_INTEGER ImageSize;

		// Token: 0x04002826 RID: 10278
		public uint HashAlgorithm;

		// Token: 0x04002827 RID: 10279
		public IntPtr pByteBlock;

		// Token: 0x04002828 RID: 10280
		public IntPtr hWndParent;

		// Token: 0x04002829 RID: 10281
		public uint dwWVTUIChoice;
	}
}
