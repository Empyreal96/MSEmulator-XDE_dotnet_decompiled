using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Sqm;
using System.Threading;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020001F8 RID: 504
	internal abstract class RunspaceBase : Runspace
	{
		// Token: 0x06001707 RID: 5895 RVA: 0x00090D3C File Offset: 0x0008EF3C
		protected RunspaceBase(PSHost host, RunspaceConfiguration runspaceConfiguration)
		{
			if (host == null)
			{
				throw PSTraceSource.NewArgumentNullException("host");
			}
			if (runspaceConfiguration == null)
			{
				throw PSTraceSource.NewArgumentNullException("runspaceConfiguration");
			}
			this._host = host;
			this._runspaceConfiguration = runspaceConfiguration;
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x00090DC8 File Offset: 0x0008EFC8
		protected RunspaceBase(PSHost host, InitialSessionState initialSessionState)
		{
			if (host == null)
			{
				throw PSTraceSource.NewArgumentNullException("host");
			}
			if (initialSessionState == null)
			{
				throw PSTraceSource.NewArgumentNullException("initialSessionState");
			}
			this._host = host;
			this._initialSessionState = initialSessionState.Clone();
			this.ThreadOptions = initialSessionState.ThreadOptions;
			base.ApartmentState = initialSessionState.ApartmentState;
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x00090E74 File Offset: 0x0008F074
		protected RunspaceBase(PSHost host, InitialSessionState initialSessionState, bool suppressClone)
		{
			if (host == null)
			{
				throw PSTraceSource.NewArgumentNullException("host");
			}
			if (initialSessionState == null)
			{
				throw PSTraceSource.NewArgumentNullException("initialSessionState");
			}
			this._host = host;
			if (suppressClone)
			{
				this._initialSessionState = initialSessionState;
			}
			else
			{
				this._initialSessionState = initialSessionState.Clone();
			}
			this.ThreadOptions = initialSessionState.ThreadOptions;
			base.ApartmentState = initialSessionState.ApartmentState;
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x0600170A RID: 5898 RVA: 0x00090F29 File Offset: 0x0008F129
		protected PSHost Host
		{
			get
			{
				return this._host;
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x0600170B RID: 5899 RVA: 0x00090F31 File Offset: 0x0008F131
		public override RunspaceConfiguration RunspaceConfiguration
		{
			get
			{
				return this._runspaceConfiguration;
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x0600170C RID: 5900 RVA: 0x00090F39 File Offset: 0x0008F139
		public override InitialSessionState InitialSessionState
		{
			get
			{
				return this._initialSessionState;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x0600170D RID: 5901 RVA: 0x00090F41 File Offset: 0x0008F141
		public override Version Version
		{
			get
			{
				return this._version;
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x0600170E RID: 5902 RVA: 0x00090F4C File Offset: 0x0008F14C
		public override RunspaceStateInfo RunspaceStateInfo
		{
			get
			{
				RunspaceStateInfo result;
				lock (this._syncRoot)
				{
					result = this._runspaceStateInfo.Clone();
				}
				return result;
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x0600170F RID: 5903 RVA: 0x00090F94 File Offset: 0x0008F194
		// (set) Token: 0x06001710 RID: 5904 RVA: 0x00090F9C File Offset: 0x0008F19C
		public override RunspaceAvailability RunspaceAvailability
		{
			get
			{
				return this._runspaceAvailability;
			}
			protected set
			{
				this._runspaceAvailability = value;
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06001711 RID: 5905 RVA: 0x00090FA5 File Offset: 0x0008F1A5
		protected internal object SyncRoot
		{
			get
			{
				return this._syncRoot;
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001712 RID: 5906 RVA: 0x00090FAD File Offset: 0x0008F1AD
		public override RunspaceConnectionInfo ConnectionInfo
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06001713 RID: 5907 RVA: 0x00090FB0 File Offset: 0x0008F1B0
		public override RunspaceConnectionInfo OriginalConnectionInfo
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x00090FB3 File Offset: 0x0008F1B3
		public override void Open()
		{
			this.CoreOpen(true);
		}

		// Token: 0x06001715 RID: 5909 RVA: 0x00090FBC File Offset: 0x0008F1BC
		public override void OpenAsync()
		{
			this.CoreOpen(false);
		}

		// Token: 0x06001716 RID: 5910 RVA: 0x00090FC8 File Offset: 0x0008F1C8
		private void CoreOpen(bool syncCall)
		{
			lock (this.SyncRoot)
			{
				if (this.RunspaceState != RunspaceState.BeforeOpen)
				{
					InvalidRunspaceStateException ex = new InvalidRunspaceStateException(StringUtil.Format(RunspaceStrings.CannotOpenAgain, new object[]
					{
						this.RunspaceState.ToString()
					}), this.RunspaceState, RunspaceState.BeforeOpen);
					throw ex;
				}
				this.SetRunspaceState(RunspaceState.Opening);
			}
			this.RaiseRunspaceStateEvents();
			PSSQMAPI.NoteRunspaceStart(base.InstanceId);
			this.OpenHelper(syncCall);
		}

		// Token: 0x06001717 RID: 5911
		protected abstract void OpenHelper(bool syncCall);

		// Token: 0x06001718 RID: 5912 RVA: 0x00091060 File Offset: 0x0008F260
		public override void Close()
		{
			this.CoreClose(true);
		}

		// Token: 0x06001719 RID: 5913 RVA: 0x00091069 File Offset: 0x0008F269
		public override void CloseAsync()
		{
			this.CoreClose(false);
		}

		// Token: 0x0600171A RID: 5914 RVA: 0x00091074 File Offset: 0x0008F274
		private void CoreClose(bool syncCall)
		{
			bool flag = false;
			lock (this.SyncRoot)
			{
				if (this.RunspaceState == RunspaceState.Closed || this.RunspaceState == RunspaceState.Broken)
				{
					return;
				}
				if (this.RunspaceState == RunspaceState.BeforeOpen)
				{
					this.SetRunspaceState(RunspaceState.Closing, null);
					this.SetRunspaceState(RunspaceState.Closed, null);
					this.RaiseRunspaceStateEvents();
					return;
				}
				if (this.RunspaceState == RunspaceState.Opening)
				{
					Monitor.Exit(this.SyncRoot);
					try
					{
						this.RunspaceOpening.Wait();
					}
					finally
					{
						Monitor.Enter(this.SyncRoot);
					}
				}
				if (this._bSessionStateProxyCallInProgress)
				{
					throw PSTraceSource.NewInvalidOperationException(RunspaceStrings.RunspaceCloseInvalidWhileSessionStateProxy, new object[0]);
				}
				if (this.RunspaceState == RunspaceState.Closing)
				{
					flag = true;
				}
				else
				{
					if (this.RunspaceState != RunspaceState.Opened)
					{
						InvalidRunspaceStateException ex = new InvalidRunspaceStateException(StringUtil.Format(RunspaceStrings.RunspaceNotInOpenedState, this.RunspaceState.ToString()), this.RunspaceState, RunspaceState.Opened);
						throw ex;
					}
					this.SetRunspaceState(RunspaceState.Closing);
				}
			}
			if (!flag)
			{
				this.RaiseRunspaceStateEvents();
				PSSQMAPI.NoteRunspaceEnd(base.InstanceId);
				this.CloseHelper(syncCall);
				return;
			}
			if (syncCall)
			{
				this.WaitForFinishofPipelines();
			}
		}

		// Token: 0x0600171B RID: 5915
		protected abstract void CloseHelper(bool syncCall);

		// Token: 0x0600171C RID: 5916 RVA: 0x000911A8 File Offset: 0x0008F3A8
		public override void Disconnect()
		{
			throw new InvalidRunspaceStateException(RunspaceStrings.DisconnectNotSupported);
		}

		// Token: 0x0600171D RID: 5917 RVA: 0x000911B4 File Offset: 0x0008F3B4
		public override void DisconnectAsync()
		{
			throw new InvalidRunspaceStateException(RunspaceStrings.DisconnectNotSupported);
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x000911C0 File Offset: 0x0008F3C0
		public override void Connect()
		{
			throw new InvalidRunspaceStateException(RunspaceStrings.ConnectNotSupported);
		}

		// Token: 0x0600171F RID: 5919 RVA: 0x000911CC File Offset: 0x0008F3CC
		public override void ConnectAsync()
		{
			throw new InvalidRunspaceStateException(RunspaceStrings.ConnectNotSupported);
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x000911D8 File Offset: 0x0008F3D8
		public override Pipeline CreateDisconnectedPipeline()
		{
			throw new InvalidRunspaceStateException(RunspaceStrings.DisconnectConnectNotSupported);
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x000911E4 File Offset: 0x0008F3E4
		public override PowerShell CreateDisconnectedPowerShell()
		{
			throw new InvalidRunspaceStateException(RunspaceStrings.DisconnectConnectNotSupported);
		}

		// Token: 0x06001722 RID: 5922 RVA: 0x000911F0 File Offset: 0x0008F3F0
		public override RunspaceCapability GetCapabilities()
		{
			return RunspaceCapability.Default;
		}

		// Token: 0x06001723 RID: 5923 RVA: 0x000911F3 File Offset: 0x0008F3F3
		public override Pipeline CreatePipeline()
		{
			return this.CoreCreatePipeline(null, false, false);
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x000911FE File Offset: 0x0008F3FE
		public override Pipeline CreatePipeline(string command)
		{
			if (command == null)
			{
				throw PSTraceSource.NewArgumentNullException("command");
			}
			return this.CoreCreatePipeline(command, false, false);
		}

		// Token: 0x06001725 RID: 5925 RVA: 0x00091217 File Offset: 0x0008F417
		public override Pipeline CreatePipeline(string command, bool addToHistory)
		{
			if (command == null)
			{
				throw PSTraceSource.NewArgumentNullException("command");
			}
			return this.CoreCreatePipeline(command, addToHistory, false);
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x00091230 File Offset: 0x0008F430
		public override Pipeline CreateNestedPipeline()
		{
			return this.CoreCreatePipeline(null, false, true);
		}

		// Token: 0x06001727 RID: 5927 RVA: 0x0009123B File Offset: 0x0008F43B
		public override Pipeline CreateNestedPipeline(string command, bool addToHistory)
		{
			if (command == null)
			{
				throw PSTraceSource.NewArgumentNullException("command");
			}
			return this.CoreCreatePipeline(command, addToHistory, true);
		}

		// Token: 0x06001728 RID: 5928
		protected abstract Pipeline CoreCreatePipeline(string command, bool addToHistory, bool isNested);

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06001729 RID: 5929 RVA: 0x00091254 File Offset: 0x0008F454
		// (remove) Token: 0x0600172A RID: 5930 RVA: 0x0009128C File Offset: 0x0008F48C
		public override event EventHandler<RunspaceStateEventArgs> StateChanged;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x0600172B RID: 5931 RVA: 0x000912C4 File Offset: 0x0008F4C4
		// (remove) Token: 0x0600172C RID: 5932 RVA: 0x000912FC File Offset: 0x0008F4FC
		public override event EventHandler<RunspaceAvailabilityEventArgs> AvailabilityChanged;

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x0600172D RID: 5933 RVA: 0x00091331 File Offset: 0x0008F531
		internal override bool HasAvailabilityChangedSubscribers
		{
			get
			{
				return this.AvailabilityChanged != null;
			}
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x00091340 File Offset: 0x0008F540
		protected override void OnAvailabilityChanged(RunspaceAvailabilityEventArgs e)
		{
			EventHandler<RunspaceAvailabilityEventArgs> availabilityChanged = this.AvailabilityChanged;
			if (availabilityChanged != null)
			{
				try
				{
					availabilityChanged(this, e);
				}
				catch (Exception e2)
				{
					CommandProcessorBase.CheckForSevereException(e2);
				}
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x0600172F RID: 5935 RVA: 0x0009137C File Offset: 0x0008F57C
		protected RunspaceState RunspaceState
		{
			get
			{
				return this._runspaceStateInfo.State;
			}
		}

		// Token: 0x06001730 RID: 5936 RVA: 0x0009138C File Offset: 0x0008F58C
		protected void SetRunspaceState(RunspaceState state, Exception reason)
		{
			lock (this.SyncRoot)
			{
				if (state != this.RunspaceState)
				{
					this._runspaceStateInfo = new RunspaceStateInfo(state, reason);
					RunspaceAvailability runspaceAvailability = this._runspaceAvailability;
					base.UpdateRunspaceAvailability(this._runspaceStateInfo.State, false);
					this._runspaceEventQueue.Enqueue(new RunspaceBase.RunspaceEventQueueItem(this._runspaceStateInfo.Clone(), runspaceAvailability, this._runspaceAvailability));
				}
			}
		}

		// Token: 0x06001731 RID: 5937 RVA: 0x00091418 File Offset: 0x0008F618
		protected void SetRunspaceState(RunspaceState state)
		{
			this.SetRunspaceState(state, null);
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x00091424 File Offset: 0x0008F624
		protected void RaiseRunspaceStateEvents()
		{
			Queue<RunspaceBase.RunspaceEventQueueItem> queue = null;
			EventHandler<RunspaceStateEventArgs> eventHandler = null;
			bool flag = false;
			lock (this.SyncRoot)
			{
				eventHandler = this.StateChanged;
				flag = this.HasAvailabilityChangedSubscribers;
				if (eventHandler != null || flag)
				{
					queue = this._runspaceEventQueue;
					this._runspaceEventQueue = new Queue<RunspaceBase.RunspaceEventQueueItem>();
				}
				else
				{
					this._runspaceEventQueue.Clear();
				}
			}
			if (queue != null)
			{
				while (queue.Count > 0)
				{
					RunspaceBase.RunspaceEventQueueItem runspaceEventQueueItem = queue.Dequeue();
					if (flag && runspaceEventQueueItem.NewRunspaceAvailability != runspaceEventQueueItem.CurrentRunspaceAvailability)
					{
						this.OnAvailabilityChanged(new RunspaceAvailabilityEventArgs(runspaceEventQueueItem.NewRunspaceAvailability));
					}
					if (eventHandler != null)
					{
						try
						{
							eventHandler(this, new RunspaceStateEventArgs(runspaceEventQueueItem.RunspaceStateInfo));
						}
						catch (Exception e)
						{
							CommandProcessorBase.CheckForSevereException(e);
						}
					}
				}
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001733 RID: 5939 RVA: 0x00091500 File Offset: 0x0008F700
		// (set) Token: 0x06001734 RID: 5940 RVA: 0x00091508 File Offset: 0x0008F708
		protected bool ByPassRunspaceStateCheck
		{
			get
			{
				return this._bypassRunspaceStateCheck;
			}
			set
			{
				this._bypassRunspaceStateCheck = value;
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001735 RID: 5941 RVA: 0x00091511 File Offset: 0x0008F711
		protected List<Pipeline> RunningPipelines
		{
			get
			{
				return this._runningPipelines;
			}
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x0009151C File Offset: 0x0008F71C
		internal void AddToRunningPipelineList(PipelineBase pipeline)
		{
			lock (this._pipelineListLock)
			{
				if (!this._bypassRunspaceStateCheck && this.RunspaceState != RunspaceState.Opened)
				{
					InvalidRunspaceStateException ex = new InvalidRunspaceStateException(StringUtil.Format(RunspaceStrings.RunspaceNotOpenForPipeline, this.RunspaceState.ToString()), this.RunspaceState, RunspaceState.Opened);
					throw ex;
				}
				this._runningPipelines.Add(pipeline);
				this.currentlyRunningPipeline = pipeline;
			}
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x000915A4 File Offset: 0x0008F7A4
		internal void RemoveFromRunningPipelineList(PipelineBase pipeline)
		{
			lock (this._pipelineListLock)
			{
				this._runningPipelines.Remove(pipeline);
				if (this._runningPipelines.Count == 0)
				{
					this.currentlyRunningPipeline = null;
				}
				else
				{
					this.currentlyRunningPipeline = this._runningPipelines[this._runningPipelines.Count - 1];
				}
				pipeline.PipelineFinishedEvent.Set();
			}
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x00091658 File Offset: 0x0008F858
		internal bool WaitForFinishofPipelines()
		{
			PipelineBase[] array;
			lock (this._pipelineListLock)
			{
				array = this.RunningPipelines.Cast<PipelineBase>().ToArray<PipelineBase>();
			}
			if (array.Length > 0)
			{
				WaitHandle[] array2 = new WaitHandle[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = array[i].PipelineFinishedEvent;
				}
				if (array.Length > 1 && Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
				{
					using (ManualResetEvent manualResetEvent = new ManualResetEvent(false))
					{
						Tuple<WaitHandle[], ManualResetEvent> state2 = new Tuple<WaitHandle[], ManualResetEvent>(array2, manualResetEvent);
						ThreadPool.QueueUserWorkItem(delegate(object state)
						{
							Tuple<WaitHandle[], ManualResetEvent> tuple = (Tuple<WaitHandle[], ManualResetEvent>)state;
							WaitHandle.WaitAll(tuple.Item1);
							tuple.Item2.Set();
						}, state2);
						return manualResetEvent.WaitOne();
					}
				}
				return WaitHandle.WaitAll(array2);
			}
			return true;
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x00091744 File Offset: 0x0008F944
		protected void StopPipelines()
		{
			PipelineBase[] array;
			lock (this._pipelineListLock)
			{
				array = this.RunningPipelines.Cast<PipelineBase>().ToArray<PipelineBase>();
			}
			if (array.Length > 0)
			{
				for (int i = array.Length - 1; i >= 0; i--)
				{
					array[i].Stop();
				}
			}
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x000917B0 File Offset: 0x0008F9B0
		internal bool RunActionIfNoRunningPipelinesWithThreadCheck(Action action)
		{
			bool result = false;
			bool flag = false;
			lock (this._pipelineListLock)
			{
				PipelineBase pipelineBase = this.currentlyRunningPipeline as PipelineBase;
				if (pipelineBase == null || Thread.CurrentThread.Equals(pipelineBase.NestedPipelineExecutionThread))
				{
					flag = true;
				}
			}
			if (flag)
			{
				action();
				result = true;
			}
			return result;
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x00091820 File Offset: 0x0008FA20
		internal override Pipeline GetCurrentlyRunningPipeline()
		{
			return this.currentlyRunningPipeline;
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x00091828 File Offset: 0x0008FA28
		internal void StopNestedPipelines(Pipeline pipeline)
		{
			List<Pipeline> list = null;
			lock (this._pipelineListLock)
			{
				if (!this._runningPipelines.Contains(pipeline))
				{
					return;
				}
				if (this.GetCurrentlyRunningPipeline() == pipeline)
				{
					return;
				}
				list = new List<Pipeline>();
				int num = this._runningPipelines.Count - 1;
				while (num >= 0 && this._runningPipelines[num] != pipeline)
				{
					list.Add(this._runningPipelines[num]);
					num--;
				}
			}
			foreach (Pipeline pipeline2 in list)
			{
				try
				{
					pipeline2.Stop();
				}
				catch (InvalidPipelineStateException)
				{
				}
			}
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x00091914 File Offset: 0x0008FB14
		internal void DoConcurrentCheckAndAddToRunningPipelines(PipelineBase pipeline, bool syncCall)
		{
			lock (this._syncRoot)
			{
				if (this._bSessionStateProxyCallInProgress)
				{
					throw PSTraceSource.NewInvalidOperationException(RunspaceStrings.NoPipelineWhenSessionStateProxyInProgress, new object[0]);
				}
				pipeline.DoConcurrentCheck(syncCall, this._syncRoot, true);
				this.AddToRunningPipelineList(pipeline);
			}
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x0009197C File Offset: 0x0008FB7C
		internal void Pulse()
		{
			bool flag = false;
			if (this.GetCurrentlyRunningPipeline() == null)
			{
				lock (this.SyncRoot)
				{
					if (this.GetCurrentlyRunningPipeline() == null)
					{
						try
						{
							this.pulsePipeline = (PipelineBase)this.CreatePipeline("0");
							this.pulsePipeline.IsPulsePipeline = true;
							flag = true;
						}
						catch (ObjectDisposedException)
						{
						}
					}
				}
			}
			if (flag)
			{
				try
				{
					this.pulsePipeline.Invoke();
				}
				catch (PSInvalidOperationException)
				{
				}
				catch (InvalidRunspaceStateException)
				{
				}
				catch (ObjectDisposedException)
				{
				}
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x0600173F RID: 5951 RVA: 0x00091A3C File Offset: 0x0008FC3C
		internal PipelineBase PulsePipeline
		{
			get
			{
				return this.pulsePipeline;
			}
		}

		// Token: 0x06001740 RID: 5952 RVA: 0x00091A44 File Offset: 0x0008FC44
		private void DoConcurrentCheckAndMarkSessionStateProxyCallInProgress()
		{
			lock (this._syncRoot)
			{
				if (this.RunspaceState != RunspaceState.Opened)
				{
					InvalidRunspaceStateException ex = new InvalidRunspaceStateException(StringUtil.Format(RunspaceStrings.RunspaceNotInOpenedState, this.RunspaceState.ToString()), this.RunspaceState, RunspaceState.Opened);
					throw ex;
				}
				if (this._bSessionStateProxyCallInProgress)
				{
					throw PSTraceSource.NewInvalidOperationException(RunspaceStrings.AnotherSessionStateProxyInProgress, new object[0]);
				}
				Pipeline pipeline = this.GetCurrentlyRunningPipeline();
				if (pipeline != null)
				{
					if (pipeline != this.pulsePipeline && (!pipeline.IsNested || this.pulsePipeline == null))
					{
						throw PSTraceSource.NewInvalidOperationException(RunspaceStrings.NoSessionStateProxyWhenPipelineInProgress, new object[0]);
					}
					Monitor.Exit(this._syncRoot);
					try
					{
						this.WaitForFinishofPipelines();
					}
					finally
					{
						Monitor.Enter(this._syncRoot);
					}
					this.DoConcurrentCheckAndMarkSessionStateProxyCallInProgress();
				}
				else
				{
					this._bSessionStateProxyCallInProgress = true;
				}
			}
		}

		// Token: 0x06001741 RID: 5953 RVA: 0x00091B38 File Offset: 0x0008FD38
		internal void SetVariable(string name, object value)
		{
			this.DoConcurrentCheckAndMarkSessionStateProxyCallInProgress();
			try
			{
				this.DoSetVariable(name, value);
			}
			finally
			{
				lock (this.SyncRoot)
				{
					this._bSessionStateProxyCallInProgress = false;
				}
			}
		}

		// Token: 0x06001742 RID: 5954 RVA: 0x00091B98 File Offset: 0x0008FD98
		internal object GetVariable(string name)
		{
			this.DoConcurrentCheckAndMarkSessionStateProxyCallInProgress();
			object result;
			try
			{
				result = this.DoGetVariable(name);
			}
			finally
			{
				lock (this._syncRoot)
				{
					this._bSessionStateProxyCallInProgress = false;
				}
			}
			return result;
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06001743 RID: 5955 RVA: 0x00091BF8 File Offset: 0x0008FDF8
		internal List<string> Applications
		{
			get
			{
				this.DoConcurrentCheckAndMarkSessionStateProxyCallInProgress();
				List<string> doApplications;
				try
				{
					doApplications = this.DoApplications;
				}
				finally
				{
					lock (this._syncRoot)
					{
						this._bSessionStateProxyCallInProgress = false;
					}
				}
				return doApplications;
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06001744 RID: 5956 RVA: 0x00091C58 File Offset: 0x0008FE58
		internal List<string> Scripts
		{
			get
			{
				this.DoConcurrentCheckAndMarkSessionStateProxyCallInProgress();
				List<string> doScripts;
				try
				{
					doScripts = this.DoScripts;
				}
				finally
				{
					lock (this._syncRoot)
					{
						this._bSessionStateProxyCallInProgress = false;
					}
				}
				return doScripts;
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001745 RID: 5957 RVA: 0x00091CB8 File Offset: 0x0008FEB8
		internal DriveManagementIntrinsics Drive
		{
			get
			{
				this.DoConcurrentCheckAndMarkSessionStateProxyCallInProgress();
				DriveManagementIntrinsics doDrive;
				try
				{
					doDrive = this.DoDrive;
				}
				finally
				{
					lock (this._syncRoot)
					{
						this._bSessionStateProxyCallInProgress = false;
					}
				}
				return doDrive;
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06001746 RID: 5958 RVA: 0x00091D18 File Offset: 0x0008FF18
		// (set) Token: 0x06001747 RID: 5959 RVA: 0x00091D60 File Offset: 0x0008FF60
		public PSLanguageMode LanguageMode
		{
			get
			{
				if (this.RunspaceState != RunspaceState.Opened)
				{
					InvalidRunspaceStateException ex = new InvalidRunspaceStateException(StringUtil.Format(RunspaceStrings.RunspaceNotInOpenedState, this.RunspaceState.ToString()), this.RunspaceState, RunspaceState.Opened);
					throw ex;
				}
				return this.DoLanguageMode;
			}
			set
			{
				if (this.RunspaceState != RunspaceState.Opened)
				{
					InvalidRunspaceStateException ex = new InvalidRunspaceStateException(StringUtil.Format(RunspaceStrings.RunspaceNotInOpenedState, this.RunspaceState.ToString()), this.RunspaceState, RunspaceState.Opened);
					throw ex;
				}
				this.DoLanguageMode = value;
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001748 RID: 5960 RVA: 0x00091DA8 File Offset: 0x0008FFA8
		internal PSModuleInfo Module
		{
			get
			{
				this.DoConcurrentCheckAndMarkSessionStateProxyCallInProgress();
				PSModuleInfo doModule;
				try
				{
					doModule = this.DoModule;
				}
				finally
				{
					lock (this._syncRoot)
					{
						this._bSessionStateProxyCallInProgress = false;
					}
				}
				return doModule;
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001749 RID: 5961 RVA: 0x00091E08 File Offset: 0x00090008
		internal PathIntrinsics PathIntrinsics
		{
			get
			{
				this.DoConcurrentCheckAndMarkSessionStateProxyCallInProgress();
				PathIntrinsics doPath;
				try
				{
					doPath = this.DoPath;
				}
				finally
				{
					lock (this._syncRoot)
					{
						this._bSessionStateProxyCallInProgress = false;
					}
				}
				return doPath;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x0600174A RID: 5962 RVA: 0x00091E68 File Offset: 0x00090068
		internal CmdletProviderManagementIntrinsics Provider
		{
			get
			{
				this.DoConcurrentCheckAndMarkSessionStateProxyCallInProgress();
				CmdletProviderManagementIntrinsics doProvider;
				try
				{
					doProvider = this.DoProvider;
				}
				finally
				{
					lock (this._syncRoot)
					{
						this._bSessionStateProxyCallInProgress = false;
					}
				}
				return doProvider;
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x0600174B RID: 5963 RVA: 0x00091EC8 File Offset: 0x000900C8
		internal PSVariableIntrinsics PSVariable
		{
			get
			{
				this.DoConcurrentCheckAndMarkSessionStateProxyCallInProgress();
				PSVariableIntrinsics doPSVariable;
				try
				{
					doPSVariable = this.DoPSVariable;
				}
				finally
				{
					lock (this._syncRoot)
					{
						this._bSessionStateProxyCallInProgress = false;
					}
				}
				return doPSVariable;
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x0600174C RID: 5964 RVA: 0x00091F28 File Offset: 0x00090128
		internal CommandInvocationIntrinsics InvokeCommand
		{
			get
			{
				this.DoConcurrentCheckAndMarkSessionStateProxyCallInProgress();
				CommandInvocationIntrinsics doInvokeCommand;
				try
				{
					doInvokeCommand = this.DoInvokeCommand;
				}
				finally
				{
					lock (this._syncRoot)
					{
						this._bSessionStateProxyCallInProgress = false;
					}
				}
				return doInvokeCommand;
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x0600174D RID: 5965 RVA: 0x00091F88 File Offset: 0x00090188
		internal ProviderIntrinsics InvokeProvider
		{
			get
			{
				this.DoConcurrentCheckAndMarkSessionStateProxyCallInProgress();
				ProviderIntrinsics doInvokeProvider;
				try
				{
					doInvokeProvider = this.DoInvokeProvider;
				}
				finally
				{
					lock (this._syncRoot)
					{
						this._bSessionStateProxyCallInProgress = false;
					}
				}
				return doInvokeProvider;
			}
		}

		// Token: 0x0600174E RID: 5966
		protected abstract void DoSetVariable(string name, object value);

		// Token: 0x0600174F RID: 5967
		protected abstract object DoGetVariable(string name);

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06001750 RID: 5968
		protected abstract List<string> DoApplications { get; }

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001751 RID: 5969
		protected abstract List<string> DoScripts { get; }

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001752 RID: 5970
		protected abstract DriveManagementIntrinsics DoDrive { get; }

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001753 RID: 5971
		// (set) Token: 0x06001754 RID: 5972
		protected abstract PSLanguageMode DoLanguageMode { get; set; }

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001755 RID: 5973
		protected abstract PSModuleInfo DoModule { get; }

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001756 RID: 5974
		protected abstract PathIntrinsics DoPath { get; }

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001757 RID: 5975
		protected abstract CmdletProviderManagementIntrinsics DoProvider { get; }

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001758 RID: 5976
		protected abstract PSVariableIntrinsics DoPSVariable { get; }

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001759 RID: 5977
		protected abstract CommandInvocationIntrinsics DoInvokeCommand { get; }

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x0600175A RID: 5978
		protected abstract ProviderIntrinsics DoInvokeProvider { get; }

		// Token: 0x0600175B RID: 5979 RVA: 0x00091FE8 File Offset: 0x000901E8
		internal override SessionStateProxy GetSessionStateProxy()
		{
			if (this._sessionStateProxy == null)
			{
				this._sessionStateProxy = new SessionStateProxy(this);
			}
			return this._sessionStateProxy;
		}

		// Token: 0x040009D6 RID: 2518
		private PSHost _host;

		// Token: 0x040009D7 RID: 2519
		private RunspaceConfiguration _runspaceConfiguration;

		// Token: 0x040009D8 RID: 2520
		private InitialSessionState _initialSessionState;

		// Token: 0x040009D9 RID: 2521
		private Version _version = PSVersionInfo.PSVersion;

		// Token: 0x040009DA RID: 2522
		private RunspaceStateInfo _runspaceStateInfo = new RunspaceStateInfo(RunspaceState.BeforeOpen);

		// Token: 0x040009DB RID: 2523
		private RunspaceAvailability _runspaceAvailability;

		// Token: 0x040009DC RID: 2524
		private object _syncRoot = new object();

		// Token: 0x040009DF RID: 2527
		private Queue<RunspaceBase.RunspaceEventQueueItem> _runspaceEventQueue = new Queue<RunspaceBase.RunspaceEventQueueItem>();

		// Token: 0x040009E0 RID: 2528
		internal ManualResetEventSlim RunspaceOpening = new ManualResetEventSlim(false);

		// Token: 0x040009E1 RID: 2529
		private bool _bypassRunspaceStateCheck;

		// Token: 0x040009E2 RID: 2530
		private readonly List<Pipeline> _runningPipelines = new List<Pipeline>();

		// Token: 0x040009E3 RID: 2531
		private readonly object _pipelineListLock = new object();

		// Token: 0x040009E4 RID: 2532
		private Pipeline currentlyRunningPipeline;

		// Token: 0x040009E5 RID: 2533
		private PipelineBase pulsePipeline;

		// Token: 0x040009E6 RID: 2534
		private bool _bSessionStateProxyCallInProgress;

		// Token: 0x040009E7 RID: 2535
		private SessionStateProxy _sessionStateProxy;

		// Token: 0x020001F9 RID: 505
		private class RunspaceEventQueueItem
		{
			// Token: 0x0600175D RID: 5981 RVA: 0x00092004 File Offset: 0x00090204
			public RunspaceEventQueueItem(RunspaceStateInfo runspaceStateInfo, RunspaceAvailability currentAvailability, RunspaceAvailability newAvailability)
			{
				this.RunspaceStateInfo = runspaceStateInfo;
				this.CurrentRunspaceAvailability = currentAvailability;
				this.NewRunspaceAvailability = newAvailability;
			}

			// Token: 0x040009E9 RID: 2537
			public RunspaceStateInfo RunspaceStateInfo;

			// Token: 0x040009EA RID: 2538
			public RunspaceAvailability CurrentRunspaceAvailability;

			// Token: 0x040009EB RID: 2539
			public RunspaceAvailability NewRunspaceAvailability;
		}
	}
}
