using System;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x02000445 RID: 1093
	internal class WildcardPatternToDosWildcardParser : WildcardPatternParser
	{
		// Token: 0x06002FC8 RID: 12232 RVA: 0x00105495 File Offset: 0x00103695
		protected override void AppendLiteralCharacter(char c)
		{
			this.result.Append(c);
		}

		// Token: 0x06002FC9 RID: 12233 RVA: 0x001054A4 File Offset: 0x001036A4
		protected override void AppendAsterix()
		{
			this.result.Append('*');
		}

		// Token: 0x06002FCA RID: 12234 RVA: 0x001054B4 File Offset: 0x001036B4
		protected override void AppendQuestionMark()
		{
			this.result.Append('?');
		}

		// Token: 0x06002FCB RID: 12235 RVA: 0x001054C4 File Offset: 0x001036C4
		protected override void BeginBracketExpression()
		{
		}

		// Token: 0x06002FCC RID: 12236 RVA: 0x001054C6 File Offset: 0x001036C6
		protected override void AppendLiteralCharacterToBracketExpression(char c)
		{
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x001054C8 File Offset: 0x001036C8
		protected override void AppendCharacterRangeToBracketExpression(char startOfCharacterRange, char endOfCharacterRange)
		{
		}

		// Token: 0x06002FCE RID: 12238 RVA: 0x001054CA File Offset: 0x001036CA
		protected override void EndBracketExpression()
		{
			this.result.Append('?');
		}

		// Token: 0x06002FCF RID: 12239 RVA: 0x001054DC File Offset: 0x001036DC
		internal static string Parse(WildcardPattern wildcardPattern)
		{
			WildcardPatternToDosWildcardParser wildcardPatternToDosWildcardParser = new WildcardPatternToDosWildcardParser();
			WildcardPatternParser.Parse(wildcardPattern, wildcardPatternToDosWildcardParser);
			return wildcardPatternToDosWildcardParser.result.ToString();
		}

		// Token: 0x040019D7 RID: 6615
		private readonly StringBuilder result = new StringBuilder();
	}
}
