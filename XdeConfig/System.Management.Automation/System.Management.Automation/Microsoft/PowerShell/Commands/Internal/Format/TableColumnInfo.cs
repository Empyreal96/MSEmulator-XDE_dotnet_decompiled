using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004E4 RID: 1252
	internal sealed class TableColumnInfo : FormatInfoData
	{
		// Token: 0x0600363C RID: 13884 RVA: 0x00125FE0 File Offset: 0x001241E0
		internal override void Deserialize(PSObject so, FormatObjectDeserializer deserializer)
		{
			base.Deserialize(so, deserializer);
			this.width = deserializer.DeserializeIntMemberVariable(so, "width");
			this.alignment = deserializer.DeserializeIntMemberVariable(so, "alignment");
			this.label = deserializer.DeserializeStringMemberVariable(so, "label");
			this.propertyName = deserializer.DeserializeStringMemberVariable(so, "propertyName");
		}

		// Token: 0x0600363D RID: 13885 RVA: 0x0012603D File Offset: 0x0012423D
		public TableColumnInfo() : base("7572aa4155ec4558817a615acf7dd92e")
		{
		}

		// Token: 0x04001BAD RID: 7085
		internal const string CLSID = "7572aa4155ec4558817a615acf7dd92e";

		// Token: 0x04001BAE RID: 7086
		public int width;

		// Token: 0x04001BAF RID: 7087
		public int alignment = 1;

		// Token: 0x04001BB0 RID: 7088
		public string label;

		// Token: 0x04001BB1 RID: 7089
		public string propertyName;
	}
}
