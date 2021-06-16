using System;
using System.Xml.Serialization;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000020 RID: 32
	public class TabData
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00003CF7 File Offset: 0x00001EF7
		// (set) Token: 0x06000172 RID: 370 RVA: 0x00003CFF File Offset: 0x00001EFF
		[XmlAttribute]
		public string Name { get; set; }
	}
}
