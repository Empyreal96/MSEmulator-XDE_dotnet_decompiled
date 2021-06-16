using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009D6 RID: 2518
	[GeneratedCode("sgen", "4.0")]
	internal sealed class PropertyMetadataSerializer : XmlSerializer1
	{
		// Token: 0x06005CF8 RID: 23800 RVA: 0x001FCE3D File Offset: 0x001FB03D
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("PropertyMetadata", "");
		}

		// Token: 0x06005CF9 RID: 23801 RVA: 0x001FCE4F File Offset: 0x001FB04F
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write54_PropertyMetadata(objectToSerialize);
		}

		// Token: 0x06005CFA RID: 23802 RVA: 0x001FCE5D File Offset: 0x001FB05D
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read54_PropertyMetadata();
		}
	}
}
