using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A2D RID: 2605
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
internal class SuggestionStrings
{
	// Token: 0x060064F9 RID: 25849 RVA: 0x0020D170 File Offset: 0x0020B370
	internal SuggestionStrings()
	{
	}

	// Token: 0x1700194E RID: 6478
	// (get) Token: 0x060064FA RID: 25850 RVA: 0x0020D178 File Offset: 0x0020B378
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(SuggestionStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("SuggestionStrings", typeof(SuggestionStrings).Assembly);
				SuggestionStrings.resourceMan = resourceManager;
			}
			return SuggestionStrings.resourceMan;
		}
	}

	// Token: 0x1700194F RID: 6479
	// (get) Token: 0x060064FB RID: 25851 RVA: 0x0020D1B7 File Offset: 0x0020B3B7
	// (set) Token: 0x060064FC RID: 25852 RVA: 0x0020D1BE File Offset: 0x0020B3BE
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return SuggestionStrings.resourceCulture;
		}
		set
		{
			SuggestionStrings.resourceCulture = value;
		}
	}

	// Token: 0x17001950 RID: 6480
	// (get) Token: 0x060064FD RID: 25853 RVA: 0x0020D1C6 File Offset: 0x0020B3C6
	internal static string InvalidMatchType
	{
		get
		{
			return SuggestionStrings.ResourceManager.GetString("InvalidMatchType", SuggestionStrings.resourceCulture);
		}
	}

	// Token: 0x17001951 RID: 6481
	// (get) Token: 0x060064FE RID: 25854 RVA: 0x0020D1DC File Offset: 0x0020B3DC
	internal static string RuleMustBeScriptBlock
	{
		get
		{
			return SuggestionStrings.ResourceManager.GetString("RuleMustBeScriptBlock", SuggestionStrings.resourceCulture);
		}
	}

	// Token: 0x17001952 RID: 6482
	// (get) Token: 0x060064FF RID: 25855 RVA: 0x0020D1F2 File Offset: 0x0020B3F2
	internal static string Suggestion_CommandExistsInCurrentDirectory
	{
		get
		{
			return SuggestionStrings.ResourceManager.GetString("Suggestion_CommandExistsInCurrentDirectory", SuggestionStrings.resourceCulture);
		}
	}

	// Token: 0x17001953 RID: 6483
	// (get) Token: 0x06006500 RID: 25856 RVA: 0x0020D208 File Offset: 0x0020B408
	internal static string Suggestion_StartTransaction
	{
		get
		{
			return SuggestionStrings.ResourceManager.GetString("Suggestion_StartTransaction", SuggestionStrings.resourceCulture);
		}
	}

	// Token: 0x17001954 RID: 6484
	// (get) Token: 0x06006501 RID: 25857 RVA: 0x0020D21E File Offset: 0x0020B41E
	internal static string Suggestion_UseTransaction
	{
		get
		{
			return SuggestionStrings.ResourceManager.GetString("Suggestion_UseTransaction", SuggestionStrings.resourceCulture);
		}
	}

	// Token: 0x04003251 RID: 12881
	private static ResourceManager resourceMan;

	// Token: 0x04003252 RID: 12882
	private static CultureInfo resourceCulture;
}
