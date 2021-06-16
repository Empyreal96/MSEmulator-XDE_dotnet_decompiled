using System;

namespace System.Management.Automation
{
	// Token: 0x0200027C RID: 636
	internal interface IOutputProcessingState
	{
		// Token: 0x14000036 RID: 54
		// (add) Token: 0x06001E08 RID: 7688
		// (remove) Token: 0x06001E09 RID: 7689
		event EventHandler<OutputProcessingStateEventArgs> OutputProcessingStateChanged;
	}
}
