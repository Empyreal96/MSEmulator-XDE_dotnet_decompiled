using System;

namespace System.Management.Automation
{
	// Token: 0x020000D0 RID: 208
	public class PSEventHandler
	{
		// Token: 0x06000BCF RID: 3023 RVA: 0x00043C96 File Offset: 0x00041E96
		public PSEventHandler()
		{
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00043C9E File Offset: 0x00041E9E
		public PSEventHandler(PSEventManager eventManager, object sender, string sourceIdentifier, PSObject extraData)
		{
			this.eventManager = eventManager;
			this.sender = sender;
			this.sourceIdentifier = sourceIdentifier;
			this.extraData = extraData;
		}

		// Token: 0x04000540 RID: 1344
		protected PSEventManager eventManager;

		// Token: 0x04000541 RID: 1345
		protected object sender;

		// Token: 0x04000542 RID: 1346
		protected string sourceIdentifier;

		// Token: 0x04000543 RID: 1347
		protected PSObject extraData;
	}
}
