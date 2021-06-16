using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009D3 RID: 2515
	[GeneratedCode("sgen", "4.0")]
	internal sealed class ClassMetadataSerializer : XmlSerializer1
	{
		// Token: 0x06005CEC RID: 23788 RVA: 0x001FCD9E File Offset: 0x001FAF9E
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("ClassMetadata", "");
		}

		// Token: 0x06005CED RID: 23789 RVA: 0x001FCDB0 File Offset: 0x001FAFB0
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write51_ClassMetadata(objectToSerialize);
		}

		// Token: 0x06005CEE RID: 23790 RVA: 0x001FCDBE File Offset: 0x001FAFBE
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read51_ClassMetadata();
		}
	}
}
