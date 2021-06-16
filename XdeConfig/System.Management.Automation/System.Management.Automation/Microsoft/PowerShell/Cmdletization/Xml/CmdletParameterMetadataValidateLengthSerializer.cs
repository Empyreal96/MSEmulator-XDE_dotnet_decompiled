using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009DE RID: 2526
	[GeneratedCode("sgen", "4.0")]
	internal sealed class CmdletParameterMetadataValidateLengthSerializer : XmlSerializer1
	{
		// Token: 0x06005D18 RID: 23832 RVA: 0x001FCFE5 File Offset: 0x001FB1E5
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("CmdletParameterMetadataValidateLength", "");
		}

		// Token: 0x06005D19 RID: 23833 RVA: 0x001FCFF7 File Offset: 0x001FB1F7
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write62_Item(objectToSerialize);
		}

		// Token: 0x06005D1A RID: 23834 RVA: 0x001FD005 File Offset: 0x001FB205
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read62_Item();
		}
	}
}
