using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009E3 RID: 2531
	[GeneratedCode("sgen", "4.0")]
	internal sealed class QueryOptionSerializer : XmlSerializer1
	{
		// Token: 0x06005D2C RID: 23852 RVA: 0x001FD0EE File Offset: 0x001FB2EE
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("QueryOption", "");
		}

		// Token: 0x06005D2D RID: 23853 RVA: 0x001FD100 File Offset: 0x001FB300
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write67_QueryOption(objectToSerialize);
		}

		// Token: 0x06005D2E RID: 23854 RVA: 0x001FD10E File Offset: 0x001FB30E
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read67_QueryOption();
		}
	}
}
