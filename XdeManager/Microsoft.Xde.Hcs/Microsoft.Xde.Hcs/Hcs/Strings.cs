using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Hcs
{
	// Token: 0x02000006 RID: 6
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
	internal class Strings
	{
		// Token: 0x06000013 RID: 19 RVA: 0x00002458 File Offset: 0x00000658
		internal Strings()
		{
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002460 File Offset: 0x00000660
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Strings.resourceMan == null)
				{
					Strings.resourceMan = new ResourceManager("Microsoft.Xde.Hcs.Strings", typeof(Strings).Assembly);
				}
				return Strings.resourceMan;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000015 RID: 21 RVA: 0x0000248C File Offset: 0x0000068C
		// (set) Token: 0x06000016 RID: 22 RVA: 0x00002493 File Offset: 0x00000693
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

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000017 RID: 23 RVA: 0x0000249B File Offset: 0x0000069B
		internal static string HCSFunctionCallError
		{
			get
			{
				return Strings.ResourceManager.GetString("HCSFunctionCallError", Strings.resourceCulture);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000024B1 File Offset: 0x000006B1
		internal static string OnlyGen2Supported
		{
			get
			{
				return Strings.ResourceManager.GetString("OnlyGen2Supported", Strings.resourceCulture);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000024C7 File Offset: 0x000006C7
		internal static string VGPUDriverInFailedState
		{
			get
			{
				return Strings.ResourceManager.GetString("VGPUDriverInFailedState", Strings.resourceCulture);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000024DD File Offset: 0x000006DD
		internal static string VMAlreadyExists
		{
			get
			{
				return Strings.ResourceManager.GetString("VMAlreadyExists", Strings.resourceCulture);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000024F3 File Offset: 0x000006F3
		internal static string VMAlreadyRunning
		{
			get
			{
				return Strings.ResourceManager.GetString("VMAlreadyRunning", Strings.resourceCulture);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002509 File Offset: 0x00000709
		internal static string VMIsntRunning
		{
			get
			{
				return Strings.ResourceManager.GetString("VMIsntRunning", Strings.resourceCulture);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0000251F File Offset: 0x0000071F
		internal static string VMMustBeRunningForSnapshot
		{
			get
			{
				return Strings.ResourceManager.GetString("VMMustBeRunningForSnapshot", Strings.resourceCulture);
			}
		}

		// Token: 0x04000004 RID: 4
		private static ResourceManager resourceMan;

		// Token: 0x04000005 RID: 5
		private static CultureInfo resourceCulture;
	}
}
