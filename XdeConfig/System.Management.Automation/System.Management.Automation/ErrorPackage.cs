using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A0B RID: 2571
[DebuggerNonUserCode]
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
internal class ErrorPackage
{
	// Token: 0x06005F16 RID: 24342 RVA: 0x00205014 File Offset: 0x00203214
	internal ErrorPackage()
	{
	}

	// Token: 0x170013AF RID: 5039
	// (get) Token: 0x06005F17 RID: 24343 RVA: 0x0020501C File Offset: 0x0020321C
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(ErrorPackage.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("ErrorPackage", typeof(ErrorPackage).Assembly);
				ErrorPackage.resourceMan = resourceManager;
			}
			return ErrorPackage.resourceMan;
		}
	}

	// Token: 0x170013B0 RID: 5040
	// (get) Token: 0x06005F18 RID: 24344 RVA: 0x0020505B File Offset: 0x0020325B
	// (set) Token: 0x06005F19 RID: 24345 RVA: 0x00205062 File Offset: 0x00203262
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return ErrorPackage.resourceCulture;
		}
		set
		{
			ErrorPackage.resourceCulture = value;
		}
	}

	// Token: 0x170013B1 RID: 5041
	// (get) Token: 0x06005F1A RID: 24346 RVA: 0x0020506A File Offset: 0x0020326A
	internal static string Ellipsize
	{
		get
		{
			return ErrorPackage.ResourceManager.GetString("Ellipsize", ErrorPackage.resourceCulture);
		}
	}

	// Token: 0x170013B2 RID: 5042
	// (get) Token: 0x06005F1B RID: 24347 RVA: 0x00205080 File Offset: 0x00203280
	internal static string ErrorDetailsEmptyTemplate
	{
		get
		{
			return ErrorPackage.ResourceManager.GetString("ErrorDetailsEmptyTemplate", ErrorPackage.resourceCulture);
		}
	}

	// Token: 0x170013B3 RID: 5043
	// (get) Token: 0x06005F1C RID: 24348 RVA: 0x00205096 File Offset: 0x00203296
	internal static string RedirectedException
	{
		get
		{
			return ErrorPackage.ResourceManager.GetString("RedirectedException", ErrorPackage.resourceCulture);
		}
	}

	// Token: 0x170013B4 RID: 5044
	// (get) Token: 0x06005F1D RID: 24349 RVA: 0x002050AC File Offset: 0x002032AC
	internal static string SuspendActionPreferenceErrorActionOnly
	{
		get
		{
			return ErrorPackage.ResourceManager.GetString("SuspendActionPreferenceErrorActionOnly", ErrorPackage.resourceCulture);
		}
	}

	// Token: 0x170013B5 RID: 5045
	// (get) Token: 0x06005F1E RID: 24350 RVA: 0x002050C2 File Offset: 0x002032C2
	internal static string SuspendActionPreferenceSupportedOnlyOnWorkflow
	{
		get
		{
			return ErrorPackage.ResourceManager.GetString("SuspendActionPreferenceSupportedOnlyOnWorkflow", ErrorPackage.resourceCulture);
		}
	}

	// Token: 0x170013B6 RID: 5046
	// (get) Token: 0x06005F1F RID: 24351 RVA: 0x002050D8 File Offset: 0x002032D8
	internal static string UnsupportedPreferenceError
	{
		get
		{
			return ErrorPackage.ResourceManager.GetString("UnsupportedPreferenceError", ErrorPackage.resourceCulture);
		}
	}

	// Token: 0x170013B7 RID: 5047
	// (get) Token: 0x06005F20 RID: 24352 RVA: 0x002050EE File Offset: 0x002032EE
	internal static string UnsupportedPreferenceVariable
	{
		get
		{
			return ErrorPackage.ResourceManager.GetString("UnsupportedPreferenceVariable", ErrorPackage.resourceCulture);
		}
	}

	// Token: 0x0400320D RID: 12813
	private static ResourceManager resourceMan;

	// Token: 0x0400320E RID: 12814
	private static CultureInfo resourceCulture;
}
