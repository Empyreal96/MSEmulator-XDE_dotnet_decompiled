using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting.Internal;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000348 RID: 840
	[Cmdlet("Debug", "Job", SupportsShouldProcess = true, DefaultParameterSetName = "JobParameterSet", HelpUri = "http://go.microsoft.com/fwlink/?LinkId=330208")]
	public sealed class DebugJobCommand : PSCmdlet
	{
		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x06002916 RID: 10518 RVA: 0x000E5932 File Offset: 0x000E3B32
		// (set) Token: 0x06002917 RID: 10519 RVA: 0x000E593A File Offset: 0x000E3B3A
		[Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, ParameterSetName = "JobParameterSet")]
		public Job Job { get; set; }

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x06002918 RID: 10520 RVA: 0x000E5943 File Offset: 0x000E3B43
		// (set) Token: 0x06002919 RID: 10521 RVA: 0x000E594B File Offset: 0x000E3B4B
		[Parameter(Position = 0, Mandatory = true, ParameterSetName = "JobNameParameterSet")]
		public string Name { get; set; }

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x0600291A RID: 10522 RVA: 0x000E5954 File Offset: 0x000E3B54
		// (set) Token: 0x0600291B RID: 10523 RVA: 0x000E595C File Offset: 0x000E3B5C
		[Parameter(Position = 0, Mandatory = true, ParameterSetName = "JobIdParameterSet")]
		public int Id { get; set; }

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x0600291C RID: 10524 RVA: 0x000E5965 File Offset: 0x000E3B65
		// (set) Token: 0x0600291D RID: 10525 RVA: 0x000E596D File Offset: 0x000E3B6D
		[Parameter(Position = 0, Mandatory = true, ParameterSetName = "JobInstanceIdParameterSet")]
		public Guid InstanceId { get; set; }

		// Token: 0x0600291E RID: 10526 RVA: 0x000E5978 File Offset: 0x000E3B78
		protected override void EndProcessing()
		{
			string parameterSetName;
			if ((parameterSetName = base.ParameterSetName) != null)
			{
				if (!(parameterSetName == "JobParameterSet"))
				{
					if (!(parameterSetName == "JobNameParameterSet"))
					{
						if (!(parameterSetName == "JobIdParameterSet"))
						{
							if (parameterSetName == "JobInstanceIdParameterSet")
							{
								this._job = this.GetJobByInstanceId(this.InstanceId);
							}
						}
						else
						{
							this._job = this.GetJobById(this.Id);
						}
					}
					else
					{
						this._job = this.GetJobByName(this.Name);
					}
				}
				else
				{
					this._job = this.Job;
				}
			}
			if (!base.ShouldProcess(this._job.Name, "Debug"))
			{
				return;
			}
			Runspace defaultRunspace = Runspace.DefaultRunspace;
			if (defaultRunspace == null || defaultRunspace.Debugger == null)
			{
				base.ThrowTerminatingError(new ErrorRecord(new PSInvalidOperationException(RemotingErrorIdStrings.CannotDebugJobNoHostDebugger), "DebugJobNoHostDebugger", ErrorCategory.InvalidOperation, this));
			}
			if (defaultRunspace.Debugger.DebugMode == DebugModes.Default || defaultRunspace.Debugger.DebugMode == DebugModes.None)
			{
				base.ThrowTerminatingError(new ErrorRecord(new PSInvalidOperationException(RemotingErrorIdStrings.CannotDebugJobInvalidDebuggerMode), "DebugJobWrongDebugMode", ErrorCategory.InvalidOperation, this));
			}
			if (base.Host == null || base.Host.UI == null)
			{
				base.ThrowTerminatingError(new ErrorRecord(new PSInvalidOperationException(RemotingErrorIdStrings.CannotDebugJobNoHostUI), "DebugJobNoHostAvailable", ErrorCategory.InvalidOperation, this));
			}
			if (!this.CheckForDebuggableJob())
			{
				base.ThrowTerminatingError(new ErrorRecord(new PSInvalidOperationException(DebuggerStrings.NoDebuggableJobsFound), "DebugJobNoDebuggableJobsFound", ErrorCategory.InvalidOperation, this));
			}
			this._debugger = defaultRunspace.Debugger;
			this._debugger.DebugJob(this._job);
			this.WaitAndReceiveJobOutput();
		}

		// Token: 0x0600291F RID: 10527 RVA: 0x000E5B04 File Offset: 0x000E3D04
		protected override void StopProcessing()
		{
			Debugger debugger = this._debugger;
			if (debugger != null && this._job != null)
			{
				debugger.StopDebugJob(this._job);
			}
			PSDataCollection<PSStreamObject> debugCollection = this._debugCollection;
			if (debugCollection != null)
			{
				debugCollection.Complete();
			}
		}

		// Token: 0x06002920 RID: 10528 RVA: 0x000E5B40 File Offset: 0x000E3D40
		private bool CheckForDebuggableJob()
		{
			bool jobDebuggable = this.GetJobDebuggable(this._job);
			if (!jobDebuggable)
			{
				foreach (Job job in this._job.ChildJobs)
				{
					jobDebuggable = this.GetJobDebuggable(job);
					if (jobDebuggable)
					{
						break;
					}
				}
			}
			return jobDebuggable;
		}

		// Token: 0x06002921 RID: 10529 RVA: 0x000E5BA8 File Offset: 0x000E3DA8
		private bool GetJobDebuggable(Job job)
		{
			return job is IJobDebugger && (job.JobStateInfo.State == JobState.Running || job.JobStateInfo.State == JobState.AtBreakpoint);
		}

		// Token: 0x06002922 RID: 10530 RVA: 0x000E5BD4 File Offset: 0x000E3DD4
		private void WaitAndReceiveJobOutput()
		{
			this._debugCollection = new PSDataCollection<PSStreamObject>();
			this._debugCollection.BlockingEnumerator = true;
			try
			{
				this.AddEventHandlers();
				foreach (PSStreamObject psstreamObject in this._debugCollection)
				{
					if (psstreamObject != null)
					{
						psstreamObject.WriteStreamObject(this, false);
					}
				}
			}
			catch (Exception)
			{
				if (!this._job.IsFinishedState(this._job.JobStateInfo.State))
				{
					this._job.StopJob();
				}
				throw;
			}
			finally
			{
				this.RemoveEventHandlers();
				this._debugCollection = null;
			}
		}

		// Token: 0x06002923 RID: 10531 RVA: 0x000E5C98 File Offset: 0x000E3E98
		private void HandleJobStateChangedEvent(object sender, JobStateEventArgs stateChangedArgs)
		{
			Job job = sender as Job;
			if (job.IsFinishedState(stateChangedArgs.JobStateInfo.State))
			{
				this._debugCollection.Complete();
			}
		}

		// Token: 0x06002924 RID: 10532 RVA: 0x000E5CCC File Offset: 0x000E3ECC
		private void HandleResultsDataAdding(object sender, DataAddingEventArgs dataAddingArgs)
		{
			if (this._debugCollection.IsOpen)
			{
				PSStreamObject psstreamObject = dataAddingArgs.ItemAdded as PSStreamObject;
				if (psstreamObject != null)
				{
					try
					{
						this._debugCollection.Add(psstreamObject);
					}
					catch (PSInvalidOperationException)
					{
					}
				}
			}
		}

		// Token: 0x06002925 RID: 10533 RVA: 0x000E5D18 File Offset: 0x000E3F18
		private void HandleDebuggerNestedDebuggingCancelledEvent(object sender, EventArgs e)
		{
			this.StopProcessing();
		}

		// Token: 0x06002926 RID: 10534 RVA: 0x000E5D20 File Offset: 0x000E3F20
		private void AddEventHandlers()
		{
			this._job.StateChanged += this.HandleJobStateChangedEvent;
			this._debugger.NestedDebuggingCancelledEvent += this.HandleDebuggerNestedDebuggingCancelledEvent;
			if (this._job.ChildJobs.Count == 0)
			{
				this._job.Results.DataAdding += this.HandleResultsDataAdding;
				return;
			}
			foreach (Job job in this._job.ChildJobs)
			{
				job.Results.DataAdding += this.HandleResultsDataAdding;
			}
		}

		// Token: 0x06002927 RID: 10535 RVA: 0x000E5DE0 File Offset: 0x000E3FE0
		private void RemoveEventHandlers()
		{
			this._job.StateChanged -= this.HandleJobStateChangedEvent;
			this._debugger.NestedDebuggingCancelledEvent -= this.HandleDebuggerNestedDebuggingCancelledEvent;
			if (this._job.ChildJobs.Count == 0)
			{
				this._job.Results.DataAdding -= this.HandleResultsDataAdding;
				return;
			}
			foreach (Job job in this._job.ChildJobs)
			{
				job.Results.DataAdding -= this.HandleResultsDataAdding;
			}
		}

		// Token: 0x06002928 RID: 10536 RVA: 0x000E5EA0 File Offset: 0x000E40A0
		private Job GetJobByName(string name)
		{
			List<Job> list = new List<Job>();
			WildcardPattern wildcardPattern = new WildcardPattern(name, WildcardOptions.Compiled | WildcardOptions.IgnoreCase);
			foreach (Job job in base.JobRepository.Jobs)
			{
				if (wildcardPattern.IsMatch(job.Name))
				{
					list.Add(job);
				}
			}
			List<Job2> jobsByName = base.JobManager.GetJobsByName(name, this, false, false, false, null);
			int num = list.Count + jobsByName.Count;
			if (num == 1)
			{
				if (list.Count <= 0)
				{
					return jobsByName[0];
				}
				return list[0];
			}
			else
			{
				if (num > 1)
				{
					base.ThrowTerminatingError(new ErrorRecord(new PSInvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.FoundMultipleJobsWithName, name)), "DebugJobFoundMultipleJobsWithName", ErrorCategory.InvalidOperation, this));
					return null;
				}
				base.ThrowTerminatingError(new ErrorRecord(new PSInvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.CannotFindJobWithName, name)), "DebugJobCannotFindJobWithName", ErrorCategory.InvalidOperation, this));
				return null;
			}
		}

		// Token: 0x06002929 RID: 10537 RVA: 0x000E5FA0 File Offset: 0x000E41A0
		private Job GetJobById(int id)
		{
			List<Job> list = new List<Job>();
			foreach (Job job in base.JobRepository.Jobs)
			{
				if (job.Id == id)
				{
					list.Add(job);
				}
			}
			Job jobById = base.JobManager.GetJobById(id, this, false, false, false);
			if (list.Count == 0 && jobById == null)
			{
				base.ThrowTerminatingError(new ErrorRecord(new PSInvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.CannotFindJobWithId, id)), "DebugJobCannotFindJobWithId", ErrorCategory.InvalidOperation, this));
				return null;
			}
			if (list.Count > 1 || (list.Count == 1 && jobById != null))
			{
				base.ThrowTerminatingError(new ErrorRecord(new PSInvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.FoundMultipleJobsWithId, id)), "DebugJobFoundMultipleJobsWithId", ErrorCategory.InvalidOperation, this));
				return null;
			}
			if (list.Count <= 0)
			{
				return jobById;
			}
			return list[0];
		}

		// Token: 0x0600292A RID: 10538 RVA: 0x000E609C File Offset: 0x000E429C
		private Job GetJobByInstanceId(Guid instanceId)
		{
			foreach (Job job in base.JobRepository.Jobs)
			{
				if (job.InstanceId == instanceId)
				{
					return job;
				}
			}
			Job2 jobByInstanceId = base.JobManager.GetJobByInstanceId(instanceId, this, false, false, false);
			if (jobByInstanceId != null)
			{
				return jobByInstanceId;
			}
			base.ThrowTerminatingError(new ErrorRecord(new PSInvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.CannotFindJobWithInstanceId, instanceId)), "DebugJobCannotFindJobWithInstanceId", ErrorCategory.InvalidOperation, this));
			return null;
		}

		// Token: 0x0400144A RID: 5194
		private const string JobParameterSet = "JobParameterSet";

		// Token: 0x0400144B RID: 5195
		private const string JobNameParameterSet = "JobNameParameterSet";

		// Token: 0x0400144C RID: 5196
		private const string JobIdParameterSet = "JobIdParameterSet";

		// Token: 0x0400144D RID: 5197
		private const string JobInstanceIdParameterSet = "JobInstanceIdParameterSet";

		// Token: 0x0400144E RID: 5198
		private Job _job;

		// Token: 0x0400144F RID: 5199
		private Debugger _debugger;

		// Token: 0x04001450 RID: 5200
		private PSDataCollection<PSStreamObject> _debugCollection;
	}
}
