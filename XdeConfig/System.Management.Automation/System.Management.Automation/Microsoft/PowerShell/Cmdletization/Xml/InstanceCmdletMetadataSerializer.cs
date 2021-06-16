using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009F1 RID: 2545
	[GeneratedCode("sgen", "4.0")]
	internal sealed class InstanceCmdletMetadataSerializer : XmlSerializer1
	{
		// Token: 0x06005D64 RID: 23908 RVA: 0x001FD3D4 File Offset: 0x001FB5D4
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("InstanceCmdletMetadata", "");
		}

		// Token: 0x06005D65 RID: 23909 RVA: 0x001FD3E6 File Offset: 0x001FB5E6
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write81_InstanceCmdletMetadata(objectToSerialize);
		}

		// Token: 0x06005D66 RID: 23910 RVA: 0x001FD3F4 File Offset: 0x001FB5F4
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read81_InstanceCmdletMetadata();
		}
	}
}
