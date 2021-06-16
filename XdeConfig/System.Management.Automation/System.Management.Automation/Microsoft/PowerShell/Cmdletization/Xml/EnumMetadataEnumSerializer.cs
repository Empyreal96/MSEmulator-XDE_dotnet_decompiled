using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009F6 RID: 2550
	[GeneratedCode("sgen", "4.0")]
	internal sealed class EnumMetadataEnumSerializer : XmlSerializer1
	{
		// Token: 0x06005D78 RID: 23928 RVA: 0x001FD4DD File Offset: 0x001FB6DD
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("EnumMetadataEnum", "");
		}

		// Token: 0x06005D79 RID: 23929 RVA: 0x001FD4EF File Offset: 0x001FB6EF
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write86_EnumMetadataEnum(objectToSerialize);
		}

		// Token: 0x06005D7A RID: 23930 RVA: 0x001FD4FD File Offset: 0x001FB6FD
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read86_EnumMetadataEnum();
		}
	}
}
