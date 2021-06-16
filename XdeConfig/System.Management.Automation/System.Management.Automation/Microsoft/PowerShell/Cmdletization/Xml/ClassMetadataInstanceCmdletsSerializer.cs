using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009D4 RID: 2516
	[GeneratedCode("sgen", "4.0")]
	internal sealed class ClassMetadataInstanceCmdletsSerializer : XmlSerializer1
	{
		// Token: 0x06005CF0 RID: 23792 RVA: 0x001FCDD3 File Offset: 0x001FAFD3
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("ClassMetadataInstanceCmdlets", "");
		}

		// Token: 0x06005CF1 RID: 23793 RVA: 0x001FCDE5 File Offset: 0x001FAFE5
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write52_ClassMetadataInstanceCmdlets(objectToSerialize);
		}

		// Token: 0x06005CF2 RID: 23794 RVA: 0x001FCDF3 File Offset: 0x001FAFF3
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read52_ClassMetadataInstanceCmdlets();
		}
	}
}
