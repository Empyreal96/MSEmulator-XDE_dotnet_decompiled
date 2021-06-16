using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004E3 RID: 1251
	internal sealed class TableHeaderInfo : ShapeInfo
	{
		// Token: 0x0600363A RID: 13882 RVA: 0x00125F9A File Offset: 0x0012419A
		internal override void Deserialize(PSObject so, FormatObjectDeserializer deserializer)
		{
			base.Deserialize(so, deserializer);
			this.hideHeader = deserializer.DeserializeBoolMemberVariable(so, "hideHeader");
			FormatInfoDataListDeserializer<TableColumnInfo>.ReadList(so, "tableColumnInfoList", this.tableColumnInfoList, deserializer);
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x00125FC8 File Offset: 0x001241C8
		public TableHeaderInfo() : base("e3b7a39c089845d388b2e84c5d38f5dd")
		{
			this.tableColumnInfoList = new List<TableColumnInfo>();
		}

		// Token: 0x04001BAA RID: 7082
		internal const string CLSID = "e3b7a39c089845d388b2e84c5d38f5dd";

		// Token: 0x04001BAB RID: 7083
		public bool hideHeader;

		// Token: 0x04001BAC RID: 7084
		public List<TableColumnInfo> tableColumnInfoList;
	}
}
