using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Remoting.Client;
using System.Management.Automation.Remoting.Internal;
using System.Management.Automation.Runspaces;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x02000278 RID: 632
	internal class PSRemotingChildJob : Job, IJobDebugger
	{
		// Token: 0x06001DB4 RID: 7604 RVA: 0x000AB230 File Offset: 0x000A9430
		internal PSRemotingChildJob(string remoteCommand, ExecutionCmdletHelper helper, ThrottleManager throttleManager) : base(remoteCommand)
		{
			base.UsesResultsCollection = true;
			this.helper = helper;
			this.remoteRunspace = helper.Pipeline.Runspace;
			this.remotePipeline = (helper.Pipeline as RemotePipeline);
			this.throttleManager = throttleManager;
			RemoteRunspace remoteRunspace = this.remoteRunspace as RemoteRunspace;
			if (remoteRunspace != null && remoteRunspace.RunspaceStateInfo.State == RunspaceState.BeforeOpen)
			{
				remoteRunspace.URIRedirectionReported += this.HandleURIDirectionReported;
			}
			this.AggregateResultsFromHelper(helper);
			this.remoteRunspace.AvailabilityChanged += this.HandleRunspaceAvailabilityChanged;
			this.RegisterThrottleComplete(throttleManager);
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x000AB2E8 File Offset: 0x000A94E8
		internal PSRemotingChildJob(ExecutionCmdletHelper helper, ThrottleManager throttleManager, bool aggregateResults = false)
		{
			base.UsesResultsCollection = true;
			this.helper = helper;
			this.remotePipeline = (helper.Pipeline as RemotePipeline);
			this.remoteRunspace = helper.Pipeline.Runspace;
			this.throttleManager = throttleManager;
			if (aggregateResults)
			{
				this.AggregateResultsFromHelper(helper);
			}
			else
			{
				this.remotePipeline.StateChanged += this.HandlePipelineStateChanged;
				this.remotePipeline.Output.DataReady += this.HandleOutputReady;
				this.remotePipeline.Error.DataReady += this.HandleErrorReady;
			}
			this.remoteRunspace.AvailabilityChanged += this.HandleRunspaceAvailabilityChanged;
			helper.OperationComplete += this.HandleOperationComplete;
			base.SetJobState(JobState.Disconnected, null);
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x000AB3DB File Offset: 0x000A95DB
		protected PSRemotingChildJob()
		{
		}

		// Token: 0x06001DB7 RID: 7607 RVA: 0x000AB3FC File Offset: 0x000A95FC
		internal void ConnectAsync()
		{
			if (base.JobStateInfo.State != JobState.Disconnected)
			{
				throw new InvalidJobStateException(base.JobStateInfo.State);
			}
			this.remotePipeline.ConnectAsync();
		}

		// Token: 0x06001DB8 RID: 7608 RVA: 0x000AB428 File Offset: 0x000A9628
		internal void RemoveJobAggregation()
		{
			this.RemoveAggreateCallbacksFromHelper(this.helper);
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x000AB438 File Offset: 0x000A9638
		public override void StopJob()
		{
			if (this.isDisposed || this.stopIsCalled || base.IsFinishedState(base.JobStateInfo.State))
			{
				return;
			}
			lock (this.SyncObject)
			{
				if (this.isDisposed || this.stopIsCalled || base.IsFinishedState(base.JobStateInfo.State))
				{
					return;
				}
				this.stopIsCalled = true;
			}
			this.throttleManager.StopOperation(this.helper);
			base.Finished.WaitOne();
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06001DBA RID: 7610 RVA: 0x000AB4E0 File Offset: 0x000A96E0
		public override string StatusMessage
		{
			get
			{
				return "";
			}
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06001DBB RID: 7611 RVA: 0x000AB4E7 File Offset: 0x000A96E7
		public override bool HasMoreData
		{
			get
			{
				return base.Results.IsOpen || base.Results.Count > 0;
			}
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06001DBC RID: 7612 RVA: 0x000AB506 File Offset: 0x000A9706
		public override string Location
		{
			get
			{
				if (this.remoteRunspace == null)
				{
					return string.Empty;
				}
				return this.remoteRunspace.ConnectionInfo.ComputerName;
			}
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06001DBD RID: 7613 RVA: 0x000AB526 File Offset: 0x000A9726
		public Runspace Runspace
		{
			get
			{
				return this.remoteRunspace;
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06001DBE RID: 7614 RVA: 0x000AB52E File Offset: 0x000A972E
		internal ExecutionCmdletHelper Helper
		{
			get
			{
				return this.helper;
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06001DBF RID: 7615 RVA: 0x000AB536 File Offset: 0x000A9736
		// (set) Token: 0x06001DC0 RID: 7616 RVA: 0x000AB540 File Offset: 0x000A9740
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

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06001DC1 RID: 7617 RVA: 0x000AB5A0 File Offset: 0x000A97A0
		internal bool DisconnectedAndBlocked
		{
			get
			{
				return this.disconnectedBlocked;
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06001DC2 RID: 7618 RVA: 0x000AB5A8 File Offset: 0x000A97A8
		internal override bool CanDisconnect
		{
			get
			{
				RemoteRunspace remoteRunspace = this.remoteRunspace as RemoteRunspace;
				return remoteRunspace != null && remoteRunspace.CanDisconnect;
			}
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06001DC3 RID: 7619 RVA: 0x000AB5CC File Offset: 0x000A97CC
		public Debugger Debugger
		{
			get
			{
				if (this._jobDebugger == null)
				{
					lock (this.SyncObject)
					{
						if (this._jobDebugger == null && this.remoteRunspace.Debugger != null)
						{
							this._jobDebugger = new RemotingJobDebugger(this.remoteRunspace.Debugger, this.remoteRunspace, base.Name);
						}
					}
				}
				return this._jobDebugger;
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06001DC4 RID: 7620 RVA: 0x000AB654 File Offset: 0x000A9854
		// (set) Token: 0x06001DC5 RID: 7621 RVA: 0x000AB65C File Offset: 0x000A985C
		public bool IsAsync
		{
			get
			{
				return this._isAsync;
			}
			set
			{
				this._isAsync = true;
			}
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x000AB668 File Offset: 0x000A9868
		private void HandleOutputReady(object sender, EventArgs eventArgs)
		{
			PSDataCollectionPipelineReader<PSObject, PSObject> psdataCollectionPipelineReader = sender as PSDataCollectionPipelineReader<PSObject, PSObject>;
			Collection<PSObject> collection = psdataCollectionPipelineReader.NonBlockingRead();
			foreach (PSObject psobject in collection)
			{
				if (psobject != null)
				{
					if (psobject.Properties[RemotingConstants.ComputerNameNoteProperty] != null)
					{
						psobject.Properties.Remove(RemotingConstants.ComputerNameNoteProperty);
					}
					if (psobject.Properties[RemotingConstants.RunspaceIdNoteProperty] != null)
					{
						psobject.Properties.Remove(RemotingConstants.RunspaceIdNoteProperty);
					}
					psobject.Properties.Add(new PSNoteProperty(RemotingConstants.ComputerNameNoteProperty, psdataCollectionPipelineReader.ComputerName));
					psobject.Properties.Add(new PSNoteProperty(RemotingConstants.RunspaceIdNoteProperty, psdataCollectionPipelineReader.RunspaceId));
					if (psobject.Properties[RemotingConstants.ShowComputerNameNoteProperty] == null)
					{
						PSNoteProperty member = new PSNoteProperty(RemotingConstants.ShowComputerNameNoteProperty, !this.hideComputerName);
						psobject.Properties.Add(member);
					}
				}
				this.WriteObject(psobject);
			}
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x000AB788 File Offset: 0x000A9988
		private void HandleErrorReady(object sender, EventArgs eventArgs)
		{
			PSDataCollectionPipelineReader<ErrorRecord, object> psdataCollectionPipelineReader = sender as PSDataCollectionPipelineReader<ErrorRecord, object>;
			Collection<object> collection = psdataCollectionPipelineReader.NonBlockingRead();
			foreach (object obj in collection)
			{
				ErrorRecord errorRecord = obj as ErrorRecord;
				if (errorRecord != null)
				{
					OriginInfo originInfo = new OriginInfo(psdataCollectionPipelineReader.ComputerName, psdataCollectionPipelineReader.RunspaceId);
					this.WriteError(new RemotingErrorRecord(errorRecord, originInfo)
					{
						PreserveInvocationInfoOnce = true
					});
				}
			}
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x000AB814 File Offset: 0x000A9A14
		protected void HandleURIDirectionReported(object sender, RemoteDataEventArgs<Uri> eventArgs)
		{
			string message = StringUtil.Format(RemotingErrorIdStrings.URIRedirectWarningToHost, eventArgs.Data.OriginalString);
			this.WriteWarning(message);
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x000AB840 File Offset: 0x000A9A40
		protected void HandleRunspaceAvailabilityChangedForInvoke(object sender, RunspaceAvailabilityEventArgs e)
		{
			RemoteRunspace remoteRunspace = sender as RemoteRunspace;
			if (remoteRunspace != null && e.RunspaceAvailability == RunspaceAvailability.RemoteDebug)
			{
				remoteRunspace.AvailabilityChanged -= this.HandleRunspaceAvailabilityChangedForInvoke;
				try
				{
					remoteRunspace.DisconnectAsync();
				}
				catch (PSNotImplementedException)
				{
				}
				catch (InvalidRunspacePoolStateException)
				{
				}
				catch (InvalidRunspaceStateException)
				{
				}
				catch (PSInvalidOperationException)
				{
				}
			}
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x000AB8BC File Offset: 0x000A9ABC
		private void HandleHostCalls(object sender, EventArgs eventArgs)
		{
			ObjectStream objectStream = sender as ObjectStream;
			if (objectStream != null)
			{
				Collection<object> collection = objectStream.NonBlockingRead(objectStream.Count);
				lock (this.SyncObject)
				{
					foreach (object obj in collection)
					{
						ClientMethodExecutor clientMethodExecutor = (ClientMethodExecutor)obj;
						base.Results.Add(new PSStreamObject(PSStreamObjectType.MethodExecutor, clientMethodExecutor));
						if (clientMethodExecutor.RemoteHostCall.CallId != -100L)
						{
							base.SetJobState(JobState.Blocked, null);
						}
					}
				}
			}
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x000AB978 File Offset: 0x000A9B78
		protected virtual void HandlePipelineStateChanged(object sender, PipelineStateEventArgs e)
		{
			if (this.remoteRunspace != null && e.PipelineStateInfo.State != PipelineState.Running)
			{
				((RemoteRunspace)this.remoteRunspace).URIRedirectionReported -= this.HandleURIDirectionReported;
				((RemoteRunspace)this.remoteRunspace).AvailabilityChanged -= this.HandleRunspaceAvailabilityChangedForInvoke;
			}
			PipelineState state = e.PipelineStateInfo.State;
			PipelineState pipelineState = state;
			if (pipelineState != PipelineState.Running)
			{
				if (pipelineState != PipelineState.Disconnected)
				{
					return;
				}
				this.disconnectedBlocked = (base.JobStateInfo.State == JobState.Blocked);
				base.SetJobState(JobState.Disconnected);
				return;
			}
			else
			{
				if (this.disconnectedBlocked)
				{
					this.disconnectedBlocked = false;
					base.SetJobState(JobState.Blocked);
					return;
				}
				base.SetJobState(JobState.Running);
				return;
			}
		}

		// Token: 0x06001DCC RID: 7628 RVA: 0x000ABA24 File Offset: 0x000A9C24
		private void HandleThrottleComplete(object sender, EventArgs eventArgs)
		{
			this.DoFinish();
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x000ABA2C File Offset: 0x000A9C2C
		protected virtual void HandleOperationComplete(object sender, OperationStateEventArgs stateEventArgs)
		{
			ExecutionCmdletHelper executionCmdletHelper = sender as ExecutionCmdletHelper;
			this.DeterminedAndSetJobState(executionCmdletHelper);
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x000ABA48 File Offset: 0x000A9C48
		protected virtual void DoFinish()
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
			this.DeterminedAndSetJobState(this.helper);
			this.DoCleanupOnFinished();
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06001DCF RID: 7631 RVA: 0x000ABAB0 File Offset: 0x000A9CB0
		internal ErrorRecord FailureErrorRecord
		{
			get
			{
				return this.failureErrorRecord;
			}
		}

		// Token: 0x06001DD0 RID: 7632 RVA: 0x000ABAB8 File Offset: 0x000A9CB8
		protected void ProcessJobFailure(ExecutionCmdletHelper helper, out Exception failureException, out ErrorRecord failureErrorRecord)
		{
			RemotePipeline remotePipeline = helper.Pipeline as RemotePipeline;
			RemoteRunspace remoteRunspace = remotePipeline.GetRunspace() as RemoteRunspace;
			failureException = null;
			failureErrorRecord = null;
			if (helper.InternalException != null)
			{
				string text = "RemotePipelineExecutionFailed";
				failureException = helper.InternalException;
				if (failureException is InvalidRunspaceStateException || failureException is InvalidRunspacePoolStateException)
				{
					text = "InvalidSessionState";
					if (!string.IsNullOrEmpty(failureException.Source))
					{
						text = string.Format(CultureInfo.InvariantCulture, "{0},{1}", new object[]
						{
							text,
							failureException.Source
						});
					}
				}
				failureErrorRecord = new ErrorRecord(helper.InternalException, text, ErrorCategory.OperationStopped, helper);
				return;
			}
			if (remoteRunspace.RunspaceStateInfo.State == RunspaceState.Broken)
			{
				failureException = remoteRunspace.RunspaceStateInfo.Reason;
				object computerName = remoteRunspace.ConnectionInfo.ComputerName;
				string text2 = null;
				PSRemotingTransportException ex = failureException as PSRemotingTransportException;
				string fqeidfromTransportError = WSManTransportManagerUtils.GetFQEIDFromTransportError((ex != null) ? ex.ErrorCode : 0, "PSSessionStateBroken");
				if (ex != null)
				{
					text2 = "[" + remoteRunspace.ConnectionInfo.ComputerName + "] ";
					if (ex.ErrorCode == -2144108135)
					{
						string str = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.URIRedirectionReported, new object[]
						{
							ex.Message,
							"MaximumConnectionRedirectionCount",
							"PSSessionOption",
							"AllowRedirection"
						});
						text2 += str;
					}
					else if (!string.IsNullOrEmpty(ex.Message))
					{
						text2 += ex.Message;
					}
					else if (!string.IsNullOrEmpty(ex.TransportMessage))
					{
						text2 += ex.TransportMessage;
					}
				}
				if (failureException == null)
				{
					failureException = new RuntimeException(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.RemoteRunspaceOpenUnknownState, new object[]
					{
						remoteRunspace.RunspaceStateInfo.State
					}));
				}
				failureErrorRecord = new ErrorRecord(failureException, computerName, fqeidfromTransportError, ErrorCategory.OpenError, null, null, null, null, null, text2, null);
				return;
			}
			if (remotePipeline.PipelineStateInfo.State == PipelineState.Failed)
			{
				object computerName2 = remoteRunspace.ConnectionInfo.ComputerName;
				failureException = remotePipeline.PipelineStateInfo.Reason;
				if (failureException != null)
				{
					RemoteException ex2 = failureException as RemoteException;
					ErrorRecord errorRecord;
					if (ex2 != null)
					{
						errorRecord = ex2.ErrorRecord;
					}
					else
					{
						errorRecord = new ErrorRecord(remotePipeline.PipelineStateInfo.Reason, "JobFailure", ErrorCategory.OperationStopped, computerName2);
					}
					string computerName3 = ((RemoteRunspace)remotePipeline.GetRunspace()).ConnectionInfo.ComputerName;
					Guid instanceId = remotePipeline.GetRunspace().InstanceId;
					OriginInfo originInfo = new OriginInfo(computerName3, instanceId);
					failureErrorRecord = new RemotingErrorRecord(errorRecord, originInfo);
				}
			}
		}

		// Token: 0x06001DD1 RID: 7633 RVA: 0x000ABD4C File Offset: 0x000A9F4C
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.isDisposed)
				{
					return;
				}
				lock (this.SyncObject)
				{
					if (this.isDisposed)
					{
						return;
					}
					this.isDisposed = true;
				}
				try
				{
					this.DoCleanupOnFinished();
				}
				finally
				{
					base.Dispose(disposing);
				}
			}
		}

		// Token: 0x06001DD2 RID: 7634 RVA: 0x000ABDC0 File Offset: 0x000A9FC0
		protected virtual void DoCleanupOnFinished()
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
			this.StopAggregateResultsFromHelper(this.helper);
			this.remoteRunspace.AvailabilityChanged -= this.HandleRunspaceAvailabilityChanged;
			IThrottleOperation throttleOperation = this.helper;
			throttleOperation.OperationComplete -= this.HandleOperationComplete;
			this.UnregisterThrottleComplete(this.throttleManager);
			this.throttleManager = null;
		}

		// Token: 0x06001DD3 RID: 7635 RVA: 0x000ABE6C File Offset: 0x000AA06C
		protected void AggregateResultsFromHelper(ExecutionCmdletHelper helper)
		{
			Pipeline pipeline = helper.Pipeline;
			pipeline.Output.DataReady += this.HandleOutputReady;
			pipeline.Error.DataReady += this.HandleErrorReady;
			pipeline.StateChanged += this.HandlePipelineStateChanged;
			RemotePipeline remotePipeline = pipeline as RemotePipeline;
			remotePipeline.MethodExecutorStream.DataReady += this.HandleHostCalls;
			remotePipeline.PowerShell.Streams.Progress.DataAdded += this.HandleProgressAdded;
			remotePipeline.PowerShell.Streams.Warning.DataAdded += this.HandleWarningAdded;
			remotePipeline.PowerShell.Streams.Verbose.DataAdded += this.HandleVerboseAdded;
			remotePipeline.PowerShell.Streams.Debug.DataAdded += this.HandleDebugAdded;
			remotePipeline.PowerShell.Streams.Information.DataAdded += this.HandleInformationAdded;
			remotePipeline.IsMethodExecutorStreamEnabled = true;
			helper.OperationComplete += this.HandleOperationComplete;
		}

		// Token: 0x06001DD4 RID: 7636 RVA: 0x000ABFA0 File Offset: 0x000AA1A0
		private PowerShell GetPipelinePowerShell(RemotePipeline pipeline, Guid instanceId)
		{
			if (pipeline != null)
			{
				return pipeline.PowerShell;
			}
			return this.GetPowerShell(instanceId);
		}

		// Token: 0x06001DD5 RID: 7637 RVA: 0x000ABFB4 File Offset: 0x000AA1B4
		private void HandleDebugAdded(object sender, DataAddedEventArgs eventArgs)
		{
			int index = eventArgs.Index;
			PowerShell pipelinePowerShell = this.GetPipelinePowerShell(this.remotePipeline, eventArgs.PowerShellInstanceId);
			if (pipelinePowerShell != null)
			{
				base.Debug.Add(pipelinePowerShell.Streams.Debug[index]);
			}
		}

		// Token: 0x06001DD6 RID: 7638 RVA: 0x000ABFFC File Offset: 0x000AA1FC
		private void HandleVerboseAdded(object sender, DataAddedEventArgs eventArgs)
		{
			int index = eventArgs.Index;
			PowerShell pipelinePowerShell = this.GetPipelinePowerShell(this.remotePipeline, eventArgs.PowerShellInstanceId);
			if (pipelinePowerShell != null)
			{
				base.Verbose.Add(pipelinePowerShell.Streams.Verbose[index]);
			}
		}

		// Token: 0x06001DD7 RID: 7639 RVA: 0x000AC044 File Offset: 0x000AA244
		private void HandleWarningAdded(object sender, DataAddedEventArgs eventArgs)
		{
			int index = eventArgs.Index;
			PowerShell pipelinePowerShell = this.GetPipelinePowerShell(this.remotePipeline, eventArgs.PowerShellInstanceId);
			if (pipelinePowerShell != null)
			{
				WarningRecord warningRecord = pipelinePowerShell.Streams.Warning[index];
				base.Warning.Add(warningRecord);
				base.Results.Add(new PSStreamObject(PSStreamObjectType.WarningRecord, warningRecord));
			}
		}

		// Token: 0x06001DD8 RID: 7640 RVA: 0x000AC0A0 File Offset: 0x000AA2A0
		private void HandleProgressAdded(object sender, DataAddedEventArgs eventArgs)
		{
			int index = eventArgs.Index;
			PowerShell pipelinePowerShell = this.GetPipelinePowerShell(this.remotePipeline, eventArgs.PowerShellInstanceId);
			if (pipelinePowerShell != null)
			{
				base.Progress.Add(pipelinePowerShell.Streams.Progress[index]);
			}
		}

		// Token: 0x06001DD9 RID: 7641 RVA: 0x000AC0E8 File Offset: 0x000AA2E8
		private void HandleInformationAdded(object sender, DataAddedEventArgs eventArgs)
		{
			int index = eventArgs.Index;
			PowerShell pipelinePowerShell = this.GetPipelinePowerShell(this.remotePipeline, eventArgs.PowerShellInstanceId);
			if (pipelinePowerShell != null)
			{
				InformationRecord informationRecord = pipelinePowerShell.Streams.Information[index];
				base.Information.Add(informationRecord);
				if (informationRecord.Tags.Contains("PSHOST"))
				{
					informationRecord.Tags.Add("FORWARDED");
				}
				base.Results.Add(new PSStreamObject(PSStreamObjectType.Information, informationRecord));
			}
		}

		// Token: 0x06001DDA RID: 7642 RVA: 0x000AC168 File Offset: 0x000AA368
		protected void StopAggregateResultsFromHelper(ExecutionCmdletHelper helper)
		{
			if (helper.PipelineRunspace != null)
			{
				helper.PipelineRunspace.AvailabilityChanged -= this.HandleRunspaceAvailabilityChangedForInvoke;
			}
			this.RemoveAggreateCallbacksFromHelper(helper);
			Pipeline pipeline = helper.Pipeline;
			pipeline.Dispose();
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x000AC1AC File Offset: 0x000AA3AC
		protected void RemoveAggreateCallbacksFromHelper(ExecutionCmdletHelper helper)
		{
			Pipeline pipeline = helper.Pipeline;
			pipeline.Output.DataReady -= this.HandleOutputReady;
			pipeline.Error.DataReady -= this.HandleErrorReady;
			pipeline.StateChanged -= this.HandlePipelineStateChanged;
			RemotePipeline remotePipeline = pipeline as RemotePipeline;
			remotePipeline.MethodExecutorStream.DataReady -= this.HandleHostCalls;
			if (remotePipeline.PowerShell != null)
			{
				remotePipeline.PowerShell.Streams.Progress.DataAdded -= this.HandleProgressAdded;
				remotePipeline.PowerShell.Streams.Warning.DataAdded -= this.HandleWarningAdded;
				remotePipeline.PowerShell.Streams.Verbose.DataAdded -= this.HandleVerboseAdded;
				remotePipeline.PowerShell.Streams.Debug.DataAdded -= this.HandleDebugAdded;
				remotePipeline.PowerShell.Streams.Information.DataAdded -= this.HandleInformationAdded;
				remotePipeline.IsMethodExecutorStreamEnabled = false;
			}
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x000AC2D6 File Offset: 0x000AA4D6
		protected void RegisterThrottleComplete(ThrottleManager throttleManager)
		{
			throttleManager.ThrottleComplete += this.HandleThrottleComplete;
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x000AC2EA File Offset: 0x000AA4EA
		protected void UnregisterThrottleComplete(ThrottleManager throttleManager)
		{
			throttleManager.ThrottleComplete -= this.HandleThrottleComplete;
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x000AC300 File Offset: 0x000AA500
		protected void DeterminedAndSetJobState(ExecutionCmdletHelper helper)
		{
			Exception ex;
			this.ProcessJobFailure(helper, out ex, out this.failureErrorRecord);
			if (ex != null)
			{
				base.SetJobState(JobState.Failed, ex);
				return;
			}
			PipelineState state = helper.Pipeline.PipelineStateInfo.State;
			if (state == PipelineState.NotStarted)
			{
				base.SetJobState(JobState.Stopped);
				return;
			}
			if (state == PipelineState.Completed)
			{
				base.SetJobState(JobState.Completed);
				return;
			}
			base.SetJobState(JobState.Stopped);
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x000AC357 File Offset: 0x000AA557
		internal void UnblockJob()
		{
			base.SetJobState(JobState.Running, null);
			this.JobUnblocked.SafeInvoke(this, EventArgs.Empty);
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x000AC372 File Offset: 0x000AA572
		internal virtual PowerShell GetPowerShell(Guid instanceId)
		{
			throw PSTraceSource.NewInvalidOperationException();
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x000AC37C File Offset: 0x000AA57C
		internal PowerShell GetPowerShell()
		{
			PowerShell result = null;
			if (this.remotePipeline != null)
			{
				result = this.remotePipeline.PowerShell;
			}
			return result;
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x000AC3A0 File Offset: 0x000AA5A0
		private void HandleRunspaceAvailabilityChanged(object sender, RunspaceAvailabilityEventArgs e)
		{
			RunspaceAvailability prevRunspaceAvailability = this._prevRunspaceAvailability;
			this._prevRunspaceAvailability = e.RunspaceAvailability;
			if (e.RunspaceAvailability == RunspaceAvailability.RemoteDebug)
			{
				base.SetJobState(JobState.AtBreakpoint);
				return;
			}
			if (prevRunspaceAvailability == RunspaceAvailability.RemoteDebug && e.RunspaceAvailability == RunspaceAvailability.Busy)
			{
				base.SetJobState(JobState.Running);
			}
		}

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x06001DE3 RID: 7651 RVA: 0x000AC3E8 File Offset: 0x000AA5E8
		// (remove) Token: 0x06001DE4 RID: 7652 RVA: 0x000AC420 File Offset: 0x000AA620
		internal event EventHandler JobUnblocked;

		// Token: 0x04000D28 RID: 3368
		private Runspace remoteRunspace;

		// Token: 0x04000D29 RID: 3369
		private bool hideComputerName = true;

		// Token: 0x04000D2A RID: 3370
		private bool doFinishCalled;

		// Token: 0x04000D2B RID: 3371
		private ErrorRecord failureErrorRecord;

		// Token: 0x04000D2C RID: 3372
		private bool isDisposed;

		// Token: 0x04000D2D RID: 3373
		private bool cleanupDone;

		// Token: 0x04000D2F RID: 3375
		private ExecutionCmdletHelper helper;

		// Token: 0x04000D30 RID: 3376
		private RemotePipeline remotePipeline;

		// Token: 0x04000D31 RID: 3377
		protected object SyncObject = new object();

		// Token: 0x04000D32 RID: 3378
		private ThrottleManager throttleManager;

		// Token: 0x04000D33 RID: 3379
		private bool stopIsCalled;

		// Token: 0x04000D34 RID: 3380
		private bool disconnectedBlocked;

		// Token: 0x04000D35 RID: 3381
		private volatile Debugger _jobDebugger;

		// Token: 0x04000D36 RID: 3382
		private bool _isAsync = true;

		// Token: 0x04000D37 RID: 3383
		private RunspaceAvailability _prevRunspaceAvailability;
	}
}
