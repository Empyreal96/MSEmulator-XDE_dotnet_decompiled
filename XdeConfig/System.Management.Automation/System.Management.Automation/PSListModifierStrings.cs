using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A24 RID: 2596
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
internal class PSListModifierStrings
{
	// Token: 0x06006381 RID: 25473 RVA: 0x0020B12C File Offset: 0x0020932C
	internal PSListModifierStrings()
	{
	}

	// Token: 0x170017E8 RID: 6120
	// (get) Token: 0x06006382 RID: 25474 RVA: 0x0020B134 File Offset: 0x00209334
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(PSListModifierStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("PSListModifierStrings", typeof(PSListModifierStrings).Assembly);
				PSListModifierStrings.resourceMan = resourceManager;
			}
			return PSListModifierStrings.resourceMan;
		}
	}

	// Token: 0x170017E9 RID: 6121
	// (get) Token: 0x06006383 RID: 25475 RVA: 0x0020B173 File Offset: 0x00209373
	// (set) Token: 0x06006384 RID: 25476 RVA: 0x0020B17A File Offset: 0x0020937A
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return PSListModifierStrings.resourceCulture;
		}
		set
		{
			PSListModifierStrings.resourceCulture = value;
		}
	}

	// Token: 0x170017EA RID: 6122
	// (get) Token: 0x06006385 RID: 25477 RVA: 0x0020B182 File Offset: 0x00209382
	internal static string ListModifierDisallowedKey
	{
		get
		{
			return PSListModifierStrings.ResourceManager.GetString("ListModifierDisallowedKey", PSListModifierStrings.resourceCulture);
		}
	}

	// Token: 0x170017EB RID: 6123
	// (get) Token: 0x06006386 RID: 25478 RVA: 0x0020B198 File Offset: 0x00209398
	internal static string UpdateFailed
	{
		get
		{
			return PSListModifierStrings.ResourceManager.GetString("UpdateFailed", PSListModifierStrings.resourceCulture);
		}
	}

	// Token: 0x0400323F RID: 12863
	private static ResourceManager resourceMan;

	// Token: 0x04003240 RID: 12864
	private static CultureInfo resourceCulture;
}
