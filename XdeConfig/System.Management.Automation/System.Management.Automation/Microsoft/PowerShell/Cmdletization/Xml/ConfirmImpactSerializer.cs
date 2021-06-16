using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009E6 RID: 2534
	[GeneratedCode("sgen", "4.0")]
	internal sealed class ConfirmImpactSerializer : XmlSerializer1
	{
		// Token: 0x06005D38 RID: 23864 RVA: 0x001FD18D File Offset: 0x001FB38D
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("ConfirmImpact", "");
		}

		// Token: 0x06005D39 RID: 23865 RVA: 0x001FD19F File Offset: 0x001FB39F
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write70_ConfirmImpact(objectToSerialize);
		}

		// Token: 0x06005D3A RID: 23866 RVA: 0x001FD1AD File Offset: 0x001FB3AD
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read70_ConfirmImpact();
		}
	}
}
