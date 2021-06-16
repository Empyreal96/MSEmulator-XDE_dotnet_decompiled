using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A37 RID: 2615
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Modules
{
	// Token: 0x06006781 RID: 26497 RVA: 0x00210910 File Offset: 0x0020EB10
	internal Modules()
	{
	}

	// Token: 0x17001BC2 RID: 7106
	// (get) Token: 0x06006782 RID: 26498 RVA: 0x00210918 File Offset: 0x0020EB18
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(Modules.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("Modules", typeof(Modules).Assembly);
				Modules.resourceMan = resourceManager;
			}
			return Modules.resourceMan;
		}
	}

	// Token: 0x17001BC3 RID: 7107
	// (get) Token: 0x06006783 RID: 26499 RVA: 0x00210957 File Offset: 0x0020EB57
	// (set) Token: 0x06006784 RID: 26500 RVA: 0x0021095E File Offset: 0x0020EB5E
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return Modules.resourceCulture;
		}
		set
		{
			Modules.resourceCulture = value;
		}
	}

	// Token: 0x17001BC4 RID: 7108
	// (get) Token: 0x06006785 RID: 26501 RVA: 0x00210966 File Offset: 0x0020EB66
	internal static string AliasesToExport
	{
		get
		{
			return Modules.ResourceManager.GetString("AliasesToExport", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BC5 RID: 7109
	// (get) Token: 0x06006786 RID: 26502 RVA: 0x0021097C File Offset: 0x0020EB7C
	internal static string Author
	{
		get
		{
			return Modules.ResourceManager.GetString("Author", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BC6 RID: 7110
	// (get) Token: 0x06006787 RID: 26503 RVA: 0x00210992 File Offset: 0x0020EB92
	internal static string CannotDefineWorkflowInconsistentLanguageMode
	{
		get
		{
			return Modules.ResourceManager.GetString("CannotDefineWorkflowInconsistentLanguageMode", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BC7 RID: 7111
	// (get) Token: 0x06006788 RID: 26504 RVA: 0x002109A8 File Offset: 0x0020EBA8
	internal static string CannotDetectNetFrameworkVersion
	{
		get
		{
			return Modules.ResourceManager.GetString("CannotDetectNetFrameworkVersion", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BC8 RID: 7112
	// (get) Token: 0x06006789 RID: 26505 RVA: 0x002109BE File Offset: 0x0020EBBE
	internal static string CanOnlyBeUsedFromWithinAModule
	{
		get
		{
			return Modules.ResourceManager.GetString("CanOnlyBeUsedFromWithinAModule", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BC9 RID: 7113
	// (get) Token: 0x0600678A RID: 26506 RVA: 0x002109D4 File Offset: 0x0020EBD4
	internal static string CantUseAsCustomObjectWithBinaryModule
	{
		get
		{
			return Modules.ResourceManager.GetString("CantUseAsCustomObjectWithBinaryModule", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BCA RID: 7114
	// (get) Token: 0x0600678B RID: 26507 RVA: 0x002109EA File Offset: 0x0020EBEA
	internal static string CLRVersion
	{
		get
		{
			return Modules.ResourceManager.GetString("CLRVersion", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BCB RID: 7115
	// (get) Token: 0x0600678C RID: 26508 RVA: 0x00210A00 File Offset: 0x0020EC00
	internal static string CmdletizationDoesSupportRexportingNestedModules
	{
		get
		{
			return Modules.ResourceManager.GetString("CmdletizationDoesSupportRexportingNestedModules", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BCC RID: 7116
	// (get) Token: 0x0600678D RID: 26509 RVA: 0x00210A16 File Offset: 0x0020EC16
	internal static string CmdletsToExport
	{
		get
		{
			return Modules.ResourceManager.GetString("CmdletsToExport", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BCD RID: 7117
	// (get) Token: 0x0600678E RID: 26510 RVA: 0x00210A2C File Offset: 0x0020EC2C
	internal static string CompanyName
	{
		get
		{
			return Modules.ResourceManager.GetString("CompanyName", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BCE RID: 7118
	// (get) Token: 0x0600678F RID: 26511 RVA: 0x00210A42 File Offset: 0x0020EC42
	internal static string ConfirmRemoveModule
	{
		get
		{
			return Modules.ResourceManager.GetString("ConfirmRemoveModule", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BCF RID: 7119
	// (get) Token: 0x06006790 RID: 26512 RVA: 0x00210A58 File Offset: 0x0020EC58
	internal static string Copyright
	{
		get
		{
			return Modules.ResourceManager.GetString("Copyright", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BD0 RID: 7120
	// (get) Token: 0x06006791 RID: 26513 RVA: 0x00210A6E File Offset: 0x0020EC6E
	internal static string CoreModuleCannotBeRemoved
	{
		get
		{
			return Modules.ResourceManager.GetString("CoreModuleCannotBeRemoved", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BD1 RID: 7121
	// (get) Token: 0x06006792 RID: 26514 RVA: 0x00210A84 File Offset: 0x0020EC84
	internal static string CreatingModuleManifestFile
	{
		get
		{
			return Modules.ResourceManager.GetString("CreatingModuleManifestFile", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BD2 RID: 7122
	// (get) Token: 0x06006793 RID: 26515 RVA: 0x00210A9A File Offset: 0x0020EC9A
	internal static string DefaultCommandPrefix
	{
		get
		{
			return Modules.ResourceManager.GetString("DefaultCommandPrefix", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BD3 RID: 7123
	// (get) Token: 0x06006794 RID: 26516 RVA: 0x00210AB0 File Offset: 0x0020ECB0
	internal static string DefaultCompanyName
	{
		get
		{
			return Modules.ResourceManager.GetString("DefaultCompanyName", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BD4 RID: 7124
	// (get) Token: 0x06006795 RID: 26517 RVA: 0x00210AC6 File Offset: 0x0020ECC6
	internal static string DefaultCopyrightMessage
	{
		get
		{
			return Modules.ResourceManager.GetString("DefaultCopyrightMessage", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BD5 RID: 7125
	// (get) Token: 0x06006796 RID: 26518 RVA: 0x00210ADC File Offset: 0x0020ECDC
	internal static string Description
	{
		get
		{
			return Modules.ResourceManager.GetString("Description", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BD6 RID: 7126
	// (get) Token: 0x06006797 RID: 26519 RVA: 0x00210AF2 File Offset: 0x0020ECF2
	internal static string DeterminingAvailableModules
	{
		get
		{
			return Modules.ResourceManager.GetString("DeterminingAvailableModules", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BD7 RID: 7127
	// (get) Token: 0x06006798 RID: 26520 RVA: 0x00210B08 File Offset: 0x0020ED08
	internal static string DotNetFrameworkVersion
	{
		get
		{
			return Modules.ResourceManager.GetString("DotNetFrameworkVersion", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BD8 RID: 7128
	// (get) Token: 0x06006799 RID: 26521 RVA: 0x00210B1E File Offset: 0x0020ED1E
	internal static string DottingScriptFile
	{
		get
		{
			return Modules.ResourceManager.GetString("DottingScriptFile", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BD9 RID: 7129
	// (get) Token: 0x0600679A RID: 26522 RVA: 0x00210B34 File Offset: 0x0020ED34
	internal static string DscResourcesToExport
	{
		get
		{
			return Modules.ResourceManager.GetString("DscResourcesToExport", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BDA RID: 7130
	// (get) Token: 0x0600679B RID: 26523 RVA: 0x00210B4A File Offset: 0x0020ED4A
	internal static string EmptyModuleManifest
	{
		get
		{
			return Modules.ResourceManager.GetString("EmptyModuleManifest", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BDB RID: 7131
	// (get) Token: 0x0600679C RID: 26524 RVA: 0x00210B60 File Offset: 0x0020ED60
	internal static string EndOfManifestHashTable
	{
		get
		{
			return Modules.ResourceManager.GetString("EndOfManifestHashTable", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BDC RID: 7132
	// (get) Token: 0x0600679D RID: 26525 RVA: 0x00210B76 File Offset: 0x0020ED76
	internal static string ExportAsWorkflowInvalidCommand
	{
		get
		{
			return Modules.ResourceManager.GetString("ExportAsWorkflowInvalidCommand", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BDD RID: 7133
	// (get) Token: 0x0600679E RID: 26526 RVA: 0x00210B8C File Offset: 0x0020ED8C
	internal static string ExportingAlias
	{
		get
		{
			return Modules.ResourceManager.GetString("ExportingAlias", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BDE RID: 7134
	// (get) Token: 0x0600679F RID: 26527 RVA: 0x00210BA2 File Offset: 0x0020EDA2
	internal static string ExportingCmdlet
	{
		get
		{
			return Modules.ResourceManager.GetString("ExportingCmdlet", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BDF RID: 7135
	// (get) Token: 0x060067A0 RID: 26528 RVA: 0x00210BB8 File Offset: 0x0020EDB8
	internal static string ExportingCommandAsWorkflow
	{
		get
		{
			return Modules.ResourceManager.GetString("ExportingCommandAsWorkflow", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BE0 RID: 7136
	// (get) Token: 0x060067A1 RID: 26529 RVA: 0x00210BCE File Offset: 0x0020EDCE
	internal static string ExportingFunction
	{
		get
		{
			return Modules.ResourceManager.GetString("ExportingFunction", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BE1 RID: 7137
	// (get) Token: 0x060067A2 RID: 26530 RVA: 0x00210BE4 File Offset: 0x0020EDE4
	internal static string ExportingVariable
	{
		get
		{
			return Modules.ResourceManager.GetString("ExportingVariable", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BE2 RID: 7138
	// (get) Token: 0x060067A3 RID: 26531 RVA: 0x00210BFA File Offset: 0x0020EDFA
	internal static string ExportingWorkflow
	{
		get
		{
			return Modules.ResourceManager.GetString("ExportingWorkflow", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BE3 RID: 7139
	// (get) Token: 0x060067A4 RID: 26532 RVA: 0x00210C10 File Offset: 0x0020EE10
	internal static string FileList
	{
		get
		{
			return Modules.ResourceManager.GetString("FileList", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BE4 RID: 7140
	// (get) Token: 0x060067A5 RID: 26533 RVA: 0x00210C26 File Offset: 0x0020EE26
	internal static string FormatsFileNotFound
	{
		get
		{
			return Modules.ResourceManager.GetString("FormatsFileNotFound", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BE5 RID: 7141
	// (get) Token: 0x060067A6 RID: 26534 RVA: 0x00210C3C File Offset: 0x0020EE3C
	internal static string FormatsToProcess
	{
		get
		{
			return Modules.ResourceManager.GetString("FormatsToProcess", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BE6 RID: 7142
	// (get) Token: 0x060067A7 RID: 26535 RVA: 0x00210C52 File Offset: 0x0020EE52
	internal static string FunctionsToExport
	{
		get
		{
			return Modules.ResourceManager.GetString("FunctionsToExport", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BE7 RID: 7143
	// (get) Token: 0x060067A8 RID: 26536 RVA: 0x00210C68 File Offset: 0x0020EE68
	internal static string GlobalAndScopeParameterCannotBeSpecifiedTogether
	{
		get
		{
			return Modules.ResourceManager.GetString("GlobalAndScopeParameterCannotBeSpecifiedTogether", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BE8 RID: 7144
	// (get) Token: 0x060067A9 RID: 26537 RVA: 0x00210C7E File Offset: 0x0020EE7E
	internal static string GUID
	{
		get
		{
			return Modules.ResourceManager.GetString("GUID", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BE9 RID: 7145
	// (get) Token: 0x060067AA RID: 26538 RVA: 0x00210C94 File Offset: 0x0020EE94
	internal static string HelpInfoURI
	{
		get
		{
			return Modules.ResourceManager.GetString("HelpInfoURI", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BEA RID: 7146
	// (get) Token: 0x060067AB RID: 26539 RVA: 0x00210CAA File Offset: 0x0020EEAA
	internal static string IconUri
	{
		get
		{
			return Modules.ResourceManager.GetString("IconUri", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BEB RID: 7147
	// (get) Token: 0x060067AC RID: 26540 RVA: 0x00210CC0 File Offset: 0x0020EEC0
	internal static string ImportingAlias
	{
		get
		{
			return Modules.ResourceManager.GetString("ImportingAlias", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BEC RID: 7148
	// (get) Token: 0x060067AD RID: 26541 RVA: 0x00210CD6 File Offset: 0x0020EED6
	internal static string ImportingCmdlet
	{
		get
		{
			return Modules.ResourceManager.GetString("ImportingCmdlet", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BED RID: 7149
	// (get) Token: 0x060067AE RID: 26542 RVA: 0x00210CEC File Offset: 0x0020EEEC
	internal static string ImportingFunction
	{
		get
		{
			return Modules.ResourceManager.GetString("ImportingFunction", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BEE RID: 7150
	// (get) Token: 0x060067AF RID: 26543 RVA: 0x00210D02 File Offset: 0x0020EF02
	internal static string ImportingNonStandardNoun
	{
		get
		{
			return Modules.ResourceManager.GetString("ImportingNonStandardNoun", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BEF RID: 7151
	// (get) Token: 0x060067B0 RID: 26544 RVA: 0x00210D18 File Offset: 0x0020EF18
	internal static string ImportingNonStandardNounVerbose
	{
		get
		{
			return Modules.ResourceManager.GetString("ImportingNonStandardNounVerbose", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BF0 RID: 7152
	// (get) Token: 0x060067B1 RID: 26545 RVA: 0x00210D2E File Offset: 0x0020EF2E
	internal static string ImportingNonStandardVerb
	{
		get
		{
			return Modules.ResourceManager.GetString("ImportingNonStandardVerb", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BF1 RID: 7153
	// (get) Token: 0x060067B2 RID: 26546 RVA: 0x00210D44 File Offset: 0x0020EF44
	internal static string ImportingNonStandardVerbVerbose
	{
		get
		{
			return Modules.ResourceManager.GetString("ImportingNonStandardVerbVerbose", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BF2 RID: 7154
	// (get) Token: 0x060067B3 RID: 26547 RVA: 0x00210D5A File Offset: 0x0020EF5A
	internal static string ImportingNonStandardVerbVerboseSuggestion
	{
		get
		{
			return Modules.ResourceManager.GetString("ImportingNonStandardVerbVerboseSuggestion", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BF3 RID: 7155
	// (get) Token: 0x060067B4 RID: 26548 RVA: 0x00210D70 File Offset: 0x0020EF70
	internal static string ImportingVariable
	{
		get
		{
			return Modules.ResourceManager.GetString("ImportingVariable", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BF4 RID: 7156
	// (get) Token: 0x060067B5 RID: 26549 RVA: 0x00210D86 File Offset: 0x0020EF86
	internal static string ImportingWorkflow
	{
		get
		{
			return Modules.ResourceManager.GetString("ImportingWorkflow", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BF5 RID: 7157
	// (get) Token: 0x060067B6 RID: 26550 RVA: 0x00210D9C File Offset: 0x0020EF9C
	internal static string ImportModuleNoClobberForAlias
	{
		get
		{
			return Modules.ResourceManager.GetString("ImportModuleNoClobberForAlias", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BF6 RID: 7158
	// (get) Token: 0x060067B7 RID: 26551 RVA: 0x00210DB2 File Offset: 0x0020EFB2
	internal static string ImportModuleNoClobberForCmdlet
	{
		get
		{
			return Modules.ResourceManager.GetString("ImportModuleNoClobberForCmdlet", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BF7 RID: 7159
	// (get) Token: 0x060067B8 RID: 26552 RVA: 0x00210DC8 File Offset: 0x0020EFC8
	internal static string ImportModuleNoClobberForFunction
	{
		get
		{
			return Modules.ResourceManager.GetString("ImportModuleNoClobberForFunction", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BF8 RID: 7160
	// (get) Token: 0x060067B9 RID: 26553 RVA: 0x00210DDE File Offset: 0x0020EFDE
	internal static string ImportModuleNoClobberForVariable
	{
		get
		{
			return Modules.ResourceManager.GetString("ImportModuleNoClobberForVariable", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BF9 RID: 7161
	// (get) Token: 0x060067BA RID: 26554 RVA: 0x00210DF4 File Offset: 0x0020EFF4
	internal static string ImportModuleNoClobberForWorkflow
	{
		get
		{
			return Modules.ResourceManager.GetString("ImportModuleNoClobberForWorkflow", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BFA RID: 7162
	// (get) Token: 0x060067BB RID: 26555 RVA: 0x00210E0A File Offset: 0x0020F00A
	internal static string IncludedItemPathFallsOutsideSaveTree
	{
		get
		{
			return Modules.ResourceManager.GetString("IncludedItemPathFallsOutsideSaveTree", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BFB RID: 7163
	// (get) Token: 0x060067BC RID: 26556 RVA: 0x00210E20 File Offset: 0x0020F020
	internal static string InvalidDotNetFrameworkVersion
	{
		get
		{
			return Modules.ResourceManager.GetString("InvalidDotNetFrameworkVersion", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BFC RID: 7164
	// (get) Token: 0x060067BD RID: 26557 RVA: 0x00210E36 File Offset: 0x0020F036
	internal static string InvalidModuleExtension
	{
		get
		{
			return Modules.ResourceManager.GetString("InvalidModuleExtension", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BFD RID: 7165
	// (get) Token: 0x060067BE RID: 26558 RVA: 0x00210E4C File Offset: 0x0020F04C
	internal static string InvalidModuleManifest
	{
		get
		{
			return Modules.ResourceManager.GetString("InvalidModuleManifest", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BFE RID: 7166
	// (get) Token: 0x060067BF RID: 26559 RVA: 0x00210E62 File Offset: 0x0020F062
	internal static string InvalidModuleManifestMember
	{
		get
		{
			return Modules.ResourceManager.GetString("InvalidModuleManifestMember", Modules.resourceCulture);
		}
	}

	// Token: 0x17001BFF RID: 7167
	// (get) Token: 0x060067C0 RID: 26560 RVA: 0x00210E78 File Offset: 0x0020F078
	internal static string InvalidModuleManifestPath
	{
		get
		{
			return Modules.ResourceManager.GetString("InvalidModuleManifestPath", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C00 RID: 7168
	// (get) Token: 0x060067C1 RID: 26561 RVA: 0x00210E8E File Offset: 0x0020F08E
	internal static string InvalidModuleManifestVersion
	{
		get
		{
			return Modules.ResourceManager.GetString("InvalidModuleManifestVersion", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C01 RID: 7169
	// (get) Token: 0x060067C2 RID: 26562 RVA: 0x00210EA4 File Offset: 0x0020F0A4
	internal static string InvalidModuleSpecificationMember
	{
		get
		{
			return Modules.ResourceManager.GetString("InvalidModuleSpecificationMember", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C02 RID: 7170
	// (get) Token: 0x060067C3 RID: 26563 RVA: 0x00210EBA File Offset: 0x0020F0BA
	internal static string InvalidOperationOnBinaryModule
	{
		get
		{
			return Modules.ResourceManager.GetString("InvalidOperationOnBinaryModule", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C03 RID: 7171
	// (get) Token: 0x060067C4 RID: 26564 RVA: 0x00210ED0 File Offset: 0x0020F0D0
	internal static string InvalidParameterValue
	{
		get
		{
			return Modules.ResourceManager.GetString("InvalidParameterValue", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C04 RID: 7172
	// (get) Token: 0x060067C5 RID: 26565 RVA: 0x00210EE6 File Offset: 0x0020F0E6
	internal static string InvalidPowerShellHostName
	{
		get
		{
			return Modules.ResourceManager.GetString("InvalidPowerShellHostName", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C05 RID: 7173
	// (get) Token: 0x060067C6 RID: 26566 RVA: 0x00210EFC File Offset: 0x0020F0FC
	internal static string InvalidPowerShellHostVersion
	{
		get
		{
			return Modules.ResourceManager.GetString("InvalidPowerShellHostVersion", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C06 RID: 7174
	// (get) Token: 0x060067C7 RID: 26567 RVA: 0x00210F12 File Offset: 0x0020F112
	internal static string InvalidProcessorArchitecture
	{
		get
		{
			return Modules.ResourceManager.GetString("InvalidProcessorArchitecture", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C07 RID: 7175
	// (get) Token: 0x060067C8 RID: 26568 RVA: 0x00210F28 File Offset: 0x0020F128
	internal static string InvalidProcessorArchitectureInManifest
	{
		get
		{
			return Modules.ResourceManager.GetString("InvalidProcessorArchitectureInManifest", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C08 RID: 7176
	// (get) Token: 0x060067C9 RID: 26569 RVA: 0x00210F3E File Offset: 0x0020F13E
	internal static string InvalidWorkflowExtension
	{
		get
		{
			return Modules.ResourceManager.GetString("InvalidWorkflowExtension", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C09 RID: 7177
	// (get) Token: 0x060067CA RID: 26570 RVA: 0x00210F54 File Offset: 0x0020F154
	internal static string InvalidWorkflowExtensionDuringManifestProcessing
	{
		get
		{
			return Modules.ResourceManager.GetString("InvalidWorkflowExtensionDuringManifestProcessing", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C0A RID: 7178
	// (get) Token: 0x060067CB RID: 26571 RVA: 0x00210F6A File Offset: 0x0020F16A
	internal static string LicenseUri
	{
		get
		{
			return Modules.ResourceManager.GetString("LicenseUri", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C0B RID: 7179
	// (get) Token: 0x060067CC RID: 26572 RVA: 0x00210F80 File Offset: 0x0020F180
	internal static string LoadingFile
	{
		get
		{
			return Modules.ResourceManager.GetString("LoadingFile", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C0C RID: 7180
	// (get) Token: 0x060067CD RID: 26573 RVA: 0x00210F96 File Offset: 0x0020F196
	internal static string LoadingModule
	{
		get
		{
			return Modules.ResourceManager.GetString("LoadingModule", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C0D RID: 7181
	// (get) Token: 0x060067CE RID: 26574 RVA: 0x00210FAC File Offset: 0x0020F1AC
	internal static string LoadingWorkflow
	{
		get
		{
			return Modules.ResourceManager.GetString("LoadingWorkflow", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C0E RID: 7182
	// (get) Token: 0x060067CF RID: 26575 RVA: 0x00210FC2 File Offset: 0x0020F1C2
	internal static string ManifestHeaderLine1
	{
		get
		{
			return Modules.ResourceManager.GetString("ManifestHeaderLine1", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C0F RID: 7183
	// (get) Token: 0x060067D0 RID: 26576 RVA: 0x00210FD8 File Offset: 0x0020F1D8
	internal static string ManifestHeaderLine2
	{
		get
		{
			return Modules.ResourceManager.GetString("ManifestHeaderLine2", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C10 RID: 7184
	// (get) Token: 0x060067D1 RID: 26577 RVA: 0x00210FEE File Offset: 0x0020F1EE
	internal static string ManifestHeaderLine3
	{
		get
		{
			return Modules.ResourceManager.GetString("ManifestHeaderLine3", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C11 RID: 7185
	// (get) Token: 0x060067D2 RID: 26578 RVA: 0x00211004 File Offset: 0x0020F204
	internal static string ManifestMemberNotFound
	{
		get
		{
			return Modules.ResourceManager.GetString("ManifestMemberNotFound", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C12 RID: 7186
	// (get) Token: 0x060067D3 RID: 26579 RVA: 0x0021101A File Offset: 0x0020F21A
	internal static string ManifestMemberNotValid
	{
		get
		{
			return Modules.ResourceManager.GetString("ManifestMemberNotValid", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C13 RID: 7187
	// (get) Token: 0x060067D4 RID: 26580 RVA: 0x00211030 File Offset: 0x0020F230
	internal static string MaximumVersionFormatIncorrect
	{
		get
		{
			return Modules.ResourceManager.GetString("MaximumVersionFormatIncorrect", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C14 RID: 7188
	// (get) Token: 0x060067D5 RID: 26581 RVA: 0x00211046 File Offset: 0x0020F246
	internal static string MaximumVersionNotFound
	{
		get
		{
			return Modules.ResourceManager.GetString("MaximumVersionNotFound", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C15 RID: 7189
	// (get) Token: 0x060067D6 RID: 26582 RVA: 0x0021105C File Offset: 0x0020F25C
	internal static string MinimumVersionAndMaximumVersionInvalidRange
	{
		get
		{
			return Modules.ResourceManager.GetString("MinimumVersionAndMaximumVersionInvalidRange", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C16 RID: 7190
	// (get) Token: 0x060067D7 RID: 26583 RVA: 0x00211072 File Offset: 0x0020F272
	internal static string MinimumVersionAndMaximumVersionNotFound
	{
		get
		{
			return Modules.ResourceManager.GetString("MinimumVersionAndMaximumVersionNotFound", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C17 RID: 7191
	// (get) Token: 0x060067D8 RID: 26584 RVA: 0x00211088 File Offset: 0x0020F288
	internal static string MixedModuleOverCimSessionWarning
	{
		get
		{
			return Modules.ResourceManager.GetString("MixedModuleOverCimSessionWarning", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C18 RID: 7192
	// (get) Token: 0x060067D9 RID: 26585 RVA: 0x0021109E File Offset: 0x0020F29E
	internal static string ModuleAssemblyFound
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleAssemblyFound", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C19 RID: 7193
	// (get) Token: 0x060067DA RID: 26586 RVA: 0x002110B4 File Offset: 0x0020F2B4
	internal static string ModuleDiscoveryForLoadedModulesWorksOnlyForUnQualifiedNames
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleDiscoveryForLoadedModulesWorksOnlyForUnQualifiedNames", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C1A RID: 7194
	// (get) Token: 0x060067DB RID: 26587 RVA: 0x002110CA File Offset: 0x0020F2CA
	internal static string ModuleDriveInUse
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleDriveInUse", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C1B RID: 7195
	// (get) Token: 0x060067DC RID: 26588 RVA: 0x002110E0 File Offset: 0x0020F2E0
	internal static string ModuleIsConstant
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleIsConstant", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C1C RID: 7196
	// (get) Token: 0x060067DD RID: 26589 RVA: 0x002110F6 File Offset: 0x0020F2F6
	internal static string ModuleIsReadOnly
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleIsReadOnly", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C1D RID: 7197
	// (get) Token: 0x060067DE RID: 26590 RVA: 0x0021110C File Offset: 0x0020F30C
	internal static string ModuleIsRequired
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleIsRequired", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C1E RID: 7198
	// (get) Token: 0x060067DF RID: 26591 RVA: 0x00211122 File Offset: 0x0020F322
	internal static string ModuleList
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleList", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C1F RID: 7199
	// (get) Token: 0x060067E0 RID: 26592 RVA: 0x00211138 File Offset: 0x0020F338
	internal static string ModuleLoadedAsASnapin
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleLoadedAsASnapin", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C20 RID: 7200
	// (get) Token: 0x060067E1 RID: 26593 RVA: 0x0021114E File Offset: 0x0020F34E
	internal static string ModuleManifestCannotContainBothModuleToProcessAndRootModule
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleManifestCannotContainBothModuleToProcessAndRootModule", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C21 RID: 7201
	// (get) Token: 0x060067E2 RID: 26594 RVA: 0x00211164 File Offset: 0x0020F364
	internal static string ModuleManifestInsufficientCLRVersion
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleManifestInsufficientCLRVersion", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C22 RID: 7202
	// (get) Token: 0x060067E3 RID: 26595 RVA: 0x0021117A File Offset: 0x0020F37A
	internal static string ModuleManifestInsufficientModuleVersion
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleManifestInsufficientModuleVersion", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C23 RID: 7203
	// (get) Token: 0x060067E4 RID: 26596 RVA: 0x00211190 File Offset: 0x0020F390
	internal static string ModuleManifestInsufficientPowerShellVersion
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleManifestInsufficientPowerShellVersion", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C24 RID: 7204
	// (get) Token: 0x060067E5 RID: 26597 RVA: 0x002111A6 File Offset: 0x0020F3A6
	internal static string ModuleManifestInvalidManifestMember
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleManifestInvalidManifestMember", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C25 RID: 7205
	// (get) Token: 0x060067E6 RID: 26598 RVA: 0x002111BC File Offset: 0x0020F3BC
	internal static string ModuleManifestInvalidValue
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleManifestInvalidValue", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C26 RID: 7206
	// (get) Token: 0x060067E7 RID: 26599 RVA: 0x002111D2 File Offset: 0x0020F3D2
	internal static string ModuleManifestMissingModuleVersion
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleManifestMissingModuleVersion", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C27 RID: 7207
	// (get) Token: 0x060067E8 RID: 26600 RVA: 0x002111E8 File Offset: 0x0020F3E8
	internal static string ModuleManifestNestedModulesCantGoWithModuleToProcess
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleManifestNestedModulesCantGoWithModuleToProcess", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C28 RID: 7208
	// (get) Token: 0x060067E9 RID: 26601 RVA: 0x002111FE File Offset: 0x0020F3FE
	internal static string ModuleNotFound
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleNotFound", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C29 RID: 7209
	// (get) Token: 0x060067EA RID: 26602 RVA: 0x00211214 File Offset: 0x0020F414
	internal static string ModuleNotFoundForGetModule
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleNotFoundForGetModule", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C2A RID: 7210
	// (get) Token: 0x060067EB RID: 26603 RVA: 0x0021122A File Offset: 0x0020F42A
	internal static string ModuleTooDeeplyNested
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleTooDeeplyNested", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C2B RID: 7211
	// (get) Token: 0x060067EC RID: 26604 RVA: 0x00211240 File Offset: 0x0020F440
	internal static string ModuleToProcess
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleToProcess", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C2C RID: 7212
	// (get) Token: 0x060067ED RID: 26605 RVA: 0x00211256 File Offset: 0x0020F456
	internal static string ModuleToProcessFieldDeprecated
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleToProcessFieldDeprecated", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C2D RID: 7213
	// (get) Token: 0x060067EE RID: 26606 RVA: 0x0021126C File Offset: 0x0020F46C
	internal static string ModuleVersion
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleVersion", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C2E RID: 7214
	// (get) Token: 0x060067EF RID: 26607 RVA: 0x00211282 File Offset: 0x0020F482
	internal static string ModuleVersionEqualsToVersionFolder
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleVersionEqualsToVersionFolder", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C2F RID: 7215
	// (get) Token: 0x060067F0 RID: 26608 RVA: 0x00211298 File Offset: 0x0020F498
	internal static string ModuleWithVersionNotFound
	{
		get
		{
			return Modules.ResourceManager.GetString("ModuleWithVersionNotFound", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C30 RID: 7216
	// (get) Token: 0x060067F1 RID: 26609 RVA: 0x002112AE File Offset: 0x0020F4AE
	internal static string NestedModules
	{
		get
		{
			return Modules.ResourceManager.GetString("NestedModules", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C31 RID: 7217
	// (get) Token: 0x060067F2 RID: 26610 RVA: 0x002112C4 File Offset: 0x0020F4C4
	internal static string NoModulesRemoved
	{
		get
		{
			return Modules.ResourceManager.GetString("NoModulesRemoved", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C32 RID: 7218
	// (get) Token: 0x060067F3 RID: 26611 RVA: 0x002112DA File Offset: 0x0020F4DA
	internal static string PopulatingRepositorySourceLocation
	{
		get
		{
			return Modules.ResourceManager.GetString("PopulatingRepositorySourceLocation", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C33 RID: 7219
	// (get) Token: 0x060067F4 RID: 26612 RVA: 0x002112F0 File Offset: 0x0020F4F0
	internal static string PowerShellHostName
	{
		get
		{
			return Modules.ResourceManager.GetString("PowerShellHostName", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C34 RID: 7220
	// (get) Token: 0x060067F5 RID: 26613 RVA: 0x00211306 File Offset: 0x0020F506
	internal static string PowerShellHostVersion
	{
		get
		{
			return Modules.ResourceManager.GetString("PowerShellHostVersion", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C35 RID: 7221
	// (get) Token: 0x060067F6 RID: 26614 RVA: 0x0021131C File Offset: 0x0020F51C
	internal static string PowerShellVersion
	{
		get
		{
			return Modules.ResourceManager.GetString("PowerShellVersion", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C36 RID: 7222
	// (get) Token: 0x060067F7 RID: 26615 RVA: 0x00211332 File Offset: 0x0020F532
	internal static string PrivateData
	{
		get
		{
			return Modules.ResourceManager.GetString("PrivateData", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C37 RID: 7223
	// (get) Token: 0x060067F8 RID: 26616 RVA: 0x00211348 File Offset: 0x0020F548
	internal static string PrivateDataValueTypeShouldBeHashTableError
	{
		get
		{
			return Modules.ResourceManager.GetString("PrivateDataValueTypeShouldBeHashTableError", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C38 RID: 7224
	// (get) Token: 0x060067F9 RID: 26617 RVA: 0x0021135E File Offset: 0x0020F55E
	internal static string PrivateDataValueTypeShouldBeHashTableWarning
	{
		get
		{
			return Modules.ResourceManager.GetString("PrivateDataValueTypeShouldBeHashTableWarning", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C39 RID: 7225
	// (get) Token: 0x060067FA RID: 26618 RVA: 0x00211374 File Offset: 0x0020F574
	internal static string ProcessorArchitecture
	{
		get
		{
			return Modules.ResourceManager.GetString("ProcessorArchitecture", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C3A RID: 7226
	// (get) Token: 0x060067FB RID: 26619 RVA: 0x0021138A File Offset: 0x0020F58A
	internal static string ProjectUri
	{
		get
		{
			return Modules.ResourceManager.GetString("ProjectUri", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C3B RID: 7227
	// (get) Token: 0x060067FC RID: 26620 RVA: 0x002113A0 File Offset: 0x0020F5A0
	internal static string PSData
	{
		get
		{
			return Modules.ResourceManager.GetString("PSData", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C3C RID: 7228
	// (get) Token: 0x060067FD RID: 26621 RVA: 0x002113B6 File Offset: 0x0020F5B6
	internal static string PsModuleOverCimSessionError
	{
		get
		{
			return Modules.ResourceManager.GetString("PsModuleOverCimSessionError", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C3D RID: 7229
	// (get) Token: 0x060067FE RID: 26622 RVA: 0x002113CC File Offset: 0x0020F5CC
	internal static string ReleaseNotes
	{
		get
		{
			return Modules.ResourceManager.GetString("ReleaseNotes", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C3E RID: 7230
	// (get) Token: 0x060067FF RID: 26623 RVA: 0x002113E2 File Offset: 0x0020F5E2
	internal static string RemoteDiscoveryFailedToGenerateProxyForRemoteModule
	{
		get
		{
			return Modules.ResourceManager.GetString("RemoteDiscoveryFailedToGenerateProxyForRemoteModule", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C3F RID: 7231
	// (get) Token: 0x06006800 RID: 26624 RVA: 0x002113F8 File Offset: 0x0020F5F8
	internal static string RemoteDiscoveryFailedToProcessRemoteModule
	{
		get
		{
			return Modules.ResourceManager.GetString("RemoteDiscoveryFailedToProcessRemoteModule", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C40 RID: 7232
	// (get) Token: 0x06006801 RID: 26625 RVA: 0x0021140E File Offset: 0x0020F60E
	internal static string RemoteDiscoveryFailureFromDiscoveryProvider
	{
		get
		{
			return Modules.ResourceManager.GetString("RemoteDiscoveryFailureFromDiscoveryProvider", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C41 RID: 7233
	// (get) Token: 0x06006802 RID: 26626 RVA: 0x00211424 File Offset: 0x0020F624
	internal static string RemoteDiscoveryProviderNotFound
	{
		get
		{
			return Modules.ResourceManager.GetString("RemoteDiscoveryProviderNotFound", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C42 RID: 7234
	// (get) Token: 0x06006803 RID: 26627 RVA: 0x0021143A File Offset: 0x0020F63A
	internal static string RemoteDiscoveryRemotePsrpCommandFailed
	{
		get
		{
			return Modules.ResourceManager.GetString("RemoteDiscoveryRemotePsrpCommandFailed", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C43 RID: 7235
	// (get) Token: 0x06006804 RID: 26628 RVA: 0x00211450 File Offset: 0x0020F650
	internal static string RemoteDiscoveryWorksOnlyForUnQualifiedNames
	{
		get
		{
			return Modules.ResourceManager.GetString("RemoteDiscoveryWorksOnlyForUnQualifiedNames", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C44 RID: 7236
	// (get) Token: 0x06006805 RID: 26629 RVA: 0x00211466 File Offset: 0x0020F666
	internal static string RemoteDiscoveryWorksOnlyInListAvailableMode
	{
		get
		{
			return Modules.ResourceManager.GetString("RemoteDiscoveryWorksOnlyInListAvailableMode", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C45 RID: 7237
	// (get) Token: 0x06006806 RID: 26630 RVA: 0x0021147C File Offset: 0x0020F67C
	internal static string RemovingImportedAlias
	{
		get
		{
			return Modules.ResourceManager.GetString("RemovingImportedAlias", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C46 RID: 7238
	// (get) Token: 0x06006807 RID: 26631 RVA: 0x00211492 File Offset: 0x0020F692
	internal static string RemovingImportedFunction
	{
		get
		{
			return Modules.ResourceManager.GetString("RemovingImportedFunction", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C47 RID: 7239
	// (get) Token: 0x06006808 RID: 26632 RVA: 0x002114A8 File Offset: 0x0020F6A8
	internal static string RemovingImportedVariable
	{
		get
		{
			return Modules.ResourceManager.GetString("RemovingImportedVariable", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C48 RID: 7240
	// (get) Token: 0x06006809 RID: 26633 RVA: 0x002114BE File Offset: 0x0020F6BE
	internal static string RequiredAssemblies
	{
		get
		{
			return Modules.ResourceManager.GetString("RequiredAssemblies", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C49 RID: 7241
	// (get) Token: 0x0600680A RID: 26634 RVA: 0x002114D4 File Offset: 0x0020F6D4
	internal static string RequiredModuleMissingModuleName
	{
		get
		{
			return Modules.ResourceManager.GetString("RequiredModuleMissingModuleName", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C4A RID: 7242
	// (get) Token: 0x0600680B RID: 26635 RVA: 0x002114EA File Offset: 0x0020F6EA
	internal static string RequiredModuleMissingModuleVersion
	{
		get
		{
			return Modules.ResourceManager.GetString("RequiredModuleMissingModuleVersion", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C4B RID: 7243
	// (get) Token: 0x0600680C RID: 26636 RVA: 0x00211500 File Offset: 0x0020F700
	internal static string RequiredModuleNotFound
	{
		get
		{
			return Modules.ResourceManager.GetString("RequiredModuleNotFound", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C4C RID: 7244
	// (get) Token: 0x0600680D RID: 26637 RVA: 0x00211516 File Offset: 0x0020F716
	internal static string RequiredModuleNotFoundWrongGuidVersion
	{
		get
		{
			return Modules.ResourceManager.GetString("RequiredModuleNotFoundWrongGuidVersion", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C4D RID: 7245
	// (get) Token: 0x0600680E RID: 26638 RVA: 0x0021152C File Offset: 0x0020F72C
	internal static string RequiredModuleNotLoaded
	{
		get
		{
			return Modules.ResourceManager.GetString("RequiredModuleNotLoaded", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C4E RID: 7246
	// (get) Token: 0x0600680F RID: 26639 RVA: 0x00211542 File Offset: 0x0020F742
	internal static string RequiredModuleNotLoadedWrongGuid
	{
		get
		{
			return Modules.ResourceManager.GetString("RequiredModuleNotLoadedWrongGuid", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C4F RID: 7247
	// (get) Token: 0x06006810 RID: 26640 RVA: 0x00211558 File Offset: 0x0020F758
	internal static string RequiredModuleNotLoadedWrongMaximumVersion
	{
		get
		{
			return Modules.ResourceManager.GetString("RequiredModuleNotLoadedWrongMaximumVersion", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C50 RID: 7248
	// (get) Token: 0x06006811 RID: 26641 RVA: 0x0021156E File Offset: 0x0020F76E
	internal static string RequiredModuleNotLoadedWrongMinimumVersionAndMaximumVersion
	{
		get
		{
			return Modules.ResourceManager.GetString("RequiredModuleNotLoadedWrongMinimumVersionAndMaximumVersion", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C51 RID: 7249
	// (get) Token: 0x06006812 RID: 26642 RVA: 0x00211584 File Offset: 0x0020F784
	internal static string RequiredModuleNotLoadedWrongVersion
	{
		get
		{
			return Modules.ResourceManager.GetString("RequiredModuleNotLoadedWrongVersion", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C52 RID: 7250
	// (get) Token: 0x06006813 RID: 26643 RVA: 0x0021159A File Offset: 0x0020F79A
	internal static string RequiredModules
	{
		get
		{
			return Modules.ResourceManager.GetString("RequiredModules", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C53 RID: 7251
	// (get) Token: 0x06006814 RID: 26644 RVA: 0x002115B0 File Offset: 0x0020F7B0
	internal static string RequiredModulesCyclicDependency
	{
		get
		{
			return Modules.ResourceManager.GetString("RequiredModulesCyclicDependency", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C54 RID: 7252
	// (get) Token: 0x06006815 RID: 26645 RVA: 0x002115C6 File Offset: 0x0020F7C6
	internal static string RootModule
	{
		get
		{
			return Modules.ResourceManager.GetString("RootModule", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C55 RID: 7253
	// (get) Token: 0x06006816 RID: 26646 RVA: 0x002115DC File Offset: 0x0020F7DC
	internal static string ScriptAnalysisModule
	{
		get
		{
			return Modules.ResourceManager.GetString("ScriptAnalysisModule", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C56 RID: 7254
	// (get) Token: 0x06006817 RID: 26647 RVA: 0x002115F2 File Offset: 0x0020F7F2
	internal static string ScriptAnalysisPreparing
	{
		get
		{
			return Modules.ResourceManager.GetString("ScriptAnalysisPreparing", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C57 RID: 7255
	// (get) Token: 0x06006818 RID: 26648 RVA: 0x00211608 File Offset: 0x0020F808
	internal static string ScriptsFileNotFound
	{
		get
		{
			return Modules.ResourceManager.GetString("ScriptsFileNotFound", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C58 RID: 7256
	// (get) Token: 0x06006819 RID: 26649 RVA: 0x0021161E File Offset: 0x0020F81E
	internal static string ScriptsToProcess
	{
		get
		{
			return Modules.ResourceManager.GetString("ScriptsToProcess", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C59 RID: 7257
	// (get) Token: 0x0600681A RID: 26650 RVA: 0x00211634 File Offset: 0x0020F834
	internal static string ScriptsToProcessIncorrectExtension
	{
		get
		{
			return Modules.ResourceManager.GetString("ScriptsToProcessIncorrectExtension", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C5A RID: 7258
	// (get) Token: 0x0600681B RID: 26651 RVA: 0x0021164A File Offset: 0x0020F84A
	internal static string SearchingUncShare
	{
		get
		{
			return Modules.ResourceManager.GetString("SearchingUncShare", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C5B RID: 7259
	// (get) Token: 0x0600681C RID: 26652 RVA: 0x00211660 File Offset: 0x0020F860
	internal static string SkippingInvalidModuleVersionFolder
	{
		get
		{
			return Modules.ResourceManager.GetString("SkippingInvalidModuleVersionFolder", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C5C RID: 7260
	// (get) Token: 0x0600681D RID: 26653 RVA: 0x00211676 File Offset: 0x0020F876
	internal static string Tags
	{
		get
		{
			return Modules.ResourceManager.GetString("Tags", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C5D RID: 7261
	// (get) Token: 0x0600681E RID: 26654 RVA: 0x0021168C File Offset: 0x0020F88C
	internal static string TypesFileNotFound
	{
		get
		{
			return Modules.ResourceManager.GetString("TypesFileNotFound", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C5E RID: 7262
	// (get) Token: 0x0600681F RID: 26655 RVA: 0x002116A2 File Offset: 0x0020F8A2
	internal static string TypesToProcess
	{
		get
		{
			return Modules.ResourceManager.GetString("TypesToProcess", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C5F RID: 7263
	// (get) Token: 0x06006820 RID: 26656 RVA: 0x002116B8 File Offset: 0x0020F8B8
	internal static string UnableToRemoveModuleMember
	{
		get
		{
			return Modules.ResourceManager.GetString("UnableToRemoveModuleMember", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C60 RID: 7264
	// (get) Token: 0x06006821 RID: 26657 RVA: 0x002116CE File Offset: 0x0020F8CE
	internal static string VariablesToExport
	{
		get
		{
			return Modules.ResourceManager.GetString("VariablesToExport", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C61 RID: 7265
	// (get) Token: 0x06006822 RID: 26658 RVA: 0x002116E4 File Offset: 0x0020F8E4
	internal static string WildCardNotAllowedInModuleToProcessAndInNestedModules
	{
		get
		{
			return Modules.ResourceManager.GetString("WildCardNotAllowedInModuleToProcessAndInNestedModules", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C62 RID: 7266
	// (get) Token: 0x06006823 RID: 26659 RVA: 0x002116FA File Offset: 0x0020F8FA
	internal static string WildCardNotAllowedInRequiredAssemblies
	{
		get
		{
			return Modules.ResourceManager.GetString("WildCardNotAllowedInRequiredAssemblies", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C63 RID: 7267
	// (get) Token: 0x06006824 RID: 26660 RVA: 0x00211710 File Offset: 0x0020F910
	internal static string WorkflowModuleNotSupportedInOneCorePowerShell
	{
		get
		{
			return Modules.ResourceManager.GetString("WorkflowModuleNotSupportedInOneCorePowerShell", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C64 RID: 7268
	// (get) Token: 0x06006825 RID: 26661 RVA: 0x00211726 File Offset: 0x0020F926
	internal static string WorkflowsToExport
	{
		get
		{
			return Modules.ResourceManager.GetString("WorkflowsToExport", Modules.resourceCulture);
		}
	}

	// Token: 0x17001C65 RID: 7269
	// (get) Token: 0x06006826 RID: 26662 RVA: 0x0021173C File Offset: 0x0020F93C
	internal static string XamlWorkflowsNotSupported
	{
		get
		{
			return Modules.ResourceManager.GetString("XamlWorkflowsNotSupported", Modules.resourceCulture);
		}
	}

	// Token: 0x04003265 RID: 12901
	private static ResourceManager resourceMan;

	// Token: 0x04003266 RID: 12902
	private static CultureInfo resourceCulture;
}
