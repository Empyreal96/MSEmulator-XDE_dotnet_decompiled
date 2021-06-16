using System;
using System.Collections.ObjectModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200003A RID: 58
	public interface IXdeVirtualServices
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000165 RID: 357
		bool CanAccessApi { get; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000166 RID: 358
		bool IsGpuSupported { get; }

		// Token: 0x06000167 RID: 359
		IXdeVirtualMachine GetVirtualMachine(string virtualMachineName);

		// Token: 0x06000168 RID: 360
		IXdeVirtualMachine GetVirtualMachine(string virtualMachineName, SettingsOptions options);

		// Token: 0x06000169 RID: 361
		IXdeVirtualMachine CreateVirtualMachine(string virtualMachineName);

		// Token: 0x0600016A RID: 362
		IXdeVirtualMachine CreateVirtualMachine(string virtualMachineName, bool generation2);

		// Token: 0x0600016B RID: 363
		ReadOnlyCollection<IXdeVirtualMachine> GetAllXdeVirtualMachines(SettingsOptions settingsOptions);
	}
}
