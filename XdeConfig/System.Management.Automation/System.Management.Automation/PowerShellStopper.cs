using System;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x0200023B RID: 571
	internal class PowerShellStopper : IDisposable
	{
		// Token: 0x06001AED RID: 6893 RVA: 0x0009FA78 File Offset: 0x0009DC78
		internal PowerShellStopper(ExecutionContext context, PowerShell powerShell)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (powerShell == null)
			{
				throw new ArgumentNullException("powerShell");
			}
			this.powerShell = powerShell;
			if (context.CurrentCommandProcessor != null && context.CurrentCommandProcessor.CommandRuntime != null && context.CurrentCommandProcessor.CommandRuntime.PipelineProcessor != null && context.CurrentCommandProcessor.CommandRuntime.PipelineProcessor.LocalPipeline != null)
			{
				this.eventHandler = new EventHandler<PipelineStateEventArgs>(this.LocalPipeline_StateChanged);
				this.pipeline = context.CurrentCommandProcessor.CommandRuntime.PipelineProcessor.LocalPipeline;
				this.pipeline.StateChanged += this.eventHandler;
			}
		}

		// Token: 0x06001AEE RID: 6894 RVA: 0x0009FB2A File Offset: 0x0009DD2A
		private void LocalPipeline_StateChanged(object sender, PipelineStateEventArgs e)
		{
			if (e.PipelineStateInfo.State == PipelineState.Stopping && this.powerShell.InvocationStateInfo.State == PSInvocationState.Running)
			{
				this.powerShell.Stop();
			}
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x0009FB58 File Offset: 0x0009DD58
		public void Dispose()
		{
			if (!this.isDisposed)
			{
				if (this.eventHandler != null)
				{
					this.pipeline.StateChanged -= this.eventHandler;
					this.eventHandler = null;
				}
				GC.SuppressFinalize(this);
				this.isDisposed = true;
			}
		}

		// Token: 0x04000B15 RID: 2837
		private PipelineBase pipeline;

		// Token: 0x04000B16 RID: 2838
		private PowerShell powerShell;

		// Token: 0x04000B17 RID: 2839
		private EventHandler<PipelineStateEventArgs> eventHandler;

		// Token: 0x04000B18 RID: 2840
		private bool isDisposed;
	}
}
