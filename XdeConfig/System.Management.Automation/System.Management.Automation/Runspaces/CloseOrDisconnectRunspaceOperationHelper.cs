using System;
using System.Management.Automation.Remoting;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000216 RID: 534
	internal sealed class CloseOrDisconnectRunspaceOperationHelper : IThrottleOperation
	{
		// Token: 0x06001913 RID: 6419 RVA: 0x0009825D File Offset: 0x0009645D
		internal CloseOrDisconnectRunspaceOperationHelper(RemoteRunspace remoteRunspace)
		{
			this.remoteRunspace = remoteRunspace;
			this.remoteRunspace.StateChanged += this.HandleRunspaceStateChanged;
		}

		// Token: 0x06001914 RID: 6420 RVA: 0x00098284 File Offset: 0x00096484
		private void HandleRunspaceStateChanged(object sender, RunspaceStateEventArgs eventArgs)
		{
			switch (eventArgs.RunspaceStateInfo.State)
			{
			case RunspaceState.BeforeOpen:
			case RunspaceState.Opening:
			case RunspaceState.Opened:
			case RunspaceState.Closing:
			case RunspaceState.Disconnecting:
				return;
			}
			this.RaiseOperationCompleteEvent();
		}

		// Token: 0x06001915 RID: 6421 RVA: 0x000982C8 File Offset: 0x000964C8
		internal override void StartOperation()
		{
			if (this.remoteRunspace.RunspaceStateInfo.State == RunspaceState.Closed || this.remoteRunspace.RunspaceStateInfo.State == RunspaceState.Broken || this.remoteRunspace.RunspaceStateInfo.State == RunspaceState.Disconnected)
			{
				this.RaiseOperationCompleteEvent();
				return;
			}
			if (this.remoteRunspace.CanDisconnect && this.remoteRunspace.GetCurrentlyRunningPipeline() != null)
			{
				this.remoteRunspace.DisconnectAsync();
				return;
			}
			this.remoteRunspace.CloseAsync();
		}

		// Token: 0x06001916 RID: 6422 RVA: 0x00098346 File Offset: 0x00096546
		internal override void StopOperation()
		{
		}

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06001917 RID: 6423 RVA: 0x00098348 File Offset: 0x00096548
		// (remove) Token: 0x06001918 RID: 6424 RVA: 0x00098380 File Offset: 0x00096580
		internal override event EventHandler<OperationStateEventArgs> OperationComplete;

		// Token: 0x06001919 RID: 6425 RVA: 0x000983B8 File Offset: 0x000965B8
		private void RaiseOperationCompleteEvent()
		{
			this.remoteRunspace.StateChanged -= this.HandleRunspaceStateChanged;
			OperationStateEventArgs operationStateEventArgs = new OperationStateEventArgs();
			operationStateEventArgs.OperationState = OperationState.StartComplete;
			operationStateEventArgs.BaseEvent = EventArgs.Empty;
			this.OperationComplete.SafeInvoke(this, operationStateEventArgs);
		}

		// Token: 0x04000A54 RID: 2644
		private RemoteRunspace remoteRunspace;
	}
}
