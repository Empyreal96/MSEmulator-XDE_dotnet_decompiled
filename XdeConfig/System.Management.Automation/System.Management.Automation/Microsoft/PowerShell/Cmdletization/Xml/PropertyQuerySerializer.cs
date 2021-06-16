using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009F2 RID: 2546
	[GeneratedCode("sgen", "4.0")]
	internal sealed class PropertyQuerySerializer : XmlSerializer1
	{
		// Token: 0x06005D68 RID: 23912 RVA: 0x001FD409 File Offset: 0x001FB609
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("PropertyQuery", "");
		}

		// Token: 0x06005D69 RID: 23913 RVA: 0x001FD41B File Offset: 0x001FB61B
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write82_PropertyQuery(objectToSerialize);
		}

		// Token: 0x06005D6A RID: 23914 RVA: 0x001FD429 File Offset: 0x001FB629
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read82_PropertyQuery();
		}
	}
}
