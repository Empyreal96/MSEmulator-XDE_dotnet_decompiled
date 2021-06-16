using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A16 RID: 2582
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Logging
{
	// Token: 0x060060DF RID: 24799 RVA: 0x0020774E File Offset: 0x0020594E
	internal Logging()
	{
	}

	// Token: 0x17001562 RID: 5474
	// (get) Token: 0x060060E0 RID: 24800 RVA: 0x00207758 File Offset: 0x00205958
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(Logging.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("Logging", typeof(Logging).Assembly);
				Logging.resourceMan = resourceManager;
			}
			return Logging.resourceMan;
		}
	}

	// Token: 0x17001563 RID: 5475
	// (get) Token: 0x060060E1 RID: 24801 RVA: 0x00207797 File Offset: 0x00205997
	// (set) Token: 0x060060E2 RID: 24802 RVA: 0x0020779E File Offset: 0x0020599E
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return Logging.resourceCulture;
		}
		set
		{
			Logging.resourceCulture = value;
		}
	}

	// Token: 0x17001564 RID: 5476
	// (get) Token: 0x060060E3 RID: 24803 RVA: 0x002077A6 File Offset: 0x002059A6
	internal static string CommandHealthContext
	{
		get
		{
			return Logging.ResourceManager.GetString("CommandHealthContext", Logging.resourceCulture);
		}
	}

	// Token: 0x17001565 RID: 5477
	// (get) Token: 0x060060E4 RID: 24804 RVA: 0x002077BC File Offset: 0x002059BC
	internal static string CommandLifecycleContext
	{
		get
		{
			return Logging.ResourceManager.GetString("CommandLifecycleContext", Logging.resourceCulture);
		}
	}

	// Token: 0x17001566 RID: 5478
	// (get) Token: 0x060060E5 RID: 24805 RVA: 0x002077D2 File Offset: 0x002059D2
	internal static string EngineHealthContext
	{
		get
		{
			return Logging.ResourceManager.GetString("EngineHealthContext", Logging.resourceCulture);
		}
	}

	// Token: 0x17001567 RID: 5479
	// (get) Token: 0x060060E6 RID: 24806 RVA: 0x002077E8 File Offset: 0x002059E8
	internal static string EngineLifecycleContext
	{
		get
		{
			return Logging.ResourceManager.GetString("EngineLifecycleContext", Logging.resourceCulture);
		}
	}

	// Token: 0x17001568 RID: 5480
	// (get) Token: 0x060060E7 RID: 24807 RVA: 0x002077FE File Offset: 0x002059FE
	internal static string PipelineExecutionDetailContext
	{
		get
		{
			return Logging.ResourceManager.GetString("PipelineExecutionDetailContext", Logging.resourceCulture);
		}
	}

	// Token: 0x17001569 RID: 5481
	// (get) Token: 0x060060E8 RID: 24808 RVA: 0x00207814 File Offset: 0x00205A14
	internal static string ProviderHealthContext
	{
		get
		{
			return Logging.ResourceManager.GetString("ProviderHealthContext", Logging.resourceCulture);
		}
	}

	// Token: 0x1700156A RID: 5482
	// (get) Token: 0x060060E9 RID: 24809 RVA: 0x0020782A File Offset: 0x00205A2A
	internal static string ProviderLifecycleContext
	{
		get
		{
			return Logging.ResourceManager.GetString("ProviderLifecycleContext", Logging.resourceCulture);
		}
	}

	// Token: 0x1700156B RID: 5483
	// (get) Token: 0x060060EA RID: 24810 RVA: 0x00207840 File Offset: 0x00205A40
	internal static string SettingsContext
	{
		get
		{
			return Logging.ResourceManager.GetString("SettingsContext", Logging.resourceCulture);
		}
	}

	// Token: 0x1700156C RID: 5484
	// (get) Token: 0x060060EB RID: 24811 RVA: 0x00207856 File Offset: 0x00205A56
	internal static string UnknownUserName
	{
		get
		{
			return Logging.ResourceManager.GetString("UnknownUserName", Logging.resourceCulture);
		}
	}

	// Token: 0x04003223 RID: 12835
	private static ResourceManager resourceMan;

	// Token: 0x04003224 RID: 12836
	private static CultureInfo resourceCulture;
}
