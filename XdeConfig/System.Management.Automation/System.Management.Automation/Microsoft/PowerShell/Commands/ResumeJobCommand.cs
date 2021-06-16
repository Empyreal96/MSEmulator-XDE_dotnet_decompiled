using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Remoting;
using System.Threading;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000347 RID: 839
	[Cmdlet("Resume", "Job", SupportsShouldProcess = true, DefaultParameterSetName = "SessionIdParameterSet", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=210611")]
	[OutputType(new Type[]
	{
		typeof(Job)
	})]
	public class ResumeJobCommand : JobCmdletBase, IDisposable
	{
		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x06002909 RID: 10505 RVA: 0x000E5376 File Offset: 0x000E3576
		// (set) Token: 0x0600290A RID: 10506 RVA: 0x000E537E File Offset: 0x000E357E
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

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x0600290B RID: 10507 RVA: 0x000E5387 File Offset: 0x000E3587
		public override string[] Command
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x0600290C RID: 10508 RVA: 0x000E538A File Offset: 0x000E358A
		// (set) Token: 0x0600290D RID: 10509 RVA: 0x000E5392 File Offset: 0x000E3592
		[Parameter(ParameterSetName = "__AllParameterSets")]
		public SwitchParameter Wait { get; set; }

		// Token: 0x0600290E RID: 10510 RVA: 0x000E539C File Offset: 0x000E359C
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
			this._allJobsToResume.AddRange(list);
			if (this._allJobsToResume.Count == 1)
			{
				this.Wait = true;
			}
			foreach (Job job in list)
			{
				Job2 job2 = job as Job2;
				if (job2 == null)
				{
					base.WriteError(new ErrorRecord(PSTraceSource.NewNotSupportedException(RemotingErrorIdStrings.JobResumeNotSupported, new object[]
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
					if (base.ShouldProcess(target, "Resume"))
					{
						this._cleanUpActions.Add(job2, new EventHandler<AsyncCompletedEventArgs>(this.HandleResumeJobCompleted));
						job2.ResumeJobCompleted += this.HandleResumeJobCompleted;
						lock (this._syncObject)
						{
							if (!this._pendingJobs.Contains(job2.InstanceId))
							{
								this._pendingJobs.Add(job2.InstanceId);
							}
						}
						job2.ResumeJobAsync();
					}
				}
			}
		}

		// Token: 0x0600290F RID: 10511 RVA: 0x000E55E4 File Offset: 0x000E37E4
		private void HandleResumeJobCompleted(object sender, AsyncCompletedEventArgs eventArgs)
		{
			Job job = sender as Job;
			if (eventArgs.Error != null && eventArgs.Error is InvalidJobStateException)
			{
				this._warnInvalidState = true;
			}
			ContainerParentJob containerParentJob = job as ContainerParentJob;
			if (containerParentJob != null && containerParentJob.ExecutionError.Count > 0)
			{
				foreach (ErrorRecord errorRecord in from e in containerParentJob.ExecutionError
				where e.FullyQualifiedErrorId == "ContainerParentJobResumeAsyncError"
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
				containerParentJob.ExecutionError.Clear();
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

		// Token: 0x06002910 RID: 10512 RVA: 0x000E5738 File Offset: 0x000E3938
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
			if (this.Wait && flag)
			{
				this._waitForJobs.WaitOne();
			}
			if (this._warnInvalidState)
			{
				base.WriteWarning(RemotingErrorIdStrings.ResumeJobInvalidJobState);
			}
			foreach (ErrorRecord errorRecord in this._errorsToWrite)
			{
				base.WriteError(errorRecord);
			}
			foreach (Job sendToPipeline in this._allJobsToResume)
			{
				base.WriteObject(sendToPipeline);
			}
			base.EndProcessing();
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x000E5848 File Offset: 0x000E3A48
		protected override void StopProcessing()
		{
			this._waitForJobs.Set();
		}

		// Token: 0x06002912 RID: 10514 RVA: 0x000E5856 File Offset: 0x000E3A56
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002913 RID: 10515 RVA: 0x000E5868 File Offset: 0x000E3A68
		protected void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			foreach (Job2 job in this._cleanUpActions.Keys)
			{
				job.ResumeJobCompleted -= this._cleanUpActions[job];
			}
			this._waitForJobs.Dispose();
		}

		// Token: 0x0400143F RID: 5183
		private Job[] jobs;

		// Token: 0x04001440 RID: 5184
		private bool _warnInvalidState;

		// Token: 0x04001441 RID: 5185
		private readonly HashSet<Guid> _pendingJobs = new HashSet<Guid>();

		// Token: 0x04001442 RID: 5186
		private readonly ManualResetEvent _waitForJobs = new ManualResetEvent(false);

		// Token: 0x04001443 RID: 5187
		private readonly Dictionary<Job2, EventHandler<AsyncCompletedEventArgs>> _cleanUpActions = new Dictionary<Job2, EventHandler<AsyncCompletedEventArgs>>();

		// Token: 0x04001444 RID: 5188
		private readonly List<ErrorRecord> _errorsToWrite = new List<ErrorRecord>();

		// Token: 0x04001445 RID: 5189
		private readonly List<Job> _allJobsToResume = new List<Job>();

		// Token: 0x04001446 RID: 5190
		private readonly object _syncObject = new object();

		// Token: 0x04001447 RID: 5191
		private bool _needToCheckForWaitingJobs;
	}
}
