using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009D1 RID: 2513
	[GeneratedCode("sgen", "4.0")]
	internal abstract class XmlSerializer1 : XmlSerializer
	{
		// Token: 0x06005CE5 RID: 23781 RVA: 0x001FCD53 File Offset: 0x001FAF53
		protected override XmlSerializationReader CreateReader()
		{
			return new XmlSerializationReader1();
		}

		// Token: 0x06005CE6 RID: 23782 RVA: 0x001FCD5A File Offset: 0x001FAF5A
		protected override XmlSerializationWriter CreateWriter()
		{
			return new XmlSerializationWriter1();
		}
	}
}
