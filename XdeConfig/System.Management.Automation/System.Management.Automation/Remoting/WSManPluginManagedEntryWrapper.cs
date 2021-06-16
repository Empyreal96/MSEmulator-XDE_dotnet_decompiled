using System;
using System.Management.Automation.Internal;
using System.Runtime.InteropServices;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020003EB RID: 1003
	public sealed class WSManPluginManagedEntryWrapper
	{
		// Token: 0x06002D4C RID: 11596 RVA: 0x000FB348 File Offset: 0x000F9548
		private WSManPluginManagedEntryWrapper()
		{
		}

		// Token: 0x06002D4D RID: 11597 RVA: 0x000FB350 File Offset: 0x000F9550
		public static int InitPlugin(IntPtr wkrPtrs)
		{
			if (IntPtr.Zero == wkrPtrs)
			{
				return 1;
			}
			ClrFacade.StructureToPtr<WSManPluginEntryDelegates.WSManPluginEntryDelegatesInternal>(WSManPluginManagedEntryWrapper.workerPtrs.UnmanagedStruct, wkrPtrs, false);
			return 0;
		}

		// Token: 0x06002D4E RID: 11598 RVA: 0x000FB373 File Offset: 0x000F9573
		public static void ShutdownPlugin(IntPtr pluginContext)
		{
			WSManPluginInstance.PerformShutdown(pluginContext);
			if (WSManPluginManagedEntryWrapper.workerPtrs != null)
			{
				WSManPluginManagedEntryWrapper.workerPtrs.Dispose();
			}
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x000FB38C File Offset: 0x000F958C
		public static void WSManPluginConnect(IntPtr pluginContext, IntPtr requestDetails, int flags, IntPtr shellContext, IntPtr commandContext, IntPtr inboundConnectInformation)
		{
			if (IntPtr.Zero == pluginContext)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.NullPluginContext, StringUtil.Format(RemotingErrorIdStrings.WSManPluginNullPluginContext, "pluginContext", "WSManPluginConnect"));
				return;
			}
			WSManPluginInstance.PerformWSManPluginConnect(pluginContext, requestDetails, flags, shellContext, commandContext, inboundConnectInformation);
		}

		// Token: 0x06002D50 RID: 11600 RVA: 0x000FB3C8 File Offset: 0x000F95C8
		public static void WSManPluginShell(IntPtr pluginContext, IntPtr requestDetails, int flags, [MarshalAs(UnmanagedType.LPWStr)] string extraInfo, IntPtr startupInfo, IntPtr inboundShellInformation)
		{
			if (IntPtr.Zero == pluginContext)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.NullPluginContext, StringUtil.Format(RemotingErrorIdStrings.WSManPluginNullPluginContext, "pluginContext", "WSManPluginShell"));
				return;
			}
			WSManPluginInstance.PerformWSManPluginShell(pluginContext, requestDetails, flags, extraInfo, startupInfo, inboundShellInformation);
		}

		// Token: 0x06002D51 RID: 11601 RVA: 0x000FB404 File Offset: 0x000F9604
		public static void WSManPluginReleaseShellContext(IntPtr pluginContext, IntPtr shellContext)
		{
		}

		// Token: 0x06002D52 RID: 11602 RVA: 0x000FB406 File Offset: 0x000F9606
		public static void WSManPluginCommand(IntPtr pluginContext, IntPtr requestDetails, int flags, IntPtr shellContext, [MarshalAs(UnmanagedType.LPWStr)] string commandLine, IntPtr arguments)
		{
			if (IntPtr.Zero == pluginContext)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.NullPluginContext, StringUtil.Format(RemotingErrorIdStrings.WSManPluginNullPluginContext, "Plugin Context", "WSManPluginCommand"));
				return;
			}
			WSManPluginInstance.PerformWSManPluginCommand(pluginContext, requestDetails, flags, shellContext, commandLine, arguments);
		}

		// Token: 0x06002D53 RID: 11603 RVA: 0x000FB442 File Offset: 0x000F9642
		public static void WSManPluginReleaseCommandContext(IntPtr pluginContext, IntPtr shellContext, IntPtr commandContext)
		{
		}

		// Token: 0x06002D54 RID: 11604 RVA: 0x000FB444 File Offset: 0x000F9644
		public static void WSManPluginSend(IntPtr pluginContext, IntPtr requestDetails, int flags, IntPtr shellContext, IntPtr commandContext, [MarshalAs(UnmanagedType.LPWStr)] string stream, IntPtr inboundData)
		{
			if (IntPtr.Zero == pluginContext)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.NullPluginContext, StringUtil.Format(RemotingErrorIdStrings.WSManPluginNullPluginContext, "Plugin Context", "WSManPluginSend"));
				return;
			}
			WSManPluginInstance.PerformWSManPluginSend(pluginContext, requestDetails, flags, shellContext, commandContext, stream, inboundData);
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x000FB482 File Offset: 0x000F9682
		public static void WSManPluginReceive(IntPtr pluginContext, IntPtr requestDetails, int flags, IntPtr shellContext, IntPtr commandContext, IntPtr streamSet)
		{
			if (IntPtr.Zero == pluginContext)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.NullPluginContext, StringUtil.Format(RemotingErrorIdStrings.WSManPluginNullPluginContext, "Plugin Context", "WSManPluginReceive"));
				return;
			}
			WSManPluginInstance.PerformWSManPluginReceive(pluginContext, requestDetails, flags, shellContext, commandContext, streamSet);
		}

		// Token: 0x06002D56 RID: 11606 RVA: 0x000FB4C0 File Offset: 0x000F96C0
		public static void WSManPluginSignal(IntPtr pluginContext, IntPtr requestDetails, int flags, IntPtr shellContext, IntPtr commandContext, [MarshalAs(UnmanagedType.LPWStr)] string code)
		{
			if (IntPtr.Zero == pluginContext || IntPtr.Zero == shellContext)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.NullPluginContext, StringUtil.Format(RemotingErrorIdStrings.WSManPluginNullPluginContext, "Plugin Context", "WSManPluginSignal"));
				return;
			}
			WSManPluginInstance.PerformWSManPluginSignal(pluginContext, requestDetails, flags, shellContext, commandContext, code);
		}

		// Token: 0x06002D57 RID: 11607 RVA: 0x000FB514 File Offset: 0x000F9714
		public static void PSPluginOperationShutdownCallback(object operationContext, bool timedOut)
		{
			if (operationContext == null)
			{
				return;
			}
			WSManPluginOperationShutdownContext wsmanPluginOperationShutdownContext = (WSManPluginOperationShutdownContext)operationContext;
			wsmanPluginOperationShutdownContext.isShuttingDown = true;
			WSManPluginInstance.PerformCloseOperation(wsmanPluginOperationShutdownContext);
		}

		// Token: 0x040017D0 RID: 6096
		private static WSManPluginEntryDelegates workerPtrs = new WSManPluginEntryDelegates();
	}
}
