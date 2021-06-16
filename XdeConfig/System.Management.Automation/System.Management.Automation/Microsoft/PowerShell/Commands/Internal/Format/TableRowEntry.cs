using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004EA RID: 1258
	internal sealed class TableRowEntry : FormatEntryInfo
	{
		// Token: 0x06003647 RID: 13895 RVA: 0x00126150 File Offset: 0x00124350
		internal override void Deserialize(PSObject so, FormatObjectDeserializer deserializer)
		{
			base.Deserialize(so, deserializer);
			FormatInfoDataListDeserializer<FormatPropertyField>.ReadList(so, "formatPropertyFieldList", this.formatPropertyFieldList, deserializer);
			this.multiLine = deserializer.DeserializeBoolMemberVariable(so, "multiLine");
		}

		// Token: 0x06003648 RID: 13896 RVA: 0x0012617E File Offset: 0x0012437E
		public TableRowEntry() : base("0e59526e2dd441aa91e7fc952caf4a36")
		{
		}

		// Token: 0x04001BBB RID: 7099
		internal const string CLSID = "0e59526e2dd441aa91e7fc952caf4a36";

		// Token: 0x04001BBC RID: 7100
		public List<FormatPropertyField> formatPropertyFieldList = new List<FormatPropertyField>();

		// Token: 0x04001BBD RID: 7101
		public bool multiLine;
	}
}
