using System;
using System.Management.Automation;
using System.Text;

namespace Microsoft.PowerShell.Cmdletization.Cim
{
	// Token: 0x020009A8 RID: 2472
	internal class WildcardPatternToCimQueryParser : WildcardPatternParser
	{
		// Token: 0x06005B29 RID: 23337 RVA: 0x001EC50C File Offset: 0x001EA70C
		protected override void AppendLiteralCharacter(char c)
		{
			if (c == '%' || c == '[' || c == '_')
			{
				this.BeginBracketExpression();
				this.AppendLiteralCharacterToBracketExpression(c);
				this.EndBracketExpression();
				return;
			}
			this.result.Append(c);
		}

		// Token: 0x06005B2A RID: 23338 RVA: 0x001EC54B File Offset: 0x001EA74B
		protected override void AppendAsterix()
		{
			this.result.Append('%');
		}

		// Token: 0x06005B2B RID: 23339 RVA: 0x001EC55B File Offset: 0x001EA75B
		protected override void AppendQuestionMark()
		{
			this.result.Append('_');
		}

		// Token: 0x06005B2C RID: 23340 RVA: 0x001EC56B File Offset: 0x001EA76B
		protected override void BeginBracketExpression()
		{
			this.result.Append('[');
		}

		// Token: 0x06005B2D RID: 23341 RVA: 0x001EC57C File Offset: 0x001EA77C
		protected override void AppendLiteralCharacterToBracketExpression(char c)
		{
			if (c != '-')
			{
				switch (c)
				{
				case '\\':
				case ']':
				case '^':
					break;
				default:
					this.result.Append(c);
					return;
				}
			}
			this.AppendCharacterRangeToBracketExpression(c, c);
		}

		// Token: 0x06005B2E RID: 23342 RVA: 0x001EC5C0 File Offset: 0x001EA7C0
		protected override void AppendCharacterRangeToBracketExpression(char startOfCharacterRange, char endOfCharacterRange)
		{
			if ('[' <= startOfCharacterRange && startOfCharacterRange <= '^')
			{
				startOfCharacterRange = 'Z';
				this.needClientSideFiltering = true;
			}
			if ('[' <= endOfCharacterRange && endOfCharacterRange <= '^')
			{
				endOfCharacterRange = '_';
				this.needClientSideFiltering = true;
			}
			if (startOfCharacterRange == '-')
			{
				startOfCharacterRange = ',';
				this.needClientSideFiltering = true;
			}
			if (endOfCharacterRange == '-')
			{
				endOfCharacterRange = '.';
				this.needClientSideFiltering = true;
			}
			this.result.Append(startOfCharacterRange);
			this.result.Append('-');
			this.result.Append(endOfCharacterRange);
		}

		// Token: 0x06005B2F RID: 23343 RVA: 0x001EC63F File Offset: 0x001EA83F
		protected override void EndBracketExpression()
		{
			this.result.Append(']');
		}

		// Token: 0x06005B30 RID: 23344 RVA: 0x001EC650 File Offset: 0x001EA850
		internal static string Parse(WildcardPattern wildcardPattern, out bool needsClientSideFiltering)
		{
			WildcardPatternToCimQueryParser wildcardPatternToCimQueryParser = new WildcardPatternToCimQueryParser();
			WildcardPatternParser.Parse(wildcardPattern, wildcardPatternToCimQueryParser);
			needsClientSideFiltering = wildcardPatternToCimQueryParser.needClientSideFiltering;
			return wildcardPatternToCimQueryParser.result.ToString();
		}

		// Token: 0x040030D6 RID: 12502
		private readonly StringBuilder result = new StringBuilder();

		// Token: 0x040030D7 RID: 12503
		private bool needClientSideFiltering;
	}
}
