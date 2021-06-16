using System;

namespace Microsoft.Tools.WindowsDevicePortal
{
	// Token: 0x02000010 RID: 16
	public enum DeviceConnectionPhase
	{
		// Token: 0x04000114 RID: 276
		Idle,
		// Token: 0x04000115 RID: 277
		AcquiringCertificate,
		// Token: 0x04000116 RID: 278
		DeterminingConnectionRequirements,
		// Token: 0x04000117 RID: 279
		RequestingOperatingSystemInformation,
		// Token: 0x04000118 RID: 280
		ConnectingToTargetNetwork,
		// Token: 0x04000119 RID: 281
		UpdatingDeviceAddress
	}
}
