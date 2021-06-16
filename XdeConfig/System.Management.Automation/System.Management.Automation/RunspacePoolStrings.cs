using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A27 RID: 2599
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
internal class RunspacePoolStrings
{
	// Token: 0x060063D7 RID: 25559 RVA: 0x0020B88C File Offset: 0x00209A8C
	internal RunspacePoolStrings()
	{
	}

	// Token: 0x17001838 RID: 6200
	// (get) Token: 0x060063D8 RID: 25560 RVA: 0x0020B894 File Offset: 0x00209A94
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(RunspacePoolStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("RunspacePoolStrings", typeof(RunspacePoolStrings).Assembly);
				RunspacePoolStrings.resourceMan = resourceManager;
			}
			return RunspacePoolStrings.resourceMan;
		}
	}

	// Token: 0x17001839 RID: 6201
	// (get) Token: 0x060063D9 RID: 25561 RVA: 0x0020B8D3 File Offset: 0x00209AD3
	// (set) Token: 0x060063DA RID: 25562 RVA: 0x0020B8DA File Offset: 0x00209ADA
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return RunspacePoolStrings.resourceCulture;
		}
		set
		{
			RunspacePoolStrings.resourceCulture = value;
		}
	}

	// Token: 0x1700183A RID: 6202
	// (get) Token: 0x060063DB RID: 25563 RVA: 0x0020B8E2 File Offset: 0x00209AE2
	internal static string AsyncResultNotOwned
	{
		get
		{
			return RunspacePoolStrings.ResourceManager.GetString("AsyncResultNotOwned", RunspacePoolStrings.resourceCulture);
		}
	}

	// Token: 0x1700183B RID: 6203
	// (get) Token: 0x060063DC RID: 25564 RVA: 0x0020B8F8 File Offset: 0x00209AF8
	internal static string CannotConnect
	{
		get
		{
			return RunspacePoolStrings.ResourceManager.GetString("CannotConnect", RunspacePoolStrings.resourceCulture);
		}
	}

	// Token: 0x1700183C RID: 6204
	// (get) Token: 0x060063DD RID: 25565 RVA: 0x0020B90E File Offset: 0x00209B0E
	internal static string CannotOpenAgain
	{
		get
		{
			return RunspacePoolStrings.ResourceManager.GetString("CannotOpenAgain", RunspacePoolStrings.resourceCulture);
		}
	}

	// Token: 0x1700183D RID: 6205
	// (get) Token: 0x060063DE RID: 25566 RVA: 0x0020B924 File Offset: 0x00209B24
	internal static string CannotReconstructCommands
	{
		get
		{
			return RunspacePoolStrings.ResourceManager.GetString("CannotReconstructCommands", RunspacePoolStrings.resourceCulture);
		}
	}

	// Token: 0x1700183E RID: 6206
	// (get) Token: 0x060063DF RID: 25567 RVA: 0x0020B93A File Offset: 0x00209B3A
	internal static string CannotSetTypeTable
	{
		get
		{
			return RunspacePoolStrings.ResourceManager.GetString("CannotSetTypeTable", RunspacePoolStrings.resourceCulture);
		}
	}

	// Token: 0x1700183F RID: 6207
	// (get) Token: 0x060063E0 RID: 25568 RVA: 0x0020B950 File Offset: 0x00209B50
	internal static string CannotWhileDisconnected
	{
		get
		{
			return RunspacePoolStrings.ResourceManager.GetString("CannotWhileDisconnected", RunspacePoolStrings.resourceCulture);
		}
	}

	// Token: 0x17001840 RID: 6208
	// (get) Token: 0x060063E1 RID: 25569 RVA: 0x0020B966 File Offset: 0x00209B66
	internal static string ChangePropertyAfterOpen
	{
		get
		{
			return RunspacePoolStrings.ResourceManager.GetString("ChangePropertyAfterOpen", RunspacePoolStrings.resourceCulture);
		}
	}

	// Token: 0x17001841 RID: 6209
	// (get) Token: 0x060063E2 RID: 25570 RVA: 0x0020B97C File Offset: 0x00209B7C
	internal static string DisconnectNotSupportedOnServer
	{
		get
		{
			return RunspacePoolStrings.ResourceManager.GetString("DisconnectNotSupportedOnServer", RunspacePoolStrings.resourceCulture);
		}
	}

	// Token: 0x17001842 RID: 6210
	// (get) Token: 0x060063E3 RID: 25571 RVA: 0x0020B992 File Offset: 0x00209B92
	internal static string InvalidRunspacePoolState
	{
		get
		{
			return RunspacePoolStrings.ResourceManager.GetString("InvalidRunspacePoolState", RunspacePoolStrings.resourceCulture);
		}
	}

	// Token: 0x17001843 RID: 6211
	// (get) Token: 0x060063E4 RID: 25572 RVA: 0x0020B9A8 File Offset: 0x00209BA8
	internal static string InvalidRunspacePoolStateGeneral
	{
		get
		{
			return RunspacePoolStrings.ResourceManager.GetString("InvalidRunspacePoolStateGeneral", RunspacePoolStrings.resourceCulture);
		}
	}

	// Token: 0x17001844 RID: 6212
	// (get) Token: 0x060063E5 RID: 25573 RVA: 0x0020B9BE File Offset: 0x00209BBE
	internal static string MaxPoolLessThan1
	{
		get
		{
			return RunspacePoolStrings.ResourceManager.GetString("MaxPoolLessThan1", RunspacePoolStrings.resourceCulture);
		}
	}

	// Token: 0x17001845 RID: 6213
	// (get) Token: 0x060063E6 RID: 25574 RVA: 0x0020B9D4 File Offset: 0x00209BD4
	internal static string MinPoolGreaterThanMaxPool
	{
		get
		{
			return RunspacePoolStrings.ResourceManager.GetString("MinPoolGreaterThanMaxPool", RunspacePoolStrings.resourceCulture);
		}
	}

	// Token: 0x17001846 RID: 6214
	// (get) Token: 0x060063E7 RID: 25575 RVA: 0x0020B9EA File Offset: 0x00209BEA
	internal static string MinPoolLessThan1
	{
		get
		{
			return RunspacePoolStrings.ResourceManager.GetString("MinPoolLessThan1", RunspacePoolStrings.resourceCulture);
		}
	}

	// Token: 0x17001847 RID: 6215
	// (get) Token: 0x060063E8 RID: 25576 RVA: 0x0020BA00 File Offset: 0x00209C00
	internal static string ResetRunspaceStateNotSupportedOnServer
	{
		get
		{
			return RunspacePoolStrings.ResourceManager.GetString("ResetRunspaceStateNotSupportedOnServer", RunspacePoolStrings.resourceCulture);
		}
	}

	// Token: 0x17001848 RID: 6216
	// (get) Token: 0x060063E9 RID: 25577 RVA: 0x0020BA16 File Offset: 0x00209C16
	internal static string RunspaceDisconnectConnectNotSupported
	{
		get
		{
			return RunspacePoolStrings.ResourceManager.GetString("RunspaceDisconnectConnectNotSupported", RunspacePoolStrings.resourceCulture);
		}
	}

	// Token: 0x17001849 RID: 6217
	// (get) Token: 0x060063EA RID: 25578 RVA: 0x0020BA2C File Offset: 0x00209C2C
	internal static string RunspaceNotBelongsToPool
	{
		get
		{
			return RunspacePoolStrings.ResourceManager.GetString("RunspaceNotBelongsToPool", RunspacePoolStrings.resourceCulture);
		}
	}

	// Token: 0x1700184A RID: 6218
	// (get) Token: 0x060063EB RID: 25579 RVA: 0x0020BA42 File Offset: 0x00209C42
	internal static string RunspacePoolClosed
	{
		get
		{
			return RunspacePoolStrings.ResourceManager.GetString("RunspacePoolClosed", RunspacePoolStrings.resourceCulture);
		}
	}

	// Token: 0x04003245 RID: 12869
	private static ResourceManager resourceMan;

	// Token: 0x04003246 RID: 12870
	private static CultureInfo resourceCulture;
}
