using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009DD RID: 2525
	[GeneratedCode("sgen", "4.0")]
	internal sealed class CmdletParameterMetadataValidateCountSerializer : XmlSerializer1
	{
		// Token: 0x06005D14 RID: 23828 RVA: 0x001FCFB0 File Offset: 0x001FB1B0
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("CmdletParameterMetadataValidateCount", "");
		}

		// Token: 0x06005D15 RID: 23829 RVA: 0x001FCFC2 File Offset: 0x001FB1C2
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write61_Item(objectToSerialize);
		}

		// Token: 0x06005D16 RID: 23830 RVA: 0x001FCFD0 File Offset: 0x001FB1D0
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read61_Item();
		}
	}
}
