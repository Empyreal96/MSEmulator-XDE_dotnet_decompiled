using System;
using System.IO;
using System.Xml.Serialization;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000014 RID: 20
	public class SkinDisplay
	{
		// Token: 0x060000EE RID: 238 RVA: 0x00003734 File Offset: 0x00001934
		public SkinDisplay()
		{
			this.DisplayCount = 1;
			this.DisplayGapWidth = 0;
			this.DisplayScaleFactor = 0;
			this.DisplaysPerChrome = 1;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000EF RID: 239 RVA: 0x0000375F File Offset: 0x0000195F
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x00003767 File Offset: 0x00001967
		public SkinAutoSize AutoSize { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00003770 File Offset: 0x00001970
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x00003778 File Offset: 0x00001978
		[XmlAttribute]
		public int VideoMode { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00003781 File Offset: 0x00001981
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x00003789 File Offset: 0x00001989
		[XmlAttribute]
		public string Alias { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00003792 File Offset: 0x00001992
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x0000379A File Offset: 0x0000199A
		[XmlAttribute]
		public string DefaultDiagonalSize { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x000037A3 File Offset: 0x000019A3
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x000037AB File Offset: 0x000019AB
		[XmlAttribute]
		public int VRAMforVGPU { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x000037B4 File Offset: 0x000019B4
		// (set) Token: 0x060000FA RID: 250 RVA: 0x000037BC File Offset: 0x000019BC
		[XmlAttribute]
		public int DisplayPosX { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000FB RID: 251 RVA: 0x000037C5 File Offset: 0x000019C5
		// (set) Token: 0x060000FC RID: 252 RVA: 0x000037CD File Offset: 0x000019CD
		[XmlAttribute]
		public int DisplayPosY { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000FD RID: 253 RVA: 0x000037D6 File Offset: 0x000019D6
		// (set) Token: 0x060000FE RID: 254 RVA: 0x000037DE File Offset: 0x000019DE
		[XmlAttribute]
		public int DisplayWidth { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000FF RID: 255 RVA: 0x000037E7 File Offset: 0x000019E7
		// (set) Token: 0x06000100 RID: 256 RVA: 0x000037EF File Offset: 0x000019EF
		[XmlAttribute]
		public int DisplayHeight { get; set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000101 RID: 257 RVA: 0x000037F8 File Offset: 0x000019F8
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00003800 File Offset: 0x00001A00
		[XmlAttribute]
		public int ExternalDisplayWidth { get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00003809 File Offset: 0x00001A09
		// (set) Token: 0x06000104 RID: 260 RVA: 0x00003811 File Offset: 0x00001A11
		[XmlAttribute]
		public int ExternalDisplayHeight { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000105 RID: 261 RVA: 0x0000381A File Offset: 0x00001A1A
		// (set) Token: 0x06000106 RID: 262 RVA: 0x00003822 File Offset: 0x00001A22
		[XmlAttribute]
		public int DisplayCount { get; set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000107 RID: 263 RVA: 0x0000382B File Offset: 0x00001A2B
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00003843 File Offset: 0x00001A43
		[XmlAttribute]
		public int InitialActiveChromeCount
		{
			get
			{
				if (this.initialActiveChromeCountOverride != -1)
				{
					return this.initialActiveChromeCountOverride;
				}
				return this.ChromeCount;
			}
			set
			{
				this.initialActiveChromeCountOverride = value;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000109 RID: 265 RVA: 0x0000384C File Offset: 0x00001A4C
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00003854 File Offset: 0x00001A54
		[XmlAttribute]
		public int DisplaysPerChrome { get; set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600010B RID: 267 RVA: 0x0000385D File Offset: 0x00001A5D
		public int ChromeCount
		{
			get
			{
				return this.DisplayCount / this.DisplaysPerChrome;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600010C RID: 268 RVA: 0x0000386C File Offset: 0x00001A6C
		// (set) Token: 0x0600010D RID: 269 RVA: 0x00003874 File Offset: 0x00001A74
		[XmlAttribute]
		public int DisplayGapWidth { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600010E RID: 270 RVA: 0x0000387D File Offset: 0x00001A7D
		// (set) Token: 0x0600010F RID: 271 RVA: 0x00003885 File Offset: 0x00001A85
		[XmlAttribute]
		public int DisplayScaleFactor { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000110 RID: 272 RVA: 0x0000388E File Offset: 0x00001A8E
		// (set) Token: 0x06000111 RID: 273 RVA: 0x00003896 File Offset: 0x00001A96
		[XmlAttribute]
		public bool DisplaysStackedVertically { get; set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000112 RID: 274 RVA: 0x0000389F File Offset: 0x00001A9F
		[XmlIgnore]
		public bool SupportsExternalDisplay
		{
			get
			{
				return this.ExternalDisplayHeight != 0 && this.ExternalDisplayWidth != 0;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000113 RID: 275 RVA: 0x000038B4 File Offset: 0x00001AB4
		[XmlIgnore]
		public int TotalDisplayWidth
		{
			get
			{
				return this.DisplayCount * this.DisplayWidth + this.ExternalDisplayWidth;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000114 RID: 276 RVA: 0x000038CA File Offset: 0x00001ACA
		// (set) Token: 0x06000115 RID: 277 RVA: 0x000038D2 File Offset: 0x00001AD2
		[XmlAttribute]
		public int QuickAccessToolbarXPos { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000116 RID: 278 RVA: 0x000038DB File Offset: 0x00001ADB
		// (set) Token: 0x06000117 RID: 279 RVA: 0x000038E3 File Offset: 0x00001AE3
		[XmlAttribute]
		public int QuickAccessToolbarYPos { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000118 RID: 280 RVA: 0x000038EC File Offset: 0x00001AEC
		// (set) Token: 0x06000119 RID: 281 RVA: 0x000038F4 File Offset: 0x00001AF4
		[XmlAttribute]
		public string MappingImage { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600011A RID: 282 RVA: 0x000038FD File Offset: 0x00001AFD
		// (set) Token: 0x0600011B RID: 283 RVA: 0x00003905 File Offset: 0x00001B05
		[XmlAttribute]
		public string NormalImage { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600011C RID: 284 RVA: 0x0000390E File Offset: 0x00001B0E
		// (set) Token: 0x0600011D RID: 285 RVA: 0x00003916 File Offset: 0x00001B16
		[XmlAttribute]
		public string DownImage { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600011E RID: 286 RVA: 0x0000391F File Offset: 0x00001B1F
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00003927 File Offset: 0x00001B27
		public SkinButton[] Buttons { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00003930 File Offset: 0x00001B30
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00003938 File Offset: 0x00001B38
		[XmlAttribute]
		public XdeSensors InvalidSensors { get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00003941 File Offset: 0x00001B41
		// (set) Token: 0x06000123 RID: 291 RVA: 0x00003949 File Offset: 0x00001B49
		[XmlAttribute]
		public XdeSensors RequiredSensors { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00003952 File Offset: 0x00001B52
		[XmlIgnore]
		public XdeSensors ValidSensors
		{
			get
			{
				return XdeSensors.All & ~this.InvalidSensors;
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00003964 File Offset: 0x00001B64
		public static SkinDisplay LoadSkinDisplay(string skinFileName)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(SkinDisplay.Skin));
			SkinDisplay display;
			using (StreamReader streamReader = new StreamReader(skinFileName))
			{
				display = ((SkinDisplay.Skin)xmlSerializer.Deserialize(streamReader)).Display;
			}
			return display;
		}

		// Token: 0x0400008E RID: 142
		private const int DefaultDisplayCount = 1;

		// Token: 0x0400008F RID: 143
		private const int DefaultDisplayGap = 0;

		// Token: 0x04000090 RID: 144
		private const int DefaultDisplayScaleFactor = 0;

		// Token: 0x04000091 RID: 145
		private const int DefaultDisplaysChrome = 1;

		// Token: 0x04000092 RID: 146
		private int initialActiveChromeCountOverride = -1;

		// Token: 0x02000072 RID: 114
		public class Skin
		{
			// Token: 0x1700008D RID: 141
			// (get) Token: 0x06000212 RID: 530 RVA: 0x00005147 File Offset: 0x00003347
			// (set) Token: 0x06000213 RID: 531 RVA: 0x0000514F File Offset: 0x0000334F
			public SkinDisplay Display { get; set; }
		}
	}
}
