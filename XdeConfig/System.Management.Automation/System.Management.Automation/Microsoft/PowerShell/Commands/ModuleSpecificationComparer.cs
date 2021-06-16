using System;
using System.Collections.Generic;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020000B0 RID: 176
	internal class ModuleSpecificationComparer : IEqualityComparer<ModuleSpecification>
	{
		// Token: 0x06000905 RID: 2309 RVA: 0x00036DE0 File Offset: 0x00034FE0
		public bool Equals(ModuleSpecification x, ModuleSpecification y)
		{
			bool flag = false;
			if (x == null && y == null)
			{
				flag = true;
			}
			else if (x != null && y != null)
			{
				flag = (x.Name == null || y.Name == null || x.Name.Equals(y.Name, StringComparison.OrdinalIgnoreCase));
				if (flag && x.Guid != null && y.Guid != null)
				{
					flag = x.Guid.Equals(y.Guid);
				}
				if (flag)
				{
					if (x.Version != null && y.Version != null)
					{
						flag = x.Version.Equals(y.Version);
					}
					else if (x.Version != null || y.Version != null)
					{
						flag = false;
					}
					if (x.MaximumVersion != null && y.MaximumVersion != null)
					{
						flag = x.MaximumVersion.Equals(y.MaximumVersion);
					}
					else if (x.MaximumVersion != null || y.MaximumVersion != null)
					{
						flag = false;
					}
					if (flag && x.RequiredVersion != null && y.RequiredVersion != null)
					{
						flag = x.RequiredVersion.Equals(y.RequiredVersion);
					}
					else if (flag && (x.RequiredVersion != null || y.RequiredVersion != null))
					{
						flag = false;
					}
				}
			}
			return flag;
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x00036F54 File Offset: 0x00035154
		public int GetHashCode(ModuleSpecification obj)
		{
			int num = 0;
			if (obj != null)
			{
				if (obj.Name != null)
				{
					num ^= obj.Name.GetHashCode();
				}
				if (obj.Guid != null)
				{
					num ^= obj.Guid.GetHashCode();
				}
				if (obj.Version != null)
				{
					num ^= obj.Version.GetHashCode();
				}
				if (obj.MaximumVersion != null)
				{
					num ^= obj.MaximumVersion.GetHashCode();
				}
				if (obj.RequiredVersion != null)
				{
					num ^= obj.RequiredVersion.GetHashCode();
				}
			}
			return num;
		}
	}
}
