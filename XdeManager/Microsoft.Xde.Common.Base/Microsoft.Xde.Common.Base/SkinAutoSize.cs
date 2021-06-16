using System;
using System.Drawing;
using System.Xml.Serialization;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000010 RID: 16
	public class SkinAutoSize
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x0000338C File Offset: 0x0000158C
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x00003399 File Offset: 0x00001599
		[XmlAttribute(AttributeName = "TopLeft")]
		public string TopLeftStr
		{
			get
			{
				return SkinAutoSize.StringFromRect(this.TopLeft);
			}
			set
			{
				this.TopLeft = SkinAutoSize.RectFromString(value);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000AA RID: 170 RVA: 0x000033A7 File Offset: 0x000015A7
		// (set) Token: 0x060000AB RID: 171 RVA: 0x000033AF File Offset: 0x000015AF
		[XmlIgnore]
		public Rectangle TopLeft { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000AC RID: 172 RVA: 0x000033B8 File Offset: 0x000015B8
		// (set) Token: 0x060000AD RID: 173 RVA: 0x000033C5 File Offset: 0x000015C5
		[XmlAttribute(AttributeName = "TopFill")]
		public string TopFillStr
		{
			get
			{
				return SkinAutoSize.StringFromRect(this.TopFill);
			}
			set
			{
				this.TopFill = SkinAutoSize.RectFromString(value);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000AE RID: 174 RVA: 0x000033D3 File Offset: 0x000015D3
		// (set) Token: 0x060000AF RID: 175 RVA: 0x000033DB File Offset: 0x000015DB
		[XmlIgnore]
		public Rectangle TopFill { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x000033E4 File Offset: 0x000015E4
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x000033F1 File Offset: 0x000015F1
		[XmlAttribute(AttributeName = "TopRight")]
		public string TopRightStr
		{
			get
			{
				return SkinAutoSize.StringFromRect(this.TopRight);
			}
			set
			{
				this.TopRight = SkinAutoSize.RectFromString(value);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x000033FF File Offset: 0x000015FF
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x00003407 File Offset: 0x00001607
		[XmlIgnore]
		public Rectangle TopRight { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00003410 File Offset: 0x00001610
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x0000341D File Offset: 0x0000161D
		[XmlAttribute(AttributeName = "MiddleLeft")]
		public string MiddleLeftStr
		{
			get
			{
				return SkinAutoSize.StringFromRect(this.MiddleLeft);
			}
			set
			{
				this.MiddleLeft = SkinAutoSize.RectFromString(value);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x0000342B File Offset: 0x0000162B
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x00003433 File Offset: 0x00001633
		[XmlIgnore]
		public Rectangle MiddleLeft { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x0000343C File Offset: 0x0000163C
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x00003449 File Offset: 0x00001649
		[XmlAttribute(AttributeName = "MiddleRight")]
		public string MiddleRightStr
		{
			get
			{
				return SkinAutoSize.StringFromRect(this.MiddleRight);
			}
			set
			{
				this.MiddleRight = SkinAutoSize.RectFromString(value);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00003457 File Offset: 0x00001657
		// (set) Token: 0x060000BB RID: 187 RVA: 0x0000345F File Offset: 0x0000165F
		[XmlIgnore]
		public Rectangle MiddleRight { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00003468 File Offset: 0x00001668
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00003475 File Offset: 0x00001675
		[XmlAttribute(AttributeName = "BottomLeft")]
		public string BottomLeftStr
		{
			get
			{
				return SkinAutoSize.StringFromRect(this.BottomLeft);
			}
			set
			{
				this.BottomLeft = SkinAutoSize.RectFromString(value);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00003483 File Offset: 0x00001683
		// (set) Token: 0x060000BF RID: 191 RVA: 0x0000348B File Offset: 0x0000168B
		[XmlIgnore]
		public Rectangle BottomLeft { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00003494 File Offset: 0x00001694
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x000034A1 File Offset: 0x000016A1
		[XmlAttribute(AttributeName = "BottomFill")]
		public string BottomFillStr
		{
			get
			{
				return SkinAutoSize.StringFromRect(this.BottomFill);
			}
			set
			{
				this.BottomFill = SkinAutoSize.RectFromString(value);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x000034AF File Offset: 0x000016AF
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x000034B7 File Offset: 0x000016B7
		[XmlIgnore]
		public Rectangle BottomFill { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x000034C0 File Offset: 0x000016C0
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x000034CD File Offset: 0x000016CD
		[XmlAttribute(AttributeName = "BottomRight")]
		public string BottomRightStr
		{
			get
			{
				return SkinAutoSize.StringFromRect(this.BottomRight);
			}
			set
			{
				this.BottomRight = SkinAutoSize.RectFromString(value);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x000034DB File Offset: 0x000016DB
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x000034E3 File Offset: 0x000016E3
		[XmlIgnore]
		public Rectangle BottomRight { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x000034EC File Offset: 0x000016EC
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x000034F9 File Offset: 0x000016F9
		[XmlAttribute(AttributeName = "Display1TopRight")]
		public string Display1TopRightStr
		{
			get
			{
				return SkinAutoSize.StringFromRect(this.Display1TopRight);
			}
			set
			{
				this.Display1TopRight = SkinAutoSize.RectFromString(value);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00003507 File Offset: 0x00001707
		// (set) Token: 0x060000CB RID: 203 RVA: 0x0000350F File Offset: 0x0000170F
		[XmlIgnore]
		public Rectangle Display1TopRight { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00003518 File Offset: 0x00001718
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00003525 File Offset: 0x00001725
		[XmlAttribute(AttributeName = "Display1BottomRight")]
		public string Display1BottomRightStr
		{
			get
			{
				return SkinAutoSize.StringFromRect(this.Display1BottomRight);
			}
			set
			{
				this.Display1BottomRight = SkinAutoSize.RectFromString(value);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00003533 File Offset: 0x00001733
		// (set) Token: 0x060000CF RID: 207 RVA: 0x0000353B File Offset: 0x0000173B
		[XmlIgnore]
		public Rectangle Display1BottomRight { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00003544 File Offset: 0x00001744
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x00003551 File Offset: 0x00001751
		[XmlAttribute(AttributeName = "Display2TopLeft")]
		public string Display2TopLeftStr
		{
			get
			{
				return SkinAutoSize.StringFromRect(this.Display2TopLeft);
			}
			set
			{
				this.Display2TopLeft = SkinAutoSize.RectFromString(value);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x0000355F File Offset: 0x0000175F
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00003567 File Offset: 0x00001767
		[XmlIgnore]
		public Rectangle Display2TopLeft { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00003570 File Offset: 0x00001770
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x0000357D File Offset: 0x0000177D
		[XmlAttribute(AttributeName = "Display2BottomLeft")]
		public string Display2BottomLeftStr
		{
			get
			{
				return SkinAutoSize.StringFromRect(this.Display2BottomLeft);
			}
			set
			{
				this.Display2BottomLeft = SkinAutoSize.RectFromString(value);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x0000358B File Offset: 0x0000178B
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x00003593 File Offset: 0x00001793
		[XmlIgnore]
		public Rectangle Display2BottomLeft { get; set; }

		// Token: 0x060000D8 RID: 216 RVA: 0x0000359C File Offset: 0x0000179C
		private static Rectangle RectFromString(string value)
		{
			return (Rectangle)SkinAutoSize.recConv.ConvertFromInvariantString(value);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000035AE File Offset: 0x000017AE
		private static string StringFromRect(Rectangle rect)
		{
			return SkinAutoSize.recConv.ConvertToInvariantString(rect);
		}

		// Token: 0x0400006C RID: 108
		private static RectangleConverter recConv = new RectangleConverter();
	}
}
