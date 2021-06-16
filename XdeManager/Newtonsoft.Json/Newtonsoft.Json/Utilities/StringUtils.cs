using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000065 RID: 101
	internal static class StringUtils
	{
		// Token: 0x060005CD RID: 1485 RVA: 0x0001940F File Offset: 0x0001760F
		public static string FormatWith(this string format, IFormatProvider provider, object arg0)
		{
			return format.FormatWith(provider, new object[]
			{
				arg0
			});
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00019422 File Offset: 0x00017622
		public static string FormatWith(this string format, IFormatProvider provider, object arg0, object arg1)
		{
			return format.FormatWith(provider, new object[]
			{
				arg0,
				arg1
			});
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00019439 File Offset: 0x00017639
		public static string FormatWith(this string format, IFormatProvider provider, object arg0, object arg1, object arg2)
		{
			return format.FormatWith(provider, new object[]
			{
				arg0,
				arg1,
				arg2
			});
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00019455 File Offset: 0x00017655
		public static string FormatWith(this string format, IFormatProvider provider, object arg0, object arg1, object arg2, object arg3)
		{
			return format.FormatWith(provider, new object[]
			{
				arg0,
				arg1,
				arg2,
				arg3
			});
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00019476 File Offset: 0x00017676
		private static string FormatWith(this string format, IFormatProvider provider, params object[] args)
		{
			ValidationUtils.ArgumentNotNull(format, "format");
			return string.Format(provider, format, args);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0001948C File Offset: 0x0001768C
		public static bool IsWhiteSpace(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (s.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < s.Length; i++)
			{
				if (!char.IsWhiteSpace(s[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x000194D3 File Offset: 0x000176D3
		public static StringWriter CreateStringWriter(int capacity)
		{
			return new StringWriter(new StringBuilder(capacity), CultureInfo.InvariantCulture);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x000194E8 File Offset: 0x000176E8
		public static void ToCharAsUnicode(char c, char[] buffer)
		{
			buffer[0] = '\\';
			buffer[1] = 'u';
			buffer[2] = MathUtils.IntToHex((int)(c >> 12 & '\u000f'));
			buffer[3] = MathUtils.IntToHex((int)(c >> 8 & '\u000f'));
			buffer[4] = MathUtils.IntToHex((int)(c >> 4 & '\u000f'));
			buffer[5] = MathUtils.IntToHex((int)(c & '\u000f'));
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00019538 File Offset: 0x00017738
		public static TSource ForgivingCaseSensitiveFind<TSource>(this IEnumerable<TSource> source, Func<TSource, string> valueSelector, string testValue)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (valueSelector == null)
			{
				throw new ArgumentNullException("valueSelector");
			}
			IEnumerable<TSource> source2 = from s in source
			where string.Equals(valueSelector(s), testValue, StringComparison.OrdinalIgnoreCase)
			select s;
			if (source2.Count<TSource>() <= 1)
			{
				return source2.SingleOrDefault<TSource>();
			}
			return (from s in source
			where string.Equals(valueSelector(s), testValue, StringComparison.Ordinal)
			select s).SingleOrDefault<TSource>();
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x000195B4 File Offset: 0x000177B4
		public static string ToCamelCase(string s)
		{
			if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
			{
				return s;
			}
			char[] array = s.ToCharArray();
			int num = 0;
			while (num < array.Length && (num != 1 || char.IsUpper(array[num])))
			{
				bool flag = num + 1 < array.Length;
				if (num > 0 && flag && !char.IsUpper(array[num + 1]))
				{
					if (char.IsSeparator(array[num + 1]))
					{
						array[num] = StringUtils.ToLower(array[num]);
						break;
					}
					break;
				}
				else
				{
					array[num] = StringUtils.ToLower(array[num]);
					num++;
				}
			}
			return new string(array);
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00019643 File Offset: 0x00017843
		private static char ToLower(char c)
		{
			c = char.ToLower(c, CultureInfo.InvariantCulture);
			return c;
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00019654 File Offset: 0x00017854
		public static string ToSnakeCase(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return s;
			}
			StringBuilder stringBuilder = new StringBuilder();
			StringUtils.SnakeCaseState snakeCaseState = StringUtils.SnakeCaseState.Start;
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == ' ')
				{
					if (snakeCaseState != StringUtils.SnakeCaseState.Start)
					{
						snakeCaseState = StringUtils.SnakeCaseState.NewWord;
					}
				}
				else if (char.IsUpper(s[i]))
				{
					switch (snakeCaseState)
					{
					case StringUtils.SnakeCaseState.Lower:
					case StringUtils.SnakeCaseState.NewWord:
						stringBuilder.Append('_');
						break;
					case StringUtils.SnakeCaseState.Upper:
					{
						bool flag = i + 1 < s.Length;
						if (i > 0 && flag)
						{
							char c = s[i + 1];
							if (!char.IsUpper(c) && c != '_')
							{
								stringBuilder.Append('_');
							}
						}
						break;
					}
					}
					char value = char.ToLower(s[i], CultureInfo.InvariantCulture);
					stringBuilder.Append(value);
					snakeCaseState = StringUtils.SnakeCaseState.Upper;
				}
				else if (s[i] == '_')
				{
					stringBuilder.Append('_');
					snakeCaseState = StringUtils.SnakeCaseState.Start;
				}
				else
				{
					if (snakeCaseState == StringUtils.SnakeCaseState.NewWord)
					{
						stringBuilder.Append('_');
					}
					stringBuilder.Append(s[i]);
					snakeCaseState = StringUtils.SnakeCaseState.Lower;
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00019763 File Offset: 0x00017963
		public static bool IsHighSurrogate(char c)
		{
			return char.IsHighSurrogate(c);
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0001976B File Offset: 0x0001796B
		public static bool IsLowSurrogate(char c)
		{
			return char.IsLowSurrogate(c);
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00019773 File Offset: 0x00017973
		public static bool StartsWith(this string source, char value)
		{
			return source.Length > 0 && source[0] == value;
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0001978A File Offset: 0x0001798A
		public static bool EndsWith(this string source, char value)
		{
			return source.Length > 0 && source[source.Length - 1] == value;
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x000197A8 File Offset: 0x000179A8
		public static string Trim(this string s, int start, int length)
		{
			if (s == null)
			{
				throw new ArgumentNullException();
			}
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			int num = start + length - 1;
			if (num >= s.Length)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			while (start < num)
			{
				if (!char.IsWhiteSpace(s[start]))
				{
					IL_6C:
					while (num >= start && char.IsWhiteSpace(s[num]))
					{
						num--;
					}
					return s.Substring(start, num - start + 1);
				}
				start++;
			}
			goto IL_6C;
		}

		// Token: 0x04000201 RID: 513
		public const string CarriageReturnLineFeed = "\r\n";

		// Token: 0x04000202 RID: 514
		public const string Empty = "";

		// Token: 0x04000203 RID: 515
		public const char CarriageReturn = '\r';

		// Token: 0x04000204 RID: 516
		public const char LineFeed = '\n';

		// Token: 0x04000205 RID: 517
		public const char Tab = '\t';

		// Token: 0x0200018D RID: 397
		internal enum SnakeCaseState
		{
			// Token: 0x040006F5 RID: 1781
			Start,
			// Token: 0x040006F6 RID: 1782
			Lower,
			// Token: 0x040006F7 RID: 1783
			Upper,
			// Token: 0x040006F8 RID: 1784
			NewWord
		}
	}
}
