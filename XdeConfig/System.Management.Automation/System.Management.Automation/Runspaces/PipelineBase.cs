using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation.Internal;
using System.Threading;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000219 RID: 537
	internal abstract class PipelineBase : Pipeline
	{
		// Token: 0x06001947 RID: 6471 RVA: 0x000985A0 File Offset: 0x000967A0
		protected PipelineBase(Runspace runspace, string command, bool addToHistory, bool isNested) : base(runspace)
		{
			this.Initialize(runspace, command, addToHistory, isNested);
			this._inputStream = new ObjectStream();
			this._outputStream = new ObjectStream();
			this.ErrorStream = new ObjectStream();
		}

		// Token: 0x06001948 RID: 6472 RVA: 0x0009860C File Offset: 0x0009680C
		protected PipelineBase(Runspace runspace, CommandCollection command, bool addToHistory, bool isNested, ObjectStreamBase inputStream, ObjectStreamBase outputStream, ObjectStreamBase errorStream, PSInformationalBuffers infoBuffers) : base(runspace, command)
		{
			this.Initialize(runspace, null, false, isNested);
			if (addToHistory)
			{
				string commandStringForHistory = command.GetCommandStringForHistory();
				this._historyString = commandStringForHistory;
				this._addToHistory = addToHistory;
			}
			this._inputStream = inputStream;
			this._outputStream = outputStream;
			this.ErrorStream = errorStream;
			this._informationalBuffers = infoBuffers;
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x00098690 File Offset: 0x00096890
		protected PipelineBase(PipelineBase pipeline) : this(pipeline.Runspace, null, false, pipeline.IsNested)
		{
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

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x0600194A RID: 6474 RVA: 0x00098738 File Offset: 0x00096938
		public override Runspace Runspace
		{
			get
			{
				return this._runspace;
			}
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x00098740 File Offset: 0x00096940
		internal Runspace GetRunspace()
		{
			return this._runspace;
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x0600194C RID: 6476 RVA: 0x00098748 File Offset: 0x00096948
		public override bool IsNested
		{
			get
			{
				return this._isNested;
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x0600194D RID: 6477 RVA: 0x00098750 File Offset: 0x00096950
		// (set) Token: 0x0600194E RID: 6478 RVA: 0x00098758 File Offset: 0x00096958
		internal bool IsPulsePipeline
		{
			get
			{
				return this._isPulsePipeline;
			}
			set
			{
				this._isPulsePipeline = value;
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x0600194F RID: 6479 RVA: 0x00098764 File Offset: 0x00096964
		public override PipelineStateInfo PipelineStateInfo
		{
			get
			{
				PipelineStateInfo result;
				lock (this.SyncRoot)
				{
					result = this._pipelineStateInfo.Clone();
				}
				return result;
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06001950 RID: 6480 RVA: 0x000987AC File Offset: 0x000969AC
		public override PipelineWriter Input
		{
			get
			{
				return this._inputStream.ObjectWriter;
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06001951 RID: 6481 RVA: 0x000987B9 File Offset: 0x000969B9
		public override PipelineReader<PSObject> Output
		{
			get
			{
				return this._outputStream.PSObjectReader;
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x06001952 RID: 6482 RVA: 0x000987C6 File Offset: 0x000969C6
		public override PipelineReader<object> Error
		{
			get
			{
				return this._errorStream.ObjectReader;
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x06001953 RID: 6483 RVA: 0x000987D3 File Offset: 0x000969D3
		// (set) Token: 0x06001954 RID: 6484 RVA: 0x000987DB File Offset: 0x000969DB
		internal override bool IsChild { get; set; }

		// Token: 0x06001955 RID: 6485 RVA: 0x000987E4 File Offset: 0x000969E4
		public override void Stop()
		{
			this.CoreStop(true);
		}

		// Token: 0x06001956 RID: 6486 RVA: 0x000987ED File Offset: 0x000969ED
		public override void StopAsync()
		{
			this.CoreStop(false);
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x000987F8 File Offset: 0x000969F8
		private void CoreStop(bool syncCall)
		{
			bool flag = false;
			lock (this.SyncRoot)
			{
				switch (this.PipelineState)
				{
				case PipelineState.NotStarted:
					this.SetPipelineState(PipelineState.Stopping);
					this.SetPipelineState(PipelineState.Stopped);
					break;
				case PipelineState.Running:
					this.SetPipelineState(PipelineState.Stopping);
					break;
				case PipelineState.Stopping:
					flag = true;
					break;
				case PipelineState.Stopped:
				case PipelineState.Completed:
				case PipelineState.Failed:
					return;
				}
			}
			if (!flag)
			{
				this.RaisePipelineStateEvents();
				lock (this.SyncRoot)
				{
					if (this.PipelineState == PipelineState.Stopped)
					{
						return;
					}
				}
				this.ImplementStop(syncCall);
				return;
			}
			if (syncCall)
			{
				this.PipelineFinishedEvent.WaitOne();
			}
		}

		// Token: 0x06001958 RID: 6488
		protected abstract void ImplementStop(bool syncCall);

		// Token: 0x06001959 RID: 6489 RVA: 0x000988D0 File Offset: 0x00096AD0
		public override Collection<PSObject> Invoke(IEnumerable input)
		{
			if (this._disposed)
			{
				throw PSTraceSource.NewObjectDisposedException("pipeline");
			}
			this.CoreInvoke(input, true);
			this.PipelineFinishedEvent.WaitOne();
			if (this.SyncInvokeCall)
			{
				this.RaisePipelineStateEvents();
			}
			if (this.PipelineStateInfo.State == PipelineState.Stopped)
			{
				return new Collection<PSObject>();
			}
			if (this.PipelineStateInfo.State == PipelineState.Failed && this.PipelineStateInfo.Reason != null)
			{
				if (this.Runspace.GetExecutionContext.EngineHostInterface.UI.IsTranscribing)
				{
					this.Runspace.ExecutionContext.InternalHost.UI.TranscribeResult(this.Runspace, this.PipelineStateInfo.Reason.Message);
				}
				RuntimeException.LockStackTrace(this.PipelineStateInfo.Reason);
				throw this.PipelineStateInfo.Reason;
			}
			return this.Output.NonBlockingRead(int.MaxValue);
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x000989B9 File Offset: 0x00096BB9
		public override void InvokeAsync()
		{
			this.CoreInvoke(null, false);
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x0600195B RID: 6491 RVA: 0x000989C3 File Offset: 0x00096BC3
		protected bool SyncInvokeCall
		{
			get
			{
				return this._syncInvokeCall;
			}
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x000989CC File Offset: 0x00096BCC
		private void CoreInvoke(IEnumerable input, bool syncCall)
		{
			lock (this.SyncRoot)
			{
				if (this._disposed)
				{
					throw PSTraceSource.NewObjectDisposedException("pipeline");
				}
				if (base.Commands == null || base.Commands.Count == 0)
				{
					throw PSTraceSource.NewInvalidOperationException(RunspaceStrings.NoCommandInPipeline, new object[0]);
				}
				if (this.PipelineState != PipelineState.NotStarted)
				{
					InvalidPipelineStateException ex = new InvalidPipelineStateException(StringUtil.Format(RunspaceStrings.PipelineReInvokeNotAllowed, new object[0]), this.PipelineState, PipelineState.NotStarted);
					throw ex;
				}
				if (syncCall && !(this._inputStream is PSDataCollectionStream<PSObject>) && !(this._inputStream is PSDataCollectionStream<object>))
				{
					if (input != null)
					{
						foreach (object value in input)
						{
							this._inputStream.Write(value);
						}
					}
					this._inputStream.Close();
				}
				this._syncInvokeCall = syncCall;
				this._pipelineFinishedEvent = new ManualResetEvent(false);
				this.RunspaceBase.DoConcurrentCheckAndAddToRunningPipelines(this, syncCall);
				this.SetPipelineState(PipelineState.Running);
			}
			try
			{
				this.StartPipelineExecution();
			}
			catch (Exception ex2)
			{
				CommandProcessorBase.CheckForSevereException(ex2);
				this.RunspaceBase.RemoveFromRunningPipelineList(this);
				this.SetPipelineState(PipelineState.Failed, ex2);
				throw;
			}
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x00098B5C File Offset: 0x00096D5C
		internal override void InvokeAsyncAndDisconnect()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600195E RID: 6494
		protected abstract void StartPipelineExecution();

		// Token: 0x1700063E RID: 1598
		// (set) Token: 0x0600195F RID: 6495 RVA: 0x00098B63 File Offset: 0x00096D63
		internal bool PerformNestedCheck
		{
			set
			{
				this._performNestedCheck = value;
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06001960 RID: 6496 RVA: 0x00098B6C File Offset: 0x00096D6C
		// (set) Token: 0x06001961 RID: 6497 RVA: 0x00098B74 File Offset: 0x00096D74
		internal Thread NestedPipelineExecutionThread
		{
			get
			{
				return this._nestedPipelineExecutionThread;
			}
			set
			{
				this._nestedPipelineExecutionThread = value;
			}
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x00098B80 File Offset: 0x00096D80
		internal void DoConcurrentCheck(bool syncCall, object syncObject, bool isInLock)
		{
			PipelineBase pipelineBase = (PipelineBase)this.RunspaceBase.GetCurrentlyRunningPipeline();
			if (this.IsNested)
			{
				if (this._performNestedCheck)
				{
					if (!syncCall)
					{
						throw PSTraceSource.NewInvalidOperationException(RunspaceStrings.NestedPipelineInvokeAsync, new object[0]);
					}
					if (pipelineBase == null)
					{
						if (this.IsChild)
						{
							this.IsChild = false;
							this._isNested = false;
							return;
						}
						throw PSTraceSource.NewInvalidOperationException(RunspaceStrings.NestedPipelineNoParentPipeline, new object[0]);
					}
					else
					{
						Thread currentThread = Thread.CurrentThread;
						if (!pipelineBase.NestedPipelineExecutionThread.Equals(currentThread))
						{
							throw PSTraceSource.NewInvalidOperationException(RunspaceStrings.NestedPipelineNoParentPipeline, new object[0]);
						}
					}
				}
				return;
			}
			if (pipelineBase == null)
			{
				return;
			}
			if (pipelineBase == this.RunspaceBase.PulsePipeline || (pipelineBase.IsNested && this.RunspaceBase.PulsePipeline != null))
			{
				if (isInLock)
				{
					Monitor.Exit(syncObject);
				}
				try
				{
					this.RunspaceBase.WaitForFinishofPipelines();
				}
				finally
				{
					if (isInLock)
					{
						Monitor.Enter(syncObject);
					}
				}
				this.DoConcurrentCheck(syncCall, syncObject, isInLock);
				return;
			}
			throw PSTraceSource.NewInvalidOperationException(RunspaceStrings.ConcurrentInvokeNotAllowed, new object[0]);
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x00098C88 File Offset: 0x00096E88
		public override Collection<PSObject> Connect()
		{
			throw PSTraceSource.NewNotSupportedException(PipelineStrings.ConnectNotSupported, new object[0]);
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x00098C9A File Offset: 0x00096E9A
		public override void ConnectAsync()
		{
			throw PSTraceSource.NewNotSupportedException(PipelineStrings.ConnectNotSupported, new object[0]);
		}

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06001965 RID: 6501 RVA: 0x00098CAC File Offset: 0x00096EAC
		// (remove) Token: 0x06001966 RID: 6502 RVA: 0x00098CE4 File Offset: 0x00096EE4
		public override event EventHandler<PipelineStateEventArgs> StateChanged;

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06001967 RID: 6503 RVA: 0x00098D19 File Offset: 0x00096F19
		protected PipelineState PipelineState
		{
			get
			{
				return this._pipelineStateInfo.State;
			}
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x00098D26 File Offset: 0x00096F26
		protected bool IsPipelineFinished()
		{
			return this.PipelineState == PipelineState.Completed || this.PipelineState == PipelineState.Failed || this.PipelineState == PipelineState.Stopped;
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x00098D48 File Offset: 0x00096F48
		protected void SetPipelineState(PipelineState state, Exception reason)
		{
			lock (this.SyncRoot)
			{
				if (state != this.PipelineState)
				{
					this._pipelineStateInfo = new PipelineStateInfo(state, reason);
					RunspaceAvailability runspaceAvailability = this._runspace.RunspaceAvailability;
					this._runspace.UpdateRunspaceAvailability(this._pipelineStateInfo.State, false, null);
					this._executionEventQueue.Enqueue(new PipelineBase.ExecutionEventQueueItem(this._pipelineStateInfo.Clone(), runspaceAvailability, this._runspace.RunspaceAvailability));
				}
			}
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x00098DEC File Offset: 0x00096FEC
		protected void SetPipelineState(PipelineState state)
		{
			this.SetPipelineState(state, null);
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x00098DF8 File Offset: 0x00096FF8
		protected void RaisePipelineStateEvents()
		{
			Queue<PipelineBase.ExecutionEventQueueItem> queue = null;
			EventHandler<PipelineStateEventArgs> eventHandler = null;
			bool flag = false;
			lock (this.SyncRoot)
			{
				eventHandler = this.StateChanged;
				flag = this._runspace.HasAvailabilityChangedSubscribers;
				if (eventHandler != null || flag)
				{
					queue = this._executionEventQueue;
					this._executionEventQueue = new Queue<PipelineBase.ExecutionEventQueueItem>();
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
					PipelineBase.ExecutionEventQueueItem executionEventQueueItem = queue.Dequeue();
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

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x0600196C RID: 6508 RVA: 0x00098EDC File Offset: 0x000970DC
		internal ManualResetEvent PipelineFinishedEvent
		{
			get
			{
				return this._pipelineFinishedEvent;
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x0600196D RID: 6509 RVA: 0x00098EE4 File Offset: 0x000970E4
		protected ObjectStreamBase OutputStream
		{
			get
			{
				return this._outputStream;
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x0600196E RID: 6510 RVA: 0x00098EEC File Offset: 0x000970EC
		// (set) Token: 0x0600196F RID: 6511 RVA: 0x00098EF4 File Offset: 0x000970F4
		private protected ObjectStreamBase ErrorStream
		{
			protected get
			{
				return this._errorStream;
			}
			private set
			{
				this._errorStream = value;
				this._errorStream.DataReady += this.OnErrorStreamDataReady;
			}
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x00098F14 File Offset: 0x00097114
		private void OnErrorStreamDataReady(object sender, EventArgs e)
		{
			if (this._errorStream.Count > 0)
			{
				this._errorStream.DataReady -= this.OnErrorStreamDataReady;
				base.SetHadErrors(true);
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06001971 RID: 6513 RVA: 0x00098F42 File Offset: 0x00097142
		protected PSInformationalBuffers InformationalBuffers
		{
			get
			{
				return this._informationalBuffers;
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06001972 RID: 6514 RVA: 0x00098F4A File Offset: 0x0009714A
		protected ObjectStreamBase InputStream
		{
			get
			{
				return this._inputStream;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06001973 RID: 6515 RVA: 0x00098F52 File Offset: 0x00097152
		// (set) Token: 0x06001974 RID: 6516 RVA: 0x00098F5A File Offset: 0x0009715A
		internal bool AddToHistory
		{
			get
			{
				return this._addToHistory;
			}
			set
			{
				this._addToHistory = value;
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06001975 RID: 6517 RVA: 0x00098F63 File Offset: 0x00097163
		// (set) Token: 0x06001976 RID: 6518 RVA: 0x00098F6B File Offset: 0x0009716B
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

		// Token: 0x06001977 RID: 6519 RVA: 0x00098F74 File Offset: 0x00097174
		private void Initialize(Runspace runspace, string command, bool addToHistory, bool isNested)
		{
			this._runspace = runspace;
			this._isNested = isNested;
			if (addToHistory && command == null)
			{
				throw PSTraceSource.NewArgumentNullException("command");
			}
			if (command != null)
			{
				base.Commands.Add(new Command(command, true, false));
			}
			this._addToHistory = addToHistory;
			if (this._addToHistory)
			{
				this._historyString = command;
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06001978 RID: 6520 RVA: 0x00098FCD File Offset: 0x000971CD
		private RunspaceBase RunspaceBase
		{
			get
			{
				return (RunspaceBase)this.Runspace;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06001979 RID: 6521 RVA: 0x00098FDA File Offset: 0x000971DA
		protected internal object SyncRoot
		{
			get
			{
				return this._syncRoot;
			}
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x00098FE4 File Offset: 0x000971E4
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					this._disposed = true;
					if (disposing)
					{
						this._inputStream.Close();
						this._outputStream.Close();
						this._errorStream.DataReady -= this.OnErrorStreamDataReady;
						this._errorStream.Close();
						this._executionEventQueue.Clear();
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x04000A5D RID: 2653
		private Runspace _runspace;

		// Token: 0x04000A5E RID: 2654
		private bool _isNested;

		// Token: 0x04000A5F RID: 2655
		private bool _isPulsePipeline;

		// Token: 0x04000A60 RID: 2656
		private PipelineStateInfo _pipelineStateInfo = new PipelineStateInfo(PipelineState.NotStarted);

		// Token: 0x04000A61 RID: 2657
		private bool _syncInvokeCall;

		// Token: 0x04000A62 RID: 2658
		private bool _performNestedCheck = true;

		// Token: 0x04000A63 RID: 2659
		private Thread _nestedPipelineExecutionThread;

		// Token: 0x04000A65 RID: 2661
		private Queue<PipelineBase.ExecutionEventQueueItem> _executionEventQueue = new Queue<PipelineBase.ExecutionEventQueueItem>();

		// Token: 0x04000A66 RID: 2662
		private ManualResetEvent _pipelineFinishedEvent;

		// Token: 0x04000A67 RID: 2663
		private ObjectStreamBase _outputStream;

		// Token: 0x04000A68 RID: 2664
		private ObjectStreamBase _errorStream;

		// Token: 0x04000A69 RID: 2665
		private PSInformationalBuffers _informationalBuffers;

		// Token: 0x04000A6A RID: 2666
		private ObjectStreamBase _inputStream;

		// Token: 0x04000A6B RID: 2667
		private bool _addToHistory;

		// Token: 0x04000A6C RID: 2668
		private string _historyString;

		// Token: 0x04000A6D RID: 2669
		private object _syncRoot = new object();

		// Token: 0x04000A6E RID: 2670
		private static readonly string[] _emptyStringArray = new string[0];

		// Token: 0x04000A6F RID: 2671
		private bool _disposed;

		// Token: 0x0200021A RID: 538
		private class ExecutionEventQueueItem
		{
			// Token: 0x0600197C RID: 6524 RVA: 0x0009906D File Offset: 0x0009726D
			public ExecutionEventQueueItem(PipelineStateInfo pipelineStateInfo, RunspaceAvailability currentAvailability, RunspaceAvailability newAvailability)
			{
				this.PipelineStateInfo = pipelineStateInfo;
				this.CurrentRunspaceAvailability = currentAvailability;
				this.NewRunspaceAvailability = newAvailability;
			}

			// Token: 0x04000A71 RID: 2673
			public PipelineStateInfo PipelineStateInfo;

			// Token: 0x04000A72 RID: 2674
			public RunspaceAvailability CurrentRunspaceAvailability;

			// Token: 0x04000A73 RID: 2675
			public RunspaceAvailability NewRunspaceAvailability;
		}
	}
}
