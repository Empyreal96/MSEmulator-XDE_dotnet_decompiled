using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009E5 RID: 2533
	[GeneratedCode("sgen", "4.0")]
	internal sealed class CommonCmdletMetadataSerializer : XmlSerializer1
	{
		// Token: 0x06005D34 RID: 23860 RVA: 0x001FD158 File Offset: 0x001FB358
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("CommonCmdletMetadata", "");
		}

		// Token: 0x06005D35 RID: 23861 RVA: 0x001FD16A File Offset: 0x001FB36A
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write69_CommonCmdletMetadata(objectToSerialize);
		}

		// Token: 0x06005D36 RID: 23862 RVA: 0x001FD178 File Offset: 0x001FB378
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read69_CommonCmdletMetadata();
		}
	}
}
