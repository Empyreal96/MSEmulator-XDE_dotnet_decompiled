using System;

namespace System.Management.Automation.Tracing
{
	// Token: 0x020008FD RID: 2301
	public interface IEtwEventCorrelator
	{
		// Token: 0x17001192 RID: 4498
		// (get) Token: 0x0600561F RID: 22047
		// (set) Token: 0x06005620 RID: 22048
		Guid CurrentActivityId { get; set; }

		// Token: 0x06005621 RID: 22049
		IEtwActivityReverter StartActivity(Guid relatedActivityId);

		// Token: 0x06005622 RID: 22050
		IEtwActivityReverter StartActivity();
	}
}
