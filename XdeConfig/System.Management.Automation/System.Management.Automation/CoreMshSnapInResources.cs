using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A3F RID: 2623
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
internal class CoreMshSnapInResources
{
	// Token: 0x0600692F RID: 26927 RVA: 0x00212DFC File Offset: 0x00210FFC
	internal CoreMshSnapInResources()
	{
	}

	// Token: 0x17001D60 RID: 7520
	// (get) Token: 0x06006930 RID: 26928 RVA: 0x00212E04 File Offset: 0x00211004
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(CoreMshSnapInResources.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("CoreMshSnapInResources", typeof(CoreMshSnapInResources).Assembly);
				CoreMshSnapInResources.resourceMan = resourceManager;
			}
			return CoreMshSnapInResources.resourceMan;
		}
	}

	// Token: 0x17001D61 RID: 7521
	// (get) Token: 0x06006931 RID: 26929 RVA: 0x00212E43 File Offset: 0x00211043
	// (set) Token: 0x06006932 RID: 26930 RVA: 0x00212E4A File Offset: 0x0021104A
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return CoreMshSnapInResources.resourceCulture;
		}
		set
		{
			CoreMshSnapInResources.resourceCulture = value;
		}
	}

	// Token: 0x17001D62 RID: 7522
	// (get) Token: 0x06006933 RID: 26931 RVA: 0x00212E52 File Offset: 0x00211052
	internal static string Description
	{
		get
		{
			return CoreMshSnapInResources.ResourceManager.GetString("Description", CoreMshSnapInResources.resourceCulture);
		}
	}

	// Token: 0x17001D63 RID: 7523
	// (get) Token: 0x06006934 RID: 26932 RVA: 0x00212E68 File Offset: 0x00211068
	internal static string Name
	{
		get
		{
			return CoreMshSnapInResources.ResourceManager.GetString("Name", CoreMshSnapInResources.resourceCulture);
		}
	}

	// Token: 0x17001D64 RID: 7524
	// (get) Token: 0x06006935 RID: 26933 RVA: 0x00212E7E File Offset: 0x0021107E
	internal static string Vendor
	{
		get
		{
			return CoreMshSnapInResources.ResourceManager.GetString("Vendor", CoreMshSnapInResources.resourceCulture);
		}
	}

	// Token: 0x04003275 RID: 12917
	private static ResourceManager resourceMan;

	// Token: 0x04003276 RID: 12918
	private static CultureInfo resourceCulture;
}
