using System;

namespace System.Management.Automation.Language
{
	// Token: 0x020005D8 RID: 1496
	public class MergingRedirectionToken : RedirectionToken
	{
		// Token: 0x06003FF0 RID: 16368 RVA: 0x00151CB8 File Offset: 0x0014FEB8
		internal MergingRedirectionToken(InternalScriptExtent scriptExtent, RedirectionStream from, RedirectionStream to) : base(scriptExtent, TokenKind.Redirection)
		{
			this.FromStream = from;
			this.ToStream = to;
		}

		// Token: 0x17000DC7 RID: 3527
		// (get) Token: 0x06003FF1 RID: 16369 RVA: 0x00151CD1 File Offset: 0x0014FED1
		// (set) Token: 0x06003FF2 RID: 16370 RVA: 0x00151CD9 File Offset: 0x0014FED9
		public RedirectionStream FromStream { get; private set; }

		// Token: 0x17000DC8 RID: 3528
		// (get) Token: 0x06003FF3 RID: 16371 RVA: 0x00151CE2 File Offset: 0x0014FEE2
		// (set) Token: 0x06003FF4 RID: 16372 RVA: 0x00151CEA File Offset: 0x0014FEEA
		public RedirectionStream ToStream { get; private set; }
	}
}
