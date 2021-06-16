using System;
using System.Management.Automation.Remoting;

namespace System.Management.Automation
{
	// Token: 0x0200026A RID: 618
	internal abstract class RemoteSession
	{
		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06001D3A RID: 7482 RVA: 0x000A97CA File Offset: 0x000A79CA
		internal Guid InstanceId
		{
			get
			{
				return this._instanceId;
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06001D3B RID: 7483
		internal abstract RemotingDestination MySelf { get; }

		// Token: 0x06001D3C RID: 7484
		internal abstract void StartKeyExchange();

		// Token: 0x06001D3D RID: 7485
		internal abstract void CompleteKeyExchange();

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06001D3E RID: 7486 RVA: 0x000A97D2 File Offset: 0x000A79D2
		// (set) Token: 0x06001D3F RID: 7487 RVA: 0x000A97DA File Offset: 0x000A79DA
		internal BaseSessionDataStructureHandler BaseSessionDataStructureHandler
		{
			get
			{
				return this._dsHandler;
			}
			set
			{
				this._dsHandler = value;
			}
		}

		// Token: 0x04000CF7 RID: 3319
		private Guid _instanceId = default(Guid);

		// Token: 0x04000CF8 RID: 3320
		private BaseSessionDataStructureHandler _dsHandler;
	}
}
