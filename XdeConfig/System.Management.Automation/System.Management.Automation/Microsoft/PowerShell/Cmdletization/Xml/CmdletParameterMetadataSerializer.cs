using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009DA RID: 2522
	[GeneratedCode("sgen", "4.0")]
	internal sealed class CmdletParameterMetadataSerializer : XmlSerializer1
	{
		// Token: 0x06005D08 RID: 23816 RVA: 0x001FCF11 File Offset: 0x001FB111
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("CmdletParameterMetadata", "");
		}

		// Token: 0x06005D09 RID: 23817 RVA: 0x001FCF23 File Offset: 0x001FB123
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write58_CmdletParameterMetadata(objectToSerialize);
		}

		// Token: 0x06005D0A RID: 23818 RVA: 0x001FCF31 File Offset: 0x001FB131
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read58_CmdletParameterMetadata();
		}
	}
}
