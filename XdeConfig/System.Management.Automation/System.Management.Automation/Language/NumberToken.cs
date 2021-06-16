using System;
using System.Globalization;

namespace System.Management.Automation.Language
{
	// Token: 0x020005CF RID: 1487
	public class NumberToken : Token
	{
		// Token: 0x06003FD6 RID: 16342 RVA: 0x0015199A File Offset: 0x0014FB9A
		internal NumberToken(InternalScriptExtent scriptExtent, object value, TokenFlags tokenFlags) : base(scriptExtent, TokenKind.Number, tokenFlags)
		{
			this._value = value;
		}

		// Token: 0x06003FD7 RID: 16343 RVA: 0x001519AC File Offset: 0x0014FBAC
		internal override string ToDebugString(int indent)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}{1}: <{2}> Value:<{3}> Type:<{4}>", new object[]
			{
				new string(' ', indent),
				base.Kind,
				base.Text,
				this._value,
				this._value.GetType().Name
			});
		}

		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x06003FD8 RID: 16344 RVA: 0x00151A0E File Offset: 0x0014FC0E
		public object Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x04002024 RID: 8228
		private readonly object _value;
	}
}
