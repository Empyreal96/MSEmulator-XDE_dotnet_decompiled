using System;
using System.Management.Automation.Language;

namespace System.Management.Automation
{
	// Token: 0x0200049D RID: 1181
	public sealed class PSParseError
	{
		// Token: 0x060034DE RID: 13534 RVA: 0x0011F41F File Offset: 0x0011D61F
		internal PSParseError(RuntimeException rte)
		{
			this._message = rte.Message;
			this._psToken = new PSToken(rte.ErrorToken);
		}

		// Token: 0x060034DF RID: 13535 RVA: 0x0011F444 File Offset: 0x0011D644
		internal PSParseError(ParseError error)
		{
			this._message = error.Message;
			this._psToken = new PSToken(error.Extent);
		}

		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x060034E0 RID: 13536 RVA: 0x0011F469 File Offset: 0x0011D669
		public PSToken Token
		{
			get
			{
				return this._psToken;
			}
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x060034E1 RID: 13537 RVA: 0x0011F471 File Offset: 0x0011D671
		public string Message
		{
			get
			{
				return this._message;
			}
		}

		// Token: 0x04001B15 RID: 6933
		private PSToken _psToken;

		// Token: 0x04001B16 RID: 6934
		private string _message;
	}
}
