using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004EB RID: 1259
	internal sealed class WideViewEntry : FormatEntryInfo
	{
		// Token: 0x06003649 RID: 13897 RVA: 0x00126196 File Offset: 0x00124396
		internal override void Deserialize(PSObject so, FormatObjectDeserializer deserializer)
		{
			base.Deserialize(so, deserializer);
			this.formatPropertyField = (FormatPropertyField)deserializer.DeserializeMandatoryMemberObject(so, "formatPropertyField");
		}

		// Token: 0x0600364A RID: 13898 RVA: 0x001261B7 File Offset: 0x001243B7
		public WideViewEntry() : base("59bf79de63354a7b9e4d1697940ff188")
		{
		}

		// Token: 0x04001BBE RID: 7102
		internal const string CLSID = "59bf79de63354a7b9e4d1697940ff188";

		// Token: 0x04001BBF RID: 7103
		public FormatPropertyField formatPropertyField = new FormatPropertyField();
	}
}
