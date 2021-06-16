using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A22 RID: 2594
[DebuggerNonUserCode]
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
internal class PSCommandStrings
{
	// Token: 0x06006373 RID: 25459 RVA: 0x0020AFFA File Offset: 0x002091FA
	internal PSCommandStrings()
	{
	}

	// Token: 0x170017DE RID: 6110
	// (get) Token: 0x06006374 RID: 25460 RVA: 0x0020B004 File Offset: 0x00209204
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(PSCommandStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("PSCommandStrings", typeof(PSCommandStrings).Assembly);
				PSCommandStrings.resourceMan = resourceManager;
			}
			return PSCommandStrings.resourceMan;
		}
	}

	// Token: 0x170017DF RID: 6111
	// (get) Token: 0x06006375 RID: 25461 RVA: 0x0020B043 File Offset: 0x00209243
	// (set) Token: 0x06006376 RID: 25462 RVA: 0x0020B04A File Offset: 0x0020924A
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return PSCommandStrings.resourceCulture;
		}
		set
		{
			PSCommandStrings.resourceCulture = value;
		}
	}

	// Token: 0x170017E0 RID: 6112
	// (get) Token: 0x06006377 RID: 25463 RVA: 0x0020B052 File Offset: 0x00209252
	internal static string ParameterRequiresCommand
	{
		get
		{
			return PSCommandStrings.ResourceManager.GetString("ParameterRequiresCommand", PSCommandStrings.resourceCulture);
		}
	}

	// Token: 0x0400323B RID: 12859
	private static ResourceManager resourceMan;

	// Token: 0x0400323C RID: 12860
	private static CultureInfo resourceCulture;
}
