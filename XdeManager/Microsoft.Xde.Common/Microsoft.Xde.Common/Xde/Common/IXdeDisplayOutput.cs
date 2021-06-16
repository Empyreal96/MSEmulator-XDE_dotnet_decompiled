using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200002C RID: 44
	public interface IXdeDisplayOutput
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000121 RID: 289
		int DisplayCount { get; }

		// Token: 0x06000122 RID: 290
		ImageSource GetDisplayOutput(int screenIndex);

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000123 RID: 291
		ImageSource ExternalMonitor { get; }

		// Token: 0x06000124 RID: 292
		BitmapSource CaptureScreen();

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000125 RID: 293
		// (set) Token: 0x06000126 RID: 294
		bool ExternalMonitorPowered { get; set; }
	}
}
