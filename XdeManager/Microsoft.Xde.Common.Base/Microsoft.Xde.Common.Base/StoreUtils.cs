using System;
using System.Runtime.InteropServices;
using Windows.Services.Store;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000021 RID: 33
	public static class StoreUtils
	{
		// Token: 0x06000174 RID: 372 RVA: 0x00003D08 File Offset: 0x00001F08
		public static StoreContext GetDefaultContext(IntPtr mainWindowHandle)
		{
			StoreContext @default = StoreContext.GetDefault();
			((StoreUtils.IInitializeWithWindow)@default).Initialize(mainWindowHandle);
			return @default;
		}

		// Token: 0x02000073 RID: 115
		[Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		private interface IInitializeWithWindow
		{
			// Token: 0x06000215 RID: 533
			void Initialize(IntPtr hwnd);
		}
	}
}
