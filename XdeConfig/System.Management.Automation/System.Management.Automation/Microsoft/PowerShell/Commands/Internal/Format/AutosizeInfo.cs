using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004DE RID: 1246
	internal sealed class AutosizeInfo : FormatInfoData
	{
		// Token: 0x06003630 RID: 13872 RVA: 0x00125DFE File Offset: 0x00123FFE
		internal override void Deserialize(PSObject so, FormatObjectDeserializer deserializer)
		{
			base.Deserialize(so, deserializer);
			this.objectCount = deserializer.DeserializeIntMemberVariable(so, "objectCount");
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x00125E1A File Offset: 0x0012401A
		public AutosizeInfo() : base("a27f094f0eec4d64845801a4c06a32ae")
		{
		}

		// Token: 0x04001B9D RID: 7069
		internal const string CLSID = "a27f094f0eec4d64845801a4c06a32ae";

		// Token: 0x04001B9E RID: 7070
		public int objectCount;
	}
}
