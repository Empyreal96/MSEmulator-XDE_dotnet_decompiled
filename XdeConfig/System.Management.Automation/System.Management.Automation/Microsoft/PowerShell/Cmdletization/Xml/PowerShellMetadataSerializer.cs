using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009D2 RID: 2514
	[GeneratedCode("sgen", "4.0")]
	internal sealed class PowerShellMetadataSerializer : XmlSerializer1
	{
		// Token: 0x06005CE8 RID: 23784 RVA: 0x001FCD69 File Offset: 0x001FAF69
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("PowerShellMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
		}

		// Token: 0x06005CE9 RID: 23785 RVA: 0x001FCD7B File Offset: 0x001FAF7B
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriter1)writer).Write50_PowerShellMetadata(objectToSerialize);
		}

		// Token: 0x06005CEA RID: 23786 RVA: 0x001FCD89 File Offset: 0x001FAF89
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((XmlSerializationReader1)reader).Read50_PowerShellMetadata();
		}
	}
}
