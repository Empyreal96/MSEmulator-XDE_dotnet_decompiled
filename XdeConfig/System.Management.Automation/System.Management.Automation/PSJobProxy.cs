using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Tracing;
using System.Threading;
using Microsoft.PowerShell;

namespace System.Management.Automation
{
	// Token: 0x02000284 RID: 644
	public sealed class PSJobProxy : Job2
	{
		// Token: 0x06001E9D RID: 7837 RVA: 0x000B0EA0 File Offset: 0x000AF0A0
		internal PSJobProxy(string command) : base(command)
		{
			this._tracer.WriteMessage("PSJobProxy", "ctor", this._remoteJobInstanceId, this, "Constructing proxy job", null);
			base.StateChanged += this.HandleMyStateChange;
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06001E9E RID: 7838 RVA: 0x000B0F5E File Offset: 0x000AF15E
		public override string StatusMessage
		{
			get
			{
				return this._remoteJobStatusMessage;
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06001E9F RID: 7839 RVA: 0x000B0F70 File Offset: 0x000AF170
		public override bool HasMoreData
		{
			get
			{
				if (this._moreData && base.IsFinishedState(base.JobStateInfo.State))
				{
					bool flag = base.ChildJobs.Any((Job t) => t.HasMoreData);
					this._moreData = (PSJobProxy.CollectionHasMoreData<PSObject>(base.Output) || PSJobProxy.CollectionHasMoreData<ErrorRecord>(base.Error) || PSJobProxy.CollectionHasMoreData<VerboseRecord>(base.Verbose) || PSJobProxy.CollectionHasMoreData<DebugRecord>(base.Debug) || PSJobProxy.CollectionHasMoreData<WarningRecord>(base.Warning) || PSJobProxy.CollectionHasMoreData<ProgressRecord>(base.Progress) || flag);
				}
				return this._moreData;
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06001EA0 RID: 7840 RVA: 0x000B102A File Offset: 0x000AF22A
		public override string Location
		{
			get
			{
				return this._remoteJobLocation;
			}
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x000B1032 File Offset: 0x000AF232
		public override void StartJob()
		{
			this.StartJob(null, null, null);
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x000B103D File Offset: 0x000AF23D
		public override void StartJobAsync()
		{
			this.StartJobAsync(null, null, null);
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x000B1048 File Offset: 0x000AF248
		public override void StopJob()
		{
			try
			{
				if (this.ShouldQueueOperation())
				{
					this._pendingOperations.Enqueue(PSJobProxy.QueueOperation.Stop);
				}
				else
				{
					this.DoStopAsync();
				}
				base.Finished.WaitOne();
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				this._tracer.WriteMessage("PSJobProxy", "StopJob", this._remoteJobInstanceId, this, "Error", null);
				this._tracer.TraceException(ex);
				throw;
			}
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x000B10C8 File Offset: 0x000AF2C8
		public override void StopJobAsync()
		{
			try
			{
				if (this.ShouldQueueOperation())
				{
					this._pendingOperations.Enqueue(PSJobProxy.QueueOperation.Stop);
				}
				else
				{
					this.DoStopAsync();
				}
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				this._tracer.WriteMessage("PSJobProxy", "StopJobAsync", this._remoteJobInstanceId, this, "Error", null);
				this._tracer.TraceException(ex);
				this.OnStopJobCompleted(new AsyncCompletedEventArgs(ex, false, null));
			}
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x000B114C File Offset: 0x000AF34C
		public override void StopJob(bool force, string reason)
		{
			throw PSTraceSource.NewNotSupportedException(PowerShellStrings.ProxyJobControlNotSupported, new object[0]);
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x000B115E File Offset: 0x000AF35E
		public override void StopJobAsync(bool force, string reason)
		{
			throw PSTraceSource.NewNotSupportedException(PowerShellStrings.ProxyJobControlNotSupported, new object[0]);
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x000B1170 File Offset: 0x000AF370
		public override void SuspendJob()
		{
			try
			{
				if (this.ShouldQueueOperation())
				{
					this._pendingOperations.Enqueue(PSJobProxy.QueueOperation.Suspend);
				}
				else
				{
					this.DoSuspendAsync();
				}
				this.JobSuspendedOrFinished.WaitOne();
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				this._tracer.WriteMessage("PSJobProxy", "SuspendJob", this._remoteJobInstanceId, this, "Error", null);
				this._tracer.TraceException(ex);
				throw;
			}
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x000B11F0 File Offset: 0x000AF3F0
		public override void SuspendJobAsync()
		{
			try
			{
				if (this.ShouldQueueOperation())
				{
					this._pendingOperations.Enqueue(PSJobProxy.QueueOperation.Suspend);
				}
				else
				{
					this.DoSuspendAsync();
				}
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				this._tracer.WriteMessage("PSJobProxy", "SuspendJobAsync", this._remoteJobInstanceId, this, "Error", null);
				this._tracer.TraceException(ex);
				this.OnSuspendJobCompleted(new AsyncCompletedEventArgs(ex, false, null));
			}
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x000B1274 File Offset: 0x000AF474
		public override void SuspendJob(bool force, string reason)
		{
			throw PSTraceSource.NewNotSupportedException(PowerShellStrings.ProxyJobControlNotSupported, new object[0]);
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x000B1286 File Offset: 0x000AF486
		public override void SuspendJobAsync(bool force, string reason)
		{
			throw PSTraceSource.NewNotSupportedException(PowerShellStrings.ProxyJobControlNotSupported, new object[0]);
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x000B1298 File Offset: 0x000AF498
		public override void ResumeJob()
		{
			try
			{
				if (this.ShouldQueueOperation())
				{
					this._pendingOperations.Enqueue(PSJobProxy.QueueOperation.Resume);
				}
				else
				{
					this.DoResumeAsync();
				}
				this.JobRunningOrFinished.WaitOne();
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				this._tracer.WriteMessage("PSJobProxy", "ResumeJob", this._remoteJobInstanceId, this, "Error", null);
				this._tracer.TraceException(ex);
				throw;
			}
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x000B1318 File Offset: 0x000AF518
		public override void ResumeJobAsync()
		{
			try
			{
				if (this.ShouldQueueOperation())
				{
					this._pendingOperations.Enqueue(PSJobProxy.QueueOperation.Resume);
				}
				else
				{
					this.DoResumeAsync();
				}
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				this._tracer.WriteMessage("PSJobProxy", "ResumeJobAsync", this._remoteJobInstanceId, this, "Error", null);
				this._tracer.TraceException(ex);
				this.OnResumeJobCompleted(new AsyncCompletedEventArgs(ex, false, null));
			}
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x000B139C File Offset: 0x000AF59C
		public override void UnblockJob()
		{
			throw PSTraceSource.NewNotSupportedException(PowerShellStrings.ProxyUnblockJobNotSupported, new object[0]);
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x000B13AE File Offset: 0x000AF5AE
		public override void UnblockJobAsync()
		{
			this.OnUnblockJobCompleted(new AsyncCompletedEventArgs(PSTraceSource.NewNotSupportedException(PowerShellStrings.ProxyUnblockJobNotSupported, new object[0]), false, null));
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x000B13CD File Offset: 0x000AF5CD
		public void StartJobAsync(PSDataCollection<object> input)
		{
			this.StartJobAsync(null, null, input);
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x000B13D8 File Offset: 0x000AF5D8
		public void StartJob(PSDataCollection<object> input)
		{
			this.StartJob(null, null, input);
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x000B13E4 File Offset: 0x000AF5E4
		public void StartJob(EventHandler<JobDataAddedEventArgs> dataAdded, EventHandler<JobStateEventArgs> stateChanged, PSDataCollection<object> input)
		{
			try
			{
				this.DoStartAsync(dataAdded, stateChanged, input);
				this._jobInitialiedWaitHandle.WaitOne();
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				this._tracer.WriteMessage("PSJobProxy", "StartJob", this._remoteJobInstanceId, this, "Error", null);
				this._tracer.TraceException(ex);
				throw;
			}
		}

		// Token: 0x06001EB2 RID: 7858 RVA: 0x000B1450 File Offset: 0x000AF650
		public void StartJobAsync(EventHandler<JobDataAddedEventArgs> dataAdded, EventHandler<JobStateEventArgs> stateChanged, PSDataCollection<object> input)
		{
			try
			{
				this.DoStartAsync(dataAdded, stateChanged, input);
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				this._tracer.WriteMessage("PSJobProxy", "StartJobAsync", this._remoteJobInstanceId, this, "Error", null);
				this._tracer.TraceException(ex);
				this.OnStartJobCompleted(new AsyncCompletedEventArgs(ex, false, null));
			}
		}

		// Token: 0x06001EB3 RID: 7859 RVA: 0x000B14C0 File Offset: 0x000AF6C0
		public void RemoveJob(bool removeRemoteJob, bool force)
		{
			if (!removeRemoteJob)
			{
				base.Dispose();
				return;
			}
			lock (base.SyncRoot)
			{
				this.AssertNotDisposed();
			}
			try
			{
				this.DoRemove(force);
				lock (base.SyncRoot)
				{
					if (!this._removeCalled)
					{
						return;
					}
				}
				this.RemoveComplete.WaitOne();
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				this._tracer.WriteMessage("PSJobProxy", "RemoveJob", this._remoteJobInstanceId, this, "Error", null);
				this._tracer.TraceException(ex);
				throw;
			}
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x000B15A0 File Offset: 0x000AF7A0
		public void RemoveJob(bool removeRemoteJob)
		{
			this.RemoveJob(removeRemoteJob, false);
		}

		// Token: 0x06001EB5 RID: 7861 RVA: 0x000B15AC File Offset: 0x000AF7AC
		public void RemoveJobAsync(bool removeRemoteJob, bool force)
		{
			if (!removeRemoteJob)
			{
				base.Dispose();
				this.OnRemoveJobCompleted(new AsyncCompletedEventArgs(null, false, null));
				return;
			}
			AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(force);
			PSJobProxy.JobActionWorkerDelegate jobActionWorkerDelegate = new PSJobProxy.JobActionWorkerDelegate(this.JobActionWorker);
			jobActionWorkerDelegate.BeginInvoke(asyncOp, PSJobProxy.ActionType.Remove, null, null);
		}

		// Token: 0x06001EB6 RID: 7862 RVA: 0x000B15F5 File Offset: 0x000AF7F5
		public void RemoveJobAsync(bool removeRemoteJob)
		{
			this.RemoveJobAsync(removeRemoteJob, false);
		}

		// Token: 0x1400003C RID: 60
		// (add) Token: 0x06001EB7 RID: 7863 RVA: 0x000B1600 File Offset: 0x000AF800
		// (remove) Token: 0x06001EB8 RID: 7864 RVA: 0x000B1638 File Offset: 0x000AF838
		public event EventHandler<AsyncCompletedEventArgs> RemoveJobCompleted;

		// Token: 0x06001EB9 RID: 7865 RVA: 0x000B1670 File Offset: 0x000AF870
		private void OnRemoveJobCompleted(AsyncCompletedEventArgs eventArgs)
		{
			EventHandler<AsyncCompletedEventArgs> removeJobCompleted = this.RemoveJobCompleted;
			try
			{
				if (removeJobCompleted != null)
				{
					removeJobCompleted(this, eventArgs);
				}
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				this._tracer.TraceException(ex);
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06001EBA RID: 7866 RVA: 0x000B16B8 File Offset: 0x000AF8B8
		// (set) Token: 0x06001EBB RID: 7867 RVA: 0x000B16C0 File Offset: 0x000AF8C0
		public bool RemoveRemoteJobOnCompletion
		{
			get
			{
				return this._removeRemoteJobOnCompletion;
			}
			set
			{
				this.AssertChangesCanBeAccepted();
				this._removeRemoteJobOnCompletion = value;
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06001EBC RID: 7868 RVA: 0x000B16CF File Offset: 0x000AF8CF
		public Guid RemoteJobInstanceId
		{
			get
			{
				return this._remoteJobInstanceId;
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06001EBD RID: 7869 RVA: 0x000B16D7 File Offset: 0x000AF8D7
		// (set) Token: 0x06001EBE RID: 7870 RVA: 0x000B16E0 File Offset: 0x000AF8E0
		public Runspace Runspace
		{
			get
			{
				return this._runspace;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("value");
				}
				lock (base.SyncRoot)
				{
					this.AssertChangesCanBeAccepted();
					this._runspacePool = null;
					this._runspace = value;
				}
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06001EBF RID: 7871 RVA: 0x000B173C File Offset: 0x000AF93C
		// (set) Token: 0x06001EC0 RID: 7872 RVA: 0x000B1744 File Offset: 0x000AF944
		public RunspacePool RunspacePool
		{
			get
			{
				return this._runspacePool;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("value");
				}
				lock (base.SyncRoot)
				{
					this.AssertChangesCanBeAccepted();
					this._runspace = null;
					this._runspacePool = value;
				}
			}
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x000B17A0 File Offset: 0x000AF9A0
		public static ICollection<PSJobProxy> Create(Runspace runspace, Hashtable filter, EventHandler<JobDataAddedEventArgs> dataAdded, EventHandler<JobStateEventArgs> stateChanged)
		{
			return PSJobProxy.Create(runspace, filter, dataAdded, stateChanged, true);
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x000B17AC File Offset: 0x000AF9AC
		public static ICollection<PSJobProxy> Create(Runspace runspace, Hashtable filter, bool receiveImmediately)
		{
			return PSJobProxy.Create(runspace, filter, null, null, receiveImmediately);
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x000B17B8 File Offset: 0x000AF9B8
		public static ICollection<PSJobProxy> Create(Runspace runspace, Hashtable filter)
		{
			return PSJobProxy.Create(runspace, filter, null, null, true);
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x000B17C4 File Offset: 0x000AF9C4
		public static ICollection<PSJobProxy> Create(Runspace runspace)
		{
			return PSJobProxy.Create(runspace, null, null, null, true);
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x000B17D0 File Offset: 0x000AF9D0
		public static ICollection<PSJobProxy> Create(RunspacePool runspacePool, Hashtable filter, EventHandler<JobDataAddedEventArgs> dataAdded, EventHandler<JobStateEventArgs> stateChanged)
		{
			return PSJobProxy.Create(runspacePool, filter, dataAdded, stateChanged, true);
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x000B17DC File Offset: 0x000AF9DC
		public static ICollection<PSJobProxy> Create(RunspacePool runspacePool, Hashtable filter, bool receiveImmediately)
		{
			return PSJobProxy.Create(runspacePool, filter, null, null, receiveImmediately);
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x000B17E8 File Offset: 0x000AF9E8
		public static ICollection<PSJobProxy> Create(RunspacePool runspacePool, Hashtable filter)
		{
			return PSJobProxy.Create(runspacePool, filter, null, null, true);
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x000B17F4 File Offset: 0x000AF9F4
		public static ICollection<PSJobProxy> Create(RunspacePool runspacePool)
		{
			return PSJobProxy.Create(runspacePool, null, null, null, true);
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x000B1800 File Offset: 0x000AFA00
		private static ICollection<PSJobProxy> Create(RunspacePool runspacePool, Hashtable filter, EventHandler<JobDataAddedEventArgs> dataAdded, EventHandler<JobStateEventArgs> stateChanged, bool connectImmediately)
		{
			if (runspacePool == null)
			{
				throw new PSArgumentNullException("runspacePool");
			}
			return PSJobProxy.Create(null, runspacePool, filter, dataAdded, stateChanged, connectImmediately);
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x000B181C File Offset: 0x000AFA1C
		private static ICollection<PSJobProxy> Create(Runspace runspace, Hashtable filter, EventHandler<JobDataAddedEventArgs> dataAdded, EventHandler<JobStateEventArgs> stateChanged, bool connectImmediately)
		{
			if (runspace == null)
			{
				throw new PSArgumentNullException("runspace");
			}
			return PSJobProxy.Create(runspace, null, filter, dataAdded, stateChanged, connectImmediately);
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x000B1838 File Offset: 0x000AFA38
		private static ICollection<PSJobProxy> Create(Runspace runspace, RunspacePool runspacePool, Hashtable filter, EventHandler<JobDataAddedEventArgs> dataAdded, EventHandler<JobStateEventArgs> stateChanged, bool connectImmediately)
		{
			Collection<PSObject> collection;
			using (PowerShell powerShell = PowerShell.Create())
			{
				if (runspacePool == null)
				{
					powerShell.Runspace = runspace;
				}
				else
				{
					powerShell.RunspacePool = runspacePool;
				}
				powerShell.AddCommand("Get-Job");
				if (filter != null)
				{
					powerShell.AddParameter("Filter", filter);
				}
				collection = powerShell.Invoke();
			}
			Collection<PSJobProxy> collection2 = new Collection<PSJobProxy>();
			foreach (PSObject o in collection)
			{
				if (Deserializer.IsDeserializedInstanceOfType(o, typeof(Job)))
				{
					string empty = string.Empty;
					PSJobProxy.TryGetJobPropertyValue<string>(o, "Command", out empty);
					PSJobProxy psjobProxy = new PSJobProxy(empty);
					psjobProxy.InitializeExistingJobProxy(o, runspace, runspacePool);
					psjobProxy._receiveIsValidCall = true;
					if (connectImmediately)
					{
						psjobProxy.ReceiveJob(dataAdded, stateChanged);
					}
					collection2.Add(psjobProxy);
				}
			}
			return collection2;
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x000B1934 File Offset: 0x000AFB34
		public void ReceiveJob()
		{
			this.ReceiveJob(null, null);
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x000B1940 File Offset: 0x000AFB40
		public void ReceiveJob(EventHandler<JobDataAddedEventArgs> dataAdded, EventHandler<JobStateEventArgs> stateChanged)
		{
			lock (base.SyncRoot)
			{
				if (!this._receiveIsValidCall)
				{
					throw PSTraceSource.NewInvalidOperationException(PowerShellStrings.JobProxyReceiveInvalid, new object[0]);
				}
				this._receiveIsValidCall = false;
				this._dataAddedHandler = dataAdded;
				this._stateChangedHandler = stateChanged;
			}
			this.RegisterChildEvents();
			this.ValidateAndDoSetJobState(JobState.Running, null);
			foreach (Job job in base.ChildJobs)
			{
				PSChildJobProxy pschildJobProxy = (PSChildJobProxy)job;
				pschildJobProxy.DoSetJobState(JobState.Running, null);
			}
			this.AssignRunspaceOrRunspacePool(this._receivePowerShell);
			this.AddReceiveJobCommandToPowerShell(this._receivePowerShell, false);
			this._receivePowerShell.AddParameter("InstanceId", this._remoteJobInstanceId);
			this._receivePowerShell.BeginInvoke<PSObject, PSObject>(null, this._receivePowerShellOutput, null, new AsyncCallback(this.CleanupReceivePowerShell), null);
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x000B1A50 File Offset: 0x000AFC50
		internal void InitializeJobProxy(PSCommand command, Runspace runspace, RunspacePool runspacePool)
		{
			this._tracer.WriteMessage("PSJobProxy", "InitializeJobProxy", this._remoteJobInstanceId, this, "Initializing Job Proxy.", null);
			this._pscommand = command.Clone();
			CommandParameterCollection commandParameterCollection = new CommandParameterCollection();
			foreach (CommandParameter item in this._pscommand.Commands[0].Parameters)
			{
				commandParameterCollection.Add(item);
			}
			base.StartParameters = new List<CommandParameterCollection>
			{
				commandParameterCollection
			};
			this._pscommand.Commands[0].Parameters.Clear();
			this.CommonInit(runspace, runspacePool);
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x000B1B18 File Offset: 0x000AFD18
		internal void InitializeExistingJobProxy(PSObject o, Runspace runspace, RunspacePool runspacePool)
		{
			this._tracer.WriteMessage("PSJobProxy", "InitializeExistingJobProxy", this._remoteJobInstanceId, this, "Initializing job proxy for existing job.", null);
			this._pscommand = null;
			this._startCalled = true;
			this._jobInitialized = true;
			this.CommonInit(runspace, runspacePool);
			this.PopulateJobProperties(o);
			List<Hashtable> list = new List<Hashtable>();
			object obj = null;
			foreach (Job job in base.ChildJobs)
			{
				PSChildJobProxy pschildJobProxy = (PSChildJobProxy)job;
				if (pschildJobProxy.StartParameters.Count != 0)
				{
					Hashtable hashtable = new Hashtable();
					foreach (CommandParameter commandParameter in pschildJobProxy.StartParameters[0])
					{
						if (obj == null && commandParameter.Name.Equals("PSPrivateMetadata", StringComparison.OrdinalIgnoreCase))
						{
							obj = commandParameter.Value;
						}
						hashtable.Add(commandParameter.Name, commandParameter.Value);
					}
					list.Add(hashtable);
				}
			}
			CommandParameterCollection commandParameterCollection = new CommandParameterCollection();
			commandParameterCollection.Add(new CommandParameter("PSParameterCollection", list));
			if (obj != null)
			{
				commandParameterCollection.Add(new CommandParameter("PSPrivateMetadata", obj));
			}
			base.StartParameters.Add(commandParameterCollection);
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x000B1C88 File Offset: 0x000AFE88
		private void CommonInit(Runspace runspace, RunspacePool runspacePool)
		{
			this._runspacePool = runspacePool;
			this._runspace = runspace;
			this._receivePowerShell.InvocationStateChanged += this.ReceivePowerShellInvocationStateChanged;
			this._receivePowerShellOutput.DataAdded += this.DataAddedToOutput;
			this._receivePowerShell.Streams.Error.DataAdded += this.DataAddedToError;
			this._receivePowerShell.Streams.Debug.DataAdded += this.DataAddedToDebug;
			this._receivePowerShell.Streams.Verbose.DataAdded += this.DataAddedToVerbose;
			this._receivePowerShell.Streams.Warning.DataAdded += this.DataAddedToWarning;
			this._receivePowerShell.Streams.Progress.DataAdded += this.DataAddedToProgress;
			this._receivePowerShell.Streams.Information.DataAdded += this.DataAddedToInformation;
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x000B1D98 File Offset: 0x000AFF98
		internal static bool TryGetJobPropertyValue<T>(PSObject o, string propertyName, out T propertyValue)
		{
			propertyValue = default(T);
			PSPropertyInfo pspropertyInfo = o.Properties[propertyName];
			if (pspropertyInfo == null || !(pspropertyInfo.Value is T))
			{
				return false;
			}
			propertyValue = (T)((object)pspropertyInfo.Value);
			return true;
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x000B1DE0 File Offset: 0x000AFFE0
		private void JobActionWorker(AsyncOperation asyncOp, PSJobProxy.ActionType action)
		{
			Exception error = null;
			try
			{
				if (action == PSJobProxy.ActionType.Remove)
				{
					this.DoRemove(asyncOp.UserSuppliedState);
				}
			}
			catch (Exception ex)
			{
				error = ex;
			}
			AsyncCompletedEventArgs eventArgs = new AsyncCompletedEventArgs(error, false, asyncOp.UserSuppliedState);
			PSJobProxy.AsyncCompleteContainer arg = new PSJobProxy.AsyncCompleteContainer
			{
				EventArgs = eventArgs,
				Action = action
			};
			asyncOp.PostOperationCompleted(new SendOrPostCallback(this.JobActionAsyncCompleted), arg);
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x000B1E58 File Offset: 0x000B0058
		private void JobActionAsyncCompleted(object operationState)
		{
			PSJobProxy.AsyncCompleteContainer asyncCompleteContainer = operationState as PSJobProxy.AsyncCompleteContainer;
			this._tracer.WriteMessage("PSJobProxy", "JobActionAsyncCompleted", this._remoteJobInstanceId, this, "Async operation {0} completed", new string[]
			{
				asyncCompleteContainer.Action.ToString()
			});
			PSJobProxy.ActionType action = asyncCompleteContainer.Action;
			if (action != PSJobProxy.ActionType.Remove)
			{
				return;
			}
			if (asyncCompleteContainer.EventArgs.Error == null)
			{
				this.RemoveComplete.WaitOne();
			}
			this.OnRemoveJobCompleted(asyncCompleteContainer.EventArgs);
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x000B1EDC File Offset: 0x000B00DC
		private bool ShouldQueueOperation()
		{
			bool result;
			lock (this._inProgressSyncObject)
			{
				if (!this._inProgress)
				{
					this._inProgress = true;
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x000B1F2C File Offset: 0x000B012C
		private void ProcessQueue()
		{
			bool flag = false;
			lock (this._inProgressSyncObject)
			{
				if (!this._pendingOperations.IsEmpty && !this._workerCreated)
				{
					flag = true;
					this._workerCreated = true;
					this._inProgress = true;
				}
				else
				{
					this._inProgress = false;
				}
			}
			if (flag)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.ProcessQueueWorker));
			}
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x000B1FAC File Offset: 0x000B01AC
		private void ProcessQueueWorker(object state)
		{
			for (;;)
			{
				PSJobProxy.QueueOperation queueOperation;
				lock (this._inProgressSyncObject)
				{
					if (!this._pendingOperations.TryDequeue(out queueOperation))
					{
						this._inProgress = false;
						this._workerCreated = false;
						break;
					}
				}
				switch (queueOperation)
				{
				case PSJobProxy.QueueOperation.Stop:
					try
					{
						this.DoStopAsync();
						base.Finished.WaitOne();
						continue;
					}
					catch (Exception error)
					{
						this.OnStopJobCompleted(new AsyncCompletedEventArgs(error, false, null));
						continue;
					}
					break;
				case PSJobProxy.QueueOperation.Suspend:
					break;
				case PSJobProxy.QueueOperation.Resume:
					goto IL_AB;
				default:
					continue;
				}
				try
				{
					this.DoSuspendAsync();
					this.JobSuspendedOrFinished.WaitOne();
					continue;
				}
				catch (Exception error2)
				{
					this.OnSuspendJobCompleted(new AsyncCompletedEventArgs(error2, false, null));
					continue;
				}
				try
				{
					IL_AB:
					this.DoResumeAsync();
					this.JobRunningOrFinished.WaitOne();
					continue;
				}
				catch (Exception error3)
				{
					this.OnResumeJobCompleted(new AsyncCompletedEventArgs(error3, false, null));
					continue;
				}
				break;
			}
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x000B20C4 File Offset: 0x000B02C4
		private static bool CollectionHasMoreData<T>(PSDataCollection<T> collection)
		{
			return collection.IsOpen || collection.Count > 0;
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x000B20DC File Offset: 0x000B02DC
		private void DoStartAsync(EventHandler<JobDataAddedEventArgs> dataAdded, EventHandler<JobStateEventArgs> stateChanged, PSDataCollection<object> input)
		{
			this.AssertJobCanBeStartedAndSetStateToRunning();
			lock (this._inProgressSyncObject)
			{
				this._inProgress = true;
			}
			lock (base.SyncRoot)
			{
				this._dataAddedHandler = dataAdded;
				this._stateChangedHandler = stateChanged;
			}
			this._tracer.WriteMessage("PSJobProxy", "DoStartAsync", this._remoteJobInstanceId, this, "Starting command invocation.", null);
			PSJobProxy.StructuredTracer.BeginProxyJobExecution(base.InstanceId);
			this.DoStartPrepare();
			this._receivePowerShell.BeginInvoke<object, PSObject>(input, this._receivePowerShellOutput, null, new AsyncCallback(this.CleanupReceivePowerShell), null);
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x000B21B4 File Offset: 0x000B03B4
		private void DoStartPrepare()
		{
			if (this._runspacePool == null && this._runspace == null)
			{
				throw PSTraceSource.NewInvalidOperationException(PowerShellStrings.RunspaceAndRunspacePoolNull, new object[0]);
			}
			this.AssignRunspaceOrRunspacePool(this._receivePowerShell);
			bool flag = false;
			if (base.StartParameters != null && base.StartParameters.Count > 0)
			{
				foreach (CommandParameter commandParameter in base.StartParameters[0])
				{
					this._pscommand.Commands[0].Parameters.Add(commandParameter);
					if (string.Compare(commandParameter.Name, "AsJob", StringComparison.OrdinalIgnoreCase) == 0)
					{
						if (!(commandParameter.Value is bool) || !(bool)commandParameter.Value)
						{
							throw PSTraceSource.NewInvalidOperationException(PowerShellStrings.JobProxyAsJobMustBeTrue, new object[0]);
						}
						flag = true;
					}
				}
			}
			if (!flag)
			{
				this._pscommand.Commands[0].Parameters.Add("AsJob", true);
			}
			this._receivePowerShell.Commands = this._pscommand;
			this.AddReceiveJobCommandToPowerShell(this._receivePowerShell, true);
		}

		// Token: 0x06001EDA RID: 7898 RVA: 0x000B22F0 File Offset: 0x000B04F0
		private void DoStopAsync()
		{
			if (!this.AssertStopJobIsValidAndSetToStopping())
			{
				this.OnStopJobCompleted(new AsyncCompletedEventArgs(null, false, null));
				return;
			}
			this._receivePowerShell.Stop();
			this._receivePowerShell.Commands.Clear();
			this._receivePowerShell.GenerateNewInstanceId();
			this._receivePowerShell.AddCommand("Stop-Job").AddParameter("InstanceId", this._remoteJobInstanceId).AddParameter("PassThru");
			this.AddReceiveJobCommandToPowerShell(this._receivePowerShell, false);
			this._receivePowerShell.BeginInvoke<PSObject, PSObject>(null, this._receivePowerShellOutput, null, new AsyncCallback(this.CleanupReceivePowerShell), null);
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x000B2398 File Offset: 0x000B0598
		private void DoSuspendAsync()
		{
			if (!this.AssertSuspendJobIsValidAndSetToSuspending())
			{
				this.OnSuspendJobCompleted(new AsyncCompletedEventArgs(null, false, null));
				return;
			}
			this._receivePowerShell.Stop();
			this._receivePowerShell.Commands.Clear();
			this._receivePowerShell.GenerateNewInstanceId();
			this._receivePowerShell.AddCommand("Suspend-Job").AddParameter("InstanceId", this._remoteJobInstanceId).AddParameter("Wait");
			this.AddReceiveJobCommandToPowerShell(this._receivePowerShell, false);
			this._receivePowerShell.BeginInvoke<PSObject, PSObject>(null, this._receivePowerShellOutput, null, new AsyncCallback(this.CleanupReceivePowerShell), null);
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x000B2440 File Offset: 0x000B0640
		private void DoResumeAsync()
		{
			this.AssertResumeJobIsValidAndSetToRunning();
			if (base.JobStateInfo.State != JobState.Running)
			{
				this.OnResumeJobCompleted(new AsyncCompletedEventArgs(null, false, null));
				return;
			}
			this._receivePowerShell.Stop();
			this._receivePowerShell.Commands.Clear();
			this._receivePowerShell.GenerateNewInstanceId();
			this._receivePowerShell.AddCommand("Resume-Job").AddParameter("InstanceId", this._remoteJobInstanceId).AddParameter("Wait");
			this.AddReceiveJobCommandToPowerShell(this._receivePowerShell, false);
			this._receivePowerShell.BeginInvoke<PSObject, PSObject>(null, this._receivePowerShellOutput, null, new AsyncCallback(this.CleanupReceivePowerShell), null);
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x000B24F4 File Offset: 0x000B06F4
		private void DoRemove(object state)
		{
			this.AssertNotDisposed();
			this._tracer.WriteMessage("PSJobProxy", "DoRemove", this._remoteJobInstanceId, this, "Start", null);
			if (this._isDisposed || this._remoteJobRemoved)
			{
				return;
			}
			lock (base.SyncRoot)
			{
				if (this._isDisposed || this._remoteJobRemoved || this._removeCalled)
				{
					return;
				}
				if (this._remoteJobInstanceId == Guid.Empty && !this._startCalled)
				{
					return;
				}
				this.AssertRemoveJobIsValid();
				this._removeCalled = true;
				this.RemoveComplete.Reset();
			}
			try
			{
				this._jobInitialiedWaitHandle.WaitOne();
				if (!(this._remoteJobInstanceId == Guid.Empty))
				{
					this._receivePowerShell.Stop();
					using (PowerShell powerShell = PowerShell.Create())
					{
						this.AssignRunspaceOrRunspacePool(powerShell);
						powerShell.Commands.AddCommand("Remove-Job").AddParameter("InstanceId", this._remoteJobInstanceId);
						if ((bool)state)
						{
							powerShell.AddParameter("Force", true).AddParameter("ErrorAction", ActionPreference.SilentlyContinue);
						}
						try
						{
							this._tracer.WriteMessage("PSJobProxy", "DoRemove", this._remoteJobInstanceId, this, "Invoking Remove-Job", null);
							powerShell.Invoke();
						}
						catch (Exception ex)
						{
							CommandProcessorBase.CheckForSevereException(ex);
							this._tracer.WriteMessage("PSJobProxy", "DoRemove", this._remoteJobInstanceId, this, "Setting job state to failed since invoking Remove-Job failed.", null);
							this.DoSetJobState(JobState.Failed, ex);
							throw;
						}
						if (powerShell.Streams.Error != null && powerShell.Streams.Error.Count > 0)
						{
							throw powerShell.Streams.Error[0].Exception;
						}
					}
					this._tracer.WriteMessage("PSJobProxy", "DoRemove", this._remoteJobInstanceId, this, "Completed Invoking Remove-Job", null);
					lock (base.SyncRoot)
					{
						this._remoteJobRemoved = true;
					}
					if (!base.IsFinishedState(base.JobStateInfo.State))
					{
						this.DoSetJobState(JobState.Stopped, null);
					}
				}
			}
			catch (Exception)
			{
				lock (base.SyncRoot)
				{
					this._removeCalled = false;
				}
				throw;
			}
			finally
			{
				this.RemoveComplete.Set();
			}
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x000B2828 File Offset: 0x000B0A28
		private void AddReceiveJobCommandToPowerShell(PowerShell powershell, bool writeJob)
		{
			powershell.AddCommand("Receive-Job").AddParameter("Wait").AddParameter("WriteEvents").AddParameter("Verbose").AddParameter("Debug");
			if (writeJob)
			{
				powershell.AddParameter("WriteJobInResults");
			}
			if (this.RemoveRemoteJobOnCompletion)
			{
				powershell.AddParameter("AutoRemoveJob");
			}
		}

		// Token: 0x06001EDF RID: 7903 RVA: 0x000B288C File Offset: 0x000B0A8C
		private void CleanupReceivePowerShell(IAsyncResult asyncResult)
		{
			try
			{
				this._receivePowerShell.EndInvoke(asyncResult);
				this._tracer.WriteMessage("PSJobProxy", "CleanupReceivePowerShell", Guid.Empty, this, "Setting job state to {0} from computed stated", new string[]
				{
					this._computedJobState.ToString()
				});
				this.ValidateAndDoSetJobState(this._computedJobState, null);
			}
			catch (PipelineStoppedException exception)
			{
				this._tracer.TraceException(exception);
			}
			catch (PSRemotingDataStructureException exception2)
			{
				this._tracer.TraceException(exception2);
			}
			catch (RemoteException ex)
			{
				if (Deserializer.IsInstanceOfType(ex.SerializedRemoteException, typeof(PipelineStoppedException)))
				{
					this._tracer.TraceException(ex);
				}
				else
				{
					this._tracer.TraceException(ex);
					this.DoSetJobState(JobState.Failed, ex);
				}
			}
			catch (Exception ex2)
			{
				this._tracer.WriteMessage("PSJobProxy", "CleanupReceivePowerShell", this._remoteJobInstanceId, this, "Exception calling receivePowerShell.EndInvoke", null);
				this._tracer.TraceException(ex2);
				this.DoSetJobState(JobState.Failed, ex2);
			}
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x000B29C8 File Offset: 0x000B0BC8
		private void AssignRunspaceOrRunspacePool(PowerShell powershell)
		{
			if (this._runspacePool == null)
			{
				powershell.Runspace = this._runspace;
				return;
			}
			powershell.RunspacePool = this._runspacePool;
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x000B29EC File Offset: 0x000B0BEC
		private void HandleMyStateChange(object sender, JobStateEventArgs e)
		{
			switch (e.JobStateInfo.State)
			{
			case JobState.Running:
				lock (base.SyncRoot)
				{
					if (e.PreviousJobStateInfo.State == JobState.NotStarted)
					{
						base.PSBeginTime = new DateTime?(DateTime.Now);
					}
					this.JobRunningOrFinished.Set();
					this.JobSuspendedOrFinished.Reset();
					this.OnResumeJobCompleted(new AsyncCompletedEventArgs(null, false, null));
					goto IL_1A1;
				}
				break;
			case JobState.Completed:
			case JobState.Failed:
			case JobState.Stopped:
				goto IL_EE;
			case JobState.Blocked:
				goto IL_1A1;
			case JobState.Suspended:
				break;
			default:
				goto IL_1A1;
			}
			lock (base.SyncRoot)
			{
				base.PSEndTime = new DateTime?(DateTime.Now);
				this.JobSuspendedOrFinished.Set();
				this.JobRunningOrFinished.Reset();
				this.OnSuspendJobCompleted(new AsyncCompletedEventArgs(null, false, null));
				goto IL_1A1;
			}
			IL_EE:
			lock (base.SyncRoot)
			{
				base.PSEndTime = new DateTime?(DateTime.Now);
				this.JobRunningOrFinished.Set();
				this.OnResumeJobCompleted(new AsyncCompletedEventArgs(e.JobStateInfo.Reason, false, null));
				this.JobSuspendedOrFinished.Set();
				this.OnSuspendJobCompleted(new AsyncCompletedEventArgs(e.JobStateInfo.Reason, false, null));
				this._jobInitialiedWaitHandle.Set();
				this.OnStartJobCompleted(new AsyncCompletedEventArgs(e.JobStateInfo.Reason, false, null));
				this.OnStopJobCompleted(new AsyncCompletedEventArgs(e.JobStateInfo.Reason, false, null));
			}
			IL_1A1:
			this.ProcessQueue();
		}

		// Token: 0x06001EE2 RID: 7906 RVA: 0x000B2BC8 File Offset: 0x000B0DC8
		private void ReceivePowerShellInvocationStateChanged(object sender, PSInvocationStateChangedEventArgs e)
		{
			this._tracer.WriteMessage("PSJobProxy", "ReceivePowerShellInvocationStateChanged", this._remoteJobInstanceId, this, "receivePowerShell state changed to {0}", new string[]
			{
				e.InvocationStateInfo.State.ToString()
			});
			switch (e.InvocationStateInfo.State)
			{
			default:
				return;
			case PSInvocationState.Stopping:
			case PSInvocationState.Stopped:
			case PSInvocationState.Completed:
				break;
			case PSInvocationState.Failed:
			{
				JobState jobState = JobState.Failed;
				string text = (e.InvocationStateInfo.Reason == null) ? string.Empty : e.InvocationStateInfo.Reason.ToString();
				this._tracer.WriteMessage("PSJobProxy", "ReceivePowerShellInvocationStateChanged", this._remoteJobInstanceId, this, "Setting job state to {0} old state was {1} and reason is {2}.", new string[]
				{
					jobState.ToString(),
					base.JobStateInfo.State.ToString(),
					text
				});
				this.DoSetJobState(jobState, e.InvocationStateInfo.Reason);
				break;
			}
			}
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x000B2CE4 File Offset: 0x000B0EE4
		private void PopulateJobProperties(PSObject o)
		{
			PSJobProxy.TryGetJobPropertyValue<Guid>(o, "InstanceId", out this._remoteJobInstanceId);
			PSJobProxy.TryGetJobPropertyValue<string>(o, "StatusMessage", out this._remoteJobStatusMessage);
			PSJobProxy.TryGetJobPropertyValue<string>(o, "Location", out this._remoteJobLocation);
			PSJobProxy.StructuredTracer.ProxyJobRemoteJobAssociation(base.InstanceId, this._remoteJobInstanceId);
			string name;
			PSJobProxy.TryGetJobPropertyValue<string>(o, "Name", out name);
			base.Name = name;
			PSObject psobject;
			if (!PSJobProxy.TryGetJobPropertyValue<PSObject>(o, "ChildJobs", out psobject))
			{
				return;
			}
			ArrayList source = psobject.BaseObject as ArrayList;
			foreach (PSObject o2 in from PSObject job in source
			where !(job.BaseObject is string)
			select job)
			{
				Guid guid;
				if (PSJobProxy.TryGetJobPropertyValue<Guid>(o2, "InstanceId", out guid))
				{
					PSChildJobProxy pschildJobProxy = new PSChildJobProxy(base.Command, o2);
					this._childJobsMapping.Add(guid, pschildJobProxy);
					pschildJobProxy.Output.DataAddedCount = base.Output.DataAddedCount;
					pschildJobProxy.Error.DataAddedCount = base.Error.DataAddedCount;
					pschildJobProxy.Progress.DataAddedCount = base.Progress.DataAddedCount;
					pschildJobProxy.Warning.DataAddedCount = base.Warning.DataAddedCount;
					pschildJobProxy.Verbose.DataAddedCount = base.Verbose.DataAddedCount;
					pschildJobProxy.Debug.DataAddedCount = base.Debug.DataAddedCount;
					pschildJobProxy.Information.DataAddedCount = base.Information.DataAddedCount;
					PSObject childJobStartParametersObject;
					if (PSJobProxy.TryGetJobPropertyValue<PSObject>(o2, "StartParameters", out childJobStartParametersObject))
					{
						this.PopulateStartParametersOnChild(childJobStartParametersObject, pschildJobProxy);
					}
					base.ChildJobs.Add(pschildJobProxy);
				}
			}
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x000B2F04 File Offset: 0x000B1104
		private void PopulateStartParametersOnChild(PSObject childJobStartParametersObject, PSChildJobProxy childProxyJob)
		{
			ArrayList arrayList = childJobStartParametersObject.BaseObject as ArrayList;
			if (arrayList != null)
			{
				List<CommandParameterCollection> list = new List<CommandParameterCollection>();
				foreach (PSObject psobject in from PSObject paramCollection in arrayList
				where !(paramCollection.BaseObject is string)
				select paramCollection)
				{
					ArrayList arrayList2 = psobject.BaseObject as ArrayList;
					if (arrayList2 != null)
					{
						CommandParameterCollection commandParameterCollection = new CommandParameterCollection();
						foreach (PSObject o in arrayList2.Cast<PSObject>().Where((PSObject deserializedCommandParameter) => !(deserializedCommandParameter.BaseObject is string)))
						{
							string name;
							object value;
							if (PSJobProxy.TryGetJobPropertyValue<string>(o, "Name", out name) && PSJobProxy.TryGetJobPropertyValue<object>(o, "Value", out value))
							{
								CommandParameter item = new CommandParameter(name, value);
								commandParameterCollection.Add(item);
							}
						}
						list.Add(commandParameterCollection);
					}
				}
				childProxyJob.StartParameters = list;
			}
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x000B3048 File Offset: 0x000B1248
		private void RegisterChildEvents()
		{
			if (this._childEventsRegistered)
			{
				return;
			}
			lock (base.SyncRoot)
			{
				if (!this._childEventsRegistered)
				{
					this._childEventsRegistered = true;
					if (this._dataAddedHandler != null)
					{
						foreach (Job job in base.ChildJobs)
						{
							PSChildJobProxy pschildJobProxy = (PSChildJobProxy)job;
							pschildJobProxy.JobDataAdded += this._dataAddedHandler;
						}
					}
					foreach (Job job2 in base.ChildJobs)
					{
						job2.StateChanged += this.HandleChildProxyJobStateChanged;
					}
					if (this._stateChangedHandler != null)
					{
						foreach (Job job3 in base.ChildJobs)
						{
							PSChildJobProxy pschildJobProxy2 = (PSChildJobProxy)job3;
							pschildJobProxy2.StateChanged += this._stateChangedHandler;
						}
					}
				}
			}
		}

		// Token: 0x06001EE6 RID: 7910 RVA: 0x000B3194 File Offset: 0x000B1394
		private void UnregisterChildEvents()
		{
			lock (base.SyncRoot)
			{
				if (this._childEventsRegistered)
				{
					if (this._dataAddedHandler != null)
					{
						foreach (Job job in base.ChildJobs)
						{
							PSChildJobProxy pschildJobProxy = (PSChildJobProxy)job;
							pschildJobProxy.JobDataAdded -= this._dataAddedHandler;
						}
					}
					if (this._stateChangedHandler != null)
					{
						foreach (Job job2 in base.ChildJobs)
						{
							PSChildJobProxy pschildJobProxy2 = (PSChildJobProxy)job2;
							pschildJobProxy2.StateChanged -= this._stateChangedHandler;
						}
					}
					foreach (Job job3 in base.ChildJobs)
					{
						job3.StateChanged -= this.HandleChildProxyJobStateChanged;
					}
					this._childEventsRegistered = false;
				}
			}
		}

		// Token: 0x06001EE7 RID: 7911 RVA: 0x000B32D8 File Offset: 0x000B14D8
		private void HandleChildProxyJobStateChanged(object sender, JobStateEventArgs e)
		{
			JobState jobState;
			if (!ContainerParentJob.ComputeJobStateFromChildJobStates("PSJobProxy", e, ref this._blockedChildJobsCount, ref this._suspendedChildJobsCount, ref this._suspendingChildJobsCount, ref this._finishedChildJobsCount, ref this._failedChildJobsCount, ref this._stoppedChildJobsCount, base.ChildJobs.Count, out jobState))
			{
				return;
			}
			if (jobState == JobState.Suspending)
			{
				return;
			}
			this._tracer.WriteMessage("PSJobProxy", "HandleChildProxyJobStateChanged", Guid.Empty, this, "storing job state to {0}", new string[]
			{
				jobState.ToString()
			});
			this._computedJobState = jobState;
		}

		// Token: 0x06001EE8 RID: 7912 RVA: 0x000B3368 File Offset: 0x000B1568
		private void AssertChangesCanBeAccepted()
		{
			lock (base.SyncRoot)
			{
				this.AssertNotDisposed();
				if (base.JobStateInfo.State != JobState.NotStarted)
				{
					throw new InvalidJobStateException(base.JobStateInfo.State);
				}
			}
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x000B33C8 File Offset: 0x000B15C8
		private void AssertJobCanBeStartedAndSetStateToRunning()
		{
			lock (base.SyncRoot)
			{
				this.AssertNotDisposed();
				if (base.JobStateInfo.State != JobState.NotStarted && !base.IsFinishedState(base.JobStateInfo.State))
				{
					throw new InvalidJobStateException(base.JobStateInfo.State, StringUtil.Format(PowerShellStrings.JobCannotBeStartedWhenRunning, new object[0]));
				}
				if (this._startCalled)
				{
					throw PSTraceSource.NewNotSupportedException(PowerShellStrings.JobCanBeStartedOnce, new object[0]);
				}
				this._startCalled = true;
			}
			this._tracer.WriteMessage("PSJobProxy", "AssertJobCanBeStartedAndSetStateToRunning", this._remoteJobInstanceId, this, "Setting job state to running", null);
			this.ValidateAndDoSetJobState(JobState.Running, null);
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x000B3494 File Offset: 0x000B1694
		private bool AssertStopJobIsValidAndSetToStopping()
		{
			lock (base.SyncRoot)
			{
				this.AssertNotDisposed();
				if (base.JobStateInfo.State == JobState.NotStarted)
				{
					throw new InvalidJobStateException(JobState.NotStarted);
				}
				if (base.IsFinishedState(base.JobStateInfo.State))
				{
					return false;
				}
			}
			this.DoSetJobState(JobState.Stopping, null);
			return true;
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x000B350C File Offset: 0x000B170C
		private bool AssertSuspendJobIsValidAndSetToSuspending()
		{
			lock (base.SyncRoot)
			{
				this.AssertNotDisposed();
				if (base.JobStateInfo.State != JobState.Suspended && base.JobStateInfo.State != JobState.Suspending && base.JobStateInfo.State != JobState.Running)
				{
					throw new InvalidJobStateException(base.JobStateInfo.State);
				}
			}
			if (base.JobStateInfo.State != JobState.Running)
			{
				return false;
			}
			this.DoSetJobState(JobState.Suspending, null);
			return true;
		}

		// Token: 0x06001EEC RID: 7916 RVA: 0x000B35A0 File Offset: 0x000B17A0
		private void AssertResumeJobIsValidAndSetToRunning()
		{
			lock (base.SyncRoot)
			{
				this.AssertNotDisposed();
				if (base.JobStateInfo.State != JobState.Suspended && base.JobStateInfo.State != JobState.Suspending && base.JobStateInfo.State != JobState.Running)
				{
					throw new InvalidJobStateException(base.JobStateInfo.State);
				}
			}
			if (base.JobStateInfo.State != JobState.Running)
			{
				this.ValidateAndDoSetJobState(JobState.Running, null);
			}
			foreach (Job job in base.ChildJobs)
			{
				PSChildJobProxy pschildJobProxy = (PSChildJobProxy)job;
				if (!base.IsFinishedState(pschildJobProxy.JobStateInfo.State))
				{
					pschildJobProxy.DoSetJobState(JobState.Running, null);
				}
			}
		}

		// Token: 0x06001EED RID: 7917 RVA: 0x000B3688 File Offset: 0x000B1888
		private void AssertRemoveJobIsValid()
		{
			if (base.JobStateInfo.State == JobState.NotStarted || this._remoteJobInstanceId == Guid.Empty)
			{
				throw new InvalidJobStateException(base.JobStateInfo.State);
			}
		}

		// Token: 0x06001EEE RID: 7918 RVA: 0x000B36BA File Offset: 0x000B18BA
		private new void AssertNotDisposed()
		{
			if (this._isDisposed)
			{
				throw PSTraceSource.NewObjectDisposedException("PSJobProxy");
			}
		}

		// Token: 0x06001EEF RID: 7919 RVA: 0x000B36CF File Offset: 0x000B18CF
		private void ValidateAndDoSetJobState(JobState state, Exception reason = null)
		{
			if ((this._previousState == JobState.Stopping || this._previousState == JobState.Suspending) && state == JobState.Running)
			{
				return;
			}
			this.DoSetJobState(state, reason);
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x000B36F4 File Offset: 0x000B18F4
		private void DoSetJobState(JobState state, Exception reason = null)
		{
			if (this._isDisposed)
			{
				return;
			}
			lock (base.SyncRoot)
			{
				if (this._previousState == state)
				{
					return;
				}
				this._previousState = state;
			}
			try
			{
				this._tracer.WriteMessage("PSJobProxy", "DoSetJobState", this._remoteJobInstanceId, this, "BEGIN Set job state to {0} and call event handlers", new string[]
				{
					state.ToString()
				});
				PSJobProxy.StructuredTracer.EndProxyJobExecution(base.InstanceId);
				PSJobProxy.StructuredTracer.BeginProxyJobEventHandler(base.InstanceId);
				base.SetJobState(state, reason);
				PSJobProxy.StructuredTracer.EndProxyJobEventHandler(base.InstanceId);
				this._tracer.WriteMessage("PSJobProxy", "DoSetJobState", this._remoteJobInstanceId, this, "END Set job state to {0} and call event handlers", new string[]
				{
					state.ToString()
				});
			}
			catch (ObjectDisposedException)
			{
				this._tracer.WriteMessage("PSJobProxy", "DoSetJobState", this._remoteJobInstanceId, this, "Caught object disposed exception", null);
			}
		}

		// Token: 0x06001EF1 RID: 7921 RVA: 0x000B3828 File Offset: 0x000B1A28
		private T GetRecord<T>(object sender)
		{
			T result;
			lock (base.SyncRoot)
			{
				PSDataCollection<T> psdataCollection = sender as PSDataCollection<T>;
				T t = psdataCollection.ReadAndRemoveAt0();
				result = t;
			}
			return result;
		}

		// Token: 0x06001EF2 RID: 7922 RVA: 0x000B3878 File Offset: 0x000B1A78
		private void DataAddedToOutput(object sender, DataAddedEventArgs e)
		{
			PSObject record = this.GetRecord<PSObject>(sender);
			if (!this._jobInitialized)
			{
				this._jobInitialized = true;
				if (!Deserializer.IsDeserializedInstanceOfType(record, typeof(Job)))
				{
					this._tracer.WriteMessage("PSJobProxy", "DataAddedToOutput", this._remoteJobInstanceId, this, "Setting job state to failed. Command did not return a job object.", null);
					Exception ex = (this._receivePowerShell.Streams.Error.Count == 0 || this._receivePowerShell.Streams.Error[0].Exception == null) ? PSTraceSource.NewNotSupportedException(PowerShellStrings.CommandDoesNotWriteJob, new object[0]) : this._receivePowerShell.Streams.Error[0].Exception;
					this.DoSetJobState(JobState.Failed, ex);
					this._jobInitialiedWaitHandle.Set();
					this.OnStartJobCompleted(new AsyncCompletedEventArgs(ex, false, null));
					return;
				}
				this.PopulateJobProperties(record);
				this.RegisterChildEvents();
				foreach (Job job in base.ChildJobs)
				{
					PSChildJobProxy pschildJobProxy = (PSChildJobProxy)job;
					pschildJobProxy.DoSetJobState(JobState.Running, null);
				}
				this._jobInitialiedWaitHandle.Set();
				this._tracer.WriteMessage("PSJobProxy", "DataAddedToOutput", Guid.Empty, this, "BEGIN Invoke StartJobCompleted event", null);
				this.OnStartJobCompleted(new AsyncCompletedEventArgs(null, false, null));
				this._tracer.WriteMessage("PSJobProxy", "DataAddedToOutput", Guid.Empty, this, "END Invoke StartJobCompleted event", null);
				this.ProcessQueue();
				return;
			}
			else
			{
				if (record.Properties[RemotingConstants.EventObject] == null)
				{
					this.SortOutputObject(record);
					return;
				}
				PSPropertyInfo pspropertyInfo = record.Properties[RemotingConstants.SourceJobInstanceId];
				Guid guid = (pspropertyInfo != null) ? ((Guid)pspropertyInfo.Value) : Guid.Empty;
				if (pspropertyInfo == null || guid == Guid.Empty)
				{
					return;
				}
				if (!this._childJobsMapping.ContainsKey(guid))
				{
					guid != this._remoteJobInstanceId;
					return;
				}
				JobStateEventArgs jobStateEventArgs = (record.BaseObject as JobStateEventArgs) ?? DeserializingTypeConverter.RehydrateJobStateEventArgs(record);
				if (jobStateEventArgs != null)
				{
					this._tracer.WriteMessage("PSJobProxy", "DataAddedToOutput", Guid.Empty, this, "Updating child job {0} state to {1} ", new string[]
					{
						guid.ToString(),
						jobStateEventArgs.JobStateInfo.State.ToString()
					});
					((PSChildJobProxy)this._childJobsMapping[guid]).DoSetJobState(jobStateEventArgs.JobStateInfo.State, jobStateEventArgs.JobStateInfo.Reason);
					this._tracer.WriteMessage("PSJobProxy", "DataAddedToOutput", Guid.Empty, this, "Finished updating child job {0} state to {1} ", new string[]
					{
						guid.ToString(),
						jobStateEventArgs.JobStateInfo.State.ToString()
					});
				}
				return;
			}
		}

		// Token: 0x06001EF3 RID: 7923 RVA: 0x000B3B8C File Offset: 0x000B1D8C
		private void SortOutputObject(PSObject newObject)
		{
			PSPropertyInfo pspropertyInfo = newObject.Properties[RemotingConstants.SourceJobInstanceId];
			Guid guid = (pspropertyInfo != null) ? ((Guid)pspropertyInfo.Value) : Guid.Empty;
			if (pspropertyInfo == null || guid == Guid.Empty || !this._childJobsMapping.ContainsKey(guid))
			{
				base.Output.Add(newObject);
				return;
			}
			newObject.Properties.Remove(RemotingConstants.SourceJobInstanceId);
			newObject.Properties.Add(new PSNoteProperty(RemotingConstants.SourceJobInstanceId, ((PSChildJobProxy)this._childJobsMapping[guid]).InstanceId));
			((PSChildJobProxy)this._childJobsMapping[guid]).Output.Add(newObject);
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x000B3C58 File Offset: 0x000B1E58
		private void DataAddedToError(object sender, DataAddedEventArgs e)
		{
			ErrorRecord record = this.GetRecord<ErrorRecord>(sender);
			this.SortError(record);
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x000B3C74 File Offset: 0x000B1E74
		private void SortError(ErrorRecord record)
		{
			Guid empty = Guid.Empty;
			string empty2 = string.Empty;
			if (record.ErrorDetails != null)
			{
				record.ErrorDetails.RecommendedAction = PSJobProxy.RemoveIdentifierInformation(record.ErrorDetails.RecommendedAction, out empty, out empty2);
			}
			if (empty == Guid.Empty || !this._childJobsMapping.ContainsKey(empty))
			{
				this.WriteError(record);
				return;
			}
			OriginInfo originInfo = new OriginInfo(null, Guid.Empty, ((PSChildJobProxy)this._childJobsMapping[empty]).InstanceId);
			((PSChildJobProxy)this._childJobsMapping[empty]).WriteError(new RemotingErrorRecord(record, originInfo));
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x000B3D28 File Offset: 0x000B1F28
		private void DataAddedToProgress(object sender, DataAddedEventArgs e)
		{
			ProgressRecord record = this.GetRecord<ProgressRecord>(sender);
			this.SortProgress(record);
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x000B3D44 File Offset: 0x000B1F44
		private void SortProgress(ProgressRecord newRecord)
		{
			Guid guid;
			string computerName;
			newRecord.CurrentOperation = PSJobProxy.RemoveIdentifierInformation(newRecord.CurrentOperation, out guid, out computerName);
			if (guid == Guid.Empty || !this._childJobsMapping.ContainsKey(guid))
			{
				this.WriteProgress(newRecord);
				return;
			}
			OriginInfo originInfo = new OriginInfo(computerName, Guid.Empty, ((PSChildJobProxy)this._childJobsMapping[guid]).InstanceId);
			((PSChildJobProxy)this._childJobsMapping[guid]).WriteProgress(new RemotingProgressRecord(newRecord, originInfo));
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x000B3DD8 File Offset: 0x000B1FD8
		private void DataAddedToDebug(object sender, DataAddedEventArgs e)
		{
			DebugRecord record = this.GetRecord<DebugRecord>(sender);
			this.SortDebug(record);
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x000B3DF4 File Offset: 0x000B1FF4
		private void SortDebug(DebugRecord record)
		{
			Guid guid;
			string computerName;
			string message = PSJobProxy.RemoveIdentifierInformation(record.Message, out guid, out computerName);
			if (guid == Guid.Empty || !this._childJobsMapping.ContainsKey(guid))
			{
				this.WriteDebug(message);
				return;
			}
			OriginInfo originInfo = new OriginInfo(computerName, Guid.Empty, ((PSChildJobProxy)this._childJobsMapping[guid]).InstanceId);
			((PSChildJobProxy)this._childJobsMapping[guid]).Debug.Add(new RemotingDebugRecord(message, originInfo));
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x000B3E88 File Offset: 0x000B2088
		private void DataAddedToWarning(object sender, DataAddedEventArgs e)
		{
			WarningRecord record = this.GetRecord<WarningRecord>(sender);
			this.SortWarning(record);
		}

		// Token: 0x06001EFB RID: 7931 RVA: 0x000B3EA4 File Offset: 0x000B20A4
		private void SortWarning(WarningRecord record)
		{
			Guid guid;
			string computerName;
			string message = PSJobProxy.RemoveIdentifierInformation(record.Message, out guid, out computerName);
			if (guid == Guid.Empty || !this._childJobsMapping.ContainsKey(guid))
			{
				this.WriteWarning(message);
				return;
			}
			OriginInfo originInfo = new OriginInfo(computerName, Guid.Empty, ((PSChildJobProxy)this._childJobsMapping[guid]).InstanceId);
			((PSChildJobProxy)this._childJobsMapping[guid]).Warning.Add(new RemotingWarningRecord(message, originInfo));
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x000B3F38 File Offset: 0x000B2138
		private void DataAddedToVerbose(object sender, DataAddedEventArgs e)
		{
			VerboseRecord record = this.GetRecord<VerboseRecord>(sender);
			this.SortVerbose(record);
		}

		// Token: 0x06001EFD RID: 7933 RVA: 0x000B3F54 File Offset: 0x000B2154
		private void SortVerbose(VerboseRecord record)
		{
			Guid guid;
			string computerName;
			string message = PSJobProxy.RemoveIdentifierInformation(record.Message, out guid, out computerName);
			if (guid == Guid.Empty || !this._childJobsMapping.ContainsKey(guid))
			{
				this.WriteVerbose(message);
				return;
			}
			OriginInfo originInfo = new OriginInfo(computerName, Guid.Empty, ((PSChildJobProxy)this._childJobsMapping[guid]).InstanceId);
			((PSChildJobProxy)this._childJobsMapping[guid]).Verbose.Add(new RemotingVerboseRecord(message, originInfo));
		}

		// Token: 0x06001EFE RID: 7934 RVA: 0x000B3FE8 File Offset: 0x000B21E8
		private void DataAddedToInformation(object sender, DataAddedEventArgs e)
		{
			InformationRecord record = this.GetRecord<InformationRecord>(sender);
			this.SortInformation(record);
		}

		// Token: 0x06001EFF RID: 7935 RVA: 0x000B4004 File Offset: 0x000B2204
		private void SortInformation(InformationRecord record)
		{
			Guid guid;
			string computerName;
			record.Source = PSJobProxy.RemoveIdentifierInformation(record.Source, out guid, out computerName);
			if (guid == Guid.Empty || !this._childJobsMapping.ContainsKey(guid))
			{
				this.WriteInformation(record);
				return;
			}
			OriginInfo originInfo = new OriginInfo(computerName, Guid.Empty, ((PSChildJobProxy)this._childJobsMapping[guid]).InstanceId);
			((PSChildJobProxy)this._childJobsMapping[guid]).Information.Add(new RemotingInformationRecord(record, originInfo));
		}

		// Token: 0x06001F00 RID: 7936 RVA: 0x000B409C File Offset: 0x000B229C
		private static string RemoveIdentifierInformation(string message, out Guid jobInstanceId, out string computerName)
		{
			jobInstanceId = Guid.Empty;
			computerName = string.Empty;
			if (!string.IsNullOrEmpty(message))
			{
				string[] array = message.Split(new char[]
				{
					':'
				}, 3);
				if (array.Length == 3)
				{
					if (!Guid.TryParse(array[0], out jobInstanceId))
					{
						jobInstanceId = Guid.Empty;
					}
					computerName = array[1];
					return array[2];
				}
			}
			return message;
		}

		// Token: 0x06001F01 RID: 7937 RVA: 0x000B4100 File Offset: 0x000B2300
		protected override void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			if (this._isDisposed)
			{
				return;
			}
			lock (base.SyncRoot)
			{
				if (this._isDisposed)
				{
					return;
				}
				this._isDisposed = true;
			}
			if (this._receivePowerShell != null)
			{
				this._receivePowerShell.Stop();
				this._receivePowerShell.InvocationStateChanged -= this.ReceivePowerShellInvocationStateChanged;
				this._receivePowerShell.Streams.Error.DataAdded -= this.DataAddedToError;
				this._receivePowerShell.Streams.Warning.DataAdded -= this.DataAddedToWarning;
				this._receivePowerShell.Streams.Verbose.DataAdded -= this.DataAddedToVerbose;
				this._receivePowerShell.Streams.Progress.DataAdded -= this.DataAddedToProgress;
				this._receivePowerShell.Streams.Debug.DataAdded -= this.DataAddedToDebug;
				this._receivePowerShell.Streams.Information.DataAdded -= this.DataAddedToInformation;
				this._receivePowerShell.Dispose();
			}
			this.UnregisterChildEvents();
			base.StateChanged -= this.HandleMyStateChange;
			this._receivePowerShellOutput.DataAdded -= this.DataAddedToOutput;
			if (this._receivePowerShellOutput != null)
			{
				this._receivePowerShellOutput.Dispose();
			}
			if (this._removeComplete != null)
			{
				this._removeComplete.Dispose();
			}
			if (this._jobRunningOrFinished != null)
			{
				this._jobRunningOrFinished.Dispose();
			}
			this._jobInitialiedWaitHandle.Dispose();
			if (this._jobSuspendedOrFinished != null)
			{
				this._jobSuspendedOrFinished.Dispose();
			}
			if (base.ChildJobs != null && base.ChildJobs.Count > 0)
			{
				foreach (Job job in base.ChildJobs)
				{
					job.Dispose();
				}
			}
			this._tracer.Dispose();
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06001F02 RID: 7938 RVA: 0x000B433C File Offset: 0x000B253C
		private ManualResetEvent RemoveComplete
		{
			get
			{
				if (this._removeComplete == null)
				{
					lock (base.SyncRoot)
					{
						if (this._removeComplete == null)
						{
							this.AssertNotDisposed();
							this._removeComplete = new ManualResetEvent(false);
						}
					}
				}
				return this._removeComplete;
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06001F03 RID: 7939 RVA: 0x000B43A0 File Offset: 0x000B25A0
		private ManualResetEvent JobRunningOrFinished
		{
			get
			{
				if (this._jobRunningOrFinished == null)
				{
					lock (base.SyncRoot)
					{
						if (this._jobRunningOrFinished == null)
						{
							this.AssertNotDisposed();
							this._jobRunningOrFinished = new ManualResetEvent(false);
						}
					}
				}
				return this._jobRunningOrFinished;
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06001F04 RID: 7940 RVA: 0x000B4404 File Offset: 0x000B2604
		private ManualResetEvent JobSuspendedOrFinished
		{
			get
			{
				if (this._jobSuspendedOrFinished == null)
				{
					lock (base.SyncRoot)
					{
						if (this._jobSuspendedOrFinished == null)
						{
							this.AssertNotDisposed();
							this._jobSuspendedOrFinished = new ManualResetEvent(false);
						}
					}
				}
				return this._jobSuspendedOrFinished;
			}
		}

		// Token: 0x04000D77 RID: 3447
		private const string ResBaseName = "PowerShellStrings";

		// Token: 0x04000D78 RID: 3448
		private const string ClassNameTrace = "PSJobProxy";

		// Token: 0x04000D7A RID: 3450
		private bool _removeRemoteJobOnCompletion;

		// Token: 0x04000D7B RID: 3451
		private ConcurrentQueue<PSJobProxy.QueueOperation> _pendingOperations = new ConcurrentQueue<PSJobProxy.QueueOperation>();

		// Token: 0x04000D7C RID: 3452
		private ManualResetEvent _removeComplete;

		// Token: 0x04000D7D RID: 3453
		private ManualResetEvent _jobRunningOrFinished;

		// Token: 0x04000D7E RID: 3454
		private readonly ManualResetEvent _jobInitialiedWaitHandle = new ManualResetEvent(false);

		// Token: 0x04000D7F RID: 3455
		private ManualResetEvent _jobSuspendedOrFinished;

		// Token: 0x04000D80 RID: 3456
		private PSCommand _pscommand;

		// Token: 0x04000D81 RID: 3457
		private Runspace _runspace;

		// Token: 0x04000D82 RID: 3458
		private RunspacePool _runspacePool;

		// Token: 0x04000D83 RID: 3459
		private EventHandler<JobDataAddedEventArgs> _dataAddedHandler;

		// Token: 0x04000D84 RID: 3460
		private EventHandler<JobStateEventArgs> _stateChangedHandler;

		// Token: 0x04000D85 RID: 3461
		private Guid _remoteJobInstanceId = Guid.Empty;

		// Token: 0x04000D86 RID: 3462
		private string _remoteJobStatusMessage = string.Empty;

		// Token: 0x04000D87 RID: 3463
		private string _remoteJobLocation = string.Empty;

		// Token: 0x04000D88 RID: 3464
		private readonly Hashtable _childJobsMapping = new Hashtable();

		// Token: 0x04000D89 RID: 3465
		private readonly PowerShell _receivePowerShell = PowerShell.Create();

		// Token: 0x04000D8A RID: 3466
		private readonly PSDataCollection<PSObject> _receivePowerShellOutput = new PSDataCollection<PSObject>();

		// Token: 0x04000D8B RID: 3467
		private bool _moreData = true;

		// Token: 0x04000D8C RID: 3468
		private JobState _previousState;

		// Token: 0x04000D8D RID: 3469
		private JobState _computedJobState;

		// Token: 0x04000D8E RID: 3470
		private readonly PowerShellTraceSource _tracer = PowerShellTraceSourceFactory.GetTraceSource();

		// Token: 0x04000D8F RID: 3471
		private static Tracer StructuredTracer = new Tracer();

		// Token: 0x04000D90 RID: 3472
		private bool _jobInitialized;

		// Token: 0x04000D91 RID: 3473
		private bool _removeCalled;

		// Token: 0x04000D92 RID: 3474
		private bool _startCalled;

		// Token: 0x04000D93 RID: 3475
		private bool _receiveIsValidCall;

		// Token: 0x04000D94 RID: 3476
		private bool _isDisposed;

		// Token: 0x04000D95 RID: 3477
		private bool _remoteJobRemoved;

		// Token: 0x04000D96 RID: 3478
		private bool _childEventsRegistered;

		// Token: 0x04000D97 RID: 3479
		private object _inProgressSyncObject = new object();

		// Token: 0x04000D98 RID: 3480
		private bool _inProgress;

		// Token: 0x04000D99 RID: 3481
		private bool _workerCreated;

		// Token: 0x04000D9A RID: 3482
		private int _finishedChildJobsCount;

		// Token: 0x04000D9B RID: 3483
		private int _blockedChildJobsCount;

		// Token: 0x04000D9C RID: 3484
		private int _suspendedChildJobsCount;

		// Token: 0x04000D9D RID: 3485
		private int _suspendingChildJobsCount;

		// Token: 0x04000D9E RID: 3486
		private int _failedChildJobsCount;

		// Token: 0x04000D9F RID: 3487
		private int _stoppedChildJobsCount;

		// Token: 0x02000285 RID: 645
		private enum ActionType
		{
			// Token: 0x04000DA5 RID: 3493
			Remove
		}

		// Token: 0x02000286 RID: 646
		private class AsyncCompleteContainer
		{
			// Token: 0x04000DA6 RID: 3494
			internal AsyncCompletedEventArgs EventArgs;

			// Token: 0x04000DA7 RID: 3495
			internal PSJobProxy.ActionType Action;
		}

		// Token: 0x02000287 RID: 647
		// (Invoke) Token: 0x06001F0C RID: 7948
		private delegate void JobActionWorkerDelegate(AsyncOperation asyncOp, PSJobProxy.ActionType action);

		// Token: 0x02000288 RID: 648
		private enum QueueOperation
		{
			// Token: 0x04000DA9 RID: 3497
			Stop,
			// Token: 0x04000DAA RID: 3498
			Suspend,
			// Token: 0x04000DAB RID: 3499
			Resume
		}
	}
}
