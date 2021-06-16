using System;
using System.ComponentModel.Composition;
using Microsoft.Xde.Common;
using Microsoft.Xde.SantPlugin;

namespace Microsoft.Xde.H2LPlugin.Toolbar
{
	// Token: 0x0200000B RID: 11
	[Export(typeof(IXdeButton))]
	[ExportMetadata("Name", "H2LPlugin.OrientationRotateRight")]
	public class OrientationRotateRightButton : OrientationRotateButtonBase
	{
		// Token: 0x06000047 RID: 71 RVA: 0x00003112 File Offset: 0x00001312
		public OrientationRotateRightButton() : base("H2LPlugin.OrientationRotateRight", Resources.Orientation_Toolbar_RotateRightTooltip, Resources.Orientation_Toolbar_RotateRightIcon, true)
		{
		}

		// Token: 0x04000022 RID: 34
		public const string SkuName = "H2LPlugin.OrientationRotateRight";
	}
}
