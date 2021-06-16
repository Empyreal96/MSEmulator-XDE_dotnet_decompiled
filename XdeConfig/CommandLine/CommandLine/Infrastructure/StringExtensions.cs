using System;
using System.Globalization;
using System.Text;

namespace CommandLine.Infrastructure
{
	// Token: 0x02000066 RID: 102
	internal static class StringExtensions
	{
		// Token: 0x06000295 RID: 661 RVA: 0x0000A6FB File Offset: 0x000088FB
		public static string ToOneCharString(this char c)
		{
			return new string(c, 1);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000A704 File Offset: 0x00008904
		public static string ToStringInvariant<T>(this T value)
		{
			return Convert.ToString(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000A716 File Offset: 0x00008916
		public static string ToStringLocal<T>(this T value)
		{
			return Convert.ToString(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000A728 File Offset: 0x00008928
		public static string FormatInvariant(this string value, params object[] arguments)
		{
			return string.Format(CultureInfo.InvariantCulture, value, arguments);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000A736 File Offset: 0x00008936
		public static string FormatLocal(this string value, params object[] arguments)
		{
			return string.Format(CultureInfo.CurrentCulture, value, arguments);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000A744 File Offset: 0x00008944
		public static string Spaces(this int value)
		{
			return new string(' ', value);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000A74E File Offset: 0x0000894E
		public static bool EqualsOrdinal(this string strA, string strB)
		{
			return string.CompareOrdinal(strA, strB) == 0;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000A75A File Offset: 0x0000895A
		public static bool EqualsOrdinalIgnoreCase(this string strA, string strB)
		{
			return string.Compare(strA, strB, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000A767 File Offset: 0x00008967
		public static int SafeLength(this string value)
		{
			if (value != null)
			{
				return value.Length;
			}
			return 0;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000A774 File Offset: 0x00008974
		public static string JoinTo(this string value, params string[] others)
		{
			StringBuilder stringBuilder = new StringBuilder(value);
			foreach (string value2 in others)
			{
				stringBuilder.Append(value2);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000A7AA File Offset: 0x000089AA
		public static bool IsBooleanString(this string value)
		{
			return value.Equals("true", StringComparison.OrdinalIgnoreCase) || value.Equals("false", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000A7C8 File Offset: 0x000089C8
		public static bool ToBoolean(this string value)
		{
			return value.Equals("true", StringComparison.OrdinalIgnoreCase);
		}
	}
}
