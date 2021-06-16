using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009F5 RID: 2549
	[GeneratedCode("sgen", "4.0")]
	internal sealed class ClassMetadataDataSerializer : XmlSerializer1
	{
		// Token: 0x06005D74 RID: 23924 RVA: 0x001FD4A8 File Offset: 0x001FB6A8
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("ClassMetadataData", "");
		}

		// Token: 0x06005D75 RID: 23925 RVA: 0x001FD4BA File Offset: 0x001FB6BA
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write85_ClassMetadataData(objectToSerialize);
		}

		// Token: 0x06005D76 RID: 23926 RVA: 0x001FD4C8 File Offset: 0x001FB6C8
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read85_ClassMetadataData();
		}
	}
}
