using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A13 RID: 2579
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
internal class HistoryStrings
{
	// Token: 0x060060A8 RID: 24744 RVA: 0x00207296 File Offset: 0x00205496
	internal HistoryStrings()
	{
	}

	// Token: 0x17001531 RID: 5425
	// (get) Token: 0x060060A9 RID: 24745 RVA: 0x002072A0 File Offset: 0x002054A0
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(HistoryStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("HistoryStrings", typeof(HistoryStrings).Assembly);
				HistoryStrings.resourceMan = resourceManager;
			}
			return HistoryStrings.resourceMan;
		}
	}

	// Token: 0x17001532 RID: 5426
	// (get) Token: 0x060060AA RID: 24746 RVA: 0x002072DF File Offset: 0x002054DF
	// (set) Token: 0x060060AB RID: 24747 RVA: 0x002072E6 File Offset: 0x002054E6
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return HistoryStrings.resourceCulture;
		}
		set
		{
			HistoryStrings.resourceCulture = value;
		}
	}

	// Token: 0x17001533 RID: 5427
	// (get) Token: 0x060060AC RID: 24748 RVA: 0x002072EE File Offset: 0x002054EE
	internal static string AddHistoryInvalidInput
	{
		get
		{
			return HistoryStrings.ResourceManager.GetString("AddHistoryInvalidInput", HistoryStrings.resourceCulture);
		}
	}

	// Token: 0x17001534 RID: 5428
	// (get) Token: 0x060060AD RID: 24749 RVA: 0x00207304 File Offset: 0x00205504
	internal static string ClearHistoryResult
	{
		get
		{
			return HistoryStrings.ResourceManager.GetString("ClearHistoryResult", HistoryStrings.resourceCulture);
		}
	}

	// Token: 0x17001535 RID: 5429
	// (get) Token: 0x060060AE RID: 24750 RVA: 0x0020731A File Offset: 0x0020551A
	internal static string ClearHistoryWarning
	{
		get
		{
			return HistoryStrings.ResourceManager.GetString("ClearHistoryWarning", HistoryStrings.resourceCulture);
		}
	}

	// Token: 0x17001536 RID: 5430
	// (get) Token: 0x060060AF RID: 24751 RVA: 0x00207330 File Offset: 0x00205530
	internal static string InvalidCountValue
	{
		get
		{
			return HistoryStrings.ResourceManager.GetString("InvalidCountValue", HistoryStrings.resourceCulture);
		}
	}

	// Token: 0x17001537 RID: 5431
	// (get) Token: 0x060060B0 RID: 24752 RVA: 0x00207346 File Offset: 0x00205546
	internal static string InvalidIdGetHistory
	{
		get
		{
			return HistoryStrings.ResourceManager.GetString("InvalidIdGetHistory", HistoryStrings.resourceCulture);
		}
	}

	// Token: 0x17001538 RID: 5432
	// (get) Token: 0x060060B1 RID: 24753 RVA: 0x0020735C File Offset: 0x0020555C
	internal static string InvokeHistoryLoopDetected
	{
		get
		{
			return HistoryStrings.ResourceManager.GetString("InvokeHistoryLoopDetected", HistoryStrings.resourceCulture);
		}
	}

	// Token: 0x17001539 RID: 5433
	// (get) Token: 0x060060B2 RID: 24754 RVA: 0x00207372 File Offset: 0x00205572
	internal static string InvokeHistoryMultipleCommandsError
	{
		get
		{
			return HistoryStrings.ResourceManager.GetString("InvokeHistoryMultipleCommandsError", HistoryStrings.resourceCulture);
		}
	}

	// Token: 0x1700153A RID: 5434
	// (get) Token: 0x060060B3 RID: 24755 RVA: 0x00207388 File Offset: 0x00205588
	internal static string NoCountWithMultipleCmdLine
	{
		get
		{
			return HistoryStrings.ResourceManager.GetString("NoCountWithMultipleCmdLine", HistoryStrings.resourceCulture);
		}
	}

	// Token: 0x1700153B RID: 5435
	// (get) Token: 0x060060B4 RID: 24756 RVA: 0x0020739E File Offset: 0x0020559E
	internal static string NoCountWithMultipleIds
	{
		get
		{
			return HistoryStrings.ResourceManager.GetString("NoCountWithMultipleIds", HistoryStrings.resourceCulture);
		}
	}

	// Token: 0x1700153C RID: 5436
	// (get) Token: 0x060060B5 RID: 24757 RVA: 0x002073B4 File Offset: 0x002055B4
	internal static string NoHistoryForCommandline
	{
		get
		{
			return HistoryStrings.ResourceManager.GetString("NoHistoryForCommandline", HistoryStrings.resourceCulture);
		}
	}

	// Token: 0x1700153D RID: 5437
	// (get) Token: 0x060060B6 RID: 24758 RVA: 0x002073CA File Offset: 0x002055CA
	internal static string NoHistoryForId
	{
		get
		{
			return HistoryStrings.ResourceManager.GetString("NoHistoryForId", HistoryStrings.resourceCulture);
		}
	}

	// Token: 0x1700153E RID: 5438
	// (get) Token: 0x060060B7 RID: 24759 RVA: 0x002073E0 File Offset: 0x002055E0
	internal static string NoHistoryFound
	{
		get
		{
			return HistoryStrings.ResourceManager.GetString("NoHistoryFound", HistoryStrings.resourceCulture);
		}
	}

	// Token: 0x1700153F RID: 5439
	// (get) Token: 0x060060B8 RID: 24760 RVA: 0x002073F6 File Offset: 0x002055F6
	internal static string NoLastHistoryEntryFound
	{
		get
		{
			return HistoryStrings.ResourceManager.GetString("NoLastHistoryEntryFound", HistoryStrings.resourceCulture);
		}
	}

	// Token: 0x0400321D RID: 12829
	private static ResourceManager resourceMan;

	// Token: 0x0400321E RID: 12830
	private static CultureInfo resourceCulture;
}
