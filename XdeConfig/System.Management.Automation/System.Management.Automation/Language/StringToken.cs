using System;
using System.Globalization;

namespace System.Management.Automation.Language
{
	// Token: 0x020005D2 RID: 1490
	public abstract class StringToken : Token
	{
		// Token: 0x06003FE2 RID: 16354 RVA: 0x00151B23 File Offset: 0x0014FD23
		internal StringToken(InternalScriptExtent scriptExtent, TokenKind kind, TokenFlags tokenFlags, string value) : base(scriptExtent, kind, tokenFlags)
		{
			this._value = value;
		}

		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x06003FE3 RID: 16355 RVA: 0x00151B36 File Offset: 0x0014FD36
		public string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x06003FE4 RID: 16356 RVA: 0x00151B40 File Offset: 0x0014FD40
		internal override string ToDebugString(int indent)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}{1}: <{2}> Value:<{3}>", new object[]
			{
				new string(' ', indent),
				base.Kind,
				base.Text,
				this.Value
			});
		}

		// Token: 0x04002028 RID: 8232
		private readonly string _value;
	}
}
