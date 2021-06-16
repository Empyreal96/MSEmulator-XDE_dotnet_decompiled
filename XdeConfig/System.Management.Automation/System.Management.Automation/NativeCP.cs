using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A3B RID: 2619
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[CompilerGenerated]
[DebuggerNonUserCode]
internal class NativeCP
{
	// Token: 0x06006860 RID: 26720 RVA: 0x00211C36 File Offset: 0x0020FE36
	internal NativeCP()
	{
	}

	// Token: 0x17001C99 RID: 7321
	// (get) Token: 0x06006861 RID: 26721 RVA: 0x00211C40 File Offset: 0x0020FE40
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(NativeCP.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("NativeCP", typeof(NativeCP).Assembly);
				NativeCP.resourceMan = resourceManager;
			}
			return NativeCP.resourceMan;
		}
	}

	// Token: 0x17001C9A RID: 7322
	// (get) Token: 0x06006862 RID: 26722 RVA: 0x00211C7F File Offset: 0x0020FE7F
	// (set) Token: 0x06006863 RID: 26723 RVA: 0x00211C86 File Offset: 0x0020FE86
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return NativeCP.resourceCulture;
		}
		set
		{
			NativeCP.resourceCulture = value;
		}
	}

	// Token: 0x17001C9B RID: 7323
	// (get) Token: 0x06006864 RID: 26724 RVA: 0x00211C8E File Offset: 0x0020FE8E
	internal static string CliXmlError
	{
		get
		{
			return NativeCP.ResourceManager.GetString("CliXmlError", NativeCP.resourceCulture);
		}
	}

	// Token: 0x17001C9C RID: 7324
	// (get) Token: 0x06006865 RID: 26725 RVA: 0x00211CA4 File Offset: 0x0020FEA4
	internal static string IncorrectValueForCommandParameter
	{
		get
		{
			return NativeCP.ResourceManager.GetString("IncorrectValueForCommandParameter", NativeCP.resourceCulture);
		}
	}

	// Token: 0x17001C9D RID: 7325
	// (get) Token: 0x06006866 RID: 26726 RVA: 0x00211CBA File Offset: 0x0020FEBA
	internal static string IncorrectValueForFormatParameter
	{
		get
		{
			return NativeCP.ResourceManager.GetString("IncorrectValueForFormatParameter", NativeCP.resourceCulture);
		}
	}

	// Token: 0x17001C9E RID: 7326
	// (get) Token: 0x06006867 RID: 26727 RVA: 0x00211CD0 File Offset: 0x0020FED0
	internal static string NoValueForCommandParameter
	{
		get
		{
			return NativeCP.ResourceManager.GetString("NoValueForCommandParameter", NativeCP.resourceCulture);
		}
	}

	// Token: 0x17001C9F RID: 7327
	// (get) Token: 0x06006868 RID: 26728 RVA: 0x00211CE6 File Offset: 0x0020FEE6
	internal static string NoValueForInputFormatParameter
	{
		get
		{
			return NativeCP.ResourceManager.GetString("NoValueForInputFormatParameter", NativeCP.resourceCulture);
		}
	}

	// Token: 0x17001CA0 RID: 7328
	// (get) Token: 0x06006869 RID: 26729 RVA: 0x00211CFC File Offset: 0x0020FEFC
	internal static string NoValueForOutputFormatParameter
	{
		get
		{
			return NativeCP.ResourceManager.GetString("NoValueForOutputFormatParameter", NativeCP.resourceCulture);
		}
	}

	// Token: 0x17001CA1 RID: 7329
	// (get) Token: 0x0600686A RID: 26730 RVA: 0x00211D12 File Offset: 0x0020FF12
	internal static string NoValuesSpecifiedForArgs
	{
		get
		{
			return NativeCP.ResourceManager.GetString("NoValuesSpecifiedForArgs", NativeCP.resourceCulture);
		}
	}

	// Token: 0x17001CA2 RID: 7330
	// (get) Token: 0x0600686B RID: 26731 RVA: 0x00211D28 File Offset: 0x0020FF28
	internal static string ParameterSpecifiedAlready
	{
		get
		{
			return NativeCP.ResourceManager.GetString("ParameterSpecifiedAlready", NativeCP.resourceCulture);
		}
	}

	// Token: 0x17001CA3 RID: 7331
	// (get) Token: 0x0600686C RID: 26732 RVA: 0x00211D3E File Offset: 0x0020FF3E
	internal static string StringValueExpectedForFormatParameter
	{
		get
		{
			return NativeCP.ResourceManager.GetString("StringValueExpectedForFormatParameter", NativeCP.resourceCulture);
		}
	}

	// Token: 0x0400326D RID: 12909
	private static ResourceManager resourceMan;

	// Token: 0x0400326E RID: 12910
	private static CultureInfo resourceCulture;
}
