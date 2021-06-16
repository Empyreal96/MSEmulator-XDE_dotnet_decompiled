using System;
using System.Xml.Serialization;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000018 RID: 24
	public class SkuRegValue
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00003A0B File Offset: 0x00001C0B
		// (set) Token: 0x06000130 RID: 304 RVA: 0x00003A13 File Offset: 0x00001C13
		[XmlAttribute]
		public string Name { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00003A1C File Offset: 0x00001C1C
		// (set) Token: 0x06000132 RID: 306 RVA: 0x00003A24 File Offset: 0x00001C24
		[XmlAttribute]
		public string Data { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00003A2D File Offset: 0x00001C2D
		// (set) Token: 0x06000134 RID: 308 RVA: 0x00003A35 File Offset: 0x00001C35
		[XmlAttribute]
		public RegValueType Type { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00003A3E File Offset: 0x00001C3E
		// (set) Token: 0x06000136 RID: 310 RVA: 0x00003A46 File Offset: 0x00001C46
		[XmlAttribute]
		public string OverrideFromKey { get; set; }
	}
}
