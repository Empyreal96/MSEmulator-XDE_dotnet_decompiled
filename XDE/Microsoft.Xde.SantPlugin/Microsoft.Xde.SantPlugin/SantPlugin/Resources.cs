using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.Xde.SantPlugin
{
	// Token: 0x02000011 RID: 17
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x06000086 RID: 134 RVA: 0x00003CFB File Offset: 0x00001EFB
		internal Resources()
		{
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00003D03 File Offset: 0x00001F03
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Resources.resourceMan == null)
				{
					Resources.resourceMan = new ResourceManager("Microsoft.Xde.SantPlugin.Resources", typeof(Resources).Assembly);
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00003D2F File Offset: 0x00001F2F
		// (set) Token: 0x06000089 RID: 137 RVA: 0x00003D36 File Offset: 0x00001F36
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

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003D3E File Offset: 0x00001F3E
		internal static Bitmap Orientation_Toolbar_ERL_Icon
		{
			get
			{
				return (Bitmap)Resources.ResourceManager.GetObject("Orientation_Toolbar_ERL_Icon", Resources.resourceCulture);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00003D59 File Offset: 0x00001F59
		internal static string Orientation_Toolbar_ERL_ToolTip
		{
			get
			{
				return Resources.ResourceManager.GetString("Orientation_Toolbar_ERL_ToolTip", Resources.resourceCulture);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00003D6F File Offset: 0x00001F6F
		internal static string Orientation_Toolbar_LRF_Tooltip
		{
			get
			{
				return Resources.ResourceManager.GetString("Orientation_Toolbar_LRF_Tooltip", Resources.resourceCulture);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00003D85 File Offset: 0x00001F85
		internal static Bitmap Orientation_Toolbar_NRL_Icon
		{
			get
			{
				return (Bitmap)Resources.ResourceManager.GetObject("Orientation_Toolbar_NRL_Icon", Resources.resourceCulture);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00003DA0 File Offset: 0x00001FA0
		internal static string Orientation_Toolbar_NRL_ToolTip
		{
			get
			{
				return Resources.ResourceManager.GetString("Orientation_Toolbar_NRL_ToolTip", Resources.resourceCulture);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00003DB6 File Offset: 0x00001FB6
		internal static string Orientation_Toolbar_OF_Tooltip
		{
			get
			{
				return Resources.ResourceManager.GetString("Orientation_Toolbar_OF_Tooltip", Resources.resourceCulture);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00003DCC File Offset: 0x00001FCC
		internal static Bitmap Orientation_Toolbar_RotateLeftIcon
		{
			get
			{
				return (Bitmap)Resources.ResourceManager.GetObject("Orientation_Toolbar_RotateLeftIcon", Resources.resourceCulture);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00003DE7 File Offset: 0x00001FE7
		internal static string Orientation_Toolbar_RotateLeftTooltip
		{
			get
			{
				return Resources.ResourceManager.GetString("Orientation_Toolbar_RotateLeftTooltip", Resources.resourceCulture);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00003DFD File Offset: 0x00001FFD
		internal static Bitmap Orientation_Toolbar_RotateRightIcon
		{
			get
			{
				return (Bitmap)Resources.ResourceManager.GetObject("Orientation_Toolbar_RotateRightIcon", Resources.resourceCulture);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00003E18 File Offset: 0x00002018
		internal static string Orientation_Toolbar_RotateRightTooltip
		{
			get
			{
				return Resources.ResourceManager.GetString("Orientation_Toolbar_RotateRightTooltip", Resources.resourceCulture);
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00003E2E File Offset: 0x0000202E
		internal static string OrientationFeature_OrientationSendFailedFormat
		{
			get
			{
				return Resources.ResourceManager.GetString("OrientationFeature_OrientationSendFailedFormat", Resources.resourceCulture);
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00003E44 File Offset: 0x00002044
		internal static string OrientationFeature_OrientationSetSampleValueFailed
		{
			get
			{
				return Resources.ResourceManager.GetString("OrientationFeature_OrientationSetSampleValueFailed", Resources.resourceCulture);
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00003E5A File Offset: 0x0000205A
		internal static string OrientationTab_Caption
		{
			get
			{
				return Resources.ResourceManager.GetString("OrientationTab_Caption", Resources.resourceCulture);
			}
		}

		// Token: 0x04000048 RID: 72
		private static ResourceManager resourceMan;

		// Token: 0x04000049 RID: 73
		private static CultureInfo resourceCulture;
	}
}
