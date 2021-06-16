using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009DF RID: 2527
	[GeneratedCode("sgen", "4.0")]
	internal sealed class CmdletParameterMetadataValidateRangeSerializer : XmlSerializer1
	{
		// Token: 0x06005D1C RID: 23836 RVA: 0x001FD01A File Offset: 0x001FB21A
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("CmdletParameterMetadataValidateRange", "");
		}

		// Token: 0x06005D1D RID: 23837 RVA: 0x001FD02C File Offset: 0x001FB22C
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write63_Item(objectToSerialize);
		}

		// Token: 0x06005D1E RID: 23838 RVA: 0x001FD03A File Offset: 0x001FB23A
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read63_Item();
		}
	}
}
