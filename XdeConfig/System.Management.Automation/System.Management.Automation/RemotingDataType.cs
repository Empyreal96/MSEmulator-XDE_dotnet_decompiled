using System;

namespace System.Management.Automation
{
	// Token: 0x020002DB RID: 731
	internal enum RemotingDataType : uint
	{
		// Token: 0x040010F9 RID: 4345
		InvalidDataType,
		// Token: 0x040010FA RID: 4346
		ExceptionAsErrorRecord,
		// Token: 0x040010FB RID: 4347
		SessionCapability = 65538U,
		// Token: 0x040010FC RID: 4348
		CloseSession,
		// Token: 0x040010FD RID: 4349
		CreateRunspacePool,
		// Token: 0x040010FE RID: 4350
		PublicKey,
		// Token: 0x040010FF RID: 4351
		EncryptedSessionKey,
		// Token: 0x04001100 RID: 4352
		PublicKeyRequest,
		// Token: 0x04001101 RID: 4353
		ConnectRunspacePool,
		// Token: 0x04001102 RID: 4354
		SetMaxRunspaces = 135170U,
		// Token: 0x04001103 RID: 4355
		SetMinRunspaces,
		// Token: 0x04001104 RID: 4356
		RunspacePoolOperationResponse,
		// Token: 0x04001105 RID: 4357
		RunspacePoolStateInfo,
		// Token: 0x04001106 RID: 4358
		CreatePowerShell,
		// Token: 0x04001107 RID: 4359
		AvailableRunspaces,
		// Token: 0x04001108 RID: 4360
		PSEventArgs,
		// Token: 0x04001109 RID: 4361
		ApplicationPrivateData,
		// Token: 0x0400110A RID: 4362
		GetCommandMetadata,
		// Token: 0x0400110B RID: 4363
		RunspacePoolInitData,
		// Token: 0x0400110C RID: 4364
		ResetRunspaceState,
		// Token: 0x0400110D RID: 4365
		RemoteHostCallUsingRunspaceHost = 135424U,
		// Token: 0x0400110E RID: 4366
		RemoteRunspaceHostResponseData,
		// Token: 0x0400110F RID: 4367
		PowerShellInput = 266242U,
		// Token: 0x04001110 RID: 4368
		PowerShellInputEnd,
		// Token: 0x04001111 RID: 4369
		PowerShellOutput,
		// Token: 0x04001112 RID: 4370
		PowerShellErrorRecord,
		// Token: 0x04001113 RID: 4371
		PowerShellStateInfo,
		// Token: 0x04001114 RID: 4372
		PowerShellDebug,
		// Token: 0x04001115 RID: 4373
		PowerShellVerbose,
		// Token: 0x04001116 RID: 4374
		PowerShellWarning,
		// Token: 0x04001117 RID: 4375
		PowerShellProgress = 266256U,
		// Token: 0x04001118 RID: 4376
		PowerShellInformationStream,
		// Token: 0x04001119 RID: 4377
		StopPowerShell,
		// Token: 0x0400111A RID: 4378
		RemoteHostCallUsingPowerShellHost = 266496U,
		// Token: 0x0400111B RID: 4379
		RemotePowerShellHostResponseData
	}
}
