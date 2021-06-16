using System;
using System.IO;
using System.Xml.Serialization;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200001B RID: 27
	[XmlRoot(ElementName = "XdeSku")]
	public class SkuData
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00003A71 File Offset: 0x00001C71
		// (set) Token: 0x0600013F RID: 319 RVA: 0x00003A79 File Offset: 0x00001C79
		[XmlAttribute]
		public string BrandingName { get; set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00003A82 File Offset: 0x00001C82
		// (set) Token: 0x06000141 RID: 321 RVA: 0x00003A8A File Offset: 0x00001C8A
		public SkuOptions Options { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00003A93 File Offset: 0x00001C93
		// (set) Token: 0x06000143 RID: 323 RVA: 0x00003A9B File Offset: 0x00001C9B
		[XmlArrayItem(ElementName = "Key")]
		public SkuRegKey[] Registry { get; set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00003AA4 File Offset: 0x00001CA4
		// (set) Token: 0x06000145 RID: 325 RVA: 0x00003AAC File Offset: 0x00001CAC
		[XmlArrayItem(ElementName = "Feature")]
		public FeatureData[] RequiredFeatures { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00003AB5 File Offset: 0x00001CB5
		// (set) Token: 0x06000147 RID: 327 RVA: 0x00003ABD File Offset: 0x00001CBD
		[XmlArrayItem(ElementName = "Tab")]
		public TabData[] RequiredTabs { get; set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00003AC6 File Offset: 0x00001CC6
		// (set) Token: 0x06000149 RID: 329 RVA: 0x00003ACE File Offset: 0x00001CCE
		[XmlArrayItem(ElementName = "Button")]
		public ButtonData[] Toolbar { get; set; }

		// Token: 0x0600014A RID: 330 RVA: 0x00003AD8 File Offset: 0x00001CD8
		public static SkuData LoadSkuInformation(string skuFileName)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(SkuData));
			SkuData result;
			using (StreamReader streamReader = new StreamReader(skuFileName))
			{
				result = (SkuData)xmlSerializer.Deserialize(streamReader);
			}
			return result;
		}
	}
}
