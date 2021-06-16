using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004EE RID: 1262
	internal sealed class FormatPropertyField : FormatValue
	{
		// Token: 0x0600364E RID: 13902 RVA: 0x00126201 File Offset: 0x00124401
		internal override void Deserialize(PSObject so, FormatObjectDeserializer deserializer)
		{
			base.Deserialize(so, deserializer);
			this.propertyValue = deserializer.DeserializeStringMemberVariable(so, "propertyValue");
			this.alignment = deserializer.DeserializeIntMemberVariable(so, "alignment");
		}

		// Token: 0x0600364F RID: 13903 RVA: 0x0012622F File Offset: 0x0012442F
		public FormatPropertyField() : base("78b102e894f742aca8c1d6737b6ff86a")
		{
		}

		// Token: 0x04001BC2 RID: 7106
		internal const string CLSID = "78b102e894f742aca8c1d6737b6ff86a";

		// Token: 0x04001BC3 RID: 7107
		public string propertyValue;

		// Token: 0x04001BC4 RID: 7108
		public int alignment;
	}
}
