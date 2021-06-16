using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A18 RID: 2584
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[CompilerGenerated]
[DebuggerNonUserCode]
internal class MshSignature
{
	// Token: 0x06006111 RID: 24849 RVA: 0x00207B98 File Offset: 0x00205D98
	internal MshSignature()
	{
	}

	// Token: 0x17001590 RID: 5520
	// (get) Token: 0x06006112 RID: 24850 RVA: 0x00207BA0 File Offset: 0x00205DA0
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(MshSignature.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("MshSignature", typeof(MshSignature).Assembly);
				MshSignature.resourceMan = resourceManager;
			}
			return MshSignature.resourceMan;
		}
	}

	// Token: 0x17001591 RID: 5521
	// (get) Token: 0x06006113 RID: 24851 RVA: 0x00207BDF File Offset: 0x00205DDF
	// (set) Token: 0x06006114 RID: 24852 RVA: 0x00207BE6 File Offset: 0x00205DE6
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return MshSignature.resourceCulture;
		}
		set
		{
			MshSignature.resourceCulture = value;
		}
	}

	// Token: 0x17001592 RID: 5522
	// (get) Token: 0x06006115 RID: 24853 RVA: 0x00207BEE File Offset: 0x00205DEE
	internal static string MshSignature_HashMismatch
	{
		get
		{
			return MshSignature.ResourceManager.GetString("MshSignature_HashMismatch", MshSignature.resourceCulture);
		}
	}

	// Token: 0x17001593 RID: 5523
	// (get) Token: 0x06006116 RID: 24854 RVA: 0x00207C04 File Offset: 0x00205E04
	internal static string MshSignature_Incompatible
	{
		get
		{
			return MshSignature.ResourceManager.GetString("MshSignature_Incompatible", MshSignature.resourceCulture);
		}
	}

	// Token: 0x17001594 RID: 5524
	// (get) Token: 0x06006117 RID: 24855 RVA: 0x00207C1A File Offset: 0x00205E1A
	internal static string MshSignature_Incompatible_HashAlgorithm
	{
		get
		{
			return MshSignature.ResourceManager.GetString("MshSignature_Incompatible_HashAlgorithm", MshSignature.resourceCulture);
		}
	}

	// Token: 0x17001595 RID: 5525
	// (get) Token: 0x06006118 RID: 24856 RVA: 0x00207C30 File Offset: 0x00205E30
	internal static string MshSignature_NotSigned
	{
		get
		{
			return MshSignature.ResourceManager.GetString("MshSignature_NotSigned", MshSignature.resourceCulture);
		}
	}

	// Token: 0x17001596 RID: 5526
	// (get) Token: 0x06006119 RID: 24857 RVA: 0x00207C46 File Offset: 0x00205E46
	internal static string MshSignature_NotSupportedFileFormat
	{
		get
		{
			return MshSignature.ResourceManager.GetString("MshSignature_NotSupportedFileFormat", MshSignature.resourceCulture);
		}
	}

	// Token: 0x17001597 RID: 5527
	// (get) Token: 0x0600611A RID: 24858 RVA: 0x00207C5C File Offset: 0x00205E5C
	internal static string MshSignature_NotSupportedFileFormat_NoExtension
	{
		get
		{
			return MshSignature.ResourceManager.GetString("MshSignature_NotSupportedFileFormat_NoExtension", MshSignature.resourceCulture);
		}
	}

	// Token: 0x17001598 RID: 5528
	// (get) Token: 0x0600611B RID: 24859 RVA: 0x00207C72 File Offset: 0x00205E72
	internal static string MshSignature_NotTrusted
	{
		get
		{
			return MshSignature.ResourceManager.GetString("MshSignature_NotTrusted", MshSignature.resourceCulture);
		}
	}

	// Token: 0x17001599 RID: 5529
	// (get) Token: 0x0600611C RID: 24860 RVA: 0x00207C88 File Offset: 0x00205E88
	internal static string MshSignature_Valid
	{
		get
		{
			return MshSignature.ResourceManager.GetString("MshSignature_Valid", MshSignature.resourceCulture);
		}
	}

	// Token: 0x04003227 RID: 12839
	private static ResourceManager resourceMan;

	// Token: 0x04003228 RID: 12840
	private static CultureInfo resourceCulture;
}
