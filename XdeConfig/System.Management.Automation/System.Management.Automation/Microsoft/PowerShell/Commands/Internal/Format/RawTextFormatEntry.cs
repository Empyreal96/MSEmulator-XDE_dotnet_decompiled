using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004E6 RID: 1254
	internal sealed class RawTextFormatEntry : FormatEntryInfo
	{
		// Token: 0x0600363F RID: 13887 RVA: 0x0012605A File Offset: 0x0012425A
		internal override void Deserialize(PSObject so, FormatObjectDeserializer deserializer)
		{
			base.Deserialize(so, deserializer);
			this.text = deserializer.DeserializeStringMemberVariableRaw(so, "text");
		}

		// Token: 0x06003640 RID: 13888 RVA: 0x00126076 File Offset: 0x00124276
		public RawTextFormatEntry() : base("29ED81BA914544d4BC430F027EE053E9")
		{
		}

		// Token: 0x04001BB2 RID: 7090
		internal const string CLSID = "29ED81BA914544d4BC430F027EE053E9";

		// Token: 0x04001BB3 RID: 7091
		public string text;
	}
}
