using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A03 RID: 2563
[DebuggerNonUserCode]
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
internal class Credential
{
	// Token: 0x06005E7A RID: 24186 RVA: 0x002042B4 File Offset: 0x002024B4
	internal Credential()
	{
	}

	// Token: 0x17001323 RID: 4899
	// (get) Token: 0x06005E7B RID: 24187 RVA: 0x002042BC File Offset: 0x002024BC
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(Credential.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("Credential", typeof(Credential).Assembly);
				Credential.resourceMan = resourceManager;
			}
			return Credential.resourceMan;
		}
	}

	// Token: 0x17001324 RID: 4900
	// (get) Token: 0x06005E7C RID: 24188 RVA: 0x002042FB File Offset: 0x002024FB
	// (set) Token: 0x06005E7D RID: 24189 RVA: 0x00204302 File Offset: 0x00202502
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return Credential.resourceCulture;
		}
		set
		{
			Credential.resourceCulture = value;
		}
	}

	// Token: 0x17001325 RID: 4901
	// (get) Token: 0x06005E7E RID: 24190 RVA: 0x0020430A File Offset: 0x0020250A
	internal static string CredentialDisallowed
	{
		get
		{
			return Credential.ResourceManager.GetString("CredentialDisallowed", Credential.resourceCulture);
		}
	}

	// Token: 0x17001326 RID: 4902
	// (get) Token: 0x06005E7F RID: 24191 RVA: 0x00204320 File Offset: 0x00202520
	internal static string InvalidUserNameFormat
	{
		get
		{
			return Credential.ResourceManager.GetString("InvalidUserNameFormat", Credential.resourceCulture);
		}
	}

	// Token: 0x040031FD RID: 12797
	private static ResourceManager resourceMan;

	// Token: 0x040031FE RID: 12798
	private static CultureInfo resourceCulture;
}
