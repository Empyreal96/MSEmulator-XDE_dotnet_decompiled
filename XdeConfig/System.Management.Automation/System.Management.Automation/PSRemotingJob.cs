using System;
using System.Collections.Generic;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x02000274 RID: 628
	internal class PSRemotingJob : Job
	{
		// Token: 0x06001D82 RID: 7554 RVA: 0x000AA170 File Offset: 0x000A8370
		internal PSRemotingJob(string[] computerNames, List<IThrottleOperation> computerNameHelpers, string remoteCommand, string name) : this(computerNames, computerNameHelpers, remoteCommand, 0, name)
		{
		}

		// Token: 0x06001D83 RID: 7555 RVA: 0x000AA17E File Offset: 0x000A837E
		internal PSRemotingJob(PSSession[] remoteRunspaceInfos, List<IThrottleOperation> runspaceHelpers, string remoteCommand, string name) : this(remoteRunspaceInfos, runspaceHelpers, remoteCommand, 0, name)
		{
		}

		// Token: 0x06001D84 RID: 7556 RVA: 0x000AA18C File Offset: 0x000A838C
		internal PSRemotingJob(string[] computerNames, List<IThrottleOperation> computerNameHelpers, string remoteCommand, int throttleLimit, string name)
		{
			this.moreData = true;
			this.hideComputerName = true;
			this.throttleManager = new ThrottleManager();
			this._syncObject = new object();
			base..ctor(remoteCommand, name);
			foreach (IThrottleOperation throttleOperation in computerNameHelpers)
			{
				ExecutionCmdletHelperComputerName helper = (ExecutionCmdletHelperComputerName)throttleOperation;
				PSRemotingChildJob psremotingChildJob = new PSRemotingChildJob(remoteCommand, helper, this.throttleManager);
				psremotingChildJob.StateChanged += this.HandleChildJobStateChanged;
				psremotingChildJob.JobUnblocked += this.HandleJobUnblocked;
				base.ChildJobs.Add(psremotingChildJob);
			}
			this.CommonInit(throttleLimit, computerNameHelpers);
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x000AA24C File Offset: 0x000A844C
		internal PSRemotingJob(PSSession[] remoteRunspaceInfos, List<IThrottleOperation> runspaceHelpers, string remoteCommand, int throttleLimit, string name)
		{
			this.moreData = true;
			this.hideComputerName = true;
			this.throttleManager = new ThrottleManager();
			this._syncObject = new object();
			base..ctor(remoteCommand, name);
			for (int i = 0; i < remoteRunspaceInfos.Length; i++)
			{
				ExecutionCmdletHelperRunspace helper = (ExecutionCmdletHelperRunspace)runspaceHelpers[i];
				PSRemotingChildJob psremotingChildJob = new PSRemotingChildJob(remoteCommand, helper, this.throttleManager);
				psremotingChildJob.StateChanged += this.HandleChildJobStateChanged;
				psremotingChildJob.JobUnblocked += this.HandleJobUnblocked;
				base.ChildJobs.Add(psremotingChildJob);
			}
			this.CommonInit(throttleLimit, runspaceHelpers);
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x000AA2E8 File Offset: 0x000A84E8
		internal PSRemotingJob(List<IThrottleOperation> helpers, int throttleLimit, string name, bool aggregateResults)
		{
			this.moreData = true;
			this.hideComputerName = true;
			this.throttleManager = new ThrottleManager();
			this._syncObject = new object();
			base..ctor(string.Empty, name);
			foreach (IThrottleOperation throttleOperation in helpers)
			{
				ExecutionCmdletHelper helper = (ExecutionCmdletHelper)throttleOperation;
				PSRemotingChildJob psremotingChildJob = new PSRemotingChildJob(helper, this.throttleManager, aggregateResults);
				psremotingChildJob.StateChanged += this.HandleChildJobStateChanged;
				psremotingChildJob.JobUnblocked += this.HandleJobUnblocked;
				base.ChildJobs.Add(psremotingChildJob);
			}
			base.CloseAllStreams();
			base.SetJobState(JobState.Disconnected);
			this.throttleManager.ThrottleLimit = throttleLimit;
			this.throttleManager.SubmitOperations(helpers);
			this.throttleManager.EndSubmitOperations();
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x000AA3D4 File Offset: 0x000A85D4
		protected PSRemotingJob()
		{
			this.moreData = true;
			this.hideComputerName = true;
			this.throttleManager = new ThrottleManager();
			this._syncObject = new object();
			base..ctor();
		}

		// Token: 0x06001D88 RID: 7560 RVA: 0x000AA400 File Offset: 0x000A8600
		private void CommonInit(int throttleLimit, List<IThrottleOperation> helpers)
		{
			base.CloseAllStreams();
			base.SetJobState(JobState.Running);
			this.throttleManager.ThrottleLimit = throttleLimit;
			this.throttleManager.SubmitOperations(helpers);
			this.throttleManager.EndSubmitOperations();
		}

		// Token: 0x06001D89 RID: 7561 RVA: 0x000AA434 File Offset: 0x000A8634
		internal List<Job> GetJobsForComputer(string computerName)
		{
			List<Job> list = new List<Job>();
			foreach (Job job in base.ChildJobs)
			{
				PSRemotingChildJob psremotingChildJob = job as PSRemotingChildJob;
				if (job != null && string.Equals(psremotingChildJob.Runspace.ConnectionInfo.ComputerName, computerName, StringComparison.OrdinalIgnoreCase))
				{
					list.Add(psremotingChildJob);
				}
			}
			return list;
		}

		// Token: 0x06001D8A RID: 7562 RVA: 0x000AA4AC File Offset: 0x000A86AC
		internal List<Job> GetJobsForRunspace(PSSession runspace)
		{
			List<Job> list = new List<Job>();
			foreach (Job job in base.ChildJobs)
			{
				PSRemotingChildJob psremotingChildJob = job as PSRemotingChildJob;
				if (job != null && psremotingChildJob.Runspace.InstanceId.Equals(runspace.InstanceId))
				{
					list.Add(psremotingChildJob);
				}
			}
			return list;
		}

		// Token: 0x06001D8B RID: 7563 RVA: 0x000AA528 File Offset: 0x000A8728
		internal List<Job> GetJobsForOperation(IThrottleOperation operation)
		{
			List<Job> list = new List<Job>();
			ExecutionCmdletHelper obj = operation as ExecutionCmdletHelper;
			foreach (Job job in base.ChildJobs)
			{
				PSRemotingChildJob psremotingChildJob = job as PSRemotingChildJob;
				if (job != null && psremotingChildJob.Helper.Equals(obj))
				{
					list.Add(psremotingChildJob);
				}
			}
			return list;
		}

		// Token: 0x06001D8C RID: 7564 RVA: 0x000AA5A0 File Offset: 0x000A87A0
		internal void ConnectJobs()
		{
			List<IThrottleOperation> list = new List<IThrottleOperation>();
			foreach (Job job in base.ChildJobs)
			{
				PSRemotingChildJob psremotingChildJob = (PSRemotingChildJob)job;
				if (psremotingChildJob.JobStateInfo.State == JobState.Disconnected)
				{
					list.Add(new PSRemotingJob.ConnectJobOperation(psremotingChildJob));
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			this.SubmitAndWaitForConnect(list);
		}

		// Token: 0x06001D8D RID: 7565 RVA: 0x000AA61C File Offset: 0x000A881C
		internal void ConnectJob(Guid runspaceInstanceId)
		{
			List<IThrottleOperation> list = new List<IThrottleOperation>();
			PSRemotingChildJob psremotingChildJob = this.FindDisconnectedChildJob(runspaceInstanceId);
			if (psremotingChildJob != null)
			{
				list.Add(new PSRemotingJob.ConnectJobOperation(psremotingChildJob));
			}
			if (list.Count == 0)
			{
				return;
			}
			this.SubmitAndWaitForConnect(list);
		}

		// Token: 0x06001D8E RID: 7566 RVA: 0x000AA66C File Offset: 0x000A886C
		private void SubmitAndWaitForConnect(List<IThrottleOperation> connectJobOperations)
		{
			using (ThrottleManager throttleManager = new ThrottleManager())
			{
				using (ManualResetEvent connectResult = new ManualResetEvent(false))
				{
					EventHandler<EventArgs> value = delegate(object sender, EventArgs eventArgs)
					{
						connectResult.Set();
					};
					throttleManager.ThrottleComplete += value;
					try
					{
						throttleManager.ThrottleLimit = 0;
						throttleManager.SubmitOperations(connectJobOperations);
						throttleManager.EndSubmitOperations();
						connectResult.WaitOne();
					}
					finally
					{
						throttleManager.ThrottleComplete -= value;
					}
				}
			}
		}

		// Token: 0x06001D8F RID: 7567 RVA: 0x000AA720 File Offset: 0x000A8920
		internal PowerShell GetAssociatedPowerShellObject(Guid runspaceInstanceId)
		{
			PowerShell result = null;
			PSRemotingChildJob psremotingChildJob = this.FindDisconnectedChildJob(runspaceInstanceId);
			if (psremotingChildJob != null)
			{
				result = psremotingChildJob.GetPowerShell();
			}
			return result;
		}

		// Token: 0x06001D90 RID: 7568 RVA: 0x000AA744 File Offset: 0x000A8944
		private PSRemotingChildJob FindDisconnectedChildJob(Guid runspaceInstanceId)
		{
			PSRemotingChildJob result = null;
			foreach (Job job in base.ChildJobs)
			{
				PSRemotingChildJob psremotingChildJob = (PSRemotingChildJob)job;
				if (psremotingChildJob.Runspace.InstanceId.Equals(runspaceInstanceId) && psremotingChildJob.JobStateInfo.State == JobState.Disconnected)
				{
					result = psremotingChildJob;
					break;
				}
			}
			return result;
		}

		// Token: 0x06001D91 RID: 7569 RVA: 0x000AA7BC File Offset: 0x000A89BC
		internal void InternalStopJob()
		{
			if (this.isDisposed || this._stopIsCalled || base.IsFinishedState(base.JobStateInfo.State))
			{
				return;
			}
			lock (this._syncObject)
			{
				if (this.isDisposed || this._stopIsCalled || base.IsFinishedState(base.JobStateInfo.State))
				{
					return;
				}
				this._stopIsCalled = true;
			}
			this.throttleManager.StopAllOperations();
			base.Finished.WaitOne();
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06001D92 RID: 7570 RVA: 0x000AA860 File Offset: 0x000A8A60
		public override bool HasMoreData
		{
			get
			{
				if (this.moreData && base.IsFinishedState(base.JobStateInfo.State))
				{
					bool flag = false;
					for (int i = 0; i < base.ChildJobs.Count; i++)
					{
						if (base.ChildJobs[i].HasMoreData)
						{
							flag = true;
							break;
						}
					}
					this.moreData = flag;
				}
				return this.moreData;
			}
		}

		// Token: 0x06001D93 RID: 7571 RVA: 0x000AA8C4 File Offset: 0x000A8AC4
		public override void StopJob()
		{
			if (base.JobStateInfo.State == JobState.Disconnected)
			{
				bool flag;
				try
				{
					this.ConnectJobs();
					flag = true;
				}
				catch (InvalidRunspaceStateException)
				{
					flag = false;
				}
				catch (PSRemotingTransportException)
				{
					flag = false;
				}
				catch (PSInvalidOperationException)
				{
					flag = false;
				}
				if (!flag && base.Error.IsOpen)
				{
					string message = StringUtil.Format(RemotingErrorIdStrings.StopJobNotConnected, base.Name);
					Exception exception = new RuntimeException(message);
					ErrorRecord errorRecord = new ErrorRecord(exception, "StopJobCannotConnectToServer", ErrorCategory.InvalidOperation, this);
					this.WriteError(errorRecord);
					return;
				}
			}
			this.InternalStopJob();
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06001D94 RID: 7572 RVA: 0x000AA964 File Offset: 0x000A8B64
		public override string StatusMessage
		{
			get
			{
				return this.statusMessage;
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06001D95 RID: 7573 RVA: 0x000AA96C File Offset: 0x000A8B6C
		// (set) Token: 0x06001D96 RID: 7574 RVA: 0x000AA974 File Offset: 0x000A8B74
		internal bool HideComputerName
		{
			get
			{
				return this.hideComputerName;
			}
			set
			{
				this.hideComputerName = value;
				foreach (Job job in base.ChildJobs)
				{
					PSRemotingChildJob psremotingChildJob = job as PSRemotingChildJob;
					if (psremotingChildJob != null)
					{
						psremotingChildJob.HideComputerName = value;
					}
				}
			}
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x000AA9D4 File Offset: 0x000A8BD4
		private void SetStatusMessage()
		{
			this.statusMessage = "test";
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x000AA9E4 File Offset: 0x000A8BE4
		private void HandleChildJobStateChanged(object sender, JobStateEventArgs e)
		{
			this.CheckDisconnectedAndUpdateState(e.JobStateInfo.State, e.PreviousJobStateInfo.State);
			if (e.JobStateInfo.State == JobState.Blocked)
			{
				lock (this._syncObject)
				{
					this.blockedChildJobsCount++;
				}
				base.SetJobState(JobState.Blocked, null);
				return;
			}
			if (e.JobStateInfo.State == JobState.AtBreakpoint)
			{
				lock (this._syncObject)
				{
					this.debugChildJobsCount++;
				}
				base.SetJobState(JobState.AtBreakpoint);
				return;
			}
			if (e.JobStateInfo.State == JobState.Running && e.PreviousJobStateInfo.State == JobState.AtBreakpoint)
			{
				int num;
				lock (this._syncObject)
				{
					num = --this.debugChildJobsCount;
				}
				if (num == 0)
				{
					base.SetJobState(JobState.Running);
					return;
				}
			}
			if (!base.IsFinishedState(e.JobStateInfo.State))
			{
				return;
			}
			if (e.JobStateInfo.State == JobState.Failed)
			{
				this.atleastOneChildJobFailed = true;
			}
			bool flag4 = false;
			lock (this._syncObject)
			{
				this.finishedChildJobsCount++;
				if (this.finishedChildJobsCount + this.disconnectedChildJobsCount == base.ChildJobs.Count)
				{
					flag4 = true;
				}
			}
			if (flag4)
			{
				if (this.disconnectedChildJobsCount > 0)
				{
					base.SetJobState(JobState.Disconnected);
					return;
				}
				if (this.atleastOneChildJobFailed)
				{
					base.SetJobState(JobState.Failed);
					return;
				}
				if (this._stopIsCalled)
				{
					base.SetJobState(JobState.Stopped);
					return;
				}
				base.SetJobState(JobState.Completed);
			}
		}

		// Token: 0x06001D99 RID: 7577 RVA: 0x000AABDC File Offset: 0x000A8DDC
		private void CheckDisconnectedAndUpdateState(JobState newState, JobState prevState)
		{
			if (base.IsFinishedState(base.JobStateInfo.State))
			{
				return;
			}
			lock (this._syncObject)
			{
				if (newState == JobState.Disconnected)
				{
					this.disconnectedChildJobsCount++;
					if (prevState == JobState.Blocked)
					{
						this.blockedChildJobsCount--;
					}
					if (this.disconnectedChildJobsCount + this.finishedChildJobsCount + this.blockedChildJobsCount == base.ChildJobs.Count)
					{
						base.SetJobState(JobState.Disconnected, null);
					}
				}
				else
				{
					if (prevState == JobState.Disconnected)
					{
						this.disconnectedChildJobsCount--;
					}
					if (newState == JobState.Running && base.JobStateInfo.State == JobState.Disconnected)
					{
						base.SetJobState(JobState.Running, null);
					}
				}
			}
		}

		// Token: 0x06001D9A RID: 7578 RVA: 0x000AACA8 File Offset: 0x000A8EA8
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.isDisposed)
				{
					return;
				}
				lock (this._syncObject)
				{
					if (this.isDisposed)
					{
						return;
					}
					this.isDisposed = true;
				}
				try
				{
					if (!base.IsFinishedState(base.JobStateInfo.State))
					{
						this.StopJob();
					}
					foreach (Job job in base.ChildJobs)
					{
						job.Dispose();
					}
					this.throttleManager.Dispose();
				}
				finally
				{
					base.Dispose(disposing);
				}
			}
		}

		// Token: 0x06001D9B RID: 7579 RVA: 0x000AAD78 File Offset: 0x000A8F78
		private string ConstructLocation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (base.ChildJobs.Count > 0)
			{
				foreach (Job job in base.ChildJobs)
				{
					PSRemotingChildJob psremotingChildJob = (PSRemotingChildJob)job;
					stringBuilder.Append(psremotingChildJob.Location);
					stringBuilder.Append(",");
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06001D9C RID: 7580 RVA: 0x000AAE08 File Offset: 0x000A9008
		public override string Location
		{
			get
			{
				return this.ConstructLocation();
			}
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06001D9D RID: 7581 RVA: 0x000AAE10 File Offset: 0x000A9010
		internal override bool CanDisconnect
		{
			get
			{
				return base.ChildJobs.Count > 0 && base.ChildJobs[0].CanDisconnect;
			}
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x000AAE34 File Offset: 0x000A9034
		internal override IEnumerable<RemoteRunspace> GetRunspaces()
		{
			List<RemoteRunspace> list = new List<RemoteRunspace>();
			foreach (Job job in base.ChildJobs)
			{
				PSRemotingChildJob psremotingChildJob = (PSRemotingChildJob)job;
				list.Add(psremotingChildJob.Runspace as RemoteRunspace);
			}
			return list;
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x000AAE98 File Offset: 0x000A9098
		private void HandleJobUnblocked(object sender, EventArgs eventArgs)
		{
			bool flag = false;
			lock (this._syncObject)
			{
				this.blockedChildJobsCount--;
				if (this.blockedChildJobsCount == 0)
				{
					flag = true;
				}
			}
			if (flag)
			{
				base.SetJobState(JobState.Running, null);
			}
		}

		// Token: 0x04000D16 RID: 3350
		private bool moreData;

		// Token: 0x04000D17 RID: 3351
		private bool _stopIsCalled;

		// Token: 0x04000D18 RID: 3352
		private string statusMessage;

		// Token: 0x04000D19 RID: 3353
		private bool hideComputerName;

		// Token: 0x04000D1A RID: 3354
		private bool atleastOneChildJobFailed;

		// Token: 0x04000D1B RID: 3355
		private int finishedChildJobsCount;

		// Token: 0x04000D1C RID: 3356
		private int blockedChildJobsCount;

		// Token: 0x04000D1D RID: 3357
		private int disconnectedChildJobsCount;

		// Token: 0x04000D1E RID: 3358
		private int debugChildJobsCount;

		// Token: 0x04000D1F RID: 3359
		private bool isDisposed;

		// Token: 0x04000D20 RID: 3360
		private ThrottleManager throttleManager;

		// Token: 0x04000D21 RID: 3361
		private readonly object _syncObject;

		// Token: 0x02000275 RID: 629
		private class ConnectJobOperation : IThrottleOperation
		{
			// Token: 0x06001DA0 RID: 7584 RVA: 0x000AAEF8 File Offset: 0x000A90F8
			internal ConnectJobOperation(PSRemotingChildJob job)
			{
				this.psRemoteChildJob = job;
				this.psRemoteChildJob.StateChanged += this.ChildJobStateChangedHandler;
			}

			// Token: 0x06001DA1 RID: 7585 RVA: 0x000AAF20 File Offset: 0x000A9120
			internal override void StartOperation()
			{
				bool flag = true;
				try
				{
					this.psRemoteChildJob.ConnectAsync();
				}
				catch (InvalidJobStateException innerException)
				{
					flag = false;
					string message = StringUtil.Format(RemotingErrorIdStrings.JobConnectFailed, this.psRemoteChildJob.Name);
					Exception exception = new RuntimeException(message, innerException);
					ErrorRecord errorRecord = new ErrorRecord(exception, "PSJobConnectFailed", ErrorCategory.InvalidOperation, this.psRemoteChildJob);
					this.psRemoteChildJob.WriteError(errorRecord);
				}
				if (!flag)
				{
					this.RemoveEventCallback();
					this.SendStartComplete();
				}
			}

			// Token: 0x06001DA2 RID: 7586 RVA: 0x000AAFA0 File Offset: 0x000A91A0
			internal override void StopOperation()
			{
				this.RemoveEventCallback();
				OperationStateEventArgs operationStateEventArgs = new OperationStateEventArgs();
				operationStateEventArgs.OperationState = OperationState.StopComplete;
				this.OperationComplete.SafeInvoke(this, operationStateEventArgs);
			}

			// Token: 0x14000033 RID: 51
			// (add) Token: 0x06001DA3 RID: 7587 RVA: 0x000AAFD0 File Offset: 0x000A91D0
			// (remove) Token: 0x06001DA4 RID: 7588 RVA: 0x000AB008 File Offset: 0x000A9208
			internal override event EventHandler<OperationStateEventArgs> OperationComplete;

			// Token: 0x06001DA5 RID: 7589 RVA: 0x000AB03D File Offset: 0x000A923D
			private void ChildJobStateChangedHandler(object sender, JobStateEventArgs eArgs)
			{
				if (eArgs.JobStateInfo.State == JobState.Disconnected)
				{
					return;
				}
				this.RemoveEventCallback();
				this.SendStartComplete();
			}

			// Token: 0x06001DA6 RID: 7590 RVA: 0x000AB05C File Offset: 0x000A925C
			private void SendStartComplete()
			{
				OperationStateEventArgs operationStateEventArgs = new OperationStateEventArgs();
				operationStateEventArgs.OperationState = OperationState.StartComplete;
				this.OperationComplete.SafeInvoke(this, operationStateEventArgs);
			}

			// Token: 0x06001DA7 RID: 7591 RVA: 0x000AB083 File Offset: 0x000A9283
			private void RemoveEventCallback()
			{
				this.psRemoteChildJob.StateChanged -= this.ChildJobStateChangedHandler;
			}

			// Token: 0x04000D22 RID: 3362
			private PSRemotingChildJob psRemoteChildJob;
		}
	}
}
