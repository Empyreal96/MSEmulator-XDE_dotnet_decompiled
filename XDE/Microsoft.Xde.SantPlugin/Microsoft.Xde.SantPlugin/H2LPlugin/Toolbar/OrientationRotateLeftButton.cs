using System;
using System.ComponentModel.Composition;
using Microsoft.Xde.Common;
using Microsoft.Xde.SantPlugin;

namespace Microsoft.Xde.H2LPlugin.Toolbar
{
	// Token: 0x0200000A RID: 10
	[Export(typeof(IXdeButton))]
	[ExportMetadata("Name", "H2LPlugin.OrientationRotateLeft")]
	public class OrientationRotateLeftButton : OrientationRotateButtonBase
	{
		// Token: 0x06000046 RID: 70 RVA: 0x000030FA File Offset: 0x000012FA
		public OrientationRotateLeftButton() : base("H2LPlugin.OrientationRotateLeft", Resources.Orientation_Toolbar_RotateLeftTooltip, Resources.Orientation_Toolbar_RotateLeftIcon, false)
		{
		}

		// Token: 0x04000021 RID: 33
		public const string SkuName = "H2LPlugin.OrientationRotateLeft";
	}
}
