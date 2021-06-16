using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A17 RID: 2583
[DebuggerNonUserCode]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[CompilerGenerated]
internal class Metadata
{
	// Token: 0x060060EC RID: 24812 RVA: 0x0020786C File Offset: 0x00205A6C
	internal Metadata()
	{
	}

	// Token: 0x1700156D RID: 5485
	// (get) Token: 0x060060ED RID: 24813 RVA: 0x00207874 File Offset: 0x00205A74
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(Metadata.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("Metadata", typeof(Metadata).Assembly);
				Metadata.resourceMan = resourceManager;
			}
			return Metadata.resourceMan;
		}
	}

	// Token: 0x1700156E RID: 5486
	// (get) Token: 0x060060EE RID: 24814 RVA: 0x002078B3 File Offset: 0x00205AB3
	// (set) Token: 0x060060EF RID: 24815 RVA: 0x002078BA File Offset: 0x00205ABA
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return Metadata.resourceCulture;
		}
		set
		{
			Metadata.resourceCulture = value;
		}
	}

	// Token: 0x1700156F RID: 5487
	// (get) Token: 0x060060F0 RID: 24816 RVA: 0x002078C2 File Offset: 0x00205AC2
	internal static string AliasParameterNameAlreadyExistsForCommand
	{
		get
		{
			return Metadata.ResourceManager.GetString("AliasParameterNameAlreadyExistsForCommand", Metadata.resourceCulture);
		}
	}

	// Token: 0x17001570 RID: 5488
	// (get) Token: 0x060060F1 RID: 24817 RVA: 0x002078D8 File Offset: 0x00205AD8
	internal static string ArgumentTransformationArgumentsShouldBeStrings
	{
		get
		{
			return Metadata.ResourceManager.GetString("ArgumentTransformationArgumentsShouldBeStrings", Metadata.resourceCulture);
		}
	}

	// Token: 0x17001571 RID: 5489
	// (get) Token: 0x060060F2 RID: 24818 RVA: 0x002078EE File Offset: 0x00205AEE
	internal static string InvalidMetadataForCurrentValue
	{
		get
		{
			return Metadata.ResourceManager.GetString("InvalidMetadataForCurrentValue", Metadata.resourceCulture);
		}
	}

	// Token: 0x17001572 RID: 5490
	// (get) Token: 0x060060F3 RID: 24819 RVA: 0x00207904 File Offset: 0x00205B04
	internal static string InvalidValueFailure
	{
		get
		{
			return Metadata.ResourceManager.GetString("InvalidValueFailure", Metadata.resourceCulture);
		}
	}

	// Token: 0x17001573 RID: 5491
	// (get) Token: 0x060060F4 RID: 24820 RVA: 0x0020791A File Offset: 0x00205B1A
	internal static string JobDefinitionMustDeriveFromIJobConverter
	{
		get
		{
			return Metadata.ResourceManager.GetString("JobDefinitionMustDeriveFromIJobConverter", Metadata.resourceCulture);
		}
	}

	// Token: 0x17001574 RID: 5492
	// (get) Token: 0x060060F5 RID: 24821 RVA: 0x00207930 File Offset: 0x00205B30
	internal static string MetadataMemberInitialization
	{
		get
		{
			return Metadata.ResourceManager.GetString("MetadataMemberInitialization", Metadata.resourceCulture);
		}
	}

	// Token: 0x17001575 RID: 5493
	// (get) Token: 0x060060F6 RID: 24822 RVA: 0x00207946 File Offset: 0x00205B46
	internal static string ParameterNameAlreadyExistsForCommand
	{
		get
		{
			return Metadata.ResourceManager.GetString("ParameterNameAlreadyExistsForCommand", Metadata.resourceCulture);
		}
	}

	// Token: 0x17001576 RID: 5494
	// (get) Token: 0x060060F7 RID: 24823 RVA: 0x0020795C File Offset: 0x00205B5C
	internal static string ParameterNameConflictsWithAlias
	{
		get
		{
			return Metadata.ResourceManager.GetString("ParameterNameConflictsWithAlias", Metadata.resourceCulture);
		}
	}

	// Token: 0x17001577 RID: 5495
	// (get) Token: 0x060060F8 RID: 24824 RVA: 0x00207972 File Offset: 0x00205B72
	internal static string ParsingTooManyParameterSets
	{
		get
		{
			return Metadata.ResourceManager.GetString("ParsingTooManyParameterSets", Metadata.resourceCulture);
		}
	}

	// Token: 0x17001578 RID: 5496
	// (get) Token: 0x060060F9 RID: 24825 RVA: 0x00207988 File Offset: 0x00205B88
	internal static string ValidateCountMaxLengthFailure
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateCountMaxLengthFailure", Metadata.resourceCulture);
		}
	}

	// Token: 0x17001579 RID: 5497
	// (get) Token: 0x060060FA RID: 24826 RVA: 0x0020799E File Offset: 0x00205B9E
	internal static string ValidateCountMaxLengthSmallerThanMinLength
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateCountMaxLengthSmallerThanMinLength", Metadata.resourceCulture);
		}
	}

	// Token: 0x1700157A RID: 5498
	// (get) Token: 0x060060FB RID: 24827 RVA: 0x002079B4 File Offset: 0x00205BB4
	internal static string ValidateCountMinLengthFailure
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateCountMinLengthFailure", Metadata.resourceCulture);
		}
	}

	// Token: 0x1700157B RID: 5499
	// (get) Token: 0x060060FC RID: 24828 RVA: 0x002079CA File Offset: 0x00205BCA
	internal static string ValidateCountNotInArray
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateCountNotInArray", Metadata.resourceCulture);
		}
	}

	// Token: 0x1700157C RID: 5500
	// (get) Token: 0x060060FD RID: 24829 RVA: 0x002079E0 File Offset: 0x00205BE0
	internal static string ValidateFailureResult
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateFailureResult", Metadata.resourceCulture);
		}
	}

	// Token: 0x1700157D RID: 5501
	// (get) Token: 0x060060FE RID: 24830 RVA: 0x002079F6 File Offset: 0x00205BF6
	internal static string ValidateLengthMaxLengthFailure
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateLengthMaxLengthFailure", Metadata.resourceCulture);
		}
	}

	// Token: 0x1700157E RID: 5502
	// (get) Token: 0x060060FF RID: 24831 RVA: 0x00207A0C File Offset: 0x00205C0C
	internal static string ValidateLengthMaxLengthSmallerThanMinLength
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateLengthMaxLengthSmallerThanMinLength", Metadata.resourceCulture);
		}
	}

	// Token: 0x1700157F RID: 5503
	// (get) Token: 0x06006100 RID: 24832 RVA: 0x00207A22 File Offset: 0x00205C22
	internal static string ValidateLengthMinLengthFailure
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateLengthMinLengthFailure", Metadata.resourceCulture);
		}
	}

	// Token: 0x17001580 RID: 5504
	// (get) Token: 0x06006101 RID: 24833 RVA: 0x00207A38 File Offset: 0x00205C38
	internal static string ValidateLengthNotString
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateLengthNotString", Metadata.resourceCulture);
		}
	}

	// Token: 0x17001581 RID: 5505
	// (get) Token: 0x06006102 RID: 24834 RVA: 0x00207A4E File Offset: 0x00205C4E
	internal static string ValidateNotNullCollectionFailure
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateNotNullCollectionFailure", Metadata.resourceCulture);
		}
	}

	// Token: 0x17001582 RID: 5506
	// (get) Token: 0x06006103 RID: 24835 RVA: 0x00207A64 File Offset: 0x00205C64
	internal static string ValidateNotNullFailure
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateNotNullFailure", Metadata.resourceCulture);
		}
	}

	// Token: 0x17001583 RID: 5507
	// (get) Token: 0x06006104 RID: 24836 RVA: 0x00207A7A File Offset: 0x00205C7A
	internal static string ValidateNotNullOrEmptyCollectionFailure
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateNotNullOrEmptyCollectionFailure", Metadata.resourceCulture);
		}
	}

	// Token: 0x17001584 RID: 5508
	// (get) Token: 0x06006105 RID: 24837 RVA: 0x00207A90 File Offset: 0x00205C90
	internal static string ValidateNotNullOrEmptyFailure
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateNotNullOrEmptyFailure", Metadata.resourceCulture);
		}
	}

	// Token: 0x17001585 RID: 5509
	// (get) Token: 0x06006106 RID: 24838 RVA: 0x00207AA6 File Offset: 0x00205CA6
	internal static string ValidatePatternFailure
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidatePatternFailure", Metadata.resourceCulture);
		}
	}

	// Token: 0x17001586 RID: 5510
	// (get) Token: 0x06006107 RID: 24839 RVA: 0x00207ABC File Offset: 0x00205CBC
	internal static string ValidateRangeElementType
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateRangeElementType", Metadata.resourceCulture);
		}
	}

	// Token: 0x17001587 RID: 5511
	// (get) Token: 0x06006108 RID: 24840 RVA: 0x00207AD2 File Offset: 0x00205CD2
	internal static string ValidateRangeGreaterThanMaxRangeFailure
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateRangeGreaterThanMaxRangeFailure", Metadata.resourceCulture);
		}
	}

	// Token: 0x17001588 RID: 5512
	// (get) Token: 0x06006109 RID: 24841 RVA: 0x00207AE8 File Offset: 0x00205CE8
	internal static string ValidateRangeMaxRangeSmallerThanMinRange
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateRangeMaxRangeSmallerThanMinRange", Metadata.resourceCulture);
		}
	}

	// Token: 0x17001589 RID: 5513
	// (get) Token: 0x0600610A RID: 24842 RVA: 0x00207AFE File Offset: 0x00205CFE
	internal static string ValidateRangeMinRangeMaxRangeType
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateRangeMinRangeMaxRangeType", Metadata.resourceCulture);
		}
	}

	// Token: 0x1700158A RID: 5514
	// (get) Token: 0x0600610B RID: 24843 RVA: 0x00207B14 File Offset: 0x00205D14
	internal static string ValidateRangeNotIComparable
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateRangeNotIComparable", Metadata.resourceCulture);
		}
	}

	// Token: 0x1700158B RID: 5515
	// (get) Token: 0x0600610C RID: 24844 RVA: 0x00207B2A File Offset: 0x00205D2A
	internal static string ValidateRangeSmallerThanMinRangeFailure
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateRangeSmallerThanMinRangeFailure", Metadata.resourceCulture);
		}
	}

	// Token: 0x1700158C RID: 5516
	// (get) Token: 0x0600610D RID: 24845 RVA: 0x00207B40 File Offset: 0x00205D40
	internal static string ValidateScriptFailure
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateScriptFailure", Metadata.resourceCulture);
		}
	}

	// Token: 0x1700158D RID: 5517
	// (get) Token: 0x0600610E RID: 24846 RVA: 0x00207B56 File Offset: 0x00205D56
	internal static string ValidateSetFailure
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateSetFailure", Metadata.resourceCulture);
		}
	}

	// Token: 0x1700158E RID: 5518
	// (get) Token: 0x0600610F RID: 24847 RVA: 0x00207B6C File Offset: 0x00205D6C
	internal static string ValidateVariableName
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateVariableName", Metadata.resourceCulture);
		}
	}

	// Token: 0x1700158F RID: 5519
	// (get) Token: 0x06006110 RID: 24848 RVA: 0x00207B82 File Offset: 0x00205D82
	internal static string ValidateVersionFailure
	{
		get
		{
			return Metadata.ResourceManager.GetString("ValidateVersionFailure", Metadata.resourceCulture);
		}
	}

	// Token: 0x04003225 RID: 12837
	private static ResourceManager resourceMan;

	// Token: 0x04003226 RID: 12838
	private static CultureInfo resourceCulture;
}
