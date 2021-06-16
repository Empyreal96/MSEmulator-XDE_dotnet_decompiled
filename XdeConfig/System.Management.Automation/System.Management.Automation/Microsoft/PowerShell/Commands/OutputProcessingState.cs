using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000342 RID: 834
	internal class OutputProcessingState : IOutputProcessingState
	{
		// Token: 0x14000089 RID: 137
		// (add) Token: 0x060028B7 RID: 10423 RVA: 0x000E37E0 File Offset: 0x000E19E0
		// (remove) Token: 0x060028B8 RID: 10424 RVA: 0x000E3818 File Offset: 0x000E1A18
		public event EventHandler<OutputProcessingStateEventArgs> OutputProcessingStateChanged;

		// Token: 0x060028B9 RID: 10425 RVA: 0x000E3850 File Offset: 0x000E1A50
		internal void RaiseOutputProcessingStateChangedEvent(bool processingOutput)
		{
			try
			{
				this.OutputProcessingStateChanged.SafeInvoke(this, new OutputProcessingStateEventArgs(processingOutput));
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
		}
	}
}
