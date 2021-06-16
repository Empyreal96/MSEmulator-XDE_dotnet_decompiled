using System;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Wmi
{
	// Token: 0x02000006 RID: 6
	public interface IXdeWmiVirtualServices : IXdeVirtualServices
	{
		// Token: 0x0600002E RID: 46
		IXdeHyperVManagementService GetHyperVManagementService();
	}
}
