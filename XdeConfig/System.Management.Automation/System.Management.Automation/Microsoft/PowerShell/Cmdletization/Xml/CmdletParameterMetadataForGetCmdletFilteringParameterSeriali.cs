using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009DC RID: 2524
	[GeneratedCode("sgen", "4.0")]
	internal sealed class CmdletParameterMetadataForGetCmdletFilteringParameterSerializer : XmlSerializer1
	{
		// Token: 0x06005D10 RID: 23824 RVA: 0x001FCF7B File Offset: 0x001FB17B
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("CmdletParameterMetadataForGetCmdletFilteringParameter", "");
		}

		// Token: 0x06005D11 RID: 23825 RVA: 0x001FCF8D File Offset: 0x001FB18D
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write60_Item(objectToSerialize);
		}

		// Token: 0x06005D12 RID: 23826 RVA: 0x001FCF9B File Offset: 0x001FB19B
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read60_Item();
		}
	}
}
