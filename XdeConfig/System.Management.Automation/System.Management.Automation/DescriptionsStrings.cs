using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A07 RID: 2567
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
internal class DescriptionsStrings
{
	// Token: 0x06005EBB RID: 24251 RVA: 0x00204846 File Offset: 0x00202A46
	internal DescriptionsStrings()
	{
	}

	// Token: 0x1700135C RID: 4956
	// (get) Token: 0x06005EBC RID: 24252 RVA: 0x00204850 File Offset: 0x00202A50
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(DescriptionsStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("DescriptionsStrings", typeof(DescriptionsStrings).Assembly);
				DescriptionsStrings.resourceMan = resourceManager;
			}
			return DescriptionsStrings.resourceMan;
		}
	}

	// Token: 0x1700135D RID: 4957
	// (get) Token: 0x06005EBD RID: 24253 RVA: 0x0020488F File Offset: 0x00202A8F
	// (set) Token: 0x06005EBE RID: 24254 RVA: 0x00204896 File Offset: 0x00202A96
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return DescriptionsStrings.resourceCulture;
		}
		set
		{
			DescriptionsStrings.resourceCulture = value;
		}
	}

	// Token: 0x1700135E RID: 4958
	// (get) Token: 0x06005EBF RID: 24255 RVA: 0x0020489E File Offset: 0x00202A9E
	internal static string NullOrEmptyErrorTemplate
	{
		get
		{
			return DescriptionsStrings.ResourceManager.GetString("NullOrEmptyErrorTemplate", DescriptionsStrings.resourceCulture);
		}
	}

	// Token: 0x04003205 RID: 12805
	private static ResourceManager resourceMan;

	// Token: 0x04003206 RID: 12806
	private static CultureInfo resourceCulture;
}
