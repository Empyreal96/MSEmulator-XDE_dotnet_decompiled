using System;

namespace System.Management.Automation.Tracing
{
	// Token: 0x020008F9 RID: 2297
	public interface IEtwActivityReverter : IDisposable
	{
		// Token: 0x06005615 RID: 22037
		void RevertCurrentActivityId();
	}
}
