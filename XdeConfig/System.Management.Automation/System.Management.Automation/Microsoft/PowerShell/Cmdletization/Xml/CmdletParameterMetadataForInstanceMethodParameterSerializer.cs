using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009E1 RID: 2529
	[GeneratedCode("sgen", "4.0")]
	internal sealed class CmdletParameterMetadataForInstanceMethodParameterSerializer : XmlSerializer1
	{
		// Token: 0x06005D24 RID: 23844 RVA: 0x001FD084 File Offset: 0x001FB284
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("CmdletParameterMetadataForInstanceMethodParameter", "");
		}

		// Token: 0x06005D25 RID: 23845 RVA: 0x001FD096 File Offset: 0x001FB296
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write65_Item(objectToSerialize);
		}

		// Token: 0x06005D26 RID: 23846 RVA: 0x001FD0A4 File Offset: 0x001FB2A4
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read65_Item();
		}
	}
}
