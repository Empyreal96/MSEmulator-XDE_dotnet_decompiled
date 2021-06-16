using System;
using System.ComponentModel.Composition;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.SantPlugin.Toolbar
{
	// Token: 0x02000015 RID: 21
	[Export(typeof(IXdeButton))]
	[ExportMetadata("Name", "SantPlugin.OrientationRotateLeft")]
	public class OrientationRotateLeftButton : OrientationRotateButtonBase
	{
		// Token: 0x060000C1 RID: 193 RVA: 0x000044AF File Offset: 0x000026AF
		public OrientationRotateLeftButton() : base("SantPlugin.OrientationRotateLeft", Resources.Orientation_Toolbar_RotateLeftTooltip, Resources.Orientation_Toolbar_RotateLeftIcon, false)
		{
		}

		// Token: 0x04000057 RID: 87
		public const string SkuName = "SantPlugin.OrientationRotateLeft";
	}
}
