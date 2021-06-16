using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002BF RID: 703
	internal enum PSRemotingErrorId : uint
	{
		// Token: 0x04000F2E RID: 3886
		DefaultRemotingExceptionMessage,
		// Token: 0x04000F2F RID: 3887
		OutOfMemory,
		// Token: 0x04000F30 RID: 3888
		PipelineIdsDoNotMatch = 10U,
		// Token: 0x04000F31 RID: 3889
		PipelineNotFoundOnServer,
		// Token: 0x04000F32 RID: 3890
		PipelineStopped,
		// Token: 0x04000F33 RID: 3891
		RunspaceAlreadyExists = 200U,
		// Token: 0x04000F34 RID: 3892
		RunspaceIdsDoNotMatch,
		// Token: 0x04000F35 RID: 3893
		RemoteRunspaceOpenFailed,
		// Token: 0x04000F36 RID: 3894
		RunspaceCannotBeFound,
		// Token: 0x04000F37 RID: 3895
		ResponsePromptIdCannotBeFound,
		// Token: 0x04000F38 RID: 3896
		RemoteHostCallFailed,
		// Token: 0x04000F39 RID: 3897
		RemoteHostMethodNotImplemented,
		// Token: 0x04000F3A RID: 3898
		RemoteHostDataEncodingNotSupported,
		// Token: 0x04000F3B RID: 3899
		RemoteHostDataDecodingNotSupported,
		// Token: 0x04000F3C RID: 3900
		NestedPipelineNotSupported,
		// Token: 0x04000F3D RID: 3901
		RelativeUriForRunspacePathNotSupported,
		// Token: 0x04000F3E RID: 3902
		RemoteHostDecodingFailed,
		// Token: 0x04000F3F RID: 3903
		MustBeAdminToOverrideThreadOptions,
		// Token: 0x04000F40 RID: 3904
		RemoteHostPromptForCredentialModifiedCaption,
		// Token: 0x04000F41 RID: 3905
		RemoteHostPromptForCredentialModifiedMessage,
		// Token: 0x04000F42 RID: 3906
		RemoteHostReadLineAsSecureStringPrompt,
		// Token: 0x04000F43 RID: 3907
		RemoteHostGetBufferContents,
		// Token: 0x04000F44 RID: 3908
		RemoteHostPromptSecureStringPrompt,
		// Token: 0x04000F45 RID: 3909
		WinPERemotingNotSupported,
		// Token: 0x04000F46 RID: 3910
		ReceivedUnsupportedRemoteHostCall = 400U,
		// Token: 0x04000F47 RID: 3911
		ReceivedUnsupportedAction,
		// Token: 0x04000F48 RID: 3912
		ReceivedUnsupportedDataType,
		// Token: 0x04000F49 RID: 3913
		MissingDestination,
		// Token: 0x04000F4A RID: 3914
		MissingTarget,
		// Token: 0x04000F4B RID: 3915
		MissingRunspaceId,
		// Token: 0x04000F4C RID: 3916
		MissingDataType,
		// Token: 0x04000F4D RID: 3917
		MissingCallId,
		// Token: 0x04000F4E RID: 3918
		MissingMethodName,
		// Token: 0x04000F4F RID: 3919
		MissingIsStartFragment,
		// Token: 0x04000F50 RID: 3920
		MissingProperty,
		// Token: 0x04000F51 RID: 3921
		ObjectIdsNotMatching,
		// Token: 0x04000F52 RID: 3922
		FragmetIdsNotInSequence,
		// Token: 0x04000F53 RID: 3923
		ObjectIsTooBig,
		// Token: 0x04000F54 RID: 3924
		MissingIsEndFragment,
		// Token: 0x04000F55 RID: 3925
		DeserializedObjectIsNull,
		// Token: 0x04000F56 RID: 3926
		BlobLengthNotInRange,
		// Token: 0x04000F57 RID: 3927
		DecodingErrorForErrorRecord,
		// Token: 0x04000F58 RID: 3928
		DecodingErrorForPipelineStateInfo,
		// Token: 0x04000F59 RID: 3929
		DecodingErrorForRunspaceStateInfo,
		// Token: 0x04000F5A RID: 3930
		ReceivedUnsupportedRemotingTargetInterfaceType,
		// Token: 0x04000F5B RID: 3931
		UnknownTargetClass,
		// Token: 0x04000F5C RID: 3932
		MissingTargetClass,
		// Token: 0x04000F5D RID: 3933
		DecodingErrorForRunspacePoolStateInfo,
		// Token: 0x04000F5E RID: 3934
		DecodingErrorForMinRunspaces,
		// Token: 0x04000F5F RID: 3935
		DecodingErrorForMaxRunspaces,
		// Token: 0x04000F60 RID: 3936
		DecodingErrorForPowerShellStateInfo,
		// Token: 0x04000F61 RID: 3937
		DecodingErrorForThreadOptions,
		// Token: 0x04000F62 RID: 3938
		CantCastPropertyToExpectedType,
		// Token: 0x04000F63 RID: 3939
		CantCastRemotingDataToPSObject,
		// Token: 0x04000F64 RID: 3940
		CantCastCommandToPSObject,
		// Token: 0x04000F65 RID: 3941
		CantCastParameterToPSObject,
		// Token: 0x04000F66 RID: 3942
		ObjectIdCannotBeLessThanZero,
		// Token: 0x04000F67 RID: 3943
		NotEnoughHeaderForRemoteDataObject,
		// Token: 0x04000F68 RID: 3944
		RemotingDestinationNotForMe = 600U,
		// Token: 0x04000F69 RID: 3945
		ClientNegotiationTimeout,
		// Token: 0x04000F6A RID: 3946
		ClientNegotiationFailed,
		// Token: 0x04000F6B RID: 3947
		ServerRequestedToCloseSession,
		// Token: 0x04000F6C RID: 3948
		ServerNegotiationFailed,
		// Token: 0x04000F6D RID: 3949
		ServerNegotiationTimeout,
		// Token: 0x04000F6E RID: 3950
		ClientRequestedToCloseSession,
		// Token: 0x04000F6F RID: 3951
		FatalErrorCausingClose,
		// Token: 0x04000F70 RID: 3952
		ClientKeyExchangeFailed,
		// Token: 0x04000F71 RID: 3953
		ServerKeyExchangeFailed,
		// Token: 0x04000F72 RID: 3954
		ClientNotFoundCapabilityProperties,
		// Token: 0x04000F73 RID: 3955
		ServerNotFoundCapabilityProperties,
		// Token: 0x04000F74 RID: 3956
		ConnectFailed = 801U,
		// Token: 0x04000F75 RID: 3957
		CloseIsCalled,
		// Token: 0x04000F76 RID: 3958
		ForceClosed,
		// Token: 0x04000F77 RID: 3959
		CloseFailed,
		// Token: 0x04000F78 RID: 3960
		CloseCompleted,
		// Token: 0x04000F79 RID: 3961
		UnsupportedWaitHandleType,
		// Token: 0x04000F7A RID: 3962
		ReceivedDataStreamIsNotStdout,
		// Token: 0x04000F7B RID: 3963
		StdInIsNotOpen,
		// Token: 0x04000F7C RID: 3964
		NativeWriteFileFailed,
		// Token: 0x04000F7D RID: 3965
		NativeReadFileFailed,
		// Token: 0x04000F7E RID: 3966
		InvalidSchemeValue,
		// Token: 0x04000F7F RID: 3967
		ClientReceiveFailed,
		// Token: 0x04000F80 RID: 3968
		ClientSendFailed,
		// Token: 0x04000F81 RID: 3969
		CommandHandleIsNull,
		// Token: 0x04000F82 RID: 3970
		StdInCannotBeSetToNoWait,
		// Token: 0x04000F83 RID: 3971
		PortIsOutOfRange,
		// Token: 0x04000F84 RID: 3972
		ServerProcessExited,
		// Token: 0x04000F85 RID: 3973
		CannotGetStdInHandle,
		// Token: 0x04000F86 RID: 3974
		CannotGetStdOutHandle,
		// Token: 0x04000F87 RID: 3975
		CannotGetStdErrHandle,
		// Token: 0x04000F88 RID: 3976
		CannotSetStdInHandle,
		// Token: 0x04000F89 RID: 3977
		CannotSetStdOutHandle,
		// Token: 0x04000F8A RID: 3978
		CannotSetStdErrHandle,
		// Token: 0x04000F8B RID: 3979
		InvalidConfigurationName,
		// Token: 0x04000F8C RID: 3980
		CreateSessionFailed = 851U,
		// Token: 0x04000F8D RID: 3981
		CreateExFailed = 853U,
		// Token: 0x04000F8E RID: 3982
		ConnectExCallBackError,
		// Token: 0x04000F8F RID: 3983
		SendExFailed,
		// Token: 0x04000F90 RID: 3984
		SendExCallBackError,
		// Token: 0x04000F91 RID: 3985
		ReceiveExFailed,
		// Token: 0x04000F92 RID: 3986
		ReceiveExCallBackError,
		// Token: 0x04000F93 RID: 3987
		RunShellCommandExFailed,
		// Token: 0x04000F94 RID: 3988
		RunShellCommandExCallBackError,
		// Token: 0x04000F95 RID: 3989
		CommandSendExFailed,
		// Token: 0x04000F96 RID: 3990
		CommandSendExCallBackError,
		// Token: 0x04000F97 RID: 3991
		CommandReceiveExFailed,
		// Token: 0x04000F98 RID: 3992
		CommandReceiveExCallBackError,
		// Token: 0x04000F99 RID: 3993
		CloseExCallBackError = 866U,
		// Token: 0x04000F9A RID: 3994
		RedirectedURINotWellFormatted,
		// Token: 0x04000F9B RID: 3995
		URIEndPointNotResolved,
		// Token: 0x04000F9C RID: 3996
		ReceivedObjectSizeExceededMaximumClient,
		// Token: 0x04000F9D RID: 3997
		ReceivedDataSizeExceededMaximumClient,
		// Token: 0x04000F9E RID: 3998
		ReceivedObjectSizeExceededMaximumServer,
		// Token: 0x04000F9F RID: 3999
		ReceivedDataSizeExceededMaximumServer,
		// Token: 0x04000FA0 RID: 4000
		StartupScriptThrewTerminatingError,
		// Token: 0x04000FA1 RID: 4001
		TroubleShootingHelpTopic,
		// Token: 0x04000FA2 RID: 4002
		DisconnectShellExFailed,
		// Token: 0x04000FA3 RID: 4003
		DisconnectShellExCallBackErrr,
		// Token: 0x04000FA4 RID: 4004
		ReconnectShellExFailed,
		// Token: 0x04000FA5 RID: 4005
		ReconnectShellExCallBackErrr,
		// Token: 0x04000FA6 RID: 4006
		RemoteRunspaceInfoHasDuplicates = 900U,
		// Token: 0x04000FA7 RID: 4007
		RemoteRunspaceInfoLimitExceeded,
		// Token: 0x04000FA8 RID: 4008
		RemoteRunspaceOpenUnknownState,
		// Token: 0x04000FA9 RID: 4009
		UriSpecifiedNotValid,
		// Token: 0x04000FAA RID: 4010
		RemoteRunspaceClosed,
		// Token: 0x04000FAB RID: 4011
		RemoteRunspaceNotAvailableForSpecifiedComputer,
		// Token: 0x04000FAC RID: 4012
		RemoteRunspaceNotAvailableForSpecifiedRunspaceId,
		// Token: 0x04000FAD RID: 4013
		StopPSJobWhatIfTarget,
		// Token: 0x04000FAE RID: 4014
		InvalidJobStateGeneral = 909U,
		// Token: 0x04000FAF RID: 4015
		JobWithSpecifiedNameNotFound,
		// Token: 0x04000FB0 RID: 4016
		JobWithSpecifiedInstanceIdNotFound,
		// Token: 0x04000FB1 RID: 4017
		JobWithSpecifiedSessionIdNotFound,
		// Token: 0x04000FB2 RID: 4018
		JobWithSpecifiedNameNotCompleted,
		// Token: 0x04000FB3 RID: 4019
		JobWithSpecifiedSessionIdNotCompleted,
		// Token: 0x04000FB4 RID: 4020
		JobWithSpecifiedInstanceIdNotCompleted,
		// Token: 0x04000FB5 RID: 4021
		RemovePSJobWhatIfTarget,
		// Token: 0x04000FB6 RID: 4022
		ComputerNameParamNotSupported,
		// Token: 0x04000FB7 RID: 4023
		RunspaceParamNotSupported,
		// Token: 0x04000FB8 RID: 4024
		RemoteRunspaceNotAvailableForSpecifiedName,
		// Token: 0x04000FB9 RID: 4025
		RemoteRunspaceNotAvailableForSpecifiedSessionId,
		// Token: 0x04000FBA RID: 4026
		ItemNotFoundInRepository,
		// Token: 0x04000FBB RID: 4027
		CannotRemoveJob,
		// Token: 0x04000FBC RID: 4028
		NewRunspaceAmbiguosAuthentication,
		// Token: 0x04000FBD RID: 4029
		WildCardErrorFilePathParameter,
		// Token: 0x04000FBE RID: 4030
		FilePathNotFromFileSystemProvider,
		// Token: 0x04000FBF RID: 4031
		FilePathShouldPS1Extension,
		// Token: 0x04000FC0 RID: 4032
		PSSessionConfigurationName,
		// Token: 0x04000FC1 RID: 4033
		PSSessionAppName,
		// Token: 0x04000FC2 RID: 4034
		CSCDoubleParameterOutOfRange,
		// Token: 0x04000FC3 RID: 4035
		URIRedirectionReported,
		// Token: 0x04000FC4 RID: 4036
		NoMoreInputWrites,
		// Token: 0x04000FC5 RID: 4037
		InvalidComputerName,
		// Token: 0x04000FC6 RID: 4038
		ProxyAmbiguosAuthentication,
		// Token: 0x04000FC7 RID: 4039
		ProxyCredentialWithoutAccess,
		// Token: 0x04000FC8 RID: 4040
		PushedRunspaceMustBeOpen = 951U,
		// Token: 0x04000FC9 RID: 4041
		HostDoesNotSupportPushRunspace,
		// Token: 0x04000FCA RID: 4042
		RemoteRunspaceHasMultipleMatchesForSpecifiedRunspaceId,
		// Token: 0x04000FCB RID: 4043
		RemoteRunspaceHasMultipleMatchesForSpecifiedSessionId,
		// Token: 0x04000FCC RID: 4044
		RemoteRunspaceHasMultipleMatchesForSpecifiedName,
		// Token: 0x04000FCD RID: 4045
		RemoteRunspaceDoesNotSupportPushRunspace,
		// Token: 0x04000FCE RID: 4046
		HostInNestedPrompt,
		// Token: 0x04000FCF RID: 4047
		RemoteHostDoesNotSupportPushRunspace,
		// Token: 0x04000FD0 RID: 4048
		InvalidVMId,
		// Token: 0x04000FD1 RID: 4049
		InvalidVMNameNoVM,
		// Token: 0x04000FD2 RID: 4050
		InvalidVMNameMultipleVM,
		// Token: 0x04000FD3 RID: 4051
		HyperVModuleNotAvailable,
		// Token: 0x04000FD4 RID: 4052
		InvalidUsername,
		// Token: 0x04000FD5 RID: 4053
		InvalidCredential,
		// Token: 0x04000FD6 RID: 4054
		VMSessionConnectFailed,
		// Token: 0x04000FD7 RID: 4055
		InvalidContainerId,
		// Token: 0x04000FD8 RID: 4056
		CannotCreateProcessInContainer,
		// Token: 0x04000FD9 RID: 4057
		CannotTerminateProcessInContainer,
		// Token: 0x04000FDA RID: 4058
		ContainersFeatureNotEnabled,
		// Token: 0x04000FDB RID: 4059
		RemoteSessionHyperVSocketServerConstructorFailure,
		// Token: 0x04000FDC RID: 4060
		InvalidContainerNameMultiple,
		// Token: 0x04000FDD RID: 4061
		InvalidContainerNameNotExist,
		// Token: 0x04000FDE RID: 4062
		ContainerSessionConnectFailed,
		// Token: 0x04000FDF RID: 4063
		InvalidVMIdNotSingle = 981U,
		// Token: 0x04000FE0 RID: 4064
		InvalidVMNameNotSingle,
		// Token: 0x04000FE1 RID: 4065
		WsmanMaxRedirectionCountVariableDescription = 1001U,
		// Token: 0x04000FE2 RID: 4066
		PSDefaultSessionOptionDescription,
		// Token: 0x04000FE3 RID: 4067
		PSSenderInfoDescription = 1004U,
		// Token: 0x04000FE4 RID: 4068
		IPCUnknownNodeType = 2001U,
		// Token: 0x04000FE5 RID: 4069
		IPCInsufficientDataforElement,
		// Token: 0x04000FE6 RID: 4070
		IPCWrongAttributeCountForDataElement,
		// Token: 0x04000FE7 RID: 4071
		IPCOnlyTextExpectedInDataElement,
		// Token: 0x04000FE8 RID: 4072
		IPCWrongAttributeCountForElement,
		// Token: 0x04000FE9 RID: 4073
		IPCUnknownElementReceived,
		// Token: 0x04000FEA RID: 4074
		IPCSupportsOnlyDefaultAuth,
		// Token: 0x04000FEB RID: 4075
		IPCWowComponentNotPresent,
		// Token: 0x04000FEC RID: 4076
		IPCServerProcessReportedError = 2100U,
		// Token: 0x04000FED RID: 4077
		IPCServerProcessExited,
		// Token: 0x04000FEE RID: 4078
		IPCErrorProcessingServerData,
		// Token: 0x04000FEF RID: 4079
		IPCUnknownCommandGuid,
		// Token: 0x04000FF0 RID: 4080
		IPCNoSignalForSession,
		// Token: 0x04000FF1 RID: 4081
		IPCSignalTimedOut,
		// Token: 0x04000FF2 RID: 4082
		IPCCloseTimedOut,
		// Token: 0x04000FF3 RID: 4083
		IPCExceptionLaunchingProcess
	}
}
