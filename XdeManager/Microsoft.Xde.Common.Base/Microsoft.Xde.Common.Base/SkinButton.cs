using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000013 RID: 19
	[XmlType("Button")]
	public class SkinButton
	{
		// Token: 0x060000DC RID: 220 RVA: 0x000035CC File Offset: 0x000017CC
		public SkinButton()
		{
			this.SensorHideMask = XdeSensors.None;
			this.SensorShowMask = XdeSensors.Default;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000DD RID: 221 RVA: 0x000035E6 File Offset: 0x000017E6
		// (set) Token: 0x060000DE RID: 222 RVA: 0x000035EE File Offset: 0x000017EE
		[XmlAttribute]
		public SkinButtonType Type { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000DF RID: 223 RVA: 0x000035F7 File Offset: 0x000017F7
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x000035FF File Offset: 0x000017FF
		[XmlIgnore]
		public Keys[] KeyMapping { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00003608 File Offset: 0x00001808
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x0000361C File Offset: 0x0000181C
		[XmlAttribute(AttributeName = "KeyMapping")]
		public string KeyMappingStr
		{
			get
			{
				return string.Join<Keys>(",", this.KeyMapping);
			}
			set
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					string[] source = value.Split(new char[]
					{
						','
					}, StringSplitOptions.RemoveEmptyEntries);
					this.KeyMapping = (from k in source
					select (Keys)Enum.Parse(typeof(Keys), k)).ToArray<Keys>();
					return;
				}
				this.KeyMapping = new Keys[1];
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00003681 File Offset: 0x00001881
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00003689 File Offset: 0x00001889
		[XmlIgnore]
		public Color MappingColor { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00003692 File Offset: 0x00001892
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x0000369A File Offset: 0x0000189A
		[XmlAttribute]
		public SkinButtonAnchor Anchor { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x000036A4 File Offset: 0x000018A4
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x000036CC File Offset: 0x000018CC
		[XmlAttribute(AttributeName = "MappingColor")]
		public string MappingColorStr
		{
			get
			{
				return this.MappingColor.ToArgb().ToString("x");
			}
			set
			{
				this.MappingColor = Color.FromArgb(SkinButton.ParseHex(value));
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x000036DF File Offset: 0x000018DF
		// (set) Token: 0x060000EA RID: 234 RVA: 0x000036E7 File Offset: 0x000018E7
		[XmlAttribute]
		public XdeSensors SensorShowMask { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000EB RID: 235 RVA: 0x000036F0 File Offset: 0x000018F0
		// (set) Token: 0x060000EC RID: 236 RVA: 0x000036F8 File Offset: 0x000018F8
		[XmlAttribute]
		public XdeSensors SensorHideMask { get; set; }

		// Token: 0x060000ED RID: 237 RVA: 0x00003704 File Offset: 0x00001904
		private static int ParseHex(string value)
		{
			string text = value;
			if (text.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
			{
				text = text.Substring(2);
			}
			return int.Parse(text, NumberStyles.HexNumber);
		}
	}
}
