using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x0200027A RID: 634
	internal class PSInvokeExpressionSyncJob : PSRemotingChildJob
	{
		// Token: 0x06001DF7 RID: 7671 RVA: 0x000AC68C File Offset: 0x000AA88C
		internal PSInvokeExpressionSyncJob(List<IThrottleOperation> operations, ThrottleManager throttleManager)
		{
			base.UsesResultsCollection = true;
			base.Results.AddRef();
			this.throttleManager = throttleManager;
			base.RegisterThrottleComplete(this.throttleManager);
			foreach (IThrottleOperation throttleOperation in operations)
			{
				ExecutionCmdletHelper executionCmdletHelper = throttleOperation as ExecutionCmdletHelper;
				RemoteRunspace remoteRunspace = executionCmdletHelper.Pipeline.Runspace as RemoteRunspace;
				if (remoteRunspace != null)
				{
					remoteRunspace.AvailabilityChanged += base.HandleRunspaceAvailabilityChangedForInvoke;
					remoteRunspace.StateChanged += this.HandleRunspaceStateChanged;
					if (remoteRunspace.RunspaceStateInfo.State == RunspaceState.BeforeOpen)
					{
						remoteRunspace.URIRedirectionReported += base.HandleURIDirectionReported;
					}
				}
				this.helpers.Add(executionCmdletHelper);
				base.AggregateResultsFromHelper(executionCmdletHelper);
				RemotePipeline remotePipeline = executionCmdletHelper.Pipeline as RemotePipeline;
				this.powershells.Add(remotePipeline.PowerShell.InstanceId, remotePipeline.PowerShell);
			}
		}

		// Token: 0x06001DF8 RID: 7672 RVA: 0x000AC7B4 File Offset: 0x000AA9B4
		protected override void DoCleanupOnFinished()
		{
			bool flag = false;
			if (!this.cleanupDone)
			{
				lock (this.SyncObject)
				{
					if (!this.cleanupDone)
					{
						this.cleanupDone = true;
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return;
			}
			foreach (ExecutionCmdletHelper executionCmdletHelper in this.helpers)
			{
				RemoteRunspace remoteRunspace = executionCmdletHelper.PipelineRunspace as RemoteRunspace;
				if (remoteRunspace != null)
				{
					remoteRunspace.StateChanged -= this.HandleRunspaceStateChanged;
					remoteRunspace.URIRedirectionReported -= base.HandleURIDirectionReported;
				}
				base.StopAggregateResultsFromHelper(executionCmdletHelper);
			}
			base.UnregisterThrottleComplete(this.throttleManager);
			base.Results.DecrementRef();
		}

		// Token: 0x06001DF9 RID: 7673 RVA: 0x000AC89C File Offset: 0x000AAA9C
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		// Token: 0x06001DFA RID: 7674 RVA: 0x000AC8A8 File Offset: 0x000AAAA8
		protected override void HandleOperationComplete(object sender, OperationStateEventArgs stateEventArgs)
		{
			ExecutionCmdletHelper helper = sender as ExecutionCmdletHelper;
			Exception ex;
			ErrorRecord errorRecord;
			base.ProcessJobFailure(helper, out ex, out errorRecord);
			if (errorRecord != null)
			{
				this.WriteError(errorRecord);
			}
		}

		// Token: 0x06001DFB RID: 7675 RVA: 0x000AC8D4 File Offset: 0x000AAAD4
		protected override void HandlePipelineStateChanged(object sender, PipelineStateEventArgs e)
		{
			PipelineState state = e.PipelineStateInfo.State;
			switch (state)
			{
			case PipelineState.Running:
				base.SetJobState(JobState.Running);
				return;
			case PipelineState.Stopping:
				break;
			case PipelineState.Stopped:
			case PipelineState.Completed:
			case PipelineState.Failed:
			case PipelineState.Disconnected:
				this.CheckForAndSetDisconnectedState(state);
				break;
			default:
				return;
			}
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x000AC920 File Offset: 0x000AAB20
		private void CheckForAndSetDisconnectedState(PipelineState pipelineState)
		{
			bool flag2;
			lock (this.SyncObject)
			{
				if (this.IsTerminalState())
				{
					return;
				}
				switch (pipelineState)
				{
				case PipelineState.Stopped:
				case PipelineState.Completed:
				case PipelineState.Failed:
					this.pipelineFinishedCount++;
					break;
				case PipelineState.Disconnected:
					this.pipelineDisconnectedCount++;
					break;
				}
				flag2 = (this.pipelineFinishedCount + this.pipelineDisconnectedCount == this.helpers.Count && this.pipelineDisconnectedCount > 0);
			}
			if (flag2)
			{
				base.SetJobState(JobState.Disconnected);
			}
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x000AC9D0 File Offset: 0x000AABD0
		public override void StopJob()
		{
			this.throttleManager.StopAllOperations();
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x000AC9E0 File Offset: 0x000AABE0
		protected override void DoFinish()
		{
			if (this.doFinishCalled)
			{
				return;
			}
			lock (this.SyncObject)
			{
				if (this.doFinishCalled)
				{
					return;
				}
				this.doFinishCalled = true;
			}
			foreach (ExecutionCmdletHelper helper in this.helpers)
			{
				base.DeterminedAndSetJobState(helper);
			}
			if (this.helpers.Count == 0 && base.JobStateInfo.State == JobState.NotStarted)
			{
				base.SetJobState(JobState.Completed);
			}
			this.DoCleanupOnFinished();
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x000ACAA0 File Offset: 0x000AACA0
		internal override PowerShell GetPowerShell(Guid instanceId)
		{
			PowerShell result = null;
			this.powershells.TryGetValue(instanceId, out result);
			return result;
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x000ACAC0 File Offset: 0x000AACC0
		private void HandleRunspaceStateChanged(object sender, RunspaceStateEventArgs e)
		{
			RemoteRunspace remoteRunspace = sender as RemoteRunspace;
			if (remoteRunspace != null && e.RunspaceStateInfo.State != RunspaceState.Opening)
			{
				remoteRunspace.URIRedirectionReported -= base.HandleURIDirectionReported;
				if (e.RunspaceStateInfo.State != RunspaceState.Opened)
				{
					remoteRunspace.StateChanged -= this.HandleRunspaceStateChanged;
					remoteRunspace.AvailabilityChanged -= base.HandleRunspaceAvailabilityChangedForInvoke;
				}
			}
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x000ACB29 File Offset: 0x000AAD29
		internal void StartOperations(List<IThrottleOperation> operations)
		{
			this.throttleManager.SubmitOperations(operations);
			this.throttleManager.EndSubmitOperations();
		}

		// Token: 0x06001E02 RID: 7682 RVA: 0x000ACB42 File Offset: 0x000AAD42
		internal bool IsTerminalState()
		{
			return base.IsFinishedState(base.JobStateInfo.State) || base.JobStateInfo.State == JobState.Disconnected;
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x000ACB68 File Offset: 0x000AAD68
		internal Collection<PowerShell> GetPowerShells()
		{
			Collection<PowerShell> collection = new Collection<PowerShell>();
			foreach (PowerShell item in this.powershells.Values)
			{
				collection.Add(item);
			}
			return collection;
		}

		// Token: 0x06001E04 RID: 7684 RVA: 0x000ACBC8 File Offset: 0x000AADC8
		internal PSRemotingJob CreateDisconnectedRemotingJob()
		{
			List<IThrottleOperation> list = new List<IThrottleOperation>();
			foreach (ExecutionCmdletHelper executionCmdletHelper in this.helpers)
			{
				if (executionCmdletHelper.Pipeline.PipelineStateInfo.State == PipelineState.Disconnected)
				{
					base.RemoveAggreateCallbacksFromHelper(executionCmdletHelper);
					list.Add(new DisconnectedJobOperation(executionCmdletHelper.Pipeline));
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			return new PSRemotingJob(list, 0, base.Name, true);
		}

		// Token: 0x04000D3B RID: 3387
		private List<ExecutionCmdletHelper> helpers = new List<ExecutionCmdletHelper>();

		// Token: 0x04000D3C RID: 3388
		private ThrottleManager throttleManager;

		// Token: 0x04000D3D RID: 3389
		private Dictionary<Guid, PowerShell> powershells = new Dictionary<Guid, PowerShell>();

		// Token: 0x04000D3E RID: 3390
		private int pipelineFinishedCount;

		// Token: 0x04000D3F RID: 3391
		private int pipelineDisconnectedCount;

		// Token: 0x04000D40 RID: 3392
		private bool cleanupDone;

		// Token: 0x04000D41 RID: 3393
		private bool doFinishCalled;
	}
}
