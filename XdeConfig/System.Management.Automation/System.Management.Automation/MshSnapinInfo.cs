using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A31 RID: 2609
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[CompilerGenerated]
[DebuggerNonUserCode]
internal class MshSnapinInfo
{
	// Token: 0x0600651C RID: 25884 RVA: 0x0020D46C File Offset: 0x0020B66C
	internal MshSnapinInfo()
	{
	}

	// Token: 0x17001969 RID: 6505
	// (get) Token: 0x0600651D RID: 25885 RVA: 0x0020D474 File Offset: 0x0020B674
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(MshSnapinInfo.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("MshSnapinInfo", typeof(MshSnapinInfo).Assembly);
				MshSnapinInfo.resourceMan = resourceManager;
			}
			return MshSnapinInfo.resourceMan;
		}
	}

	// Token: 0x1700196A RID: 6506
	// (get) Token: 0x0600651E RID: 25886 RVA: 0x0020D4B3 File Offset: 0x0020B6B3
	// (set) Token: 0x0600651F RID: 25887 RVA: 0x0020D4BA File Offset: 0x0020B6BA
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return MshSnapinInfo.resourceCulture;
		}
		set
		{
			MshSnapinInfo.resourceCulture = value;
		}
	}

	// Token: 0x1700196B RID: 6507
	// (get) Token: 0x06006520 RID: 25888 RVA: 0x0020D4C2 File Offset: 0x0020B6C2
	internal static string DefaultMshSnapinNotPresent
	{
		get
		{
			return MshSnapinInfo.ResourceManager.GetString("DefaultMshSnapinNotPresent", MshSnapinInfo.resourceCulture);
		}
	}

	// Token: 0x1700196C RID: 6508
	// (get) Token: 0x06006521 RID: 25889 RVA: 0x0020D4D8 File Offset: 0x0020B6D8
	internal static string MandatoryValueNotInCorrectFormat
	{
		get
		{
			return MshSnapinInfo.ResourceManager.GetString("MandatoryValueNotInCorrectFormat", MshSnapinInfo.resourceCulture);
		}
	}

	// Token: 0x1700196D RID: 6509
	// (get) Token: 0x06006522 RID: 25890 RVA: 0x0020D4EE File Offset: 0x0020B6EE
	internal static string MandatoryValueNotInCorrectFormatMultiString
	{
		get
		{
			return MshSnapinInfo.ResourceManager.GetString("MandatoryValueNotInCorrectFormatMultiString", MshSnapinInfo.resourceCulture);
		}
	}

	// Token: 0x1700196E RID: 6510
	// (get) Token: 0x06006523 RID: 25891 RVA: 0x0020D504 File Offset: 0x0020B704
	internal static string MandatoryValueNotPresent
	{
		get
		{
			return MshSnapinInfo.ResourceManager.GetString("MandatoryValueNotPresent", MshSnapinInfo.resourceCulture);
		}
	}

	// Token: 0x1700196F RID: 6511
	// (get) Token: 0x06006524 RID: 25892 RVA: 0x0020D51A File Offset: 0x0020B71A
	internal static string MonadEngineRegistryAccessFailed
	{
		get
		{
			return MshSnapinInfo.ResourceManager.GetString("MonadEngineRegistryAccessFailed", MshSnapinInfo.resourceCulture);
		}
	}

	// Token: 0x17001970 RID: 6512
	// (get) Token: 0x06006525 RID: 25893 RVA: 0x0020D530 File Offset: 0x0020B730
	internal static string MonadRootRegistryAccessFailed
	{
		get
		{
			return MshSnapinInfo.ResourceManager.GetString("MonadRootRegistryAccessFailed", MshSnapinInfo.resourceCulture);
		}
	}

	// Token: 0x17001971 RID: 6513
	// (get) Token: 0x06006526 RID: 25894 RVA: 0x0020D546 File Offset: 0x0020B746
	internal static string MshSnapinDoesNotExist
	{
		get
		{
			return MshSnapinInfo.ResourceManager.GetString("MshSnapinDoesNotExist", MshSnapinInfo.resourceCulture);
		}
	}

	// Token: 0x17001972 RID: 6514
	// (get) Token: 0x06006527 RID: 25895 RVA: 0x0020D55C File Offset: 0x0020B75C
	internal static string NoMshSnapinPresentForVersion
	{
		get
		{
			return MshSnapinInfo.ResourceManager.GetString("NoMshSnapinPresentForVersion", MshSnapinInfo.resourceCulture);
		}
	}

	// Token: 0x17001973 RID: 6515
	// (get) Token: 0x06006528 RID: 25896 RVA: 0x0020D572 File Offset: 0x0020B772
	internal static string PSVersionAttributeNotExist
	{
		get
		{
			return MshSnapinInfo.ResourceManager.GetString("PSVersionAttributeNotExist", MshSnapinInfo.resourceCulture);
		}
	}

	// Token: 0x17001974 RID: 6516
	// (get) Token: 0x06006529 RID: 25897 RVA: 0x0020D588 File Offset: 0x0020B788
	internal static string PublicKeyTokenAccessFailed
	{
		get
		{
			return MshSnapinInfo.ResourceManager.GetString("PublicKeyTokenAccessFailed", MshSnapinInfo.resourceCulture);
		}
	}

	// Token: 0x17001975 RID: 6517
	// (get) Token: 0x0600652A RID: 25898 RVA: 0x0020D59E File Offset: 0x0020B79E
	internal static string ResourceReaderDisposed
	{
		get
		{
			return MshSnapinInfo.ResourceManager.GetString("ResourceReaderDisposed", MshSnapinInfo.resourceCulture);
		}
	}

	// Token: 0x17001976 RID: 6518
	// (get) Token: 0x0600652B RID: 25899 RVA: 0x0020D5B4 File Offset: 0x0020B7B4
	internal static string SpecifiedVersionNotFound
	{
		get
		{
			return MshSnapinInfo.ResourceManager.GetString("SpecifiedVersionNotFound", MshSnapinInfo.resourceCulture);
		}
	}

	// Token: 0x17001977 RID: 6519
	// (get) Token: 0x0600652C RID: 25900 RVA: 0x0020D5CA File Offset: 0x0020B7CA
	internal static string VersionValueInCorrect
	{
		get
		{
			return MshSnapinInfo.ResourceManager.GetString("VersionValueInCorrect", MshSnapinInfo.resourceCulture);
		}
	}

	// Token: 0x04003259 RID: 12889
	private static ResourceManager resourceMan;

	// Token: 0x0400325A RID: 12890
	private static CultureInfo resourceCulture;
}
