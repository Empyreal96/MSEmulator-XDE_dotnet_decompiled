using System;

namespace System.Management.Automation
{
	// Token: 0x02000446 RID: 1094
	[Flags]
	internal enum SerializationOptions
	{
		// Token: 0x040019D9 RID: 6617
		None = 0,
		// Token: 0x040019DA RID: 6618
		UseDepthFromTypes = 1,
		// Token: 0x040019DB RID: 6619
		NoRootElement = 2,
		// Token: 0x040019DC RID: 6620
		NoNamespace = 4,
		// Token: 0x040019DD RID: 6621
		NoObjectRefIds = 8,
		// Token: 0x040019DE RID: 6622
		PreserveSerializationSettingOfOriginal = 16,
		// Token: 0x040019DF RID: 6623
		RemotingOptions = 23
	}
}
