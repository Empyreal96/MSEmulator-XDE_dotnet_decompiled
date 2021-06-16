using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004DF RID: 1247
	internal sealed class FormatStartData : StartData
	{
		// Token: 0x06003632 RID: 13874 RVA: 0x00125E28 File Offset: 0x00124028
		internal override void Deserialize(PSObject so, FormatObjectDeserializer deserializer)
		{
			base.Deserialize(so, deserializer);
			deserializer.VerifyDataNotNull(this.shapeInfo, "shapeInfo");
			this.pageHeaderEntry = (PageHeaderEntry)deserializer.DeserializeMemberObject(so, "pageHeaderEntry");
			this.pageFooterEntry = (PageFooterEntry)deserializer.DeserializeMemberObject(so, "pageFooterEntry");
			this.autosizeInfo = (AutosizeInfo)deserializer.DeserializeMemberObject(so, "autosizeInfo");
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x00125E93 File Offset: 0x00124093
		public FormatStartData() : base("033ecb2bc07a4d43b5ef94ed5a35d280")
		{
		}

		// Token: 0x04001B9F RID: 7071
		internal const string CLSID = "033ecb2bc07a4d43b5ef94ed5a35d280";

		// Token: 0x04001BA0 RID: 7072
		public PageHeaderEntry pageHeaderEntry;

		// Token: 0x04001BA1 RID: 7073
		public PageFooterEntry pageFooterEntry;

		// Token: 0x04001BA2 RID: 7074
		public AutosizeInfo autosizeInfo;
	}
}
