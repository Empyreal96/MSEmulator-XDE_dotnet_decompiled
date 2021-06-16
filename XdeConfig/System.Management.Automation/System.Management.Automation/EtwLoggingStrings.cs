using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A41 RID: 2625
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
internal class EtwLoggingStrings
{
	// Token: 0x0600693A RID: 26938 RVA: 0x00212EEA File Offset: 0x002110EA
	internal EtwLoggingStrings()
	{
	}

	// Token: 0x17001D67 RID: 7527
	// (get) Token: 0x0600693B RID: 26939 RVA: 0x00212EF4 File Offset: 0x002110F4
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(EtwLoggingStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("EtwLoggingStrings", typeof(EtwLoggingStrings).Assembly);
				EtwLoggingStrings.resourceMan = resourceManager;
			}
			return EtwLoggingStrings.resourceMan;
		}
	}

	// Token: 0x17001D68 RID: 7528
	// (get) Token: 0x0600693C RID: 26940 RVA: 0x00212F33 File Offset: 0x00211133
	// (set) Token: 0x0600693D RID: 26941 RVA: 0x00212F3A File Offset: 0x0021113A
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return EtwLoggingStrings.resourceCulture;
		}
		set
		{
			EtwLoggingStrings.resourceCulture = value;
		}
	}

	// Token: 0x17001D69 RID: 7529
	// (get) Token: 0x0600693E RID: 26942 RVA: 0x00212F42 File Offset: 0x00211142
	internal static string CommandStateChange
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("CommandStateChange", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D6A RID: 7530
	// (get) Token: 0x0600693F RID: 26943 RVA: 0x00212F58 File Offset: 0x00211158
	internal static string EngineStateChange
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("EngineStateChange", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D6B RID: 7531
	// (get) Token: 0x06006940 RID: 26944 RVA: 0x00212F6E File Offset: 0x0021116E
	internal static string ErrorRecordId
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("ErrorRecordId", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D6C RID: 7532
	// (get) Token: 0x06006941 RID: 26945 RVA: 0x00212F84 File Offset: 0x00211184
	internal static string ErrorRecordMessage
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("ErrorRecordMessage", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D6D RID: 7533
	// (get) Token: 0x06006942 RID: 26946 RVA: 0x00212F9A File Offset: 0x0021119A
	internal static string ErrorRecordRecommendedAction
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("ErrorRecordRecommendedAction", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D6E RID: 7534
	// (get) Token: 0x06006943 RID: 26947 RVA: 0x00212FB0 File Offset: 0x002111B0
	internal static string ExecutionPolicyName
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("ExecutionPolicyName", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D6F RID: 7535
	// (get) Token: 0x06006944 RID: 26948 RVA: 0x00212FC6 File Offset: 0x002111C6
	internal static string JobCommand
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("JobCommand", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D70 RID: 7536
	// (get) Token: 0x06006945 RID: 26949 RVA: 0x00212FDC File Offset: 0x002111DC
	internal static string JobId
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("JobId", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D71 RID: 7537
	// (get) Token: 0x06006946 RID: 26950 RVA: 0x00212FF2 File Offset: 0x002111F2
	internal static string JobInstanceId
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("JobInstanceId", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D72 RID: 7538
	// (get) Token: 0x06006947 RID: 26951 RVA: 0x00213008 File Offset: 0x00211208
	internal static string JobLocation
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("JobLocation", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D73 RID: 7539
	// (get) Token: 0x06006948 RID: 26952 RVA: 0x0021301E File Offset: 0x0021121E
	internal static string JobName
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("JobName", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D74 RID: 7540
	// (get) Token: 0x06006949 RID: 26953 RVA: 0x00213034 File Offset: 0x00211234
	internal static string JobState
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("JobState", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D75 RID: 7541
	// (get) Token: 0x0600694A RID: 26954 RVA: 0x0021304A File Offset: 0x0021124A
	internal static string LogContextCommandName
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("LogContextCommandName", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D76 RID: 7542
	// (get) Token: 0x0600694B RID: 26955 RVA: 0x00213060 File Offset: 0x00211260
	internal static string LogContextCommandPath
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("LogContextCommandPath", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D77 RID: 7543
	// (get) Token: 0x0600694C RID: 26956 RVA: 0x00213076 File Offset: 0x00211276
	internal static string LogContextCommandType
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("LogContextCommandType", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D78 RID: 7544
	// (get) Token: 0x0600694D RID: 26957 RVA: 0x0021308C File Offset: 0x0021128C
	internal static string LogContextConnectedUser
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("LogContextConnectedUser", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D79 RID: 7545
	// (get) Token: 0x0600694E RID: 26958 RVA: 0x002130A2 File Offset: 0x002112A2
	internal static string LogContextEngineVersion
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("LogContextEngineVersion", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D7A RID: 7546
	// (get) Token: 0x0600694F RID: 26959 RVA: 0x002130B8 File Offset: 0x002112B8
	internal static string LogContextHostApplication
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("LogContextHostApplication", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D7B RID: 7547
	// (get) Token: 0x06006950 RID: 26960 RVA: 0x002130CE File Offset: 0x002112CE
	internal static string LogContextHostId
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("LogContextHostId", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D7C RID: 7548
	// (get) Token: 0x06006951 RID: 26961 RVA: 0x002130E4 File Offset: 0x002112E4
	internal static string LogContextHostName
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("LogContextHostName", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D7D RID: 7549
	// (get) Token: 0x06006952 RID: 26962 RVA: 0x002130FA File Offset: 0x002112FA
	internal static string LogContextHostVersion
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("LogContextHostVersion", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D7E RID: 7550
	// (get) Token: 0x06006953 RID: 26963 RVA: 0x00213110 File Offset: 0x00211310
	internal static string LogContextPipelineId
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("LogContextPipelineId", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D7F RID: 7551
	// (get) Token: 0x06006954 RID: 26964 RVA: 0x00213126 File Offset: 0x00211326
	internal static string LogContextRunspaceId
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("LogContextRunspaceId", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D80 RID: 7552
	// (get) Token: 0x06006955 RID: 26965 RVA: 0x0021313C File Offset: 0x0021133C
	internal static string LogContextScriptName
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("LogContextScriptName", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D81 RID: 7553
	// (get) Token: 0x06006956 RID: 26966 RVA: 0x00213152 File Offset: 0x00211352
	internal static string LogContextSequenceNumber
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("LogContextSequenceNumber", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D82 RID: 7554
	// (get) Token: 0x06006957 RID: 26967 RVA: 0x00213168 File Offset: 0x00211368
	internal static string LogContextSeverity
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("LogContextSeverity", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D83 RID: 7555
	// (get) Token: 0x06006958 RID: 26968 RVA: 0x0021317E File Offset: 0x0021137E
	internal static string LogContextShellId
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("LogContextShellId", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D84 RID: 7556
	// (get) Token: 0x06006959 RID: 26969 RVA: 0x00213194 File Offset: 0x00211394
	internal static string LogContextTime
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("LogContextTime", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D85 RID: 7557
	// (get) Token: 0x0600695A RID: 26970 RVA: 0x002131AA File Offset: 0x002113AA
	internal static string LogContextUser
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("LogContextUser", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D86 RID: 7558
	// (get) Token: 0x0600695B RID: 26971 RVA: 0x002131C0 File Offset: 0x002113C0
	internal static string NullJobName
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("NullJobName", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D87 RID: 7559
	// (get) Token: 0x0600695C RID: 26972 RVA: 0x002131D6 File Offset: 0x002113D6
	internal static string ProviderNameString
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("ProviderNameString", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D88 RID: 7560
	// (get) Token: 0x0600695D RID: 26973 RVA: 0x002131EC File Offset: 0x002113EC
	internal static string ProviderStateChange
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("ProviderStateChange", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D89 RID: 7561
	// (get) Token: 0x0600695E RID: 26974 RVA: 0x00213202 File Offset: 0x00211402
	internal static string ScriptStateChange
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("ScriptStateChange", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D8A RID: 7562
	// (get) Token: 0x0600695F RID: 26975 RVA: 0x00213218 File Offset: 0x00211418
	internal static string SettingChange
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("SettingChange", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x17001D8B RID: 7563
	// (get) Token: 0x06006960 RID: 26976 RVA: 0x0021322E File Offset: 0x0021142E
	internal static string SettingChangeNoPrevious
	{
		get
		{
			return EtwLoggingStrings.ResourceManager.GetString("SettingChangeNoPrevious", EtwLoggingStrings.resourceCulture);
		}
	}

	// Token: 0x04003279 RID: 12921
	private static ResourceManager resourceMan;

	// Token: 0x0400327A RID: 12922
	private static CultureInfo resourceCulture;
}
