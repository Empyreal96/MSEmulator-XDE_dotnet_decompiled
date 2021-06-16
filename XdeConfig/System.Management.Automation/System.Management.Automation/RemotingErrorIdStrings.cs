using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A35 RID: 2613
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class RemotingErrorIdStrings
{
	// Token: 0x06006581 RID: 25985 RVA: 0x0020DD14 File Offset: 0x0020BF14
	internal RemotingErrorIdStrings()
	{
	}

	// Token: 0x170019C6 RID: 6598
	// (get) Token: 0x06006582 RID: 25986 RVA: 0x0020DD1C File Offset: 0x0020BF1C
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(RemotingErrorIdStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("RemotingErrorIdStrings", typeof(RemotingErrorIdStrings).Assembly);
				RemotingErrorIdStrings.resourceMan = resourceManager;
			}
			return RemotingErrorIdStrings.resourceMan;
		}
	}

	// Token: 0x170019C7 RID: 6599
	// (get) Token: 0x06006583 RID: 25987 RVA: 0x0020DD5B File Offset: 0x0020BF5B
	// (set) Token: 0x06006584 RID: 25988 RVA: 0x0020DD62 File Offset: 0x0020BF62
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return RemotingErrorIdStrings.resourceCulture;
		}
		set
		{
			RemotingErrorIdStrings.resourceCulture = value;
		}
	}

	// Token: 0x170019C8 RID: 6600
	// (get) Token: 0x06006585 RID: 25989 RVA: 0x0020DD6A File Offset: 0x0020BF6A
	internal static string AsJobAndDisconnectedError
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("AsJobAndDisconnectedError", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019C9 RID: 6601
	// (get) Token: 0x06006586 RID: 25990 RVA: 0x0020DD80 File Offset: 0x0020BF80
	internal static string AssemblyLoadAttributesNotFound
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("AssemblyLoadAttributesNotFound", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019CA RID: 6602
	// (get) Token: 0x06006587 RID: 25991 RVA: 0x0020DD96 File Offset: 0x0020BF96
	internal static string AuthenticationMechanismRequiresCredential
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("AuthenticationMechanismRequiresCredential", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019CB RID: 6603
	// (get) Token: 0x06006588 RID: 25992 RVA: 0x0020DDAC File Offset: 0x0020BFAC
	internal static string AutoRemoveCannotBeUsedWithoutWait
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("AutoRemoveCannotBeUsedWithoutWait", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019CC RID: 6604
	// (get) Token: 0x06006589 RID: 25993 RVA: 0x0020DDC2 File Offset: 0x0020BFC2
	internal static string BadRunspaceTypeForJob
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("BadRunspaceTypeForJob", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019CD RID: 6605
	// (get) Token: 0x0600658A RID: 25994 RVA: 0x0020DDD8 File Offset: 0x0020BFD8
	internal static string BlobLengthNotInRange
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("BlobLengthNotInRange", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019CE RID: 6606
	// (get) Token: 0x0600658B RID: 25995 RVA: 0x0020DDEE File Offset: 0x0020BFEE
	internal static string BlockCannotBeUsedWithKeep
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("BlockCannotBeUsedWithKeep", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019CF RID: 6607
	// (get) Token: 0x0600658C RID: 25996 RVA: 0x0020DE04 File Offset: 0x0020C004
	internal static string CannotConnectContainerNamedPipe
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CannotConnectContainerNamedPipe", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019D0 RID: 6608
	// (get) Token: 0x0600658D RID: 25997 RVA: 0x0020DE1A File Offset: 0x0020C01A
	internal static string CannotConnectNamedPipe
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CannotConnectNamedPipe", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019D1 RID: 6609
	// (get) Token: 0x0600658E RID: 25998 RVA: 0x0020DE30 File Offset: 0x0020C030
	internal static string CannotCreateNamedPipe
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CannotCreateNamedPipe", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019D2 RID: 6610
	// (get) Token: 0x0600658F RID: 25999 RVA: 0x0020DE46 File Offset: 0x0020C046
	internal static string CannotCreateProcessInContainer
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CannotCreateProcessInContainer", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019D3 RID: 6611
	// (get) Token: 0x06006590 RID: 26000 RVA: 0x0020DE5C File Offset: 0x0020C05C
	internal static string CannotCreateRunspaceInconsistentState
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CannotCreateRunspaceInconsistentState", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019D4 RID: 6612
	// (get) Token: 0x06006591 RID: 26001 RVA: 0x0020DE72 File Offset: 0x0020C072
	internal static string CannotDebugJobInvalidDebuggerMode
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CannotDebugJobInvalidDebuggerMode", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019D5 RID: 6613
	// (get) Token: 0x06006592 RID: 26002 RVA: 0x0020DE88 File Offset: 0x0020C088
	internal static string CannotDebugJobNoHostDebugger
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CannotDebugJobNoHostDebugger", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019D6 RID: 6614
	// (get) Token: 0x06006593 RID: 26003 RVA: 0x0020DE9E File Offset: 0x0020C09E
	internal static string CannotDebugJobNoHostUI
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CannotDebugJobNoHostUI", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019D7 RID: 6615
	// (get) Token: 0x06006594 RID: 26004 RVA: 0x0020DEB4 File Offset: 0x0020C0B4
	internal static string CannotDisconnectSessionWithInvalidIdleTimeout
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CannotDisconnectSessionWithInvalidIdleTimeout", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019D8 RID: 6616
	// (get) Token: 0x06006595 RID: 26005 RVA: 0x0020DECA File Offset: 0x0020C0CA
	internal static string CannotExitNestedPipeline
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CannotExitNestedPipeline", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019D9 RID: 6617
	// (get) Token: 0x06006596 RID: 26006 RVA: 0x0020DEE0 File Offset: 0x0020C0E0
	internal static string CannotFindJobWithId
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CannotFindJobWithId", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019DA RID: 6618
	// (get) Token: 0x06006597 RID: 26007 RVA: 0x0020DEF6 File Offset: 0x0020C0F6
	internal static string CannotFindJobWithInstanceId
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CannotFindJobWithInstanceId", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019DB RID: 6619
	// (get) Token: 0x06006598 RID: 26008 RVA: 0x0020DF0C File Offset: 0x0020C10C
	internal static string CannotFindJobWithName
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CannotFindJobWithName", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019DC RID: 6620
	// (get) Token: 0x06006599 RID: 26009 RVA: 0x0020DF22 File Offset: 0x0020C122
	internal static string CannotFindSessionForConnect
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CannotFindSessionForConnect", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019DD RID: 6621
	// (get) Token: 0x0600659A RID: 26010 RVA: 0x0020DF38 File Offset: 0x0020C138
	internal static string CannotGetStdErrHandle
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CannotGetStdErrHandle", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019DE RID: 6622
	// (get) Token: 0x0600659B RID: 26011 RVA: 0x0020DF4E File Offset: 0x0020C14E
	internal static string CannotGetStdInHandle
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CannotGetStdInHandle", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019DF RID: 6623
	// (get) Token: 0x0600659C RID: 26012 RVA: 0x0020DF64 File Offset: 0x0020C164
	internal static string CannotGetStdOutHandle
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CannotGetStdOutHandle", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019E0 RID: 6624
	// (get) Token: 0x0600659D RID: 26013 RVA: 0x0020DF7A File Offset: 0x0020C17A
	internal static string CannotInvokeNestedCommandNestedCommandRunning
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CannotInvokeNestedCommandNestedCommandRunning", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019E1 RID: 6625
	// (get) Token: 0x0600659E RID: 26014 RVA: 0x0020DF90 File Offset: 0x0020C190
	internal static string CannotRemoveJob
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CannotRemoveJob", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019E2 RID: 6626
	// (get) Token: 0x0600659F RID: 26015 RVA: 0x0020DFA6 File Offset: 0x0020C1A6
	internal static string CannotStartJobInconsistentLanguageMode
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CannotStartJobInconsistentLanguageMode", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019E3 RID: 6627
	// (get) Token: 0x060065A0 RID: 26016 RVA: 0x0020DFBC File Offset: 0x0020C1BC
	internal static string CannotTerminateProcessInContainer
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CannotTerminateProcessInContainer", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019E4 RID: 6628
	// (get) Token: 0x060065A1 RID: 26017 RVA: 0x0020DFD2 File Offset: 0x0020C1D2
	internal static string CantCastCommandToPSObject
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CantCastCommandToPSObject", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019E5 RID: 6629
	// (get) Token: 0x060065A2 RID: 26018 RVA: 0x0020DFE8 File Offset: 0x0020C1E8
	internal static string CantCastParameterToPSObject
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CantCastParameterToPSObject", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019E6 RID: 6630
	// (get) Token: 0x060065A3 RID: 26019 RVA: 0x0020DFFE File Offset: 0x0020C1FE
	internal static string CantCastPropertyToExpectedType
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CantCastPropertyToExpectedType", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019E7 RID: 6631
	// (get) Token: 0x060065A4 RID: 26020 RVA: 0x0020E014 File Offset: 0x0020C214
	internal static string CantCastRemotingDataToPSObject
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CantCastRemotingDataToPSObject", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019E8 RID: 6632
	// (get) Token: 0x060065A5 RID: 26021 RVA: 0x0020E02A File Offset: 0x0020C22A
	internal static string ClientKeyExchangeFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ClientKeyExchangeFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019E9 RID: 6633
	// (get) Token: 0x060065A6 RID: 26022 RVA: 0x0020E040 File Offset: 0x0020C240
	internal static string ClientNegotiationFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ClientNegotiationFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019EA RID: 6634
	// (get) Token: 0x060065A7 RID: 26023 RVA: 0x0020E056 File Offset: 0x0020C256
	internal static string ClientNegotiationTimeout
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ClientNegotiationTimeout", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019EB RID: 6635
	// (get) Token: 0x060065A8 RID: 26024 RVA: 0x0020E06C File Offset: 0x0020C26C
	internal static string ClientNotFoundCapabilityProperties
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ClientNotFoundCapabilityProperties", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019EC RID: 6636
	// (get) Token: 0x060065A9 RID: 26025 RVA: 0x0020E082 File Offset: 0x0020C282
	internal static string ClientReceiveFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ClientReceiveFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019ED RID: 6637
	// (get) Token: 0x060065AA RID: 26026 RVA: 0x0020E098 File Offset: 0x0020C298
	internal static string ClientRequestedToCloseSession
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ClientRequestedToCloseSession", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019EE RID: 6638
	// (get) Token: 0x060065AB RID: 26027 RVA: 0x0020E0AE File Offset: 0x0020C2AE
	internal static string ClientSendFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ClientSendFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019EF RID: 6639
	// (get) Token: 0x060065AC RID: 26028 RVA: 0x0020E0C4 File Offset: 0x0020C2C4
	internal static string CloseCompleted
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CloseCompleted", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019F0 RID: 6640
	// (get) Token: 0x060065AD RID: 26029 RVA: 0x0020E0DA File Offset: 0x0020C2DA
	internal static string CloseExCallBackError
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CloseExCallBackError", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019F1 RID: 6641
	// (get) Token: 0x060065AE RID: 26030 RVA: 0x0020E0F0 File Offset: 0x0020C2F0
	internal static string CloseFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CloseFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019F2 RID: 6642
	// (get) Token: 0x060065AF RID: 26031 RVA: 0x0020E106 File Offset: 0x0020C306
	internal static string CloseIsCalled
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CloseIsCalled", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019F3 RID: 6643
	// (get) Token: 0x060065B0 RID: 26032 RVA: 0x0020E11C File Offset: 0x0020C31C
	internal static string CommandHandleIsNull
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CommandHandleIsNull", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019F4 RID: 6644
	// (get) Token: 0x060065B1 RID: 26033 RVA: 0x0020E132 File Offset: 0x0020C332
	internal static string CommandReceiveExCallBackError
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CommandReceiveExCallBackError", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019F5 RID: 6645
	// (get) Token: 0x060065B2 RID: 26034 RVA: 0x0020E148 File Offset: 0x0020C348
	internal static string CommandReceiveExFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CommandReceiveExFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019F6 RID: 6646
	// (get) Token: 0x060065B3 RID: 26035 RVA: 0x0020E15E File Offset: 0x0020C35E
	internal static string CommandSendExCallBackError
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CommandSendExCallBackError", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019F7 RID: 6647
	// (get) Token: 0x060065B4 RID: 26036 RVA: 0x0020E174 File Offset: 0x0020C374
	internal static string CommandSendExFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CommandSendExFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019F8 RID: 6648
	// (get) Token: 0x060065B5 RID: 26037 RVA: 0x0020E18A File Offset: 0x0020C38A
	internal static string ComputerNameParamNotSupported
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ComputerNameParamNotSupported", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019F9 RID: 6649
	// (get) Token: 0x060065B6 RID: 26038 RVA: 0x0020E1A0 File Offset: 0x0020C3A0
	internal static string ConnectExCallBackError
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ConnectExCallBackError", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019FA RID: 6650
	// (get) Token: 0x060065B7 RID: 26039 RVA: 0x0020E1B6 File Offset: 0x0020C3B6
	internal static string ConnectExFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ConnectExFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019FB RID: 6651
	// (get) Token: 0x060065B8 RID: 26040 RVA: 0x0020E1CC File Offset: 0x0020C3CC
	internal static string ConnectFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ConnectFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019FC RID: 6652
	// (get) Token: 0x060065B9 RID: 26041 RVA: 0x0020E1E2 File Offset: 0x0020C3E2
	internal static string ConnectNamedPipeTimeout
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ConnectNamedPipeTimeout", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019FD RID: 6653
	// (get) Token: 0x060065BA RID: 26042 RVA: 0x0020E1F8 File Offset: 0x0020C3F8
	internal static string ContainerSessionConnectFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ContainerSessionConnectFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019FE RID: 6654
	// (get) Token: 0x060065BB RID: 26043 RVA: 0x0020E20E File Offset: 0x0020C40E
	internal static string ContainersFeatureNotEnabled
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ContainersFeatureNotEnabled", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x170019FF RID: 6655
	// (get) Token: 0x060065BC RID: 26044 RVA: 0x0020E224 File Offset: 0x0020C424
	internal static string CouldNotFindRoleCapability
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CouldNotFindRoleCapability", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A00 RID: 6656
	// (get) Token: 0x060065BD RID: 26045 RVA: 0x0020E23A File Offset: 0x0020C43A
	internal static string CouldNotResolveRoleDefinitionPrincipal
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CouldNotResolveRoleDefinitionPrincipal", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A01 RID: 6657
	// (get) Token: 0x060065BE RID: 26046 RVA: 0x0020E250 File Offset: 0x0020C450
	internal static string CouldNotResolveUsername
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CouldNotResolveUsername", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A02 RID: 6658
	// (get) Token: 0x060065BF RID: 26047 RVA: 0x0020E266 File Offset: 0x0020C466
	internal static string CSCDoubleParameterOutOfRange
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CSCDoubleParameterOutOfRange", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A03 RID: 6659
	// (get) Token: 0x060065C0 RID: 26048 RVA: 0x0020E27C File Offset: 0x0020C47C
	internal static string CSCmdsShellNotFound
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CSCmdsShellNotFound", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A04 RID: 6660
	// (get) Token: 0x060065C1 RID: 26049 RVA: 0x0020E292 File Offset: 0x0020C492
	internal static string CSCmdsShellNotPowerShellBased
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CSCmdsShellNotPowerShellBased", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A05 RID: 6661
	// (get) Token: 0x060065C2 RID: 26050 RVA: 0x0020E2A8 File Offset: 0x0020C4A8
	internal static string CSCmdsTypeNeedsAssembly
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CSCmdsTypeNeedsAssembly", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A06 RID: 6662
	// (get) Token: 0x060065C3 RID: 26051 RVA: 0x0020E2BE File Offset: 0x0020C4BE
	internal static string CSShouldProcessAction
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CSShouldProcessAction", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A07 RID: 6663
	// (get) Token: 0x060065C4 RID: 26052 RVA: 0x0020E2D4 File Offset: 0x0020C4D4
	internal static string CSShouldProcessTarget
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CSShouldProcessTarget", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A08 RID: 6664
	// (get) Token: 0x060065C5 RID: 26053 RVA: 0x0020E2EA File Offset: 0x0020C4EA
	internal static string CSShouldProcessTargetAdminEnable
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CSShouldProcessTargetAdminEnable", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A09 RID: 6665
	// (get) Token: 0x060065C6 RID: 26054 RVA: 0x0020E300 File Offset: 0x0020C500
	internal static string CustomShellNotFound
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("CustomShellNotFound", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A0A RID: 6666
	// (get) Token: 0x060065C7 RID: 26055 RVA: 0x0020E316 File Offset: 0x0020C516
	internal static string DcsScriptMessageV
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DcsScriptMessageV", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A0B RID: 6667
	// (get) Token: 0x060065C8 RID: 26056 RVA: 0x0020E32C File Offset: 0x0020C52C
	internal static string DcsShouldProcessTarget
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DcsShouldProcessTarget", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A0C RID: 6668
	// (get) Token: 0x060065C9 RID: 26057 RVA: 0x0020E342 File Offset: 0x0020C542
	internal static string DcsWarningMessage
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DcsWarningMessage", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A0D RID: 6669
	// (get) Token: 0x060065CA RID: 26058 RVA: 0x0020E358 File Offset: 0x0020C558
	internal static string DecodingErrorForErrorRecord
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DecodingErrorForErrorRecord", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A0E RID: 6670
	// (get) Token: 0x060065CB RID: 26059 RVA: 0x0020E36E File Offset: 0x0020C56E
	internal static string DecodingErrorForMaxRunspaces
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DecodingErrorForMaxRunspaces", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A0F RID: 6671
	// (get) Token: 0x060065CC RID: 26060 RVA: 0x0020E384 File Offset: 0x0020C584
	internal static string DecodingErrorForMinRunspaces
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DecodingErrorForMinRunspaces", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A10 RID: 6672
	// (get) Token: 0x060065CD RID: 26061 RVA: 0x0020E39A File Offset: 0x0020C59A
	internal static string DecodingErrorForPipelineStateInfo
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DecodingErrorForPipelineStateInfo", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A11 RID: 6673
	// (get) Token: 0x060065CE RID: 26062 RVA: 0x0020E3B0 File Offset: 0x0020C5B0
	internal static string DecodingErrorForPowerShellStateInfo
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DecodingErrorForPowerShellStateInfo", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A12 RID: 6674
	// (get) Token: 0x060065CF RID: 26063 RVA: 0x0020E3C6 File Offset: 0x0020C5C6
	internal static string DecodingErrorForRunspacePoolStateInfo
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DecodingErrorForRunspacePoolStateInfo", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A13 RID: 6675
	// (get) Token: 0x060065D0 RID: 26064 RVA: 0x0020E3DC File Offset: 0x0020C5DC
	internal static string DecodingErrorForRunspaceStateInfo
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DecodingErrorForRunspaceStateInfo", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A14 RID: 6676
	// (get) Token: 0x060065D1 RID: 26065 RVA: 0x0020E3F2 File Offset: 0x0020C5F2
	internal static string DefaultRemotingExceptionMessage
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DefaultRemotingExceptionMessage", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A15 RID: 6677
	// (get) Token: 0x060065D2 RID: 26066 RVA: 0x0020E408 File Offset: 0x0020C608
	internal static string DeserializedObjectIsNull
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DeserializedObjectIsNull", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A16 RID: 6678
	// (get) Token: 0x060065D3 RID: 26067 RVA: 0x0020E41E File Offset: 0x0020C61E
	internal static string DisableRemotingShouldProcessTarget
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DisableRemotingShouldProcessTarget", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A17 RID: 6679
	// (get) Token: 0x060065D4 RID: 26068 RVA: 0x0020E434 File Offset: 0x0020C634
	internal static string DISCAliasDefinitionsComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCAliasDefinitionsComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A18 RID: 6680
	// (get) Token: 0x060065D5 RID: 26069 RVA: 0x0020E44A File Offset: 0x0020C64A
	internal static string DISCAssembliesToLoadComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCAssembliesToLoadComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A19 RID: 6681
	// (get) Token: 0x060065D6 RID: 26070 RVA: 0x0020E460 File Offset: 0x0020C660
	internal static string DISCAuthorComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCAuthorComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A1A RID: 6682
	// (get) Token: 0x060065D7 RID: 26071 RVA: 0x0020E476 File Offset: 0x0020C676
	internal static string DISCCLRVersionComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCCLRVersionComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A1B RID: 6683
	// (get) Token: 0x060065D8 RID: 26072 RVA: 0x0020E48C File Offset: 0x0020C68C
	internal static string DISCCommandModificationSyntax
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCCommandModificationSyntax", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A1C RID: 6684
	// (get) Token: 0x060065D9 RID: 26073 RVA: 0x0020E4A2 File Offset: 0x0020C6A2
	internal static string DISCCompanyNameComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCCompanyNameComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A1D RID: 6685
	// (get) Token: 0x060065DA RID: 26074 RVA: 0x0020E4B8 File Offset: 0x0020C6B8
	internal static string DISCCopyrightComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCCopyrightComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A1E RID: 6686
	// (get) Token: 0x060065DB RID: 26075 RVA: 0x0020E4CE File Offset: 0x0020C6CE
	internal static string DISCDescriptionComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCDescriptionComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A1F RID: 6687
	// (get) Token: 0x060065DC RID: 26076 RVA: 0x0020E4E4 File Offset: 0x0020C6E4
	internal static string DISCEnvironmentVariablesComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCEnvironmentVariablesComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A20 RID: 6688
	// (get) Token: 0x060065DD RID: 26077 RVA: 0x0020E4FA File Offset: 0x0020C6FA
	internal static string DISCErrorParsingConfigFile
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCErrorParsingConfigFile", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A21 RID: 6689
	// (get) Token: 0x060065DE RID: 26078 RVA: 0x0020E510 File Offset: 0x0020C710
	internal static string DISCExecutionPolicyComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCExecutionPolicyComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A22 RID: 6690
	// (get) Token: 0x060065DF RID: 26079 RVA: 0x0020E526 File Offset: 0x0020C726
	internal static string DISCFormatsToProcessComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCFormatsToProcessComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A23 RID: 6691
	// (get) Token: 0x060065E0 RID: 26080 RVA: 0x0020E53C File Offset: 0x0020C73C
	internal static string DISCFunctionDefinitionsComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCFunctionDefinitionsComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A24 RID: 6692
	// (get) Token: 0x060065E1 RID: 26081 RVA: 0x0020E552 File Offset: 0x0020C752
	internal static string DISCGUIDComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCGUIDComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A25 RID: 6693
	// (get) Token: 0x060065E2 RID: 26082 RVA: 0x0020E568 File Offset: 0x0020C768
	internal static string DISCInitialSessionStateComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCInitialSessionStateComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A26 RID: 6694
	// (get) Token: 0x060065E3 RID: 26083 RVA: 0x0020E57E File Offset: 0x0020C77E
	internal static string DISCInvalidExtension
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCInvalidExtension", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A27 RID: 6695
	// (get) Token: 0x060065E4 RID: 26084 RVA: 0x0020E594 File Offset: 0x0020C794
	internal static string DISCInvalidKey
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCInvalidKey", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A28 RID: 6696
	// (get) Token: 0x060065E5 RID: 26085 RVA: 0x0020E5AA File Offset: 0x0020C7AA
	internal static string DISCKeyMustBeScriptBlock
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCKeyMustBeScriptBlock", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A29 RID: 6697
	// (get) Token: 0x060065E6 RID: 26086 RVA: 0x0020E5C0 File Offset: 0x0020C7C0
	internal static string DISCLanguageModeComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCLanguageModeComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A2A RID: 6698
	// (get) Token: 0x060065E7 RID: 26087 RVA: 0x0020E5D6 File Offset: 0x0020C7D6
	internal static string DISCMissingSchemaVersion
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCMissingSchemaVersion", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A2B RID: 6699
	// (get) Token: 0x060065E8 RID: 26088 RVA: 0x0020E5EC File Offset: 0x0020C7EC
	internal static string DISCModulesToImportComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCModulesToImportComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A2C RID: 6700
	// (get) Token: 0x060065E9 RID: 26089 RVA: 0x0020E602 File Offset: 0x0020C802
	internal static string DisconnectShellExCallBackErrr
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DisconnectShellExCallBackErrr", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A2D RID: 6701
	// (get) Token: 0x060065EA RID: 26090 RVA: 0x0020E618 File Offset: 0x0020C818
	internal static string DisconnectShellExFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DisconnectShellExFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A2E RID: 6702
	// (get) Token: 0x060065EB RID: 26091 RVA: 0x0020E62E File Offset: 0x0020C82E
	internal static string DISCPathsMustBeAbsolute
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCPathsMustBeAbsolute", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A2F RID: 6703
	// (get) Token: 0x060065EC RID: 26092 RVA: 0x0020E644 File Offset: 0x0020C844
	internal static string DISCPowerShellVersionComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCPowerShellVersionComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A30 RID: 6704
	// (get) Token: 0x060065ED RID: 26093 RVA: 0x0020E65A File Offset: 0x0020C85A
	internal static string DISCProcessorArchitectureComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCProcessorArchitectureComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A31 RID: 6705
	// (get) Token: 0x060065EE RID: 26094 RVA: 0x0020E670 File Offset: 0x0020C870
	internal static string DISCRoleDefinitionsComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCRoleDefinitionsComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A32 RID: 6706
	// (get) Token: 0x060065EF RID: 26095 RVA: 0x0020E686 File Offset: 0x0020C886
	internal static string DISCRunAsVirtualAccountComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCRunAsVirtualAccountComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A33 RID: 6707
	// (get) Token: 0x060065F0 RID: 26096 RVA: 0x0020E69C File Offset: 0x0020C89C
	internal static string DISCRunAsVirtualAccountGroupsComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCRunAsVirtualAccountGroupsComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A34 RID: 6708
	// (get) Token: 0x060065F1 RID: 26097 RVA: 0x0020E6B2 File Offset: 0x0020C8B2
	internal static string DISCSchemaVersionComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCSchemaVersionComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A35 RID: 6709
	// (get) Token: 0x060065F2 RID: 26098 RVA: 0x0020E6C8 File Offset: 0x0020C8C8
	internal static string DISCScriptsToProcessComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCScriptsToProcessComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A36 RID: 6710
	// (get) Token: 0x060065F3 RID: 26099 RVA: 0x0020E6DE File Offset: 0x0020C8DE
	internal static string DISCTranscriptDirectoryComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCTranscriptDirectoryComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A37 RID: 6711
	// (get) Token: 0x060065F4 RID: 26100 RVA: 0x0020E6F4 File Offset: 0x0020C8F4
	internal static string DISCTypeContainsInvalidKey
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCTypeContainsInvalidKey", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A38 RID: 6712
	// (get) Token: 0x060065F5 RID: 26101 RVA: 0x0020E70A File Offset: 0x0020C90A
	internal static string DISCTypeMustBeHashtable
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCTypeMustBeHashtable", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A39 RID: 6713
	// (get) Token: 0x060065F6 RID: 26102 RVA: 0x0020E720 File Offset: 0x0020C920
	internal static string DISCTypeMustBeHashtableArray
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCTypeMustBeHashtableArray", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A3A RID: 6714
	// (get) Token: 0x060065F7 RID: 26103 RVA: 0x0020E736 File Offset: 0x0020C936
	internal static string DISCTypeMustBeString
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCTypeMustBeString", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A3B RID: 6715
	// (get) Token: 0x060065F8 RID: 26104 RVA: 0x0020E74C File Offset: 0x0020C94C
	internal static string DISCTypeMustBeStringArray
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCTypeMustBeStringArray", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A3C RID: 6716
	// (get) Token: 0x060065F9 RID: 26105 RVA: 0x0020E762 File Offset: 0x0020C962
	internal static string DISCTypeMustBeStringOrHashtableArray
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCTypeMustBeStringOrHashtableArray", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A3D RID: 6717
	// (get) Token: 0x060065FA RID: 26106 RVA: 0x0020E778 File Offset: 0x0020C978
	internal static string DISCTypeMustBeStringOrHashtableArrayInFile
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCTypeMustBeStringOrHashtableArrayInFile", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A3E RID: 6718
	// (get) Token: 0x060065FB RID: 26107 RVA: 0x0020E78E File Offset: 0x0020C98E
	internal static string DISCTypeMustBeValidEnum
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCTypeMustBeValidEnum", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A3F RID: 6719
	// (get) Token: 0x060065FC RID: 26108 RVA: 0x0020E7A4 File Offset: 0x0020C9A4
	internal static string DISCTypeMustContainKey
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCTypeMustContainKey", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A40 RID: 6720
	// (get) Token: 0x060065FD RID: 26109 RVA: 0x0020E7BA File Offset: 0x0020C9BA
	internal static string DISCTypesToAddComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCTypesToAddComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A41 RID: 6721
	// (get) Token: 0x060065FE RID: 26110 RVA: 0x0020E7D0 File Offset: 0x0020C9D0
	internal static string DISCTypesToProcessComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCTypesToProcessComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A42 RID: 6722
	// (get) Token: 0x060065FF RID: 26111 RVA: 0x0020E7E6 File Offset: 0x0020C9E6
	internal static string DISCVariableDefinitionsComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCVariableDefinitionsComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A43 RID: 6723
	// (get) Token: 0x06006600 RID: 26112 RVA: 0x0020E7FC File Offset: 0x0020C9FC
	internal static string DISCVisibilityAndAutoLoadingCannotBeBothSpecified
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCVisibilityAndAutoLoadingCannotBeBothSpecified", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A44 RID: 6724
	// (get) Token: 0x06006601 RID: 26113 RVA: 0x0020E812 File Offset: 0x0020CA12
	internal static string DISCVisibleAliasesComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCVisibleAliasesComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A45 RID: 6725
	// (get) Token: 0x06006602 RID: 26114 RVA: 0x0020E828 File Offset: 0x0020CA28
	internal static string DISCVisibleCmdletsComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCVisibleCmdletsComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A46 RID: 6726
	// (get) Token: 0x06006603 RID: 26115 RVA: 0x0020E83E File Offset: 0x0020CA3E
	internal static string DISCVisibleExternalCommandsComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCVisibleExternalCommandsComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A47 RID: 6727
	// (get) Token: 0x06006604 RID: 26116 RVA: 0x0020E854 File Offset: 0x0020CA54
	internal static string DISCVisibleFunctionsComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCVisibleFunctionsComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A48 RID: 6728
	// (get) Token: 0x06006605 RID: 26117 RVA: 0x0020E86A File Offset: 0x0020CA6A
	internal static string DISCVisibleProvidersComment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DISCVisibleProvidersComment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A49 RID: 6729
	// (get) Token: 0x06006606 RID: 26118 RVA: 0x0020E880 File Offset: 0x0020CA80
	internal static string DuplicateInitializationParameterFound
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("DuplicateInitializationParameterFound", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A4A RID: 6730
	// (get) Token: 0x06006607 RID: 26119 RVA: 0x0020E896 File Offset: 0x0020CA96
	internal static string EcsScriptMessageV
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("EcsScriptMessageV", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A4B RID: 6731
	// (get) Token: 0x06006608 RID: 26120 RVA: 0x0020E8AC File Offset: 0x0020CAAC
	internal static string EcsShouldProcessTarget
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("EcsShouldProcessTarget", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A4C RID: 6732
	// (get) Token: 0x06006609 RID: 26121 RVA: 0x0020E8C2 File Offset: 0x0020CAC2
	internal static string EcsWSManQCCaption
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("EcsWSManQCCaption", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A4D RID: 6733
	// (get) Token: 0x0600660A RID: 26122 RVA: 0x0020E8D8 File Offset: 0x0020CAD8
	internal static string EcsWSManQCQuery
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("EcsWSManQCQuery", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A4E RID: 6734
	// (get) Token: 0x0600660B RID: 26123 RVA: 0x0020E8EE File Offset: 0x0020CAEE
	internal static string EcsWSManShouldProcessDesc
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("EcsWSManShouldProcessDesc", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A4F RID: 6735
	// (get) Token: 0x0600660C RID: 26124 RVA: 0x0020E904 File Offset: 0x0020CB04
	internal static string EDcsRequiresElevation
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("EDcsRequiresElevation", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A50 RID: 6736
	// (get) Token: 0x0600660D RID: 26125 RVA: 0x0020E91A File Offset: 0x0020CB1A
	internal static string EnableNetworkAccessWarning
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("EnableNetworkAccessWarning", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A51 RID: 6737
	// (get) Token: 0x0600660E RID: 26126 RVA: 0x0020E930 File Offset: 0x0020CB30
	internal static string EndpointDoesNotSupportDisconnect
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("EndpointDoesNotSupportDisconnect", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A52 RID: 6738
	// (get) Token: 0x0600660F RID: 26127 RVA: 0x0020E946 File Offset: 0x0020CB46
	internal static string EnterPSHostProcessCannotConnectToProcess
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("EnterPSHostProcessCannotConnectToProcess", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A53 RID: 6739
	// (get) Token: 0x06006610 RID: 26128 RVA: 0x0020E95C File Offset: 0x0020CB5C
	internal static string EnterPSHostProcessCantEnterSameProcess
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("EnterPSHostProcessCantEnterSameProcess", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A54 RID: 6740
	// (get) Token: 0x06006611 RID: 26129 RVA: 0x0020E972 File Offset: 0x0020CB72
	internal static string EnterPSHostProcessMultipleProcessesFoundWithName
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("EnterPSHostProcessMultipleProcessesFoundWithName", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A55 RID: 6741
	// (get) Token: 0x06006612 RID: 26130 RVA: 0x0020E988 File Offset: 0x0020CB88
	internal static string EnterPSHostProcessNoPowerShell
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("EnterPSHostProcessNoPowerShell", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A56 RID: 6742
	// (get) Token: 0x06006613 RID: 26131 RVA: 0x0020E99E File Offset: 0x0020CB9E
	internal static string EnterPSHostProcessNoProcessFoundWithId
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("EnterPSHostProcessNoProcessFoundWithId", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A57 RID: 6743
	// (get) Token: 0x06006614 RID: 26132 RVA: 0x0020E9B4 File Offset: 0x0020CBB4
	internal static string EnterPSHostProcessNoProcessFoundWithName
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("EnterPSHostProcessNoProcessFoundWithName", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A58 RID: 6744
	// (get) Token: 0x06006615 RID: 26133 RVA: 0x0020E9CA File Offset: 0x0020CBCA
	internal static string EnterPSHostProcessPrompt
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("EnterPSHostProcessPrompt", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A59 RID: 6745
	// (get) Token: 0x06006616 RID: 26134 RVA: 0x0020E9E0 File Offset: 0x0020CBE0
	internal static string EnterPSSessionBrokenSession
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("EnterPSSessionBrokenSession", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A5A RID: 6746
	// (get) Token: 0x06006617 RID: 26135 RVA: 0x0020E9F6 File Offset: 0x0020CBF6
	internal static string EnterPSSessionDisconnected
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("EnterPSSessionDisconnected", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A5B RID: 6747
	// (get) Token: 0x06006618 RID: 26136 RVA: 0x0020EA0C File Offset: 0x0020CC0C
	internal static string EnterVMSessionPrompt
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("EnterVMSessionPrompt", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A5C RID: 6748
	// (get) Token: 0x06006619 RID: 26137 RVA: 0x0020EA22 File Offset: 0x0020CC22
	internal static string ERemotingCaption
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ERemotingCaption", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A5D RID: 6749
	// (get) Token: 0x0600661A RID: 26138 RVA: 0x0020EA38 File Offset: 0x0020CC38
	internal static string ERemotingQuery
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ERemotingQuery", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A5E RID: 6750
	// (get) Token: 0x0600661B RID: 26139 RVA: 0x0020EA4E File Offset: 0x0020CC4E
	internal static string ErrorParsingTheKeyInPSSessionConfigurationFile
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ErrorParsingTheKeyInPSSessionConfigurationFile", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A5F RID: 6751
	// (get) Token: 0x0600661C RID: 26140 RVA: 0x0020EA64 File Offset: 0x0020CC64
	internal static string FatalErrorCausingClose
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("FatalErrorCausingClose", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A60 RID: 6752
	// (get) Token: 0x0600661D RID: 26141 RVA: 0x0020EA7A File Offset: 0x0020CC7A
	internal static string FilePathNotFromFileSystemProvider
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("FilePathNotFromFileSystemProvider", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A61 RID: 6753
	// (get) Token: 0x0600661E RID: 26142 RVA: 0x0020EA90 File Offset: 0x0020CC90
	internal static string FilePathShouldPS1Extension
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("FilePathShouldPS1Extension", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A62 RID: 6754
	// (get) Token: 0x0600661F RID: 26143 RVA: 0x0020EAA6 File Offset: 0x0020CCA6
	internal static string ForceCannotBeUsedWithoutWait
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ForceCannotBeUsedWithoutWait", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A63 RID: 6755
	// (get) Token: 0x06006620 RID: 26144 RVA: 0x0020EABC File Offset: 0x0020CCBC
	internal static string ForceClosed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ForceClosed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A64 RID: 6756
	// (get) Token: 0x06006621 RID: 26145 RVA: 0x0020EAD2 File Offset: 0x0020CCD2
	internal static string ForceSuspendJob
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ForceSuspendJob", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A65 RID: 6757
	// (get) Token: 0x06006622 RID: 26146 RVA: 0x0020EAE8 File Offset: 0x0020CCE8
	internal static string FoundMultipleJobsWithId
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("FoundMultipleJobsWithId", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A66 RID: 6758
	// (get) Token: 0x06006623 RID: 26147 RVA: 0x0020EAFE File Offset: 0x0020CCFE
	internal static string FoundMultipleJobsWithName
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("FoundMultipleJobsWithName", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A67 RID: 6759
	// (get) Token: 0x06006624 RID: 26148 RVA: 0x0020EB14 File Offset: 0x0020CD14
	internal static string FragmetIdsNotInSequence
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("FragmetIdsNotInSequence", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A68 RID: 6760
	// (get) Token: 0x06006625 RID: 26149 RVA: 0x0020EB2A File Offset: 0x0020CD2A
	internal static string GcsScriptMessageV
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("GcsScriptMessageV", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A69 RID: 6761
	// (get) Token: 0x06006626 RID: 26150 RVA: 0x0020EB40 File Offset: 0x0020CD40
	internal static string GeneralError
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("GeneralError", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A6A RID: 6762
	// (get) Token: 0x06006627 RID: 26151 RVA: 0x0020EB56 File Offset: 0x0020CD56
	internal static string HostDoesNotSupportIASession
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("HostDoesNotSupportIASession", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A6B RID: 6763
	// (get) Token: 0x06006628 RID: 26152 RVA: 0x0020EB6C File Offset: 0x0020CD6C
	internal static string HostDoesNotSupportPushRunspace
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("HostDoesNotSupportPushRunspace", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A6C RID: 6764
	// (get) Token: 0x06006629 RID: 26153 RVA: 0x0020EB82 File Offset: 0x0020CD82
	internal static string HostInNestedPrompt
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("HostInNestedPrompt", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A6D RID: 6765
	// (get) Token: 0x0600662A RID: 26154 RVA: 0x0020EB98 File Offset: 0x0020CD98
	internal static string HyperVModuleNotAvailable
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("HyperVModuleNotAvailable", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A6E RID: 6766
	// (get) Token: 0x0600662B RID: 26155 RVA: 0x0020EBAE File Offset: 0x0020CDAE
	internal static string HyperVSocketTransportProcessEnded
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("HyperVSocketTransportProcessEnded", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A6F RID: 6767
	// (get) Token: 0x0600662C RID: 26156 RVA: 0x0020EBC4 File Offset: 0x0020CDC4
	internal static string ICMInvalidSessionAvailability
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ICMInvalidSessionAvailability", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A70 RID: 6768
	// (get) Token: 0x0600662D RID: 26157 RVA: 0x0020EBDA File Offset: 0x0020CDDA
	internal static string ICMInvalidSessionState
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ICMInvalidSessionState", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A71 RID: 6769
	// (get) Token: 0x0600662E RID: 26158 RVA: 0x0020EBF0 File Offset: 0x0020CDF0
	internal static string ICMNoValidRunspaces
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ICMNoValidRunspaces", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A72 RID: 6770
	// (get) Token: 0x0600662F RID: 26159 RVA: 0x0020EC06 File Offset: 0x0020CE06
	internal static string InitialSessionStateNull
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InitialSessionStateNull", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A73 RID: 6771
	// (get) Token: 0x06006630 RID: 26160 RVA: 0x0020EC1C File Offset: 0x0020CE1C
	internal static string InvalidComputerName
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidComputerName", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A74 RID: 6772
	// (get) Token: 0x06006631 RID: 26161 RVA: 0x0020EC32 File Offset: 0x0020CE32
	internal static string InvalidConfigurationName
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidConfigurationName", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A75 RID: 6773
	// (get) Token: 0x06006632 RID: 26162 RVA: 0x0020EC48 File Offset: 0x0020CE48
	internal static string InvalidConfigurationXMLAttribute
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidConfigurationXMLAttribute", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A76 RID: 6774
	// (get) Token: 0x06006633 RID: 26163 RVA: 0x0020EC5E File Offset: 0x0020CE5E
	internal static string InvalidContainerId
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidContainerId", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A77 RID: 6775
	// (get) Token: 0x06006634 RID: 26164 RVA: 0x0020EC74 File Offset: 0x0020CE74
	internal static string InvalidContainerNameMultiple
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidContainerNameMultiple", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A78 RID: 6776
	// (get) Token: 0x06006635 RID: 26165 RVA: 0x0020EC8A File Offset: 0x0020CE8A
	internal static string InvalidContainerNameNotExist
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidContainerNameNotExist", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A79 RID: 6777
	// (get) Token: 0x06006636 RID: 26166 RVA: 0x0020ECA0 File Offset: 0x0020CEA0
	internal static string InvalidCredential
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidCredential", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A7A RID: 6778
	// (get) Token: 0x06006637 RID: 26167 RVA: 0x0020ECB6 File Offset: 0x0020CEB6
	internal static string InvalidIdleTimeoutOption
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidIdleTimeoutOption", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A7B RID: 6779
	// (get) Token: 0x06006638 RID: 26168 RVA: 0x0020ECCC File Offset: 0x0020CECC
	internal static string InvalidJobStateGeneral
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidJobStateGeneral", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A7C RID: 6780
	// (get) Token: 0x06006639 RID: 26169 RVA: 0x0020ECE2 File Offset: 0x0020CEE2
	internal static string InvalidJobStateSpecific
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidJobStateSpecific", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A7D RID: 6781
	// (get) Token: 0x0600663A RID: 26170 RVA: 0x0020ECF8 File Offset: 0x0020CEF8
	internal static string InvalidPSSessionConfigurationFile
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidPSSessionConfigurationFile", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A7E RID: 6782
	// (get) Token: 0x0600663B RID: 26171 RVA: 0x0020ED0E File Offset: 0x0020CF0E
	internal static string InvalidPSSessionConfigurationFileErrorProcessing
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidPSSessionConfigurationFileErrorProcessing", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A7F RID: 6783
	// (get) Token: 0x0600663C RID: 26172 RVA: 0x0020ED24 File Offset: 0x0020CF24
	internal static string InvalidPSSessionConfigurationFilePath
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidPSSessionConfigurationFilePath", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A80 RID: 6784
	// (get) Token: 0x0600663D RID: 26173 RVA: 0x0020ED3A File Offset: 0x0020CF3A
	internal static string InvalidRegisterPSSessionConfigurationModulePath
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidRegisterPSSessionConfigurationModulePath", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A81 RID: 6785
	// (get) Token: 0x0600663E RID: 26174 RVA: 0x0020ED50 File Offset: 0x0020CF50
	internal static string InvalidRoleCapabilityFilePath
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidRoleCapabilityFilePath", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A82 RID: 6786
	// (get) Token: 0x0600663F RID: 26175 RVA: 0x0020ED66 File Offset: 0x0020CF66
	internal static string InvalidRoleCapabilityKey
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidRoleCapabilityKey", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A83 RID: 6787
	// (get) Token: 0x06006640 RID: 26176 RVA: 0x0020ED7C File Offset: 0x0020CF7C
	internal static string InvalidRoleEntry
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidRoleEntry", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A84 RID: 6788
	// (get) Token: 0x06006641 RID: 26177 RVA: 0x0020ED92 File Offset: 0x0020CF92
	internal static string InvalidRoleValue
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidRoleValue", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A85 RID: 6789
	// (get) Token: 0x06006642 RID: 26178 RVA: 0x0020EDA8 File Offset: 0x0020CFA8
	internal static string InvalidSchemeValue
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidSchemeValue", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A86 RID: 6790
	// (get) Token: 0x06006643 RID: 26179 RVA: 0x0020EDBE File Offset: 0x0020CFBE
	internal static string InvalidUsername
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidUsername", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A87 RID: 6791
	// (get) Token: 0x06006644 RID: 26180 RVA: 0x0020EDD4 File Offset: 0x0020CFD4
	internal static string InvalidVMId
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidVMId", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A88 RID: 6792
	// (get) Token: 0x06006645 RID: 26181 RVA: 0x0020EDEA File Offset: 0x0020CFEA
	internal static string InvalidVMIdNotSingle
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidVMIdNotSingle", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A89 RID: 6793
	// (get) Token: 0x06006646 RID: 26182 RVA: 0x0020EE00 File Offset: 0x0020D000
	internal static string InvalidVMNameMultipleVM
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidVMNameMultipleVM", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A8A RID: 6794
	// (get) Token: 0x06006647 RID: 26183 RVA: 0x0020EE16 File Offset: 0x0020D016
	internal static string InvalidVMNameNotSingle
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidVMNameNotSingle", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A8B RID: 6795
	// (get) Token: 0x06006648 RID: 26184 RVA: 0x0020EE2C File Offset: 0x0020D02C
	internal static string InvalidVMNameNoVM
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvalidVMNameNoVM", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A8C RID: 6796
	// (get) Token: 0x06006649 RID: 26185 RVA: 0x0020EE42 File Offset: 0x0020D042
	internal static string InvokeDisconnectedWithoutComputerName
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("InvokeDisconnectedWithoutComputerName", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A8D RID: 6797
	// (get) Token: 0x0600664A RID: 26186 RVA: 0x0020EE58 File Offset: 0x0020D058
	internal static string IPCCloseTimedOut
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("IPCCloseTimedOut", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A8E RID: 6798
	// (get) Token: 0x0600664B RID: 26187 RVA: 0x0020EE6E File Offset: 0x0020D06E
	internal static string IPCErrorProcessingServerData
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("IPCErrorProcessingServerData", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A8F RID: 6799
	// (get) Token: 0x0600664C RID: 26188 RVA: 0x0020EE84 File Offset: 0x0020D084
	internal static string IPCExceptionLaunchingProcess
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("IPCExceptionLaunchingProcess", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A90 RID: 6800
	// (get) Token: 0x0600664D RID: 26189 RVA: 0x0020EE9A File Offset: 0x0020D09A
	internal static string IPCInsufficientDataforElement
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("IPCInsufficientDataforElement", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A91 RID: 6801
	// (get) Token: 0x0600664E RID: 26190 RVA: 0x0020EEB0 File Offset: 0x0020D0B0
	internal static string IPCNoSignalForSession
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("IPCNoSignalForSession", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A92 RID: 6802
	// (get) Token: 0x0600664F RID: 26191 RVA: 0x0020EEC6 File Offset: 0x0020D0C6
	internal static string IPCOnlyTextExpectedInDataElement
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("IPCOnlyTextExpectedInDataElement", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A93 RID: 6803
	// (get) Token: 0x06006650 RID: 26192 RVA: 0x0020EEDC File Offset: 0x0020D0DC
	internal static string IPCServerProcessExited
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("IPCServerProcessExited", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A94 RID: 6804
	// (get) Token: 0x06006651 RID: 26193 RVA: 0x0020EEF2 File Offset: 0x0020D0F2
	internal static string IPCServerProcessReportedError
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("IPCServerProcessReportedError", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A95 RID: 6805
	// (get) Token: 0x06006652 RID: 26194 RVA: 0x0020EF08 File Offset: 0x0020D108
	internal static string IPCSignalTimedOut
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("IPCSignalTimedOut", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A96 RID: 6806
	// (get) Token: 0x06006653 RID: 26195 RVA: 0x0020EF1E File Offset: 0x0020D11E
	internal static string IPCSupportsOnlyDefaultAuth
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("IPCSupportsOnlyDefaultAuth", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A97 RID: 6807
	// (get) Token: 0x06006654 RID: 26196 RVA: 0x0020EF34 File Offset: 0x0020D134
	internal static string IPCTransportConnectError
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("IPCTransportConnectError", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A98 RID: 6808
	// (get) Token: 0x06006655 RID: 26197 RVA: 0x0020EF4A File Offset: 0x0020D14A
	internal static string IPCUnknownCommandGuid
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("IPCUnknownCommandGuid", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A99 RID: 6809
	// (get) Token: 0x06006656 RID: 26198 RVA: 0x0020EF60 File Offset: 0x0020D160
	internal static string IPCUnknownElementReceived
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("IPCUnknownElementReceived", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A9A RID: 6810
	// (get) Token: 0x06006657 RID: 26199 RVA: 0x0020EF76 File Offset: 0x0020D176
	internal static string IPCUnknownNodeType
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("IPCUnknownNodeType", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A9B RID: 6811
	// (get) Token: 0x06006658 RID: 26200 RVA: 0x0020EF8C File Offset: 0x0020D18C
	internal static string IPCWowComponentNotPresent
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("IPCWowComponentNotPresent", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A9C RID: 6812
	// (get) Token: 0x06006659 RID: 26201 RVA: 0x0020EFA2 File Offset: 0x0020D1A2
	internal static string IPCWrongAttributeCountForDataElement
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("IPCWrongAttributeCountForDataElement", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A9D RID: 6813
	// (get) Token: 0x0600665A RID: 26202 RVA: 0x0020EFB8 File Offset: 0x0020D1B8
	internal static string IPCWrongAttributeCountForElement
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("IPCWrongAttributeCountForElement", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A9E RID: 6814
	// (get) Token: 0x0600665B RID: 26203 RVA: 0x0020EFCE File Offset: 0x0020D1CE
	internal static string ItemNotFoundInRepository
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ItemNotFoundInRepository", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001A9F RID: 6815
	// (get) Token: 0x0600665C RID: 26204 RVA: 0x0020EFE4 File Offset: 0x0020D1E4
	internal static string JobActionInvalidWithNoChildJobs
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("JobActionInvalidWithNoChildJobs", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AA0 RID: 6816
	// (get) Token: 0x0600665D RID: 26205 RVA: 0x0020EFFA File Offset: 0x0020D1FA
	internal static string JobActionInvalidWithNullChild
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("JobActionInvalidWithNullChild", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AA1 RID: 6817
	// (get) Token: 0x0600665E RID: 26206 RVA: 0x0020F010 File Offset: 0x0020D210
	internal static string JobBlockedSoWaitJobCannotContinue
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("JobBlockedSoWaitJobCannotContinue", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AA2 RID: 6818
	// (get) Token: 0x0600665F RID: 26207 RVA: 0x0020F026 File Offset: 0x0020D226
	internal static string JobConnectFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("JobConnectFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AA3 RID: 6819
	// (get) Token: 0x06006660 RID: 26208 RVA: 0x0020F03C File Offset: 0x0020D23C
	internal static string JobIdentifierNull
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("JobIdentifierNull", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AA4 RID: 6820
	// (get) Token: 0x06006661 RID: 26209 RVA: 0x0020F052 File Offset: 0x0020D252
	internal static string JobIdNotYetAssigned
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("JobIdNotYetAssigned", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AA5 RID: 6821
	// (get) Token: 0x06006662 RID: 26210 RVA: 0x0020F068 File Offset: 0x0020D268
	internal static string JobManagerRegistrationConstructorError
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("JobManagerRegistrationConstructorError", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AA6 RID: 6822
	// (get) Token: 0x06006663 RID: 26211 RVA: 0x0020F07E File Offset: 0x0020D27E
	internal static string JobResumeNotSupported
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("JobResumeNotSupported", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AA7 RID: 6823
	// (get) Token: 0x06006664 RID: 26212 RVA: 0x0020F094 File Offset: 0x0020D294
	internal static string JobSessionIdLessThanOne
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("JobSessionIdLessThanOne", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AA8 RID: 6824
	// (get) Token: 0x06006665 RID: 26213 RVA: 0x0020F0AA File Offset: 0x0020D2AA
	internal static string JobSourceAdapterCannotSaveNullJob
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("JobSourceAdapterCannotSaveNullJob", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AA9 RID: 6825
	// (get) Token: 0x06006666 RID: 26214 RVA: 0x0020F0C0 File Offset: 0x0020D2C0
	internal static string JobSourceAdapterError
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("JobSourceAdapterError", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AAA RID: 6826
	// (get) Token: 0x06006667 RID: 26215 RVA: 0x0020F0D6 File Offset: 0x0020D2D6
	internal static string JobSourceAdapterNotFound
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("JobSourceAdapterNotFound", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AAB RID: 6827
	// (get) Token: 0x06006668 RID: 26216 RVA: 0x0020F0EC File Offset: 0x0020D2EC
	internal static string JobSuspendedDisconnectedWaitWithForce
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("JobSuspendedDisconnectedWaitWithForce", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AAC RID: 6828
	// (get) Token: 0x06006669 RID: 26217 RVA: 0x0020F102 File Offset: 0x0020D302
	internal static string JobSuspendNotSupported
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("JobSuspendNotSupported", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AAD RID: 6829
	// (get) Token: 0x0600666A RID: 26218 RVA: 0x0020F118 File Offset: 0x0020D318
	internal static string JobWasStopped
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("JobWasStopped", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AAE RID: 6830
	// (get) Token: 0x0600666B RID: 26219 RVA: 0x0020F12E File Offset: 0x0020D32E
	internal static string JobWithSpecifiedInstanceIdNotCompleted
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("JobWithSpecifiedInstanceIdNotCompleted", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AAF RID: 6831
	// (get) Token: 0x0600666C RID: 26220 RVA: 0x0020F144 File Offset: 0x0020D344
	internal static string JobWithSpecifiedInstanceIdNotFound
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("JobWithSpecifiedInstanceIdNotFound", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AB0 RID: 6832
	// (get) Token: 0x0600666D RID: 26221 RVA: 0x0020F15A File Offset: 0x0020D35A
	internal static string JobWithSpecifiedNameNotCompleted
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("JobWithSpecifiedNameNotCompleted", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AB1 RID: 6833
	// (get) Token: 0x0600666E RID: 26222 RVA: 0x0020F170 File Offset: 0x0020D370
	internal static string JobWithSpecifiedNameNotFound
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("JobWithSpecifiedNameNotFound", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AB2 RID: 6834
	// (get) Token: 0x0600666F RID: 26223 RVA: 0x0020F186 File Offset: 0x0020D386
	internal static string JobWithSpecifiedSessionIdNotCompleted
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("JobWithSpecifiedSessionIdNotCompleted", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AB3 RID: 6835
	// (get) Token: 0x06006670 RID: 26224 RVA: 0x0020F19C File Offset: 0x0020D39C
	internal static string JobWithSpecifiedSessionIdNotFound
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("JobWithSpecifiedSessionIdNotFound", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AB4 RID: 6836
	// (get) Token: 0x06006671 RID: 26225 RVA: 0x0020F1B2 File Offset: 0x0020D3B2
	internal static string MandatoryValueNotInCorrectFormat
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("MandatoryValueNotInCorrectFormat", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AB5 RID: 6837
	// (get) Token: 0x06006672 RID: 26226 RVA: 0x0020F1C8 File Offset: 0x0020D3C8
	internal static string MandatoryValueNotPresent
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("MandatoryValueNotPresent", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AB6 RID: 6838
	// (get) Token: 0x06006673 RID: 26227 RVA: 0x0020F1DE File Offset: 0x0020D3DE
	internal static string MissingCallId
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("MissingCallId", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AB7 RID: 6839
	// (get) Token: 0x06006674 RID: 26228 RVA: 0x0020F1F4 File Offset: 0x0020D3F4
	internal static string MissingDataType
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("MissingDataType", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AB8 RID: 6840
	// (get) Token: 0x06006675 RID: 26229 RVA: 0x0020F20A File Offset: 0x0020D40A
	internal static string MissingDestination
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("MissingDestination", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AB9 RID: 6841
	// (get) Token: 0x06006676 RID: 26230 RVA: 0x0020F220 File Offset: 0x0020D420
	internal static string MissingIsEndFragment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("MissingIsEndFragment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001ABA RID: 6842
	// (get) Token: 0x06006677 RID: 26231 RVA: 0x0020F236 File Offset: 0x0020D436
	internal static string MissingIsStartFragment
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("MissingIsStartFragment", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001ABB RID: 6843
	// (get) Token: 0x06006678 RID: 26232 RVA: 0x0020F24C File Offset: 0x0020D44C
	internal static string MissingMethodName
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("MissingMethodName", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001ABC RID: 6844
	// (get) Token: 0x06006679 RID: 26233 RVA: 0x0020F262 File Offset: 0x0020D462
	internal static string MissingProperty
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("MissingProperty", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001ABD RID: 6845
	// (get) Token: 0x0600667A RID: 26234 RVA: 0x0020F278 File Offset: 0x0020D478
	internal static string MissingRunspaceId
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("MissingRunspaceId", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001ABE RID: 6846
	// (get) Token: 0x0600667B RID: 26235 RVA: 0x0020F28E File Offset: 0x0020D48E
	internal static string MissingTarget
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("MissingTarget", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001ABF RID: 6847
	// (get) Token: 0x0600667C RID: 26236 RVA: 0x0020F2A4 File Offset: 0x0020D4A4
	internal static string MissingTargetClass
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("MissingTargetClass", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AC0 RID: 6848
	// (get) Token: 0x0600667D RID: 26237 RVA: 0x0020F2BA File Offset: 0x0020D4BA
	internal static string MustBeAdminToOverrideThreadOptions
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("MustBeAdminToOverrideThreadOptions", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AC1 RID: 6849
	// (get) Token: 0x0600667E RID: 26238 RVA: 0x0020F2D0 File Offset: 0x0020D4D0
	internal static string NamedPipeAlreadyListening
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("NamedPipeAlreadyListening", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AC2 RID: 6850
	// (get) Token: 0x0600667F RID: 26239 RVA: 0x0020F2E6 File Offset: 0x0020D4E6
	internal static string NamedPipeServerCannotStart
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("NamedPipeServerCannotStart", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AC3 RID: 6851
	// (get) Token: 0x06006680 RID: 26240 RVA: 0x0020F2FC File Offset: 0x0020D4FC
	internal static string NamedPipeTransportProcessEnded
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("NamedPipeTransportProcessEnded", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AC4 RID: 6852
	// (get) Token: 0x06006681 RID: 26241 RVA: 0x0020F312 File Offset: 0x0020D512
	internal static string NativeReadFileFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("NativeReadFileFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AC5 RID: 6853
	// (get) Token: 0x06006682 RID: 26242 RVA: 0x0020F328 File Offset: 0x0020D528
	internal static string NativeWriteFileFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("NativeWriteFileFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AC6 RID: 6854
	// (get) Token: 0x06006683 RID: 26243 RVA: 0x0020F33E File Offset: 0x0020D53E
	internal static string NcsCannotDeleteFile
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("NcsCannotDeleteFile", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AC7 RID: 6855
	// (get) Token: 0x06006684 RID: 26244 RVA: 0x0020F354 File Offset: 0x0020D554
	internal static string NcsCannotDeleteFileAfterInstall
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("NcsCannotDeleteFileAfterInstall", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AC8 RID: 6856
	// (get) Token: 0x06006685 RID: 26245 RVA: 0x0020F36A File Offset: 0x0020D56A
	internal static string NcsCannotWritePluginContent
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("NcsCannotWritePluginContent", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AC9 RID: 6857
	// (get) Token: 0x06006686 RID: 26246 RVA: 0x0020F380 File Offset: 0x0020D580
	internal static string NcsScriptMessageV
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("NcsScriptMessageV", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001ACA RID: 6858
	// (get) Token: 0x06006687 RID: 26247 RVA: 0x0020F396 File Offset: 0x0020D596
	internal static string NcsShouldProcessTargetSDDL
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("NcsShouldProcessTargetSDDL", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001ACB RID: 6859
	// (get) Token: 0x06006688 RID: 26248 RVA: 0x0020F3AC File Offset: 0x0020D5AC
	internal static string NestedPipelineMissingRunspace
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("NestedPipelineMissingRunspace", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001ACC RID: 6860
	// (get) Token: 0x06006689 RID: 26249 RVA: 0x0020F3C2 File Offset: 0x0020D5C2
	internal static string NestedPipelineNotSupported
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("NestedPipelineNotSupported", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001ACD RID: 6861
	// (get) Token: 0x0600668A RID: 26250 RVA: 0x0020F3D8 File Offset: 0x0020D5D8
	internal static string NetFrameWorkV2NotInstalled
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("NetFrameWorkV2NotInstalled", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001ACE RID: 6862
	// (get) Token: 0x0600668B RID: 26251 RVA: 0x0020F3EE File Offset: 0x0020D5EE
	internal static string NewJobSpecificationError
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("NewJobSpecificationError", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001ACF RID: 6863
	// (get) Token: 0x0600668C RID: 26252 RVA: 0x0020F404 File Offset: 0x0020D604
	internal static string NewRunspaceAmbiguosAuthentication
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("NewRunspaceAmbiguosAuthentication", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AD0 RID: 6864
	// (get) Token: 0x0600668D RID: 26253 RVA: 0x0020F41A File Offset: 0x0020D61A
	internal static string NoAttributesFoundForParamElement
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("NoAttributesFoundForParamElement", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AD1 RID: 6865
	// (get) Token: 0x0600668E RID: 26254 RVA: 0x0020F430 File Offset: 0x0020D630
	internal static string NonExistentInitialSessionStateProvider
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("NonExistentInitialSessionStateProvider", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AD2 RID: 6866
	// (get) Token: 0x0600668F RID: 26255 RVA: 0x0020F446 File Offset: 0x0020D646
	internal static string NoPowerShellForJob
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("NoPowerShellForJob", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AD3 RID: 6867
	// (get) Token: 0x06006690 RID: 26256 RVA: 0x0020F45C File Offset: 0x0020D65C
	internal static string NotEnoughHeaderForRemoteDataObject
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("NotEnoughHeaderForRemoteDataObject", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AD4 RID: 6868
	// (get) Token: 0x06006691 RID: 26257 RVA: 0x0020F472 File Offset: 0x0020D672
	internal static string ObjectIdCannotBeLessThanZero
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ObjectIdCannotBeLessThanZero", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AD5 RID: 6869
	// (get) Token: 0x06006692 RID: 26258 RVA: 0x0020F488 File Offset: 0x0020D688
	internal static string ObjectIdsNotMatching
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ObjectIdsNotMatching", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AD6 RID: 6870
	// (get) Token: 0x06006693 RID: 26259 RVA: 0x0020F49E File Offset: 0x0020D69E
	internal static string ObjectIsTooBig
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ObjectIsTooBig", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AD7 RID: 6871
	// (get) Token: 0x06006694 RID: 26260 RVA: 0x0020F4B4 File Offset: 0x0020D6B4
	internal static string OutOfMemory
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("OutOfMemory", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AD8 RID: 6872
	// (get) Token: 0x06006695 RID: 26261 RVA: 0x0020F4CA File Offset: 0x0020D6CA
	internal static string PipelineFailedWithoutReason
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("PipelineFailedWithoutReason", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AD9 RID: 6873
	// (get) Token: 0x06006696 RID: 26262 RVA: 0x0020F4E0 File Offset: 0x0020D6E0
	internal static string PipelineFailedWithReason
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("PipelineFailedWithReason", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001ADA RID: 6874
	// (get) Token: 0x06006697 RID: 26263 RVA: 0x0020F4F6 File Offset: 0x0020D6F6
	internal static string PipelineIdsDoNotMatch
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("PipelineIdsDoNotMatch", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001ADB RID: 6875
	// (get) Token: 0x06006698 RID: 26264 RVA: 0x0020F50C File Offset: 0x0020D70C
	internal static string PipelineNotFoundOnServer
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("PipelineNotFoundOnServer", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001ADC RID: 6876
	// (get) Token: 0x06006699 RID: 26265 RVA: 0x0020F522 File Offset: 0x0020D722
	internal static string PipelineStopped
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("PipelineStopped", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001ADD RID: 6877
	// (get) Token: 0x0600669A RID: 26266 RVA: 0x0020F538 File Offset: 0x0020D738
	internal static string PortIsOutOfRange
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("PortIsOutOfRange", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001ADE RID: 6878
	// (get) Token: 0x0600669B RID: 26267 RVA: 0x0020F54E File Offset: 0x0020D74E
	internal static string PowerShellInvokerInvalidState
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("PowerShellInvokerInvalidState", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001ADF RID: 6879
	// (get) Token: 0x0600669C RID: 26268 RVA: 0x0020F564 File Offset: 0x0020D764
	internal static string PowerShellNotInstalled
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("PowerShellNotInstalled", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AE0 RID: 6880
	// (get) Token: 0x0600669D RID: 26269 RVA: 0x0020F57A File Offset: 0x0020D77A
	internal static string ProxyAmbiguosAuthentication
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ProxyAmbiguosAuthentication", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AE1 RID: 6881
	// (get) Token: 0x0600669E RID: 26270 RVA: 0x0020F590 File Offset: 0x0020D790
	internal static string ProxyCredentialWithoutAccess
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ProxyCredentialWithoutAccess", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AE2 RID: 6882
	// (get) Token: 0x0600669F RID: 26271 RVA: 0x0020F5A6 File Offset: 0x0020D7A6
	internal static string PSDefaultSessionOptionDescription
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("PSDefaultSessionOptionDescription", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AE3 RID: 6883
	// (get) Token: 0x060066A0 RID: 26272 RVA: 0x0020F5BC File Offset: 0x0020D7BC
	internal static string PSJobProxyInvalidReasonException
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("PSJobProxyInvalidReasonException", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AE4 RID: 6884
	// (get) Token: 0x060066A1 RID: 26273 RVA: 0x0020F5D2 File Offset: 0x0020D7D2
	internal static string PSSenderInfoDescription
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("PSSenderInfoDescription", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AE5 RID: 6885
	// (get) Token: 0x060066A2 RID: 26274 RVA: 0x0020F5E8 File Offset: 0x0020D7E8
	internal static string PSSessionAppName
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("PSSessionAppName", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AE6 RID: 6886
	// (get) Token: 0x060066A3 RID: 26275 RVA: 0x0020F5FE File Offset: 0x0020D7FE
	internal static string PSSessionConfigurationFileNotFound
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("PSSessionConfigurationFileNotFound", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AE7 RID: 6887
	// (get) Token: 0x060066A4 RID: 26276 RVA: 0x0020F614 File Offset: 0x0020D814
	internal static string PSSessionConfigurationName
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("PSSessionConfigurationName", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AE8 RID: 6888
	// (get) Token: 0x060066A5 RID: 26277 RVA: 0x0020F62A File Offset: 0x0020D82A
	internal static string PSVersionParameterOutOfRange
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("PSVersionParameterOutOfRange", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AE9 RID: 6889
	// (get) Token: 0x060066A6 RID: 26278 RVA: 0x0020F640 File Offset: 0x0020D840
	internal static string PushedRunspaceMustBeOpen
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("PushedRunspaceMustBeOpen", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AEA RID: 6890
	// (get) Token: 0x060066A7 RID: 26279 RVA: 0x0020F656 File Offset: 0x0020D856
	internal static string QueryForRunspacesFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("QueryForRunspacesFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AEB RID: 6891
	// (get) Token: 0x060066A8 RID: 26280 RVA: 0x0020F66C File Offset: 0x0020D86C
	internal static string RCAutoDisconnected
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RCAutoDisconnected", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AEC RID: 6892
	// (get) Token: 0x060066A9 RID: 26281 RVA: 0x0020F682 File Offset: 0x0020D882
	internal static string RCAutoDisconnectingError
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RCAutoDisconnectingError", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AED RID: 6893
	// (get) Token: 0x060066AA RID: 26282 RVA: 0x0020F698 File Offset: 0x0020D898
	internal static string RCAutoDisconnectingWarning
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RCAutoDisconnectingWarning", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AEE RID: 6894
	// (get) Token: 0x060066AB RID: 26283 RVA: 0x0020F6AE File Offset: 0x0020D8AE
	internal static string RCConnectionRetryAttempt
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RCConnectionRetryAttempt", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AEF RID: 6895
	// (get) Token: 0x060066AC RID: 26284 RVA: 0x0020F6C4 File Offset: 0x0020D8C4
	internal static string RCDisconnectDebug
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RCDisconnectDebug", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AF0 RID: 6896
	// (get) Token: 0x060066AD RID: 26285 RVA: 0x0020F6DA File Offset: 0x0020D8DA
	internal static string RCDisconnectedJob
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RCDisconnectedJob", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AF1 RID: 6897
	// (get) Token: 0x060066AE RID: 26286 RVA: 0x0020F6F0 File Offset: 0x0020D8F0
	internal static string RCDisconnectSession
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RCDisconnectSession", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AF2 RID: 6898
	// (get) Token: 0x060066AF RID: 26287 RVA: 0x0020F706 File Offset: 0x0020D906
	internal static string RCDisconnectSessionCreated
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RCDisconnectSessionCreated", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AF3 RID: 6899
	// (get) Token: 0x060066B0 RID: 26288 RVA: 0x0020F71C File Offset: 0x0020D91C
	internal static string RCInternalError
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RCInternalError", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AF4 RID: 6900
	// (get) Token: 0x060066B1 RID: 26289 RVA: 0x0020F732 File Offset: 0x0020D932
	internal static string RCNetworkFailureDetected
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RCNetworkFailureDetected", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AF5 RID: 6901
	// (get) Token: 0x060066B2 RID: 26290 RVA: 0x0020F748 File Offset: 0x0020D948
	internal static string RCProgressActivity
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RCProgressActivity", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AF6 RID: 6902
	// (get) Token: 0x060066B3 RID: 26291 RVA: 0x0020F75E File Offset: 0x0020D95E
	internal static string RCProgressStatus
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RCProgressStatus", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AF7 RID: 6903
	// (get) Token: 0x060066B4 RID: 26292 RVA: 0x0020F774 File Offset: 0x0020D974
	internal static string RCReconnectSucceeded
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RCReconnectSucceeded", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AF8 RID: 6904
	// (get) Token: 0x060066B5 RID: 26293 RVA: 0x0020F78A File Offset: 0x0020D98A
	internal static string RcsScriptMessageV
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RcsScriptMessageV", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AF9 RID: 6905
	// (get) Token: 0x060066B6 RID: 26294 RVA: 0x0020F7A0 File Offset: 0x0020D9A0
	internal static string ReceivedDataSizeExceededMaximumClient
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ReceivedDataSizeExceededMaximumClient", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AFA RID: 6906
	// (get) Token: 0x060066B7 RID: 26295 RVA: 0x0020F7B6 File Offset: 0x0020D9B6
	internal static string ReceivedDataSizeExceededMaximumServer
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ReceivedDataSizeExceededMaximumServer", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AFB RID: 6907
	// (get) Token: 0x060066B8 RID: 26296 RVA: 0x0020F7CC File Offset: 0x0020D9CC
	internal static string ReceivedDataStreamIsNotStdout
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ReceivedDataStreamIsNotStdout", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AFC RID: 6908
	// (get) Token: 0x060066B9 RID: 26297 RVA: 0x0020F7E2 File Offset: 0x0020D9E2
	internal static string ReceivedObjectSizeExceededMaximumClient
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ReceivedObjectSizeExceededMaximumClient", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AFD RID: 6909
	// (get) Token: 0x060066BA RID: 26298 RVA: 0x0020F7F8 File Offset: 0x0020D9F8
	internal static string ReceivedObjectSizeExceededMaximumServer
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ReceivedObjectSizeExceededMaximumServer", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AFE RID: 6910
	// (get) Token: 0x060066BB RID: 26299 RVA: 0x0020F80E File Offset: 0x0020DA0E
	internal static string ReceivedUnsupportedAction
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ReceivedUnsupportedAction", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001AFF RID: 6911
	// (get) Token: 0x060066BC RID: 26300 RVA: 0x0020F824 File Offset: 0x0020DA24
	internal static string ReceivedUnsupportedDataType
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ReceivedUnsupportedDataType", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B00 RID: 6912
	// (get) Token: 0x060066BD RID: 26301 RVA: 0x0020F83A File Offset: 0x0020DA3A
	internal static string ReceivedUnsupportedRemoteHostCall
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ReceivedUnsupportedRemoteHostCall", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B01 RID: 6913
	// (get) Token: 0x060066BE RID: 26302 RVA: 0x0020F850 File Offset: 0x0020DA50
	internal static string ReceivedUnsupportedRemotingTargetInterfaceType
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ReceivedUnsupportedRemotingTargetInterfaceType", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B02 RID: 6914
	// (get) Token: 0x060066BF RID: 26303 RVA: 0x0020F866 File Offset: 0x0020DA66
	internal static string ReceiveExCallBackError
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ReceiveExCallBackError", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B03 RID: 6915
	// (get) Token: 0x060066C0 RID: 26304 RVA: 0x0020F87C File Offset: 0x0020DA7C
	internal static string ReceiveExFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ReceiveExFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B04 RID: 6916
	// (get) Token: 0x060066C1 RID: 26305 RVA: 0x0020F892 File Offset: 0x0020DA92
	internal static string ReceivePSSessionInDebugMode
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ReceivePSSessionInDebugMode", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B05 RID: 6917
	// (get) Token: 0x060066C2 RID: 26306 RVA: 0x0020F8A8 File Offset: 0x0020DAA8
	internal static string ReconnectShellCommandExCallBackError
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ReconnectShellCommandExCallBackError", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B06 RID: 6918
	// (get) Token: 0x060066C3 RID: 26307 RVA: 0x0020F8BE File Offset: 0x0020DABE
	internal static string ReconnectShellExCallBackErrr
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ReconnectShellExCallBackErrr", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B07 RID: 6919
	// (get) Token: 0x060066C4 RID: 26308 RVA: 0x0020F8D4 File Offset: 0x0020DAD4
	internal static string ReconnectShellExFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ReconnectShellExFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B08 RID: 6920
	// (get) Token: 0x060066C5 RID: 26309 RVA: 0x0020F8EA File Offset: 0x0020DAEA
	internal static string RedirectedURINotWellFormatted
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RedirectedURINotWellFormatted", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B09 RID: 6921
	// (get) Token: 0x060066C6 RID: 26310 RVA: 0x0020F900 File Offset: 0x0020DB00
	internal static string RelativeUriForRunspacePathNotSupported
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RelativeUriForRunspacePathNotSupported", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B0A RID: 6922
	// (get) Token: 0x060066C7 RID: 26311 RVA: 0x0020F916 File Offset: 0x0020DB16
	internal static string RemoteDebuggingEndpointVersionError
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteDebuggingEndpointVersionError", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B0B RID: 6923
	// (get) Token: 0x060066C8 RID: 26312 RVA: 0x0020F92C File Offset: 0x0020DB2C
	internal static string RemoteHostCallFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteHostCallFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B0C RID: 6924
	// (get) Token: 0x060066C9 RID: 26313 RVA: 0x0020F942 File Offset: 0x0020DB42
	internal static string RemoteHostDataDecodingNotSupported
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteHostDataDecodingNotSupported", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B0D RID: 6925
	// (get) Token: 0x060066CA RID: 26314 RVA: 0x0020F958 File Offset: 0x0020DB58
	internal static string RemoteHostDataEncodingNotSupported
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteHostDataEncodingNotSupported", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B0E RID: 6926
	// (get) Token: 0x060066CB RID: 26315 RVA: 0x0020F96E File Offset: 0x0020DB6E
	internal static string RemoteHostDecodingFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteHostDecodingFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B0F RID: 6927
	// (get) Token: 0x060066CC RID: 26316 RVA: 0x0020F984 File Offset: 0x0020DB84
	internal static string RemoteHostDoesNotSupportPushRunspace
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteHostDoesNotSupportPushRunspace", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B10 RID: 6928
	// (get) Token: 0x060066CD RID: 26317 RVA: 0x0020F99A File Offset: 0x0020DB9A
	internal static string RemoteHostGetBufferContents
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteHostGetBufferContents", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B11 RID: 6929
	// (get) Token: 0x060066CE RID: 26318 RVA: 0x0020F9B0 File Offset: 0x0020DBB0
	internal static string RemoteHostMethodNotImplemented
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteHostMethodNotImplemented", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B12 RID: 6930
	// (get) Token: 0x060066CF RID: 26319 RVA: 0x0020F9C6 File Offset: 0x0020DBC6
	internal static string RemoteHostNullClientHost
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteHostNullClientHost", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B13 RID: 6931
	// (get) Token: 0x060066D0 RID: 26320 RVA: 0x0020F9DC File Offset: 0x0020DBDC
	internal static string RemoteHostPromptForCredentialModifiedCaption
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteHostPromptForCredentialModifiedCaption", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B14 RID: 6932
	// (get) Token: 0x060066D1 RID: 26321 RVA: 0x0020F9F2 File Offset: 0x0020DBF2
	internal static string RemoteHostPromptForCredentialModifiedMessage
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteHostPromptForCredentialModifiedMessage", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B15 RID: 6933
	// (get) Token: 0x060066D2 RID: 26322 RVA: 0x0020FA08 File Offset: 0x0020DC08
	internal static string RemoteHostPromptSecureStringPrompt
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteHostPromptSecureStringPrompt", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B16 RID: 6934
	// (get) Token: 0x060066D3 RID: 26323 RVA: 0x0020FA1E File Offset: 0x0020DC1E
	internal static string RemoteHostReadLineAsSecureStringPrompt
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteHostReadLineAsSecureStringPrompt", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B17 RID: 6935
	// (get) Token: 0x060066D4 RID: 26324 RVA: 0x0020FA34 File Offset: 0x0020DC34
	internal static string RemoteRunspaceClosed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteRunspaceClosed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B18 RID: 6936
	// (get) Token: 0x060066D5 RID: 26325 RVA: 0x0020FA4A File Offset: 0x0020DC4A
	internal static string RemoteRunspaceDoesNotSupportPushRunspace
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteRunspaceDoesNotSupportPushRunspace", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B19 RID: 6937
	// (get) Token: 0x060066D6 RID: 26326 RVA: 0x0020FA60 File Offset: 0x0020DC60
	internal static string RemoteRunspaceHasMultipleMatchesForSpecifiedName
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteRunspaceHasMultipleMatchesForSpecifiedName", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B1A RID: 6938
	// (get) Token: 0x060066D7 RID: 26327 RVA: 0x0020FA76 File Offset: 0x0020DC76
	internal static string RemoteRunspaceHasMultipleMatchesForSpecifiedRunspaceId
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteRunspaceHasMultipleMatchesForSpecifiedRunspaceId", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B1B RID: 6939
	// (get) Token: 0x060066D8 RID: 26328 RVA: 0x0020FA8C File Offset: 0x0020DC8C
	internal static string RemoteRunspaceHasMultipleMatchesForSpecifiedSessionId
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteRunspaceHasMultipleMatchesForSpecifiedSessionId", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B1C RID: 6940
	// (get) Token: 0x060066D9 RID: 26329 RVA: 0x0020FAA2 File Offset: 0x0020DCA2
	internal static string RemoteRunspaceInfoHasDuplicates
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteRunspaceInfoHasDuplicates", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B1D RID: 6941
	// (get) Token: 0x060066DA RID: 26330 RVA: 0x0020FAB8 File Offset: 0x0020DCB8
	internal static string RemoteRunspaceInfoLimitExceeded
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteRunspaceInfoLimitExceeded", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B1E RID: 6942
	// (get) Token: 0x060066DB RID: 26331 RVA: 0x0020FACE File Offset: 0x0020DCCE
	internal static string RemoteRunspaceNotAvailableForSpecifiedComputer
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteRunspaceNotAvailableForSpecifiedComputer", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B1F RID: 6943
	// (get) Token: 0x060066DC RID: 26332 RVA: 0x0020FAE4 File Offset: 0x0020DCE4
	internal static string RemoteRunspaceNotAvailableForSpecifiedName
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteRunspaceNotAvailableForSpecifiedName", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B20 RID: 6944
	// (get) Token: 0x060066DD RID: 26333 RVA: 0x0020FAFA File Offset: 0x0020DCFA
	internal static string RemoteRunspaceNotAvailableForSpecifiedRunspaceId
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteRunspaceNotAvailableForSpecifiedRunspaceId", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B21 RID: 6945
	// (get) Token: 0x060066DE RID: 26334 RVA: 0x0020FB10 File Offset: 0x0020DD10
	internal static string RemoteRunspaceNotAvailableForSpecifiedSessionId
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteRunspaceNotAvailableForSpecifiedSessionId", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B22 RID: 6946
	// (get) Token: 0x060066DF RID: 26335 RVA: 0x0020FB26 File Offset: 0x0020DD26
	internal static string RemoteRunspaceOpenFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteRunspaceOpenFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B23 RID: 6947
	// (get) Token: 0x060066E0 RID: 26336 RVA: 0x0020FB3C File Offset: 0x0020DD3C
	internal static string RemoteRunspaceOpenUnknownState
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteRunspaceOpenUnknownState", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B24 RID: 6948
	// (get) Token: 0x060066E1 RID: 26337 RVA: 0x0020FB52 File Offset: 0x0020DD52
	internal static string RemoteSessionHyperVSocketServerConstructorFailure
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteSessionHyperVSocketServerConstructorFailure", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B25 RID: 6949
	// (get) Token: 0x060066E2 RID: 26338 RVA: 0x0020FB68 File Offset: 0x0020DD68
	internal static string RemoteTransportError
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoteTransportError", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B26 RID: 6950
	// (get) Token: 0x060066E3 RID: 26339 RVA: 0x0020FB7E File Offset: 0x0020DD7E
	internal static string RemotingDestinationNotForMe
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemotingDestinationNotForMe", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B27 RID: 6951
	// (get) Token: 0x060066E4 RID: 26340 RVA: 0x0020FB94 File Offset: 0x0020DD94
	internal static string RemovePSJobWhatIfTarget
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemovePSJobWhatIfTarget", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B28 RID: 6952
	// (get) Token: 0x060066E5 RID: 26341 RVA: 0x0020FBAA File Offset: 0x0020DDAA
	internal static string RemoveRunspaceNotConnected
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RemoveRunspaceNotConnected", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B29 RID: 6953
	// (get) Token: 0x060066E6 RID: 26342 RVA: 0x0020FBC0 File Offset: 0x0020DDC0
	internal static string ResponsePromptIdCannotBeFound
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ResponsePromptIdCannotBeFound", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B2A RID: 6954
	// (get) Token: 0x060066E7 RID: 26343 RVA: 0x0020FBD6 File Offset: 0x0020DDD6
	internal static string RestartWinRMMessage
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RestartWinRMMessage", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B2B RID: 6955
	// (get) Token: 0x060066E8 RID: 26344 RVA: 0x0020FBEC File Offset: 0x0020DDEC
	internal static string RestartWSManRequiredShowUI
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RestartWSManRequiredShowUI", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B2C RID: 6956
	// (get) Token: 0x060066E9 RID: 26345 RVA: 0x0020FC02 File Offset: 0x0020DE02
	internal static string RestartWSManServiceAction
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RestartWSManServiceAction", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B2D RID: 6957
	// (get) Token: 0x060066EA RID: 26346 RVA: 0x0020FC18 File Offset: 0x0020DE18
	internal static string RestartWSManServiceMessageV
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RestartWSManServiceMessageV", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B2E RID: 6958
	// (get) Token: 0x060066EB RID: 26347 RVA: 0x0020FC2E File Offset: 0x0020DE2E
	internal static string RestartWSManServiceTarget
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RestartWSManServiceTarget", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B2F RID: 6959
	// (get) Token: 0x060066EC RID: 26348 RVA: 0x0020FC44 File Offset: 0x0020DE44
	internal static string ResumeJobInvalidJobState
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ResumeJobInvalidJobState", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B30 RID: 6960
	// (get) Token: 0x060066ED RID: 26349 RVA: 0x0020FC5A File Offset: 0x0020DE5A
	internal static string RunAsSessionConfigurationSecurityWarning
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RunAsSessionConfigurationSecurityWarning", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B31 RID: 6961
	// (get) Token: 0x060066EE RID: 26350 RVA: 0x0020FC70 File Offset: 0x0020DE70
	internal static string RunShellCommandExCallBackError
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RunShellCommandExCallBackError", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B32 RID: 6962
	// (get) Token: 0x060066EF RID: 26351 RVA: 0x0020FC86 File Offset: 0x0020DE86
	internal static string RunShellCommandExFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RunShellCommandExFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B33 RID: 6963
	// (get) Token: 0x060066F0 RID: 26352 RVA: 0x0020FC9C File Offset: 0x0020DE9C
	internal static string RunspaceAlreadyExists
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RunspaceAlreadyExists", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B34 RID: 6964
	// (get) Token: 0x060066F1 RID: 26353 RVA: 0x0020FCB2 File Offset: 0x0020DEB2
	internal static string RunspaceCannotBeConnected
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RunspaceCannotBeConnected", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B35 RID: 6965
	// (get) Token: 0x060066F2 RID: 26354 RVA: 0x0020FCC8 File Offset: 0x0020DEC8
	internal static string RunspaceCannotBeDisconnected
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RunspaceCannotBeDisconnected", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B36 RID: 6966
	// (get) Token: 0x060066F3 RID: 26355 RVA: 0x0020FCDE File Offset: 0x0020DEDE
	internal static string RunspaceCannotBeFound
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RunspaceCannotBeFound", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B37 RID: 6967
	// (get) Token: 0x060066F4 RID: 26356 RVA: 0x0020FCF4 File Offset: 0x0020DEF4
	internal static string RunspaceConnectFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RunspaceConnectFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B38 RID: 6968
	// (get) Token: 0x060066F5 RID: 26357 RVA: 0x0020FD0A File Offset: 0x0020DF0A
	internal static string RunspaceConnectFailedWithMessage
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RunspaceConnectFailedWithMessage", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B39 RID: 6969
	// (get) Token: 0x060066F6 RID: 26358 RVA: 0x0020FD20 File Offset: 0x0020DF20
	internal static string RunspaceDisconnectFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RunspaceDisconnectFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B3A RID: 6970
	// (get) Token: 0x060066F7 RID: 26359 RVA: 0x0020FD36 File Offset: 0x0020DF36
	internal static string RunspaceDisconnectFailedWithReason
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RunspaceDisconnectFailedWithReason", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B3B RID: 6971
	// (get) Token: 0x060066F8 RID: 26360 RVA: 0x0020FD4C File Offset: 0x0020DF4C
	internal static string RunspaceIdsDoNotMatch
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RunspaceIdsDoNotMatch", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B3C RID: 6972
	// (get) Token: 0x060066F9 RID: 26361 RVA: 0x0020FD62 File Offset: 0x0020DF62
	internal static string RunspaceParamNotSupported
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RunspaceParamNotSupported", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B3D RID: 6973
	// (get) Token: 0x060066FA RID: 26362 RVA: 0x0020FD78 File Offset: 0x0020DF78
	internal static string RunspaceQueryFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("RunspaceQueryFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B3E RID: 6974
	// (get) Token: 0x060066FB RID: 26363 RVA: 0x0020FD8E File Offset: 0x0020DF8E
	internal static string ScsScriptMessageV
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ScsScriptMessageV", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B3F RID: 6975
	// (get) Token: 0x060066FC RID: 26364 RVA: 0x0020FDA4 File Offset: 0x0020DFA4
	internal static string ScsShouldProcessTargetSDDL
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ScsShouldProcessTargetSDDL", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B40 RID: 6976
	// (get) Token: 0x060066FD RID: 26365 RVA: 0x0020FDBA File Offset: 0x0020DFBA
	internal static string SendExCallBackError
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("SendExCallBackError", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B41 RID: 6977
	// (get) Token: 0x060066FE RID: 26366 RVA: 0x0020FDD0 File Offset: 0x0020DFD0
	internal static string SendExFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("SendExFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B42 RID: 6978
	// (get) Token: 0x060066FF RID: 26367 RVA: 0x0020FDE6 File Offset: 0x0020DFE6
	internal static string ServerConnectFailedOnInputValidation
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ServerConnectFailedOnInputValidation", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B43 RID: 6979
	// (get) Token: 0x06006700 RID: 26368 RVA: 0x0020FDFC File Offset: 0x0020DFFC
	internal static string ServerConnectFailedOnMismatchedRunspacePoolProperties
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ServerConnectFailedOnMismatchedRunspacePoolProperties", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B44 RID: 6980
	// (get) Token: 0x06006701 RID: 26369 RVA: 0x0020FE12 File Offset: 0x0020E012
	internal static string ServerConnectFailedOnNegotiation
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ServerConnectFailedOnNegotiation", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B45 RID: 6981
	// (get) Token: 0x06006702 RID: 26370 RVA: 0x0020FE28 File Offset: 0x0020E028
	internal static string ServerConnectFailedOnServerStateValidation
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ServerConnectFailedOnServerStateValidation", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B46 RID: 6982
	// (get) Token: 0x06006703 RID: 26371 RVA: 0x0020FE3E File Offset: 0x0020E03E
	internal static string ServerDriverRemoteHostAlreadyPushed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ServerDriverRemoteHostAlreadyPushed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B47 RID: 6983
	// (get) Token: 0x06006704 RID: 26372 RVA: 0x0020FE54 File Offset: 0x0020E054
	internal static string ServerDriverRemoteHostNoDebuggerToPush
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ServerDriverRemoteHostNoDebuggerToPush", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B48 RID: 6984
	// (get) Token: 0x06006705 RID: 26373 RVA: 0x0020FE6A File Offset: 0x0020E06A
	internal static string ServerDriverRemoteHostNotRemoteRunspace
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ServerDriverRemoteHostNotRemoteRunspace", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B49 RID: 6985
	// (get) Token: 0x06006706 RID: 26374 RVA: 0x0020FE80 File Offset: 0x0020E080
	internal static string ServerKeyExchangeFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ServerKeyExchangeFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B4A RID: 6986
	// (get) Token: 0x06006707 RID: 26375 RVA: 0x0020FE96 File Offset: 0x0020E096
	internal static string ServerNegotiationFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ServerNegotiationFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B4B RID: 6987
	// (get) Token: 0x06006708 RID: 26376 RVA: 0x0020FEAC File Offset: 0x0020E0AC
	internal static string ServerNegotiationTimeout
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ServerNegotiationTimeout", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B4C RID: 6988
	// (get) Token: 0x06006709 RID: 26377 RVA: 0x0020FEC2 File Offset: 0x0020E0C2
	internal static string ServerNotFoundCapabilityProperties
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ServerNotFoundCapabilityProperties", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B4D RID: 6989
	// (get) Token: 0x0600670A RID: 26378 RVA: 0x0020FED8 File Offset: 0x0020E0D8
	internal static string ServerProcessExited
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ServerProcessExited", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B4E RID: 6990
	// (get) Token: 0x0600670B RID: 26379 RVA: 0x0020FEEE File Offset: 0x0020E0EE
	internal static string ServerRequestedToCloseSession
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ServerRequestedToCloseSession", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B4F RID: 6991
	// (get) Token: 0x0600670C RID: 26380 RVA: 0x0020FF04 File Offset: 0x0020E104
	internal static string ServerSideNestedCommandInvokeFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ServerSideNestedCommandInvokeFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B50 RID: 6992
	// (get) Token: 0x0600670D RID: 26381 RVA: 0x0020FF1A File Offset: 0x0020E11A
	internal static string SessionConfigurationMustBeFileBased
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("SessionConfigurationMustBeFileBased", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B51 RID: 6993
	// (get) Token: 0x0600670E RID: 26382 RVA: 0x0020FF30 File Offset: 0x0020E130
	internal static string SessionConnectFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("SessionConnectFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B52 RID: 6994
	// (get) Token: 0x0600670F RID: 26383 RVA: 0x0020FF46 File Offset: 0x0020E146
	internal static string SessionIdMatchFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("SessionIdMatchFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B53 RID: 6995
	// (get) Token: 0x06006710 RID: 26384 RVA: 0x0020FF5C File Offset: 0x0020E15C
	internal static string SessionNameMatchFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("SessionNameMatchFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B54 RID: 6996
	// (get) Token: 0x06006711 RID: 26385 RVA: 0x0020FF72 File Offset: 0x0020E172
	internal static string SessionNameWithoutInvokeDisconnected
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("SessionNameWithoutInvokeDisconnected", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B55 RID: 6997
	// (get) Token: 0x06006712 RID: 26386 RVA: 0x0020FF88 File Offset: 0x0020E188
	internal static string SessionNotAvailableForConnection
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("SessionNotAvailableForConnection", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B56 RID: 6998
	// (get) Token: 0x06006713 RID: 26387 RVA: 0x0020FF9E File Offset: 0x0020E19E
	internal static string SetEnabledFalseTarget
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("SetEnabledFalseTarget", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B57 RID: 6999
	// (get) Token: 0x06006714 RID: 26388 RVA: 0x0020FFB4 File Offset: 0x0020E1B4
	internal static string SetEnabledTrueTarget
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("SetEnabledTrueTarget", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B58 RID: 7000
	// (get) Token: 0x06006715 RID: 26389 RVA: 0x0020FFCA File Offset: 0x0020E1CA
	internal static string ShowUIAndSDDLCannotExist
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ShowUIAndSDDLCannotExist", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B59 RID: 7001
	// (get) Token: 0x06006716 RID: 26390 RVA: 0x0020FFE0 File Offset: 0x0020E1E0
	internal static string StartJobDefinitionNotFound1
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("StartJobDefinitionNotFound1", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B5A RID: 7002
	// (get) Token: 0x06006717 RID: 26391 RVA: 0x0020FFF6 File Offset: 0x0020E1F6
	internal static string StartJobDefinitionNotFound2
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("StartJobDefinitionNotFound2", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B5B RID: 7003
	// (get) Token: 0x06006718 RID: 26392 RVA: 0x0021000C File Offset: 0x0020E20C
	internal static string StartJobDefinitionPathInvalidNotFSProvider
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("StartJobDefinitionPathInvalidNotFSProvider", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B5C RID: 7004
	// (get) Token: 0x06006719 RID: 26393 RVA: 0x00210022 File Offset: 0x0020E222
	internal static string StartJobDefinitionPathInvalidNotSingle
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("StartJobDefinitionPathInvalidNotSingle", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B5D RID: 7005
	// (get) Token: 0x0600671A RID: 26394 RVA: 0x00210038 File Offset: 0x0020E238
	internal static string StartJobManyDefNameMatches
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("StartJobManyDefNameMatches", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B5E RID: 7006
	// (get) Token: 0x0600671B RID: 26395 RVA: 0x0021004E File Offset: 0x0020E24E
	internal static string StartupScriptNotCorrect
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("StartupScriptNotCorrect", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B5F RID: 7007
	// (get) Token: 0x0600671C RID: 26396 RVA: 0x00210064 File Offset: 0x0020E264
	internal static string StartupScriptThrewTerminatingError
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("StartupScriptThrewTerminatingError", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B60 RID: 7008
	// (get) Token: 0x0600671D RID: 26397 RVA: 0x0021007A File Offset: 0x0020E27A
	internal static string StdInCannotBeSetToNoWait
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("StdInCannotBeSetToNoWait", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B61 RID: 7009
	// (get) Token: 0x0600671E RID: 26398 RVA: 0x00210090 File Offset: 0x0020E290
	internal static string StdInIsNotOpen
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("StdInIsNotOpen", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B62 RID: 7010
	// (get) Token: 0x0600671F RID: 26399 RVA: 0x002100A6 File Offset: 0x0020E2A6
	internal static string StopCommandOnRetry
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("StopCommandOnRetry", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B63 RID: 7011
	// (get) Token: 0x06006720 RID: 26400 RVA: 0x002100BC File Offset: 0x0020E2BC
	internal static string StopJobNotConnected
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("StopJobNotConnected", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B64 RID: 7012
	// (get) Token: 0x06006721 RID: 26401 RVA: 0x002100D2 File Offset: 0x0020E2D2
	internal static string StopPSJobWhatIfTarget
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("StopPSJobWhatIfTarget", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B65 RID: 7013
	// (get) Token: 0x06006722 RID: 26402 RVA: 0x002100E8 File Offset: 0x0020E2E8
	internal static string SuspendJobInvalidJobState
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("SuspendJobInvalidJobState", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B66 RID: 7014
	// (get) Token: 0x06006723 RID: 26403 RVA: 0x002100FE File Offset: 0x0020E2FE
	internal static string ThrottlingJobChildAddedAfterEndOfChildJobs
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ThrottlingJobChildAddedAfterEndOfChildJobs", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B67 RID: 7015
	// (get) Token: 0x06006724 RID: 26404 RVA: 0x00210114 File Offset: 0x0020E314
	internal static string ThrottlingJobChildAlreadyRunning
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ThrottlingJobChildAlreadyRunning", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B68 RID: 7016
	// (get) Token: 0x06006725 RID: 26405 RVA: 0x0021012A File Offset: 0x0020E32A
	internal static string ThrottlingJobFlowControlMemoryWarning
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ThrottlingJobFlowControlMemoryWarning", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B69 RID: 7017
	// (get) Token: 0x06006726 RID: 26406 RVA: 0x00210140 File Offset: 0x0020E340
	internal static string ThrottlingJobStatusMessage
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("ThrottlingJobStatusMessage", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B6A RID: 7018
	// (get) Token: 0x06006727 RID: 26407 RVA: 0x00210156 File Offset: 0x0020E356
	internal static string TroubleShootingHelpTopic
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("TroubleShootingHelpTopic", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B6B RID: 7019
	// (get) Token: 0x06006728 RID: 26408 RVA: 0x0021016C File Offset: 0x0020E36C
	internal static string TypeNeedsAssembly
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("TypeNeedsAssembly", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B6C RID: 7020
	// (get) Token: 0x06006729 RID: 26409 RVA: 0x00210182 File Offset: 0x0020E382
	internal static string UnableToLoadAssembly
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("UnableToLoadAssembly", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B6D RID: 7021
	// (get) Token: 0x0600672A RID: 26410 RVA: 0x00210198 File Offset: 0x0020E398
	internal static string UnableToLoadType
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("UnableToLoadType", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B6E RID: 7022
	// (get) Token: 0x0600672B RID: 26411 RVA: 0x002101AE File Offset: 0x0020E3AE
	internal static string UnknownTargetClass
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("UnknownTargetClass", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B6F RID: 7023
	// (get) Token: 0x0600672C RID: 26412 RVA: 0x002101C4 File Offset: 0x0020E3C4
	internal static string UnsupportedWaitHandleType
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("UnsupportedWaitHandleType", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B70 RID: 7024
	// (get) Token: 0x0600672D RID: 26413 RVA: 0x002101DA File Offset: 0x0020E3DA
	internal static string URIEndPointNotResolved
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("URIEndPointNotResolved", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B71 RID: 7025
	// (get) Token: 0x0600672E RID: 26414 RVA: 0x002101F0 File Offset: 0x0020E3F0
	internal static string URIRedirectionReported
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("URIRedirectionReported", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B72 RID: 7026
	// (get) Token: 0x0600672F RID: 26415 RVA: 0x00210206 File Offset: 0x0020E406
	internal static string URIRedirectWarningToHost
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("URIRedirectWarningToHost", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B73 RID: 7027
	// (get) Token: 0x06006730 RID: 26416 RVA: 0x0021021C File Offset: 0x0020E41C
	internal static string UriSpecifiedNotValid
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("UriSpecifiedNotValid", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B74 RID: 7028
	// (get) Token: 0x06006731 RID: 26417 RVA: 0x00210232 File Offset: 0x0020E432
	internal static string UseSharedProcessCannotBeFalseForWorkflowSessionType
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("UseSharedProcessCannotBeFalseForWorkflowSessionType", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B75 RID: 7029
	// (get) Token: 0x06006732 RID: 26418 RVA: 0x00210248 File Offset: 0x0020E448
	internal static string VMSessionConnectFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("VMSessionConnectFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B76 RID: 7030
	// (get) Token: 0x06006733 RID: 26419 RVA: 0x0021025E File Offset: 0x0020E45E
	internal static string WildCardErrorFilePathParameter
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WildCardErrorFilePathParameter", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B77 RID: 7031
	// (get) Token: 0x06006734 RID: 26420 RVA: 0x00210274 File Offset: 0x0020E474
	internal static string WinPERemotingNotSupported
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WinPERemotingNotSupported", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B78 RID: 7032
	// (get) Token: 0x06006735 RID: 26421 RVA: 0x0021028A File Offset: 0x0020E48A
	internal static string WinRMRequiresRestart
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WinRMRequiresRestart", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B79 RID: 7033
	// (get) Token: 0x06006736 RID: 26422 RVA: 0x002102A0 File Offset: 0x0020E4A0
	internal static string WinRMRestartWarning
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WinRMRestartWarning", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B7A RID: 7034
	// (get) Token: 0x06006737 RID: 26423 RVA: 0x002102B6 File Offset: 0x0020E4B6
	internal static string WriteEventsCannotBeUsedWithoutWait
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WriteEventsCannotBeUsedWithoutWait", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B7B RID: 7035
	// (get) Token: 0x06006738 RID: 26424 RVA: 0x002102CC File Offset: 0x0020E4CC
	internal static string WriteJobInResultsCannotBeUsedWithoutWait
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WriteJobInResultsCannotBeUsedWithoutWait", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B7C RID: 7036
	// (get) Token: 0x06006739 RID: 26425 RVA: 0x002102E2 File Offset: 0x0020E4E2
	internal static string WrongSessionOptionValue
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WrongSessionOptionValue", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B7D RID: 7037
	// (get) Token: 0x0600673A RID: 26426 RVA: 0x002102F8 File Offset: 0x0020E4F8
	internal static string WSManInitFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManInitFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B7E RID: 7038
	// (get) Token: 0x0600673B RID: 26427 RVA: 0x0021030E File Offset: 0x0020E50E
	internal static string WsmanMaxRedirectionCountVariableDescription
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WsmanMaxRedirectionCountVariableDescription", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B7F RID: 7039
	// (get) Token: 0x0600673C RID: 26428 RVA: 0x00210324 File Offset: 0x0020E524
	internal static string WSManPluginConnectNoNegotiationData
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManPluginConnectNoNegotiationData", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B80 RID: 7040
	// (get) Token: 0x0600673D RID: 26429 RVA: 0x0021033A File Offset: 0x0020E53A
	internal static string WSManPluginConnectOperationFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManPluginConnectOperationFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B81 RID: 7041
	// (get) Token: 0x0600673E RID: 26430 RVA: 0x00210350 File Offset: 0x0020E550
	internal static string WSManPluginContextNotFound
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManPluginContextNotFound", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B82 RID: 7042
	// (get) Token: 0x0600673F RID: 26431 RVA: 0x00210366 File Offset: 0x0020E566
	internal static string WSManPluginInvalidArgSet
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManPluginInvalidArgSet", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B83 RID: 7043
	// (get) Token: 0x06006740 RID: 26432 RVA: 0x0021037C File Offset: 0x0020E57C
	internal static string WSManPluginInvalidCommandContext
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManPluginInvalidCommandContext", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B84 RID: 7044
	// (get) Token: 0x06006741 RID: 26433 RVA: 0x00210392 File Offset: 0x0020E592
	internal static string WSManPluginInvalidInputDataType
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManPluginInvalidInputDataType", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B85 RID: 7045
	// (get) Token: 0x06006742 RID: 26434 RVA: 0x002103A8 File Offset: 0x0020E5A8
	internal static string WSManPluginInvalidInputStream
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManPluginInvalidInputStream", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B86 RID: 7046
	// (get) Token: 0x06006743 RID: 26435 RVA: 0x002103BE File Offset: 0x0020E5BE
	internal static string WSManPluginInvalidOutputStream
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManPluginInvalidOutputStream", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B87 RID: 7047
	// (get) Token: 0x06006744 RID: 26436 RVA: 0x002103D4 File Offset: 0x0020E5D4
	internal static string WSManPluginInvalidSenderDetails
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManPluginInvalidSenderDetails", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B88 RID: 7048
	// (get) Token: 0x06006745 RID: 26437 RVA: 0x002103EA File Offset: 0x0020E5EA
	internal static string WSManPluginInvalidShellContext
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManPluginInvalidShellContext", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B89 RID: 7049
	// (get) Token: 0x06006746 RID: 26438 RVA: 0x00210400 File Offset: 0x0020E600
	internal static string WSManPluginManagedException
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManPluginManagedException", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B8A RID: 7050
	// (get) Token: 0x06006747 RID: 26439 RVA: 0x00210416 File Offset: 0x0020E616
	internal static string WSManPluginNullInvalidInput
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManPluginNullInvalidInput", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B8B RID: 7051
	// (get) Token: 0x06006748 RID: 26440 RVA: 0x0021042C File Offset: 0x0020E62C
	internal static string WSManPluginNullInvalidStreamSet
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManPluginNullInvalidStreamSet", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B8C RID: 7052
	// (get) Token: 0x06006749 RID: 26441 RVA: 0x00210442 File Offset: 0x0020E642
	internal static string WSManPluginNullPluginContext
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManPluginNullPluginContext", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B8D RID: 7053
	// (get) Token: 0x0600674A RID: 26442 RVA: 0x00210458 File Offset: 0x0020E658
	internal static string WSManPluginNullShellContext
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManPluginNullShellContext", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B8E RID: 7054
	// (get) Token: 0x0600674B RID: 26443 RVA: 0x0021046E File Offset: 0x0020E66E
	internal static string WSManPluginOperationClose
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManPluginOperationClose", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B8F RID: 7055
	// (get) Token: 0x0600674C RID: 26444 RVA: 0x00210484 File Offset: 0x0020E684
	internal static string WSManPluginOptionNotUnderstood
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManPluginOptionNotUnderstood", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B90 RID: 7056
	// (get) Token: 0x0600674D RID: 26445 RVA: 0x0021049A File Offset: 0x0020E69A
	internal static string WSManPluginProtocolVersionNotFound
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManPluginProtocolVersionNotFound", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B91 RID: 7057
	// (get) Token: 0x0600674E RID: 26446 RVA: 0x002104B0 File Offset: 0x0020E6B0
	internal static string WSManPluginProtocolVersionNotMatch
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManPluginProtocolVersionNotMatch", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B92 RID: 7058
	// (get) Token: 0x0600674F RID: 26447 RVA: 0x002104C6 File Offset: 0x0020E6C6
	internal static string WSManPluginReportContextFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManPluginReportContextFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B93 RID: 7059
	// (get) Token: 0x06006750 RID: 26448 RVA: 0x002104DC File Offset: 0x0020E6DC
	internal static string WSManPluginSessionCreationFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManPluginSessionCreationFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x17001B94 RID: 7060
	// (get) Token: 0x06006751 RID: 26449 RVA: 0x002104F2 File Offset: 0x0020E6F2
	internal static string WSManPluginShutdownRegistrationFailed
	{
		get
		{
			return RemotingErrorIdStrings.ResourceManager.GetString("WSManPluginShutdownRegistrationFailed", RemotingErrorIdStrings.resourceCulture);
		}
	}

	// Token: 0x04003261 RID: 12897
	private static ResourceManager resourceMan;

	// Token: 0x04003262 RID: 12898
	private static CultureInfo resourceCulture;
}
