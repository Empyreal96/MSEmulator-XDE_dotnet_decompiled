using System;

namespace System.Management.Automation
{
	// Token: 0x0200047C RID: 1148
	internal static class SpecialCharacters
	{
		// Token: 0x06003333 RID: 13107 RVA: 0x00118111 File Offset: 0x00116311
		public static bool IsDash(char c)
		{
			return c == '–' || c == '—' || c == '―' || c == '-';
		}

		// Token: 0x06003334 RID: 13108 RVA: 0x00118132 File Offset: 0x00116332
		public static bool IsSingleQuote(char c)
		{
			return c == '‘' || c == '’' || c == '‚' || c == '‛' || c == '\'';
		}

		// Token: 0x06003335 RID: 13109 RVA: 0x0011815B File Offset: 0x0011635B
		public static bool IsDoubleQuote(char c)
		{
			return c == '"' || c == '“' || c == '”' || c == '„';
		}

		// Token: 0x06003336 RID: 13110 RVA: 0x0011817C File Offset: 0x0011637C
		public static bool IsQuote(char c)
		{
			return SpecialCharacters.IsSingleQuote(c) || SpecialCharacters.IsDoubleQuote(c);
		}

		// Token: 0x06003337 RID: 13111 RVA: 0x0011818E File Offset: 0x0011638E
		public static bool IsDelimiter(char c, char delimiter)
		{
			if (delimiter == '"')
			{
				return SpecialCharacters.IsDoubleQuote(c);
			}
			if (delimiter == '\'')
			{
				return SpecialCharacters.IsSingleQuote(c);
			}
			return c == delimiter;
		}

		// Token: 0x06003338 RID: 13112 RVA: 0x001181AC File Offset: 0x001163AC
		public static bool IsCurlyBracket(char c)
		{
			return c == '{' || c == '}';
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x001181BA File Offset: 0x001163BA
		public static char AsQuote(char c)
		{
			if (SpecialCharacters.IsSingleQuote(c))
			{
				return '\'';
			}
			if (SpecialCharacters.IsDoubleQuote(c))
			{
				return '"';
			}
			return c;
		}

		// Token: 0x04001A91 RID: 6801
		public const char enDash = '–';

		// Token: 0x04001A92 RID: 6802
		public const char emDash = '—';

		// Token: 0x04001A93 RID: 6803
		public const char horizontalBar = '―';

		// Token: 0x04001A94 RID: 6804
		public const char quoteSingleLeft = '‘';

		// Token: 0x04001A95 RID: 6805
		public const char quoteSingleRight = '’';

		// Token: 0x04001A96 RID: 6806
		public const char quoteSingleBase = '‚';

		// Token: 0x04001A97 RID: 6807
		public const char quoteReversed = '‛';

		// Token: 0x04001A98 RID: 6808
		public const char quoteDoubleLeft = '“';

		// Token: 0x04001A99 RID: 6809
		public const char quoteDoubleRight = '”';

		// Token: 0x04001A9A RID: 6810
		public const char quoteLowDoubleLeft = '„';
	}
}
