using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009F4 RID: 2548
	[GeneratedCode("sgen", "4.0")]
	internal sealed class ItemsChoiceTypeSerializer : XmlSerializer1
	{
		// Token: 0x06005D70 RID: 23920 RVA: 0x001FD473 File Offset: 0x001FB673
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("ItemsChoiceType", "");
		}

		// Token: 0x06005D71 RID: 23921 RVA: 0x001FD485 File Offset: 0x001FB685
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write84_ItemsChoiceType(objectToSerialize);
		}

		// Token: 0x06005D72 RID: 23922 RVA: 0x001FD493 File Offset: 0x001FB693
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read84_ItemsChoiceType();
		}
	}
}
