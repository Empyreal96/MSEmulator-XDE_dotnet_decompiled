using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.Xde.Common.Properties
{
	// Token: 0x0200002D RID: 45
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x060001C2 RID: 450 RVA: 0x000022FA File Offset: 0x000004FA
		internal Resources()
		{
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00004DE5 File Offset: 0x00002FE5
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

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x00004E11 File Offset: 0x00003011
		// (set) Token: 0x060001C5 RID: 453 RVA: 0x00004E18 File Offset: 0x00003018
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

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x00004E20 File Offset: 0x00003020
		internal static string InvalidIPMask
		{
			get
			{
				return Resources.ResourceManager.GetString("InvalidIPMask", Resources.resourceCulture);
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00004E36 File Offset: 0x00003036
		internal static string InvalidIPPrefixString
		{
			get
			{
				return Resources.ResourceManager.GetString("InvalidIPPrefixString", Resources.resourceCulture);
			}
		}

		// Token: 0x0400010B RID: 267
		private static ResourceManager resourceMan;

		// Token: 0x0400010C RID: 268
		private static CultureInfo resourceCulture;
	}
}
