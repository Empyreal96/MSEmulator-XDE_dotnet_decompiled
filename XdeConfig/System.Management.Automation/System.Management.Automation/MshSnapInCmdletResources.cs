using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A30 RID: 2608
[DebuggerNonUserCode]
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
internal class MshSnapInCmdletResources
{
	// Token: 0x06006513 RID: 25875 RVA: 0x0020D3A8 File Offset: 0x0020B5A8
	internal MshSnapInCmdletResources()
	{
	}

	// Token: 0x17001962 RID: 6498
	// (get) Token: 0x06006514 RID: 25876 RVA: 0x0020D3B0 File Offset: 0x0020B5B0
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(MshSnapInCmdletResources.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("MshSnapInCmdletResources", typeof(MshSnapInCmdletResources).Assembly);
				MshSnapInCmdletResources.resourceMan = resourceManager;
			}
			return MshSnapInCmdletResources.resourceMan;
		}
	}

	// Token: 0x17001963 RID: 6499
	// (get) Token: 0x06006515 RID: 25877 RVA: 0x0020D3EF File Offset: 0x0020B5EF
	// (set) Token: 0x06006516 RID: 25878 RVA: 0x0020D3F6 File Offset: 0x0020B5F6
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return MshSnapInCmdletResources.resourceCulture;
		}
		set
		{
			MshSnapInCmdletResources.resourceCulture = value;
		}
	}

	// Token: 0x17001964 RID: 6500
	// (get) Token: 0x06006517 RID: 25879 RVA: 0x0020D3FE File Offset: 0x0020B5FE
	internal static string CmdletNotAvailable
	{
		get
		{
			return MshSnapInCmdletResources.ResourceManager.GetString("CmdletNotAvailable", MshSnapInCmdletResources.resourceCulture);
		}
	}

	// Token: 0x17001965 RID: 6501
	// (get) Token: 0x06006518 RID: 25880 RVA: 0x0020D414 File Offset: 0x0020B614
	internal static string CustomPSSnapInNotSupportedInOneCorePowerShell
	{
		get
		{
			return MshSnapInCmdletResources.ResourceManager.GetString("CustomPSSnapInNotSupportedInOneCorePowerShell", MshSnapInCmdletResources.resourceCulture);
		}
	}

	// Token: 0x17001966 RID: 6502
	// (get) Token: 0x06006519 RID: 25881 RVA: 0x0020D42A File Offset: 0x0020B62A
	internal static string InvalidPSSnapInName
	{
		get
		{
			return MshSnapInCmdletResources.ResourceManager.GetString("InvalidPSSnapInName", MshSnapInCmdletResources.resourceCulture);
		}
	}

	// Token: 0x17001967 RID: 6503
	// (get) Token: 0x0600651A RID: 25882 RVA: 0x0020D440 File Offset: 0x0020B640
	internal static string LoadSystemSnapinAsModule
	{
		get
		{
			return MshSnapInCmdletResources.ResourceManager.GetString("LoadSystemSnapinAsModule", MshSnapInCmdletResources.resourceCulture);
		}
	}

	// Token: 0x17001968 RID: 6504
	// (get) Token: 0x0600651B RID: 25883 RVA: 0x0020D456 File Offset: 0x0020B656
	internal static string NoPSSnapInsFound
	{
		get
		{
			return MshSnapInCmdletResources.ResourceManager.GetString("NoPSSnapInsFound", MshSnapInCmdletResources.resourceCulture);
		}
	}

	// Token: 0x04003257 RID: 12887
	private static ResourceManager resourceMan;

	// Token: 0x04003258 RID: 12888
	private static CultureInfo resourceCulture;
}
