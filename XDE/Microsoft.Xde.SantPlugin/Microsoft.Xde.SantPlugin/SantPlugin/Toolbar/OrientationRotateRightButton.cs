using System;
using System.ComponentModel.Composition;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.SantPlugin.Toolbar
{
	// Token: 0x02000016 RID: 22
	[Export(typeof(IXdeButton))]
	[ExportMetadata("Name", "SantPlugin.OrientationRotateRight")]
	public class OrientationRotateRightButton : OrientationRotateButtonBase
	{
		// Token: 0x060000C2 RID: 194 RVA: 0x000044C7 File Offset: 0x000026C7
		public OrientationRotateRightButton() : base("SantPlugin.OrientationRotateRight", Resources.Orientation_Toolbar_RotateRightTooltip, Resources.Orientation_Toolbar_RotateRightIcon, true)
		{
		}

		// Token: 0x04000058 RID: 88
		public const string SkuName = "SantPlugin.OrientationRotateRight";
	}
}
