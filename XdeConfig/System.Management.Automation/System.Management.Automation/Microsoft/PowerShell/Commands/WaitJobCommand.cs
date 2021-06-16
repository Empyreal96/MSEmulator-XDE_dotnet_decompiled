using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000344 RID: 836
	[OutputType(new Type[]
	{
		typeof(Job)
	})]
	[Cmdlet("Wait", "Job", DefaultParameterSetName = "SessionIdParameterSet", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=113422")]
	public class WaitJobCommand : JobCmdletBase, IDisposable
	{
		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x060028C9 RID: 10441 RVA: 0x000E3EAE File Offset: 0x000E20AE
		// (set) Token: 0x060028CA RID: 10442 RVA: 0x000E3EB6 File Offset: 0x000E20B6
		[ValidateNotNullOrEmpty]
		[Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "JobParameterSet")]
		public Job[] Job { get; set; }

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x060028CB RID: 10443 RVA: 0x000E3EBF File Offset: 0x000E20BF
		// (set) Token: 0x060028CC RID: 10444 RVA: 0x000E3EC7 File Offset: 0x000E20C7
		[Parameter]
		public SwitchParameter Any { get; set; }

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x060028CD RID: 10445 RVA: 0x000E3ED0 File Offset: 0x000E20D0
		// (set) Token: 0x060028CE RID: 10446 RVA: 0x000E3ED8 File Offset: 0x000E20D8
		[ValidateRange(-1, 2147483647)]
		[Parameter]
		[Alias(new string[]
		{
			"TimeoutSec"
		})]
		public int Timeout
		{
			get
			{
				return this._timeoutInSeconds;
			}
			set
			{
				this._timeoutInSeconds = value;
			}
		}

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x060028CF RID: 10447 RVA: 0x000E3EE1 File Offset: 0x000E20E1
		// (set) Token: 0x060028D0 RID: 10448 RVA: 0x000E3EE9 File Offset: 0x000E20E9
		[Parameter]
		public SwitchParameter Force { get; set; }

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x060028D1 RID: 10449 RVA: 0x000E3EF2 File Offset: 0x000E20F2
		// (set) Token: 0x060028D2 RID: 10450 RVA: 0x000E3EFA File Offset: 0x000E20FA
		public override string[] Command { get; set; }

		// Token: 0x060028D3 RID: 10451 RVA: 0x000E3F04 File Offset: 0x000E2104
		private void SetEndProcessingAction(Action endProcessingAction)
		{
			lock (this._endProcessingActionLock)
			{
				if (this._endProcessingAction == null)
				{
					this._endProcessingAction = endProcessingAction;
					this._endProcessingActionIsReady.Set();
				}
			}
		}

		// Token: 0x060028D4 RID: 10452 RVA: 0x000E3F58 File Offset: 0x000E2158
		private void InvokeEndProcesingAction()
		{
			this._endProcessingActionIsReady.Wait();
			Action endProcessingAction;
			lock (this._endProcessingActionLock)
			{
				endProcessingAction = this._endProcessingAction;
			}
			if (endProcessingAction != null)
			{
				endProcessingAction();
			}
		}

		// Token: 0x060028D5 RID: 10453 RVA: 0x000E3FB0 File Offset: 0x000E21B0
		private void CleanUpEndProcessing()
		{
			this._endProcessingActionIsReady.Dispose();
		}

		// Token: 0x060028D6 RID: 10454 RVA: 0x000E3FC0 File Offset: 0x000E21C0
		private void HandleJobStateChangedEvent(object source, JobStateEventArgs eventArgs)
		{
			Job job = (Job)source;
			lock (this._jobTrackingLock)
			{
				if (eventArgs.JobStateInfo.State == JobState.Blocked)
				{
					this._blockedJobs.Add(job);
				}
				else
				{
					this._blockedJobs.Remove(job);
				}
				if ((!this.Force && job.IsPersistentState(eventArgs.JobStateInfo.State)) || (this.Force && job.IsFinishedState(eventArgs.JobStateInfo.State)))
				{
					if (!job.IsFinishedState(eventArgs.JobStateInfo.State))
					{
						this._warnNotTerminal = true;
					}
					this._finishedJobs.Add(job);
				}
				else
				{
					this._finishedJobs.Remove(job);
				}
				if (this.Any.IsPresent)
				{
					if (this._finishedJobs.Count > 0)
					{
						this.SetEndProcessingAction(new Action(this.EndProcessingOutputSingleFinishedJob));
					}
					else if (this._blockedJobs.Count == this._jobsToWaitFor.Count)
					{
						this.SetEndProcessingAction(new Action(this.EndProcessingBlockedJobsError));
					}
				}
				else if (this._finishedJobs.Count == this._jobsToWaitFor.Count)
				{
					this.SetEndProcessingAction(new Action(this.EndProcessingOutputAllFinishedJobs));
				}
				else if (this._blockedJobs.Count > 0)
				{
					this.SetEndProcessingAction(new Action(this.EndProcessingBlockedJobsError));
				}
			}
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x000E4158 File Offset: 0x000E2358
		private void AddJobsThatNeedJobChangesTracking(IEnumerable<Job> jobsToAdd)
		{
			lock (this._jobTrackingLock)
			{
				this._jobsToWaitFor.AddRange(jobsToAdd);
			}
		}

		// Token: 0x060028D8 RID: 10456 RVA: 0x000E41A0 File Offset: 0x000E23A0
		private void StartJobChangesTracking()
		{
			lock (this._jobTrackingLock)
			{
				if (this._jobsToWaitFor.Count == 0)
				{
					this.SetEndProcessingAction(new Action(this.EndProcessingDoNothing));
				}
				else
				{
					foreach (Job job in this._jobsToWaitFor)
					{
						job.StateChanged += this.HandleJobStateChangedEvent;
						this.HandleJobStateChangedEvent(job, new JobStateEventArgs(job.JobStateInfo));
					}
				}
			}
		}

		// Token: 0x060028D9 RID: 10457 RVA: 0x000E425C File Offset: 0x000E245C
		private void CleanUpJobChangesTracking()
		{
			lock (this._jobTrackingLock)
			{
				foreach (Job job in this._jobsToWaitFor)
				{
					job.StateChanged -= this.HandleJobStateChangedEvent;
				}
			}
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x000E4334 File Offset: 0x000E2534
		private List<Job> GetFinishedJobs()
		{
			List<Job> result;
			lock (this._jobTrackingLock)
			{
				result = (from j in this._jobsToWaitFor
				where (!this.Force && j.IsPersistentState(j.JobStateInfo.State)) || (this.Force && j.IsFinishedState(j.JobStateInfo.State))
				select j).ToList<Job>();
			}
			return result;
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x000E43A4 File Offset: 0x000E25A4
		private Job GetOneBlockedJob()
		{
			Job result;
			lock (this._jobTrackingLock)
			{
				result = this._jobsToWaitFor.FirstOrDefault((Job j) => j.JobStateInfo.State == JobState.Blocked);
			}
			return result;
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x000E441C File Offset: 0x000E261C
		private void StartTimeoutTracking(int timeoutInSeconds)
		{
			if (timeoutInSeconds == 0)
			{
				this.SetEndProcessingAction(new Action(this.EndProcessingDoNothing));
				return;
			}
			if (timeoutInSeconds > 0)
			{
				lock (this._timerLock)
				{
					this._timer = new Timer(delegate(object _)
					{
						this.SetEndProcessingAction(new Action(this.EndProcessingDoNothing));
					}, null, timeoutInSeconds * 1000, -1);
				}
			}
		}

		// Token: 0x060028DD RID: 10461 RVA: 0x000E4498 File Offset: 0x000E2698
		private void CleanUpTimoutTracking()
		{
			lock (this._timerLock)
			{
				if (this._timer != null)
				{
					this._timer.Dispose();
					this._timer = null;
				}
			}
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x000E44EC File Offset: 0x000E26EC
		protected override void StopProcessing()
		{
			this.SetEndProcessingAction(new Action(this.EndProcessingDoNothing));
		}

		// Token: 0x060028DF RID: 10463 RVA: 0x000E4500 File Offset: 0x000E2700
		protected override void BeginProcessing()
		{
			this.StartTimeoutTracking(this._timeoutInSeconds);
		}

		// Token: 0x060028E0 RID: 10464 RVA: 0x000E4510 File Offset: 0x000E2710
		protected override void ProcessRecord()
		{
			string parameterSetName;
			List<Job> jobsToAdd;
			if ((parameterSetName = base.ParameterSetName) != null)
			{
				if (parameterSetName == "NameParameterSet")
				{
					jobsToAdd = base.FindJobsMatchingByName(true, false, true, false);
					goto IL_97;
				}
				if (parameterSetName == "InstanceIdParameterSet")
				{
					jobsToAdd = base.FindJobsMatchingByInstanceId(true, false, true, false);
					goto IL_97;
				}
				if (parameterSetName == "SessionIdParameterSet")
				{
					jobsToAdd = base.FindJobsMatchingBySessionId(true, false, true, false);
					goto IL_97;
				}
				if (parameterSetName == "StateParameterSet")
				{
					jobsToAdd = base.FindJobsMatchingByState(false);
					goto IL_97;
				}
				if (parameterSetName == "FilterParameterSet")
				{
					jobsToAdd = base.FindJobsMatchingByFilter(false);
					goto IL_97;
				}
			}
			jobsToAdd = base.CopyJobsToList(this.Job, false, false);
			IL_97:
			this.AddJobsThatNeedJobChangesTracking(jobsToAdd);
		}

		// Token: 0x060028E1 RID: 10465 RVA: 0x000E45BB File Offset: 0x000E27BB
		protected override void EndProcessing()
		{
			this.StartJobChangesTracking();
			this.InvokeEndProcesingAction();
			if (this._warnNotTerminal)
			{
				base.WriteWarning(RemotingErrorIdStrings.JobSuspendedDisconnectedWaitWithForce);
			}
		}

		// Token: 0x060028E2 RID: 10466 RVA: 0x000E45DC File Offset: 0x000E27DC
		private void EndProcessingOutputSingleFinishedJob()
		{
			Job job = this.GetFinishedJobs().FirstOrDefault<Job>();
			if (job != null)
			{
				base.WriteObject(job);
			}
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x000E4600 File Offset: 0x000E2800
		private void EndProcessingOutputAllFinishedJobs()
		{
			IEnumerable<Job> finishedJobs = this.GetFinishedJobs();
			foreach (Job sendToPipeline in finishedJobs)
			{
				base.WriteObject(sendToPipeline);
			}
		}

		// Token: 0x060028E4 RID: 10468 RVA: 0x000E4650 File Offset: 0x000E2850
		private void EndProcessingBlockedJobsError()
		{
			string jobBlockedSoWaitJobCannotContinue = RemotingErrorIdStrings.JobBlockedSoWaitJobCannotContinue;
			Exception exception = new ArgumentException(jobBlockedSoWaitJobCannotContinue);
			ErrorRecord errorRecord = new ErrorRecord(exception, "BlockedJobsDeadlockWithWaitJob", ErrorCategory.DeadlockDetected, this.GetOneBlockedJob());
			base.ThrowTerminatingError(errorRecord);
		}

		// Token: 0x060028E5 RID: 10469 RVA: 0x000E4684 File Offset: 0x000E2884
		private void EndProcessingDoNothing()
		{
		}

		// Token: 0x060028E6 RID: 10470 RVA: 0x000E4686 File Offset: 0x000E2886
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060028E7 RID: 10471 RVA: 0x000E4698 File Offset: 0x000E2898
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				lock (this._disposableLock)
				{
					if (!this._isDisposed)
					{
						this._isDisposed = true;
						this.CleanUpTimoutTracking();
						this.CleanUpJobChangesTracking();
						this.CleanUpEndProcessing();
					}
				}
			}
		}

		// Token: 0x0400141A RID: 5146
		private int _timeoutInSeconds = -1;

		// Token: 0x0400141B RID: 5147
		private readonly object _endProcessingActionLock = new object();

		// Token: 0x0400141C RID: 5148
		private Action _endProcessingAction;

		// Token: 0x0400141D RID: 5149
		private readonly ManualResetEventSlim _endProcessingActionIsReady = new ManualResetEventSlim(false);

		// Token: 0x0400141E RID: 5150
		private readonly HashSet<Job> _finishedJobs = new HashSet<Job>();

		// Token: 0x0400141F RID: 5151
		private readonly HashSet<Job> _blockedJobs = new HashSet<Job>();

		// Token: 0x04001420 RID: 5152
		private readonly List<Job> _jobsToWaitFor = new List<Job>();

		// Token: 0x04001421 RID: 5153
		private readonly object _jobTrackingLock = new object();

		// Token: 0x04001422 RID: 5154
		private Timer _timer;

		// Token: 0x04001423 RID: 5155
		private readonly object _timerLock = new object();

		// Token: 0x04001424 RID: 5156
		private bool _isDisposed;

		// Token: 0x04001425 RID: 5157
		private readonly object _disposableLock = new object();

		// Token: 0x04001426 RID: 5158
		private bool _warnNotTerminal;
	}
}
