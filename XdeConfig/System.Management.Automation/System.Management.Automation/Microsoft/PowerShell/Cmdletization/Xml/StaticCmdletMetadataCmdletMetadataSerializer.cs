using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009E8 RID: 2536
	[GeneratedCode("sgen", "4.0")]
	internal sealed class StaticCmdletMetadataCmdletMetadataSerializer : XmlSerializer1
	{
		// Token: 0x06005D40 RID: 23872 RVA: 0x001FD1F7 File Offset: 0x001FB3F7
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("StaticCmdletMetadataCmdletMetadata", "");
		}

		// Token: 0x06005D41 RID: 23873 RVA: 0x001FD209 File Offset: 0x001FB409
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write72_Item(objectToSerialize);
		}

		// Token: 0x06005D42 RID: 23874 RVA: 0x001FD217 File Offset: 0x001FB417
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read72_Item();
		}
	}
}
