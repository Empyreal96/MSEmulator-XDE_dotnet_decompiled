using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000023 RID: 35
	public static class StringUtilities
	{
		// Token: 0x06000185 RID: 389 RVA: 0x00003E5E File Offset: 0x0000205E
		public static int CompareNoCase(string string1, string string2)
		{
			return StringComparer.InvariantCultureIgnoreCase.Compare(string1, string2);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00003E6C File Offset: 0x0000206C
		public static bool EqualsNoCase(string string1, string string2)
		{
			return StringComparer.InvariantCultureIgnoreCase.Equals(string1, string2);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00003E7A File Offset: 0x0000207A
		public static string CurrentCultureFormat(string format, params object[] args)
		{
			return string.Format(CultureInfo.CurrentCulture, format, args);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00003E88 File Offset: 0x00002088
		public static string InvariantCultureFormat(string format, params object[] args)
		{
			return string.Format(CultureInfo.InvariantCulture, format, args);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00003E98 File Offset: 0x00002098
		public static string ConvertToBase36(ulong number)
		{
			char[] array = "0123456789abcdefghijklmnopqrstuvwxyz".ToCharArray();
			Stack<char> stack = new Stack<char>();
			while (number != 0UL)
			{
				stack.Push(array[(int)(checked((IntPtr)(number % 36UL)))]);
				number /= 36UL;
			}
			return new string(stack.ToArray());
		}
	}
}
