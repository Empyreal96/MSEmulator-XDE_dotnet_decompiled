using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200005C RID: 92
	public enum InvalidSettingsReason
	{
		// Token: 0x04000140 RID: 320
		NotInvalid,
		// Token: 0x04000141 RID: 321
		NicAttachedToDeletedSwitch,
		// Token: 0x04000142 RID: 322
		ExtraSwitchFound,
		// Token: 0x04000143 RID: 323
		TooManySwitchesFound,
		// Token: 0x04000144 RID: 324
		InvalidPortSettings,
		// Token: 0x04000145 RID: 325
		NicNotFoundForSwitch,
		// Token: 0x04000146 RID: 326
		FailedToLoadSettings,
		// Token: 0x04000147 RID: 327
		WrongLanguage,
		// Token: 0x04000148 RID: 328
		WrongResolution,
		// Token: 0x04000149 RID: 329
		SnapshotFailedToStart,
		// Token: 0x0400014A RID: 330
		SnapshotSettingsMismatchWithNewSettings,
		// Token: 0x0400014B RID: 331
		InvalidSettingsForInternalNic,
		// Token: 0x0400014C RID: 332
		InvalidNATSwitchFound
	}
}
