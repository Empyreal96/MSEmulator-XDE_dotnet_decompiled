using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000065 RID: 101
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Strings
	{
		// Token: 0x06000240 RID: 576 RVA: 0x00004A6C File Offset: 0x00002C6C
		internal Strings()
		{
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000241 RID: 577 RVA: 0x000051C8 File Offset: 0x000033C8
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

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000242 RID: 578 RVA: 0x000051F4 File Offset: 0x000033F4
		// (set) Token: 0x06000243 RID: 579 RVA: 0x000051FB File Offset: 0x000033FB
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

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000244 RID: 580 RVA: 0x00005203 File Offset: 0x00003403
		internal static string BytesSize_BFormat
		{
			get
			{
				return Strings.ResourceManager.GetString("BytesSize_BFormat", Strings.resourceCulture);
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000245 RID: 581 RVA: 0x00005219 File Offset: 0x00003419
		internal static string BytesSize_GBFormat
		{
			get
			{
				return Strings.ResourceManager.GetString("BytesSize_GBFormat", Strings.resourceCulture);
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000246 RID: 582 RVA: 0x0000522F File Offset: 0x0000342F
		internal static string BytesSize_KBFormat
		{
			get
			{
				return Strings.ResourceManager.GetString("BytesSize_KBFormat", Strings.resourceCulture);
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000247 RID: 583 RVA: 0x00005245 File Offset: 0x00003445
		internal static string BytesSize_MBFormat
		{
			get
			{
				return Strings.ResourceManager.GetString("BytesSize_MBFormat", Strings.resourceCulture);
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000525B File Offset: 0x0000345B
		internal static string BytesSize_TBFormat
		{
			get
			{
				return Strings.ResourceManager.GetString("BytesSize_TBFormat", Strings.resourceCulture);
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000249 RID: 585 RVA: 0x00005271 File Offset: 0x00003471
		internal static string CantBeEmpty
		{
			get
			{
				return Strings.ResourceManager.GetString("CantBeEmpty", Strings.resourceCulture);
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600024A RID: 586 RVA: 0x00005287 File Offset: 0x00003487
		internal static string CantBeNullFormat
		{
			get
			{
				return Strings.ResourceManager.GetString("CantBeNullFormat", Strings.resourceCulture);
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600024B RID: 587 RVA: 0x0000529D File Offset: 0x0000349D
		internal static string FailedToAddAccessPathToPartition
		{
			get
			{
				return Strings.ResourceManager.GetString("FailedToAddAccessPathToPartition", Strings.resourceCulture);
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600024C RID: 588 RVA: 0x000052B3 File Offset: 0x000034B3
		internal static string FailedToFindPartitionInMountedVHD
		{
			get
			{
				return Strings.ResourceManager.GetString("FailedToFindPartitionInMountedVHD", Strings.resourceCulture);
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600024D RID: 589 RVA: 0x000052C9 File Offset: 0x000034C9
		internal static string InvalidIPAddressLengths
		{
			get
			{
				return Strings.ResourceManager.GetString("InvalidIPAddressLengths", Strings.resourceCulture);
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600024E RID: 590 RVA: 0x000052DF File Offset: 0x000034DF
		internal static string InvalidParameterValue
		{
			get
			{
				return Strings.ResourceManager.GetString("InvalidParameterValue", Strings.resourceCulture);
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600024F RID: 591 RVA: 0x000052F5 File Offset: 0x000034F5
		internal static string TargetTypeMustBeBool
		{
			get
			{
				return Strings.ResourceManager.GetString("TargetTypeMustBeBool", Strings.resourceCulture);
			}
		}

		// Token: 0x04000166 RID: 358
		private static ResourceManager resourceMan;

		// Token: 0x04000167 RID: 359
		private static CultureInfo resourceCulture;
	}
}
