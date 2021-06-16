using System;
using System.Xml.Serialization;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200001A RID: 26
	public class FeatureData
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00003A60 File Offset: 0x00001C60
		// (set) Token: 0x0600013C RID: 316 RVA: 0x00003A68 File Offset: 0x00001C68
		[XmlAttribute]
		public string Name { get; set; }
	}
}
