using System;
using System.CodeDom.Compiler;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x0200011E RID: 286
	[GeneratedCode("MarsComp", "")]
	public enum VPMEMHostInjectedError
	{
		// Token: 0x040005B1 RID: 1457
		None,
		// Token: 0x040005B2 RID: 1458
		DataPersistenceLoss,
		// Token: 0x040005B3 RID: 1459
		WritePersistenceLoss,
		// Token: 0x040005B4 RID: 1460
		HardwareFailure,
		// Token: 0x040005B5 RID: 1461
		DataPersistenceLossImminent,
		// Token: 0x040005B6 RID: 1462
		WritePersistenceLossImminent,
		// Token: 0x040005B7 RID: 1463
		HardwareFailureImminent
	}
}
