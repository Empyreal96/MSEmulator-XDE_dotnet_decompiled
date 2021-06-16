using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009D5 RID: 2517
	[GeneratedCode("sgen", "4.0")]
	internal sealed class GetCmdletParametersSerializer : XmlSerializer1
	{
		// Token: 0x06005CF4 RID: 23796 RVA: 0x001FCE08 File Offset: 0x001FB008
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("GetCmdletParameters", "");
		}

		// Token: 0x06005CF5 RID: 23797 RVA: 0x001FCE1A File Offset: 0x001FB01A
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write53_GetCmdletParameters(objectToSerialize);
		}

		// Token: 0x06005CF6 RID: 23798 RVA: 0x001FCE28 File Offset: 0x001FB028
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read53_GetCmdletParameters();
		}
	}
}
