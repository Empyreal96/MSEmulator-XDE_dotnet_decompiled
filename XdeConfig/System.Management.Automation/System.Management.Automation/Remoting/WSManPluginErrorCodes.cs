using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020003DC RID: 988
	internal enum WSManPluginErrorCodes
	{
		// Token: 0x0400179B RID: 6043
		NullPluginContext = -2141976624,
		// Token: 0x0400179C RID: 6044
		PluginContextNotFound,
		// Token: 0x0400179D RID: 6045
		NullInvalidInput = -2141975624,
		// Token: 0x0400179E RID: 6046
		NullInvalidStreamSets,
		// Token: 0x0400179F RID: 6047
		SessionCreationFailed,
		// Token: 0x040017A0 RID: 6048
		NullShellContext,
		// Token: 0x040017A1 RID: 6049
		InvalidShellContext,
		// Token: 0x040017A2 RID: 6050
		InvalidCommandContext,
		// Token: 0x040017A3 RID: 6051
		InvalidInputStream,
		// Token: 0x040017A4 RID: 6052
		InvalidInputDatatype,
		// Token: 0x040017A5 RID: 6053
		InvalidOutputStream,
		// Token: 0x040017A6 RID: 6054
		InvalidSenderDetails,
		// Token: 0x040017A7 RID: 6055
		ShutdownRegistrationFailed,
		// Token: 0x040017A8 RID: 6056
		ReportContextFailed,
		// Token: 0x040017A9 RID: 6057
		InvalidArgSet,
		// Token: 0x040017AA RID: 6058
		ProtocolVersionNotMatch,
		// Token: 0x040017AB RID: 6059
		OptionNotUnderstood,
		// Token: 0x040017AC RID: 6060
		ProtocolVersionNotFound,
		// Token: 0x040017AD RID: 6061
		ManagedException = -2141974624,
		// Token: 0x040017AE RID: 6062
		PluginOperationClose,
		// Token: 0x040017AF RID: 6063
		PluginConnectNoNegotiationData,
		// Token: 0x040017B0 RID: 6064
		PluginConnectOperationFailed,
		// Token: 0x040017B1 RID: 6065
		NoError = 0,
		// Token: 0x040017B2 RID: 6066
		OutOfMemory = -2147024882
	}
}
