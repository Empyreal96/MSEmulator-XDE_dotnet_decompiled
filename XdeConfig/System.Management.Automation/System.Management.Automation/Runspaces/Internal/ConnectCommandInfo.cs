using System;

namespace System.Management.Automation.Runspaces.Internal
{
	// Token: 0x020002A0 RID: 672
	internal class ConnectCommandInfo
	{
		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06002059 RID: 8281 RVA: 0x000BB802 File Offset: 0x000B9A02
		public Guid CommandId
		{
			get
			{
				return this.cmdId;
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x0600205A RID: 8282 RVA: 0x000BB80A File Offset: 0x000B9A0A
		public string Command
		{
			get
			{
				return this.cmdStr;
			}
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x000BB812 File Offset: 0x000B9A12
		public ConnectCommandInfo(Guid cmdId, string cmdStr)
		{
			this.cmdId = cmdId;
			this.cmdStr = cmdStr;
		}

		// Token: 0x04000E3D RID: 3645
		private Guid cmdId = Guid.Empty;

		// Token: 0x04000E3E RID: 3646
		private string cmdStr = string.Empty;
	}
}
