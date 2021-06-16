using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200006C RID: 108
	public class RegKeys
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600026F RID: 623 RVA: 0x00005618 File Offset: 0x00003818
		// (set) Token: 0x06000270 RID: 624 RVA: 0x00005620 File Offset: 0x00003820
		[XmlAttribute]
		public string Language { get; set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000271 RID: 625 RVA: 0x00005629 File Offset: 0x00003829
		[XmlElement(ElementName = "RegKey")]
		public List<RegKey> Keys
		{
			get
			{
				return this.keys;
			}
		}

		// Token: 0x04000185 RID: 389
		private List<RegKey> keys = new List<RegKey>();
	}
}
