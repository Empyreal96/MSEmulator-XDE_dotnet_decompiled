using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A34 RID: 2612
[DebuggerNonUserCode]
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
internal class WildcardPatternStrings
{
	// Token: 0x0600657C RID: 25980 RVA: 0x0020DCA6 File Offset: 0x0020BEA6
	internal WildcardPatternStrings()
	{
	}

	// Token: 0x170019C3 RID: 6595
	// (get) Token: 0x0600657D RID: 25981 RVA: 0x0020DCB0 File Offset: 0x0020BEB0
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(WildcardPatternStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("WildcardPatternStrings", typeof(WildcardPatternStrings).Assembly);
				WildcardPatternStrings.resourceMan = resourceManager;
			}
			return WildcardPatternStrings.resourceMan;
		}
	}

	// Token: 0x170019C4 RID: 6596
	// (get) Token: 0x0600657E RID: 25982 RVA: 0x0020DCEF File Offset: 0x0020BEEF
	// (set) Token: 0x0600657F RID: 25983 RVA: 0x0020DCF6 File Offset: 0x0020BEF6
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return WildcardPatternStrings.resourceCulture;
		}
		set
		{
			WildcardPatternStrings.resourceCulture = value;
		}
	}

	// Token: 0x170019C5 RID: 6597
	// (get) Token: 0x06006580 RID: 25984 RVA: 0x0020DCFE File Offset: 0x0020BEFE
	internal static string InvalidPattern
	{
		get
		{
			return WildcardPatternStrings.ResourceManager.GetString("InvalidPattern", WildcardPatternStrings.resourceCulture);
		}
	}

	// Token: 0x0400325F RID: 12895
	private static ResourceManager resourceMan;

	// Token: 0x04003260 RID: 12896
	private static CultureInfo resourceCulture;
}
