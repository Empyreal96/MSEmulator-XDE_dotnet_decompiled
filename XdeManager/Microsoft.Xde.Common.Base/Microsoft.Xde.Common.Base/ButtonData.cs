using System;
using System.Xml.Serialization;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000019 RID: 25
	public class ButtonData
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00003A4F File Offset: 0x00001C4F
		// (set) Token: 0x06000139 RID: 313 RVA: 0x00003A57 File Offset: 0x00001C57
		[XmlAttribute]
		public string Name { get; set; }
	}
}
