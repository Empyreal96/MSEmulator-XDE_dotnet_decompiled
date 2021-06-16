using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009EB RID: 2539
	[GeneratedCode("sgen", "4.0")]
	internal sealed class CommonMethodParameterMetadataSerializer : XmlSerializer1
	{
		// Token: 0x06005D4C RID: 23884 RVA: 0x001FD296 File Offset: 0x001FB496
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("CommonMethodParameterMetadata", "");
		}

		// Token: 0x06005D4D RID: 23885 RVA: 0x001FD2A8 File Offset: 0x001FB4A8
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write75_CommonMethodParameterMetadata(objectToSerialize);
		}

		// Token: 0x06005D4E RID: 23886 RVA: 0x001FD2B6 File Offset: 0x001FB4B6
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read75_CommonMethodParameterMetadata();
		}
	}
}
