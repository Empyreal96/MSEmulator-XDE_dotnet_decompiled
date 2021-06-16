using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000004 RID: 4
	public class DpiAwareScreenInfo
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000245C File Offset: 0x0000065C
		// (set) Token: 0x06000012 RID: 18 RVA: 0x00002464 File Offset: 0x00000664
		public Rectangle Bounds { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000013 RID: 19 RVA: 0x0000246D File Offset: 0x0000066D
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002475 File Offset: 0x00000675
		public float DpiScale { get; private set; }

		// Token: 0x06000015 RID: 21 RVA: 0x00002480 File Offset: 0x00000680
		private static void GetDpi(DpiAwareScreenInfo.DpiType dpiType, int x, int y, out uint dpiX, out uint dpiY)
		{
			int num = DpiAwareScreenInfo.GetDpiForMonitor(DpiAwareScreenInfo.MonitorFromPoint(new Point(x, y), 2U), dpiType, out dpiX, out dpiY).ToInt32();
			if (num == -2147024809)
			{
				throw new ArgumentException("Unknown error. See https://msdn.microsoft.com/en-us/library/windows/desktop/dn280510.aspx for more information.");
			}
			if (num == 0)
			{
				return;
			}
			throw new COMException("Unknown error. See https://msdn.microsoft.com/en-us/library/windows/desktop/dn280510.aspx for more information.");
		}

		// Token: 0x06000016 RID: 22
		[DllImport("User32.dll")]
		private static extern IntPtr MonitorFromPoint([In] Point pt, [In] uint dwFlags);

		// Token: 0x06000017 RID: 23
		[DllImport("Shcore.dll")]
		private static extern IntPtr GetDpiForMonitor([In] IntPtr hmonitor, [In] DpiAwareScreenInfo.DpiType dpiType, out uint dpiX, out uint dpiY);

		// Token: 0x06000018 RID: 24 RVA: 0x000024CD File Offset: 0x000006CD
		public static IEnumerable<DpiAwareScreenInfo> GetScreensInfo()
		{
			foreach (Screen screen in Screen.AllScreens)
			{
				uint num;
				uint num2;
				DpiAwareScreenInfo.GetDpiForMonitor(DpiAwareScreenInfo.MonitorFromPoint(screen.Bounds.Location, 2U), DpiAwareScreenInfo.DpiType.EFFECTIVE, out num, out num2);
				Rectangle bounds = new Rectangle(screen.Bounds.Location, new Size(screen.Bounds.Width * (int)num / 96, screen.Bounds.Height * (int)num2 / 96));
				DpiAwareScreenInfo dpiAwareScreenInfo = new DpiAwareScreenInfo
				{
					Bounds = bounds,
					DpiScale = num / 96U
				};
				yield return dpiAwareScreenInfo;
			}
			Screen[] array = null;
			yield break;
		}

		// Token: 0x0400000E RID: 14
		private const int _S_OK = 0;

		// Token: 0x0400000F RID: 15
		private const int _MONITOR_DEFAULTTONEAREST = 2;

		// Token: 0x04000010 RID: 16
		private const int _E_INVALIDARG = -2147024809;

		// Token: 0x02000033 RID: 51
		public enum DpiType
		{
			// Token: 0x0400012D RID: 301
			EFFECTIVE,
			// Token: 0x0400012E RID: 302
			ANGULAR,
			// Token: 0x0400012F RID: 303
			RAW
		}
	}
}
