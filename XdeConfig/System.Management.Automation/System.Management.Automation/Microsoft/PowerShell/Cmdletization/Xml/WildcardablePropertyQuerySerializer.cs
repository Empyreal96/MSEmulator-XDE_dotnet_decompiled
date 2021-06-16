using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009F3 RID: 2547
	[GeneratedCode("sgen", "4.0")]
	internal sealed class WildcardablePropertyQuerySerializer : XmlSerializer1
	{
		// Token: 0x06005D6C RID: 23916 RVA: 0x001FD43E File Offset: 0x001FB63E
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("WildcardablePropertyQuery", "");
		}

		// Token: 0x06005D6D RID: 23917 RVA: 0x001FD450 File Offset: 0x001FB650
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write83_WildcardablePropertyQuery(objectToSerialize);
		}

		// Token: 0x06005D6E RID: 23918 RVA: 0x001FD45E File Offset: 0x001FB65E
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read83_WildcardablePropertyQuery();
		}
	}
}
