using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A21 RID: 2593
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[CompilerGenerated]
[DebuggerNonUserCode]
internal class ProxyCommandStrings
{
	// Token: 0x0600636D RID: 25453 RVA: 0x0020AF78 File Offset: 0x00209178
	internal ProxyCommandStrings()
	{
	}

	// Token: 0x170017DA RID: 6106
	// (get) Token: 0x0600636E RID: 25454 RVA: 0x0020AF80 File Offset: 0x00209180
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(ProxyCommandStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("ProxyCommandStrings", typeof(ProxyCommandStrings).Assembly);
				ProxyCommandStrings.resourceMan = resourceManager;
			}
			return ProxyCommandStrings.resourceMan;
		}
	}

	// Token: 0x170017DB RID: 6107
	// (get) Token: 0x0600636F RID: 25455 RVA: 0x0020AFBF File Offset: 0x002091BF
	// (set) Token: 0x06006370 RID: 25456 RVA: 0x0020AFC6 File Offset: 0x002091C6
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return ProxyCommandStrings.resourceCulture;
		}
		set
		{
			ProxyCommandStrings.resourceCulture = value;
		}
	}

	// Token: 0x170017DC RID: 6108
	// (get) Token: 0x06006371 RID: 25457 RVA: 0x0020AFCE File Offset: 0x002091CE
	internal static string CommandMetadataMissingCommandName
	{
		get
		{
			return ProxyCommandStrings.ResourceManager.GetString("CommandMetadataMissingCommandName", ProxyCommandStrings.resourceCulture);
		}
	}

	// Token: 0x170017DD RID: 6109
	// (get) Token: 0x06006372 RID: 25458 RVA: 0x0020AFE4 File Offset: 0x002091E4
	internal static string HelpInfoObjectRequired
	{
		get
		{
			return ProxyCommandStrings.ResourceManager.GetString("HelpInfoObjectRequired", ProxyCommandStrings.resourceCulture);
		}
	}

	// Token: 0x04003239 RID: 12857
	private static ResourceManager resourceMan;

	// Token: 0x0400323A RID: 12858
	private static CultureInfo resourceCulture;
}
