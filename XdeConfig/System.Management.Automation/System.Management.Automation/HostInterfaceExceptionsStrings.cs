using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A11 RID: 2577
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
internal class HostInterfaceExceptionsStrings
{
	// Token: 0x06006026 RID: 24614 RVA: 0x0020676C File Offset: 0x0020496C
	internal HostInterfaceExceptionsStrings()
	{
	}

	// Token: 0x170014B3 RID: 5299
	// (get) Token: 0x06006027 RID: 24615 RVA: 0x00206774 File Offset: 0x00204974
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(HostInterfaceExceptionsStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("HostInterfaceExceptionsStrings", typeof(HostInterfaceExceptionsStrings).Assembly);
				HostInterfaceExceptionsStrings.resourceMan = resourceManager;
			}
			return HostInterfaceExceptionsStrings.resourceMan;
		}
	}

	// Token: 0x170014B4 RID: 5300
	// (get) Token: 0x06006028 RID: 24616 RVA: 0x002067B3 File Offset: 0x002049B3
	// (set) Token: 0x06006029 RID: 24617 RVA: 0x002067BA File Offset: 0x002049BA
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return HostInterfaceExceptionsStrings.resourceCulture;
		}
		set
		{
			HostInterfaceExceptionsStrings.resourceCulture = value;
		}
	}

	// Token: 0x170014B5 RID: 5301
	// (get) Token: 0x0600602A RID: 24618 RVA: 0x002067C2 File Offset: 0x002049C2
	internal static string DefaultCtorMessageTemplate
	{
		get
		{
			return HostInterfaceExceptionsStrings.ResourceManager.GetString("DefaultCtorMessageTemplate", HostInterfaceExceptionsStrings.resourceCulture);
		}
	}

	// Token: 0x170014B6 RID: 5302
	// (get) Token: 0x0600602B RID: 24619 RVA: 0x002067D8 File Offset: 0x002049D8
	internal static string HostFunctionNotImplemented
	{
		get
		{
			return HostInterfaceExceptionsStrings.ResourceManager.GetString("HostFunctionNotImplemented", HostInterfaceExceptionsStrings.resourceCulture);
		}
	}

	// Token: 0x170014B7 RID: 5303
	// (get) Token: 0x0600602C RID: 24620 RVA: 0x002067EE File Offset: 0x002049EE
	internal static string HostFunctionPromptNotImplemented
	{
		get
		{
			return HostInterfaceExceptionsStrings.ResourceManager.GetString("HostFunctionPromptNotImplemented", HostInterfaceExceptionsStrings.resourceCulture);
		}
	}

	// Token: 0x170014B8 RID: 5304
	// (get) Token: 0x0600602D RID: 24621 RVA: 0x00206804 File Offset: 0x00204A04
	internal static string RunspacePoolNotOpened
	{
		get
		{
			return HostInterfaceExceptionsStrings.ResourceManager.GetString("RunspacePoolNotOpened", HostInterfaceExceptionsStrings.resourceCulture);
		}
	}

	// Token: 0x04003219 RID: 12825
	private static ResourceManager resourceMan;

	// Token: 0x0400321A RID: 12826
	private static CultureInfo resourceCulture;
}
