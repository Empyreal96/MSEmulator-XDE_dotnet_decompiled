using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A1E RID: 2590
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class PowerShellStrings
{
	// Token: 0x06006338 RID: 25400 RVA: 0x0020AAEC File Offset: 0x00208CEC
	internal PowerShellStrings()
	{
	}

	// Token: 0x170017AB RID: 6059
	// (get) Token: 0x06006339 RID: 25401 RVA: 0x0020AAF4 File Offset: 0x00208CF4
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(PowerShellStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("PowerShellStrings", typeof(PowerShellStrings).Assembly);
				PowerShellStrings.resourceMan = resourceManager;
			}
			return PowerShellStrings.resourceMan;
		}
	}

	// Token: 0x170017AC RID: 6060
	// (get) Token: 0x0600633A RID: 25402 RVA: 0x0020AB33 File Offset: 0x00208D33
	// (set) Token: 0x0600633B RID: 25403 RVA: 0x0020AB3A File Offset: 0x00208D3A
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return PowerShellStrings.resourceCulture;
		}
		set
		{
			PowerShellStrings.resourceCulture = value;
		}
	}

	// Token: 0x170017AD RID: 6061
	// (get) Token: 0x0600633C RID: 25404 RVA: 0x0020AB42 File Offset: 0x00208D42
	internal static string ApartmentStateMismatch
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("ApartmentStateMismatch", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017AE RID: 6062
	// (get) Token: 0x0600633D RID: 25405 RVA: 0x0020AB58 File Offset: 0x00208D58
	internal static string ApartmentStateMismatchCurrentThread
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("ApartmentStateMismatchCurrentThread", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017AF RID: 6063
	// (get) Token: 0x0600633E RID: 25406 RVA: 0x0020AB6E File Offset: 0x00208D6E
	internal static string AsyncResultNotOwned
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("AsyncResultNotOwned", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017B0 RID: 6064
	// (get) Token: 0x0600633F RID: 25407 RVA: 0x0020AB84 File Offset: 0x00208D84
	internal static string CannotConnect
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("CannotConnect", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017B1 RID: 6065
	// (get) Token: 0x06006340 RID: 25408 RVA: 0x0020AB9A File Offset: 0x00208D9A
	internal static string CommandDoesNotWriteJob
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("CommandDoesNotWriteJob", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017B2 RID: 6066
	// (get) Token: 0x06006341 RID: 25409 RVA: 0x0020ABB0 File Offset: 0x00208DB0
	internal static string CommandInvokedFromWrongThreadWithCommand
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("CommandInvokedFromWrongThreadWithCommand", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017B3 RID: 6067
	// (get) Token: 0x06006342 RID: 25410 RVA: 0x0020ABC6 File Offset: 0x00208DC6
	internal static string CommandInvokedFromWrongThreadWithoutCommand
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("CommandInvokedFromWrongThreadWithoutCommand", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017B4 RID: 6068
	// (get) Token: 0x06006343 RID: 25411 RVA: 0x0020ABDC File Offset: 0x00208DDC
	internal static string ConnectFailed
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("ConnectFailed", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017B5 RID: 6069
	// (get) Token: 0x06006344 RID: 25412 RVA: 0x0020ABF2 File Offset: 0x00208DF2
	internal static string DiscOnSyncCommand
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("DiscOnSyncCommand", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017B6 RID: 6070
	// (get) Token: 0x06006345 RID: 25413 RVA: 0x0020AC08 File Offset: 0x00208E08
	internal static string ExecutionAlreadyStarted
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("ExecutionAlreadyStarted", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017B7 RID: 6071
	// (get) Token: 0x06006346 RID: 25414 RVA: 0x0020AC1E File Offset: 0x00208E1E
	internal static string ExecutionDisconnected
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("ExecutionDisconnected", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017B8 RID: 6072
	// (get) Token: 0x06006347 RID: 25415 RVA: 0x0020AC34 File Offset: 0x00208E34
	internal static string ExecutionStopping
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("ExecutionStopping", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017B9 RID: 6073
	// (get) Token: 0x06006348 RID: 25416 RVA: 0x0020AC4A File Offset: 0x00208E4A
	internal static string GetJobForCommandNotSupported
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("GetJobForCommandNotSupported", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017BA RID: 6074
	// (get) Token: 0x06006349 RID: 25417 RVA: 0x0020AC60 File Offset: 0x00208E60
	internal static string GetJobForCommandRequiresACommand
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("GetJobForCommandRequiresACommand", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017BB RID: 6075
	// (get) Token: 0x0600634A RID: 25418 RVA: 0x0020AC76 File Offset: 0x00208E76
	internal static string InvalidPowerShellStateGeneral
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("InvalidPowerShellStateGeneral", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017BC RID: 6076
	// (get) Token: 0x0600634B RID: 25419 RVA: 0x0020AC8C File Offset: 0x00208E8C
	internal static string InvalidRunspaceState
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("InvalidRunspaceState", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017BD RID: 6077
	// (get) Token: 0x0600634C RID: 25420 RVA: 0x0020ACA2 File Offset: 0x00208EA2
	internal static string InvalidStateCreateNested
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("InvalidStateCreateNested", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017BE RID: 6078
	// (get) Token: 0x0600634D RID: 25421 RVA: 0x0020ACB8 File Offset: 0x00208EB8
	internal static string IsDisconnected
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("IsDisconnected", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017BF RID: 6079
	// (get) Token: 0x0600634E RID: 25422 RVA: 0x0020ACCE File Offset: 0x00208ECE
	internal static string JobCanBeStartedOnce
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("JobCanBeStartedOnce", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017C0 RID: 6080
	// (get) Token: 0x0600634F RID: 25423 RVA: 0x0020ACE4 File Offset: 0x00208EE4
	internal static string JobCannotBeStartedWhenRunning
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("JobCannotBeStartedWhenRunning", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017C1 RID: 6081
	// (get) Token: 0x06006350 RID: 25424 RVA: 0x0020ACFA File Offset: 0x00208EFA
	internal static string JobObjectCanBeUsedOnce
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("JobObjectCanBeUsedOnce", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017C2 RID: 6082
	// (get) Token: 0x06006351 RID: 25425 RVA: 0x0020AD10 File Offset: 0x00208F10
	internal static string JobProxyAsJobMustBeTrue
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("JobProxyAsJobMustBeTrue", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017C3 RID: 6083
	// (get) Token: 0x06006352 RID: 25426 RVA: 0x0020AD26 File Offset: 0x00208F26
	internal static string JobProxyReceiveInvalid
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("JobProxyReceiveInvalid", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017C4 RID: 6084
	// (get) Token: 0x06006353 RID: 25427 RVA: 0x0020AD3C File Offset: 0x00208F3C
	internal static string KeyMustBeString
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("KeyMustBeString", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017C5 RID: 6085
	// (get) Token: 0x06006354 RID: 25428 RVA: 0x0020AD52 File Offset: 0x00208F52
	internal static string NestedPowerShellInvokeAsync
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("NestedPowerShellInvokeAsync", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017C6 RID: 6086
	// (get) Token: 0x06006355 RID: 25429 RVA: 0x0020AD68 File Offset: 0x00208F68
	internal static string NoCommandToInvoke
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("NoCommandToInvoke", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017C7 RID: 6087
	// (get) Token: 0x06006356 RID: 25430 RVA: 0x0020AD7E File Offset: 0x00208F7E
	internal static string NoDefaultRunspaceForPSCreate
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("NoDefaultRunspaceForPSCreate", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017C8 RID: 6088
	// (get) Token: 0x06006357 RID: 25431 RVA: 0x0020AD94 File Offset: 0x00208F94
	internal static string OnlyWorkflowInvocationSettingsSupported
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("OnlyWorkflowInvocationSettingsSupported", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017C9 RID: 6089
	// (get) Token: 0x06006358 RID: 25432 RVA: 0x0020ADAA File Offset: 0x00208FAA
	internal static string OperationNotSupportedForRemoting
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("OperationNotSupportedForRemoting", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017CA RID: 6090
	// (get) Token: 0x06006359 RID: 25433 RVA: 0x0020ADC0 File Offset: 0x00208FC0
	internal static string ParameterRequiresCommand
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("ParameterRequiresCommand", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017CB RID: 6091
	// (get) Token: 0x0600635A RID: 25434 RVA: 0x0020ADD6 File Offset: 0x00208FD6
	internal static string ProxyChildJobControlNotSupported
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("ProxyChildJobControlNotSupported", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017CC RID: 6092
	// (get) Token: 0x0600635B RID: 25435 RVA: 0x0020ADEC File Offset: 0x00208FEC
	internal static string ProxyJobControlNotSupported
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("ProxyJobControlNotSupported", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017CD RID: 6093
	// (get) Token: 0x0600635C RID: 25436 RVA: 0x0020AE02 File Offset: 0x00209002
	internal static string ProxyUnblockJobNotSupported
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("ProxyUnblockJobNotSupported", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017CE RID: 6094
	// (get) Token: 0x0600635D RID: 25437 RVA: 0x0020AE18 File Offset: 0x00209018
	internal static string RemoteRunspacePoolNotOpened
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("RemoteRunspacePoolNotOpened", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017CF RID: 6095
	// (get) Token: 0x0600635E RID: 25438 RVA: 0x0020AE2E File Offset: 0x0020902E
	internal static string RunspaceAndRunspacePoolNull
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("RunspaceAndRunspacePoolNull", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x170017D0 RID: 6096
	// (get) Token: 0x0600635F RID: 25439 RVA: 0x0020AE44 File Offset: 0x00209044
	internal static string UnblockNotSupported
	{
		get
		{
			return PowerShellStrings.ResourceManager.GetString("UnblockNotSupported", PowerShellStrings.resourceCulture);
		}
	}

	// Token: 0x04003233 RID: 12851
	private static ResourceManager resourceMan;

	// Token: 0x04003234 RID: 12852
	private static CultureInfo resourceCulture;
}
