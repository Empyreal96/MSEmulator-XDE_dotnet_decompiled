using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020003DB RID: 987
	internal static class WSManPluginConstants
	{
		// Token: 0x0400178F RID: 6031
		internal const int ExitCodeSuccess = 0;

		// Token: 0x04001790 RID: 6032
		internal const int ExitCodeFailure = 1;

		// Token: 0x04001791 RID: 6033
		internal const string CtrlCSignal = "powershell/signal/crtl_c";

		// Token: 0x04001792 RID: 6034
		internal const string SupportedInputStream = "stdin";

		// Token: 0x04001793 RID: 6035
		internal const string SupportedOutputStream = "stdout";

		// Token: 0x04001794 RID: 6036
		internal const string SupportedPromptResponseStream = "pr";

		// Token: 0x04001795 RID: 6037
		internal const string PowerShellStartupProtocolVersionName = "protocolversion";

		// Token: 0x04001796 RID: 6038
		internal const string PowerShellStartupProtocolVersionValue = "2.0";

		// Token: 0x04001797 RID: 6039
		internal const string PowerShellOptionPrefix = "PS_";

		// Token: 0x04001798 RID: 6040
		internal const int WSManPluginParamsGetRequestedLocale = 5;

		// Token: 0x04001799 RID: 6041
		internal const int WSManPluginParamsGetRequestedDataLocale = 6;
	}
}
