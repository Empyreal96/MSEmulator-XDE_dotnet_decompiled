using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004DA RID: 1242
	internal abstract class FormatInfoData
	{
		// Token: 0x06003628 RID: 13864 RVA: 0x00125D88 File Offset: 0x00123F88
		internal virtual void Deserialize(PSObject so, FormatObjectDeserializer deserializer)
		{
		}

		// Token: 0x06003629 RID: 13865 RVA: 0x00125D8A File Offset: 0x00123F8A
		protected FormatInfoData(string classId)
		{
			this.classId = classId;
		}

		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x0600362A RID: 13866 RVA: 0x00125D99 File Offset: 0x00123F99
		public string ClassId2e4f51ef21dd47e99d3c952918aff9cd
		{
			get
			{
				return this.classId;
			}
		}

		// Token: 0x04001B98 RID: 7064
		private const int classIdLength = 32;

		// Token: 0x04001B99 RID: 7065
		internal const string classidProperty = "ClassId2e4f51ef21dd47e99d3c952918aff9cd";

		// Token: 0x04001B9A RID: 7066
		private string classId;
	}
}
