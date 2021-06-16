using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200006D RID: 109
	public class RegValue
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000273 RID: 627 RVA: 0x00005644 File Offset: 0x00003844
		// (set) Token: 0x06000274 RID: 628 RVA: 0x0000564C File Offset: 0x0000384C
		[XmlAttribute]
		public string Name { get; set; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000275 RID: 629 RVA: 0x00005655 File Offset: 0x00003855
		// (set) Token: 0x06000276 RID: 630 RVA: 0x0000565D File Offset: 0x0000385D
		[XmlAttribute]
		public string Value { get; set; }

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000277 RID: 631 RVA: 0x00005666 File Offset: 0x00003866
		// (set) Token: 0x06000278 RID: 632 RVA: 0x0000566E File Offset: 0x0000386E
		[XmlAttribute]
		public RegValueType Type { get; set; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000279 RID: 633 RVA: 0x00005677 File Offset: 0x00003877
		[XmlIgnore]
		public string RegValueName
		{
			get
			{
				if (!(this.Name == "@"))
				{
					return this.Name;
				}
				return null;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600027A RID: 634 RVA: 0x00005694 File Offset: 0x00003894
		[XmlIgnore]
		public object TypedValue
		{
			get
			{
				switch (this.Type)
				{
				case RegValueType.REG_SZ:
					return this.Value;
				case RegValueType.REG_DWORD:
					return int.Parse(this.Value, NumberStyles.AllowHexSpecifier);
				case RegValueType.REG_MULTI_SZ:
					return new string[]
					{
						this.Value
					};
				case RegValueType.REG_BINARY:
				{
					string[] array = this.Value.Split(new char[]
					{
						','
					});
					List<byte> list = new List<byte>();
					foreach (string s in array)
					{
						list.Add(byte.Parse(s, NumberStyles.HexNumber));
					}
					return list.ToArray();
				}
				default:
					return null;
				}
			}
		}
	}
}
