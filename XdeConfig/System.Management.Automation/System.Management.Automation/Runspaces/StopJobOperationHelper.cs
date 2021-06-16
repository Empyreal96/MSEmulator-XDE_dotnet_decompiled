using System;
using System.Management.Automation.Remoting;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000215 RID: 533
	internal sealed class StopJobOperationHelper : IThrottleOperation
	{
		// Token: 0x0600190C RID: 6412 RVA: 0x00098123 File Offset: 0x00096323
		internal StopJobOperationHelper(Job job)
		{
			this.job = job;
			this.job.StateChanged += this.HandleJobStateChanged;
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x00098149 File Offset: 0x00096349
		private void HandleJobStateChanged(object sender, JobStateEventArgs eventArgs)
		{
			if (this.job.IsFinishedState(this.job.JobStateInfo.State))
			{
				this.RaiseOperationCompleteEvent();
			}
		}

		// Token: 0x0600190E RID: 6414 RVA: 0x0009816E File Offset: 0x0009636E
		internal override void StartOperation()
		{
			if (this.job.IsFinishedState(this.job.JobStateInfo.State))
			{
				this.RaiseOperationCompleteEvent();
				return;
			}
			this.job.StopJob();
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x0009819F File Offset: 0x0009639F
		internal override void StopOperation()
		{
		}

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06001910 RID: 6416 RVA: 0x000981A4 File Offset: 0x000963A4
		// (remove) Token: 0x06001911 RID: 6417 RVA: 0x000981DC File Offset: 0x000963DC
		internal override event EventHandler<OperationStateEventArgs> OperationComplete;

		// Token: 0x06001912 RID: 6418 RVA: 0x00098214 File Offset: 0x00096414
		private void RaiseOperationCompleteEvent()
		{
			this.job.StateChanged -= this.HandleJobStateChanged;
			OperationStateEventArgs operationStateEventArgs = new OperationStateEventArgs();
			operationStateEventArgs.OperationState = OperationState.StartComplete;
			operationStateEventArgs.BaseEvent = EventArgs.Empty;
			this.OperationComplete.SafeInvoke(this, operationStateEventArgs);
		}

		// Token: 0x04000A52 RID: 2642
		private Job job;
	}
}
