using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A14 RID: 2580
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
internal class InternalHostStrings
{
	// Token: 0x060060B9 RID: 24761 RVA: 0x0020740C File Offset: 0x0020560C
	internal InternalHostStrings()
	{
	}

	// Token: 0x17001540 RID: 5440
	// (get) Token: 0x060060BA RID: 24762 RVA: 0x00207414 File Offset: 0x00205614
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(InternalHostStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("InternalHostStrings", typeof(InternalHostStrings).Assembly);
				InternalHostStrings.resourceMan = resourceManager;
			}
			return InternalHostStrings.resourceMan;
		}
	}

	// Token: 0x17001541 RID: 5441
	// (get) Token: 0x060060BB RID: 24763 RVA: 0x00207453 File Offset: 0x00205653
	// (set) Token: 0x060060BC RID: 24764 RVA: 0x0020745A File Offset: 0x0020565A
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return InternalHostStrings.resourceCulture;
		}
		set
		{
			InternalHostStrings.resourceCulture = value;
		}
	}

	// Token: 0x17001542 RID: 5442
	// (get) Token: 0x060060BD RID: 24765 RVA: 0x00207462 File Offset: 0x00205662
	internal static string EnterExitNestedPromptOutOfSync
	{
		get
		{
			return InternalHostStrings.ResourceManager.GetString("EnterExitNestedPromptOutOfSync", InternalHostStrings.resourceCulture);
		}
	}

	// Token: 0x17001543 RID: 5443
	// (get) Token: 0x060060BE RID: 24766 RVA: 0x00207478 File Offset: 0x00205678
	internal static string ExitNonExistentNestedPromptError
	{
		get
		{
			return InternalHostStrings.ResourceManager.GetString("ExitNonExistentNestedPromptError", InternalHostStrings.resourceCulture);
		}
	}

	// Token: 0x0400321F RID: 12831
	private static ResourceManager resourceMan;

	// Token: 0x04003220 RID: 12832
	private static CultureInfo resourceCulture;
}
