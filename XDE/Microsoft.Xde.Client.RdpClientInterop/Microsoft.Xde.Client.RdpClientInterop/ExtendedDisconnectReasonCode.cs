using System;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200006C RID: 108
	public enum ExtendedDisconnectReasonCode
	{
		// Token: 0x04000015 RID: 21
		exDiscReasonNoInfo,
		// Token: 0x04000016 RID: 22
		exDiscReasonAPIInitiatedDisconnect,
		// Token: 0x04000017 RID: 23
		exDiscReasonAPIInitiatedLogoff,
		// Token: 0x04000018 RID: 24
		exDiscReasonServerIdleTimeout,
		// Token: 0x04000019 RID: 25
		exDiscReasonServerLogonTimeout,
		// Token: 0x0400001A RID: 26
		exDiscReasonReplacedByOtherConnection,
		// Token: 0x0400001B RID: 27
		exDiscReasonOutOfMemory,
		// Token: 0x0400001C RID: 28
		exDiscReasonServerDeniedConnection,
		// Token: 0x0400001D RID: 29
		exDiscReasonServerDeniedConnectionFips,
		// Token: 0x0400001E RID: 30
		exDiscReasonServerInsufficientPrivileges,
		// Token: 0x0400001F RID: 31
		exDiscReasonServerFreshCredsRequired,
		// Token: 0x04000020 RID: 32
		exDiscReasonLicenseInternal = 256,
		// Token: 0x04000021 RID: 33
		exDiscReasonLicenseNoLicenseServer,
		// Token: 0x04000022 RID: 34
		exDiscReasonLicenseNoLicense,
		// Token: 0x04000023 RID: 35
		exDiscReasonLicenseErrClientMsg,
		// Token: 0x04000024 RID: 36
		exDiscReasonLicenseHwidDoesntMatchLicense,
		// Token: 0x04000025 RID: 37
		exDiscReasonLicenseErrClientLicense,
		// Token: 0x04000026 RID: 38
		exDiscReasonLicenseCantFinishProtocol,
		// Token: 0x04000027 RID: 39
		exDiscReasonLicenseClientEndedProtocol,
		// Token: 0x04000028 RID: 40
		exDiscReasonLicenseErrClientEncryption,
		// Token: 0x04000029 RID: 41
		exDiscReasonLicenseCantUpgradeLicense,
		// Token: 0x0400002A RID: 42
		exDiscReasonLicenseNoRemoteConnections,
		// Token: 0x0400002B RID: 43
		exDiscReasonLicenseCreatingLicStoreAccDenied,
		// Token: 0x0400002C RID: 44
		exDiscReasonRdpEncInvalidCredentials = 768,
		// Token: 0x0400002D RID: 45
		exDiscReasonProtocolRangeStart = 4096,
		// Token: 0x0400002E RID: 46
		exDiscReasonProtocolRangeEnd = 32767
	}
}
