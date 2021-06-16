using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009E7 RID: 2535
	[GeneratedCode("sgen", "4.0")]
	internal sealed class StaticCmdletMetadataSerializer : XmlSerializer1
	{
		// Token: 0x06005D3C RID: 23868 RVA: 0x001FD1C2 File Offset: 0x001FB3C2
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("StaticCmdletMetadata", "");
		}

		// Token: 0x06005D3D RID: 23869 RVA: 0x001FD1D4 File Offset: 0x001FB3D4
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write71_StaticCmdletMetadata(objectToSerialize);
		}

		// Token: 0x06005D3E RID: 23870 RVA: 0x001FD1E2 File Offset: 0x001FB3E2
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read71_StaticCmdletMetadata();
		}
	}
}
