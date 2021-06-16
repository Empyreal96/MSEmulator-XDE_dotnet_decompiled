using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A29 RID: 2601
[DebuggerNonUserCode]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[CompilerGenerated]
internal class SecuritySupportStrings
{
	// Token: 0x060063F1 RID: 25585 RVA: 0x0020BAC4 File Offset: 0x00209CC4
	internal SecuritySupportStrings()
	{
	}

	// Token: 0x1700184E RID: 6222
	// (get) Token: 0x060063F2 RID: 25586 RVA: 0x0020BACC File Offset: 0x00209CCC
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(SecuritySupportStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("SecuritySupportStrings", typeof(SecuritySupportStrings).Assembly);
				SecuritySupportStrings.resourceMan = resourceManager;
			}
			return SecuritySupportStrings.resourceMan;
		}
	}

	// Token: 0x1700184F RID: 6223
	// (get) Token: 0x060063F3 RID: 25587 RVA: 0x0020BB0B File Offset: 0x00209D0B
	// (set) Token: 0x060063F4 RID: 25588 RVA: 0x0020BB12 File Offset: 0x00209D12
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return SecuritySupportStrings.resourceCulture;
		}
		set
		{
			SecuritySupportStrings.resourceCulture = value;
		}
	}

	// Token: 0x17001850 RID: 6224
	// (get) Token: 0x060063F5 RID: 25589 RVA: 0x0020BB1A File Offset: 0x00209D1A
	internal static string CertificateCannotBeUsedForEncryption
	{
		get
		{
			return SecuritySupportStrings.ResourceManager.GetString("CertificateCannotBeUsedForEncryption", SecuritySupportStrings.resourceCulture);
		}
	}

	// Token: 0x17001851 RID: 6225
	// (get) Token: 0x060063F6 RID: 25590 RVA: 0x0020BB30 File Offset: 0x00209D30
	internal static string CertificateContainsPrivateKey
	{
		get
		{
			return SecuritySupportStrings.ResourceManager.GetString("CertificateContainsPrivateKey", SecuritySupportStrings.resourceCulture);
		}
	}

	// Token: 0x17001852 RID: 6226
	// (get) Token: 0x060063F7 RID: 25591 RVA: 0x0020BB46 File Offset: 0x00209D46
	internal static string CertificatePathMustBeFileSystemPath
	{
		get
		{
			return SecuritySupportStrings.ResourceManager.GetString("CertificatePathMustBeFileSystemPath", SecuritySupportStrings.resourceCulture);
		}
	}

	// Token: 0x17001853 RID: 6227
	// (get) Token: 0x060063F8 RID: 25592 RVA: 0x0020BB5C File Offset: 0x00209D5C
	internal static string CouldNotEncryptContent
	{
		get
		{
			return SecuritySupportStrings.ResourceManager.GetString("CouldNotEncryptContent", SecuritySupportStrings.resourceCulture);
		}
	}

	// Token: 0x17001854 RID: 6228
	// (get) Token: 0x060063F9 RID: 25593 RVA: 0x0020BB72 File Offset: 0x00209D72
	internal static string CouldNotUseCertificate
	{
		get
		{
			return SecuritySupportStrings.ResourceManager.GetString("CouldNotUseCertificate", SecuritySupportStrings.resourceCulture);
		}
	}

	// Token: 0x17001855 RID: 6229
	// (get) Token: 0x060063FA RID: 25594 RVA: 0x0020BB88 File Offset: 0x00209D88
	internal static string IdentifierMustReferenceSingleCertificate
	{
		get
		{
			return SecuritySupportStrings.ResourceManager.GetString("IdentifierMustReferenceSingleCertificate", SecuritySupportStrings.resourceCulture);
		}
	}

	// Token: 0x17001856 RID: 6230
	// (get) Token: 0x060063FB RID: 25595 RVA: 0x0020BB9E File Offset: 0x00209D9E
	internal static string NoCertificateFound
	{
		get
		{
			return SecuritySupportStrings.ResourceManager.GetString("NoCertificateFound", SecuritySupportStrings.resourceCulture);
		}
	}

	// Token: 0x04003249 RID: 12873
	private static ResourceManager resourceMan;

	// Token: 0x0400324A RID: 12874
	private static CultureInfo resourceCulture;
}
