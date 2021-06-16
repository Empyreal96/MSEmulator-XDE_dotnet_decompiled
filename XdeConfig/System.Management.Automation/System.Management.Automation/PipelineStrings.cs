using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A1D RID: 2589
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[CompilerGenerated]
[DebuggerNonUserCode]
internal class PipelineStrings
{
	// Token: 0x06006321 RID: 25377 RVA: 0x0020A8F2 File Offset: 0x00208AF2
	internal PipelineStrings()
	{
	}

	// Token: 0x17001796 RID: 6038
	// (get) Token: 0x06006322 RID: 25378 RVA: 0x0020A8FC File Offset: 0x00208AFC
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(PipelineStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("PipelineStrings", typeof(PipelineStrings).Assembly);
				PipelineStrings.resourceMan = resourceManager;
			}
			return PipelineStrings.resourceMan;
		}
	}

	// Token: 0x17001797 RID: 6039
	// (get) Token: 0x06006323 RID: 25379 RVA: 0x0020A93B File Offset: 0x00208B3B
	// (set) Token: 0x06006324 RID: 25380 RVA: 0x0020A942 File Offset: 0x00208B42
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return PipelineStrings.resourceCulture;
		}
		set
		{
			PipelineStrings.resourceCulture = value;
		}
	}

	// Token: 0x17001798 RID: 6040
	// (get) Token: 0x06006325 RID: 25381 RVA: 0x0020A94A File Offset: 0x00208B4A
	internal static string ActionPreferenceStop
	{
		get
		{
			return PipelineStrings.ResourceManager.GetString("ActionPreferenceStop", PipelineStrings.resourceCulture);
		}
	}

	// Token: 0x17001799 RID: 6041
	// (get) Token: 0x06006326 RID: 25382 RVA: 0x0020A960 File Offset: 0x00208B60
	internal static string CannotCreatePipeline
	{
		get
		{
			return PipelineStrings.ResourceManager.GetString("CannotCreatePipeline", PipelineStrings.resourceCulture);
		}
	}

	// Token: 0x1700179A RID: 6042
	// (get) Token: 0x06006327 RID: 25383 RVA: 0x0020A976 File Offset: 0x00208B76
	internal static string CommandProcessorAlreadyUsed
	{
		get
		{
			return PipelineStrings.ResourceManager.GetString("CommandProcessorAlreadyUsed", PipelineStrings.resourceCulture);
		}
	}

	// Token: 0x1700179B RID: 6043
	// (get) Token: 0x06006328 RID: 25384 RVA: 0x0020A98C File Offset: 0x00208B8C
	internal static string ConnectNotSupported
	{
		get
		{
			return PipelineStrings.ResourceManager.GetString("ConnectNotSupported", PipelineStrings.resourceCulture);
		}
	}

	// Token: 0x1700179C RID: 6044
	// (get) Token: 0x06006329 RID: 25385 RVA: 0x0020A9A2 File Offset: 0x00208BA2
	internal static string ExecutionAlreadyStarted
	{
		get
		{
			return PipelineStrings.ResourceManager.GetString("ExecutionAlreadyStarted", PipelineStrings.resourceCulture);
		}
	}

	// Token: 0x1700179D RID: 6045
	// (get) Token: 0x0600632A RID: 25386 RVA: 0x0020A9B8 File Offset: 0x00208BB8
	internal static string FirstCommandCannotHaveInput
	{
		get
		{
			return PipelineStrings.ResourceManager.GetString("FirstCommandCannotHaveInput", PipelineStrings.resourceCulture);
		}
	}

	// Token: 0x1700179E RID: 6046
	// (get) Token: 0x0600632B RID: 25387 RVA: 0x0020A9CE File Offset: 0x00208BCE
	internal static string InvalidCommandNumber
	{
		get
		{
			return PipelineStrings.ResourceManager.GetString("InvalidCommandNumber", PipelineStrings.resourceCulture);
		}
	}

	// Token: 0x1700179F RID: 6047
	// (get) Token: 0x0600632C RID: 25388 RVA: 0x0020A9E4 File Offset: 0x00208BE4
	internal static string InvalidRemoteCommand
	{
		get
		{
			return PipelineStrings.ResourceManager.GetString("InvalidRemoteCommand", PipelineStrings.resourceCulture);
		}
	}

	// Token: 0x170017A0 RID: 6048
	// (get) Token: 0x0600632D RID: 25389 RVA: 0x0020A9FA File Offset: 0x00208BFA
	internal static string PipeAlreadyTaken
	{
		get
		{
			return PipelineStrings.ResourceManager.GetString("PipeAlreadyTaken", PipelineStrings.resourceCulture);
		}
	}

	// Token: 0x170017A1 RID: 6049
	// (get) Token: 0x0600632E RID: 25390 RVA: 0x0020AA10 File Offset: 0x00208C10
	internal static string PipelineExecuteRequiresAtLeastOneCommand
	{
		get
		{
			return PipelineStrings.ResourceManager.GetString("PipelineExecuteRequiresAtLeastOneCommand", PipelineStrings.resourceCulture);
		}
	}

	// Token: 0x170017A2 RID: 6050
	// (get) Token: 0x0600632F RID: 25391 RVA: 0x0020AA26 File Offset: 0x00208C26
	internal static string PipelineExecutionInformation
	{
		get
		{
			return PipelineStrings.ResourceManager.GetString("PipelineExecutionInformation", PipelineStrings.resourceCulture);
		}
	}

	// Token: 0x170017A3 RID: 6051
	// (get) Token: 0x06006330 RID: 25392 RVA: 0x0020AA3C File Offset: 0x00208C3C
	internal static string PipelineExecutionNonTerminatingError
	{
		get
		{
			return PipelineStrings.ResourceManager.GetString("PipelineExecutionNonTerminatingError", PipelineStrings.resourceCulture);
		}
	}

	// Token: 0x170017A4 RID: 6052
	// (get) Token: 0x06006331 RID: 25393 RVA: 0x0020AA52 File Offset: 0x00208C52
	internal static string PipelineExecutionParameterBinding
	{
		get
		{
			return PipelineStrings.ResourceManager.GetString("PipelineExecutionParameterBinding", PipelineStrings.resourceCulture);
		}
	}

	// Token: 0x170017A5 RID: 6053
	// (get) Token: 0x06006332 RID: 25394 RVA: 0x0020AA68 File Offset: 0x00208C68
	internal static string PipelineExecutionTerminatingError
	{
		get
		{
			return PipelineStrings.ResourceManager.GetString("PipelineExecutionTerminatingError", PipelineStrings.resourceCulture);
		}
	}

	// Token: 0x170017A6 RID: 6054
	// (get) Token: 0x06006333 RID: 25395 RVA: 0x0020AA7E File Offset: 0x00208C7E
	internal static string PipelineNotDisconnected
	{
		get
		{
			return PipelineStrings.ResourceManager.GetString("PipelineNotDisconnected", PipelineStrings.resourceCulture);
		}
	}

	// Token: 0x170017A7 RID: 6055
	// (get) Token: 0x06006334 RID: 25396 RVA: 0x0020AA94 File Offset: 0x00208C94
	internal static string PipelineNotStarted
	{
		get
		{
			return PipelineStrings.ResourceManager.GetString("PipelineNotStarted", PipelineStrings.resourceCulture);
		}
	}

	// Token: 0x170017A8 RID: 6056
	// (get) Token: 0x06006335 RID: 25397 RVA: 0x0020AAAA File Offset: 0x00208CAA
	internal static string SecondFailure
	{
		get
		{
			return PipelineStrings.ResourceManager.GetString("SecondFailure", PipelineStrings.resourceCulture);
		}
	}

	// Token: 0x170017A9 RID: 6057
	// (get) Token: 0x06006336 RID: 25398 RVA: 0x0020AAC0 File Offset: 0x00208CC0
	internal static string WriteNotPermitted
	{
		get
		{
			return PipelineStrings.ResourceManager.GetString("WriteNotPermitted", PipelineStrings.resourceCulture);
		}
	}

	// Token: 0x170017AA RID: 6058
	// (get) Token: 0x06006337 RID: 25399 RVA: 0x0020AAD6 File Offset: 0x00208CD6
	internal static string WriteToClosedPipeline
	{
		get
		{
			return PipelineStrings.ResourceManager.GetString("WriteToClosedPipeline", PipelineStrings.resourceCulture);
		}
	}

	// Token: 0x04003231 RID: 12849
	private static ResourceManager resourceMan;

	// Token: 0x04003232 RID: 12850
	private static CultureInfo resourceCulture;
}
