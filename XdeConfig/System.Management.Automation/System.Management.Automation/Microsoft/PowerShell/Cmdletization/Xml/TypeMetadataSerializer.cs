using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009D7 RID: 2519
	[GeneratedCode("sgen", "4.0")]
	internal sealed class TypeMetadataSerializer : XmlSerializer1
	{
		// Token: 0x06005CFC RID: 23804 RVA: 0x001FCE72 File Offset: 0x001FB072
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("TypeMetadata", "");
		}

		// Token: 0x06005CFD RID: 23805 RVA: 0x001FCE84 File Offset: 0x001FB084
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write55_TypeMetadata(objectToSerialize);
		}

		// Token: 0x06005CFE RID: 23806 RVA: 0x001FCE92 File Offset: 0x001FB092
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read55_TypeMetadata();
		}
	}
}
