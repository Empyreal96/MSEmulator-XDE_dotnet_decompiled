using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.PowerShell.Cmdletization.Cim;

namespace System.Management.Automation
{
	// Token: 0x02000438 RID: 1080
	public sealed class WildcardPattern
	{
		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x06002F79 RID: 12153 RVA: 0x001048BA File Offset: 0x00102ABA
		internal string Pattern
		{
			get
			{
				return this.pattern;
			}
		}

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x06002F7A RID: 12154 RVA: 0x001048C2 File Offset: 0x00102AC2
		internal WildcardOptions Options
		{
			get
			{
				return this.options;
			}
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x06002F7B RID: 12155 RVA: 0x001048CC File Offset: 0x00102ACC
		internal string PatternConvertedToRegex
		{
			get
			{
				Regex regex = WildcardPatternToRegexParser.Parse(this);
				return regex.ToString();
			}
		}

		// Token: 0x06002F7C RID: 12156 RVA: 0x001048E6 File Offset: 0x00102AE6
		public WildcardPattern(string pattern)
		{
			if (pattern == null)
			{
				throw PSTraceSource.NewArgumentNullException("pattern");
			}
			this.pattern = pattern;
		}

		// Token: 0x06002F7D RID: 12157 RVA: 0x00104903 File Offset: 0x00102B03
		public WildcardPattern(string pattern, WildcardOptions options)
		{
			if (pattern == null)
			{
				throw PSTraceSource.NewArgumentNullException("pattern");
			}
			this.pattern = pattern;
			this.options = options;
		}

		// Token: 0x06002F7E RID: 12158 RVA: 0x00104928 File Offset: 0x00102B28
		private bool Init()
		{
			if (this._isMatch == null)
			{
				bool flag = false;
				if (flag)
				{
					Regex @object = WildcardPatternToRegexParser.Parse(this);
					this._isMatch = new Predicate<string>(@object.IsMatch);
				}
				else
				{
					WildcardPatternMatcher object2 = new WildcardPatternMatcher(this);
					this._isMatch = new Predicate<string>(object2.IsMatch);
				}
			}
			return this._isMatch != null;
		}

		// Token: 0x06002F7F RID: 12159 RVA: 0x00104984 File Offset: 0x00102B84
		public bool IsMatch(string input)
		{
			if (input == null)
			{
				return false;
			}
			bool result = false;
			if (this.Init())
			{
				result = this._isMatch(input);
			}
			return result;
		}

		// Token: 0x06002F80 RID: 12160 RVA: 0x001049B0 File Offset: 0x00102BB0
		internal static string Escape(string pattern, char[] charsNotToEscape)
		{
			if (pattern == null)
			{
				throw PSTraceSource.NewArgumentNullException("pattern");
			}
			if (charsNotToEscape == null)
			{
				throw PSTraceSource.NewArgumentNullException("charsNotToEscape");
			}
			char[] array = new char[pattern.Length * 2 + 1];
			int num = 0;
			foreach (char c in pattern)
			{
				if (WildcardPattern.IsWildcardChar(c) && !charsNotToEscape.Contains(c))
				{
					array[num++] = '`';
				}
				array[num++] = c;
			}
			string result;
			if (num > 0)
			{
				result = new string(array, 0, num);
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x06002F81 RID: 12161 RVA: 0x00104A42 File Offset: 0x00102C42
		public static string Escape(string pattern)
		{
			return WildcardPattern.Escape(pattern, new char[0]);
		}

		// Token: 0x06002F82 RID: 12162 RVA: 0x00104A50 File Offset: 0x00102C50
		public static bool ContainsWildcardCharacters(string pattern)
		{
			if (string.IsNullOrEmpty(pattern))
			{
				return false;
			}
			bool result = false;
			for (int i = 0; i < pattern.Length; i++)
			{
				if (WildcardPattern.IsWildcardChar(pattern[i]))
				{
					result = true;
					break;
				}
				if (pattern[i] == '`')
				{
					i++;
				}
			}
			return result;
		}

		// Token: 0x06002F83 RID: 12163 RVA: 0x00104A9C File Offset: 0x00102C9C
		public static string Unescape(string pattern)
		{
			if (pattern == null)
			{
				throw PSTraceSource.NewArgumentNullException("pattern");
			}
			char[] array = new char[pattern.Length];
			int num = 0;
			bool flag = false;
			foreach (char c in pattern)
			{
				if (c == '`')
				{
					if (flag)
					{
						array[num++] = c;
						flag = false;
					}
					else
					{
						flag = true;
					}
				}
				else
				{
					if (flag && !WildcardPattern.IsWildcardChar(c))
					{
						array[num++] = '`';
					}
					array[num++] = c;
					flag = false;
				}
			}
			if (flag)
			{
				array[num++] = '`';
			}
			string result;
			if (num > 0)
			{
				result = new string(array, 0, num);
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x00104B45 File Offset: 0x00102D45
		private static bool IsWildcardChar(char ch)
		{
			return ch == '*' || ch == '?' || ch == '[' || ch == ']';
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x00104B60 File Offset: 0x00102D60
		public string ToWql()
		{
			bool flag;
			string result = WildcardPatternToCimQueryParser.Parse(this, out flag);
			if (!flag)
			{
				return result;
			}
			throw new PSInvalidCastException("UnsupportedWildcardToWqlConversion", null, ExtendedTypeSystem.InvalidCastException, new object[]
			{
				this.Pattern,
				base.GetType().FullName,
				"WQL"
			});
		}

		// Token: 0x040019C0 RID: 6592
		private const char escapeChar = '`';

		// Token: 0x040019C1 RID: 6593
		private Predicate<string> _isMatch;

		// Token: 0x040019C2 RID: 6594
		private readonly string pattern;

		// Token: 0x040019C3 RID: 6595
		private readonly WildcardOptions options;
	}
}
