using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.SantPlugin.Orientation
{
	// Token: 0x0200002A RID: 42
	[Export(typeof(IXdeTab))]
	[ExportMetadata("Name", "SantPlugin.OrientationTab")]
	public class OrientationTabCreator : IXdeTab, IXdePluginComponent
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00006B67 File Offset: 0x00004D67
		// (set) Token: 0x06000170 RID: 368 RVA: 0x00006B6F File Offset: 0x00004D6F
		[Import]
		public IXdeMinUiFactory UiFactory { get; set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00006B78 File Offset: 0x00004D78
		// (set) Token: 0x06000172 RID: 370 RVA: 0x00006B80 File Offset: 0x00004D80
		[Import]
		public IXdeOrientationFeature OrientationFeature { get; set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00006B89 File Offset: 0x00004D89
		// (set) Token: 0x06000174 RID: 372 RVA: 0x00006B91 File Offset: 0x00004D91
		[Import]
		public IXdeSku Sku { get; set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00006B9A File Offset: 0x00004D9A
		// (set) Token: 0x06000176 RID: 374 RVA: 0x00006BA2 File Offset: 0x00004DA2
		[Import]
		public IXdeControllerState ControllerState { get; set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00006BAB File Offset: 0x00004DAB
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00006BB3 File Offset: 0x00004DB3
		[Import]
		public IXdeDisplayOutput DisplayOutput { get; set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00006BBC File Offset: 0x00004DBC
		public string Name
		{
			get
			{
				return "SantPlugin.OrientationTab";
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00006BC3 File Offset: 0x00004DC3
		public string Caption
		{
			get
			{
				return Resources.OrientationTab_Caption;
			}
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00006BCA File Offset: 0x00004DCA
		public UserControl CreateTab()
		{
			if (this.ControllerState.Skin != null && this.ControllerState.Skin.Display.DisplayCount != 2)
			{
				return null;
			}
			DeviceView deviceView = new DeviceView();
			deviceView.Init(this.OrientationFeature, this.DisplayOutput);
			return deviceView;
		}

		// Token: 0x040000E3 RID: 227
		public const string SkuName = "SantPlugin.OrientationTab";
	}
}
