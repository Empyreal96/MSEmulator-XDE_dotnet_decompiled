using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A28 RID: 2600
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class AuthorizationManagerBase
{
	// Token: 0x060063EC RID: 25580 RVA: 0x0020BA58 File Offset: 0x00209C58
	internal AuthorizationManagerBase()
	{
	}

	// Token: 0x1700184B RID: 6219
	// (get) Token: 0x060063ED RID: 25581 RVA: 0x0020BA60 File Offset: 0x00209C60
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(AuthorizationManagerBase.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("AuthorizationManagerBase", typeof(AuthorizationManagerBase).Assembly);
				AuthorizationManagerBase.resourceMan = resourceManager;
			}
			return AuthorizationManagerBase.resourceMan;
		}
	}

	// Token: 0x1700184C RID: 6220
	// (get) Token: 0x060063EE RID: 25582 RVA: 0x0020BA9F File Offset: 0x00209C9F
	// (set) Token: 0x060063EF RID: 25583 RVA: 0x0020BAA6 File Offset: 0x00209CA6
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return AuthorizationManagerBase.resourceCulture;
		}
		set
		{
			AuthorizationManagerBase.resourceCulture = value;
		}
	}

	// Token: 0x1700184D RID: 6221
	// (get) Token: 0x060063F0 RID: 25584 RVA: 0x0020BAAE File Offset: 0x00209CAE
	internal static string AuthorizationManagerDefaultFailureReason
	{
		get
		{
			return AuthorizationManagerBase.ResourceManager.GetString("AuthorizationManagerDefaultFailureReason", AuthorizationManagerBase.resourceCulture);
		}
	}

	// Token: 0x04003247 RID: 12871
	private static ResourceManager resourceMan;

	// Token: 0x04003248 RID: 12872
	private static CultureInfo resourceCulture;
}
