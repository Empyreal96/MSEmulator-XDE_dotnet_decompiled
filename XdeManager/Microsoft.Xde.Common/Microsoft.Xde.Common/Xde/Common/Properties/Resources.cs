using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.Xde.Common.Properties
{
	// Token: 0x0200007D RID: 125
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x060002FD RID: 765 RVA: 0x00004A6C File Offset: 0x00002C6C
		internal Resources()
		{
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060002FE RID: 766 RVA: 0x00007F51 File Offset: 0x00006151
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Resources.resourceMan == null)
				{
					Resources.resourceMan = new ResourceManager("Microsoft.Xde.Common.Properties.Resources", typeof(Resources).Assembly);
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060002FF RID: 767 RVA: 0x00007F7D File Offset: 0x0000617D
		// (set) Token: 0x06000300 RID: 768 RVA: 0x00007F84 File Offset: 0x00006184
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000301 RID: 769 RVA: 0x00007F8C File Offset: 0x0000618C
		internal static string InvalidIPMask
		{
			get
			{
				return Resources.ResourceManager.GetString("InvalidIPMask", Resources.resourceCulture);
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000302 RID: 770 RVA: 0x00007FA2 File Offset: 0x000061A2
		internal static string InvalidIPPrefixString
		{
			get
			{
				return Resources.ResourceManager.GetString("InvalidIPPrefixString", Resources.resourceCulture);
			}
		}

		// Token: 0x040001C4 RID: 452
		private static ResourceManager resourceMan;

		// Token: 0x040001C5 RID: 453
		private static CultureInfo resourceCulture;
	}
}
