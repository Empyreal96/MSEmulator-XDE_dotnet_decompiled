using System;
using System.Management.Automation.Internal;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x0200043A RID: 1082
	internal abstract class WildcardPatternParser
	{
		// Token: 0x06002F8B RID: 12171 RVA: 0x00104BFA File Offset: 0x00102DFA
		protected virtual void BeginWildcardPattern(WildcardPattern pattern)
		{
		}

		// Token: 0x06002F8C RID: 12172
		protected abstract void AppendLiteralCharacter(char c);

		// Token: 0x06002F8D RID: 12173
		protected abstract void AppendAsterix();

		// Token: 0x06002F8E RID: 12174
		protected abstract void AppendQuestionMark();

		// Token: 0x06002F8F RID: 12175 RVA: 0x00104BFC File Offset: 0x00102DFC
		protected virtual void EndWildcardPattern()
		{
		}

		// Token: 0x06002F90 RID: 12176
		protected abstract void BeginBracketExpression();

		// Token: 0x06002F91 RID: 12177
		protected abstract void AppendLiteralCharacterToBracketExpression(char c);

		// Token: 0x06002F92 RID: 12178
		protected abstract void AppendCharacterRangeToBracketExpression(char startOfCharacterRange, char endOfCharacterRange);

		// Token: 0x06002F93 RID: 12179
		protected abstract void EndBracketExpression();

		// Token: 0x06002F94 RID: 12180 RVA: 0x00104C00 File Offset: 0x00102E00
		internal void AppendBracketExpression(string brackedExpressionContents, string bracketExpressionOperators, string pattern)
		{
			this.BeginBracketExpression();
			int i = 0;
			while (i < brackedExpressionContents.Length)
			{
				if (i + 2 < brackedExpressionContents.Length && bracketExpressionOperators[i + 1] == '-')
				{
					char c = brackedExpressionContents[i];
					char c2 = brackedExpressionContents[i + 2];
					i += 3;
					if (c > c2)
					{
						throw WildcardPatternParser.NewWildcardPatternException(pattern);
					}
					this.AppendCharacterRangeToBracketExpression(c, c2);
				}
				else
				{
					this.AppendLiteralCharacterToBracketExpression(brackedExpressionContents[i]);
					i++;
				}
			}
			this.EndBracketExpression();
		}

		// Token: 0x06002F95 RID: 12181 RVA: 0x00104C7C File Offset: 0x00102E7C
		public static void Parse(WildcardPattern pattern, WildcardPatternParser parser)
		{
			parser.BeginWildcardPattern(pattern);
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			StringBuilder stringBuilder = null;
			StringBuilder stringBuilder2 = null;
			foreach (char c in pattern.Pattern)
			{
				if (flag3)
				{
					if (c == ']' && !flag2 && !flag)
					{
						flag3 = false;
						parser.AppendBracketExpression(stringBuilder.ToString(), stringBuilder2.ToString(), pattern.Pattern);
						stringBuilder = null;
						stringBuilder2 = null;
					}
					else if (c != '`' || flag)
					{
						stringBuilder.Append(c);
						stringBuilder2.Append((c == '-' && !flag) ? '-' : ' ');
					}
					flag2 = false;
				}
				else if (c == '*' && !flag)
				{
					parser.AppendAsterix();
				}
				else if (c == '?' && !flag)
				{
					parser.AppendQuestionMark();
				}
				else if (c == '[' && !flag)
				{
					flag3 = true;
					stringBuilder = new StringBuilder();
					stringBuilder2 = new StringBuilder();
					flag2 = true;
				}
				else if (c != '`' || flag)
				{
					parser.AppendLiteralCharacter(c);
				}
				flag = (c == '`' && !flag);
			}
			if (flag3)
			{
				throw WildcardPatternParser.NewWildcardPatternException(pattern.Pattern);
			}
			if (flag && !pattern.Pattern.Equals("`", StringComparison.Ordinal))
			{
				parser.AppendLiteralCharacter(pattern.Pattern[pattern.Pattern.Length - 1]);
			}
			parser.EndWildcardPattern();
		}

		// Token: 0x06002F96 RID: 12182 RVA: 0x00104DD0 File Offset: 0x00102FD0
		internal static WildcardPatternException NewWildcardPatternException(string invalidPattern)
		{
			string message = StringUtil.Format(WildcardPatternStrings.InvalidPattern, invalidPattern);
			ParentContainsErrorRecordException exception = new ParentContainsErrorRecordException(message);
			ErrorRecord errorRecord = new ErrorRecord(exception, "WildcardPattern_Invalid", ErrorCategory.InvalidArgument, null);
			return new WildcardPatternException(errorRecord);
		}
	}
}
