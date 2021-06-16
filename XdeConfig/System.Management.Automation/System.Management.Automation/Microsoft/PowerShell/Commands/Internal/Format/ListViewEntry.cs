using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004E8 RID: 1256
	internal sealed class ListViewEntry : FormatEntryInfo
	{
		// Token: 0x06003643 RID: 13891 RVA: 0x001260B3 File Offset: 0x001242B3
		internal override void Deserialize(PSObject so, FormatObjectDeserializer deserializer)
		{
			base.Deserialize(so, deserializer);
			FormatInfoDataListDeserializer<ListViewField>.ReadList(so, "listViewFieldList", this.listViewFieldList, deserializer);
		}

		// Token: 0x06003644 RID: 13892 RVA: 0x001260CF File Offset: 0x001242CF
		public ListViewEntry() : base("cf58f450baa848ef8eb3504008be6978")
		{
		}

		// Token: 0x04001BB5 RID: 7093
		internal const string CLSID = "cf58f450baa848ef8eb3504008be6978";

		// Token: 0x04001BB6 RID: 7094
		public List<ListViewField> listViewFieldList = new List<ListViewField>();
	}
}
