using System;

namespace MS.WindowsAPICodePack.Internal
{
	// Token: 0x02000004 RID: 4
	internal static class CoreErrorHelper
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		public static int HResultFromWin32(int win32ErrorCode)
		{
			if (win32ErrorCode > 0)
			{
				win32ErrorCode = ((win32ErrorCode & 65535) | 458752 | int.MinValue);
			}
			return win32ErrorCode;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002064 File Offset: 0x00000264
		public static bool Succeeded(int result)
		{
			return result >= 0;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000206D File Offset: 0x0000026D
		public static bool Succeeded(HResult result)
		{
			return CoreErrorHelper.Succeeded((int)result);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002075 File Offset: 0x00000275
		public static bool Failed(HResult result)
		{
			return !CoreErrorHelper.Succeeded(result);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002080 File Offset: 0x00000280
		public static bool Failed(int result)
		{
			return !CoreErrorHelper.Succeeded(result);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000208B File Offset: 0x0000028B
		public static bool Matches(int result, int win32ErrorCode)
		{
			return result == CoreErrorHelper.HResultFromWin32(win32ErrorCode);
		}

		// Token: 0x040000DF RID: 223
		private const int FacilityWin32 = 7;

		// Token: 0x040000E0 RID: 224
		public const int Ignored = 0;
	}
}
