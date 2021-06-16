using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A25 RID: 2597
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
internal class RunspaceStrings
{
	// Token: 0x06006387 RID: 25479 RVA: 0x0020B1AE File Offset: 0x002093AE
	internal RunspaceStrings()
	{
	}

	// Token: 0x170017EC RID: 6124
	// (get) Token: 0x06006388 RID: 25480 RVA: 0x0020B1B8 File Offset: 0x002093B8
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(RunspaceStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("RunspaceStrings", typeof(RunspaceStrings).Assembly);
				RunspaceStrings.resourceMan = resourceManager;
			}
			return RunspaceStrings.resourceMan;
		}
	}

	// Token: 0x170017ED RID: 6125
	// (get) Token: 0x06006389 RID: 25481 RVA: 0x0020B1F7 File Offset: 0x002093F7
	// (set) Token: 0x0600638A RID: 25482 RVA: 0x0020B1FE File Offset: 0x002093FE
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return RunspaceStrings.resourceCulture;
		}
		set
		{
			RunspaceStrings.resourceCulture = value;
		}
	}

	// Token: 0x170017EE RID: 6126
	// (get) Token: 0x0600638B RID: 25483 RVA: 0x0020B206 File Offset: 0x00209406
	internal static string AnotherSessionStateProxyInProgress
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("AnotherSessionStateProxyInProgress", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x170017EF RID: 6127
	// (get) Token: 0x0600638C RID: 25484 RVA: 0x0020B21C File Offset: 0x0020941C
	internal static string CannotConnect
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("CannotConnect", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x170017F0 RID: 6128
	// (get) Token: 0x0600638D RID: 25485 RVA: 0x0020B232 File Offset: 0x00209432
	internal static string CannotOpenAgain
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("CannotOpenAgain", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x170017F1 RID: 6129
	// (get) Token: 0x0600638E RID: 25486 RVA: 0x0020B248 File Offset: 0x00209448
	internal static string ChangePropertyAfterOpen
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("ChangePropertyAfterOpen", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x170017F2 RID: 6130
	// (get) Token: 0x0600638F RID: 25487 RVA: 0x0020B25E File Offset: 0x0020945E
	internal static string CmdletNotFoundWhileLoadingModulesOnRunspaceOpen
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("CmdletNotFoundWhileLoadingModulesOnRunspaceOpen", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x170017F3 RID: 6131
	// (get) Token: 0x06006390 RID: 25488 RVA: 0x0020B274 File Offset: 0x00209474
	internal static string ConcurrentInvokeNotAllowed
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("ConcurrentInvokeNotAllowed", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x170017F4 RID: 6132
	// (get) Token: 0x06006391 RID: 25489 RVA: 0x0020B28A File Offset: 0x0020948A
	internal static string ConnectNotSupported
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("ConnectNotSupported", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x170017F5 RID: 6133
	// (get) Token: 0x06006392 RID: 25490 RVA: 0x0020B2A0 File Offset: 0x002094A0
	internal static string DebugRedirectionNotSupported
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("DebugRedirectionNotSupported", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x170017F6 RID: 6134
	// (get) Token: 0x06006393 RID: 25491 RVA: 0x0020B2B6 File Offset: 0x002094B6
	internal static string DisconnectConnectNotSupported
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("DisconnectConnectNotSupported", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x170017F7 RID: 6135
	// (get) Token: 0x06006394 RID: 25492 RVA: 0x0020B2CC File Offset: 0x002094CC
	internal static string DisconnectNotSupported
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("DisconnectNotSupported", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x170017F8 RID: 6136
	// (get) Token: 0x06006395 RID: 25493 RVA: 0x0020B2E2 File Offset: 0x002094E2
	internal static string DisconnectNotSupportedOnServer
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("DisconnectNotSupportedOnServer", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x170017F9 RID: 6137
	// (get) Token: 0x06006396 RID: 25494 RVA: 0x0020B2F8 File Offset: 0x002094F8
	internal static string ErrorLoadingModulesOnRunspaceOpen
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("ErrorLoadingModulesOnRunspaceOpen", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x170017FA RID: 6138
	// (get) Token: 0x06006397 RID: 25495 RVA: 0x0020B30E File Offset: 0x0020950E
	internal static string InformationRedirectionNotSupported
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("InformationRedirectionNotSupported", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x170017FB RID: 6139
	// (get) Token: 0x06006398 RID: 25496 RVA: 0x0020B324 File Offset: 0x00209524
	internal static string InvalidMyResultError
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("InvalidMyResultError", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x170017FC RID: 6140
	// (get) Token: 0x06006399 RID: 25497 RVA: 0x0020B33A File Offset: 0x0020953A
	internal static string InvalidPipelineStateStateGeneral
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("InvalidPipelineStateStateGeneral", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x170017FD RID: 6141
	// (get) Token: 0x0600639A RID: 25498 RVA: 0x0020B350 File Offset: 0x00209550
	internal static string InvalidRunspacePool
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("InvalidRunspacePool", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x170017FE RID: 6142
	// (get) Token: 0x0600639B RID: 25499 RVA: 0x0020B366 File Offset: 0x00209566
	internal static string InvalidRunspaceStateGeneral
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("InvalidRunspaceStateGeneral", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x170017FF RID: 6143
	// (get) Token: 0x0600639C RID: 25500 RVA: 0x0020B37C File Offset: 0x0020957C
	internal static string InvalidThreadOptionsChange
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("InvalidThreadOptionsChange", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x17001800 RID: 6144
	// (get) Token: 0x0600639D RID: 25501 RVA: 0x0020B392 File Offset: 0x00209592
	internal static string InvalidValueToResult
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("InvalidValueToResult", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x17001801 RID: 6145
	// (get) Token: 0x0600639E RID: 25502 RVA: 0x0020B3A8 File Offset: 0x002095A8
	internal static string InvalidValueToResultError
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("InvalidValueToResultError", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x17001802 RID: 6146
	// (get) Token: 0x0600639F RID: 25503 RVA: 0x0020B3BE File Offset: 0x002095BE
	internal static string NestedPipelineInvokeAsync
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("NestedPipelineInvokeAsync", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x17001803 RID: 6147
	// (get) Token: 0x060063A0 RID: 25504 RVA: 0x0020B3D4 File Offset: 0x002095D4
	internal static string NestedPipelineNoParentPipeline
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("NestedPipelineNoParentPipeline", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x17001804 RID: 6148
	// (get) Token: 0x060063A1 RID: 25505 RVA: 0x0020B3EA File Offset: 0x002095EA
	internal static string NoCommandInPipeline
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("NoCommandInPipeline", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x17001805 RID: 6149
	// (get) Token: 0x060063A2 RID: 25506 RVA: 0x0020B400 File Offset: 0x00209600
	internal static string NoDisconnectedCommand
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("NoDisconnectedCommand", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x17001806 RID: 6150
	// (get) Token: 0x060063A3 RID: 25507 RVA: 0x0020B416 File Offset: 0x00209616
	internal static string NoPipelineWhenSessionStateProxyInProgress
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("NoPipelineWhenSessionStateProxyInProgress", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x17001807 RID: 6151
	// (get) Token: 0x060063A4 RID: 25508 RVA: 0x0020B42C File Offset: 0x0020962C
	internal static string NoSessionStateProxyWhenPipelineInProgress
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("NoSessionStateProxyWhenPipelineInProgress", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x17001808 RID: 6152
	// (get) Token: 0x060063A5 RID: 25509 RVA: 0x0020B442 File Offset: 0x00209642
	internal static string NotSupportedOnRestrictedRunspace
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("NotSupportedOnRestrictedRunspace", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x17001809 RID: 6153
	// (get) Token: 0x060063A6 RID: 25510 RVA: 0x0020B458 File Offset: 0x00209658
	internal static string ParameterNameOrValueNeeded
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("ParameterNameOrValueNeeded", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x1700180A RID: 6154
	// (get) Token: 0x060063A7 RID: 25511 RVA: 0x0020B46E File Offset: 0x0020966E
	internal static string PipelineReInvokeNotAllowed
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("PipelineReInvokeNotAllowed", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x1700180B RID: 6155
	// (get) Token: 0x060063A8 RID: 25512 RVA: 0x0020B484 File Offset: 0x00209684
	internal static string RunningCmdDebugStop
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("RunningCmdDebugStop", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x1700180C RID: 6156
	// (get) Token: 0x060063A9 RID: 25513 RVA: 0x0020B49A File Offset: 0x0020969A
	internal static string RunningCmdWithJob
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("RunningCmdWithJob", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x1700180D RID: 6157
	// (get) Token: 0x060063AA RID: 25514 RVA: 0x0020B4B0 File Offset: 0x002096B0
	internal static string RunningCmdWithoutJob
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("RunningCmdWithoutJob", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x1700180E RID: 6158
	// (get) Token: 0x060063AB RID: 25515 RVA: 0x0020B4C6 File Offset: 0x002096C6
	internal static string RunspaceCloseInvalidWhileSessionStateProxy
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("RunspaceCloseInvalidWhileSessionStateProxy", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x1700180F RID: 6159
	// (get) Token: 0x060063AC RID: 25516 RVA: 0x0020B4DC File Offset: 0x002096DC
	internal static string RunspaceNotInOpenedState
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("RunspaceNotInOpenedState", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x17001810 RID: 6160
	// (get) Token: 0x060063AD RID: 25517 RVA: 0x0020B4F2 File Offset: 0x002096F2
	internal static string RunspaceNotLocal
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("RunspaceNotLocal", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x17001811 RID: 6161
	// (get) Token: 0x060063AE RID: 25518 RVA: 0x0020B508 File Offset: 0x00209708
	internal static string RunspaceNotOpenForPipeline
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("RunspaceNotOpenForPipeline", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x17001812 RID: 6162
	// (get) Token: 0x060063AF RID: 25519 RVA: 0x0020B51E File Offset: 0x0020971E
	internal static string RunspaceNotOpenForPipelineConnect
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("RunspaceNotOpenForPipelineConnect", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x17001813 RID: 6163
	// (get) Token: 0x060063B0 RID: 25520 RVA: 0x0020B534 File Offset: 0x00209734
	internal static string RunspaceNotReady
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("RunspaceNotReady", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x17001814 RID: 6164
	// (get) Token: 0x060063B1 RID: 25521 RVA: 0x0020B54A File Offset: 0x0020974A
	internal static string UseLocalScopeNotAllowed
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("UseLocalScopeNotAllowed", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x17001815 RID: 6165
	// (get) Token: 0x060063B2 RID: 25522 RVA: 0x0020B560 File Offset: 0x00209760
	internal static string VerboseRedirectionNotSupported
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("VerboseRedirectionNotSupported", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x17001816 RID: 6166
	// (get) Token: 0x060063B3 RID: 25523 RVA: 0x0020B576 File Offset: 0x00209776
	internal static string WarningRedirectionNotSupported
	{
		get
		{
			return RunspaceStrings.ResourceManager.GetString("WarningRedirectionNotSupported", RunspaceStrings.resourceCulture);
		}
	}

	// Token: 0x04003241 RID: 12865
	private static ResourceManager resourceMan;

	// Token: 0x04003242 RID: 12866
	private static CultureInfo resourceCulture;
}
