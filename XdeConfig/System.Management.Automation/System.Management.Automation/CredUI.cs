using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A06 RID: 2566
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[CompilerGenerated]
[DebuggerNonUserCode]
internal class CredUI
{
	// Token: 0x06005EB1 RID: 24241 RVA: 0x0020476C File Offset: 0x0020296C
	internal CredUI()
	{
	}

	// Token: 0x17001354 RID: 4948
	// (get) Token: 0x06005EB2 RID: 24242 RVA: 0x00204774 File Offset: 0x00202974
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(CredUI.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("CredUI", typeof(CredUI).Assembly);
				CredUI.resourceMan = resourceManager;
			}
			return CredUI.resourceMan;
		}
	}

	// Token: 0x17001355 RID: 4949
	// (get) Token: 0x06005EB3 RID: 24243 RVA: 0x002047B3 File Offset: 0x002029B3
	// (set) Token: 0x06005EB4 RID: 24244 RVA: 0x002047BA File Offset: 0x002029BA
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return CredUI.resourceCulture;
		}
		set
		{
			CredUI.resourceCulture = value;
		}
	}

	// Token: 0x17001356 RID: 4950
	// (get) Token: 0x06005EB5 RID: 24245 RVA: 0x002047C2 File Offset: 0x002029C2
	internal static string PromptForCredential_DefaultCaption
	{
		get
		{
			return CredUI.ResourceManager.GetString("PromptForCredential_DefaultCaption", CredUI.resourceCulture);
		}
	}

	// Token: 0x17001357 RID: 4951
	// (get) Token: 0x06005EB6 RID: 24246 RVA: 0x002047D8 File Offset: 0x002029D8
	internal static string PromptForCredential_DefaultMessage
	{
		get
		{
			return CredUI.ResourceManager.GetString("PromptForCredential_DefaultMessage", CredUI.resourceCulture);
		}
	}

	// Token: 0x17001358 RID: 4952
	// (get) Token: 0x06005EB7 RID: 24247 RVA: 0x002047EE File Offset: 0x002029EE
	internal static string PromptForCredential_DefaultTarget
	{
		get
		{
			return CredUI.ResourceManager.GetString("PromptForCredential_DefaultTarget", CredUI.resourceCulture);
		}
	}

	// Token: 0x17001359 RID: 4953
	// (get) Token: 0x06005EB8 RID: 24248 RVA: 0x00204804 File Offset: 0x00202A04
	internal static string PromptForCredential_InvalidCaption
	{
		get
		{
			return CredUI.ResourceManager.GetString("PromptForCredential_InvalidCaption", CredUI.resourceCulture);
		}
	}

	// Token: 0x1700135A RID: 4954
	// (get) Token: 0x06005EB9 RID: 24249 RVA: 0x0020481A File Offset: 0x00202A1A
	internal static string PromptForCredential_InvalidMessage
	{
		get
		{
			return CredUI.ResourceManager.GetString("PromptForCredential_InvalidMessage", CredUI.resourceCulture);
		}
	}

	// Token: 0x1700135B RID: 4955
	// (get) Token: 0x06005EBA RID: 24250 RVA: 0x00204830 File Offset: 0x00202A30
	internal static string PromptForCredential_InvalidUserName
	{
		get
		{
			return CredUI.ResourceManager.GetString("PromptForCredential_InvalidUserName", CredUI.resourceCulture);
		}
	}

	// Token: 0x04003203 RID: 12803
	private static ResourceManager resourceMan;

	// Token: 0x04003204 RID: 12804
	private static CultureInfo resourceCulture;
}
