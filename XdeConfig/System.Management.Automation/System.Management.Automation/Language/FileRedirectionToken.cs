using System;

namespace System.Management.Automation.Language
{
	// Token: 0x020005D9 RID: 1497
	public class FileRedirectionToken : RedirectionToken
	{
		// Token: 0x06003FF5 RID: 16373 RVA: 0x00151CF3 File Offset: 0x0014FEF3
		internal FileRedirectionToken(InternalScriptExtent scriptExtent, RedirectionStream from, bool append) : base(scriptExtent, TokenKind.Redirection)
		{
			this.FromStream = from;
			this.Append = append;
		}

		// Token: 0x17000DC9 RID: 3529
		// (get) Token: 0x06003FF6 RID: 16374 RVA: 0x00151D0C File Offset: 0x0014FF0C
		// (set) Token: 0x06003FF7 RID: 16375 RVA: 0x00151D14 File Offset: 0x0014FF14
		public RedirectionStream FromStream { get; private set; }

		// Token: 0x17000DCA RID: 3530
		// (get) Token: 0x06003FF8 RID: 16376 RVA: 0x00151D1D File Offset: 0x0014FF1D
		// (set) Token: 0x06003FF9 RID: 16377 RVA: 0x00151D25 File Offset: 0x0014FF25
		public bool Append { get; private set; }
	}
}
