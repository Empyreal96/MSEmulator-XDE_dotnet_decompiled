using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200002A RID: 42
	public static class XamlUtils
	{
		// Token: 0x060001B0 RID: 432 RVA: 0x000044B0 File Offset: 0x000026B0
		public static BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
		{
			BitmapImage bitmapImage;
			try
			{
				using (MemoryStream bitmapAsPngStream = XamlUtils.GetBitmapAsPngStream(bitmap))
				{
					bitmapImage = new BitmapImage();
					bitmapImage.BeginInit();
					bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
					bitmapImage.StreamSource = bitmapAsPngStream;
					bitmapImage.EndInit();
				}
			}
			finally
			{
				GC.Collect();
			}
			return bitmapImage;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00004514 File Offset: 0x00002714
		public static Bitmap ConvertBitmapSourceToBitmap(BitmapSource bitmapSource)
		{
			FormatConvertedBitmap formatConvertedBitmap = new FormatConvertedBitmap();
			formatConvertedBitmap.BeginInit();
			formatConvertedBitmap.Source = bitmapSource;
			formatConvertedBitmap.DestinationFormat = PixelFormats.Bgra32;
			formatConvertedBitmap.EndInit();
			Bitmap bitmap = new Bitmap(formatConvertedBitmap.PixelWidth, formatConvertedBitmap.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(System.Drawing.Point.Empty, bitmap.Size), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			formatConvertedBitmap.CopyPixels(Int32Rect.Empty, bitmapData.Scan0, bitmapData.Height * bitmapData.Stride, bitmapData.Stride);
			bitmap.UnlockBits(bitmapData);
			return bitmap;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x000045A8 File Offset: 0x000027A8
		public static T GetParentObjectFromVisualTree<T>(DependencyObject startObject) where T : class
		{
			T t = default(T);
			DependencyObject parent = VisualTreeHelper.GetParent(startObject);
			while (parent != null && t == null)
			{
				t = (parent as T);
				parent = VisualTreeHelper.GetParent(parent);
			}
			return t;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x000045E8 File Offset: 0x000027E8
		private static ImageCodecInfo GetEncoder(ImageFormat format)
		{
			return ImageCodecInfo.GetImageDecoders().FirstOrDefault((ImageCodecInfo c) => c.FormatID == format.Guid);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00004618 File Offset: 0x00002818
		private static MemoryStream GetBitmapAsPngStream(Bitmap rawBitMap)
		{
			MemoryStream result;
			using (Bitmap bitmap = new Bitmap(rawBitMap))
			{
				MemoryStream memoryStream = null;
				bitmap.SetResolution(96f, 96f);
				try
				{
					memoryStream = new MemoryStream();
					using (EncoderParameters encoderParameters = new EncoderParameters(1))
					{
						ImageCodecInfo encoder = XamlUtils.GetEncoder(ImageFormat.Png);
						EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, 100L);
						encoderParameters.Param[0] = encoderParameter;
						bitmap.Save(memoryStream, encoder, encoderParameters);
					}
					MemoryStream memoryStream2 = memoryStream;
					memoryStream = null;
					result = memoryStream2;
				}
				finally
				{
					if (memoryStream != null)
					{
						memoryStream.Dispose();
					}
				}
			}
			return result;
		}
	}
}
