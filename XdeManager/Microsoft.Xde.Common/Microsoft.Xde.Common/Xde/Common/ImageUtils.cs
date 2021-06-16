using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000020 RID: 32
	public static class ImageUtils
	{
		// Token: 0x060000BF RID: 191
		[DllImport("gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool DeleteObject(IntPtr hObject);

		// Token: 0x060000C0 RID: 192 RVA: 0x00004C38 File Offset: 0x00002E38
		public static Bitmap CreateBitmapFromBitmapSource(BitmapSource bitmapsource)
		{
			if (bitmapsource == null)
			{
				throw new ArgumentNullException("bitmapsource");
			}
			FormatConvertedBitmap formatConvertedBitmap = new FormatConvertedBitmap();
			formatConvertedBitmap.BeginInit();
			formatConvertedBitmap.Source = bitmapsource;
			formatConvertedBitmap.DestinationFormat = PixelFormats.Bgra32;
			formatConvertedBitmap.EndInit();
			Bitmap bitmap = new Bitmap(formatConvertedBitmap.PixelWidth, formatConvertedBitmap.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(System.Drawing.Point.Empty, bitmap.Size), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			formatConvertedBitmap.CopyPixels(Int32Rect.Empty, bitmapData.Scan0, bitmapData.Height * bitmapData.Stride, bitmapData.Stride);
			bitmap.UnlockBits(bitmapData);
			return bitmap;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004CD8 File Offset: 0x00002ED8
		public static BitmapSource CreateBitmapSourceFromBitmap(Bitmap bitmap)
		{
			if (bitmap == null)
			{
				throw new ArgumentNullException("bitmap");
			}
			IntPtr hbitmap = bitmap.GetHbitmap();
			BitmapSource result;
			try
			{
				result = Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			}
			finally
			{
				ImageUtils.DeleteObject(hbitmap);
			}
			return result;
		}
	}
}
