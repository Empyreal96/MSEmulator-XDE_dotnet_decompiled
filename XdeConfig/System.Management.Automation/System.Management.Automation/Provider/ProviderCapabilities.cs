using System;

namespace System.Management.Automation.Provider
{
	// Token: 0x02000472 RID: 1138
	[Flags]
	public enum ProviderCapabilities
	{
		// Token: 0x04001A7D RID: 6781
		None = 0,
		// Token: 0x04001A7E RID: 6782
		Include = 1,
		// Token: 0x04001A7F RID: 6783
		Exclude = 2,
		// Token: 0x04001A80 RID: 6784
		Filter = 4,
		// Token: 0x04001A81 RID: 6785
		ExpandWildcards = 8,
		// Token: 0x04001A82 RID: 6786
		ShouldProcess = 16,
		// Token: 0x04001A83 RID: 6787
		Credentials = 32,
		// Token: 0x04001A84 RID: 6788
		Transactions = 64
	}
}
