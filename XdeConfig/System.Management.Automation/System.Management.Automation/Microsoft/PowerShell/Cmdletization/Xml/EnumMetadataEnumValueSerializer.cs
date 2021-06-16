using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009F7 RID: 2551
	[GeneratedCode("sgen", "4.0")]
	internal sealed class EnumMetadataEnumValueSerializer : XmlSerializer1
	{
		// Token: 0x06005D7C RID: 23932 RVA: 0x001FD512 File Offset: 0x001FB712
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("EnumMetadataEnumValue", "");
		}

		// Token: 0x06005D7D RID: 23933 RVA: 0x001FD524 File Offset: 0x001FB724
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write87_EnumMetadataEnumValue(objectToSerialize);
		}

		// Token: 0x06005D7E RID: 23934 RVA: 0x001FD532 File Offset: 0x001FB732
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read87_EnumMetadataEnumValue();
		}
	}
}
