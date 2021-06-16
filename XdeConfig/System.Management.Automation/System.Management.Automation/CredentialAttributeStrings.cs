using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A04 RID: 2564
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class CredentialAttributeStrings
{
	// Token: 0x06005E80 RID: 24192 RVA: 0x00204336 File Offset: 0x00202536
	internal CredentialAttributeStrings()
	{
	}

	// Token: 0x17001327 RID: 4903
	// (get) Token: 0x06005E81 RID: 24193 RVA: 0x00204340 File Offset: 0x00202540
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(CredentialAttributeStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("CredentialAttributeStrings", typeof(CredentialAttributeStrings).Assembly);
				CredentialAttributeStrings.resourceMan = resourceManager;
			}
			return CredentialAttributeStrings.resourceMan;
		}
	}

	// Token: 0x17001328 RID: 4904
	// (get) Token: 0x06005E82 RID: 24194 RVA: 0x0020437F File Offset: 0x0020257F
	// (set) Token: 0x06005E83 RID: 24195 RVA: 0x00204386 File Offset: 0x00202586
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return CredentialAttributeStrings.resourceCulture;
		}
		set
		{
			CredentialAttributeStrings.resourceCulture = value;
		}
	}

	// Token: 0x17001329 RID: 4905
	// (get) Token: 0x06005E84 RID: 24196 RVA: 0x0020438E File Offset: 0x0020258E
	internal static string CredentialAttribute_Prompt
	{
		get
		{
			return CredentialAttributeStrings.ResourceManager.GetString("CredentialAttribute_Prompt", CredentialAttributeStrings.resourceCulture);
		}
	}

	// Token: 0x1700132A RID: 4906
	// (get) Token: 0x06005E85 RID: 24197 RVA: 0x002043A4 File Offset: 0x002025A4
	internal static string CredentialAttribute_Prompt_Caption
	{
		get
		{
			return CredentialAttributeStrings.ResourceManager.GetString("CredentialAttribute_Prompt_Caption", CredentialAttributeStrings.resourceCulture);
		}
	}

	// Token: 0x040031FF RID: 12799
	private static ResourceManager resourceMan;

	// Token: 0x04003200 RID: 12800
	private static CultureInfo resourceCulture;
}
