using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace System.Management.Automation.Language
{
	// Token: 0x020005D4 RID: 1492
	public class StringExpandableToken : StringToken
	{
		// Token: 0x06003FE6 RID: 16358 RVA: 0x00151B9C File Offset: 0x0014FD9C
		internal StringExpandableToken(InternalScriptExtent scriptExtent, TokenKind tokenKind, string value, string formatString, List<Token> nestedTokens, TokenFlags flags) : base(scriptExtent, tokenKind, flags, value)
		{
			if (nestedTokens != null && nestedTokens.Any<Token>())
			{
				this._nestedTokens = new ReadOnlyCollection<Token>(nestedTokens.ToArray());
			}
			this._formatString = formatString;
		}

		// Token: 0x06003FE7 RID: 16359 RVA: 0x00151BD0 File Offset: 0x0014FDD0
		internal static void ToDebugString(ReadOnlyCollection<Token> nestedTokens, StringBuilder sb, int indent)
		{
			foreach (Token token in nestedTokens)
			{
				sb.Append(Environment.NewLine);
				sb.Append(token.ToDebugString(indent + 4));
			}
		}

		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x06003FE8 RID: 16360 RVA: 0x00151C30 File Offset: 0x0014FE30
		// (set) Token: 0x06003FE9 RID: 16361 RVA: 0x00151C38 File Offset: 0x0014FE38
		public ReadOnlyCollection<Token> NestedTokens
		{
			get
			{
				return this._nestedTokens;
			}
			internal set
			{
				this._nestedTokens = value;
			}
		}

		// Token: 0x17000DC5 RID: 3525
		// (get) Token: 0x06003FEA RID: 16362 RVA: 0x00151C41 File Offset: 0x0014FE41
		internal string FormatString
		{
			get
			{
				return this._formatString;
			}
		}

		// Token: 0x06003FEB RID: 16363 RVA: 0x00151C4C File Offset: 0x0014FE4C
		internal override string ToDebugString(int indent)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.ToDebugString(indent));
			if (this._nestedTokens != null)
			{
				StringExpandableToken.ToDebugString(this._nestedTokens, stringBuilder, indent);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04002029 RID: 8233
		private readonly string _formatString;

		// Token: 0x0400202A RID: 8234
		private ReadOnlyCollection<Token> _nestedTokens;
	}
}
