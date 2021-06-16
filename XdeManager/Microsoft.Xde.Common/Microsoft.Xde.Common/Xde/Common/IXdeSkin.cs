using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000063 RID: 99
	public interface IXdeSkin : INotifyPropertyChanged
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600022A RID: 554
		SkinDisplay Display { get; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600022B RID: 555
		// (set) Token: 0x0600022C RID: 556
		MonitorMode MonitorMode { get; set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600022D RID: 557
		// (set) Token: 0x0600022E RID: 558
		double DisplayScale { get; set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600022F RID: 559
		// (set) Token: 0x06000230 RID: 560
		DisplayOrientation Orientation { get; set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000231 RID: 561
		Size UnscaledBitmapSize { get; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000232 RID: 562
		IEnumerable<string> BitmapFileNames { get; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000233 RID: 563
		IEnumerable<ISkinButtonInfo> Buttons { get; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000234 RID: 564
		// (set) Token: 0x06000235 RID: 565
		string InformationText { get; set; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000236 RID: 566
		// (set) Token: 0x06000237 RID: 567
		XdeSensors Sensors { get; set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000238 RID: 568
		// (set) Token: 0x06000239 RID: 569
		int ActiveChromeCount { get; set; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600023A RID: 570
		// (set) Token: 0x0600023B RID: 571
		bool ShowExternalDisplay { get; set; }

		// Token: 0x0600023C RID: 572
		BitmapSource GetSkinBitmap(SkinBitmapIndex which);
	}
}
