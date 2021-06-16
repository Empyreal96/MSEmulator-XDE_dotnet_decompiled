using System;

namespace System.Management.Automation
{
	// Token: 0x02000273 RID: 627
	public interface IJobDebugger
	{
		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06001D7F RID: 7551
		Debugger Debugger { get; }

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06001D80 RID: 7552
		// (set) Token: 0x06001D81 RID: 7553
		bool IsAsync { get; set; }
	}
}
