using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009ED RID: 2541
	[GeneratedCode("sgen", "4.0")]
	internal sealed class CmdletOutputMetadataSerializer : XmlSerializer1
	{
		// Token: 0x06005D54 RID: 23892 RVA: 0x001FD300 File Offset: 0x001FB500
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("CmdletOutputMetadata", "");
		}

		// Token: 0x06005D55 RID: 23893 RVA: 0x001FD312 File Offset: 0x001FB512
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write77_CmdletOutputMetadata(objectToSerialize);
		}

		// Token: 0x06005D56 RID: 23894 RVA: 0x001FD320 File Offset: 0x001FB520
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read77_CmdletOutputMetadata();
		}
	}
}
