using System;

namespace CommandLine.Core
{
	// Token: 0x0200007E RID: 126
	internal abstract class Token
	{
		// Token: 0x060002FC RID: 764 RVA: 0x0000BF48 File Offset: 0x0000A148
		protected Token(TokenType tag, string text)
		{
			this.tag = tag;
			this.text = text;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000BF5E File Offset: 0x0000A15E
		public static Token Name(string text)
		{
			return new Name(text);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000BF66 File Offset: 0x0000A166
		public static Token Value(string text)
		{
			return new Value(text);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000BF6E File Offset: 0x0000A16E
		public static Token Value(string text, bool explicitlyAssigned)
		{
			return new Value(text, explicitlyAssigned);
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000300 RID: 768 RVA: 0x0000BF77 File Offset: 0x0000A177
		public TokenType Tag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000301 RID: 769 RVA: 0x0000BF7F File Offset: 0x0000A17F
		public string Text
		{
			get
			{
				return this.text;
			}
		}

		// Token: 0x040000E5 RID: 229
		private readonly TokenType tag;

		// Token: 0x040000E6 RID: 230
		private readonly string text;
	}
}
