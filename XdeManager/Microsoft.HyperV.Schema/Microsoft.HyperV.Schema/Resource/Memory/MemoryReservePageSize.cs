using System;
using System.CodeDom.Compiler;

namespace HCS.Resource.Memory
{
	// Token: 0x020000C5 RID: 197
	[GeneratedCode("MarsComp", "")]
	public enum MemoryReservePageSize
	{
		// Token: 0x040003D7 RID: 983
		NoPreference,
		// Token: 0x040003D8 RID: 984
		HugePagesOnly,
		// Token: 0x040003D9 RID: 985
		FastPagesOnly,
		// Token: 0x040003DA RID: 986
		FastHugePagesOnly,
		// Token: 0x040003DB RID: 987
		AnyPagesOkay
	}
}
