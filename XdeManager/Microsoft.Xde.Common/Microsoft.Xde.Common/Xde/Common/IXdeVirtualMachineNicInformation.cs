using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000055 RID: 85
	public interface IXdeVirtualMachineNicInformation
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001AE RID: 430
		IXdeVirtualSwitchInformation SwitchInformation { get; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001AF RID: 431
		string GuestMacAddress { get; }

		// Token: 0x060001B0 RID: 432
		void CleanupForDeletion();
	}
}
