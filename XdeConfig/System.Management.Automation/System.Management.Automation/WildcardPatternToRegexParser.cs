using System;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Management.Automation
{
	// Token: 0x0200043B RID: 1083
	internal class WildcardPatternToRegexParser : WildcardPatternParser
	{
		// Token: 0x06002F98 RID: 12184 RVA: 0x00104E10 File Offset: 0x00103010
		private static bool IsRegexChar(char ch)
		{
			for (int i = 0; i < "()[.?*{}^$+|\\".Length; i++)
			{
				if (ch == "()[.?*{}^$+|\\"[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F99 RID: 12185 RVA: 0x00104E44 File Offset: 0x00103044
		internal static RegexOptions TranslateWildcardOptionsIntoRegexOptions(WildcardOptions options)
		{
			RegexOptions regexOptions = RegexOptions.Singleline;
			if ((options & WildcardOptions.Compiled) != WildcardOptions.None)
			{
				regexOptions |= RegexOptions.Compiled;
			}
			if ((options & WildcardOptions.IgnoreCase) != WildcardOptions.None)
			{
				regexOptions |= RegexOptions.IgnoreCase;
			}
			if ((options & WildcardOptions.CultureInvariant) == WildcardOptions.CultureInvariant)
			{
				regexOptions |= RegexOptions.CultureInvariant;
			}
			return regexOptions;
		}

		// Token: 0x06002F9A RID: 12186 RVA: 0x00104E75 File Offset: 0x00103075
		protected override void BeginWildcardPattern(WildcardPattern pattern)
		{
			this.regexPattern = new StringBuilder(pattern.Pattern.Length * 2 + 2);
			this.regexPattern.Append('^');
			this.regexOptions = WildcardPatternToRegexParser.TranslateWildcardOptionsIntoRegexOptions(pattern.Options);
		}

		// Token: 0x06002F9B RID: 12187 RVA: 0x00104EB0 File Offset: 0x001030B0
		internal static void AppendLiteralCharacter(StringBuilder regexPattern, char c)
		{
			if (WildcardPatternToRegexParser.IsRegexChar(c))
			{
				regexPattern.Append('\\');
			}
			regexPattern.Append(c);
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x00104ECB File Offset: 0x001030CB
		protected override void AppendLiteralCharacter(char c)
		{
			WildcardPatternToRegexParser.AppendLiteralCharacter(this.regexPattern, c);
		}

		// Token: 0x06002F9D RID: 12189 RVA: 0x00104ED9 File Offset: 0x001030D9
		protected override void AppendAsterix()
		{
			this.regexPattern.Append(".*");
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x00104EEC File Offset: 0x001030EC
		protected override void AppendQuestionMark()
		{
			this.regexPattern.Append('.');
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x00104EFC File Offset: 0x001030FC
		protected override void EndWildcardPattern()
		{
			this.regexPattern.Append('$');
			string text = this.regexPattern.ToString();
			if (text.Equals("^.*$", StringComparison.Ordinal))
			{
				this.regexPattern.Remove(0, 4);
				return;
			}
			if (text.StartsWith("^.*", StringComparison.Ordinal))
			{
				this.regexPattern.Remove(0, 3);
			}
			if (text.EndsWith(".*$", StringComparison.Ordinal))
			{
				this.regexPattern.Remove(this.regexPattern.Length - 3, 3);
			}
		}

		// Token: 0x06002FA0 RID: 12192 RVA: 0x00104F84 File Offset: 0x00103184
		protected override void BeginBracketExpression()
		{
			this.regexPattern.Append('[');
		}

		// Token: 0x06002FA1 RID: 12193 RVA: 0x00104F94 File Offset: 0x00103194
		internal static void AppendLiteralCharacterToBracketExpression(StringBuilder regexPattern, char c)
		{
			if (c == '[')
			{
				regexPattern.Append('[');
				return;
			}
			if (c == ']')
			{
				regexPattern.Append("\\]");
				return;
			}
			if (c == '-')
			{
				regexPattern.Append("\\x2d");
				return;
			}
			WildcardPatternToRegexParser.AppendLiteralCharacter(regexPattern, c);
		}

		// Token: 0x06002FA2 RID: 12194 RVA: 0x00104FD0 File Offset: 0x001031D0
		protected override void AppendLiteralCharacterToBracketExpression(char c)
		{
			WildcardPatternToRegexParser.AppendLiteralCharacterToBracketExpression(this.regexPattern, c);
		}

		// Token: 0x06002FA3 RID: 12195 RVA: 0x00104FDE File Offset: 0x001031DE
		internal static void AppendCharacterRangeToBracketExpression(StringBuilder regexPattern, char startOfCharacterRange, char endOfCharacterRange)
		{
			WildcardPatternToRegexParser.AppendLiteralCharacterToBracketExpression(regexPattern, startOfCharacterRange);
			regexPattern.Append('-');
			WildcardPatternToRegexParser.AppendLiteralCharacterToBracketExpression(regexPattern, endOfCharacterRange);
		}

		// Token: 0x06002FA4 RID: 12196 RVA: 0x00104FF7 File Offset: 0x001031F7
		protected override void AppendCharacterRangeToBracketExpression(char startOfCharacterRange, char endOfCharacterRange)
		{
			WildcardPatternToRegexParser.AppendCharacterRangeToBracketExpression(this.regexPattern, startOfCharacterRange, endOfCharacterRange);
		}

		// Token: 0x06002FA5 RID: 12197 RVA: 0x00105006 File Offset: 0x00103206
		protected override void EndBracketExpression()
		{
			this.regexPattern.Append(']');
		}

		// Token: 0x06002FA6 RID: 12198 RVA: 0x00105018 File Offset: 0x00103218
		public static Regex Parse(WildcardPattern wildcardPattern)
		{
			WildcardPatternToRegexParser wildcardPatternToRegexParser = new WildcardPatternToRegexParser();
			WildcardPatternParser.Parse(wildcardPattern, wildcardPatternToRegexParser);
			Regex result;
			try
			{
				result = new Regex(wildcardPatternToRegexParser.regexPattern.ToString(), wildcardPatternToRegexParser.regexOptions);
			}
			catch (ArgumentException)
			{
				throw WildcardPatternParser.NewWildcardPatternException(wildcardPattern.Pattern);
			}
			return result;
		}

		// Token: 0x040019C5 RID: 6597
		private const string regexChars = "()[.?*{}^$+|\\";

		// Token: 0x040019C6 RID: 6598
		private StringBuilder regexPattern;

		// Token: 0x040019C7 RID: 6599
		private RegexOptions regexOptions;
	}
}
