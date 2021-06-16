using System;
using System.Xml.Serialization;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000017 RID: 23
	public class SkuRegKey
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000126 RID: 294 RVA: 0x000039B8 File Offset: 0x00001BB8
		// (set) Token: 0x06000127 RID: 295 RVA: 0x000039C0 File Offset: 0x00001BC0
		[XmlAttribute]
		public SkuRegRoots Root { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000128 RID: 296 RVA: 0x000039C9 File Offset: 0x00001BC9
		// (set) Token: 0x06000129 RID: 297 RVA: 0x000039D1 File Offset: 0x00001BD1
		[XmlAttribute]
		public string Path { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600012A RID: 298 RVA: 0x000039DA File Offset: 0x00001BDA
		// (set) Token: 0x0600012B RID: 299 RVA: 0x000039E2 File Offset: 0x00001BE2
		[XmlElement(ElementName = "Value")]
		public SkuRegValue[] Values { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600012C RID: 300 RVA: 0x000039EB File Offset: 0x00001BEB
		// (set) Token: 0x0600012D RID: 301 RVA: 0x000039F3 File Offset: 0x00001BF3
		[XmlAttribute]
		public ContainerFlags ContainerFlags { get; set; } = ContainerFlags.Host;
	}
}
