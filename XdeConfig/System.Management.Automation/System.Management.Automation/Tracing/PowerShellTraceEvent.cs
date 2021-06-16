using System;

namespace System.Management.Automation.Tracing
{
	// Token: 0x020008D8 RID: 2264
	public enum PowerShellTraceEvent
	{
		// Token: 0x04002CC8 RID: 11464
		None,
		// Token: 0x04002CC9 RID: 11465
		HostNameResolve = 4097,
		// Token: 0x04002CCA RID: 11466
		SchemeResolve,
		// Token: 0x04002CCB RID: 11467
		ShellResolve,
		// Token: 0x04002CCC RID: 11468
		RunspaceConstructor = 8193,
		// Token: 0x04002CCD RID: 11469
		RunspacePoolConstructor,
		// Token: 0x04002CCE RID: 11470
		RunspacePoolOpen,
		// Token: 0x04002CCF RID: 11471
		OperationalTransferEventRunspacePool,
		// Token: 0x04002CD0 RID: 11472
		RunspacePort = 12033,
		// Token: 0x04002CD1 RID: 11473
		AppName,
		// Token: 0x04002CD2 RID: 11474
		ComputerName,
		// Token: 0x04002CD3 RID: 11475
		Scheme,
		// Token: 0x04002CD4 RID: 11476
		TestAnalytic,
		// Token: 0x04002CD5 RID: 11477
		WSManConnectionInfoDump,
		// Token: 0x04002CD6 RID: 11478
		AnalyticTransferEventRunspacePool,
		// Token: 0x04002CD7 RID: 11479
		TransportReceivedObject = 32769,
		// Token: 0x04002CD8 RID: 11480
		AppDomainUnhandledExceptionAnalytic = 32775,
		// Token: 0x04002CD9 RID: 11481
		TransportErrorAnalytic,
		// Token: 0x04002CDA RID: 11482
		AppDomainUnhandledException,
		// Token: 0x04002CDB RID: 11483
		TransportError = 32784,
		// Token: 0x04002CDC RID: 11484
		WSManCreateShell,
		// Token: 0x04002CDD RID: 11485
		WSManCreateShellCallbackReceived,
		// Token: 0x04002CDE RID: 11486
		WSManCloseShell,
		// Token: 0x04002CDF RID: 11487
		WSManCloseShellCallbackReceived,
		// Token: 0x04002CE0 RID: 11488
		WSManSendShellInputExtended,
		// Token: 0x04002CE1 RID: 11489
		WSManSendShellInputExtendedCallbackReceived,
		// Token: 0x04002CE2 RID: 11490
		WSManReceiveShellOutputExtended,
		// Token: 0x04002CE3 RID: 11491
		WSManReceiveShellOutputExtendedCallbackReceived,
		// Token: 0x04002CE4 RID: 11492
		WSManCreateCommand,
		// Token: 0x04002CE5 RID: 11493
		WSManCreateCommandCallbackReceived = 32800,
		// Token: 0x04002CE6 RID: 11494
		WSManCloseCommand,
		// Token: 0x04002CE7 RID: 11495
		WSManCloseCommandCallbackReceived,
		// Token: 0x04002CE8 RID: 11496
		WSManSignal,
		// Token: 0x04002CE9 RID: 11497
		WSManSignalCallbackReceived,
		// Token: 0x04002CEA RID: 11498
		UriRedirection,
		// Token: 0x04002CEB RID: 11499
		ServerSendData = 32849,
		// Token: 0x04002CEC RID: 11500
		ServerCreateRemoteSession,
		// Token: 0x04002CED RID: 11501
		ReportContext,
		// Token: 0x04002CEE RID: 11502
		ReportOperationComplete,
		// Token: 0x04002CEF RID: 11503
		ServerCreateCommandSession,
		// Token: 0x04002CF0 RID: 11504
		ServerStopCommand,
		// Token: 0x04002CF1 RID: 11505
		ServerReceivedData,
		// Token: 0x04002CF2 RID: 11506
		ServerClientReceiveRequest,
		// Token: 0x04002CF3 RID: 11507
		ServerCloseOperation,
		// Token: 0x04002CF4 RID: 11508
		LoadingPSCustomShellAssembly = 32865,
		// Token: 0x04002CF5 RID: 11509
		LoadingPSCustomShellType,
		// Token: 0x04002CF6 RID: 11510
		ReceivedRemotingFragment,
		// Token: 0x04002CF7 RID: 11511
		SentRemotingFragment,
		// Token: 0x04002CF8 RID: 11512
		WSManPluginShutdown,
		// Token: 0x04002CF9 RID: 11513
		SerializerWorkflowLoadSuccess = 28673,
		// Token: 0x04002CFA RID: 11514
		SerializerWorkflowLoadFailure,
		// Token: 0x04002CFB RID: 11515
		SerializerDepthOverride,
		// Token: 0x04002CFC RID: 11516
		SerializerModeOverride,
		// Token: 0x04002CFD RID: 11517
		SerializerScriptPropertyWithoutRunspace,
		// Token: 0x04002CFE RID: 11518
		SerializerPropertyGetterFailed,
		// Token: 0x04002CFF RID: 11519
		SerializerEnumerationFailed,
		// Token: 0x04002D00 RID: 11520
		SerializerToStringFailed,
		// Token: 0x04002D01 RID: 11521
		SerializerMaxDepthWhenSerializing = 28682,
		// Token: 0x04002D02 RID: 11522
		SerializerXmlExceptionWhenDeserializing,
		// Token: 0x04002D03 RID: 11523
		SerializerSpecificPropertyMissing,
		// Token: 0x04002D04 RID: 11524
		PerformanceTrackConsoleStartupStart = 40961,
		// Token: 0x04002D05 RID: 11525
		PerformanceTrackConsoleStartupStop,
		// Token: 0x04002D06 RID: 11526
		ErrorRecord = 45057,
		// Token: 0x04002D07 RID: 11527
		Exception,
		// Token: 0x04002D08 RID: 11528
		PowerShellObject,
		// Token: 0x04002D09 RID: 11529
		Job,
		// Token: 0x04002D0A RID: 11530
		TraceMessage,
		// Token: 0x04002D0B RID: 11531
		TraceWSManConnectionInfo,
		// Token: 0x04002D0C RID: 11532
		TraceMessage2 = 49153,
		// Token: 0x04002D0D RID: 11533
		TraceMessageGuid
	}
}
