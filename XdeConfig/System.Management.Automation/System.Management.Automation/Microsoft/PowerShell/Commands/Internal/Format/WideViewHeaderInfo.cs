using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004E2 RID: 1250
	internal sealed class WideViewHeaderInfo : ShapeInfo
	{
		// Token: 0x06003638 RID: 13880 RVA: 0x00125F71 File Offset: 0x00124171
		internal override void Deserialize(PSObject so, FormatObjectDeserializer deserializer)
		{
			base.Deserialize(so, deserializer);
			this.columns = deserializer.DeserializeIntMemberVariable(so, "columns");
		}

		// Token: 0x06003639 RID: 13881 RVA: 0x00125F8D File Offset: 0x0012418D
		public WideViewHeaderInfo() : base("b2e2775d33d544c794d0081f27021b5c")
		{
		}

		// Token: 0x04001BA8 RID: 7080
		internal const string CLSID = "b2e2775d33d544c794d0081f27021b5c";

		// Token: 0x04001BA9 RID: 7081
		public int columns;
	}
}
