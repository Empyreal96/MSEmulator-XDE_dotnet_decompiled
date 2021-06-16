using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009E2 RID: 2530
	[GeneratedCode("sgen", "4.0")]
	internal sealed class CmdletParameterMetadataForStaticMethodParameterSerializer : XmlSerializer1
	{
		// Token: 0x06005D28 RID: 23848 RVA: 0x001FD0B9 File Offset: 0x001FB2B9
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("CmdletParameterMetadataForStaticMethodParameter", "");
		}

		// Token: 0x06005D29 RID: 23849 RVA: 0x001FD0CB File Offset: 0x001FB2CB
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write66_Item(objectToSerialize);
		}

		// Token: 0x06005D2A RID: 23850 RVA: 0x001FD0D9 File Offset: 0x001FB2D9
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read66_Item();
		}
	}
}
