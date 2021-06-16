using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009E4 RID: 2532
	[GeneratedCode("sgen", "4.0")]
	internal sealed class GetCmdletMetadataSerializer : XmlSerializer1
	{
		// Token: 0x06005D30 RID: 23856 RVA: 0x001FD123 File Offset: 0x001FB323
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("GetCmdletMetadata", "");
		}

		// Token: 0x06005D31 RID: 23857 RVA: 0x001FD135 File Offset: 0x001FB335
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write68_GetCmdletMetadata(objectToSerialize);
		}

		// Token: 0x06005D32 RID: 23858 RVA: 0x001FD143 File Offset: 0x001FB343
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read68_GetCmdletMetadata();
		}
	}
}
