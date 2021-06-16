using System;

namespace System.Management.Automation.Remoting.Client
{
	// Token: 0x020003D9 RID: 985
	internal interface IWSManNativeApiFacade
	{
		// Token: 0x06002CEE RID: 11502
		int WSManPluginGetOperationParameters(IntPtr requestDetails, int flags, WSManNativeApi.WSManDataStruct data);

		// Token: 0x06002CEF RID: 11503
		int WSManPluginOperationComplete(IntPtr requestDetails, int flags, int errorCode, string extendedInformation);

		// Token: 0x06002CF0 RID: 11504
		int WSManPluginReceiveResult(IntPtr requestDetails, int flags, string stream, IntPtr streamResult, string commandState, int exitCode);

		// Token: 0x06002CF1 RID: 11505
		int WSManPluginReportContext(IntPtr requestDetails, int flags, IntPtr context);
	}
}
