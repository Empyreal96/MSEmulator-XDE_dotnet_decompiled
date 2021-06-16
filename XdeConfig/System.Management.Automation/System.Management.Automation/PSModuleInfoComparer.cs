using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x020000B7 RID: 183
	internal sealed class PSModuleInfoComparer : IEqualityComparer<PSModuleInfo>
	{
		// Token: 0x06000A15 RID: 2581 RVA: 0x0003BFE4 File Offset: 0x0003A1E4
		public bool Equals(PSModuleInfo x, PSModuleInfo y)
		{
			return object.ReferenceEquals(x, y) || (!object.ReferenceEquals(x, null) && !object.ReferenceEquals(y, null) && (string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase) && x.Guid == y.Guid) && x.Version == y.Version);
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x0003C050 File Offset: 0x0003A250
		public int GetHashCode(PSModuleInfo obj)
		{
			int num = 0;
			if (obj != null)
			{
				num = 23;
				if (obj.Name != null)
				{
					num = num * 17 + obj.Name.GetHashCode();
				}
				if (obj.Guid != Guid.Empty)
				{
					num = num * 17 + obj.Guid.GetHashCode();
				}
				if (obj.Version != null)
				{
					num = num * 17 + obj.Version.GetHashCode();
				}
			}
			return num;
		}
	}
}
