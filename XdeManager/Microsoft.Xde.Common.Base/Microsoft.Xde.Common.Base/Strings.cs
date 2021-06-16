using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000022 RID: 34
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Strings
	{
		// Token: 0x06000175 RID: 373 RVA: 0x000022FA File Offset: 0x000004FA
		internal Strings()
		{
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00003D1B File Offset: 0x00001F1B
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Strings.resourceMan == null)
				{
					Strings.resourceMan = new ResourceManager("Microsoft.Xde.Common.Strings", typeof(Strings).Assembly);
				}
				return Strings.resourceMan;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00003D47 File Offset: 0x00001F47
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00003D4E File Offset: 0x00001F4E
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Strings.resourceCulture;
			}
			set
			{
				Strings.resourceCulture = value;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00003D56 File Offset: 0x00001F56
		internal static string BytesSize_BFormat
		{
			get
			{
				return Strings.ResourceManager.GetString("BytesSize_BFormat", Strings.resourceCulture);
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00003D6C File Offset: 0x00001F6C
		internal static string BytesSize_GBFormat
		{
			get
			{
				return Strings.ResourceManager.GetString("BytesSize_GBFormat", Strings.resourceCulture);
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00003D82 File Offset: 0x00001F82
		internal static string BytesSize_KBFormat
		{
			get
			{
				return Strings.ResourceManager.GetString("BytesSize_KBFormat", Strings.resourceCulture);
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00003D98 File Offset: 0x00001F98
		internal static string BytesSize_MBFormat
		{
			get
			{
				return Strings.ResourceManager.GetString("BytesSize_MBFormat", Strings.resourceCulture);
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00003DAE File Offset: 0x00001FAE
		internal static string BytesSize_TBFormat
		{
			get
			{
				return Strings.ResourceManager.GetString("BytesSize_TBFormat", Strings.resourceCulture);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00003DC4 File Offset: 0x00001FC4
		internal static string CantBeEmpty
		{
			get
			{
				return Strings.ResourceManager.GetString("CantBeEmpty", Strings.resourceCulture);
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00003DDA File Offset: 0x00001FDA
		internal static string CantBeNullFormat
		{
			get
			{
				return Strings.ResourceManager.GetString("CantBeNullFormat", Strings.resourceCulture);
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00003DF0 File Offset: 0x00001FF0
		internal static string FailedToAddAccessPathToPartition
		{
			get
			{
				return Strings.ResourceManager.GetString("FailedToAddAccessPathToPartition", Strings.resourceCulture);
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00003E06 File Offset: 0x00002006
		internal static string FailedToFindPartitionInMountedVHD
		{
			get
			{
				return Strings.ResourceManager.GetString("FailedToFindPartitionInMountedVHD", Strings.resourceCulture);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00003E1C File Offset: 0x0000201C
		internal static string InvalidIPAddressLengths
		{
			get
			{
				return Strings.ResourceManager.GetString("InvalidIPAddressLengths", Strings.resourceCulture);
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00003E32 File Offset: 0x00002032
		internal static string InvalidParameterValue
		{
			get
			{
				return Strings.ResourceManager.GetString("InvalidParameterValue", Strings.resourceCulture);
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00003E48 File Offset: 0x00002048
		internal static string TargetTypeMustBeBool
		{
			get
			{
				return Strings.ResourceManager.GetString("TargetTypeMustBeBool", Strings.resourceCulture);
			}
		}

		// Token: 0x040000FE RID: 254
		private static ResourceManager resourceMan;

		// Token: 0x040000FF RID: 255
		private static CultureInfo resourceCulture;
	}
}
