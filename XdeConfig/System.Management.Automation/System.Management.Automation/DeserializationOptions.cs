using System;

namespace System.Management.Automation
{
	// Token: 0x0200044A RID: 1098
	[Flags]
	internal enum DeserializationOptions
	{
		// Token: 0x040019E8 RID: 6632
		None = 0,
		// Token: 0x040019E9 RID: 6633
		NoRootElement = 256,
		// Token: 0x040019EA RID: 6634
		NoNamespace = 512,
		// Token: 0x040019EB RID: 6635
		DeserializeScriptBlocks = 1024,
		// Token: 0x040019EC RID: 6636
		RemotingOptions = 768
	}
}
