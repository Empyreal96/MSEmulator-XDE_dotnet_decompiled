using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009D9 RID: 2521
	[GeneratedCode("sgen", "4.0")]
	internal sealed class AssociationAssociatedInstanceSerializer : XmlSerializer1
	{
		// Token: 0x06005D04 RID: 23812 RVA: 0x001FCEDC File Offset: 0x001FB0DC
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("AssociationAssociatedInstance", "");
		}

		// Token: 0x06005D05 RID: 23813 RVA: 0x001FCEEE File Offset: 0x001FB0EE
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write57_AssociationAssociatedInstance(objectToSerialize);
		}

		// Token: 0x06005D06 RID: 23814 RVA: 0x001FCEFC File Offset: 0x001FB0FC
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read57_AssociationAssociatedInstance();
		}
	}
}
