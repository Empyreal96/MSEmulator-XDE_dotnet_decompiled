using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009EC RID: 2540
	[GeneratedCode("sgen", "4.0")]
	internal sealed class StaticMethodParameterMetadataSerializer : XmlSerializer1
	{
		// Token: 0x06005D50 RID: 23888 RVA: 0x001FD2CB File Offset: 0x001FB4CB
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("StaticMethodParameterMetadata", "");
		}

		// Token: 0x06005D51 RID: 23889 RVA: 0x001FD2DD File Offset: 0x001FB4DD
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write76_StaticMethodParameterMetadata(objectToSerialize);
		}

		// Token: 0x06005D52 RID: 23890 RVA: 0x001FD2EB File Offset: 0x001FB4EB
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read76_StaticMethodParameterMetadata();
		}
	}
}
