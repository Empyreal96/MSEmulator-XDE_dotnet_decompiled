using System;

namespace System.Management.Automation
{
	// Token: 0x0200027B RID: 635
	internal class OutputProcessingStateEventArgs : EventArgs
	{
		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06001E05 RID: 7685 RVA: 0x000ACC60 File Offset: 0x000AAE60
		// (set) Token: 0x06001E06 RID: 7686 RVA: 0x000ACC68 File Offset: 0x000AAE68
		internal bool ProcessingOutput { get; private set; }

		// Token: 0x06001E07 RID: 7687 RVA: 0x000ACC71 File Offset: 0x000AAE71
		internal OutputProcessingStateEventArgs(bool processingOutput)
		{
			this.ProcessingOutput = processingOutput;
		}
	}
}
