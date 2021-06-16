using System;

namespace System.Management.Automation.Internal
{
	// Token: 0x0200025B RID: 603
	internal enum PSEventId
	{
		// Token: 0x04000BE2 RID: 3042
		HostNameResolve = 4097,
		// Token: 0x04000BE3 RID: 3043
		SchemeResolve,
		// Token: 0x04000BE4 RID: 3044
		ShellResolve,
		// Token: 0x04000BE5 RID: 3045
		RunspaceConstructor = 8193,
		// Token: 0x04000BE6 RID: 3046
		RunspacePoolConstructor,
		// Token: 0x04000BE7 RID: 3047
		RunspacePoolOpen,
		// Token: 0x04000BE8 RID: 3048
		OperationalTransferEventRunspacePool,
		// Token: 0x04000BE9 RID: 3049
		RunspaceStateChange,
		// Token: 0x04000BEA RID: 3050
		RetrySessionCreation,
		// Token: 0x04000BEB RID: 3051
		Port = 12033,
		// Token: 0x04000BEC RID: 3052
		AppName,
		// Token: 0x04000BED RID: 3053
		ComputerName,
		// Token: 0x04000BEE RID: 3054
		Scheme,
		// Token: 0x04000BEF RID: 3055
		TestAnalytic,
		// Token: 0x04000BF0 RID: 3056
		WSManConnectionInfoDump,
		// Token: 0x04000BF1 RID: 3057
		AnalyticTransferEventRunspacePool,
		// Token: 0x04000BF2 RID: 3058
		TransportReceivedObject = 32769,
		// Token: 0x04000BF3 RID: 3059
		TransportSendingData,
		// Token: 0x04000BF4 RID: 3060
		TransportReceivedData,
		// Token: 0x04000BF5 RID: 3061
		AppDomainUnhandledException_Analytic = 32775,
		// Token: 0x04000BF6 RID: 3062
		TransportError_Analytic,
		// Token: 0x04000BF7 RID: 3063
		AppDomainUnhandledException,
		// Token: 0x04000BF8 RID: 3064
		TransportError = 32784,
		// Token: 0x04000BF9 RID: 3065
		WSManCreateShell,
		// Token: 0x04000BFA RID: 3066
		WSManCreateShellCallbackReceived,
		// Token: 0x04000BFB RID: 3067
		WSManCloseShell,
		// Token: 0x04000BFC RID: 3068
		WSManCloseShellCallbackReceived,
		// Token: 0x04000BFD RID: 3069
		WSManSendShellInputEx,
		// Token: 0x04000BFE RID: 3070
		WSManSendShellInputExCallbackReceived,
		// Token: 0x04000BFF RID: 3071
		WSManReceiveShellOutputEx,
		// Token: 0x04000C00 RID: 3072
		WSManReceiveShellOutputExCallbackReceived,
		// Token: 0x04000C01 RID: 3073
		WSManCreateCommand,
		// Token: 0x04000C02 RID: 3074
		WSManCreateCommandCallbackReceived = 32800,
		// Token: 0x04000C03 RID: 3075
		WSManCloseCommand,
		// Token: 0x04000C04 RID: 3076
		WSManCloseCommandCallbackReceived,
		// Token: 0x04000C05 RID: 3077
		WSManSignal,
		// Token: 0x04000C06 RID: 3078
		WSManSignalCallbackReceived,
		// Token: 0x04000C07 RID: 3079
		URIRedirection,
		// Token: 0x04000C08 RID: 3080
		ServerSendData = 32849,
		// Token: 0x04000C09 RID: 3081
		ServerCreateRemoteSession,
		// Token: 0x04000C0A RID: 3082
		ReportContext,
		// Token: 0x04000C0B RID: 3083
		ReportOperationComplete,
		// Token: 0x04000C0C RID: 3084
		ServerCreateCommandSession,
		// Token: 0x04000C0D RID: 3085
		ServerStopCommand,
		// Token: 0x04000C0E RID: 3086
		ServerReceivedData,
		// Token: 0x04000C0F RID: 3087
		ServerClientReceiveRequest,
		// Token: 0x04000C10 RID: 3088
		ServerCloseOperation,
		// Token: 0x04000C11 RID: 3089
		LoadingPSCustomShellAssembly = 32865,
		// Token: 0x04000C12 RID: 3090
		LoadingPSCustomShellType,
		// Token: 0x04000C13 RID: 3091
		ReceivedRemotingFragment,
		// Token: 0x04000C14 RID: 3092
		SentRemotingFragment,
		// Token: 0x04000C15 RID: 3093
		WSManPluginShutdown,
		// Token: 0x04000C16 RID: 3094
		Serializer_RehydrationSuccess = 28673,
		// Token: 0x04000C17 RID: 3095
		Serializer_RehydrationFailure,
		// Token: 0x04000C18 RID: 3096
		Serializer_DepthOverride,
		// Token: 0x04000C19 RID: 3097
		Serializer_ModeOverride,
		// Token: 0x04000C1A RID: 3098
		Serializer_ScriptPropertyWithoutRunspace,
		// Token: 0x04000C1B RID: 3099
		Serializer_PropertyGetterFailed,
		// Token: 0x04000C1C RID: 3100
		Serializer_EnumerationFailed,
		// Token: 0x04000C1D RID: 3101
		Serializer_ToStringFailed,
		// Token: 0x04000C1E RID: 3102
		Serializer_MaxDepthWhenSerializing = 28682,
		// Token: 0x04000C1F RID: 3103
		Serializer_XmlExceptionWhenDeserializing,
		// Token: 0x04000C20 RID: 3104
		Serializer_SpecificPropertyMissing,
		// Token: 0x04000C21 RID: 3105
		Perftrack_ConsoleStartupStart = 40961,
		// Token: 0x04000C22 RID: 3106
		Perftrack_ConsoleStartupStop,
		// Token: 0x04000C23 RID: 3107
		Command_Health = 4100,
		// Token: 0x04000C24 RID: 3108
		Engine_Health,
		// Token: 0x04000C25 RID: 3109
		Provider_Health,
		// Token: 0x04000C26 RID: 3110
		Pipeline_Detail,
		// Token: 0x04000C27 RID: 3111
		ScriptBlock_Compile_Detail,
		// Token: 0x04000C28 RID: 3112
		ScriptBlock_Invoke_Start_Detail,
		// Token: 0x04000C29 RID: 3113
		ScriptBlock_Invoke_Complete_Detail,
		// Token: 0x04000C2A RID: 3114
		Command_Lifecycle = 7937,
		// Token: 0x04000C2B RID: 3115
		Engine_Lifecycle,
		// Token: 0x04000C2C RID: 3116
		Provider_Lifecycle,
		// Token: 0x04000C2D RID: 3117
		Settings,
		// Token: 0x04000C2E RID: 3118
		Engine_Trace = 7942,
		// Token: 0x04000C2F RID: 3119
		ScheduledJob_Start = 53249,
		// Token: 0x04000C30 RID: 3120
		ScheduledJob_Complete,
		// Token: 0x04000C31 RID: 3121
		ScheduledJob_Error,
		// Token: 0x04000C32 RID: 3122
		NamedPipeIPC_ServerListenerStarted = 53504,
		// Token: 0x04000C33 RID: 3123
		NamedPipeIPC_ServerListenerEnded,
		// Token: 0x04000C34 RID: 3124
		NamedPipeIPC_ServerListenerError,
		// Token: 0x04000C35 RID: 3125
		NamedPipeIPC_ServerConnect,
		// Token: 0x04000C36 RID: 3126
		NamedPipeIPC_ServerDisconnect,
		// Token: 0x04000C37 RID: 3127
		ISEExecuteScript = 24577,
		// Token: 0x04000C38 RID: 3128
		ISEExecuteSelection,
		// Token: 0x04000C39 RID: 3129
		ISEStopCommand,
		// Token: 0x04000C3A RID: 3130
		ISEResumeDebugger,
		// Token: 0x04000C3B RID: 3131
		ISEStopDebugger,
		// Token: 0x04000C3C RID: 3132
		ISEDebuggerStepInto,
		// Token: 0x04000C3D RID: 3133
		ISEDebuggerStepOver,
		// Token: 0x04000C3E RID: 3134
		ISEDebuggerStepOut,
		// Token: 0x04000C3F RID: 3135
		ISEEnableAllBreakpoints = 24592,
		// Token: 0x04000C40 RID: 3136
		ISEDisableAllBreakpoints,
		// Token: 0x04000C41 RID: 3137
		ISERemoveAllBreakpoints,
		// Token: 0x04000C42 RID: 3138
		ISESetBreakpoint,
		// Token: 0x04000C43 RID: 3139
		ISERemoveBreakpoint,
		// Token: 0x04000C44 RID: 3140
		ISEEnableBreakpoint,
		// Token: 0x04000C45 RID: 3141
		ISEDisableBreakpoint,
		// Token: 0x04000C46 RID: 3142
		ISEHitBreakpoint
	}
}
