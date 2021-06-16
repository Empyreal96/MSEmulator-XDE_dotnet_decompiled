using System;

namespace System.Threading.Tasks.Dataflow.Internal
{
	// Token: 0x0200008B RID: 139
	[Flags]
	internal enum TargetCoreOptions : byte
	{
		// Token: 0x040001BB RID: 443
		None = 0,
		// Token: 0x040001BC RID: 444
		UsesAsyncCompletion = 1,
		// Token: 0x040001BD RID: 445
		RepresentsBlockCompletion = 2
	}
}
