using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x0200033B RID: 827
	public class JobRepository : Repository<Job>
	{
		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x0600282F RID: 10287 RVA: 0x000DFD3D File Offset: 0x000DDF3D
		public List<Job> Jobs
		{
			get
			{
				return base.Items;
			}
		}

		// Token: 0x06002830 RID: 10288 RVA: 0x000DFD45 File Offset: 0x000DDF45
		public Job GetJob(Guid instanceId)
		{
			return base.GetItem(instanceId);
		}

		// Token: 0x06002831 RID: 10289 RVA: 0x000DFD4E File Offset: 0x000DDF4E
		internal JobRepository() : base("job")
		{
		}

		// Token: 0x06002832 RID: 10290 RVA: 0x000DFD5B File Offset: 0x000DDF5B
		protected override Guid GetKey(Job item)
		{
			if (item != null)
			{
				return item.InstanceId;
			}
			return Guid.Empty;
		}
	}
}
