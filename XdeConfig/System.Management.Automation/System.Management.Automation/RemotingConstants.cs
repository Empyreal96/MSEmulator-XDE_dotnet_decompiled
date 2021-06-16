using System;

namespace System.Management.Automation
{
	// Token: 0x020002D7 RID: 727
	internal static class RemotingConstants
	{
		// Token: 0x04001086 RID: 4230
		internal const string PSSessionConfigurationNoun = "PSSessionConfiguration";

		// Token: 0x04001087 RID: 4231
		internal const string PSRemotingNoun = "PSRemoting";

		// Token: 0x04001088 RID: 4232
		internal const string PSPluginDLLName = "pwrshplugin.dll";

		// Token: 0x04001089 RID: 4233
		internal const string DefaultShellName = "Microsoft.PowerShell";

		// Token: 0x0400108A RID: 4234
		internal const string MaxIdleTimeoutMS = "2147483647";

		// Token: 0x0400108B RID: 4235
		internal static readonly Version HostVersion = new Version(1, 0, 0, 0);

		// Token: 0x0400108C RID: 4236
		internal static readonly Version ProtocolVersionWin7RC = new Version(2, 0);

		// Token: 0x0400108D RID: 4237
		internal static readonly Version ProtocolVersionWin7RTM = new Version(2, 1);

		// Token: 0x0400108E RID: 4238
		internal static readonly Version ProtocolVersionWin8RTM = new Version(2, 2);

		// Token: 0x0400108F RID: 4239
		internal static readonly Version ProtocolVersionWin10RTM = new Version(2, 3);

		// Token: 0x04001090 RID: 4240
		internal static readonly Version ProtocolVersionCurrent = new Version(2, 3);

		// Token: 0x04001091 RID: 4241
		internal static readonly Version ProtocolVersion = RemotingConstants.ProtocolVersionCurrent;

		// Token: 0x04001092 RID: 4242
		internal static readonly string ComputerNameNoteProperty = "PSComputerName";

		// Token: 0x04001093 RID: 4243
		internal static readonly string RunspaceIdNoteProperty = "RunspaceId";

		// Token: 0x04001094 RID: 4244
		internal static readonly string ShowComputerNameNoteProperty = "PSShowComputerName";

		// Token: 0x04001095 RID: 4245
		internal static readonly string SourceJobInstanceId = "PSSourceJobInstanceId";

		// Token: 0x04001096 RID: 4246
		internal static readonly string SourceLength = "Length";

		// Token: 0x04001097 RID: 4247
		internal static readonly string EventObject = "PSEventObject";
	}
}
