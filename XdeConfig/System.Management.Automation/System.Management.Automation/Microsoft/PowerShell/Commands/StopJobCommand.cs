using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Remoting;
using System.Threading;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000343 RID: 835
	[Cmdlet("Stop", "Job", SupportsShouldProcess = true, DefaultParameterSetName = "SessionIdParameterSet", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=113413")]
	[OutputType(new Type[]
	{
		typeof(Job)
	})]
	public class StopJobCommand : JobCmdletBase, IDisposable
	{
		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x060028BB RID: 10427 RVA: 0x000E3894 File Offset: 0x000E1A94
		// (set) Token: 0x060028BC RID: 10428 RVA: 0x000E389C File Offset: 0x000E1A9C
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

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x060028BD RID: 10429 RVA: 0x000E38A5 File Offset: 0x000E1AA5
		// (set) Token: 0x060028BE RID: 10430 RVA: 0x000E38B2 File Offset: 0x000E1AB2
		[Parameter]
		public SwitchParameter PassThru
		{
			get
			{
				return this.passThru;
			}
			set
			{
				this.passThru = value;
			}
		}

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x060028BF RID: 10431 RVA: 0x000E38C0 File Offset: 0x000E1AC0
		public override string[] Command
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060028C0 RID: 10432 RVA: 0x000E38D8 File Offset: 0x000E1AD8
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
			this._allJobsToStop.AddRange(list);
			foreach (Job job in list)
			{
				if (base.Stopping)
				{
					break;
				}
				if (!job.IsFinishedState(job.JobStateInfo.State))
				{
					string target = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.RemovePSJobWhatIfTarget, new object[]
					{
						job.Command,
						job.Id
					});
					if (base.ShouldProcess(target, "Stop"))
					{
						Job2 job2 = job as Job2;
						if (job2 != null)
						{
							this._cleanUpActions.Add(job2, new EventHandler<AsyncCompletedEventArgs>(this.HandleStopJobCompleted));
							job2.StopJobCompleted += this.HandleStopJobCompleted;
							lock (this._syncObject)
							{
								if (!job2.IsFinishedState(job2.JobStateInfo.State) && !this._pendingJobs.Contains(job2.InstanceId))
								{
									this._pendingJobs.Add(job2.InstanceId);
								}
							}
							job2.StopJobAsync();
						}
						else
						{
							job.StopJob();
							ContainerParentJob containerParentJob = job as ContainerParentJob;
							if (containerParentJob != null && containerParentJob.ExecutionError.Count > 0)
							{
								foreach (ErrorRecord errorRecord in from e in containerParentJob.ExecutionError
								where e.FullyQualifiedErrorId == "ContainerParentJobStopError"
								select e)
								{
									base.WriteError(errorRecord);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060028C1 RID: 10433 RVA: 0x000E3B88 File Offset: 0x000E1D88
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
			foreach (ErrorRecord errorRecord in this._errorsToWrite)
			{
				base.WriteError(errorRecord);
			}
			if (this.passThru)
			{
				foreach (Job sendToPipeline in this._allJobsToStop)
				{
					base.WriteObject(sendToPipeline);
				}
			}
		}

		// Token: 0x060028C2 RID: 10434 RVA: 0x000E3C7C File Offset: 0x000E1E7C
		protected override void StopProcessing()
		{
			this._waitForJobs.Set();
		}

		// Token: 0x060028C3 RID: 10435 RVA: 0x000E3C9C File Offset: 0x000E1E9C
		private void HandleStopJobCompleted(object sender, AsyncCompletedEventArgs eventArgs)
		{
			Job job = sender as Job;
			if (eventArgs.Error != null)
			{
				this._errorsToWrite.Add(new ErrorRecord(eventArgs.Error, "StopJobError", ErrorCategory.ReadError, job));
			}
			ContainerParentJob containerParentJob = job as ContainerParentJob;
			if (containerParentJob != null && containerParentJob.ExecutionError.Count > 0)
			{
				foreach (ErrorRecord item in from e in containerParentJob.ExecutionError
				where e.FullyQualifiedErrorId == "ContainerParentJobStopAsyncError"
				select e)
				{
					this._errorsToWrite.Add(item);
				}
			}
			bool flag = false;
			lock (this._syncObject)
			{
				if (this._pendingJobs.Contains(job.InstanceId))
				{
					this._pendingJobs.Remove(job.InstanceId);
				}
				if (this._needToCheckForWaitingJobs && this._pendingJobs.Count == 0)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this._waitForJobs.Set();
			}
		}

		// Token: 0x060028C4 RID: 10436 RVA: 0x000E3DD4 File Offset: 0x000E1FD4
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060028C5 RID: 10437 RVA: 0x000E3DE4 File Offset: 0x000E1FE4
		protected void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			foreach (Job2 job in this._cleanUpActions.Keys)
			{
				job.StopJobCompleted -= this._cleanUpActions[job];
			}
			this._waitForJobs.Dispose();
		}

		// Token: 0x0400140F RID: 5135
		private Job[] jobs;

		// Token: 0x04001410 RID: 5136
		private bool passThru;

		// Token: 0x04001411 RID: 5137
		private readonly HashSet<Guid> _pendingJobs = new HashSet<Guid>();

		// Token: 0x04001412 RID: 5138
		private readonly ManualResetEvent _waitForJobs = new ManualResetEvent(false);

		// Token: 0x04001413 RID: 5139
		private readonly Dictionary<Job2, EventHandler<AsyncCompletedEventArgs>> _cleanUpActions = new Dictionary<Job2, EventHandler<AsyncCompletedEventArgs>>();

		// Token: 0x04001414 RID: 5140
		private readonly List<Job> _allJobsToStop = new List<Job>();

		// Token: 0x04001415 RID: 5141
		private readonly List<ErrorRecord> _errorsToWrite = new List<ErrorRecord>();

		// Token: 0x04001416 RID: 5142
		private readonly object _syncObject = new object();

		// Token: 0x04001417 RID: 5143
		private bool _needToCheckForWaitingJobs;
	}
}
