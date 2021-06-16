using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A2E RID: 2606
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class MiniShellErrors
{
	// Token: 0x06006502 RID: 25858 RVA: 0x0020D234 File Offset: 0x0020B434
	internal MiniShellErrors()
	{
	}

	// Token: 0x17001955 RID: 6485
	// (get) Token: 0x06006503 RID: 25859 RVA: 0x0020D23C File Offset: 0x0020B43C
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(MiniShellErrors.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("MiniShellErrors", typeof(MiniShellErrors).Assembly);
				MiniShellErrors.resourceMan = resourceManager;
			}
			return MiniShellErrors.resourceMan;
		}
	}

	// Token: 0x17001956 RID: 6486
	// (get) Token: 0x06006504 RID: 25860 RVA: 0x0020D27B File Offset: 0x0020B47B
	// (set) Token: 0x06006505 RID: 25861 RVA: 0x0020D282 File Offset: 0x0020B482
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return MiniShellErrors.resourceCulture;
		}
		set
		{
			MiniShellErrors.resourceCulture = value;
		}
	}

	// Token: 0x17001957 RID: 6487
	// (get) Token: 0x06006506 RID: 25862 RVA: 0x0020D28A File Offset: 0x0020B48A
	internal static string InvalidEntryAssembly
	{
		get
		{
			return MiniShellErrors.ResourceManager.GetString("InvalidEntryAssembly", MiniShellErrors.resourceCulture);
		}
	}

	// Token: 0x17001958 RID: 6488
	// (get) Token: 0x06006507 RID: 25863 RVA: 0x0020D2A0 File Offset: 0x0020B4A0
	internal static string RunspaceConfigurationAttributeDuplicate
	{
		get
		{
			return MiniShellErrors.ResourceManager.GetString("RunspaceConfigurationAttributeDuplicate", MiniShellErrors.resourceCulture);
		}
	}

	// Token: 0x17001959 RID: 6489
	// (get) Token: 0x06006508 RID: 25864 RVA: 0x0020D2B6 File Offset: 0x0020B4B6
	internal static string RunspaceConfigurationAttributeNotExist
	{
		get
		{
			return MiniShellErrors.ResourceManager.GetString("RunspaceConfigurationAttributeNotExist", MiniShellErrors.resourceCulture);
		}
	}

	// Token: 0x1700195A RID: 6490
	// (get) Token: 0x06006509 RID: 25865 RVA: 0x0020D2CC File Offset: 0x0020B4CC
	internal static string UndefinedRunspaceConfigurationType
	{
		get
		{
			return MiniShellErrors.ResourceManager.GetString("UndefinedRunspaceConfigurationType", MiniShellErrors.resourceCulture);
		}
	}

	// Token: 0x1700195B RID: 6491
	// (get) Token: 0x0600650A RID: 25866 RVA: 0x0020D2E2 File Offset: 0x0020B4E2
	internal static string UpdateAssemblyErrors
	{
		get
		{
			return MiniShellErrors.ResourceManager.GetString("UpdateAssemblyErrors", MiniShellErrors.resourceCulture);
		}
	}

	// Token: 0x1700195C RID: 6492
	// (get) Token: 0x0600650B RID: 25867 RVA: 0x0020D2F8 File Offset: 0x0020B4F8
	internal static string UpdateNotSupportedForConfigurationCategory
	{
		get
		{
			return MiniShellErrors.ResourceManager.GetString("UpdateNotSupportedForConfigurationCategory", MiniShellErrors.resourceCulture);
		}
	}

	// Token: 0x04003253 RID: 12883
	private static ResourceManager resourceMan;

	// Token: 0x04003254 RID: 12884
	private static CultureInfo resourceCulture;
}
