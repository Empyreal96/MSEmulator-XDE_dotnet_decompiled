using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A2F RID: 2607
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
internal class MshHostRawUserInterfaceStrings
{
	// Token: 0x0600650C RID: 25868 RVA: 0x0020D30E File Offset: 0x0020B50E
	internal MshHostRawUserInterfaceStrings()
	{
	}

	// Token: 0x1700195D RID: 6493
	// (get) Token: 0x0600650D RID: 25869 RVA: 0x0020D318 File Offset: 0x0020B518
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(MshHostRawUserInterfaceStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("MshHostRawUserInterfaceStrings", typeof(MshHostRawUserInterfaceStrings).Assembly);
				MshHostRawUserInterfaceStrings.resourceMan = resourceManager;
			}
			return MshHostRawUserInterfaceStrings.resourceMan;
		}
	}

	// Token: 0x1700195E RID: 6494
	// (get) Token: 0x0600650E RID: 25870 RVA: 0x0020D357 File Offset: 0x0020B557
	// (set) Token: 0x0600650F RID: 25871 RVA: 0x0020D35E File Offset: 0x0020B55E
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return MshHostRawUserInterfaceStrings.resourceCulture;
		}
		set
		{
			MshHostRawUserInterfaceStrings.resourceCulture = value;
		}
	}

	// Token: 0x1700195F RID: 6495
	// (get) Token: 0x06006510 RID: 25872 RVA: 0x0020D366 File Offset: 0x0020B566
	internal static string AllNullOrEmptyStringsErrorTemplate
	{
		get
		{
			return MshHostRawUserInterfaceStrings.ResourceManager.GetString("AllNullOrEmptyStringsErrorTemplate", MshHostRawUserInterfaceStrings.resourceCulture);
		}
	}

	// Token: 0x17001960 RID: 6496
	// (get) Token: 0x06006511 RID: 25873 RVA: 0x0020D37C File Offset: 0x0020B57C
	internal static string LessThanErrorTemplate
	{
		get
		{
			return MshHostRawUserInterfaceStrings.ResourceManager.GetString("LessThanErrorTemplate", MshHostRawUserInterfaceStrings.resourceCulture);
		}
	}

	// Token: 0x17001961 RID: 6497
	// (get) Token: 0x06006512 RID: 25874 RVA: 0x0020D392 File Offset: 0x0020B592
	internal static string NonPositiveNumberErrorTemplate
	{
		get
		{
			return MshHostRawUserInterfaceStrings.ResourceManager.GetString("NonPositiveNumberErrorTemplate", MshHostRawUserInterfaceStrings.resourceCulture);
		}
	}

	// Token: 0x04003255 RID: 12885
	private static ResourceManager resourceMan;

	// Token: 0x04003256 RID: 12886
	private static CultureInfo resourceCulture;
}
