using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A40 RID: 2624
[CompilerGenerated]
[DebuggerNonUserCode]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
internal class ErrorPackageRemoting
{
	// Token: 0x06006936 RID: 26934 RVA: 0x00212E94 File Offset: 0x00211094
	internal ErrorPackageRemoting()
	{
	}

	// Token: 0x17001D65 RID: 7525
	// (get) Token: 0x06006937 RID: 26935 RVA: 0x00212E9C File Offset: 0x0021109C
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(ErrorPackageRemoting.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("ErrorPackageRemoting", typeof(ErrorPackageRemoting).Assembly);
				ErrorPackageRemoting.resourceMan = resourceManager;
			}
			return ErrorPackageRemoting.resourceMan;
		}
	}

	// Token: 0x17001D66 RID: 7526
	// (get) Token: 0x06006938 RID: 26936 RVA: 0x00212EDB File Offset: 0x002110DB
	// (set) Token: 0x06006939 RID: 26937 RVA: 0x00212EE2 File Offset: 0x002110E2
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return ErrorPackageRemoting.resourceCulture;
		}
		set
		{
			ErrorPackageRemoting.resourceCulture = value;
		}
	}

	// Token: 0x04003277 RID: 12919
	private static ResourceManager resourceMan;

	// Token: 0x04003278 RID: 12920
	private static CultureInfo resourceCulture;
}
