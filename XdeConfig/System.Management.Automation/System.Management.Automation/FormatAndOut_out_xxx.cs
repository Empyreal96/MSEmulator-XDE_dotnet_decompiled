using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A3A RID: 2618
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class FormatAndOut_out_xxx
{
	// Token: 0x06006856 RID: 26710 RVA: 0x00211B5A File Offset: 0x0020FD5A
	internal FormatAndOut_out_xxx()
	{
	}

	// Token: 0x17001C91 RID: 7313
	// (get) Token: 0x06006857 RID: 26711 RVA: 0x00211B64 File Offset: 0x0020FD64
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(FormatAndOut_out_xxx.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("FormatAndOut_out_xxx", typeof(FormatAndOut_out_xxx).Assembly);
				FormatAndOut_out_xxx.resourceMan = resourceManager;
			}
			return FormatAndOut_out_xxx.resourceMan;
		}
	}

	// Token: 0x17001C92 RID: 7314
	// (get) Token: 0x06006858 RID: 26712 RVA: 0x00211BA3 File Offset: 0x0020FDA3
	// (set) Token: 0x06006859 RID: 26713 RVA: 0x00211BAA File Offset: 0x0020FDAA
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return FormatAndOut_out_xxx.resourceCulture;
		}
		set
		{
			FormatAndOut_out_xxx.resourceCulture = value;
		}
	}

	// Token: 0x17001C93 RID: 7315
	// (get) Token: 0x0600685A RID: 26714 RVA: 0x00211BB2 File Offset: 0x0020FDB2
	internal static string ConsoleLineOutput_PagingPrompt
	{
		get
		{
			return FormatAndOut_out_xxx.ResourceManager.GetString("ConsoleLineOutput_PagingPrompt", FormatAndOut_out_xxx.resourceCulture);
		}
	}

	// Token: 0x17001C94 RID: 7316
	// (get) Token: 0x0600685B RID: 26715 RVA: 0x00211BC8 File Offset: 0x0020FDC8
	internal static string OutFile_Action
	{
		get
		{
			return FormatAndOut_out_xxx.ResourceManager.GetString("OutFile_Action", FormatAndOut_out_xxx.resourceCulture);
		}
	}

	// Token: 0x17001C95 RID: 7317
	// (get) Token: 0x0600685C RID: 26716 RVA: 0x00211BDE File Offset: 0x0020FDDE
	internal static string OutFile_FileOpenFailure
	{
		get
		{
			return FormatAndOut_out_xxx.ResourceManager.GetString("OutFile_FileOpenFailure", FormatAndOut_out_xxx.resourceCulture);
		}
	}

	// Token: 0x17001C96 RID: 7318
	// (get) Token: 0x0600685D RID: 26717 RVA: 0x00211BF4 File Offset: 0x0020FDF4
	internal static string OutLineOutput_InvalidLineOutputParameterType
	{
		get
		{
			return FormatAndOut_out_xxx.ResourceManager.GetString("OutLineOutput_InvalidLineOutputParameterType", FormatAndOut_out_xxx.resourceCulture);
		}
	}

	// Token: 0x17001C97 RID: 7319
	// (get) Token: 0x0600685E RID: 26718 RVA: 0x00211C0A File Offset: 0x0020FE0A
	internal static string OutLineOutput_NullLineOutputParameter
	{
		get
		{
			return FormatAndOut_out_xxx.ResourceManager.GetString("OutLineOutput_NullLineOutputParameter", FormatAndOut_out_xxx.resourceCulture);
		}
	}

	// Token: 0x17001C98 RID: 7320
	// (get) Token: 0x0600685F RID: 26719 RVA: 0x00211C20 File Offset: 0x0020FE20
	internal static string OutLineOutput_OutOfSequencePacket
	{
		get
		{
			return FormatAndOut_out_xxx.ResourceManager.GetString("OutLineOutput_OutOfSequencePacket", FormatAndOut_out_xxx.resourceCulture);
		}
	}

	// Token: 0x0400326B RID: 12907
	private static ResourceManager resourceMan;

	// Token: 0x0400326C RID: 12908
	private static CultureInfo resourceCulture;
}
