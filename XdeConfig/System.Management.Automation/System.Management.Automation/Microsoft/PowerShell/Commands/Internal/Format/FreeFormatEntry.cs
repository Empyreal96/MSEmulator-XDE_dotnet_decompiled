using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004E7 RID: 1255
	internal abstract class FreeFormatEntry : FormatEntryInfo
	{
		// Token: 0x06003641 RID: 13889 RVA: 0x00126083 File Offset: 0x00124283
		internal override void Deserialize(PSObject so, FormatObjectDeserializer deserializer)
		{
			base.Deserialize(so, deserializer);
			FormatInfoDataListDeserializer<FormatValue>.ReadList(so, "formatValueList", this.formatValueList, deserializer);
		}

		// Token: 0x06003642 RID: 13890 RVA: 0x0012609F File Offset: 0x0012429F
		public FreeFormatEntry(string clsid) : base(clsid)
		{
		}

		// Token: 0x04001BB4 RID: 7092
		public List<FormatValue> formatValueList = new List<FormatValue>();
	}
}
