using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management.Automation.Remoting.Internal;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x02000290 RID: 656
	internal sealed class ThrottlingJob : Job
	{
		// Token: 0x06001F69 RID: 8041 RVA: 0x000B5D6C File Offset: 0x000B3F6C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this.StopJob();
					List<Job> list;
					lock (this._lockObject)
					{
						list = new List<Job>(base.ChildJobs);
						base.ChildJobs.Clear();
					}
					foreach (Job job in list)
					{
						job.Dispose();
					}
					if (this._jobResultsThrottlingSemaphore != null)
					{
						this._jobResultsThrottlingSemaphore.Dispose();
					}
					this._cancellationTokenSource.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x000B5E3C File Offset: 0x000B403C
		internal int GetProgressActivityId()
		{
			int progressActivityId;
			lock (this._progressLock)
			{
				if (this._progressReportLastTime.Equals(DateTime.MinValue))
				{
					try
					{
						this.ReportProgress(false);
					}
					catch (PSInvalidOperationException)
					{
						return -1;
					}
				}
				progressActivityId = this._progressActivityId;
			}
			return progressActivityId;
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x000B5EAC File Offset: 0x000B40AC
		private void ReportProgress(bool minimizeFrequentUpdates)
		{
			lock (this._progressLock)
			{
				DateTime utcNow = DateTime.UtcNow;
				if (minimizeFrequentUpdates)
				{
					if (utcNow - this._progressStartTime < TimeSpan.FromSeconds(1.0))
					{
						return;
					}
					if (!this._progressReportLastTime.Equals(DateTime.MinValue) && utcNow - this._progressReportLastTime < TimeSpan.FromMilliseconds(200.0))
					{
						return;
					}
				}
				this._progressReportLastTime = utcNow;
				double num;
				double num2;
				lock (this._lockObject)
				{
					num = (double)this._countOfAllChildJobs;
					num2 = (double)this.CountOfFinishedChildJobs;
				}
				int num3;
				if (num >= 1.0)
				{
					num3 = (int)(100.0 * num2 / num);
				}
				else
				{
					num3 = -1;
				}
				num3 = Math.Max(-1, Math.Min(100, num3));
				ProgressRecord progressRecord = new ProgressRecord(this._progressActivityId, base.Command, this.StatusMessage);
				if (this.IsThrottlingJobCompleted)
				{
					if (this._progressReportLastTime.Equals(DateTime.MinValue))
					{
						return;
					}
					progressRecord.RecordType = ProgressRecordType.Completed;
					progressRecord.PercentComplete = 100;
					progressRecord.SecondsRemaining = 0;
				}
				else
				{
					progressRecord.RecordType = ProgressRecordType.Processing;
					progressRecord.PercentComplete = num3;
					int? num4 = null;
					if (num3 >= 0)
					{
						num4 = ProgressRecord.GetSecondsRemaining(this._progressStartTime, (double)num3 / 100.0);
					}
					if (num4 != null)
					{
						progressRecord.SecondsRemaining = num4.Value;
					}
				}
				this.WriteProgress(progressRecord);
			}
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06001F6C RID: 8044 RVA: 0x000B6080 File Offset: 0x000B4280
		private bool IsEndOfChildJobs
		{
			get
			{
				bool result;
				lock (this._lockObject)
				{
					result = (this._isStopping || (this._ownerWontSubmitNewChildJobs && this._setOfChildJobsThatCanAddMoreChildJobs.Count == 0));
				}
				return result;
			}
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06001F6D RID: 8045 RVA: 0x000B60E0 File Offset: 0x000B42E0
		private bool IsThrottlingJobCompleted
		{
			get
			{
				bool result;
				lock (this._lockObject)
				{
					result = (this.IsEndOfChildJobs && this._countOfAllChildJobs <= this.CountOfFinishedChildJobs);
				}
				return result;
			}
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x06001F6E RID: 8046 RVA: 0x000B6138 File Offset: 0x000B4338
		private int CountOfFinishedChildJobs
		{
			get
			{
				int result;
				lock (this._lockObject)
				{
					result = this._countOfFailedChildJobs + this._countOfStoppedChildJobs + this._countOfSuccessfullyCompletedChildJobs;
				}
				return result;
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06001F6F RID: 8047 RVA: 0x000B6188 File Offset: 0x000B4388
		private int CountOfRunningOrReadyToRunChildJobs
		{
			get
			{
				int result;
				lock (this._lockObject)
				{
					result = this._countOfAllChildJobs - this.CountOfFinishedChildJobs;
				}
				return result;
			}
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x000B61D4 File Offset: 0x000B43D4
		internal ThrottlingJob(string command, string jobName, string jobTypeName, int maximumConcurrentChildJobs, bool cmdletMode) : base(command, jobName)
		{
			base.Results.BlockingEnumerator = true;
			this._cmdletMode = cmdletMode;
			base.PSJobTypeName = jobTypeName;
			if (this._cmdletMode)
			{
				this._jobResultsThrottlingSemaphore = new SemaphoreSlim(ThrottlingJob.ForwardingHelper.AggregationQueueMaxCapacity);
			}
			this._progressActivityId = new Random(this.GetHashCode()).Next();
			this.SetupThrottlingQueue(maximumConcurrentChildJobs);
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x000B6298 File Offset: 0x000B4498
		internal void AddChildJobAndPotentiallyBlock(StartableJob childJob, ThrottlingJob.ChildJobFlags flags)
		{
			using (ManualResetEventSlim manualResetEventSlim = new ManualResetEventSlim(false))
			{
				if (childJob == null)
				{
					throw new ArgumentNullException("childJob");
				}
				this.AddChildJobWithoutBlocking(childJob, flags, new Action(manualResetEventSlim.Set));
				manualResetEventSlim.Wait();
			}
		}

		// Token: 0x06001F72 RID: 8050 RVA: 0x000B62F0 File Offset: 0x000B44F0
		internal void AddChildJobAndPotentiallyBlock(Cmdlet cmdlet, StartableJob childJob, ThrottlingJob.ChildJobFlags flags)
		{
			using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
			{
				if (childJob == null)
				{
					throw new ArgumentNullException("childJob");
				}
				this.AddChildJobWithoutBlocking(childJob, flags, new Action(cancellationTokenSource.Cancel));
				this.ForwardAllResultsToCmdlet(cmdlet, new CancellationToken?(cancellationTokenSource.Token));
			}
		}

		// Token: 0x06001F73 RID: 8051 RVA: 0x000B6354 File Offset: 0x000B4554
		internal void DisableFlowControlForPendingJobsQueue()
		{
			if (!this._cmdletMode || this._alreadyDisabledFlowControlForPendingJobsQueue)
			{
				return;
			}
			this._alreadyDisabledFlowControlForPendingJobsQueue = true;
			lock (this._lockObject)
			{
				this._maxReadyToRunJobs = int.MaxValue;
				while (this._actionsForUnblockingChildAdditions.Count > 0)
				{
					Action action = this._actionsForUnblockingChildAdditions.Dequeue();
					if (action != null)
					{
						action();
					}
				}
			}
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x000B63D8 File Offset: 0x000B45D8
		internal void DisableFlowControlForPendingCmdletActionsQueue()
		{
			if (!this._cmdletMode || this._alreadyDisabledFlowControlForPendingCmdletActionsQueue)
			{
				return;
			}
			this._alreadyDisabledFlowControlForPendingCmdletActionsQueue = true;
			long num = 1073741823L - (long)this._jobResultsThrottlingSemaphore.CurrentCount;
			if (num > 0L && num < 2147483647L)
			{
				this._jobResultsThrottlingSemaphore.Release((int)num);
			}
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x000B6430 File Offset: 0x000B4630
		internal void AddChildJobWithoutBlocking(StartableJob childJob, ThrottlingJob.ChildJobFlags flags, Action jobEnqueuedAction = null)
		{
			if (childJob == null)
			{
				throw new ArgumentNullException("childJob");
			}
			if (childJob.JobStateInfo.State != JobState.NotStarted)
			{
				throw new ArgumentException(RemotingErrorIdStrings.ThrottlingJobChildAlreadyRunning, "childJob");
			}
			base.AssertNotDisposed();
			JobStateInfo jobStateInfo = null;
			lock (this._lockObject)
			{
				if (this.IsEndOfChildJobs)
				{
					throw new InvalidOperationException(RemotingErrorIdStrings.ThrottlingJobChildAddedAfterEndOfChildJobs);
				}
				if (this._isStopping)
				{
					return;
				}
				if (this._countOfAllChildJobs == 0)
				{
					jobStateInfo = new JobStateInfo(JobState.Running);
				}
				if (ThrottlingJob.ChildJobFlags.CreatesChildJobs == (ThrottlingJob.ChildJobFlags.CreatesChildJobs & flags))
				{
					this._setOfChildJobsThatCanAddMoreChildJobs.Add(childJob.InstanceId);
				}
				base.ChildJobs.Add(childJob);
				this._childJobLocations.Add(childJob.Location);
				this._countOfAllChildJobs++;
				this.WriteWarningAboutHighUsageOfFlowControlBuffers((long)this.CountOfRunningOrReadyToRunChildJobs);
				if (this.CountOfRunningOrReadyToRunChildJobs > this._maxReadyToRunJobs)
				{
					this._actionsForUnblockingChildAdditions.Enqueue(jobEnqueuedAction);
				}
				else if (jobEnqueuedAction != null)
				{
					jobEnqueuedAction();
				}
			}
			if (jobStateInfo != null)
			{
				base.SetJobState(jobStateInfo.State, jobStateInfo.Reason);
			}
			this.ChildJobAdded.SafeInvoke(this, new ThrottlingJobChildAddedEventArgs(childJob));
			childJob.SetParentActivityIdGetter(new Func<int>(this.GetProgressActivityId));
			childJob.StateChanged += this.childJob_StateChanged;
			if (this._cmdletMode)
			{
				childJob.Results.DataAdded += this.childJob_ResultsAdded;
			}
			this.EnqueueReadyToRunChildJob(childJob);
			this.ReportProgress(true);
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x000B65BC File Offset: 0x000B47BC
		private void childJob_ResultsAdded(object sender, DataAddedEventArgs e)
		{
			try
			{
				long currentCount = Interlocked.Increment(ref this._jobResultsCurrentCount);
				this.WriteWarningAboutHighUsageOfFlowControlBuffers(currentCount);
				this._jobResultsThrottlingSemaphore.Wait(this._cancellationTokenSource.Token);
			}
			catch (ObjectDisposedException)
			{
			}
			catch (OperationCanceledException)
			{
			}
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x000B6618 File Offset: 0x000B4818
		private void WriteWarningAboutHighUsageOfFlowControlBuffers(long currentCount)
		{
			if (!this._cmdletMode)
			{
				return;
			}
			if (currentCount < 30000L)
			{
				return;
			}
			lock (this._alreadyWroteFlowControlBuffersHighMemoryUsageWarningLock)
			{
				if (this._alreadyWroteFlowControlBuffersHighMemoryUsageWarning)
				{
					return;
				}
				this._alreadyWroteFlowControlBuffersHighMemoryUsageWarning = true;
			}
			string message = string.Format(CultureInfo.InvariantCulture, RemotingErrorIdStrings.ThrottlingJobFlowControlMemoryWarning, new object[]
			{
				base.Command
			});
			this.WriteWarning(message);
		}

		// Token: 0x1400003E RID: 62
		// (add) Token: 0x06001F78 RID: 8056 RVA: 0x000B66A0 File Offset: 0x000B48A0
		// (remove) Token: 0x06001F79 RID: 8057 RVA: 0x000B66D8 File Offset: 0x000B48D8
		internal event EventHandler<ThrottlingJobChildAddedEventArgs> ChildJobAdded;

		// Token: 0x06001F7A RID: 8058 RVA: 0x000B6710 File Offset: 0x000B4910
		private void SetupThrottlingQueue(int maximumConcurrentChildJobs)
		{
			this._maximumConcurrentChildJobs = ((maximumConcurrentChildJobs > 0) ? maximumConcurrentChildJobs : int.MaxValue);
			if (this._cmdletMode)
			{
				this._maxReadyToRunJobs = ThrottlingJob.MaximumReadyToRunJobs;
			}
			else
			{
				this._maxReadyToRunJobs = int.MaxValue;
			}
			this._extraCapacityForRunningAllJobs = this._maximumConcurrentChildJobs;
			this._extraCapacityForRunningQueryJobs = Math.Max(1, this._extraCapacityForRunningAllJobs / 2);
			this._inBoostModeToPreventQueryJobDeadlock = false;
			this._readyToRunQueryJobs = new Queue<StartableJob>();
			this._readyToRunRegularJobs = new Queue<StartableJob>();
			this._actionsForUnblockingChildAdditions = new Queue<Action>();
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x000B6798 File Offset: 0x000B4998
		private void StartChildJobIfPossible()
		{
			StartableJob startableJob = null;
			lock (this._lockObject)
			{
				if (this._readyToRunQueryJobs.Count > 0 && this._extraCapacityForRunningQueryJobs > 0 && this._extraCapacityForRunningAllJobs > 0)
				{
					this._extraCapacityForRunningQueryJobs--;
					this._extraCapacityForRunningAllJobs--;
					startableJob = this._readyToRunQueryJobs.Dequeue();
				}
				else if (this._readyToRunRegularJobs.Count > 0 && this._extraCapacityForRunningAllJobs > 0)
				{
					this._extraCapacityForRunningAllJobs--;
					startableJob = this._readyToRunRegularJobs.Dequeue();
				}
			}
			if (startableJob != null)
			{
				startableJob.StartJob();
			}
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x000B6858 File Offset: 0x000B4A58
		private void EnqueueReadyToRunChildJob(StartableJob childJob)
		{
			lock (this._lockObject)
			{
				bool flag2 = this._setOfChildJobsThatCanAddMoreChildJobs.Contains(childJob.InstanceId);
				if (flag2 && !this._inBoostModeToPreventQueryJobDeadlock && this._maximumConcurrentChildJobs == 1)
				{
					this._inBoostModeToPreventQueryJobDeadlock = true;
					this._extraCapacityForRunningAllJobs++;
				}
				if (flag2)
				{
					this._readyToRunQueryJobs.Enqueue(childJob);
				}
				else
				{
					this._readyToRunRegularJobs.Enqueue(childJob);
				}
			}
			this.StartChildJobIfPossible();
		}

		// Token: 0x06001F7D RID: 8061 RVA: 0x000B68F0 File Offset: 0x000B4AF0
		private void MakeRoomForRunningOtherJobs(Job completedChildJob)
		{
			lock (this._lockObject)
			{
				this._extraCapacityForRunningAllJobs++;
				bool flag2 = this._setOfChildJobsThatCanAddMoreChildJobs.Contains(completedChildJob.InstanceId);
				if (flag2)
				{
					this._setOfChildJobsThatCanAddMoreChildJobs.Remove(completedChildJob.InstanceId);
					this._extraCapacityForRunningQueryJobs++;
					if (this._inBoostModeToPreventQueryJobDeadlock && this._setOfChildJobsThatCanAddMoreChildJobs.Count == 0)
					{
						this._inBoostModeToPreventQueryJobDeadlock = false;
						this._extraCapacityForRunningAllJobs--;
					}
				}
			}
			this.StartChildJobIfPossible();
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x000B69A0 File Offset: 0x000B4BA0
		private void FigureOutIfThrottlingJobIsCompleted()
		{
			JobStateInfo jobStateInfo = null;
			lock (this._lockObject)
			{
				if (this.IsThrottlingJobCompleted && !base.IsFinishedState(base.JobStateInfo.State))
				{
					if (this._isStopping)
					{
						jobStateInfo = new JobStateInfo(JobState.Stopped, null);
					}
					else if (this._countOfFailedChildJobs > 0)
					{
						jobStateInfo = new JobStateInfo(JobState.Failed, null);
					}
					else if (this._countOfStoppedChildJobs > 0)
					{
						jobStateInfo = new JobStateInfo(JobState.Stopped, null);
					}
					else
					{
						jobStateInfo = new JobStateInfo(JobState.Completed);
					}
				}
			}
			if (jobStateInfo != null)
			{
				base.SetJobState(jobStateInfo.State, jobStateInfo.Reason);
				base.CloseAllStreams();
			}
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x000B6A54 File Offset: 0x000B4C54
		internal void EndOfChildJobs()
		{
			base.AssertNotDisposed();
			lock (this._lockObject)
			{
				this._ownerWontSubmitNewChildJobs = true;
			}
			this.FigureOutIfThrottlingJobIsCompleted();
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x000B6AA4 File Offset: 0x000B4CA4
		public override void StopJob()
		{
			List<Job> list = null;
			lock (this._lockObject)
			{
				if (!this._isStopping && !this.IsThrottlingJobCompleted)
				{
					this._isStopping = true;
					list = this.GetChildJobsSnapshot();
				}
			}
			if (list != null)
			{
				base.SetJobState(JobState.Stopping);
				this._cancellationTokenSource.Cancel();
				foreach (Job job in list)
				{
					if (!job.IsFinishedState(job.JobStateInfo.State))
					{
						job.StopJob();
					}
				}
				this.FigureOutIfThrottlingJobIsCompleted();
			}
			base.Finished.WaitOne();
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x000B6B78 File Offset: 0x000B4D78
		private void childJob_StateChanged(object sender, JobStateEventArgs e)
		{
			Job job = (Job)sender;
			if (e.PreviousJobStateInfo.State == JobState.Blocked && e.JobStateInfo.State != JobState.Blocked)
			{
				bool flag = false;
				lock (this._lockObject)
				{
					this._countOfBlockedChildJobs--;
					if (this._countOfBlockedChildJobs == 0)
					{
						flag = true;
					}
				}
				if (flag)
				{
					base.SetJobState(JobState.Running);
				}
			}
			switch (e.JobStateInfo.State)
			{
			case JobState.Completed:
			case JobState.Failed:
			case JobState.Stopped:
				job.StateChanged -= this.childJob_StateChanged;
				this.MakeRoomForRunningOtherJobs(job);
				lock (this._lockObject)
				{
					if (e.JobStateInfo.State == JobState.Failed)
					{
						this._countOfFailedChildJobs++;
					}
					else if (e.JobStateInfo.State == JobState.Stopped)
					{
						this._countOfStoppedChildJobs++;
					}
					else if (e.JobStateInfo.State == JobState.Completed)
					{
						this._countOfSuccessfullyCompletedChildJobs++;
					}
					if (this._actionsForUnblockingChildAdditions.Count > 0)
					{
						Action action = this._actionsForUnblockingChildAdditions.Dequeue();
						if (action != null)
						{
							action();
						}
					}
					if (this._cmdletMode)
					{
						foreach (PSStreamObject item in job.Results.ReadAll())
						{
							base.Results.Add(item);
						}
						base.ChildJobs.Remove(job);
						this._setOfChildJobsThatCanAddMoreChildJobs.Remove(job.InstanceId);
						job.Dispose();
					}
				}
				this.ReportProgress(!this.IsThrottlingJobCompleted);
				break;
			case JobState.Blocked:
				lock (this._lockObject)
				{
					this._countOfBlockedChildJobs++;
				}
				base.SetJobState(JobState.Blocked);
				break;
			}
			this.FigureOutIfThrottlingJobIsCompleted();
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x000B6DC0 File Offset: 0x000B4FC0
		private List<Job> GetChildJobsSnapshot()
		{
			List<Job> result;
			lock (this._lockObject)
			{
				result = new List<Job>(base.ChildJobs);
			}
			return result;
		}

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06001F83 RID: 8067 RVA: 0x000B6E10 File Offset: 0x000B5010
		public override bool HasMoreData
		{
			get
			{
				return this.GetChildJobsSnapshot().Any((Job childJob) => childJob.HasMoreData) || base.Results.Count != 0;
			}
		}

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06001F84 RID: 8068 RVA: 0x000B6E50 File Offset: 0x000B5050
		public override string Location
		{
			get
			{
				string result;
				lock (this._lockObject)
				{
					result = string.Join(", ", this._childJobLocations);
				}
				return result;
			}
		}

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06001F85 RID: 8069 RVA: 0x000B6E9C File Offset: 0x000B509C
		public override string StatusMessage
		{
			get
			{
				int countOfFinishedChildJobs;
				int countOfAllChildJobs;
				lock (this._lockObject)
				{
					countOfFinishedChildJobs = this.CountOfFinishedChildJobs;
					countOfAllChildJobs = this._countOfAllChildJobs;
				}
				string text = countOfAllChildJobs.ToString(CultureInfo.CurrentCulture);
				if (!this.IsEndOfChildJobs)
				{
					text += "+";
				}
				return string.Format(CultureInfo.CurrentUICulture, RemotingErrorIdStrings.ThrottlingJobStatusMessage, new object[]
				{
					countOfFinishedChildJobs,
					text
				});
			}
		}

		// Token: 0x06001F86 RID: 8070 RVA: 0x000B6F30 File Offset: 0x000B5130
		internal override void ForwardAvailableResultsToCmdlet(Cmdlet cmdlet)
		{
			base.AssertNotDisposed();
			base.ForwardAvailableResultsToCmdlet(cmdlet);
			foreach (Job job in this.GetChildJobsSnapshot())
			{
				job.ForwardAvailableResultsToCmdlet(cmdlet);
			}
		}

		// Token: 0x06001F87 RID: 8071 RVA: 0x000B6F90 File Offset: 0x000B5190
		internal override void ForwardAllResultsToCmdlet(Cmdlet cmdlet)
		{
			this.ForwardAllResultsToCmdlet(cmdlet, null);
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x000B6FAD File Offset: 0x000B51AD
		private void ForwardAllResultsToCmdlet(Cmdlet cmdlet, CancellationToken? cancellationToken)
		{
			base.AssertNotDisposed();
			ThrottlingJob.ForwardingHelper.ForwardAllResultsToCmdlet(this, cmdlet, cancellationToken);
		}

		// Token: 0x04000DD5 RID: 3541
		private const long FlowControlBuffersHighMemoryUsageThreshold = 30000L;

		// Token: 0x04000DD6 RID: 3542
		private readonly DateTime _progressStartTime = DateTime.UtcNow;

		// Token: 0x04000DD7 RID: 3543
		private readonly int _progressActivityId;

		// Token: 0x04000DD8 RID: 3544
		private readonly object _progressLock = new object();

		// Token: 0x04000DD9 RID: 3545
		private DateTime _progressReportLastTime = DateTime.MinValue;

		// Token: 0x04000DDA RID: 3546
		private bool _ownerWontSubmitNewChildJobs;

		// Token: 0x04000DDB RID: 3547
		private readonly HashSet<Guid> _setOfChildJobsThatCanAddMoreChildJobs = new HashSet<Guid>();

		// Token: 0x04000DDC RID: 3548
		private readonly bool _cmdletMode;

		// Token: 0x04000DDD RID: 3549
		private int _countOfAllChildJobs;

		// Token: 0x04000DDE RID: 3550
		private int _countOfBlockedChildJobs;

		// Token: 0x04000DDF RID: 3551
		private int _countOfFailedChildJobs;

		// Token: 0x04000DE0 RID: 3552
		private int _countOfStoppedChildJobs;

		// Token: 0x04000DE1 RID: 3553
		private int _countOfSuccessfullyCompletedChildJobs;

		// Token: 0x04000DE2 RID: 3554
		private readonly object _lockObject = new object();

		// Token: 0x04000DE3 RID: 3555
		private bool _alreadyDisabledFlowControlForPendingJobsQueue;

		// Token: 0x04000DE4 RID: 3556
		private bool _alreadyDisabledFlowControlForPendingCmdletActionsQueue;

		// Token: 0x04000DE5 RID: 3557
		private readonly object _alreadyWroteFlowControlBuffersHighMemoryUsageWarningLock = new object();

		// Token: 0x04000DE6 RID: 3558
		private bool _alreadyWroteFlowControlBuffersHighMemoryUsageWarning;

		// Token: 0x04000DE8 RID: 3560
		private int _maximumConcurrentChildJobs;

		// Token: 0x04000DE9 RID: 3561
		private int _extraCapacityForRunningQueryJobs;

		// Token: 0x04000DEA RID: 3562
		private int _extraCapacityForRunningAllJobs;

		// Token: 0x04000DEB RID: 3563
		private bool _inBoostModeToPreventQueryJobDeadlock;

		// Token: 0x04000DEC RID: 3564
		private Queue<StartableJob> _readyToRunQueryJobs;

		// Token: 0x04000DED RID: 3565
		private Queue<StartableJob> _readyToRunRegularJobs;

		// Token: 0x04000DEE RID: 3566
		private Queue<Action> _actionsForUnblockingChildAdditions;

		// Token: 0x04000DEF RID: 3567
		private int _maxReadyToRunJobs;

		// Token: 0x04000DF0 RID: 3568
		private readonly SemaphoreSlim _jobResultsThrottlingSemaphore;

		// Token: 0x04000DF1 RID: 3569
		private long _jobResultsCurrentCount;

		// Token: 0x04000DF2 RID: 3570
		private static readonly int MaximumReadyToRunJobs = 10000;

		// Token: 0x04000DF3 RID: 3571
		private bool _isStopping;

		// Token: 0x04000DF4 RID: 3572
		private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

		// Token: 0x04000DF5 RID: 3573
		private readonly HashSet<string> _childJobLocations = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x02000291 RID: 657
		[Flags]
		internal enum ChildJobFlags
		{
			// Token: 0x04000DF8 RID: 3576
			None = 0,
			// Token: 0x04000DF9 RID: 3577
			CreatesChildJobs = 1
		}

		// Token: 0x02000292 RID: 658
		private class ForwardingHelper : IDisposable
		{
			// Token: 0x06001F8B RID: 8075 RVA: 0x000B6FC9 File Offset: 0x000B51C9
			private ForwardingHelper(ThrottlingJob throttlingJob)
			{
				this._throttlingJob = throttlingJob;
				this._myLock = new object();
				this._monitoredJobs = new HashSet<Job>();
				this._aggregatedResults = new BlockingCollection<PSStreamObject>();
			}

			// Token: 0x06001F8C RID: 8076 RVA: 0x000B7004 File Offset: 0x000B5204
			private void StartMonitoringJob(Job job)
			{
				lock (this._myLock)
				{
					if (this._disposed || this._stoppedMonitoringAllJobs)
					{
						return;
					}
					if (this._monitoredJobs.Contains(job))
					{
						return;
					}
					this._monitoredJobs.Add(job);
					job.Results.DataAdded += this.MonitoredJobResults_DataAdded;
					job.StateChanged += this.MonitoredJob_StateChanged;
				}
				this.AggregateJobResults(job.Results);
				this.CheckIfMonitoredJobIsComplete(job);
			}

			// Token: 0x06001F8D RID: 8077 RVA: 0x000B70AC File Offset: 0x000B52AC
			private void StopMonitoringJob(Job job)
			{
				lock (this._myLock)
				{
					if (this._monitoredJobs.Contains(job))
					{
						job.Results.DataAdded -= this.MonitoredJobResults_DataAdded;
						job.StateChanged -= this.MonitoredJob_StateChanged;
						this._monitoredJobs.Remove(job);
					}
				}
			}

			// Token: 0x06001F8E RID: 8078 RVA: 0x000B712C File Offset: 0x000B532C
			private void AggregateJobResults(PSDataCollection<PSStreamObject> resultsCollection)
			{
				lock (this._myLock)
				{
					if (this._disposed || this._stoppedMonitoringAllJobs || this._aggregatedResults.IsAddingCompleted || this._cancellationTokenSource.IsCancellationRequested)
					{
						return;
					}
				}
				foreach (PSStreamObject item in resultsCollection.ReadAll())
				{
					bool flag2 = false;
					try
					{
						lock (this._myLock)
						{
							if (!this._disposed && !this._stoppedMonitoringAllJobs && !this._aggregatedResults.IsAddingCompleted && !this._cancellationTokenSource.IsCancellationRequested)
							{
								this._aggregatedResults.Add(item, this._cancellationTokenSource.Token);
								flag2 = true;
							}
						}
					}
					catch (Exception)
					{
					}
					if (!flag2)
					{
						this.StopMonitoringJob(this._throttlingJob);
						try
						{
							this._throttlingJob.Results.Add(item);
						}
						catch (InvalidOperationException)
						{
						}
					}
				}
			}

			// Token: 0x06001F8F RID: 8079 RVA: 0x000B728C File Offset: 0x000B548C
			private void CancelForwarding()
			{
				this._cancellationTokenSource.Cancel();
				lock (this._myLock)
				{
					this._aggregatedResults.CompleteAdding();
				}
			}

			// Token: 0x06001F90 RID: 8080 RVA: 0x000B72DC File Offset: 0x000B54DC
			private void CheckIfMonitoredJobIsComplete(Job job)
			{
				this.CheckIfMonitoredJobIsComplete(job, job.JobStateInfo.State);
			}

			// Token: 0x06001F91 RID: 8081 RVA: 0x000B72F0 File Offset: 0x000B54F0
			private void CheckIfMonitoredJobIsComplete(Job job, JobState jobState)
			{
				if (job.IsFinishedState(jobState))
				{
					lock (this._myLock)
					{
						this.StopMonitoringJob(job);
					}
				}
			}

			// Token: 0x06001F92 RID: 8082 RVA: 0x000B733C File Offset: 0x000B553C
			private void CheckIfThrottlingJobIsComplete()
			{
				if (this._throttlingJob.IsThrottlingJobCompleted)
				{
					List<PSDataCollection<PSStreamObject>> list = new List<PSDataCollection<PSStreamObject>>();
					lock (this._myLock)
					{
						foreach (Job job in this._monitoredJobs)
						{
							list.Add(job.Results);
						}
						foreach (Job job2 in this._throttlingJob.GetChildJobsSnapshot())
						{
							list.Add(job2.Results);
						}
						list.Add(this._throttlingJob.Results);
					}
					foreach (PSDataCollection<PSStreamObject> resultsCollection in list)
					{
						this.AggregateJobResults(resultsCollection);
					}
					lock (this._myLock)
					{
						if (!this._disposed && !this._aggregatedResults.IsAddingCompleted)
						{
							this._aggregatedResults.CompleteAdding();
						}
					}
				}
			}

			// Token: 0x06001F93 RID: 8083 RVA: 0x000B74C4 File Offset: 0x000B56C4
			private void MonitoredJobResults_DataAdded(object sender, DataAddedEventArgs e)
			{
				PSDataCollection<PSStreamObject> resultsCollection = (PSDataCollection<PSStreamObject>)sender;
				this.AggregateJobResults(resultsCollection);
			}

			// Token: 0x06001F94 RID: 8084 RVA: 0x000B74E0 File Offset: 0x000B56E0
			private void MonitoredJob_StateChanged(object sender, JobStateEventArgs e)
			{
				Job job = (Job)sender;
				this.CheckIfMonitoredJobIsComplete(job, e.JobStateInfo.State);
			}

			// Token: 0x06001F95 RID: 8085 RVA: 0x000B7506 File Offset: 0x000B5706
			private void ThrottlingJob_ChildJobAdded(object sender, ThrottlingJobChildAddedEventArgs e)
			{
				this.StartMonitoringJob(e.AddedChildJob);
			}

			// Token: 0x06001F96 RID: 8086 RVA: 0x000B7514 File Offset: 0x000B5714
			private void ThrottlingJob_StateChanged(object sender, JobStateEventArgs e)
			{
				this.CheckIfThrottlingJobIsComplete();
			}

			// Token: 0x06001F97 RID: 8087 RVA: 0x000B751C File Offset: 0x000B571C
			private void AttemptToPreserveAggregatedResults()
			{
				bool flag = false;
				foreach (PSStreamObject item in ((IEnumerable<PSStreamObject>)this._aggregatedResults))
				{
					if (!flag)
					{
						try
						{
							this._throttlingJob.Results.Add(item);
						}
						catch (PSInvalidOperationException)
						{
							flag = this._throttlingJob.IsFinishedState(this._throttlingJob.JobStateInfo.State);
						}
					}
				}
			}

			// Token: 0x06001F98 RID: 8088 RVA: 0x000B75A8 File Offset: 0x000B57A8
			private void ForwardResults(Cmdlet cmdlet)
			{
				try
				{
					foreach (PSStreamObject psstreamObject in this._aggregatedResults.GetConsumingEnumerable(this._throttlingJob._cancellationTokenSource.Token))
					{
						if (psstreamObject != null)
						{
							try
							{
								psstreamObject.WriteStreamObject(cmdlet, false);
							}
							finally
							{
								if (this._throttlingJob._cmdletMode)
								{
									Interlocked.Decrement(ref this._throttlingJob._jobResultsCurrentCount);
									this._throttlingJob._jobResultsThrottlingSemaphore.Release();
								}
							}
						}
					}
				}
				catch
				{
					this.StopMonitoringAllJobs();
					this.AttemptToPreserveAggregatedResults();
					throw;
				}
			}

			// Token: 0x06001F99 RID: 8089 RVA: 0x000B766C File Offset: 0x000B586C
			private void StopMonitoringAllJobs()
			{
				this._cancellationTokenSource.Cancel();
				lock (this._myLock)
				{
					this._stoppedMonitoringAllJobs = true;
					List<Job> list = this._monitoredJobs.ToList<Job>();
					foreach (Job job in list)
					{
						this.StopMonitoringJob(job);
					}
					if (!this._disposed && !this._aggregatedResults.IsAddingCompleted)
					{
						this._aggregatedResults.CompleteAdding();
					}
				}
			}

			// Token: 0x06001F9A RID: 8090 RVA: 0x000B7724 File Offset: 0x000B5924
			public void Dispose()
			{
				GC.SuppressFinalize(this);
				this._cancellationTokenSource.Cancel();
				lock (this._myLock)
				{
					if (!this._disposed)
					{
						this.StopMonitoringAllJobs();
						this._aggregatedResults.Dispose();
						this._cancellationTokenSource.Dispose();
						this._disposed = true;
					}
				}
			}

			// Token: 0x06001F9B RID: 8091 RVA: 0x000B7830 File Offset: 0x000B5A30
			public static void ForwardAllResultsToCmdlet(ThrottlingJob throttlingJob, Cmdlet cmdlet, CancellationToken? cancellationToken)
			{
				ThrottlingJob.ForwardingHelper.<>c__DisplayClass24 CS$<>8__locals1 = new ThrottlingJob.ForwardingHelper.<>c__DisplayClass24();
				CS$<>8__locals1.throttlingJob = throttlingJob;
				using (ThrottlingJob.ForwardingHelper helper = new ThrottlingJob.ForwardingHelper(CS$<>8__locals1.throttlingJob))
				{
					try
					{
						CS$<>8__locals1.throttlingJob.ChildJobAdded += helper.ThrottlingJob_ChildJobAdded;
						try
						{
							CS$<>8__locals1.throttlingJob.StateChanged += helper.ThrottlingJob_StateChanged;
							IDisposable disposable = null;
							if (cancellationToken != null)
							{
								disposable = cancellationToken.Value.Register(new Action(helper.CancelForwarding));
							}
							try
							{
								Interlocked.MemoryBarrier();
								ThreadPool.QueueUserWorkItem(delegate(object param0)
								{
									helper.StartMonitoringJob(CS$<>8__locals1.throttlingJob);
									foreach (Job job in CS$<>8__locals1.throttlingJob.GetChildJobsSnapshot())
									{
										helper.StartMonitoringJob(job);
									}
									helper.CheckIfThrottlingJobIsComplete();
								});
								helper.ForwardResults(cmdlet);
							}
							finally
							{
								if (disposable != null)
								{
									disposable.Dispose();
								}
							}
						}
						finally
						{
							CS$<>8__locals1.throttlingJob.StateChanged -= helper.ThrottlingJob_StateChanged;
						}
					}
					finally
					{
						CS$<>8__locals1.throttlingJob.ChildJobAdded -= helper.ThrottlingJob_ChildJobAdded;
					}
				}
			}

			// Token: 0x04000DFA RID: 3578
			internal static readonly int AggregationQueueMaxCapacity = 10000;

			// Token: 0x04000DFB RID: 3579
			private readonly ThrottlingJob _throttlingJob;

			// Token: 0x04000DFC RID: 3580
			private readonly object _myLock;

			// Token: 0x04000DFD RID: 3581
			private readonly BlockingCollection<PSStreamObject> _aggregatedResults;

			// Token: 0x04000DFE RID: 3582
			private readonly HashSet<Job> _monitoredJobs;

			// Token: 0x04000DFF RID: 3583
			private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

			// Token: 0x04000E00 RID: 3584
			private bool _disposed;

			// Token: 0x04000E01 RID: 3585
			private bool _stoppedMonitoringAllJobs;
		}
	}
}
