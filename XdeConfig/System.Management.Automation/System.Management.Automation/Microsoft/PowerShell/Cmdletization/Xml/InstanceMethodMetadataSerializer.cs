using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009F0 RID: 2544
	[GeneratedCode("sgen", "4.0")]
	internal sealed class InstanceMethodMetadataSerializer : XmlSerializer1
	{
		// Token: 0x06005D60 RID: 23904 RVA: 0x001FD39F File Offset: 0x001FB59F
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("InstanceMethodMetadata", "");
		}

		// Token: 0x06005D61 RID: 23905 RVA: 0x001FD3B1 File Offset: 0x001FB5B1
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write80_InstanceMethodMetadata(objectToSerialize);
		}

		// Token: 0x06005D62 RID: 23906 RVA: 0x001FD3BF File Offset: 0x001FB5BF
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read80_InstanceMethodMetadata();
		}
	}
}
