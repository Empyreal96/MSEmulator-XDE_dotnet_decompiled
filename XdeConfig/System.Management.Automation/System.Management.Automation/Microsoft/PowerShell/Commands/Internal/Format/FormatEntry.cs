using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004EF RID: 1263
	internal sealed class FormatEntry : FormatValue
	{
		// Token: 0x06003650 RID: 13904 RVA: 0x0012623C File Offset: 0x0012443C
		internal override void Deserialize(PSObject so, FormatObjectDeserializer deserializer)
		{
			base.Deserialize(so, deserializer);
			FormatInfoDataListDeserializer<FormatValue>.ReadList(so, "formatValueList", this.formatValueList, deserializer);
			this.frameInfo = (FrameInfo)deserializer.DeserializeMemberObject(so, "frameInfo");
		}

		// Token: 0x06003651 RID: 13905 RVA: 0x0012626F File Offset: 0x0012446F
		public FormatEntry() : base("fba029a113a5458d932a2ed4871fadf2")
		{
			this.formatValueList = new List<FormatValue>();
		}

		// Token: 0x04001BC5 RID: 7109
		internal const string CLSID = "fba029a113a5458d932a2ed4871fadf2";

		// Token: 0x04001BC6 RID: 7110
		public List<FormatValue> formatValueList;

		// Token: 0x04001BC7 RID: 7111
		public FrameInfo frameInfo;
	}
}
