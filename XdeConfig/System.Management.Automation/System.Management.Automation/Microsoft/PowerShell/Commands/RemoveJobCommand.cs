using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management.Automation;
using System.Management.Automation.Remoting;
using System.Threading;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000345 RID: 837
	[OutputType(new Type[]
	{
		typeof(Job)
	}, ParameterSetName = new string[]
	{
		"JobParameterSet"
	})]
	[Cmdlet("Remove", "Job", SupportsShouldProcess = true, DefaultParameterSetName = "SessionIdParameterSet", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=113377")]
	public class RemoveJobCommand : JobCmdletBase, IDisposable
	{
		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x060028EC RID: 10476 RVA: 0x000E476B File Offset: 0x000E296B
		// (set) Token: 0x060028ED RID: 10477 RVA: 0x000E4773 File Offset: 0x000E2973
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

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x060028EE RID: 10478 RVA: 0x000E477C File Offset: 0x000E297C
		// (set) Token: 0x060028EF RID: 10479 RVA: 0x000E4789 File Offset: 0x000E2989
		[Alias(new string[]
		{
			"F"
		})]
		[Parameter(ParameterSetName = "JobParameterSet")]
		[Parameter(ParameterSetName = "InstanceIdParameterSet")]
		[Parameter(ParameterSetName = "FilterParameterSet")]
		[Parameter(ParameterSetName = "NameParameterSet")]
		[Parameter(ParameterSetName = "SessionIdParameterSet")]
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

		// Token: 0x060028F0 RID: 10480 RVA: 0x000E4798 File Offset: 0x000E2998
		protected override void ProcessRecord()
		{
			string parameterSetName;
			List<Job> list;
			switch (parameterSetName = base.ParameterSetName)
			{
			case "NameParameterSet":
				list = base.FindJobsMatchingByName(false, false, true, !this.force);
				goto IL_115;
			case "InstanceIdParameterSet":
				list = base.FindJobsMatchingByInstanceId(true, false, true, !this.force);
				goto IL_115;
			case "SessionIdParameterSet":
				list = base.FindJobsMatchingBySessionId(true, false, true, !this.force);
				goto IL_115;
			case "CommandParameterSet":
				list = base.FindJobsMatchingByCommand(false);
				goto IL_115;
			case "StateParameterSet":
				list = base.FindJobsMatchingByState(false);
				goto IL_115;
			case "FilterParameterSet":
				list = base.FindJobsMatchingByFilter(false);
				goto IL_115;
			}
			list = base.CopyJobsToList(this.jobs, false, !this.force);
			IL_115:
			foreach (Job job in list)
			{
				string message = base.GetMessage(RemotingErrorIdStrings.StopPSJobWhatIfTarget, new object[]
				{
					job.Command,
					job.Id
				});
				if (base.ShouldProcess(message, "Remove"))
				{
					Job2 job2 = job as Job2;
					if (!job.IsFinishedState(job.JobStateInfo.State))
					{
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
							this.RemoveJobAndDispose(job, false);
						}
					}
					else
					{
						this.RemoveJobAndDispose(job, job2 != null);
					}
				}
			}
		}

		// Token: 0x060028F1 RID: 10481 RVA: 0x000E4A2C File Offset: 0x000E2C2C
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
		}

		// Token: 0x060028F2 RID: 10482 RVA: 0x000E4A90 File Offset: 0x000E2C90
		protected override void StopProcessing()
		{
			this._waitForJobs.Set();
		}

		// Token: 0x060028F3 RID: 10483 RVA: 0x000E4AA0 File Offset: 0x000E2CA0
		private void RemoveJobAndDispose(Job job, bool jobIsJob2)
		{
			try
			{
				bool flag = false;
				if (jobIsJob2)
				{
					flag = base.JobManager.RemoveJob(job as Job2, this, true, false);
				}
				if (!flag)
				{
					base.JobRepository.Remove(job);
				}
				job.Dispose();
			}
			catch (ArgumentException innerException)
			{
				string message = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.CannotRemoveJob, new object[0]);
				ArgumentException exception = new ArgumentException(message, innerException);
				base.WriteError(new ErrorRecord(exception, "CannotRemoveJob", ErrorCategory.InvalidOperation, job));
			}
		}

		// Token: 0x060028F4 RID: 10484 RVA: 0x000E4B20 File Offset: 0x000E2D20
		private void HandleStopJobCompleted(object sender, AsyncCompletedEventArgs eventArgs)
		{
			Job job = sender as Job;
			this.RemoveJobAndDispose(job, true);
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

		// Token: 0x060028F5 RID: 10485 RVA: 0x000E4BB8 File Offset: 0x000E2DB8
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060028F6 RID: 10486 RVA: 0x000E4BC8 File Offset: 0x000E2DC8
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

		// Token: 0x0400142C RID: 5164
		private Job[] jobs;

		// Token: 0x0400142D RID: 5165
		private bool force;

		// Token: 0x0400142E RID: 5166
		private HashSet<Guid> _pendingJobs = new HashSet<Guid>();

		// Token: 0x0400142F RID: 5167
		private readonly ManualResetEvent _waitForJobs = new ManualResetEvent(false);

		// Token: 0x04001430 RID: 5168
		private readonly Dictionary<Job2, EventHandler<AsyncCompletedEventArgs>> _cleanUpActions = new Dictionary<Job2, EventHandler<AsyncCompletedEventArgs>>();

		// Token: 0x04001431 RID: 5169
		private readonly object _syncObject = new object();

		// Token: 0x04001432 RID: 5170
		private bool _needToCheckForWaitingJobs;
	}
}
