using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009D8 RID: 2520
	[GeneratedCode("sgen", "4.0")]
	internal sealed class AssociationSerializer : XmlSerializer1
	{
		// Token: 0x06005D00 RID: 23808 RVA: 0x001FCEA7 File Offset: 0x001FB0A7
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("Association", "");
		}

		// Token: 0x06005D01 RID: 23809 RVA: 0x001FCEB9 File Offset: 0x001FB0B9
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write56_Association(objectToSerialize);
		}

		// Token: 0x06005D02 RID: 23810 RVA: 0x001FCEC7 File Offset: 0x001FB0C7
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read56_Association();
		}
	}
}
