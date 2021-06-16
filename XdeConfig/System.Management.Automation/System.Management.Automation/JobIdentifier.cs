using System;

namespace System.Management.Automation
{
	// Token: 0x02000272 RID: 626
	public sealed class JobIdentifier
	{
		// Token: 0x06001D7A RID: 7546 RVA: 0x000AA108 File Offset: 0x000A8308
		internal JobIdentifier(int id, Guid instanceId)
		{
			if (id <= 0)
			{
				PSTraceSource.NewArgumentException("id", RemotingErrorIdStrings.JobSessionIdLessThanOne, new object[]
				{
					id
				});
			}
			this.Id = id;
			this.InstanceId = instanceId;
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06001D7B RID: 7547 RVA: 0x000AA14E File Offset: 0x000A834E
		// (set) Token: 0x06001D7C RID: 7548 RVA: 0x000AA156 File Offset: 0x000A8356
		internal int Id { get; private set; }

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06001D7D RID: 7549 RVA: 0x000AA15F File Offset: 0x000A835F
		// (set) Token: 0x06001D7E RID: 7550 RVA: 0x000AA167 File Offset: 0x000A8367
		internal Guid InstanceId { get; private set; }
	}
}
