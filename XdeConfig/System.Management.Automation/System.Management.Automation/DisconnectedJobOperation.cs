using System;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x02000277 RID: 631
	internal class DisconnectedJobOperation : ExecutionCmdletHelper
	{
		// Token: 0x06001DAD RID: 7597 RVA: 0x000AB0C5 File Offset: 0x000A92C5
		internal DisconnectedJobOperation(Pipeline pipeline)
		{
			this.pipeline = pipeline;
			this.pipeline.StateChanged += this.HandlePipelineStateChanged;
		}

		// Token: 0x06001DAE RID: 7598 RVA: 0x000AB0EB File Offset: 0x000A92EB
		internal override void StartOperation()
		{
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x000AB0F0 File Offset: 0x000A92F0
		internal override void StopOperation()
		{
			if (this.pipeline.PipelineStateInfo.State == PipelineState.Running || this.pipeline.PipelineStateInfo.State == PipelineState.Disconnected || this.pipeline.PipelineStateInfo.State == PipelineState.NotStarted)
			{
				this.pipeline.StopAsync();
				return;
			}
			this.SendStopComplete(null);
		}

		// Token: 0x14000034 RID: 52
		// (add) Token: 0x06001DB0 RID: 7600 RVA: 0x000AB148 File Offset: 0x000A9348
		// (remove) Token: 0x06001DB1 RID: 7601 RVA: 0x000AB180 File Offset: 0x000A9380
		internal override event EventHandler<OperationStateEventArgs> OperationComplete;

		// Token: 0x06001DB2 RID: 7602 RVA: 0x000AB1B8 File Offset: 0x000A93B8
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
			this.SendStopComplete(stateEventArgs);
		}

		// Token: 0x06001DB3 RID: 7603 RVA: 0x000AB200 File Offset: 0x000A9400
		private void SendStopComplete(EventArgs eventArgs = null)
		{
			OperationStateEventArgs operationStateEventArgs = new OperationStateEventArgs();
			operationStateEventArgs.BaseEvent = eventArgs;
			operationStateEventArgs.OperationState = OperationState.StopComplete;
			this.OperationComplete.SafeInvoke(this, operationStateEventArgs);
		}
	}
}
