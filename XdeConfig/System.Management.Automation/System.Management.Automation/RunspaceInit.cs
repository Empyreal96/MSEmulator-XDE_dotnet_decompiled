using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A26 RID: 2598
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
internal class RunspaceInit
{
	// Token: 0x060063B4 RID: 25524 RVA: 0x0020B58C File Offset: 0x0020978C
	internal RunspaceInit()
	{
	}

	// Token: 0x17001817 RID: 6167
	// (get) Token: 0x060063B5 RID: 25525 RVA: 0x0020B594 File Offset: 0x00209794
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(RunspaceInit.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("RunspaceInit", typeof(RunspaceInit).Assembly);
				RunspaceInit.resourceMan = resourceManager;
			}
			return RunspaceInit.resourceMan;
		}
	}

	// Token: 0x17001818 RID: 6168
	// (get) Token: 0x060063B6 RID: 25526 RVA: 0x0020B5D3 File Offset: 0x002097D3
	// (set) Token: 0x060063B7 RID: 25527 RVA: 0x0020B5DA File Offset: 0x002097DA
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return RunspaceInit.resourceCulture;
		}
		set
		{
			RunspaceInit.resourceCulture = value;
		}
	}

	// Token: 0x17001819 RID: 6169
	// (get) Token: 0x060063B8 RID: 25528 RVA: 0x0020B5E2 File Offset: 0x002097E2
	internal static string ConfirmPreferenceDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("ConfirmPreferenceDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x1700181A RID: 6170
	// (get) Token: 0x060063B9 RID: 25529 RVA: 0x0020B5F8 File Offset: 0x002097F8
	internal static string ConsoleDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("ConsoleDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x1700181B RID: 6171
	// (get) Token: 0x060063BA RID: 25530 RVA: 0x0020B60E File Offset: 0x0020980E
	internal static string DebugPreferenceDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("DebugPreferenceDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x1700181C RID: 6172
	// (get) Token: 0x060063BB RID: 25531 RVA: 0x0020B624 File Offset: 0x00209824
	internal static string DollarHookDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("DollarHookDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x1700181D RID: 6173
	// (get) Token: 0x060063BC RID: 25532 RVA: 0x0020B63A File Offset: 0x0020983A
	internal static string DollarPSCultureDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("DollarPSCultureDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x1700181E RID: 6174
	// (get) Token: 0x060063BD RID: 25533 RVA: 0x0020B650 File Offset: 0x00209850
	internal static string DollarPSUICultureDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("DollarPSUICultureDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x1700181F RID: 6175
	// (get) Token: 0x060063BE RID: 25534 RVA: 0x0020B666 File Offset: 0x00209866
	internal static string ErrorActionPreferenceDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("ErrorActionPreferenceDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x17001820 RID: 6176
	// (get) Token: 0x060063BF RID: 25535 RVA: 0x0020B67C File Offset: 0x0020987C
	internal static string ErrorViewDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("ErrorViewDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x17001821 RID: 6177
	// (get) Token: 0x060063C0 RID: 25536 RVA: 0x0020B692 File Offset: 0x00209892
	internal static string ExecutionContextDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("ExecutionContextDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x17001822 RID: 6178
	// (get) Token: 0x060063C1 RID: 25537 RVA: 0x0020B6A8 File Offset: 0x002098A8
	internal static string FormatEnunmerationLimitDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("FormatEnunmerationLimitDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x17001823 RID: 6179
	// (get) Token: 0x060063C2 RID: 25538 RVA: 0x0020B6BE File Offset: 0x002098BE
	internal static string HOMEDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("HOMEDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x17001824 RID: 6180
	// (get) Token: 0x060063C3 RID: 25539 RVA: 0x0020B6D4 File Offset: 0x002098D4
	internal static string InformationPreferenceDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("InformationPreferenceDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x17001825 RID: 6181
	// (get) Token: 0x060063C4 RID: 25540 RVA: 0x0020B6EA File Offset: 0x002098EA
	internal static string MshShellIdDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("MshShellIdDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x17001826 RID: 6182
	// (get) Token: 0x060063C5 RID: 25541 RVA: 0x0020B700 File Offset: 0x00209900
	internal static string NestedPromptLevelDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("NestedPromptLevelDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x17001827 RID: 6183
	// (get) Token: 0x060063C6 RID: 25542 RVA: 0x0020B716 File Offset: 0x00209916
	internal static string OutputEncodingDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("OutputEncodingDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x17001828 RID: 6184
	// (get) Token: 0x060063C7 RID: 25543 RVA: 0x0020B72C File Offset: 0x0020992C
	internal static string PauseDefinitionString
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("PauseDefinitionString", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x17001829 RID: 6185
	// (get) Token: 0x060063C8 RID: 25544 RVA: 0x0020B742 File Offset: 0x00209942
	internal static string PIDDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("PIDDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x1700182A RID: 6186
	// (get) Token: 0x060063C9 RID: 25545 RVA: 0x0020B758 File Offset: 0x00209958
	internal static string PPIDDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("PPIDDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x1700182B RID: 6187
	// (get) Token: 0x060063CA RID: 25546 RVA: 0x0020B76E File Offset: 0x0020996E
	internal static string ProgressPreferenceDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("ProgressPreferenceDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x1700182C RID: 6188
	// (get) Token: 0x060063CB RID: 25547 RVA: 0x0020B784 File Offset: 0x00209984
	internal static string PSDefaultParameterValuesDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("PSDefaultParameterValuesDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x1700182D RID: 6189
	// (get) Token: 0x060063CC RID: 25548 RVA: 0x0020B79A File Offset: 0x0020999A
	internal static string PSEmailServerDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("PSEmailServerDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x1700182E RID: 6190
	// (get) Token: 0x060063CD RID: 25549 RVA: 0x0020B7B0 File Offset: 0x002099B0
	internal static string PSHOMEDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("PSHOMEDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x1700182F RID: 6191
	// (get) Token: 0x060063CE RID: 25550 RVA: 0x0020B7C6 File Offset: 0x002099C6
	internal static string PSHostDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("PSHostDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x17001830 RID: 6192
	// (get) Token: 0x060063CF RID: 25551 RVA: 0x0020B7DC File Offset: 0x002099DC
	internal static string PSVersionTableDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("PSVersionTableDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x17001831 RID: 6193
	// (get) Token: 0x060063D0 RID: 25552 RVA: 0x0020B7F2 File Offset: 0x002099F2
	internal static string ReportErrorShowExceptionClassDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("ReportErrorShowExceptionClassDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x17001832 RID: 6194
	// (get) Token: 0x060063D1 RID: 25553 RVA: 0x0020B808 File Offset: 0x00209A08
	internal static string ReportErrorShowInnerExceptionDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("ReportErrorShowInnerExceptionDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x17001833 RID: 6195
	// (get) Token: 0x060063D2 RID: 25554 RVA: 0x0020B81E File Offset: 0x00209A1E
	internal static string ReportErrorShowSourceDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("ReportErrorShowSourceDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x17001834 RID: 6196
	// (get) Token: 0x060063D3 RID: 25555 RVA: 0x0020B834 File Offset: 0x00209A34
	internal static string ReportErrorShowStackTraceDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("ReportErrorShowStackTraceDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x17001835 RID: 6197
	// (get) Token: 0x060063D4 RID: 25556 RVA: 0x0020B84A File Offset: 0x00209A4A
	internal static string VerbosePreferenceDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("VerbosePreferenceDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x17001836 RID: 6198
	// (get) Token: 0x060063D5 RID: 25557 RVA: 0x0020B860 File Offset: 0x00209A60
	internal static string WarningPreferenceDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("WarningPreferenceDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x17001837 RID: 6199
	// (get) Token: 0x060063D6 RID: 25558 RVA: 0x0020B876 File Offset: 0x00209A76
	internal static string WhatIfPreferenceDescription
	{
		get
		{
			return RunspaceInit.ResourceManager.GetString("WhatIfPreferenceDescription", RunspaceInit.resourceCulture);
		}
	}

	// Token: 0x04003243 RID: 12867
	private static ResourceManager resourceMan;

	// Token: 0x04003244 RID: 12868
	private static CultureInfo resourceCulture;
}
