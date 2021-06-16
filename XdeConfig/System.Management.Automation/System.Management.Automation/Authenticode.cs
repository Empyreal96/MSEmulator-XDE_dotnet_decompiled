using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x020009FE RID: 2558
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
internal class Authenticode
{
	// Token: 0x06005E00 RID: 24064 RVA: 0x0020383B File Offset: 0x00201A3B
	internal Authenticode()
	{
	}

	// Token: 0x170012B3 RID: 4787
	// (get) Token: 0x06005E01 RID: 24065 RVA: 0x00203844 File Offset: 0x00201A44
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(Authenticode.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("Authenticode", typeof(Authenticode).Assembly);
				Authenticode.resourceMan = resourceManager;
			}
			return Authenticode.resourceMan;
		}
	}

	// Token: 0x170012B4 RID: 4788
	// (get) Token: 0x06005E02 RID: 24066 RVA: 0x00203883 File Offset: 0x00201A83
	// (set) Token: 0x06005E03 RID: 24067 RVA: 0x0020388A File Offset: 0x00201A8A
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return Authenticode.resourceCulture;
		}
		set
		{
			Authenticode.resourceCulture = value;
		}
	}

	// Token: 0x170012B5 RID: 4789
	// (get) Token: 0x06005E04 RID: 24068 RVA: 0x00203892 File Offset: 0x00201A92
	internal static string AuthenticodePromptCaption
	{
		get
		{
			return Authenticode.ResourceManager.GetString("AuthenticodePromptCaption", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012B6 RID: 4790
	// (get) Token: 0x06005E05 RID: 24069 RVA: 0x002038A8 File Offset: 0x00201AA8
	internal static string AuthenticodePromptText
	{
		get
		{
			return Authenticode.ResourceManager.GetString("AuthenticodePromptText", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012B7 RID: 4791
	// (get) Token: 0x06005E06 RID: 24070 RVA: 0x002038BE File Offset: 0x00201ABE
	internal static string AuthenticodePromptText_UnknownPublisher
	{
		get
		{
			return Authenticode.ResourceManager.GetString("AuthenticodePromptText_UnknownPublisher", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012B8 RID: 4792
	// (get) Token: 0x06005E07 RID: 24071 RVA: 0x002038D4 File Offset: 0x00201AD4
	internal static string CertNotGoodForSigning
	{
		get
		{
			return Authenticode.ResourceManager.GetString("CertNotGoodForSigning", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012B9 RID: 4793
	// (get) Token: 0x06005E08 RID: 24072 RVA: 0x002038EA File Offset: 0x00201AEA
	internal static string Choice_AlwaysRun
	{
		get
		{
			return Authenticode.ResourceManager.GetString("Choice_AlwaysRun", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012BA RID: 4794
	// (get) Token: 0x06005E09 RID: 24073 RVA: 0x00203900 File Offset: 0x00201B00
	internal static string Choice_AlwaysRun_Help
	{
		get
		{
			return Authenticode.ResourceManager.GetString("Choice_AlwaysRun_Help", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012BB RID: 4795
	// (get) Token: 0x06005E0A RID: 24074 RVA: 0x00203916 File Offset: 0x00201B16
	internal static string Choice_DoNotRun
	{
		get
		{
			return Authenticode.ResourceManager.GetString("Choice_DoNotRun", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012BC RID: 4796
	// (get) Token: 0x06005E0B RID: 24075 RVA: 0x0020392C File Offset: 0x00201B2C
	internal static string Choice_DoNotRun_Help
	{
		get
		{
			return Authenticode.ResourceManager.GetString("Choice_DoNotRun_Help", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012BD RID: 4797
	// (get) Token: 0x06005E0C RID: 24076 RVA: 0x00203942 File Offset: 0x00201B42
	internal static string Choice_NeverRun
	{
		get
		{
			return Authenticode.ResourceManager.GetString("Choice_NeverRun", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012BE RID: 4798
	// (get) Token: 0x06005E0D RID: 24077 RVA: 0x00203958 File Offset: 0x00201B58
	internal static string Choice_NeverRun_Help
	{
		get
		{
			return Authenticode.ResourceManager.GetString("Choice_NeverRun_Help", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012BF RID: 4799
	// (get) Token: 0x06005E0E RID: 24078 RVA: 0x0020396E File Offset: 0x00201B6E
	internal static string Choice_RunOnce
	{
		get
		{
			return Authenticode.ResourceManager.GetString("Choice_RunOnce", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012C0 RID: 4800
	// (get) Token: 0x06005E0F RID: 24079 RVA: 0x00203984 File Offset: 0x00201B84
	internal static string Choice_RunOnce_Help
	{
		get
		{
			return Authenticode.ResourceManager.GetString("Choice_RunOnce_Help", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012C1 RID: 4801
	// (get) Token: 0x06005E10 RID: 24080 RVA: 0x0020399A File Offset: 0x00201B9A
	internal static string Choice_Suspend
	{
		get
		{
			return Authenticode.ResourceManager.GetString("Choice_Suspend", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012C2 RID: 4802
	// (get) Token: 0x06005E11 RID: 24081 RVA: 0x002039B0 File Offset: 0x00201BB0
	internal static string Choice_Suspend_Help
	{
		get
		{
			return Authenticode.ResourceManager.GetString("Choice_Suspend_Help", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012C3 RID: 4803
	// (get) Token: 0x06005E12 RID: 24082 RVA: 0x002039C6 File Offset: 0x00201BC6
	internal static string InvalidHashAlgorithm
	{
		get
		{
			return Authenticode.ResourceManager.GetString("InvalidHashAlgorithm", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012C4 RID: 4804
	// (get) Token: 0x06005E13 RID: 24083 RVA: 0x002039DC File Offset: 0x00201BDC
	internal static string Reason_DisallowedBySafer
	{
		get
		{
			return Authenticode.ResourceManager.GetString("Reason_DisallowedBySafer", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012C5 RID: 4805
	// (get) Token: 0x06005E14 RID: 24084 RVA: 0x002039F2 File Offset: 0x00201BF2
	internal static string Reason_DoNotRun
	{
		get
		{
			return Authenticode.ResourceManager.GetString("Reason_DoNotRun", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012C6 RID: 4806
	// (get) Token: 0x06005E15 RID: 24085 RVA: 0x00203A08 File Offset: 0x00201C08
	internal static string Reason_FileContentUnavailable
	{
		get
		{
			return Authenticode.ResourceManager.GetString("Reason_FileContentUnavailable", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012C7 RID: 4807
	// (get) Token: 0x06005E16 RID: 24086 RVA: 0x00203A1E File Offset: 0x00201C1E
	internal static string Reason_NeverRun
	{
		get
		{
			return Authenticode.ResourceManager.GetString("Reason_NeverRun", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012C8 RID: 4808
	// (get) Token: 0x06005E17 RID: 24087 RVA: 0x00203A34 File Offset: 0x00201C34
	internal static string Reason_NotTrusted
	{
		get
		{
			return Authenticode.ResourceManager.GetString("Reason_NotTrusted", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012C9 RID: 4809
	// (get) Token: 0x06005E18 RID: 24088 RVA: 0x00203A4A File Offset: 0x00201C4A
	internal static string Reason_RestrictedMode
	{
		get
		{
			return Authenticode.ResourceManager.GetString("Reason_RestrictedMode", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012CA RID: 4810
	// (get) Token: 0x06005E19 RID: 24089 RVA: 0x00203A60 File Offset: 0x00201C60
	internal static string Reason_Unknown
	{
		get
		{
			return Authenticode.ResourceManager.GetString("Reason_Unknown", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012CB RID: 4811
	// (get) Token: 0x06005E1A RID: 24090 RVA: 0x00203A76 File Offset: 0x00201C76
	internal static string RemoteFilePromptCaption
	{
		get
		{
			return Authenticode.ResourceManager.GetString("RemoteFilePromptCaption", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012CC RID: 4812
	// (get) Token: 0x06005E1B RID: 24091 RVA: 0x00203A8C File Offset: 0x00201C8C
	internal static string RemoteFilePromptText
	{
		get
		{
			return Authenticode.ResourceManager.GetString("RemoteFilePromptText", Authenticode.resourceCulture);
		}
	}

	// Token: 0x170012CD RID: 4813
	// (get) Token: 0x06005E1C RID: 24092 RVA: 0x00203AA2 File Offset: 0x00201CA2
	internal static string TimeStampUrlRequired
	{
		get
		{
			return Authenticode.ResourceManager.GetString("TimeStampUrlRequired", Authenticode.resourceCulture);
		}
	}

	// Token: 0x040031F3 RID: 12787
	private static ResourceManager resourceMan;

	// Token: 0x040031F4 RID: 12788
	private static CultureInfo resourceCulture;
}
