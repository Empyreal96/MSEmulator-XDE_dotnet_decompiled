using System;
using System.Management.Automation;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200033D RID: 829
	internal class ExecutionCmdletHelperComputerName : ExecutionCmdletHelper
	{
		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x0600283B RID: 10299 RVA: 0x000DFFAC File Offset: 0x000DE1AC
		internal RemoteRunspace RemoteRunspace
		{
			get
			{
				return this.remoteRunspace;
			}
		}

		// Token: 0x0600283C RID: 10300 RVA: 0x000DFFB4 File Offset: 0x000DE1B4
		internal ExecutionCmdletHelperComputerName(RemoteRunspace remoteRunspace, Pipeline pipeline, bool invokeAndDisconnect = false)
		{
			base.PipelineRunspace = remoteRunspace;
			this.invokeAndDisconnect = invokeAndDisconnect;
			this.remoteRunspace = remoteRunspace;
			remoteRunspace.StateChanged += this.HandleRunspaceStateChanged;
			this.pipeline = pipeline;
			pipeline.StateChanged += this.HandlePipelineStateChanged;
		}

		// Token: 0x0600283D RID: 10301 RVA: 0x000E0008 File Offset: 0x000DE208
		internal override void StartOperation()
		{
			try
			{
				this.remoteRunspace.OpenAsync();
			}
			catch (PSRemotingTransportException internalException)
			{
				this.internalException = internalException;
				this.RaiseOperationCompleteEvent();
			}
		}

		// Token: 0x0600283E RID: 10302 RVA: 0x000E0044 File Offset: 0x000DE244
		internal override void StopOperation()
		{
			bool flag = false;
			if (this.pipeline.PipelineStateInfo.State == PipelineState.Running || this.pipeline.PipelineStateInfo.State == PipelineState.NotStarted)
			{
				flag = true;
			}
			if (flag)
			{
				this.pipeline.StopAsync();
				return;
			}
			this.RaiseOperationCompleteEvent();
		}

		// Token: 0x14000088 RID: 136
		// (add) Token: 0x0600283F RID: 10303 RVA: 0x000E0090 File Offset: 0x000DE290
		// (remove) Token: 0x06002840 RID: 10304 RVA: 0x000E00C8 File Offset: 0x000DE2C8
		internal override event EventHandler<OperationStateEventArgs> OperationComplete;

		// Token: 0x06002841 RID: 10305 RVA: 0x000E0100 File Offset: 0x000DE300
		private void HandleRunspaceStateChanged(object sender, RunspaceStateEventArgs stateEventArgs)
		{
			switch (stateEventArgs.RunspaceStateInfo.State)
			{
			default:
				return;
			case RunspaceState.Opened:
				try
				{
					if (this.invokeAndDisconnect)
					{
						this.pipeline.InvokeAsyncAndDisconnect();
					}
					else
					{
						this.pipeline.InvokeAsync();
					}
					return;
				}
				catch (InvalidPipelineStateException)
				{
					this.remoteRunspace.CloseAsync();
					return;
				}
				catch (InvalidRunspaceStateException internalException)
				{
					this.internalException = internalException;
					this.remoteRunspace.CloseAsync();
					return;
				}
				break;
			case RunspaceState.Closed:
				if (stateEventArgs.RunspaceStateInfo.Reason != null)
				{
					this.RaiseOperationCompleteEvent(stateEventArgs);
					return;
				}
				this.RaiseOperationCompleteEvent();
				return;
			case RunspaceState.Broken:
				break;
			}
			this.RaiseOperationCompleteEvent(stateEventArgs);
		}

		// Token: 0x06002842 RID: 10306 RVA: 0x000E01C0 File Offset: 0x000DE3C0
		private void HandlePipelineStateChanged(object sender, PipelineStateEventArgs stateEventArgs)
		{
			switch (stateEventArgs.PipelineStateInfo.State)
			{
			default:
				return;
			case PipelineState.Stopped:
			case PipelineState.Completed:
			case PipelineState.Failed:
				if (this.remoteRunspace != null)
				{
					this.remoteRunspace.CloseAsync();
				}
				return;
			}
		}

		// Token: 0x06002843 RID: 10307 RVA: 0x000E020D File Offset: 0x000DE40D
		private void RaiseOperationCompleteEvent()
		{
			this.RaiseOperationCompleteEvent(null);
		}

		// Token: 0x06002844 RID: 10308 RVA: 0x000E0218 File Offset: 0x000DE418
		private void RaiseOperationCompleteEvent(EventArgs baseEventArgs)
		{
			if (this.pipeline != null)
			{
				this.pipeline.StateChanged -= this.HandlePipelineStateChanged;
				this.pipeline.Dispose();
			}
			if (this.remoteRunspace != null)
			{
				this.remoteRunspace.Dispose();
				this.remoteRunspace = null;
			}
			OperationStateEventArgs operationStateEventArgs = new OperationStateEventArgs();
			operationStateEventArgs.OperationState = OperationState.StopComplete;
			operationStateEventArgs.BaseEvent = baseEventArgs;
			this.OperationComplete.SafeInvoke(this, operationStateEventArgs);
		}

		// Token: 0x040013D5 RID: 5077
		private bool invokeAndDisconnect;

		// Token: 0x040013D6 RID: 5078
		private RemoteRunspace remoteRunspace;
	}
}
