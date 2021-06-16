using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004F0 RID: 1264
	internal sealed class FrameInfo : FormatInfoData
	{
		// Token: 0x06003652 RID: 13906 RVA: 0x00126287 File Offset: 0x00124487
		internal override void Deserialize(PSObject so, FormatObjectDeserializer deserializer)
		{
			base.Deserialize(so, deserializer);
			this.leftIndentation = deserializer.DeserializeIntMemberVariable(so, "leftIndentation");
			this.rightIndentation = deserializer.DeserializeIntMemberVariable(so, "rightIndentation");
			this.firstLine = deserializer.DeserializeIntMemberVariable(so, "firstLine");
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x001262C7 File Offset: 0x001244C7
		public FrameInfo() : base("091C9E762E33499eBE318901B6EFB733")
		{
		}

		// Token: 0x04001BC8 RID: 7112
		internal const string CLSID = "091C9E762E33499eBE318901B6EFB733";

		// Token: 0x04001BC9 RID: 7113
		public int leftIndentation;

		// Token: 0x04001BCA RID: 7114
		public int rightIndentation;

		// Token: 0x04001BCB RID: 7115
		public int firstLine;
	}
}
