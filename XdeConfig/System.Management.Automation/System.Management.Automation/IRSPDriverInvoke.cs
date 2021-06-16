using System;

namespace System.Management.Automation
{
	// Token: 0x0200030A RID: 778
	internal interface IRSPDriverInvoke
	{
		// Token: 0x060024DE RID: 9438
		void EnterNestedPipeline();

		// Token: 0x060024DF RID: 9439
		void ExitNestedPipeline();

		// Token: 0x060024E0 RID: 9440
		bool HandleStopSignal();
	}
}
