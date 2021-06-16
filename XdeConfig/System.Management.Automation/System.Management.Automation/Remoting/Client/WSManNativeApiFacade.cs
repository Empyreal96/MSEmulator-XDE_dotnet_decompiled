using System;

namespace System.Management.Automation.Remoting.Client
{
	// Token: 0x020003DA RID: 986
	internal class WSManNativeApiFacade : IWSManNativeApiFacade
	{
		// Token: 0x06002CF2 RID: 11506 RVA: 0x000F9B0B File Offset: 0x000F7D0B
		int IWSManNativeApiFacade.WSManPluginGetOperationParameters(IntPtr requestDetails, int flags, WSManNativeApi.WSManDataStruct data)
		{
			return WSManNativeApi.WSManPluginGetOperationParameters(requestDetails, flags, data);
		}

		// Token: 0x06002CF3 RID: 11507 RVA: 0x000F9B15 File Offset: 0x000F7D15
		int IWSManNativeApiFacade.WSManPluginOperationComplete(IntPtr requestDetails, int flags, int errorCode, string extendedInformation)
		{
			return WSManNativeApi.WSManPluginOperationComplete(requestDetails, flags, errorCode, extendedInformation);
		}

		// Token: 0x06002CF4 RID: 11508 RVA: 0x000F9B21 File Offset: 0x000F7D21
		int IWSManNativeApiFacade.WSManPluginReceiveResult(IntPtr requestDetails, int flags, string stream, IntPtr streamResult, string commandState, int exitCode)
		{
			return WSManNativeApi.WSManPluginReceiveResult(requestDetails, flags, stream, streamResult, commandState, exitCode);
		}

		// Token: 0x06002CF5 RID: 11509 RVA: 0x000F9B31 File Offset: 0x000F7D31
		int IWSManNativeApiFacade.WSManPluginReportContext(IntPtr requestDetails, int flags, IntPtr context)
		{
			return WSManNativeApi.WSManPluginReportContext(requestDetails, flags, context);
		}
	}
}
