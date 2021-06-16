using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Runspaces.Internal;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x02000263 RID: 611
	internal class RemotePipeline : Pipeline
	{
		// Token: 0x06001C79 RID: 7289 RVA: 0x000A5F58 File Offset: 0x000A4158
		private RemotePipeline(RemoteRunspace runspace, bool addToHistory, bool isNested) : base(runspace)
		{
			this._addToHistory = addToHistory;
			this._isNested = isNested;
			this._isSteppable = false;
			this._runspace = runspace;
			this._computerName = ((RemoteRunspace)this._runspace).ConnectionInfo.ComputerName;
			this._runspaceId = this._runspace.InstanceId;
			this._inputCollection = new PSDataCollection<object>();
			this._inputCollection.ReleaseOnEnumeration = true;
			this._inputStream = new PSDataCollectionStream<object>(Guid.Empty, this._inputCollection);
			this._outputCollection = new PSDataCollection<PSObject>();
			this._outputStream = new PSDataCollectionStream<PSObject>(Guid.Empty, this._outputCollection);
			this._errorCollection = new PSDataCollection<ErrorRecord>();
			this._errorStream = new PSDataCollectionStream<ErrorRecord>(Guid.Empty, this._errorCollection);
			this._methodExecutorStream = new ObjectStream();
			this._isMethodExecutorStreamEnabled = false;
			base.SetCommandCollection(this._commands);
			this._pipelineFinishedEvent = new ManualResetEvent(false);
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x000A6084 File Offset: 0x000A4284
		internal RemotePipeline(RemoteRunspace runspace, string command, bool addToHistory, bool isNested) : this(runspace, addToHistory, isNested)
		{
			if (command != null)
			{
				this._commands.Add(new Command(command, true));
			}
			this._powershell = new PowerShell(this._inputStream, this._outputStream, this._errorStream, ((RemoteRunspace)this._runspace).RunspacePool);
			this._powershell.SetIsNested(isNested);
			this._powershell.InvocationStateChanged += this.HandleInvocationStateChanged;
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x000A6104 File Offset: 0x000A4304
		internal RemotePipeline(RemoteRunspace runspace) : this(runspace, false, false)
		{
			if (runspace.RemoteCommand == null)
			{
				throw new InvalidOperationException(PipelineStrings.InvalidRemoteCommand);
			}
			this._connectCmdInfo = runspace.RemoteCommand;
			this._commands.Add(this._connectCmdInfo.Command);
			this.SetPipelineState(PipelineState.Disconnected, null);
			this._powershell = new PowerShell(this._connectCmdInfo, this._inputStream, this._outputStream, this._errorStream, ((RemoteRunspace)this._runspace).RunspacePool);
			this._powershell.InvocationStateChanged += this.HandleInvocationStateChanged;
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x000A61A4 File Offset: 0x000A43A4
		private RemotePipeline(RemotePipeline pipeline) : this((RemoteRunspace)pipeline.Runspace, null, false, pipeline.IsNested)
		{
			this._isSteppable = pipeline._isSteppable;
			if (pipeline == null)
			{
				throw PSTraceSource.NewArgumentNullException("pipeline");
			}
			if (pipeline._disposed)
			{
				throw PSTraceSource.NewObjectDisposedException("pipeline");
			}
			this._addToHistory = pipeline._addToHistory;
			this._historyString = pipeline._historyString;
			foreach (Command command in pipeline.Commands)
			{
				Command item = command.Clone();
				base.Commands.Add(item);
			}
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x000A625C File Offset: 0x000A445C
		public override Pipeline Copy()
		{
			if (this._disposed)
			{
				throw PSTraceSource.NewObjectDisposedException("pipeline");
			}
			return new RemotePipeline(this);
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06001C7E RID: 7294 RVA: 0x000A6277 File Offset: 0x000A4477
		public override Runspace Runspace
		{
			get
			{
				if (this._disposed)
				{
					throw PSTraceSource.NewObjectDisposedException("pipeline");
				}
				return this._runspace;
			}
		}

		// Token: 0x06001C7F RID: 7295 RVA: 0x000A6292 File Offset: 0x000A4492
		internal Runspace GetRunspace()
		{
			return this._runspace;
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06001C80 RID: 7296 RVA: 0x000A629A File Offset: 0x000A449A
		public override bool IsNested
		{
			get
			{
				return this._isNested;
			}
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x000A62A2 File Offset: 0x000A44A2
		internal void SetIsNested(bool isNested)
		{
			this._isNested = isNested;
			this._powershell.SetIsNested(isNested);
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x000A62B7 File Offset: 0x000A44B7
		internal void SetIsSteppable(bool isSteppable)
		{
			this._isSteppable = isSteppable;
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06001C83 RID: 7299 RVA: 0x000A62C0 File Offset: 0x000A44C0
		public override PipelineStateInfo PipelineStateInfo
		{
			get
			{
				PipelineStateInfo result;
				lock (this._syncRoot)
				{
					result = this._pipelineStateInfo.Clone();
				}
				return result;
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06001C84 RID: 7300 RVA: 0x000A6308 File Offset: 0x000A4508
		public override PipelineWriter Input
		{
			get
			{
				return this._inputStream.ObjectWriter;
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06001C85 RID: 7301 RVA: 0x000A6315 File Offset: 0x000A4515
		public override PipelineReader<PSObject> Output
		{
			get
			{
				return this._outputStream.GetPSObjectReaderForPipeline(this._computerName, this._runspaceId);
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06001C86 RID: 7302 RVA: 0x000A632E File Offset: 0x000A452E
		public override PipelineReader<object> Error
		{
			get
			{
				return this._errorStream.GetObjectReaderForPipeline(this._computerName, this._runspaceId);
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06001C87 RID: 7303 RVA: 0x000A6347 File Offset: 0x000A4547
		// (set) Token: 0x06001C88 RID: 7304 RVA: 0x000A634F File Offset: 0x000A454F
		internal string HistoryString
		{
			get
			{
				return this._historyString;
			}
			set
			{
				this._historyString = value;
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06001C89 RID: 7305 RVA: 0x000A6358 File Offset: 0x000A4558
		public bool AddToHistory
		{
			get
			{
				return this._addToHistory;
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06001C8A RID: 7306 RVA: 0x000A6360 File Offset: 0x000A4560
		protected PSDataCollectionStream<object> InputStream
		{
			get
			{
				return this._inputStream;
			}
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x000A6368 File Offset: 0x000A4568
		public override void InvokeAsync()
		{
			this.InitPowerShell(false, false);
			this.CoreInvokeAsync();
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x000A6378 File Offset: 0x000A4578
		internal override void InvokeAsyncAndDisconnect()
		{
			this.InitPowerShell(false, true);
			this.CoreInvokeAsync();
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x000A6388 File Offset: 0x000A4588
		public override Collection<PSObject> Invoke(IEnumerable input)
		{
			if (input == null)
			{
				this.InputStream.Close();
			}
			this.InitPowerShell(true, false);
			Collection<PSObject> result;
			try
			{
				result = this._powershell.Invoke(input);
			}
			catch (InvalidRunspacePoolStateException)
			{
				InvalidRunspaceStateException ex = new InvalidRunspaceStateException(StringUtil.Format(RunspaceStrings.RunspaceNotOpenForPipeline, this._runspace.RunspaceStateInfo.State.ToString()), this._runspace.RunspaceStateInfo.State, RunspaceState.Opened);
				throw ex;
			}
			return result;
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x000A640C File Offset: 0x000A460C
		public override Collection<PSObject> Connect()
		{
			this.InitPowerShellForConnect(true);
			Collection<PSObject> collection;
			try
			{
				collection = this._powershell.Connect();
			}
			catch (InvalidRunspacePoolStateException)
			{
				InvalidRunspaceStateException ex = new InvalidRunspaceStateException(StringUtil.Format(RunspaceStrings.RunspaceNotOpenForPipelineConnect, this._runspace.RunspaceStateInfo.State.ToString()), this._runspace.RunspaceStateInfo.State, RunspaceState.Opened);
				throw ex;
			}
			if (collection.Count == 0 && this._outputCollection != null && this._outputCollection.Count > 0)
			{
				collection = new Collection<PSObject>(this._outputCollection);
			}
			return collection;
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x000A64A8 File Offset: 0x000A46A8
		public override void ConnectAsync()
		{
			this.InitPowerShellForConnect(false);
			try
			{
				this._powershell.ConnectAsync();
			}
			catch (InvalidRunspacePoolStateException)
			{
				InvalidRunspaceStateException ex = new InvalidRunspaceStateException(StringUtil.Format(RunspaceStrings.RunspaceNotOpenForPipelineConnect, this._runspace.RunspaceStateInfo.State.ToString()), this._runspace.RunspaceStateInfo.State, RunspaceState.Opened);
				throw ex;
			}
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x000A6518 File Offset: 0x000A4718
		public override void Stop()
		{
			bool flag = false;
			if (this.CanStopPipeline(out flag) && this._powershell != null)
			{
				IAsyncResult asyncResult = null;
				try
				{
					asyncResult = this._powershell.BeginStop(null, null);
				}
				catch (ObjectDisposedException)
				{
					throw PSTraceSource.NewObjectDisposedException("Pipeline");
				}
				asyncResult.AsyncWaitHandle.WaitOne();
			}
			this.PipelineFinishedEvent.WaitOne();
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x000A6580 File Offset: 0x000A4780
		public override void StopAsync()
		{
			bool flag;
			if (this.CanStopPipeline(out flag))
			{
				try
				{
					this._powershell.BeginStop(null, null);
				}
				catch (ObjectDisposedException)
				{
					throw PSTraceSource.NewObjectDisposedException("Pipeline");
				}
			}
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x000A65C4 File Offset: 0x000A47C4
		private bool CanStopPipeline(out bool isAlreadyStopping)
		{
			bool result = false;
			isAlreadyStopping = false;
			lock (this._syncRoot)
			{
				switch (this._pipelineStateInfo.State)
				{
				case PipelineState.NotStarted:
					this.SetPipelineState(PipelineState.Stopping, null);
					this.SetPipelineState(PipelineState.Stopped, null);
					result = false;
					break;
				case PipelineState.Running:
				case PipelineState.Disconnected:
					this.SetPipelineState(PipelineState.Stopping, null);
					result = true;
					break;
				case PipelineState.Stopping:
					isAlreadyStopping = true;
					return false;
				case PipelineState.Stopped:
				case PipelineState.Completed:
				case PipelineState.Failed:
					return false;
				}
			}
			this.RaisePipelineStateEvents();
			return result;
		}

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06001C93 RID: 7315 RVA: 0x000A6668 File Offset: 0x000A4868
		// (remove) Token: 0x06001C94 RID: 7316 RVA: 0x000A66A0 File Offset: 0x000A48A0
		public override event EventHandler<PipelineStateEventArgs> StateChanged;

		// Token: 0x06001C95 RID: 7317 RVA: 0x000A66D8 File Offset: 0x000A48D8
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					lock (this._syncRoot)
					{
						if (this._disposed)
						{
							return;
						}
						this._disposed = true;
					}
					if (disposing)
					{
						this.Stop();
						if (this._powershell != null)
						{
							this._powershell.Dispose();
							this._powershell = null;
						}
						this._inputCollection.Dispose();
						this._inputStream.Dispose();
						this._outputCollection.Dispose();
						this._outputStream.Dispose();
						this._errorCollection.Dispose();
						this._errorStream.Dispose();
						this._methodExecutorStream.Dispose();
						this._pipelineFinishedEvent.Dispose();
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x000A67C4 File Offset: 0x000A49C4
		private void CoreInvokeAsync()
		{
			try
			{
				this._powershell.BeginInvoke();
			}
			catch (InvalidRunspacePoolStateException)
			{
				InvalidRunspaceStateException ex = new InvalidRunspaceStateException(StringUtil.Format(RunspaceStrings.RunspaceNotOpenForPipeline, this._runspace.RunspaceStateInfo.State.ToString()), this._runspace.RunspaceStateInfo.State, RunspaceState.Opened);
				throw ex;
			}
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x000A6830 File Offset: 0x000A4A30
		private void HandleInvocationStateChanged(object sender, PSInvocationStateChangedEventArgs e)
		{
			this.SetPipelineState((PipelineState)e.InvocationStateInfo.State, e.InvocationStateInfo.Reason);
			this.RaisePipelineStateEvents();
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x000A6854 File Offset: 0x000A4A54
		private void SetPipelineState(PipelineState state, Exception reason)
		{
			PipelineState state2 = state;
			PipelineStateInfo pipelineStateInfo = null;
			lock (this._syncRoot)
			{
				switch (this._pipelineStateInfo.State)
				{
				case PipelineState.Running:
					if (state == PipelineState.Running)
					{
						return;
					}
					break;
				case PipelineState.Stopping:
					if (state == PipelineState.Running || state == PipelineState.Stopping)
					{
						return;
					}
					state2 = PipelineState.Stopped;
					break;
				case PipelineState.Stopped:
				case PipelineState.Completed:
				case PipelineState.Failed:
					return;
				}
				this._pipelineStateInfo = new PipelineStateInfo(state2, reason);
				pipelineStateInfo = this._pipelineStateInfo;
				RunspaceAvailability runspaceAvailability = this._runspace.RunspaceAvailability;
				Guid? cmdInstanceId = (this._powershell != null) ? new Guid?(this._powershell.InstanceId) : null;
				this._runspace.UpdateRunspaceAvailability(this._pipelineStateInfo.State, false, cmdInstanceId);
				this._executionEventQueue.Enqueue(new RemotePipeline.ExecutionEventQueueItem(this._pipelineStateInfo.Clone(), runspaceAvailability, this._runspace.RunspaceAvailability));
			}
			if (pipelineStateInfo.State == PipelineState.Completed || pipelineStateInfo.State == PipelineState.Failed || pipelineStateInfo.State == PipelineState.Stopped)
			{
				this.Cleanup();
			}
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x000A6984 File Offset: 0x000A4B84
		protected void RaisePipelineStateEvents()
		{
			Queue<RemotePipeline.ExecutionEventQueueItem> queue = null;
			EventHandler<PipelineStateEventArgs> eventHandler = null;
			bool flag = false;
			lock (this._syncRoot)
			{
				eventHandler = this.StateChanged;
				flag = this._runspace.HasAvailabilityChangedSubscribers;
				if (eventHandler != null || flag)
				{
					queue = this._executionEventQueue;
					this._executionEventQueue = new Queue<RemotePipeline.ExecutionEventQueueItem>();
				}
				else
				{
					this._executionEventQueue.Clear();
				}
			}
			if (queue != null)
			{
				while (queue.Count > 0)
				{
					RemotePipeline.ExecutionEventQueueItem executionEventQueueItem = queue.Dequeue();
					if (flag && executionEventQueueItem.NewRunspaceAvailability != executionEventQueueItem.CurrentRunspaceAvailability)
					{
						this._runspace.RaiseAvailabilityChangedEvent(executionEventQueueItem.NewRunspaceAvailability);
					}
					if (eventHandler != null)
					{
						try
						{
							eventHandler(this, new PipelineStateEventArgs(executionEventQueueItem.PipelineStateInfo));
						}
						catch (Exception e)
						{
							CommandProcessorBase.CheckForSevereException(e);
						}
					}
				}
			}
		}

		// Token: 0x06001C9A RID: 7322 RVA: 0x000A6A68 File Offset: 0x000A4C68
		private void InitPowerShell(bool syncCall, bool invokeAndDisconnect = false)
		{
			if (this._commands == null || this._commands.Count == 0)
			{
				throw PSTraceSource.NewInvalidOperationException(RunspaceStrings.NoCommandInPipeline, new object[0]);
			}
			if (this._pipelineStateInfo.State != PipelineState.NotStarted)
			{
				InvalidPipelineStateException ex = new InvalidPipelineStateException(StringUtil.Format(RunspaceStrings.PipelineReInvokeNotAllowed, new object[0]), this._pipelineStateInfo.State, PipelineState.NotStarted);
				throw ex;
			}
			((RemoteRunspace)this._runspace).DoConcurrentCheckAndAddToRunningPipelines(this, syncCall);
			PSInvocationSettings psinvocationSettings = new PSInvocationSettings();
			psinvocationSettings.AddToHistory = this._addToHistory;
			psinvocationSettings.InvokeAndDisconnect = invokeAndDisconnect;
			this._powershell.InitForRemotePipeline(this._commands, this._inputStream, this._outputStream, this._errorStream, psinvocationSettings, base.RedirectShellErrorOutputPipe);
			this._powershell.RemotePowerShell.HostCallReceived += this.HandleHostCallReceived;
		}

		// Token: 0x06001C9B RID: 7323 RVA: 0x000A6B40 File Offset: 0x000A4D40
		private void InitPowerShellForConnect(bool syncCall)
		{
			if (this._pipelineStateInfo.State != PipelineState.Disconnected)
			{
				throw new InvalidPipelineStateException(StringUtil.Format(PipelineStrings.PipelineNotDisconnected, new object[0]), this._pipelineStateInfo.State, PipelineState.Disconnected);
			}
			RemotePipeline remotePipeline = (RemotePipeline)((RemoteRunspace)this._runspace).GetCurrentlyRunningPipeline();
			if (remotePipeline == null || (remotePipeline != null && !object.ReferenceEquals(remotePipeline, this)))
			{
				((RemoteRunspace)this._runspace).DoConcurrentCheckAndAddToRunningPipelines(this, syncCall);
			}
			if (this._powershell.RemotePowerShell == null || !this._powershell.RemotePowerShell.Initialized)
			{
				PSInvocationSettings psinvocationSettings = new PSInvocationSettings();
				psinvocationSettings.AddToHistory = this._addToHistory;
				this._powershell.InitForRemotePipelineConnect(this._inputStream, this._outputStream, this._errorStream, psinvocationSettings, base.RedirectShellErrorOutputPipe);
				this._powershell.RemotePowerShell.HostCallReceived += this.HandleHostCallReceived;
			}
		}

		// Token: 0x06001C9C RID: 7324 RVA: 0x000A6C28 File Offset: 0x000A4E28
		private void HandleHostCallReceived(object sender, RemoteDataEventArgs<RemoteHostCall> eventArgs)
		{
			ClientMethodExecutor.Dispatch(this._powershell.RemotePowerShell.DataStructureHandler.TransportManager, ((RemoteRunspace)this._runspace).RunspacePool.RemoteRunspacePoolInternal.Host, this._errorStream, this._methodExecutorStream, this.IsMethodExecutorStreamEnabled, ((RemoteRunspace)this._runspace).RunspacePool.RemoteRunspacePoolInternal, this._powershell.InstanceId, eventArgs.Data);
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x000A6CA4 File Offset: 0x000A4EA4
		private void Cleanup()
		{
			if (this._outputStream.IsOpen)
			{
				try
				{
					this._outputCollection.Complete();
					this._outputStream.Close();
				}
				catch (ObjectDisposedException)
				{
				}
			}
			if (this._errorStream.IsOpen)
			{
				try
				{
					this._errorCollection.Complete();
					this._errorStream.Close();
				}
				catch (ObjectDisposedException)
				{
				}
			}
			if (this._inputStream.IsOpen)
			{
				try
				{
					this._inputCollection.Complete();
					this._inputStream.Close();
				}
				catch (ObjectDisposedException)
				{
				}
			}
			try
			{
				((RemoteRunspace)this._runspace).RemoveFromRunningPipelineList(this);
				this._pipelineFinishedEvent.Set();
			}
			catch (ObjectDisposedException)
			{
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06001C9E RID: 7326 RVA: 0x000A6D80 File Offset: 0x000A4F80
		internal ManualResetEvent PipelineFinishedEvent
		{
			get
			{
				return this._pipelineFinishedEvent;
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06001C9F RID: 7327 RVA: 0x000A6D88 File Offset: 0x000A4F88
		// (set) Token: 0x06001CA0 RID: 7328 RVA: 0x000A6D90 File Offset: 0x000A4F90
		internal bool IsMethodExecutorStreamEnabled
		{
			get
			{
				return this._isMethodExecutorStreamEnabled;
			}
			set
			{
				this._isMethodExecutorStreamEnabled = value;
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06001CA1 RID: 7329 RVA: 0x000A6D99 File Offset: 0x000A4F99
		internal ObjectStream MethodExecutorStream
		{
			get
			{
				return this._methodExecutorStream;
			}
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x000A6DA4 File Offset: 0x000A4FA4
		internal void DoConcurrentCheck(bool syncCall)
		{
			RemotePipeline remotePipeline = (RemotePipeline)((RemoteRunspace)this._runspace).GetCurrentlyRunningPipeline();
			if (!this._isNested)
			{
				if (remotePipeline == null && ((RemoteRunspace)this._runspace).RunspaceAvailability != RunspaceAvailability.Busy && ((RemoteRunspace)this._runspace).RunspaceAvailability != RunspaceAvailability.RemoteDebug)
				{
					return;
				}
				if (remotePipeline == null && ((RemoteRunspace)this._runspace).RemoteCommand != null && this._connectCmdInfo != null && object.Equals(((RemoteRunspace)this._runspace).RemoteCommand.CommandId, this._connectCmdInfo.CommandId))
				{
					return;
				}
				if (remotePipeline != null && object.ReferenceEquals(remotePipeline, this))
				{
					return;
				}
				if (!this._isSteppable)
				{
					throw PSTraceSource.NewInvalidOperationException(RunspaceStrings.ConcurrentInvokeNotAllowed, new object[0]);
				}
			}
			else if (this._performNestedCheck)
			{
				if (this._isSteppable)
				{
					return;
				}
				if (!syncCall)
				{
					throw PSTraceSource.NewInvalidOperationException(RunspaceStrings.NestedPipelineInvokeAsync, new object[0]);
				}
				if (remotePipeline == null && !this._isSteppable)
				{
					throw PSTraceSource.NewInvalidOperationException(RunspaceStrings.NestedPipelineNoParentPipeline, new object[0]);
				}
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06001CA3 RID: 7331 RVA: 0x000A6EB2 File Offset: 0x000A50B2
		internal PowerShell PowerShell
		{
			get
			{
				return this._powershell;
			}
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x000A6EBA File Offset: 0x000A50BA
		internal override void SetHistoryString(string historyString)
		{
			this._powershell.HistoryString = historyString;
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x000A6EC8 File Offset: 0x000A50C8
		internal override void SuspendIncomingData()
		{
			this._powershell.SuspendIncomingData();
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x000A6ED5 File Offset: 0x000A50D5
		internal override void ResumeIncomingData()
		{
			this._powershell.ResumeIncomingData();
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x000A6EE2 File Offset: 0x000A50E2
		internal override void DrainIncomingData()
		{
			this._powershell.WaitForServicingComplete();
		}

		// Token: 0x04000CA1 RID: 3233
		private PowerShell _powershell;

		// Token: 0x04000CA2 RID: 3234
		private bool _addToHistory;

		// Token: 0x04000CA3 RID: 3235
		private bool _isNested;

		// Token: 0x04000CA4 RID: 3236
		private bool _isSteppable;

		// Token: 0x04000CA5 RID: 3237
		private Runspace _runspace;

		// Token: 0x04000CA6 RID: 3238
		private object _syncRoot = new object();

		// Token: 0x04000CA7 RID: 3239
		private bool _disposed;

		// Token: 0x04000CA8 RID: 3240
		private string _historyString;

		// Token: 0x04000CA9 RID: 3241
		private PipelineStateInfo _pipelineStateInfo = new PipelineStateInfo(PipelineState.NotStarted);

		// Token: 0x04000CAA RID: 3242
		private CommandCollection _commands = new CommandCollection();

		// Token: 0x04000CAB RID: 3243
		private string _computerName;

		// Token: 0x04000CAC RID: 3244
		private Guid _runspaceId;

		// Token: 0x04000CAD RID: 3245
		private ConnectCommandInfo _connectCmdInfo;

		// Token: 0x04000CAE RID: 3246
		private Queue<RemotePipeline.ExecutionEventQueueItem> _executionEventQueue = new Queue<RemotePipeline.ExecutionEventQueueItem>();

		// Token: 0x04000CAF RID: 3247
		private ManualResetEvent _pipelineFinishedEvent;

		// Token: 0x04000CB0 RID: 3248
		private bool _isMethodExecutorStreamEnabled;

		// Token: 0x04000CB1 RID: 3249
		private ObjectStream _methodExecutorStream;

		// Token: 0x04000CB2 RID: 3250
		private bool _performNestedCheck = true;

		// Token: 0x04000CB3 RID: 3251
		private PSDataCollection<PSObject> _outputCollection;

		// Token: 0x04000CB4 RID: 3252
		private PSDataCollectionStream<PSObject> _outputStream;

		// Token: 0x04000CB5 RID: 3253
		private PSDataCollection<ErrorRecord> _errorCollection;

		// Token: 0x04000CB6 RID: 3254
		private PSDataCollectionStream<ErrorRecord> _errorStream;

		// Token: 0x04000CB7 RID: 3255
		private PSDataCollection<object> _inputCollection;

		// Token: 0x04000CB8 RID: 3256
		private PSDataCollectionStream<object> _inputStream;

		// Token: 0x02000264 RID: 612
		private class ExecutionEventQueueItem
		{
			// Token: 0x06001CA8 RID: 7336 RVA: 0x000A6EEF File Offset: 0x000A50EF
			public ExecutionEventQueueItem(PipelineStateInfo pipelineStateInfo, RunspaceAvailability currentAvailability, RunspaceAvailability newAvailability)
			{
				this.PipelineStateInfo = pipelineStateInfo;
				this.CurrentRunspaceAvailability = currentAvailability;
				this.NewRunspaceAvailability = newAvailability;
			}

			// Token: 0x04000CBA RID: 3258
			public PipelineStateInfo PipelineStateInfo;

			// Token: 0x04000CBB RID: 3259
			public RunspaceAvailability CurrentRunspaceAvailability;

			// Token: 0x04000CBC RID: 3260
			public RunspaceAvailability NewRunspaceAvailability;
		}
	}
}
