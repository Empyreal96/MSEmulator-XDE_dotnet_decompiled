using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009EA RID: 2538
	[GeneratedCode("sgen", "4.0")]
	internal sealed class StaticMethodMetadataSerializer : XmlSerializer1
	{
		// Token: 0x06005D48 RID: 23880 RVA: 0x001FD261 File Offset: 0x001FB461
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("StaticMethodMetadata", "");
		}

		// Token: 0x06005D49 RID: 23881 RVA: 0x001FD273 File Offset: 0x001FB473
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write74_StaticMethodMetadata(objectToSerialize);
		}

		// Token: 0x06005D4A RID: 23882 RVA: 0x001FD281 File Offset: 0x001FB481
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read74_StaticMethodMetadata();
		}
	}
}
