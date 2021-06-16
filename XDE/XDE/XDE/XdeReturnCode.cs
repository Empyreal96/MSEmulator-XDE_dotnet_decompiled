using System;

namespace Microsoft.Xde.Client
{
	// Token: 0x0200002D RID: 45
	public enum XdeReturnCode
	{
		// Token: 0x0400010F RID: 271
		Success,
		// Token: 0x04000110 RID: 272
		GenericError,
		// Token: 0x04000111 RID: 273
		ShowUsage,
		// Token: 0x04000112 RID: 274
		InvalidArguments,
		// Token: 0x04000113 RID: 275
		InstanceAlreadyRunning,
		// Token: 0x04000114 RID: 276
		WrongWindows,
		// Token: 0x04000115 RID: 277
		CantAccessHyperVApi,
		// Token: 0x04000116 RID: 278
		FailedToConnect,
		// Token: 0x04000117 RID: 279
		CouldntInitNetworkConfig,
		// Token: 0x04000118 RID: 280
		CouldntFindOrCreateVm,
		// Token: 0x04000119 RID: 281
		CouldntStartVm,
		// Token: 0x0400011A RID: 282
		CouldntLoadSettings,
		// Token: 0x0400011B RID: 283
		HyperVNotInstalled,
		// Token: 0x0400011C RID: 284
		HypervisorNotRunning,
		// Token: 0x0400011D RID: 285
		HyperVManagementServiceNotRunning,
		// Token: 0x0400011E RID: 286
		CouldntLoadSkin,
		// Token: 0x0400011F RID: 287
		CouldntAquireNetworkModifyPermissions,
		// Token: 0x04000120 RID: 288
		NotEnoughVramForResolution,
		// Token: 0x04000121 RID: 289
		HostSettingsModified,
		// Token: 0x04000122 RID: 290
		CouldntMountVhd,
		// Token: 0x04000123 RID: 291
		EulaNotAccepted,
		// Token: 0x04000124 RID: 292
		FailedToWriteBootSettings
	}
}
