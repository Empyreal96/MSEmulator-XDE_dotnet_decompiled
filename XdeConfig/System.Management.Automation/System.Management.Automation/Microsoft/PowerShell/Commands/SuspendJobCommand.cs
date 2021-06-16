using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Remoting;
using System.Threading;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000346 RID: 838
	[Cmdlet("Suspend", "Job", SupportsShouldProcess = true, DefaultParameterSetName = "SessionIdParameterSet", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=210613")]
	[OutputType(new Type[]
	{
		typeof(Job)
	})]
	public class SuspendJobCommand : JobCmdletBase, IDisposable
	{
		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x060028F8 RID: 10488 RVA: 0x000E4C71 File Offset: 0x000E2E71
		// (set) Token: 0x060028F9 RID: 10489 RVA: 0x000E4C79 File Offset: 0x000E2E79
		[ValidateNotNullOrEmpty]
		[Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "JobParameterSet")]
		public Job[] Job
		{
			get
			{
				return this.jobs;
			}
			set
			{
				this.jobs = value;
			}
		}

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x060028FA RID: 10490 RVA: 0x000E4C82 File Offset: 0x000E2E82
		public override string[] Command
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x060028FB RID: 10491 RVA: 0x000E4C85 File Offset: 0x000E2E85
		// (set) Token: 0x060028FC RID: 10492 RVA: 0x000E4C92 File Offset: 0x000E2E92
		[Parameter(ParameterSetName = "FilterParameterSet")]
		[Parameter(ParameterSetName = "NameParameterSet")]
		[Parameter(ParameterSetName = "InstanceIdParameterSet")]
		[Parameter(ParameterSetName = "JobParameterSet")]
		[Alias(new string[]
		{
			"F"
		})]
		[Parameter(ParameterSetName = "SessionIdParameterSet")]
		[Parameter(ParameterSetName = "StateParameterSet")]
		public SwitchParameter Force
		{
			get
			{
				return this.force;
			}
			set
			{
				this.force = value;
			}
		}

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x060028FD RID: 10493 RVA: 0x000E4CA0 File Offset: 0x000E2EA0
		// (set) Token: 0x060028FE RID: 10494 RVA: 0x000E4CAD File Offset: 0x000E2EAD
		[Parameter]
		public SwitchParameter Wait
		{
			get
			{
				return this._wait;
			}
			set
			{
				this._wait = value;
			}
		}

		// Token: 0x060028FF RID: 10495 RVA: 0x000E4CBC File Offset: 0x000E2EBC
		protected override void ProcessRecord()
		{
			string parameterSetName;
			List<Job> list;
			if ((parameterSetName = base.ParameterSetName) != null)
			{
				if (parameterSetName == "NameParameterSet")
				{
					list = base.FindJobsMatchingByName(true, false, true, false);
					goto IL_A2;
				}
				if (parameterSetName == "InstanceIdParameterSet")
				{
					list = base.FindJobsMatchingByInstanceId(true, false, true, false);
					goto IL_A2;
				}
				if (parameterSetName == "SessionIdParameterSet")
				{
					list = base.FindJobsMatchingBySessionId(true, false, true, false);
					goto IL_A2;
				}
				if (parameterSetName == "StateParameterSet")
				{
					list = base.FindJobsMatchingByState(false);
					goto IL_A2;
				}
				if (parameterSetName == "FilterParameterSet")
				{
					list = base.FindJobsMatchingByFilter(false);
					goto IL_A2;
				}
			}
			list = base.CopyJobsToList(this.jobs, false, false);
			IL_A2:
			this._allJobsToSuspend.AddRange(list);
			foreach (Job job in list)
			{
				Job2 job2 = job as Job2;
				if (job2 == null)
				{
					base.WriteError(new ErrorRecord(PSTraceSource.NewNotSupportedException(RemotingErrorIdStrings.JobSuspendNotSupported, new object[]
					{
						job.Id
					}), "Job2OperationNotSupportedOnJob", ErrorCategory.InvalidType, job));
				}
				else
				{
					string target = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.RemovePSJobWhatIfTarget, new object[]
					{
						job.Command,
						job.Id
					});
					if (base.ShouldProcess(target, "Suspend"))
					{
						if (this._wait)
						{
							this._cleanUpActions.Add(job2, new EventHandler<AsyncCompletedEventArgs>(this.HandleSuspendJobCompleted));
						}
						else
						{
							if (job2.IsFinishedState(job2.JobStateInfo.State) || job2.JobStateInfo.State == JobState.Stopping)
							{
								this._warnInvalidState = true;
								continue;
							}
							if (job2.JobStateInfo.State == JobState.Suspending || job2.JobStateInfo.State == JobState.Suspended)
							{
								continue;
							}
							job2.StateChanged += this.noWait_Job2_StateChanged;
						}
						job2.SuspendJobCompleted += this.HandleSuspendJobCompleted;
						lock (this._syncObject)
						{
							if (!this._pendingJobs.Contains(job2.InstanceId))
							{
								this._pendingJobs.Add(job2.InstanceId);
							}
						}
						if (!this._wait && (job2.IsFinishedState(job2.JobStateInfo.State) || job2.JobStateInfo.State == JobState.Suspending || job2.JobStateInfo.State == JobState.Suspended))
						{
							this.ProcessExecutionErrorsAndReleaseWaitHandle(job2);
						}
						job2.SuspendJobAsync(this.force, RemotingErrorIdStrings.ForceSuspendJob);
					}
				}
			}
		}

		// Token: 0x06002900 RID: 10496 RVA: 0x000E4F8C File Offset: 0x000E318C
		private void noWait_Job2_StateChanged(object sender, JobStateEventArgs e)
		{
			Job job = sender as Job;
			switch (e.JobStateInfo.State)
			{
			case JobState.Completed:
			case JobState.Failed:
			case JobState.Stopped:
			case JobState.Suspended:
			case JobState.Suspending:
				this.ProcessExecutionErrorsAndReleaseWaitHandle(job);
				break;
			case JobState.Blocked:
			case JobState.Disconnected:
				break;
			default:
				return;
			}
		}

		// Token: 0x06002901 RID: 10497 RVA: 0x000E4FD8 File Offset: 0x000E31D8
		private void HandleSuspendJobCompleted(object sender, AsyncCompletedEventArgs eventArgs)
		{
			Job job = sender as Job;
			if (eventArgs.Error != null && eventArgs.Error is InvalidJobStateException)
			{
				this._warnInvalidState = true;
			}
			this.ProcessExecutionErrorsAndReleaseWaitHandle(job);
		}

		// Token: 0x06002902 RID: 10498 RVA: 0x000E5024 File Offset: 0x000E3224
		private void ProcessExecutionErrorsAndReleaseWaitHandle(Job job)
		{
			bool flag = false;
			lock (this._syncObject)
			{
				if (!this._pendingJobs.Contains(job.InstanceId))
				{
					return;
				}
				this._pendingJobs.Remove(job.InstanceId);
				if (this._needToCheckForWaitingJobs && this._pendingJobs.Count == 0)
				{
					flag = true;
				}
			}
			if (!this._wait)
			{
				job.StateChanged -= this.noWait_Job2_StateChanged;
				Job2 job2 = job as Job2;
				if (job2 != null)
				{
					job2.SuspendJobCompleted -= this.HandleSuspendJobCompleted;
				}
			}
			ContainerParentJob containerParentJob = job as ContainerParentJob;
			if (containerParentJob != null && containerParentJob.ExecutionError.Count > 0)
			{
				foreach (ErrorRecord errorRecord in from e in containerParentJob.ExecutionError
				where e.FullyQualifiedErrorId == "ContainerParentJobSuspendAsyncError"
				select e)
				{
					if (errorRecord.Exception is InvalidJobStateException)
					{
						this._warnInvalidState = true;
					}
					else
					{
						this._errorsToWrite.Add(errorRecord);
					}
				}
			}
			if (flag)
			{
				this._waitForJobs.Set();
			}
		}

		// Token: 0x06002903 RID: 10499 RVA: 0x000E5188 File Offset: 0x000E3388
		protected override void EndProcessing()
		{
			bool flag = false;
			lock (this._syncObject)
			{
				this._needToCheckForWaitingJobs = true;
				if (this._pendingJobs.Count > 0)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this._waitForJobs.WaitOne();
			}
			if (this._warnInvalidState)
			{
				base.WriteWarning(RemotingErrorIdStrings.SuspendJobInvalidJobState);
			}
			foreach (ErrorRecord errorRecord in this._errorsToWrite)
			{
				base.WriteError(errorRecord);
			}
			foreach (Job sendToPipeline in this._allJobsToSuspend)
			{
				base.WriteObject(sendToPipeline);
			}
			base.EndProcessing();
		}

		// Token: 0x06002904 RID: 10500 RVA: 0x000E528C File Offset: 0x000E348C
		protected override void StopProcessing()
		{
			this._waitForJobs.Set();
		}

		// Token: 0x06002905 RID: 10501 RVA: 0x000E529A File Offset: 0x000E349A
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002906 RID: 10502 RVA: 0x000E52AC File Offset: 0x000E34AC
		protected void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			foreach (Job2 job in this._cleanUpActions.Keys)
			{
				job.SuspendJobCompleted -= this._cleanUpActions[job];
			}
			this._waitForJobs.Dispose();
		}

		// Token: 0x04001433 RID: 5171
		private Job[] jobs;

		// Token: 0x04001434 RID: 5172
		private bool force;

		// Token: 0x04001435 RID: 5173
		private bool _wait;

		// Token: 0x04001436 RID: 5174
		private bool _warnInvalidState;

		// Token: 0x04001437 RID: 5175
		private readonly HashSet<Guid> _pendingJobs = new HashSet<Guid>();

		// Token: 0x04001438 RID: 5176
		private readonly ManualResetEvent _waitForJobs = new ManualResetEvent(false);

		// Token: 0x04001439 RID: 5177
		private readonly Dictionary<Job2, EventHandler<AsyncCompletedEventArgs>> _cleanUpActions = new Dictionary<Job2, EventHandler<AsyncCompletedEventArgs>>();

		// Token: 0x0400143A RID: 5178
		private readonly List<ErrorRecord> _errorsToWrite = new List<ErrorRecord>();

		// Token: 0x0400143B RID: 5179
		private readonly List<Job> _allJobsToSuspend = new List<Job>();

		// Token: 0x0400143C RID: 5180
		private readonly object _syncObject = new object();

		// Token: 0x0400143D RID: 5181
		private bool _needToCheckForWaitingJobs;
	}
}
