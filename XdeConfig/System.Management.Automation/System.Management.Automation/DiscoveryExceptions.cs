using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A08 RID: 2568
[DebuggerNonUserCode]
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
internal class DiscoveryExceptions
{
	// Token: 0x06005EC0 RID: 24256 RVA: 0x002048B4 File Offset: 0x00202AB4
	internal DiscoveryExceptions()
	{
	}

	// Token: 0x1700135F RID: 4959
	// (get) Token: 0x06005EC1 RID: 24257 RVA: 0x002048BC File Offset: 0x00202ABC
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(DiscoveryExceptions.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("DiscoveryExceptions", typeof(DiscoveryExceptions).Assembly);
				DiscoveryExceptions.resourceMan = resourceManager;
			}
			return DiscoveryExceptions.resourceMan;
		}
	}

	// Token: 0x17001360 RID: 4960
	// (get) Token: 0x06005EC2 RID: 24258 RVA: 0x002048FB File Offset: 0x00202AFB
	// (set) Token: 0x06005EC3 RID: 24259 RVA: 0x00204902 File Offset: 0x00202B02
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return DiscoveryExceptions.resourceCulture;
		}
		set
		{
			DiscoveryExceptions.resourceCulture = value;
		}
	}

	// Token: 0x17001361 RID: 4961
	// (get) Token: 0x06005EC4 RID: 24260 RVA: 0x0020490A File Offset: 0x00202B0A
	internal static string AliasDeclaredMultipleTimes
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("AliasDeclaredMultipleTimes", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x17001362 RID: 4962
	// (get) Token: 0x06005EC5 RID: 24261 RVA: 0x00204920 File Offset: 0x00202B20
	internal static string AliasNotResolvedException
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("AliasNotResolvedException", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x17001363 RID: 4963
	// (get) Token: 0x06005EC6 RID: 24262 RVA: 0x00204936 File Offset: 0x00202B36
	internal static string CmdletDoesNotDeriveFromCmdletType
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("CmdletDoesNotDeriveFromCmdletType", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x17001364 RID: 4964
	// (get) Token: 0x06005EC7 RID: 24263 RVA: 0x0020494C File Offset: 0x00202B4C
	internal static string CmdletFormatInvalid
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("CmdletFormatInvalid", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x17001365 RID: 4965
	// (get) Token: 0x06005EC8 RID: 24264 RVA: 0x00204962 File Offset: 0x00202B62
	internal static string CmdletNotFoundException
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("CmdletNotFoundException", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x17001366 RID: 4966
	// (get) Token: 0x06005EC9 RID: 24265 RVA: 0x00204978 File Offset: 0x00202B78
	internal static string CommandArgsOnlyForSingleCmdlet
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("CommandArgsOnlyForSingleCmdlet", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x17001367 RID: 4967
	// (get) Token: 0x06005ECA RID: 24266 RVA: 0x0020498E File Offset: 0x00202B8E
	internal static string CommandDiscoveryMissing
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("CommandDiscoveryMissing", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x17001368 RID: 4968
	// (get) Token: 0x06005ECB RID: 24267 RVA: 0x002049A4 File Offset: 0x00202BA4
	internal static string CommandNameNotCmdlet
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("CommandNameNotCmdlet", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x17001369 RID: 4969
	// (get) Token: 0x06005ECC RID: 24268 RVA: 0x002049BA File Offset: 0x00202BBA
	internal static string CommandNotFoundException
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("CommandNotFoundException", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x1700136A RID: 4970
	// (get) Token: 0x06005ECD RID: 24269 RVA: 0x002049D0 File Offset: 0x00202BD0
	internal static string CommandParameterNotFound
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("CommandParameterNotFound", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x1700136B RID: 4971
	// (get) Token: 0x06005ECE RID: 24270 RVA: 0x002049E6 File Offset: 0x00202BE6
	internal static string CompiledCommandParameterMemberMustBeFieldOrProperty
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("CompiledCommandParameterMemberMustBeFieldOrProperty", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x1700136C RID: 4972
	// (get) Token: 0x06005ECF RID: 24271 RVA: 0x002049FC File Offset: 0x00202BFC
	internal static string CouldNotAutoImportMatchingModule
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("CouldNotAutoImportMatchingModule", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x1700136D RID: 4973
	// (get) Token: 0x06005ED0 RID: 24272 RVA: 0x00204A12 File Offset: 0x00202C12
	internal static string CouldNotAutoImportModule
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("CouldNotAutoImportModule", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x1700136E RID: 4974
	// (get) Token: 0x06005ED1 RID: 24273 RVA: 0x00204A28 File Offset: 0x00202C28
	internal static string DotSourceNotSupported
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("DotSourceNotSupported", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x1700136F RID: 4975
	// (get) Token: 0x06005ED2 RID: 24274 RVA: 0x00204A3E File Offset: 0x00202C3E
	internal static string DuplicateAssemblyName
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("DuplicateAssemblyName", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x17001370 RID: 4976
	// (get) Token: 0x06005ED3 RID: 24275 RVA: 0x00204A54 File Offset: 0x00202C54
	internal static string DuplicateCmdletName
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("DuplicateCmdletName", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x17001371 RID: 4977
	// (get) Token: 0x06005ED4 RID: 24276 RVA: 0x00204A6A File Offset: 0x00202C6A
	internal static string DuplicateCmdletProviderName
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("DuplicateCmdletProviderName", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x17001372 RID: 4978
	// (get) Token: 0x06005ED5 RID: 24277 RVA: 0x00204A80 File Offset: 0x00202C80
	internal static string DuplicateScriptName
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("DuplicateScriptName", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x17001373 RID: 4979
	// (get) Token: 0x06005ED6 RID: 24278 RVA: 0x00204A96 File Offset: 0x00202C96
	internal static string ExecutionContextNotSet
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("ExecutionContextNotSet", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x17001374 RID: 4980
	// (get) Token: 0x06005ED7 RID: 24279 RVA: 0x00204AAC File Offset: 0x00202CAC
	internal static string GetCommandShowCommandInfoParamError
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("GetCommandShowCommandInfoParamError", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x17001375 RID: 4981
	// (get) Token: 0x06005ED8 RID: 24280 RVA: 0x00204AC2 File Offset: 0x00202CC2
	internal static string InvalidCmdletNameFormat
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("InvalidCmdletNameFormat", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x17001376 RID: 4982
	// (get) Token: 0x06005ED9 RID: 24281 RVA: 0x00204AD8 File Offset: 0x00202CD8
	internal static string ParameterDeclaredInParameterSetMultipleTimes
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("ParameterDeclaredInParameterSetMultipleTimes", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x17001377 RID: 4983
	// (get) Token: 0x06005EDA RID: 24282 RVA: 0x00204AEE File Offset: 0x00202CEE
	internal static string PSSnapInNameVersion
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("PSSnapInNameVersion", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x17001378 RID: 4984
	// (get) Token: 0x06005EDB RID: 24283 RVA: 0x00204B04 File Offset: 0x00202D04
	internal static string RequiresElevation
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("RequiresElevation", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x17001379 RID: 4985
	// (get) Token: 0x06005EDC RID: 24284 RVA: 0x00204B1A File Offset: 0x00202D1A
	internal static string RequiresInterpreterNotCompatible
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("RequiresInterpreterNotCompatible", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x1700137A RID: 4986
	// (get) Token: 0x06005EDD RID: 24285 RVA: 0x00204B30 File Offset: 0x00202D30
	internal static string RequiresInterpreterNotCompatibleNoPath
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("RequiresInterpreterNotCompatibleNoPath", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x1700137B RID: 4987
	// (get) Token: 0x06005EDE RID: 24286 RVA: 0x00204B46 File Offset: 0x00202D46
	internal static string RequiresMissingModules
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("RequiresMissingModules", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x1700137C RID: 4988
	// (get) Token: 0x06005EDF RID: 24287 RVA: 0x00204B5C File Offset: 0x00202D5C
	internal static string RequiresMissingPSSnapIns
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("RequiresMissingPSSnapIns", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x1700137D RID: 4989
	// (get) Token: 0x06005EE0 RID: 24288 RVA: 0x00204B72 File Offset: 0x00202D72
	internal static string RequiresPSVersionNotCompatible
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("RequiresPSVersionNotCompatible", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x1700137E RID: 4990
	// (get) Token: 0x06005EE1 RID: 24289 RVA: 0x00204B88 File Offset: 0x00202D88
	internal static string RequiresShellIDInvalidForSingleShell
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("RequiresShellIDInvalidForSingleShell", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x1700137F RID: 4991
	// (get) Token: 0x06005EE2 RID: 24290 RVA: 0x00204B9E File Offset: 0x00202D9E
	internal static string ReservedParameterName
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("ReservedParameterName", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x17001380 RID: 4992
	// (get) Token: 0x06005EE3 RID: 24291 RVA: 0x00204BB4 File Offset: 0x00202DB4
	internal static string ScriptRequiresInvalidFormat
	{
		get
		{
			return DiscoveryExceptions.ResourceManager.GetString("ScriptRequiresInvalidFormat", DiscoveryExceptions.resourceCulture);
		}
	}

	// Token: 0x04003207 RID: 12807
	private static ResourceManager resourceMan;

	// Token: 0x04003208 RID: 12808
	private static CultureInfo resourceCulture;
}
