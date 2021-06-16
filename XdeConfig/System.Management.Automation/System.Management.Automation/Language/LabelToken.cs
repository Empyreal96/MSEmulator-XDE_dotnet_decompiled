using System;

namespace System.Management.Automation.Language
{
	// Token: 0x020005D5 RID: 1493
	public class LabelToken : Token
	{
		// Token: 0x06003FEC RID: 16364 RVA: 0x00151C88 File Offset: 0x0014FE88
		internal LabelToken(InternalScriptExtent scriptExtent, TokenFlags tokenFlags, string labelText) : base(scriptExtent, TokenKind.Label, tokenFlags)
		{
			this._labelText = labelText;
		}

		// Token: 0x17000DC6 RID: 3526
		// (get) Token: 0x06003FED RID: 16365 RVA: 0x00151C9A File Offset: 0x0014FE9A
		public string LabelText
		{
			get
			{
				return this._labelText;
			}
		}

		// Token: 0x0400202B RID: 8235
		private readonly string _labelText;
	}
}
