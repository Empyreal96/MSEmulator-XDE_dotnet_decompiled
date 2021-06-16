using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009DB RID: 2523
	[GeneratedCode("sgen", "4.0")]
	internal sealed class CmdletParameterMetadataForGetCmdletParameterSerializer : XmlSerializer1
	{
		// Token: 0x06005D0C RID: 23820 RVA: 0x001FCF46 File Offset: 0x001FB146
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("CmdletParameterMetadataForGetCmdletParameter", "");
		}

		// Token: 0x06005D0D RID: 23821 RVA: 0x001FCF58 File Offset: 0x001FB158
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write59_Item(objectToSerialize);
		}

		// Token: 0x06005D0E RID: 23822 RVA: 0x001FCF66 File Offset: 0x001FB166
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read59_Item();
		}
	}
}
