using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A00 RID: 2560
[DebuggerNonUserCode]
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
internal class CimInstanceTypeAdapterResources
{
	// Token: 0x06005E3F RID: 24127 RVA: 0x00203DA2 File Offset: 0x00201FA2
	internal CimInstanceTypeAdapterResources()
	{
	}

	// Token: 0x170012EE RID: 4846
	// (get) Token: 0x06005E40 RID: 24128 RVA: 0x00203DAC File Offset: 0x00201FAC
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(CimInstanceTypeAdapterResources.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("CimInstanceTypeAdapterResources", typeof(CimInstanceTypeAdapterResources).Assembly);
				CimInstanceTypeAdapterResources.resourceMan = resourceManager;
			}
			return CimInstanceTypeAdapterResources.resourceMan;
		}
	}

	// Token: 0x170012EF RID: 4847
	// (get) Token: 0x06005E41 RID: 24129 RVA: 0x00203DEB File Offset: 0x00201FEB
	// (set) Token: 0x06005E42 RID: 24130 RVA: 0x00203DF2 File Offset: 0x00201FF2
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return CimInstanceTypeAdapterResources.resourceCulture;
		}
		set
		{
			CimInstanceTypeAdapterResources.resourceCulture = value;
		}
	}

	// Token: 0x170012F0 RID: 4848
	// (get) Token: 0x06005E43 RID: 24131 RVA: 0x00203DFA File Offset: 0x00201FFA
	internal static string BaseObjectNotCimInstance
	{
		get
		{
			return CimInstanceTypeAdapterResources.ResourceManager.GetString("BaseObjectNotCimInstance", CimInstanceTypeAdapterResources.resourceCulture);
		}
	}

	// Token: 0x170012F1 RID: 4849
	// (get) Token: 0x06005E44 RID: 24132 RVA: 0x00203E10 File Offset: 0x00202010
	internal static string ReadOnlyCIMProperty
	{
		get
		{
			return CimInstanceTypeAdapterResources.ResourceManager.GetString("ReadOnlyCIMProperty", CimInstanceTypeAdapterResources.resourceCulture);
		}
	}

	// Token: 0x040031F7 RID: 12791
	private static ResourceManager resourceMan;

	// Token: 0x040031F8 RID: 12792
	private static CultureInfo resourceCulture;
}
