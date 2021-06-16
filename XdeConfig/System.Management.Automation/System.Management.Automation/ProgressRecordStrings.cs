using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A1F RID: 2591
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
internal class ProgressRecordStrings
{
	// Token: 0x06006360 RID: 25440 RVA: 0x0020AE5A File Offset: 0x0020905A
	internal ProgressRecordStrings()
	{
	}

	// Token: 0x170017D1 RID: 6097
	// (get) Token: 0x06006361 RID: 25441 RVA: 0x0020AE64 File Offset: 0x00209064
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(ProgressRecordStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("ProgressRecordStrings", typeof(ProgressRecordStrings).Assembly);
				ProgressRecordStrings.resourceMan = resourceManager;
			}
			return ProgressRecordStrings.resourceMan;
		}
	}

	// Token: 0x170017D2 RID: 6098
	// (get) Token: 0x06006362 RID: 25442 RVA: 0x0020AEA3 File Offset: 0x002090A3
	// (set) Token: 0x06006363 RID: 25443 RVA: 0x0020AEAA File Offset: 0x002090AA
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return ProgressRecordStrings.resourceCulture;
		}
		set
		{
			ProgressRecordStrings.resourceCulture = value;
		}
	}

	// Token: 0x170017D3 RID: 6099
	// (get) Token: 0x06006364 RID: 25444 RVA: 0x0020AEB2 File Offset: 0x002090B2
	internal static string ArgMayNotBeNegative
	{
		get
		{
			return ProgressRecordStrings.ResourceManager.GetString("ArgMayNotBeNegative", ProgressRecordStrings.resourceCulture);
		}
	}

	// Token: 0x170017D4 RID: 6100
	// (get) Token: 0x06006365 RID: 25445 RVA: 0x0020AEC8 File Offset: 0x002090C8
	internal static string ArgMayNotBeNullOrEmpty
	{
		get
		{
			return ProgressRecordStrings.ResourceManager.GetString("ArgMayNotBeNullOrEmpty", ProgressRecordStrings.resourceCulture);
		}
	}

	// Token: 0x170017D5 RID: 6101
	// (get) Token: 0x06006366 RID: 25446 RVA: 0x0020AEDE File Offset: 0x002090DE
	internal static string ParentActivityIdCantBeActivityId
	{
		get
		{
			return ProgressRecordStrings.ResourceManager.GetString("ParentActivityIdCantBeActivityId", ProgressRecordStrings.resourceCulture);
		}
	}

	// Token: 0x170017D6 RID: 6102
	// (get) Token: 0x06006367 RID: 25447 RVA: 0x0020AEF4 File Offset: 0x002090F4
	internal static string PercentMayNotBeMoreThan100
	{
		get
		{
			return ProgressRecordStrings.ResourceManager.GetString("PercentMayNotBeMoreThan100", ProgressRecordStrings.resourceCulture);
		}
	}

	// Token: 0x04003235 RID: 12853
	private static ResourceManager resourceMan;

	// Token: 0x04003236 RID: 12854
	private static CultureInfo resourceCulture;
}
