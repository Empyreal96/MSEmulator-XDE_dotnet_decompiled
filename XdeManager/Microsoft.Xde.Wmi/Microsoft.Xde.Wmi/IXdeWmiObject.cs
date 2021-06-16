using System;
using System.Management;

namespace Microsoft.Xde.Wmi
{
	// Token: 0x02000004 RID: 4
	public interface IXdeWmiObject
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000D RID: 13
		ManagementObject WmiObject { get; }
	}
}
