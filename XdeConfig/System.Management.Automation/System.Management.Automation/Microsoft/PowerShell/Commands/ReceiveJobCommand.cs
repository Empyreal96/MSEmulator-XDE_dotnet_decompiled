using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Remoting.Internal;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Tracing;
using System.Threading;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000341 RID: 833
	[Cmdlet("Receive", "Job", DefaultParameterSetName = "Location", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=113372", RemotingCapability = RemotingCapability.SupportedByCommand)]
	public class ReceiveJobCommand : JobCmdletBase, IDisposable
	{
		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x06002879 RID: 10361 RVA: 0x000E1764 File Offset: 0x000DF964
		// (set) Token: 0x0600287A RID: 10362 RVA: 0x000E176C File Offset: 0x000DF96C
		[Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Location")]
		[Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerName")]
		[Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Session")]
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

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x0600287B RID: 10363 RVA: 0x000E1775 File Offset: 0x000DF975
		// (set) Token: 0x0600287C RID: 10364 RVA: 0x000E177D File Offset: 0x000DF97D
		[Alias(new string[]
		{
			"Cn"
		})]
		[ValidateNotNullOrEmpty]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerName", Position = 1)]
		public string[] ComputerName
		{
			get
			{
				return this.computerNames;
			}
			set
			{
				this.computerNames = value;
			}
		}

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x0600287D RID: 10365 RVA: 0x000E1786 File Offset: 0x000DF986
		// (set) Token: 0x0600287E RID: 10366 RVA: 0x000E178E File Offset: 0x000DF98E
		[ValidateNotNullOrEmpty]
		[Parameter(ParameterSetName = "Location", Position = 1)]
		public string[] Location
		{
			get
			{
				return this.locations;
			}
			set
			{
				this.locations = value;
			}
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x0600287F RID: 10367 RVA: 0x000E1797 File Offset: 0x000DF997
		// (set) Token: 0x06002880 RID: 10368 RVA: 0x000E179F File Offset: 0x000DF99F
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Session", Position = 1)]
		[ValidateNotNull]
		public PSSession[] Session
		{
			get
			{
				return this.remoteRunspaceInfos;
			}
			set
			{
				this.remoteRunspaceInfos = value;
			}
		}

		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x06002881 RID: 10369 RVA: 0x000E17A8 File Offset: 0x000DF9A8
		// (set) Token: 0x06002882 RID: 10370 RVA: 0x000E17B8 File Offset: 0x000DF9B8
		[Parameter]
		public SwitchParameter Keep
		{
			get
			{
				return !this.flush;
			}
			set
			{
				this.flush = !value;
				this.ValidateWait();
			}
		}

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x06002883 RID: 10371 RVA: 0x000E17CF File Offset: 0x000DF9CF
		// (set) Token: 0x06002884 RID: 10372 RVA: 0x000E17DF File Offset: 0x000DF9DF
		[Parameter]
		public SwitchParameter NoRecurse
		{
			get
			{
				return !this.recurse;
			}
			set
			{
				this.recurse = !value;
			}
		}

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x06002885 RID: 10373 RVA: 0x000E17F0 File Offset: 0x000DF9F0
		// (set) Token: 0x06002886 RID: 10374 RVA: 0x000E17F8 File Offset: 0x000DF9F8
		[Parameter]
		public SwitchParameter Force { get; set; }

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x06002887 RID: 10375 RVA: 0x000E1801 File Offset: 0x000DFA01
		public override JobState State
		{
			get
			{
				return JobState.NotStarted;
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x06002888 RID: 10376 RVA: 0x000E1804 File Offset: 0x000DFA04
		public override Hashtable Filter
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x06002889 RID: 10377 RVA: 0x000E1807 File Offset: 0x000DFA07
		public override string[] Command
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x0600288A RID: 10378 RVA: 0x000E180A File Offset: 0x000DFA0A
		// (set) Token: 0x0600288B RID: 10379 RVA: 0x000E1817 File Offset: 0x000DFA17
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
				this.ValidateWait();
			}
		}

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x0600288C RID: 10380 RVA: 0x000E182B File Offset: 0x000DFA2B
		// (set) Token: 0x0600288D RID: 10381 RVA: 0x000E1838 File Offset: 0x000DFA38
		[Parameter]
		public SwitchParameter AutoRemoveJob
		{
			get
			{
				return this._autoRemoveJob;
			}
			set
			{
				this._autoRemoveJob = value;
			}
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x0600288E RID: 10382 RVA: 0x000E1846 File Offset: 0x000DFA46
		// (set) Token: 0x0600288F RID: 10383 RVA: 0x000E1853 File Offset: 0x000DFA53
		[Parameter]
		public SwitchParameter WriteEvents
		{
			get
			{
				return this._writeStateChangedEvents;
			}
			set
			{
				this._writeStateChangedEvents = value;
			}
		}

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x06002890 RID: 10384 RVA: 0x000E1861 File Offset: 0x000DFA61
		// (set) Token: 0x06002891 RID: 10385 RVA: 0x000E186E File Offset: 0x000DFA6E
		[Parameter]
		public SwitchParameter WriteJobInResults
		{
			get
			{
				return this._outputJobFirst;
			}
			set
			{
				this._outputJobFirst = value;
			}
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x000E187C File Offset: 0x000DFA7C
		protected override void BeginProcessing()
		{
			this.ValidateAutoRemove();
			this.ValidateWriteJobInResults();
			this.ValidateWriteEvents();
			this.ValidateForce();
		}

		// Token: 0x06002893 RID: 10387 RVA: 0x000E1898 File Offset: 0x000DFA98
		protected override void ProcessRecord()
		{
			bool checkForRecurse = false;
			List<Job> list = new List<Job>();
			string parameterSetName;
			if ((parameterSetName = base.ParameterSetName) != null)
			{
				if (!(parameterSetName == "Session"))
				{
					if (!(parameterSetName == "ComputerName"))
					{
						if (!(parameterSetName == "Location"))
						{
							if (!(parameterSetName == "InstanceIdParameterSet"))
							{
								if (!(parameterSetName == "SessionIdParameterSet"))
								{
									if (parameterSetName == "NameParameterSet")
									{
										List<Job> collection = base.FindJobsMatchingByName(true, false, true, false);
										list.AddRange(collection);
										checkForRecurse = true;
									}
								}
								else
								{
									List<Job> collection2 = base.FindJobsMatchingBySessionId(true, false, true, false);
									list.AddRange(collection2);
									checkForRecurse = true;
								}
							}
							else
							{
								List<Job> collection3 = base.FindJobsMatchingByInstanceId(true, false, true, false);
								list.AddRange(collection3);
								checkForRecurse = true;
							}
						}
						else if (this.locations == null)
						{
							list.AddRange(this.jobs);
							checkForRecurse = true;
						}
						else
						{
							foreach (Job job in this.jobs)
							{
								foreach (string location in this.locations)
								{
									List<Job> jobsForLocation = job.GetJobsForLocation(location);
									list.AddRange(jobsForLocation);
								}
							}
						}
					}
					else
					{
						foreach (Job job2 in this.jobs)
						{
							PSRemotingJob psremotingJob = job2 as PSRemotingJob;
							if (psremotingJob == null)
							{
								string message = base.GetMessage(RemotingErrorIdStrings.ComputerNameParamNotSupported);
								base.WriteError(new ErrorRecord(new ArgumentException(message), "ComputerNameParameterNotSupported", ErrorCategory.InvalidArgument, job2));
							}
							else
							{
								string[] array4 = null;
								base.ResolveComputerNames(this.computerNames, out array4);
								foreach (string computerName in array4)
								{
									List<Job> jobsForComputer = psremotingJob.GetJobsForComputer(computerName);
									list.AddRange(jobsForComputer);
								}
							}
						}
					}
				}
				else
				{
					foreach (Job job3 in this.jobs)
					{
						PSRemotingJob psremotingJob2 = job3 as PSRemotingJob;
						if (psremotingJob2 == null)
						{
							string message2 = base.GetMessage(RemotingErrorIdStrings.RunspaceParamNotSupported);
							base.WriteError(new ErrorRecord(new ArgumentException(message2), "RunspaceParameterNotSupported", ErrorCategory.InvalidArgument, job3));
						}
						else
						{
							foreach (PSSession runspace in this.remoteRunspaceInfos)
							{
								List<Job> jobsForRunspace = psremotingJob2.GetJobsForRunspace(runspace);
								list.AddRange(jobsForRunspace);
							}
						}
					}
				}
			}
			if (this._wait)
			{
				this._writeExistingData.Reset();
				this.WriteJobsIfRequired(list);
				foreach (Job job4 in list)
				{
					this._jobsSpecifiedInParameters.Add(job4.InstanceId);
				}
				lock (this._syncObject)
				{
					if (this._isDisposed || this._isStopping)
					{
						return;
					}
					if (!this._holdingResultsRef)
					{
						this._tracer.WriteMessage("ReceiveJobCommand", "ProcessRecord", Guid.Empty, null, "Adding Ref to results collection", null);
						this._results.AddRef();
						this._holdingResultsRef = true;
					}
				}
				this._tracer.WriteMessage("ReceiveJobCommand", "ProcessRecord", Guid.Empty, null, "BEGIN Register for jobs", new string[0]);
				this.WriteResultsForJobsInCollection(list, checkForRecurse, true);
				this._tracer.WriteMessage("ReceiveJobCommand", "ProcessRecord", Guid.Empty, null, "END Register for jobs", new string[0]);
				lock (this._syncObject)
				{
					if (this._jobsBeingAggregated.Count == 0 && this._holdingResultsRef)
					{
						this._tracer.WriteMessage("ReceiveJobCommand", "ProcessRecord", Guid.Empty, null, "Removing Ref to results collection", null);
						this._results.DecrementRef();
						this._holdingResultsRef = false;
					}
				}
				this._tracer.WriteMessage("ReceiveJobCommand", "ProcessRecord", Guid.Empty, null, "BEGIN Write existing job data", new string[0]);
				this.WriteResultsForJobsInCollection(list, checkForRecurse, false);
				this._tracer.WriteMessage("ReceiveJobCommand", "ProcessRecord", Guid.Empty, null, "END Write existing job data", new string[0]);
				this._writeExistingData.Set();
				return;
			}
			this.WriteResultsForJobsInCollection(list, checkForRecurse, false);
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x000E1D2C File Offset: 0x000DFF2C
		protected override void StopProcessing()
		{
			this._tracer.WriteMessage("ReceiveJobCommand", "StopProcessing", Guid.Empty, null, "Entered Stop Processing", null);
			lock (this._syncObject)
			{
				this._isStopping = true;
			}
			this._writeExistingData.Set();
			Job[] array = new Job[this._jobsBeingAggregated.Count];
			for (int i = 0; i < this._jobsBeingAggregated.Count; i++)
			{
				array[i] = this._jobsBeingAggregated[i];
			}
			foreach (Job job in array)
			{
				this.StopAggregateResultsFromJob(job);
			}
			this._resultsReaderWriterLock.EnterWriteLock();
			try
			{
				this._results.Complete();
				this.SetOutputProcessingState(false);
			}
			finally
			{
				this._resultsReaderWriterLock.ExitWriteLock();
			}
			base.StopProcessing();
			this._tracer.WriteMessage("ReceiveJobCommand", "StopProcessing", Guid.Empty, null, "Exiting Stop Processing", null);
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x000E1E54 File Offset: 0x000E0054
		protected override void EndProcessing()
		{
			try
			{
				if (this._wait)
				{
					int num = 0;
					foreach (PSStreamObject psstreamObject in this._results)
					{
						if (this._isStopping)
						{
							break;
						}
						this.SetOutputProcessingState(true);
						psstreamObject.WriteStreamObject(this, true, true);
						if (++num == this._results.Count)
						{
							this.SetOutputProcessingState(false);
						}
					}
					this._eventArgsWritten.Clear();
				}
				else
				{
					int num2 = 0;
					foreach (PSStreamObject psstreamObject2 in this._results)
					{
						if (this._isStopping)
						{
							break;
						}
						this.SetOutputProcessingState(true);
						psstreamObject2.WriteStreamObject(this, false, true);
						if (++num2 == this._results.Count)
						{
							this.SetOutputProcessingState(false);
						}
					}
				}
			}
			finally
			{
				this.SetOutputProcessingState(false);
			}
		}

		// Token: 0x06002896 RID: 10390 RVA: 0x000E1F6C File Offset: 0x000E016C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002897 RID: 10391 RVA: 0x000E1F7C File Offset: 0x000E017C
		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this._isDisposed)
				{
					return;
				}
				lock (this._syncObject)
				{
					if (this._isDisposed)
					{
						return;
					}
					this._isDisposed = true;
				}
				this.SetOutputProcessingState(false);
				if (this._jobsBeingAggregated != null)
				{
					foreach (Job job in this._jobsBeingAggregated)
					{
						if (job.MonitorOutputProcessing)
						{
							job.RemoveMonitorOutputProcessing(this._outputProcessingNotification);
						}
						if (job.UsesResultsCollection)
						{
							job.Results.DataAdded -= this.ResultsAdded;
						}
						else
						{
							job.Output.DataAdded -= this.Output_DataAdded;
							job.Error.DataAdded -= this.Error_DataAdded;
							job.Progress.DataAdded -= this.Progress_DataAdded;
							job.Verbose.DataAdded -= this.Verbose_DataAdded;
							job.Warning.DataAdded -= this.Warning_DataAdded;
							job.Debug.DataAdded -= this.Debug_DataAdded;
							job.Information.DataAdded -= this.Information_DataAdded;
						}
						job.StateChanged -= this.HandleJobStateChanged;
					}
				}
				this._resultsReaderWriterLock.EnterWriteLock();
				try
				{
					this._results.Complete();
				}
				finally
				{
					this._resultsReaderWriterLock.ExitWriteLock();
				}
				this._resultsReaderWriterLock.Dispose();
				this._results.Clear();
				this._results.Dispose();
				this._writeExistingData.Set();
				this._writeExistingData.Dispose();
			}
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x000E21A4 File Offset: 0x000E03A4
		private static void DoUnblockJob(Job job)
		{
			if (job.ChildJobs.Count != 0)
			{
				return;
			}
			PSRemotingChildJob psremotingChildJob = job as PSRemotingChildJob;
			if (psremotingChildJob != null)
			{
				psremotingChildJob.UnblockJob();
				return;
			}
			job.SetJobState(JobState.Running, null);
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x000E21D8 File Offset: 0x000E03D8
		private void WriteJobResults(Job job)
		{
			if (job == null)
			{
				return;
			}
			if (job.JobStateInfo.State == JobState.Disconnected)
			{
				PSRemotingChildJob psremotingChildJob = job as PSRemotingChildJob;
				if (psremotingChildJob != null && psremotingChildJob.DisconnectedAndBlocked)
				{
					return;
				}
			}
			if (job.JobStateInfo.State == JobState.Blocked)
			{
				ReceiveJobCommand.DoUnblockJob(job);
			}
			if (!(job is Job2) && job.UsesResultsCollection)
			{
				Collection<PSStreamObject> collection = this.ReadAll<PSStreamObject>(job.Results);
				if (this._wait)
				{
					using (IEnumerator<PSStreamObject> enumerator = collection.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							PSStreamObject psstreamObject = enumerator.Current;
							psstreamObject.WriteStreamObject(this, job.Results.SourceId, false);
						}
						goto IL_334;
					}
				}
				using (IEnumerator<PSStreamObject> enumerator2 = collection.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						PSStreamObject psstreamObject2 = enumerator2.Current;
						psstreamObject2.WriteStreamObject(this, false);
					}
					goto IL_334;
				}
			}
			Collection<PSObject> collection2 = this.ReadAll<PSObject>(job.Output);
			foreach (PSObject psobject in collection2)
			{
				if (psobject != null)
				{
					base.WriteObject(psobject);
				}
			}
			Collection<ErrorRecord> collection3 = this.ReadAll<ErrorRecord>(job.Error);
			foreach (ErrorRecord errorRecord in collection3)
			{
				if (errorRecord != null)
				{
					MshCommandRuntime mshCommandRuntime = base.CommandRuntime as MshCommandRuntime;
					if (mshCommandRuntime != null)
					{
						errorRecord.PreserveInvocationInfoOnce = true;
						mshCommandRuntime.WriteError(errorRecord, true);
					}
				}
			}
			Collection<VerboseRecord> collection4 = this.ReadAll<VerboseRecord>(job.Verbose);
			foreach (VerboseRecord verboseRecord in collection4)
			{
				if (verboseRecord != null)
				{
					MshCommandRuntime mshCommandRuntime2 = base.CommandRuntime as MshCommandRuntime;
					if (mshCommandRuntime2 != null)
					{
						mshCommandRuntime2.WriteVerbose(verboseRecord, true);
					}
				}
			}
			Collection<DebugRecord> collection5 = this.ReadAll<DebugRecord>(job.Debug);
			foreach (DebugRecord debugRecord in collection5)
			{
				if (debugRecord != null)
				{
					MshCommandRuntime mshCommandRuntime3 = base.CommandRuntime as MshCommandRuntime;
					if (mshCommandRuntime3 != null)
					{
						mshCommandRuntime3.WriteDebug(debugRecord, true);
					}
				}
			}
			Collection<WarningRecord> collection6 = this.ReadAll<WarningRecord>(job.Warning);
			foreach (WarningRecord warningRecord in collection6)
			{
				if (warningRecord != null)
				{
					MshCommandRuntime mshCommandRuntime4 = base.CommandRuntime as MshCommandRuntime;
					if (mshCommandRuntime4 != null)
					{
						mshCommandRuntime4.WriteWarning(warningRecord, true);
					}
				}
			}
			Collection<ProgressRecord> collection7 = this.ReadAll<ProgressRecord>(job.Progress);
			foreach (ProgressRecord progressRecord in collection7)
			{
				if (progressRecord != null)
				{
					MshCommandRuntime mshCommandRuntime5 = base.CommandRuntime as MshCommandRuntime;
					if (mshCommandRuntime5 != null)
					{
						mshCommandRuntime5.WriteProgress(progressRecord, true);
					}
				}
			}
			Collection<InformationRecord> collection8 = this.ReadAll<InformationRecord>(job.Information);
			foreach (InformationRecord informationRecord in collection8)
			{
				if (informationRecord != null)
				{
					MshCommandRuntime mshCommandRuntime6 = base.CommandRuntime as MshCommandRuntime;
					if (mshCommandRuntime6 != null)
					{
						mshCommandRuntime6.WriteInformation(informationRecord, true);
					}
				}
			}
			IL_334:
			if (job.JobStateInfo.State != JobState.Failed)
			{
				return;
			}
			this.WriteReasonError(job);
		}

		// Token: 0x0600289A RID: 10394 RVA: 0x000E25A0 File Offset: 0x000E07A0
		private void WriteReasonError(Job job)
		{
			PSRemotingChildJob psremotingChildJob = job as PSRemotingChildJob;
			if (psremotingChildJob != null && psremotingChildJob.FailureErrorRecord != null)
			{
				this._results.Add(new PSStreamObject(PSStreamObjectType.Error, psremotingChildJob.FailureErrorRecord, psremotingChildJob.InstanceId));
				return;
			}
			if (job.JobStateInfo.Reason != null)
			{
				Exception reason = job.JobStateInfo.Reason;
				Exception exception = reason;
				JobFailedException ex = reason as JobFailedException;
				if (ex != null)
				{
					exception = ex.Reason;
				}
				ErrorRecord errorRecord = new ErrorRecord(exception, "JobStateFailed", ErrorCategory.InvalidResult, null);
				if (ex != null && ex.DisplayScriptPosition != null)
				{
					if (errorRecord.InvocationInfo == null)
					{
						errorRecord.SetInvocationInfo(new InvocationInfo(null, null));
					}
					errorRecord.InvocationInfo.DisplayScriptPosition = ex.DisplayScriptPosition;
				}
				this._results.Add(new PSStreamObject(PSStreamObjectType.Error, errorRecord, job.InstanceId));
			}
		}

		// Token: 0x0600289B RID: 10395 RVA: 0x000E2668 File Offset: 0x000E0868
		private Collection<T> ReadAll<T>(PSDataCollection<T> psDataCollection)
		{
			if (this.flush)
			{
				return psDataCollection.ReadAll();
			}
			T[] array = new T[psDataCollection.Count];
			psDataCollection.CopyTo(array, 0);
			Collection<T> collection = new Collection<T>();
			foreach (T item in array)
			{
				collection.Add(item);
			}
			return collection;
		}

		// Token: 0x0600289C RID: 10396 RVA: 0x000E26C4 File Offset: 0x000E08C4
		private void WriteJobResultsRecursivelyHelper(Hashtable duplicate, Job job, bool registerInsteadOfWrite)
		{
			if (duplicate.ContainsKey(job))
			{
				return;
			}
			duplicate.Add(job, job);
			IList<Job> childJobs = job.ChildJobs;
			foreach (Job job2 in childJobs)
			{
				this.WriteJobResultsRecursivelyHelper(duplicate, job2, registerInsteadOfWrite);
			}
			if (registerInsteadOfWrite)
			{
				this._eventArgsWritten[job.InstanceId] = false;
				this.AggregateResultsFromJob(job);
				return;
			}
			this.WriteJobResults(job);
			this.WriteJobStateInformationIfRequired(job, null);
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x000E2754 File Offset: 0x000E0954
		private void WriteJobsIfRequired(IEnumerable<Job> jobsToWrite)
		{
			if (!this._outputJobFirst)
			{
				return;
			}
			foreach (Job job in jobsToWrite)
			{
				this._tracer.WriteMessage("ReceiveJobCommand", "WriteJobsIfRequired", Guid.Empty, job, "Writing job object as output", null);
				base.WriteObject(job);
			}
		}

		// Token: 0x0600289E RID: 10398 RVA: 0x000E27C8 File Offset: 0x000E09C8
		private void AggregateResultsFromJob(Job job)
		{
			if ((!this.Force && job.IsPersistentState(job.JobStateInfo.State)) || (this.Force && job.IsFinishedState(job.JobStateInfo.State)))
			{
				return;
			}
			job.StateChanged += this.HandleJobStateChanged;
			if ((!this.Force && job.IsPersistentState(job.JobStateInfo.State)) || (this.Force && job.IsFinishedState(job.JobStateInfo.State)))
			{
				job.StateChanged -= this.HandleJobStateChanged;
				return;
			}
			this._tracer.WriteMessage("ReceiveJobCommand", "AggregateResultsFromJob", Guid.Empty, job, "BEGIN Adding job for aggregation", null);
			this._jobsBeingAggregated.Add(job);
			if (job.UsesResultsCollection)
			{
				job.Results.SourceId = job.InstanceId;
				job.Results.DataAdded += this.ResultsAdded;
			}
			else
			{
				job.Output.SourceId = job.InstanceId;
				job.Error.SourceId = job.InstanceId;
				job.Progress.SourceId = job.InstanceId;
				job.Verbose.SourceId = job.InstanceId;
				job.Warning.SourceId = job.InstanceId;
				job.Debug.SourceId = job.InstanceId;
				job.Information.SourceId = job.InstanceId;
				job.Output.DataAdded += this.Output_DataAdded;
				job.Error.DataAdded += this.Error_DataAdded;
				job.Progress.DataAdded += this.Progress_DataAdded;
				job.Verbose.DataAdded += this.Verbose_DataAdded;
				job.Warning.DataAdded += this.Warning_DataAdded;
				job.Debug.DataAdded += this.Debug_DataAdded;
				job.Information.DataAdded += this.Information_DataAdded;
			}
			if (job.MonitorOutputProcessing)
			{
				if (this._outputProcessingNotification == null)
				{
					lock (this._syncObject)
					{
						if (this._outputProcessingNotification == null)
						{
							this._outputProcessingNotification = new OutputProcessingState();
						}
					}
				}
				job.SetMonitorOutputProcessing(this._outputProcessingNotification);
			}
			this._tracer.WriteMessage("ReceiveJobCommand", "AggregateResultsFromJob", Guid.Empty, job, "END Adding job for aggregation", null);
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x000E2A74 File Offset: 0x000E0C74
		private void ResultsAdded(object sender, DataAddedEventArgs e)
		{
			lock (this._syncObject)
			{
				if (this._isDisposed)
				{
					return;
				}
			}
			this._writeExistingData.WaitOne();
			PSDataCollection<PSStreamObject> psdataCollection = sender as PSDataCollection<PSStreamObject>;
			PSStreamObject data = this.GetData<PSStreamObject>(psdataCollection, e.Index);
			if (data != null)
			{
				data.Id = psdataCollection.SourceId;
				this._results.Add(data);
			}
		}

		// Token: 0x060028A0 RID: 10400 RVA: 0x000E2AF4 File Offset: 0x000E0CF4
		private void HandleJobStateChanged(object sender, JobStateEventArgs e)
		{
			Job job = sender as Job;
			this._tracer.WriteMessage("ReceiveJobCommand", "HandleJobStateChanged", Guid.Empty, job, "BEGIN wait for write existing data", null);
			if (e.JobStateInfo.State != JobState.Running)
			{
				this._writeExistingData.WaitOne();
			}
			this._tracer.WriteMessage("ReceiveJobCommand", "HandleJobStateChanged", Guid.Empty, job, "END wait for write existing data", null);
			lock (this._syncObject)
			{
				if (!this._jobsBeingAggregated.Contains(job))
				{
					this._tracer.WriteMessage("ReceiveJobCommand", "HandleJobStateChanged", Guid.Empty, job, "Returning because job is not in _jobsBeingAggregated", null);
					return;
				}
			}
			if (e.JobStateInfo.State == JobState.Blocked)
			{
				ReceiveJobCommand.DoUnblockJob(job);
			}
			if ((!this.Force && job.IsPersistentState(e.JobStateInfo.State)) || (this.Force && job.IsFinishedState(e.JobStateInfo.State)))
			{
				this.WriteReasonError(job);
				this.WriteJobStateInformationIfRequired(job, e);
				this.StopAggregateResultsFromJob(job);
				return;
			}
			this._tracer.WriteMessage("ReceiveJobCommand", "HandleJobStateChanged", Guid.Empty, job, "Returning because job state does not meet wait requirements (continue aggregating)", new string[0]);
		}

		// Token: 0x060028A1 RID: 10401 RVA: 0x000E2C58 File Offset: 0x000E0E58
		private void Progress_DataAdded(object sender, DataAddedEventArgs e)
		{
			lock (this._syncObject)
			{
				if (this._isDisposed)
				{
					return;
				}
			}
			this._writeExistingData.WaitOne();
			this._resultsReaderWriterLock.EnterReadLock();
			try
			{
				if (this._results.IsOpen)
				{
					PSDataCollection<ProgressRecord> psdataCollection = sender as PSDataCollection<ProgressRecord>;
					ProgressRecord data = this.GetData<ProgressRecord>(psdataCollection, e.Index);
					if (data != null)
					{
						this._results.Add(new PSStreamObject(PSStreamObjectType.Progress, data, psdataCollection.SourceId));
					}
				}
			}
			finally
			{
				this._resultsReaderWriterLock.ExitReadLock();
			}
		}

		// Token: 0x060028A2 RID: 10402 RVA: 0x000E2D10 File Offset: 0x000E0F10
		private void Error_DataAdded(object sender, DataAddedEventArgs e)
		{
			lock (this._syncObject)
			{
				if (this._isDisposed)
				{
					return;
				}
			}
			this._writeExistingData.WaitOne();
			this._resultsReaderWriterLock.EnterReadLock();
			try
			{
				if (this._results.IsOpen)
				{
					PSDataCollection<ErrorRecord> collection = sender as PSDataCollection<ErrorRecord>;
					ErrorRecord data = this.GetData<ErrorRecord>(collection, e.Index);
					if (data != null)
					{
						this._results.Add(new PSStreamObject(PSStreamObjectType.Error, data, Guid.Empty));
					}
				}
			}
			finally
			{
				this._resultsReaderWriterLock.ExitReadLock();
			}
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x000E2DC4 File Offset: 0x000E0FC4
		private void Debug_DataAdded(object sender, DataAddedEventArgs e)
		{
			lock (this._syncObject)
			{
				if (this._isDisposed)
				{
					return;
				}
			}
			this._writeExistingData.WaitOne();
			this._resultsReaderWriterLock.EnterReadLock();
			try
			{
				if (this._results.IsOpen)
				{
					PSDataCollection<DebugRecord> collection = sender as PSDataCollection<DebugRecord>;
					DebugRecord data = this.GetData<DebugRecord>(collection, e.Index);
					if (data != null)
					{
						this._results.Add(new PSStreamObject(PSStreamObjectType.Debug, data.Message, Guid.Empty));
					}
				}
			}
			finally
			{
				this._resultsReaderWriterLock.ExitReadLock();
			}
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x000E2E7C File Offset: 0x000E107C
		private void Warning_DataAdded(object sender, DataAddedEventArgs e)
		{
			lock (this._syncObject)
			{
				if (this._isDisposed)
				{
					return;
				}
			}
			this._writeExistingData.WaitOne();
			this._resultsReaderWriterLock.EnterReadLock();
			try
			{
				if (this._results.IsOpen)
				{
					PSDataCollection<WarningRecord> collection = sender as PSDataCollection<WarningRecord>;
					WarningRecord data = this.GetData<WarningRecord>(collection, e.Index);
					if (data != null)
					{
						this._results.Add(new PSStreamObject(PSStreamObjectType.Warning, data.Message, Guid.Empty));
					}
				}
			}
			finally
			{
				this._resultsReaderWriterLock.ExitReadLock();
			}
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x000E2F34 File Offset: 0x000E1134
		private void Verbose_DataAdded(object sender, DataAddedEventArgs e)
		{
			lock (this._syncObject)
			{
				if (this._isDisposed)
				{
					return;
				}
			}
			this._writeExistingData.WaitOne();
			this._resultsReaderWriterLock.EnterReadLock();
			try
			{
				if (this._results.IsOpen)
				{
					PSDataCollection<VerboseRecord> collection = sender as PSDataCollection<VerboseRecord>;
					VerboseRecord data = this.GetData<VerboseRecord>(collection, e.Index);
					if (data != null)
					{
						this._results.Add(new PSStreamObject(PSStreamObjectType.Verbose, data.Message, Guid.Empty));
					}
				}
			}
			finally
			{
				this._resultsReaderWriterLock.ExitReadLock();
			}
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x000E2FF0 File Offset: 0x000E11F0
		private void Information_DataAdded(object sender, DataAddedEventArgs e)
		{
			lock (this._syncObject)
			{
				if (this._isDisposed)
				{
					return;
				}
			}
			this._writeExistingData.WaitOne();
			this._resultsReaderWriterLock.EnterReadLock();
			try
			{
				if (this._results.IsOpen)
				{
					PSDataCollection<InformationRecord> collection = sender as PSDataCollection<InformationRecord>;
					InformationRecord data = this.GetData<InformationRecord>(collection, e.Index);
					if (data != null)
					{
						this._results.Add(new PSStreamObject(PSStreamObjectType.Information, data, Guid.Empty));
					}
				}
			}
			finally
			{
				this._resultsReaderWriterLock.ExitReadLock();
			}
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x000E30A4 File Offset: 0x000E12A4
		private void Output_DataAdded(object sender, DataAddedEventArgs e)
		{
			lock (this._syncObject)
			{
				if (this._isDisposed)
				{
					return;
				}
			}
			this._writeExistingData.WaitOne();
			this._resultsReaderWriterLock.EnterReadLock();
			try
			{
				if (this._results.IsOpen)
				{
					PSDataCollection<PSObject> collection = sender as PSDataCollection<PSObject>;
					PSObject data = this.GetData<PSObject>(collection, e.Index);
					if (data != null)
					{
						this._results.Add(new PSStreamObject(PSStreamObjectType.Output, data, Guid.Empty));
					}
				}
			}
			finally
			{
				this._resultsReaderWriterLock.ExitReadLock();
			}
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x000E3158 File Offset: 0x000E1358
		private T GetData<T>(PSDataCollection<T> collection, int index)
		{
			if (!this.flush)
			{
				return collection[index];
			}
			Collection<T> collection2 = collection.ReadAndRemove(1);
			if (collection2.Count > 0)
			{
				return collection2[0];
			}
			return default(T);
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x000E3198 File Offset: 0x000E1398
		private void StopAggregateResultsFromJob(Job job)
		{
			this.SetOutputProcessingState(false);
			lock (this._syncObject)
			{
				this._tracer.WriteMessage("ReceiveJobCommand", "StopAggregateResultsFromJob", Guid.Empty, job, "Removing job from aggregation", null);
				this._jobsBeingAggregated.Remove(job);
				if (this._jobsBeingAggregated.Count == 0 && this._holdingResultsRef)
				{
					this._tracer.WriteMessage("ReceiveJobCommand", "StopAggregateResultsFromJob", Guid.Empty, null, "Removing Ref to results collection", null);
					this._results.DecrementRef();
					this._holdingResultsRef = false;
				}
			}
			if (job.MonitorOutputProcessing)
			{
				job.RemoveMonitorOutputProcessing(this._outputProcessingNotification);
			}
			if (job.UsesResultsCollection)
			{
				job.Results.DataAdded -= this.ResultsAdded;
			}
			else
			{
				job.Output.DataAdded -= this.Output_DataAdded;
				job.Error.DataAdded -= this.Error_DataAdded;
				job.Progress.DataAdded -= this.Progress_DataAdded;
				job.Verbose.DataAdded -= this.Verbose_DataAdded;
				job.Warning.DataAdded -= this.Warning_DataAdded;
				job.Debug.DataAdded -= this.Debug_DataAdded;
				job.Information.DataAdded -= this.Information_DataAdded;
			}
			job.StateChanged -= this.HandleJobStateChanged;
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x000E333C File Offset: 0x000E153C
		private void AutoRemoveJobIfRequired(Job job)
		{
			if (!this._autoRemoveJob)
			{
				return;
			}
			if (!this._jobsSpecifiedInParameters.Contains(job.InstanceId))
			{
				return;
			}
			if (!job.IsFinishedState(job.JobStateInfo.State))
			{
				return;
			}
			if (job.HasMoreData)
			{
				this._tracer.WriteMessage("ReceiveJobCommand", "AutoRemoveJobIfRequired", Guid.Empty, job, "Job has data and is being removed.", new string[0]);
			}
			Job2 job2 = job as Job2;
			if (job2 != null)
			{
				try
				{
					base.JobManager.RemoveJob(job2, this, false, true);
					job.Dispose();
					return;
				}
				catch (Exception ex)
				{
					this.AddRemoveErrorToResults(job2, ex);
					return;
				}
			}
			try
			{
				base.JobRepository.Remove(job);
				job.Dispose();
			}
			catch (ArgumentException ex2)
			{
				this.AddRemoveErrorToResults(job, ex2);
			}
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x000E3410 File Offset: 0x000E1610
		private void AddRemoveErrorToResults(Job job, Exception ex)
		{
			ArgumentException exception = new ArgumentException(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.CannotRemoveJob, new object[0]), ex);
			ErrorRecord value = new ErrorRecord(exception, "ReceiveJobAutoRemovalError", ErrorCategory.InvalidOperation, job);
			this._results.Add(new PSStreamObject(PSStreamObjectType.Error, value));
		}

		// Token: 0x060028AC RID: 10412 RVA: 0x000E3454 File Offset: 0x000E1654
		private void WriteJobResultsRecursively(Job job, bool registerInsteadOfWrite)
		{
			Hashtable hashtable = new Hashtable();
			this.WriteJobResultsRecursivelyHelper(hashtable, job, registerInsteadOfWrite);
			hashtable.Clear();
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x000E3478 File Offset: 0x000E1678
		private void WriteResultsForJobsInCollection(List<Job> jobs, bool checkForRecurse, bool registerInsteadOfWrite)
		{
			foreach (Job job in jobs)
			{
				if (base.JobManager.IsJobFromAdapter(job.InstanceId, "PSWorkflowJob") && job.JobStateInfo.State == JobState.Stopped)
				{
					MshCommandRuntime mshCommandRuntime = base.CommandRuntime as MshCommandRuntime;
					if (mshCommandRuntime != null)
					{
						mshCommandRuntime.WriteWarning(new WarningRecord(StringUtil.Format(RemotingErrorIdStrings.JobWasStopped, job.Name)), true);
					}
				}
				if (checkForRecurse && this.recurse)
				{
					this.WriteJobResultsRecursively(job, registerInsteadOfWrite);
				}
				else if (registerInsteadOfWrite)
				{
					this._eventArgsWritten[job.InstanceId] = false;
					this.AggregateResultsFromJob(job);
				}
				else
				{
					this.WriteJobResults(job);
					this.WriteJobStateInformationIfRequired(job, null);
				}
			}
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x000E3558 File Offset: 0x000E1758
		private void WriteJobStateInformation(Job job, JobStateEventArgs args = null)
		{
			bool flag;
			this._eventArgsWritten.TryGetValue(job.InstanceId, out flag);
			if (flag)
			{
				this._tracer.WriteMessage("ReceiveJobCommand", "WriteJobStateInformation", Guid.Empty, job, "State information already written, skipping another write", null);
				return;
			}
			JobStateEventArgs obj = args ?? new JobStateEventArgs(job.JobStateInfo);
			this._eventArgsWritten[job.InstanceId] = true;
			this._tracer.WriteMessage("ReceiveJobCommand", "WriteJobStateInformation", Guid.Empty, job, "Writing job state changed event args", null);
			PSObject psobject = new PSObject(obj);
			psobject.Properties.Add(new PSNoteProperty(RemotingConstants.EventObject, true));
			this._results.Add(new PSStreamObject(PSStreamObjectType.Output, psobject, job.InstanceId));
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x000E361C File Offset: 0x000E181C
		private void WriteJobStateInformationIfRequired(Job job, JobStateEventArgs args = null)
		{
			if (this._writeStateChangedEvents && job.IsPersistentState(job.JobStateInfo.State))
			{
				this.WriteJobStateInformation(job, args);
			}
			this.AutoRemoveJobIfRequired(job);
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x000E3648 File Offset: 0x000E1848
		private void ValidateWait()
		{
			if (this._wait && !this.flush)
			{
				throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.BlockCannotBeUsedWithKeep, new object[0]);
			}
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x000E366B File Offset: 0x000E186B
		private void ValidateWriteEvents()
		{
			if (this._writeStateChangedEvents && !this._wait)
			{
				throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.WriteEventsCannotBeUsedWithoutWait, new object[0]);
			}
		}

		// Token: 0x060028B2 RID: 10418 RVA: 0x000E368E File Offset: 0x000E188E
		private void ValidateAutoRemove()
		{
			if (this._autoRemoveJob && !this._wait)
			{
				throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.AutoRemoveCannotBeUsedWithoutWait, new object[0]);
			}
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x000E36B1 File Offset: 0x000E18B1
		private void ValidateForce()
		{
			if (this.Force && !this._wait)
			{
				throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.ForceCannotBeUsedWithoutWait, new object[0]);
			}
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x000E36D9 File Offset: 0x000E18D9
		private void ValidateWriteJobInResults()
		{
			if (this._outputJobFirst && !this._wait)
			{
				throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.WriteJobInResultsCannotBeUsedWithoutWait, new object[0]);
			}
		}

		// Token: 0x060028B5 RID: 10421 RVA: 0x000E36FC File Offset: 0x000E18FC
		private void SetOutputProcessingState(bool processingOutput)
		{
			bool flag2;
			lock (this._syncObject)
			{
				flag2 = (processingOutput != this._processingOutput);
				if (flag2)
				{
					this._processingOutput = processingOutput;
				}
			}
			if (this._outputProcessingNotification != null && flag2)
			{
				this._outputProcessingNotification.RaiseOutputProcessingStateChangedEvent(processingOutput);
			}
		}

		// Token: 0x040013F4 RID: 5108
		protected const string LocationParameterSet = "Location";

		// Token: 0x040013F5 RID: 5109
		private const string ClassNameTrace = "ReceiveJobCommand";

		// Token: 0x040013F6 RID: 5110
		private Job[] jobs;

		// Token: 0x040013F7 RID: 5111
		private string[] computerNames;

		// Token: 0x040013F8 RID: 5112
		private string[] locations;

		// Token: 0x040013F9 RID: 5113
		private PSSession[] remoteRunspaceInfos;

		// Token: 0x040013FA RID: 5114
		private bool flush = true;

		// Token: 0x040013FB RID: 5115
		private bool recurse = true;

		// Token: 0x040013FC RID: 5116
		private bool _autoRemoveJob;

		// Token: 0x040013FD RID: 5117
		private bool _writeStateChangedEvents;

		// Token: 0x040013FE RID: 5118
		private bool _wait;

		// Token: 0x040013FF RID: 5119
		private bool _isStopping;

		// Token: 0x04001400 RID: 5120
		private bool _isDisposed;

		// Token: 0x04001401 RID: 5121
		private readonly ReaderWriterLockSlim _resultsReaderWriterLock = new ReaderWriterLockSlim();

		// Token: 0x04001402 RID: 5122
		private readonly PowerShellTraceSource _tracer = PowerShellTraceSourceFactory.GetTraceSource();

		// Token: 0x04001403 RID: 5123
		private readonly ManualResetEvent _writeExistingData = new ManualResetEvent(true);

		// Token: 0x04001404 RID: 5124
		private readonly PSDataCollection<PSStreamObject> _results = new PSDataCollection<PSStreamObject>();

		// Token: 0x04001405 RID: 5125
		private bool _holdingResultsRef;

		// Token: 0x04001406 RID: 5126
		private readonly List<Job> _jobsBeingAggregated = new List<Job>();

		// Token: 0x04001407 RID: 5127
		private readonly List<Guid> _jobsSpecifiedInParameters = new List<Guid>();

		// Token: 0x04001408 RID: 5128
		private readonly object _syncObject = new object();

		// Token: 0x04001409 RID: 5129
		private bool _outputJobFirst;

		// Token: 0x0400140A RID: 5130
		private OutputProcessingState _outputProcessingNotification;

		// Token: 0x0400140B RID: 5131
		private bool _processingOutput;

		// Token: 0x0400140C RID: 5132
		private readonly Dictionary<Guid, bool> _eventArgsWritten = new Dictionary<Guid, bool>();
	}
}
