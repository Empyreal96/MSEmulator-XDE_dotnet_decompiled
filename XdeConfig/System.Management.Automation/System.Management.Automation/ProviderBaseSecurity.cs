using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A20 RID: 2592
[DebuggerNonUserCode]
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
internal class ProviderBaseSecurity
{
	// Token: 0x06006368 RID: 25448 RVA: 0x0020AF0A File Offset: 0x0020910A
	internal ProviderBaseSecurity()
	{
	}

	// Token: 0x170017D7 RID: 6103
	// (get) Token: 0x06006369 RID: 25449 RVA: 0x0020AF14 File Offset: 0x00209114
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(ProviderBaseSecurity.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("ProviderBaseSecurity", typeof(ProviderBaseSecurity).Assembly);
				ProviderBaseSecurity.resourceMan = resourceManager;
			}
			return ProviderBaseSecurity.resourceMan;
		}
	}

	// Token: 0x170017D8 RID: 6104
	// (get) Token: 0x0600636A RID: 25450 RVA: 0x0020AF53 File Offset: 0x00209153
	// (set) Token: 0x0600636B RID: 25451 RVA: 0x0020AF5A File Offset: 0x0020915A
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return ProviderBaseSecurity.resourceCulture;
		}
		set
		{
			ProviderBaseSecurity.resourceCulture = value;
		}
	}

	// Token: 0x170017D9 RID: 6105
	// (get) Token: 0x0600636C RID: 25452 RVA: 0x0020AF62 File Offset: 0x00209162
	internal static string ISecurityDescriptorCmdletProvider_NotSupported
	{
		get
		{
			return ProviderBaseSecurity.ResourceManager.GetString("ISecurityDescriptorCmdletProvider_NotSupported", ProviderBaseSecurity.resourceCulture);
		}
	}

	// Token: 0x04003237 RID: 12855
	private static ResourceManager resourceMan;

	// Token: 0x04003238 RID: 12856
	private static CultureInfo resourceCulture;
}
