using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009EF RID: 2543
	[GeneratedCode("sgen", "4.0")]
	internal sealed class CommonMethodMetadataReturnValueSerializer : XmlSerializer1
	{
		// Token: 0x06005D5C RID: 23900 RVA: 0x001FD36A File Offset: 0x001FB56A
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("CommonMethodMetadataReturnValue", "");
		}

		// Token: 0x06005D5D RID: 23901 RVA: 0x001FD37C File Offset: 0x001FB57C
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write79_Item(objectToSerialize);
		}

		// Token: 0x06005D5E RID: 23902 RVA: 0x001FD38A File Offset: 0x001FB58A
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read79_Item();
		}
	}
}
