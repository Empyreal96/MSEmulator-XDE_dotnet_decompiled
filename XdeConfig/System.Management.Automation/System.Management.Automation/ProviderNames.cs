using System;

namespace System.Management.Automation
{
	// Token: 0x0200081D RID: 2077
	internal abstract class ProviderNames
	{
		// Token: 0x1700102C RID: 4140
		// (get) Token: 0x06004FC5 RID: 20421
		internal abstract string Environment { get; }

		// Token: 0x1700102D RID: 4141
		// (get) Token: 0x06004FC6 RID: 20422
		internal abstract string Certificate { get; }

		// Token: 0x1700102E RID: 4142
		// (get) Token: 0x06004FC7 RID: 20423
		internal abstract string Variable { get; }

		// Token: 0x1700102F RID: 4143
		// (get) Token: 0x06004FC8 RID: 20424
		internal abstract string Alias { get; }

		// Token: 0x17001030 RID: 4144
		// (get) Token: 0x06004FC9 RID: 20425
		internal abstract string Function { get; }

		// Token: 0x17001031 RID: 4145
		// (get) Token: 0x06004FCA RID: 20426
		internal abstract string FileSystem { get; }

		// Token: 0x17001032 RID: 4146
		// (get) Token: 0x06004FCB RID: 20427
		internal abstract string Registry { get; }
	}
}
