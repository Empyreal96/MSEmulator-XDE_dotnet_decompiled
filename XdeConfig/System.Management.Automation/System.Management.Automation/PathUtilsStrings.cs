using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A1C RID: 2588
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
internal class PathUtilsStrings
{
	// Token: 0x06006315 RID: 25365 RVA: 0x0020A7EA File Offset: 0x002089EA
	internal PathUtilsStrings()
	{
	}

	// Token: 0x1700178C RID: 6028
	// (get) Token: 0x06006316 RID: 25366 RVA: 0x0020A7F4 File Offset: 0x002089F4
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(PathUtilsStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("PathUtilsStrings", typeof(PathUtilsStrings).Assembly);
				PathUtilsStrings.resourceMan = resourceManager;
			}
			return PathUtilsStrings.resourceMan;
		}
	}

	// Token: 0x1700178D RID: 6029
	// (get) Token: 0x06006317 RID: 25367 RVA: 0x0020A833 File Offset: 0x00208A33
	// (set) Token: 0x06006318 RID: 25368 RVA: 0x0020A83A File Offset: 0x00208A3A
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return PathUtilsStrings.resourceCulture;
		}
		set
		{
			PathUtilsStrings.resourceCulture = value;
		}
	}

	// Token: 0x1700178E RID: 6030
	// (get) Token: 0x06006319 RID: 25369 RVA: 0x0020A842 File Offset: 0x00208A42
	internal static string ExportPSSession_CannotCreateOutputDirectory
	{
		get
		{
			return PathUtilsStrings.ResourceManager.GetString("ExportPSSession_CannotCreateOutputDirectory", PathUtilsStrings.resourceCulture);
		}
	}

	// Token: 0x1700178F RID: 6031
	// (get) Token: 0x0600631A RID: 25370 RVA: 0x0020A858 File Offset: 0x00208A58
	internal static string ExportPSSession_ErrorDirectoryExists
	{
		get
		{
			return PathUtilsStrings.ResourceManager.GetString("ExportPSSession_ErrorDirectoryExists", PathUtilsStrings.resourceCulture);
		}
	}

	// Token: 0x17001790 RID: 6032
	// (get) Token: 0x0600631B RID: 25371 RVA: 0x0020A86E File Offset: 0x00208A6E
	internal static string ExportPSSession_ScriptGeneratorVersionMismatch
	{
		get
		{
			return PathUtilsStrings.ResourceManager.GetString("ExportPSSession_ScriptGeneratorVersionMismatch", PathUtilsStrings.resourceCulture);
		}
	}

	// Token: 0x17001791 RID: 6033
	// (get) Token: 0x0600631C RID: 25372 RVA: 0x0020A884 File Offset: 0x00208A84
	internal static string OutFile_DidNotResolveFile
	{
		get
		{
			return PathUtilsStrings.ResourceManager.GetString("OutFile_DidNotResolveFile", PathUtilsStrings.resourceCulture);
		}
	}

	// Token: 0x17001792 RID: 6034
	// (get) Token: 0x0600631D RID: 25373 RVA: 0x0020A89A File Offset: 0x00208A9A
	internal static string OutFile_MultipleFilesNotSupported
	{
		get
		{
			return PathUtilsStrings.ResourceManager.GetString("OutFile_MultipleFilesNotSupported", PathUtilsStrings.resourceCulture);
		}
	}

	// Token: 0x17001793 RID: 6035
	// (get) Token: 0x0600631E RID: 25374 RVA: 0x0020A8B0 File Offset: 0x00208AB0
	internal static string OutFile_ReadWriteFileNotFileSystemProvider
	{
		get
		{
			return PathUtilsStrings.ResourceManager.GetString("OutFile_ReadWriteFileNotFileSystemProvider", PathUtilsStrings.resourceCulture);
		}
	}

	// Token: 0x17001794 RID: 6036
	// (get) Token: 0x0600631F RID: 25375 RVA: 0x0020A8C6 File Offset: 0x00208AC6
	internal static string OutFile_WriteToFileEncodingUnknown
	{
		get
		{
			return PathUtilsStrings.ResourceManager.GetString("OutFile_WriteToFileEncodingUnknown", PathUtilsStrings.resourceCulture);
		}
	}

	// Token: 0x17001795 RID: 6037
	// (get) Token: 0x06006320 RID: 25376 RVA: 0x0020A8DC File Offset: 0x00208ADC
	internal static string UtilityFileExistsNoClobber
	{
		get
		{
			return PathUtilsStrings.ResourceManager.GetString("UtilityFileExistsNoClobber", PathUtilsStrings.resourceCulture);
		}
	}

	// Token: 0x0400322F RID: 12847
	private static ResourceManager resourceMan;

	// Token: 0x04003230 RID: 12848
	private static CultureInfo resourceCulture;
}
