using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009E9 RID: 2537
	[GeneratedCode("sgen", "4.0")]
	internal sealed class CommonMethodMetadataSerializer : XmlSerializer1
	{
		// Token: 0x06005D44 RID: 23876 RVA: 0x001FD22C File Offset: 0x001FB42C
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("CommonMethodMetadata", "");
		}

		// Token: 0x06005D45 RID: 23877 RVA: 0x001FD23E File Offset: 0x001FB43E
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write73_CommonMethodMetadata(objectToSerialize);
		}

		// Token: 0x06005D46 RID: 23878 RVA: 0x001FD24C File Offset: 0x001FB44C
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read73_CommonMethodMetadata();
		}
	}
}
