using System;
using System.Management.Automation;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200033C RID: 828
	internal class ExecutionCmdletHelperRunspace : ExecutionCmdletHelper
	{
		// Token: 0x06002833 RID: 10291 RVA: 0x000DFD6C File Offset: 0x000DDF6C
		internal ExecutionCmdletHelperRunspace(Pipeline pipeline)
		{
			this.pipeline = pipeline;
			base.PipelineRunspace = pipeline.Runspace;
			this.pipeline.StateChanged += this.HandlePipelineStateChanged;
		}

		// Token: 0x06002834 RID: 10292 RVA: 0x000DFDA0 File Offset: 0x000DDFA0
		internal override void StartOperation()
		{
			try
			{
				if (this.ShouldUseSteppablePipelineOnServer)
				{
					RemotePipeline remotePipeline = this.pipeline as RemotePipeline;
					remotePipeline.SetIsNested(true);
					remotePipeline.SetIsSteppable(true);
				}
				this.pipeline.InvokeAsync();
			}
			catch (InvalidRunspaceStateException internalException)
			{
				this.internalException = internalException;
				this.RaiseOperationCompleteEvent();
			}
			catch (InvalidPipelineStateException internalException2)
			{
				this.internalException = internalException2;
				this.RaiseOperationCompleteEvent();
			}
			catch (InvalidOperationException internalException3)
			{
				this.internalException = internalException3;
				this.RaiseOperationCompleteEvent();
			}
		}

		// Token: 0x06002835 RID: 10293 RVA: 0x000DFE34 File Offset: 0x000DE034
		internal override void StopOperation()
		{
			if (this.pipeline.PipelineStateInfo.State == PipelineState.Running || this.pipeline.PipelineStateInfo.State == PipelineState.Disconnected || this.pipeline.PipelineStateInfo.State == PipelineState.NotStarted)
			{
				this.pipeline.StopAsync();
				return;
			}
			this.RaiseOperationCompleteEvent();
		}

		// Token: 0x14000087 RID: 135
		// (add) Token: 0x06002836 RID: 10294 RVA: 0x000DFE8C File Offset: 0x000DE08C
		// (remove) Token: 0x06002837 RID: 10295 RVA: 0x000DFEC4 File Offset: 0x000DE0C4
		internal override event EventHandler<OperationStateEventArgs> OperationComplete;

		// Token: 0x06002838 RID: 10296 RVA: 0x000DFEFC File Offset: 0x000DE0FC
		private void HandlePipelineStateChanged(object sender, PipelineStateEventArgs stateEventArgs)
		{
			PipelineStateInfo pipelineStateInfo = stateEventArgs.PipelineStateInfo;
			switch (pipelineStateInfo.State)
			{
			case PipelineState.NotStarted:
			case PipelineState.Running:
			case PipelineState.Stopping:
			case PipelineState.Disconnected:
				return;
			}
			this.RaiseOperationCompleteEvent(stateEventArgs);
		}

		// Token: 0x06002839 RID: 10297 RVA: 0x000DFF43 File Offset: 0x000DE143
		private void RaiseOperationCompleteEvent()
		{
			this.RaiseOperationCompleteEvent(null);
		}

		// Token: 0x0600283A RID: 10298 RVA: 0x000DFF4C File Offset: 0x000DE14C
		private void RaiseOperationCompleteEvent(EventArgs baseEventArgs)
		{
			if (this.pipeline != null)
			{
				this.pipeline.StateChanged -= this.HandlePipelineStateChanged;
				this.pipeline.Dispose();
			}
			OperationStateEventArgs operationStateEventArgs = new OperationStateEventArgs();
			operationStateEventArgs.OperationState = OperationState.StopComplete;
			operationStateEventArgs.BaseEvent = baseEventArgs;
			if (this.OperationComplete != null)
			{
				this.OperationComplete.SafeInvoke(this, operationStateEventArgs);
			}
		}

		// Token: 0x040013D3 RID: 5075
		internal bool ShouldUseSteppablePipelineOnServer;
	}
}
