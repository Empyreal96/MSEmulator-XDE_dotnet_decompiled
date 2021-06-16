using System;
using System.Collections.Generic;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x02000335 RID: 821
	public class RunspaceRepository : Repository<PSSession>
	{
		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x060027C3 RID: 10179 RVA: 0x000DEE08 File Offset: 0x000DD008
		public List<PSSession> Runspaces
		{
			get
			{
				return base.Items;
			}
		}

		// Token: 0x060027C4 RID: 10180 RVA: 0x000DEE10 File Offset: 0x000DD010
		internal RunspaceRepository() : base("runspace")
		{
		}

		// Token: 0x060027C5 RID: 10181 RVA: 0x000DEE1D File Offset: 0x000DD01D
		protected override Guid GetKey(PSSession item)
		{
			if (item != null)
			{
				return item.InstanceId;
			}
			return Guid.Empty;
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x000DEE2E File Offset: 0x000DD02E
		internal void AddOrReplace(PSSession item)
		{
			if (base.Dictionary.ContainsKey(this.GetKey(item)))
			{
				base.Dictionary.Remove(this.GetKey(item));
			}
			base.Dictionary.Add(item.InstanceId, item);
		}
	}
}
