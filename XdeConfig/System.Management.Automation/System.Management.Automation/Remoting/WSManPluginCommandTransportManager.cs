using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020003F2 RID: 1010
	internal class WSManPluginCommandTransportManager : WSManPluginServerTransportManager
	{
		// Token: 0x06002D91 RID: 11665 RVA: 0x000FC719 File Offset: 0x000FA919
		internal WSManPluginCommandTransportManager(WSManPluginServerTransportManager srvrTransportMgr) : base(srvrTransportMgr.Fragmentor.FragmentSize, srvrTransportMgr.CryptoHelper)
		{
			this.serverTransportMgr = srvrTransportMgr;
			base.TypeTable = srvrTransportMgr.TypeTable;
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x000FC745 File Offset: 0x000FA945
		internal void Initialize()
		{
			base.PowerShellGuidObserver += this.OnPowershellGuidReported;
			base.MigrateDataReadyEventHandlers(this.serverTransportMgr);
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x000FC765 File Offset: 0x000FA965
		private void OnPowershellGuidReported(object src, EventArgs args)
		{
			this.cmdId = (Guid)src;
			this.serverTransportMgr.ReportTransportMgrForCmd(this.cmdId, this);
			base.PowerShellGuidObserver -= this.OnPowershellGuidReported;
		}

		// Token: 0x040017EE RID: 6126
		private WSManPluginServerTransportManager serverTransportMgr;

		// Token: 0x040017EF RID: 6127
		private Guid cmdId;
	}
}
