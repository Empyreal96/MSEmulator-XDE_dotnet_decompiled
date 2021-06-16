using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using Microsoft.Xde.Base.Properties;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Base
{
	// Token: 0x02000005 RID: 5
	public class Skin : IXdeSkin, INotifyPropertyChanged
	{
		// Token: 0x0600003A RID: 58 RVA: 0x00002A0E File Offset: 0x00000C0E
		private void OnPropertyChanged(string propName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(propName));
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600003B RID: 59 RVA: 0x00002A28 File Offset: 0x00000C28
		// (remove) Token: 0x0600003C RID: 60 RVA: 0x00002A60 File Offset: 0x00000C60
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002A95 File Offset: 0x00000C95
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00002A9D File Offset: 0x00000C9D
		public double DisplayScale
		{
			get
			{
				return this.scale;
			}
			set
			{
				if (this.scale != value)
				{
					this.scale = value;
					this.OnPropertyChanged("DisplayScale");
				}
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002ABA File Offset: 0x00000CBA
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00002AC2 File Offset: 0x00000CC2
		public DisplayOrientation Orientation
		{
			get
			{
				return this.orientation;
			}
			set
			{
				this.orientation = value;
				this.OnPropertyChanged("Orientation");
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002AD6 File Offset: 0x00000CD6
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00002ADE File Offset: 0x00000CDE
		public MonitorMode MonitorMode
		{
			get
			{
				return this.monitorMode;
			}
			set
			{
				if (this.monitorMode != value)
				{
					this.monitorMode = value;
					this.OnPropertyChanged("MonitorMode");
				}
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002AFB File Offset: 0x00000CFB
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00002B03 File Offset: 0x00000D03
		[XmlIgnore]
		public XdeSensors Sensors
		{
			get
			{
				return this.sensors;
			}
			set
			{
				if (this.sensors != value)
				{
					this.sensors = value;
					this.OnPropertyChanged("Sensors");
				}
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002B20 File Offset: 0x00000D20
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00002B28 File Offset: 0x00000D28
		[XmlIgnore]
		public Point DisplayLocation { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002B34 File Offset: 0x00000D34
		[XmlIgnore]
		public Size UnscaledBitmapSize
		{
			get
			{
				BitmapSource skinBitmap = this.GetSkinBitmap(SkinBitmapIndex.Up);
				return new Size(skinBitmap.PixelWidth, skinBitmap.PixelHeight);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002B5A File Offset: 0x00000D5A
		[XmlIgnore]
		public IEnumerable<string> BitmapFileNames
		{
			get
			{
				return this.bitmapFileNames.AsReadOnly();
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002B67 File Offset: 0x00000D67
		[XmlIgnore]
		public IEnumerable<ISkinButtonInfo> Buttons
		{
			get
			{
				return this.buttons;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002B6F File Offset: 0x00000D6F
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00002B77 File Offset: 0x00000D77
		public string InformationText
		{
			get
			{
				return this.infoText;
			}
			set
			{
				if (!string.IsNullOrEmpty(this.infoText))
				{
					throw new InvalidOperationException(Strings.CanOnlySetInformationTextOnce);
				}
				this.infoText = value;
				this.OnPropertyChanged("InformationText");
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002BA3 File Offset: 0x00000DA3
		// (set) Token: 0x0600004D RID: 77 RVA: 0x00002BC0 File Offset: 0x00000DC0
		[XmlIgnore]
		public int ActiveChromeCount
		{
			get
			{
				if (this.activeChromeCount != -1)
				{
					return this.activeChromeCount;
				}
				return this.Display.InitialActiveChromeCount;
			}
			set
			{
				this.activeChromeCount = value;
				this.OnPropertyChanged("ActiveChromeCount");
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002BD4 File Offset: 0x00000DD4
		// (set) Token: 0x0600004F RID: 79 RVA: 0x00002BDC File Offset: 0x00000DDC
		[XmlIgnore]
		public bool ShowExternalDisplay
		{
			get
			{
				return this.showExternalDisplay;
			}
			set
			{
				this.showExternalDisplay = value;
				this.OnPropertyChanged("ShowExternalDisplay");
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00002BF0 File Offset: 0x00000DF0
		private bool IsAutoSize
		{
			get
			{
				return this.Display.AutoSize != null;
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002C00 File Offset: 0x00000E00
		public static Skin LoadSkinInformation(string skinFileName, string resName = null)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Skin));
			Skin skin;
			using (StreamReader streamReader = new StreamReader(skinFileName))
			{
				skin = (Skin)xmlSerializer.Deserialize(streamReader);
				if (resName != null)
				{
					skin.InitForAutoSize(resName);
				}
			}
			return skin;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002C58 File Offset: 0x00000E58
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Skin LoadSkin(string skinFileName, XdeSensors sensors, Skin skinInfo)
		{
			Skin skin = skinInfo;
			if (skin == null)
			{
				skin = Skin.LoadSkinInformation(skinFileName, null);
			}
			string directoryName = Path.GetDirectoryName(skinFileName);
			skin.sensors = sensors;
			skin.LoadBitmaps(directoryName);
			return skin;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002C88 File Offset: 0x00000E88
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Skin LoadSkin(string basedOnSkinFileName, string skinFileName)
		{
			Skin skin = Skin.LoadSkinInformation(basedOnSkinFileName, null);
			Skin skin2 = Skin.LoadSkinInformation(skinFileName, null);
			string directoryName = Path.GetDirectoryName(skinFileName);
			skin2.LoadBitmaps(directoryName, skin.Display);
			return skin2;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002CB8 File Offset: 0x00000EB8
		public ISkinButtonInfo GetButtonFromPoint(Point point)
		{
			foreach (Skin.ButtonInfo buttonInfo in this.buttons)
			{
				if (buttonInfo.IsEnabled && buttonInfo.Bounds.Contains(point))
				{
					return buttonInfo;
				}
			}
			return null;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002D24 File Offset: 0x00000F24
		public BitmapSource GetSkinBitmap(SkinBitmapIndex which)
		{
			BitmapSource bitmapSource = Skin.LoadSkinBitmapSource(this.bitmapFileNames[(int)which]);
			if (!this.IsAutoSize)
			{
				return bitmapSource;
			}
			return this.CreateAutoSizedBitmap(bitmapSource);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002D54 File Offset: 0x00000F54
		private void InitForAutoSize(string resName)
		{
			if (this.Display.DisplayWidth == 0 || this.Display.DisplayHeight == 0)
			{
				Size size;
				if (!SkinFactory.TryParseSizeFromResName(resName, out size))
				{
					throw new ArgumentException();
				}
				this.Display.DisplayWidth = size.Width;
				this.Display.DisplayHeight = size.Height;
			}
			if (this.IsAutoSize)
			{
				this.Display.DisplayPosX = this.Display.AutoSize.TopLeft.Right;
				this.Display.DisplayPosY = this.Display.AutoSize.TopLeft.Bottom;
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002DFD File Offset: 0x00000FFD
		private static Bitmap LoadSkinBitmap(string fileName)
		{
			return (Bitmap)Image.FromFile(fileName);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002E0A File Offset: 0x0000100A
		private static BitmapSource LoadSkinBitmapSource(string fileName)
		{
			return new BitmapImage(new Uri(fileName));
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002E18 File Offset: 0x00001018
		private void LoadBitmaps(string folderName, SkinDisplay displayForImages)
		{
			this.bitmapFileNames.Clear();
			this.bitmapFileNames.Add(Path.Combine(folderName, displayForImages.NormalImage));
			this.bitmapFileNames.Add(Path.Combine(folderName, displayForImages.DownImage));
			this.bitmapFileNames.Add(Path.Combine(folderName, displayForImages.MappingImage));
			this.Display.Buttons = displayForImages.Buttons;
			this.Display.QuickAccessToolbarXPos = displayForImages.QuickAccessToolbarXPos;
			this.Display.QuickAccessToolbarYPos = displayForImages.QuickAccessToolbarYPos;
			this.Display.DisplayPosX = displayForImages.DisplayPosX;
			this.Display.DisplayPosY = displayForImages.DisplayPosY;
			using (Bitmap bitmap = XamlUtils.ConvertBitmapSourceToBitmap(this.GetSkinBitmap(SkinBitmapIndex.Mask)))
			{
				Color[] array = new Color[this.Display.Buttons.Length];
				for (int i = 0; i < this.Display.Buttons.Length; i++)
				{
					array[i] = this.Display.Buttons[i].MappingColor;
				}
				Rectangle[] maskedRectFromBitmap = Skin.ButtonInfo.GetMaskedRectFromBitmap(bitmap, array);
				for (int j = 0; j < this.Display.Buttons.Length; j++)
				{
					Rectangle bounds = maskedRectFromBitmap[j];
					if (!bounds.IsEmpty)
					{
						SkinButton button = this.Display.Buttons[j];
						Skin.ButtonInfo buttonInfo = new Skin.ButtonInfo(this, button);
						buttonInfo.Init(bounds);
						this.buttons.Add(buttonInfo);
					}
				}
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002FA0 File Offset: 0x000011A0
		private void LoadBitmaps(string folderName)
		{
			this.LoadBitmaps(folderName, this.Display);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002FB0 File Offset: 0x000011B0
		private BitmapSource CreateAutoSizedBitmap(BitmapSource bitmapSource)
		{
			BitmapSource result;
			using (Bitmap bitmap = XamlUtils.ConvertBitmapSourceToBitmap(bitmapSource))
			{
				SkinAutoSize autoSize = this.Display.AutoSize;
				int totalDisplayWidth = this.Display.TotalDisplayWidth;
				int displayHeight = this.Display.DisplayHeight;
				int width = autoSize.TopLeft.Width + totalDisplayWidth + autoSize.TopRight.Width;
				int height = autoSize.TopLeft.Height + displayHeight + autoSize.BottomLeft.Height;
				int displayWidth = this.Display.DisplayWidth;
				int displayGapWidth = this.Display.DisplayGapWidth;
				using (Bitmap bitmap2 = new Bitmap(width, height, bitmap.PixelFormat))
				{
					using (Graphics graphics = Graphics.FromImage(bitmap2))
					{
						graphics.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, width, height));
						Rectangle topLeft = autoSize.TopLeft;
						graphics.DrawImage(bitmap, topLeft, autoSize.TopLeft, GraphicsUnit.Pixel);
						Rectangle rectangle = new Rectangle(autoSize.TopLeft.Width, 0, totalDisplayWidth, autoSize.TopLeft.Height);
						Rectangle destRect = new Rectangle(rectangle.Right, 0, autoSize.TopRight.Width, autoSize.TopRight.Height);
						graphics.DrawImage(bitmap, destRect, autoSize.TopRight, GraphicsUnit.Pixel);
						Rectangle rect = new Rectangle(autoSize.TopLeft.Width, autoSize.TopLeft.Height, totalDisplayWidth, displayHeight);
						graphics.FillRectangle(Brushes.Black, rect);
						Rectangle destRect2 = new Rectangle(0, autoSize.TopLeft.Height, autoSize.TopLeft.Width, rect.Height);
						graphics.DrawImage(bitmap, destRect2, autoSize.MiddleLeft, GraphicsUnit.Pixel);
						Rectangle rectangle2 = new Rectangle(destRect.X, destRect.Y + destRect.Height, autoSize.TopRight.Width, rect.Height);
						graphics.DrawImage(bitmap, rectangle2, autoSize.MiddleRight, GraphicsUnit.Pixel);
						Rectangle destRect3 = new Rectangle(destRect2.X, destRect2.Bottom, autoSize.BottomLeft.Width, autoSize.BottomLeft.Height);
						graphics.DrawImage(bitmap, destRect3, autoSize.BottomLeft, GraphicsUnit.Pixel);
						Rectangle rectangle3 = new Rectangle(destRect3.Right, destRect3.Y, rect.Width, autoSize.BottomFill.Height);
						Rectangle destRect4 = new Rectangle(rectangle3.Right, rectangle3.Y, autoSize.BottomRight.Width, autoSize.BottomRight.Height);
						graphics.DrawImage(bitmap, destRect4, autoSize.BottomRight, GraphicsUnit.Pixel);
						if (this.Display.DisplayCount == 1)
						{
							graphics.DrawImage(bitmap, rectangle, autoSize.TopFill, GraphicsUnit.Pixel);
							graphics.DrawImage(bitmap, rectangle3, autoSize.BottomFill, GraphicsUnit.Pixel);
						}
						else if (this.Display.DisplayCount == 2)
						{
							Rectangle rectangle4 = new Rectangle(rectangle.Left + displayWidth - autoSize.Display1TopRight.Width, 0, autoSize.Display1TopRight.Width + displayGapWidth + autoSize.Display2TopLeft.Width, autoSize.Display1TopRight.Height);
							Rectangle rectangle5 = new Rectangle(rectangle3.Left + displayWidth - autoSize.Display1BottomRight.Width, rectangle3.Bottom - autoSize.Display1BottomRight.Height, autoSize.Display1BottomRight.Width + displayGapWidth + autoSize.Display2BottomLeft.Width, autoSize.Display1TopRight.Height);
							Rectangle destRect5 = new Rectangle(rectangle.Left, rectangle.Top, displayWidth, rectangle.Height);
							graphics.DrawImage(bitmap, destRect5, autoSize.TopFill, GraphicsUnit.Pixel);
							Rectangle destRect6 = new Rectangle(rectangle4.Right, rectangle.Top, rectangle.Right - rectangle4.Right, rectangle.Height);
							graphics.DrawImage(bitmap, destRect6, autoSize.TopFill, GraphicsUnit.Pixel);
							Rectangle destRect7 = new Rectangle(rectangle3.Left, rectangle3.Top, displayWidth, rectangle3.Height);
							graphics.DrawImage(bitmap, destRect7, autoSize.BottomFill, GraphicsUnit.Pixel);
							Rectangle destRect8 = new Rectangle(rectangle5.Right, rectangle3.Top, rectangle3.Right - rectangle5.Right, rectangle3.Height);
							graphics.DrawImage(bitmap, destRect8, autoSize.BottomFill, GraphicsUnit.Pixel);
							Rectangle destRect9 = new Rectangle(rectangle4.Left, rectangle4.Top, autoSize.Display1TopRight.Width, autoSize.Display1TopRight.Height);
							graphics.DrawImage(bitmap, destRect9, autoSize.Display1TopRight, GraphicsUnit.Pixel);
							Rectangle destRect10 = new Rectangle(destRect9.Right + displayGapWidth, destRect9.Top, autoSize.Display2TopLeft.Width, autoSize.Display2TopLeft.Height);
							graphics.DrawImage(bitmap, destRect10, autoSize.Display2TopLeft, GraphicsUnit.Pixel);
							Rectangle destRect11 = new Rectangle(rectangle5.Left, rectangle5.Top, autoSize.Display1BottomRight.Width, autoSize.Display1BottomRight.Height);
							graphics.DrawImage(bitmap, destRect11, autoSize.Display1BottomRight, GraphicsUnit.Pixel);
							Rectangle destRect12 = new Rectangle(destRect11.Right + displayGapWidth, destRect11.Top, autoSize.Display2BottomLeft.Width, autoSize.Display2BottomLeft.Height);
							graphics.DrawImage(bitmap, destRect12, autoSize.Display2BottomLeft, GraphicsUnit.Pixel);
						}
						int num = autoSize.TopRight.Left - autoSize.TopLeft.Right;
						int num2 = autoSize.BottomLeft.Top - autoSize.TopLeft.Top;
						Rectangle rectangle6 = Rectangle.Empty;
						Skin.ButtonInfo buttonInfo = null;
						foreach (Skin.ButtonInfo buttonInfo2 in this.buttons)
						{
							Rectangle bounds = buttonInfo2.Bounds;
							Rectangle rectangle7 = bounds;
							switch (buttonInfo2.Anchor)
							{
							case SkinButtonAnchor.Bottom:
							case SkinButtonAnchor.Top:
							{
								Rectangle rectangle8 = (buttonInfo2.Anchor == SkinButtonAnchor.Bottom) ? rectangle3 : rectangle;
								int num3 = (buttonInfo2.Anchor == SkinButtonAnchor.Bottom) ? autoSize.BottomFill.Top : autoSize.TopFill.Top;
								int num4 = bounds.Top - num3;
								float num5 = ((float)bounds.X + (float)bounds.Width / 2f - (float)autoSize.BottomLeft.Right) / (float)num;
								rectangle7.Y = rectangle8.Top + num4;
								rectangle7.X = rectangle3.X + (int)((float)rectangle3.Width * num5) - bounds.Width / 2;
								graphics.DrawImage(bitmap, rectangle7, bounds, GraphicsUnit.Pixel);
								break;
							}
							case SkinButtonAnchor.Right:
							{
								Rectangle rectangle9 = rectangle2;
								int left = autoSize.TopRight.Left;
								int num6 = bounds.Left - left;
								float num7 = ((float)bounds.Y + (float)bounds.Height / 2f - (float)autoSize.BottomLeft.Right) / (float)num2;
								rectangle7.X = rectangle9.Left + num6;
								rectangle7.Y = rectangle2.Y + (int)((float)rectangle2.Height * num7) - bounds.Height / 2;
								graphics.DrawImage(bitmap, rectangle7, bounds, GraphicsUnit.Pixel);
								break;
							}
							case SkinButtonAnchor.Previous:
								rectangle7 = rectangle6;
								rectangle7.Width = bounds.Width;
								rectangle7.Height = bounds.Height;
								if (buttonInfo.Anchor == SkinButtonAnchor.Bottom || buttonInfo.Anchor == SkinButtonAnchor.Top)
								{
									SkinButtonAnchor anchor = buttonInfo.Anchor;
									int num8 = (buttonInfo2.Anchor == SkinButtonAnchor.Bottom) ? autoSize.BottomFill.Top : autoSize.TopFill.Top;
									int top = bounds.Top;
									rectangle7.X = rectangle7.Right;
								}
								else
								{
									if (buttonInfo.Anchor != SkinButtonAnchor.Right)
									{
										throw new InvalidOperationException();
									}
									Rectangle rectangle10 = rectangle2;
									int left2 = autoSize.TopRight.Left;
									int num9 = bounds.Left - left2;
									rectangle7.X = rectangle10.Left + num9;
									rectangle7.Y = rectangle7.Bottom;
								}
								graphics.DrawImage(bitmap, rectangle7, bounds, GraphicsUnit.Pixel);
								break;
							}
							rectangle6 = rectangle7;
							buttonInfo = buttonInfo2;
							buttonInfo2.Init(rectangle7);
						}
						result = XamlUtils.ConvertBitmapToBitmapImage(bitmap2);
					}
				}
			}
			return result;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600005C RID: 92 RVA: 0x000038DC File Offset: 0x00001ADC
		// (set) Token: 0x0600005D RID: 93 RVA: 0x000038E4 File Offset: 0x00001AE4
		public SkinDisplay Display { get; set; }

		// Token: 0x04000020 RID: 32
		private double scale;

		// Token: 0x04000021 RID: 33
		private List<Skin.ButtonInfo> buttons = new List<Skin.ButtonInfo>();

		// Token: 0x04000022 RID: 34
		private string infoText;

		// Token: 0x04000023 RID: 35
		private DisplayOrientation orientation;

		// Token: 0x04000024 RID: 36
		private List<string> bitmapFileNames = new List<string>();

		// Token: 0x04000025 RID: 37
		private MonitorMode monitorMode;

		// Token: 0x04000026 RID: 38
		private XdeSensors sensors = XdeSensors.Default;

		// Token: 0x04000027 RID: 39
		private int activeChromeCount = -1;

		// Token: 0x04000028 RID: 40
		private bool showExternalDisplay;

		// Token: 0x02000010 RID: 16
		private class ButtonInfo : ISkinButtonInfo
		{
			// Token: 0x06000117 RID: 279 RVA: 0x00005BDF File Offset: 0x00003DDF
			public ButtonInfo(Skin skin, SkinButton button)
			{
				this.Skin = skin;
				this.Button = button;
				this.ButtonType = button.Type;
				this.Anchor = button.Anchor;
			}

			// Token: 0x17000050 RID: 80
			// (get) Token: 0x06000118 RID: 280 RVA: 0x00005C0D File Offset: 0x00003E0D
			// (set) Token: 0x06000119 RID: 281 RVA: 0x00005C15 File Offset: 0x00003E15
			public SkinButtonType ButtonType { get; set; }

			// Token: 0x17000051 RID: 81
			// (get) Token: 0x0600011A RID: 282 RVA: 0x00005C1E File Offset: 0x00003E1E
			// (set) Token: 0x0600011B RID: 283 RVA: 0x00005C26 File Offset: 0x00003E26
			public SkinButtonAnchor Anchor { get; set; }

			// Token: 0x17000052 RID: 82
			// (get) Token: 0x0600011C RID: 284 RVA: 0x00005C2F File Offset: 0x00003E2F
			public Keys[] KeyCode
			{
				get
				{
					return this.Button.KeyMapping;
				}
			}

			// Token: 0x17000053 RID: 83
			// (get) Token: 0x0600011D RID: 285 RVA: 0x00005C3C File Offset: 0x00003E3C
			// (set) Token: 0x0600011E RID: 286 RVA: 0x00005C44 File Offset: 0x00003E44
			public Skin Skin { get; set; }

			// Token: 0x17000054 RID: 84
			// (get) Token: 0x0600011F RID: 287 RVA: 0x00005C4D File Offset: 0x00003E4D
			public Rectangle Bounds
			{
				get
				{
					return this.bounds;
				}
			}

			// Token: 0x17000055 RID: 85
			// (get) Token: 0x06000120 RID: 288 RVA: 0x00005C55 File Offset: 0x00003E55
			// (set) Token: 0x06000121 RID: 289 RVA: 0x00005C5D File Offset: 0x00003E5D
			public SkinButton Button { get; set; }

			// Token: 0x17000056 RID: 86
			// (get) Token: 0x06000122 RID: 290 RVA: 0x00005C66 File Offset: 0x00003E66
			public bool IsEnabled
			{
				get
				{
					return this.KeyCode != null && this.KeyCode.Length != 0 && this.IsVisible;
				}
			}

			// Token: 0x17000057 RID: 87
			// (get) Token: 0x06000123 RID: 291 RVA: 0x00005C81 File Offset: 0x00003E81
			public bool IsVisible
			{
				get
				{
					return (this.Button.SensorHideMask & this.Skin.Sensors) == XdeSensors.None && (this.Button.SensorShowMask & this.Skin.Sensors) > XdeSensors.None;
				}
			}

			// Token: 0x06000124 RID: 292 RVA: 0x00005CB8 File Offset: 0x00003EB8
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public unsafe static Rectangle[] GetMaskedRectFromBitmap(Bitmap image, Color[] colors)
			{
				int num = colors.Length;
				int[] array = new int[num];
				Rectangle[] array2 = new Rectangle[num];
				bool[] array3 = new bool[num];
				for (int i = 0; i < colors.Length; i++)
				{
					Color color = colors[i];
					array[i] = Color.FromArgb((int)color.R, (int)color.G, (int)color.B).ToArgb();
					array3[i] = false;
				}
				BitmapData bitmapData = null;
				try
				{
					bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
					for (int j = 0; j < image.Height; j++)
					{
						for (int k = 0; k < image.Width; k++)
						{
							int* ptr = (int*)((byte*)((byte*)((void*)bitmapData.Scan0) + k * 4) + j * bitmapData.Stride);
							int num2 = *ptr;
							for (int l = 0; l < num; l++)
							{
								int num3 = array[l];
								if (num2 == num3)
								{
									int num4 = k + 1;
									while (num4 < image.Width && num2 == num3)
									{
										ptr++;
										num2 = *ptr;
										num4++;
									}
									int width = num4 - k;
									Rectangle rectangle = new Rectangle(k, j, width, 1);
									if (!array3[l])
									{
										array3[l] = true;
										array2[l] = rectangle;
									}
									else
									{
										array2[l] = Rectangle.Union(array2[l], rectangle);
									}
									k = num4;
									break;
								}
							}
						}
					}
				}
				finally
				{
					if (bitmapData != null)
					{
						image.UnlockBits(bitmapData);
					}
				}
				return array2;
			}

			// Token: 0x06000125 RID: 293 RVA: 0x00005E64 File Offset: 0x00004064
			public void Init(Rectangle bounds)
			{
				this.bounds = bounds;
			}

			// Token: 0x04000068 RID: 104
			private Rectangle bounds;
		}
	}
}
