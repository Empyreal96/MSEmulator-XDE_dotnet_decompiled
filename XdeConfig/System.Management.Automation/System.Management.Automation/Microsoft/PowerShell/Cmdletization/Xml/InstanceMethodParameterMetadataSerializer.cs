using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009EE RID: 2542
	[GeneratedCode("sgen", "4.0")]
	internal sealed class InstanceMethodParameterMetadataSerializer : XmlSerializer1
	{
		// Token: 0x06005D58 RID: 23896 RVA: 0x001FD335 File Offset: 0x001FB535
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("InstanceMethodParameterMetadata", "");
		}

		// Token: 0x06005D59 RID: 23897 RVA: 0x001FD347 File Offset: 0x001FB547
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write78_Item(objectToSerialize);
		}

		// Token: 0x06005D5A RID: 23898 RVA: 0x001FD355 File Offset: 0x001FB555
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read78_Item();
		}
	}
}
