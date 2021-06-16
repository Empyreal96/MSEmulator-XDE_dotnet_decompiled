using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009E0 RID: 2528
	[GeneratedCode("sgen", "4.0")]
	internal sealed class ObsoleteAttributeMetadataSerializer : XmlSerializer1
	{
		// Token: 0x06005D20 RID: 23840 RVA: 0x001FD04F File Offset: 0x001FB24F
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("ObsoleteAttributeMetadata", "");
		}

		// Token: 0x06005D21 RID: 23841 RVA: 0x001FD061 File Offset: 0x001FB261
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write64_ObsoleteAttributeMetadata(objectToSerialize);
		}

		// Token: 0x06005D22 RID: 23842 RVA: 0x001FD06F File Offset: 0x001FB26F
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read64_ObsoleteAttributeMetadata();
		}
	}
}
