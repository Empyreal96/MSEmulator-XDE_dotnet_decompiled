using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces.Internal;
using System.Threading;
using System.Transactions;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020001F6 RID: 502
	public abstract class Runspace : IDisposable
	{
		// Token: 0x0600169F RID: 5791 RVA: 0x0009025C File Offset: 0x0008E45C
		internal Runspace()
		{
			this.Id = Interlocked.Increment(ref Runspace._globalId);
			this.Name = "Runspace" + this.Id.ToString(NumberFormatInfo.InvariantInfo);
			this._runningPowerShells = new Stack<PowerShell>();
			this._syncObject = new object();
			lock (Runspace.s_syncObject)
			{
				Runspace.s_runspaceDictionary.Add(this.Id, new WeakReference<Runspace>(this));
			}
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x00090318 File Offset: 0x0008E518
		static Runspace()
		{
			Runspace.s_runspaceDictionary = new SortedDictionary<int, WeakReference<Runspace>>();
			Runspace._globalId = 0;
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x060016A1 RID: 5793 RVA: 0x0009033A File Offset: 0x0008E53A
		// (set) Token: 0x060016A2 RID: 5794 RVA: 0x00090341 File Offset: 0x0008E541
		public static Runspace DefaultRunspace
		{
			get
			{
				return Runspace.ThreadSpecificDefaultRunspace;
			}
			set
			{
				if (value == null || !value.RunspaceIsRemote)
				{
					Runspace.ThreadSpecificDefaultRunspace = value;
					return;
				}
				throw new InvalidOperationException(RunspaceStrings.RunspaceNotLocal);
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x060016A3 RID: 5795 RVA: 0x00090360 File Offset: 0x0008E560
		public static bool CanUseDefaultRunspace
		{
			get
			{
				RunspaceBase runspaceBase = Runspace.DefaultRunspace as RunspaceBase;
				if (runspaceBase != null)
				{
					Pipeline currentlyRunningPipeline = runspaceBase.GetCurrentlyRunningPipeline();
					LocalPipeline localPipeline = currentlyRunningPipeline as LocalPipeline;
					if (localPipeline != null && localPipeline.NestedPipelineExecutionThread != null)
					{
						return localPipeline.NestedPipelineExecutionThread.ManagedThreadId == Thread.CurrentThread.ManagedThreadId;
					}
				}
				return false;
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x060016A4 RID: 5796 RVA: 0x000903AD File Offset: 0x0008E5AD
		// (set) Token: 0x060016A5 RID: 5797 RVA: 0x000903B5 File Offset: 0x0008E5B5
		public ApartmentState ApartmentState
		{
			get
			{
				return this.apartmentState;
			}
			set
			{
				if (this.RunspaceStateInfo.State != RunspaceState.BeforeOpen)
				{
					throw new InvalidRunspaceStateException(StringUtil.Format(RunspaceStrings.ChangePropertyAfterOpen, new object[0]));
				}
				this.apartmentState = value;
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x060016A6 RID: 5798
		// (set) Token: 0x060016A7 RID: 5799
		public abstract PSThreadOptions ThreadOptions { get; set; }

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x060016A8 RID: 5800
		public abstract Version Version { get; }

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x060016A9 RID: 5801 RVA: 0x000903E1 File Offset: 0x0008E5E1
		public bool RunspaceIsRemote
		{
			get
			{
				return !(this is LocalRunspace) && this.ConnectionInfo != null;
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x060016AA RID: 5802
		public abstract RunspaceStateInfo RunspaceStateInfo { get; }

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x060016AB RID: 5803
		// (set) Token: 0x060016AC RID: 5804
		public abstract RunspaceAvailability RunspaceAvailability { get; protected set; }

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x060016AD RID: 5805
		public abstract RunspaceConfiguration RunspaceConfiguration { get; }

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x060016AE RID: 5806
		public abstract InitialSessionState InitialSessionState { get; }

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x060016AF RID: 5807 RVA: 0x000903F9 File Offset: 0x0008E5F9
		// (set) Token: 0x060016B0 RID: 5808 RVA: 0x00090401 File Offset: 0x0008E601
		public Guid InstanceId
		{
			get
			{
				return this._instanceId;
			}
			internal set
			{
				this._instanceId = value;
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x060016B1 RID: 5809 RVA: 0x0009040A File Offset: 0x0008E60A
		internal ExecutionContext ExecutionContext
		{
			get
			{
				return this.GetExecutionContext;
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x060016B2 RID: 5810 RVA: 0x00090412 File Offset: 0x0008E612
		// (set) Token: 0x060016B3 RID: 5811 RVA: 0x0009041A File Offset: 0x0008E61A
		internal bool SkipUserProfile
		{
			get
			{
				return this._skipUserProfile;
			}
			set
			{
				this._skipUserProfile = value;
			}
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x060016B4 RID: 5812
		public abstract RunspaceConnectionInfo ConnectionInfo { get; }

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x060016B5 RID: 5813
		public abstract RunspaceConnectionInfo OriginalConnectionInfo { get; }

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x060016B6 RID: 5814
		public abstract JobManager JobManager { get; }

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x060016B7 RID: 5815 RVA: 0x00090423 File Offset: 0x0008E623
		// (set) Token: 0x060016B8 RID: 5816 RVA: 0x0009042B File Offset: 0x0008E62B
		public DateTime? DisconnectedOn { get; internal set; }

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x060016B9 RID: 5817 RVA: 0x00090434 File Offset: 0x0008E634
		// (set) Token: 0x060016BA RID: 5818 RVA: 0x0009043C File Offset: 0x0008E63C
		public DateTime? ExpiresOn { get; internal set; }

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x060016BB RID: 5819 RVA: 0x00090445 File Offset: 0x0008E645
		// (set) Token: 0x060016BC RID: 5820 RVA: 0x0009044D File Offset: 0x0008E64D
		public string Name { get; set; }

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x060016BD RID: 5821 RVA: 0x00090456 File Offset: 0x0008E656
		// (set) Token: 0x060016BE RID: 5822 RVA: 0x0009045E File Offset: 0x0008E65E
		public int Id { get; private set; }

		// Token: 0x060016BF RID: 5823 RVA: 0x00090468 File Offset: 0x0008E668
		internal Version GetRemoteProtocolVersion()
		{
			Version result;
			bool flag = PSPrimitiveDictionary.TryPathGet<Version>(this.GetApplicationPrivateData(), out result, new string[]
			{
				"PSVersionTable",
				"PSRemotingProtocolVersion"
			});
			if (flag)
			{
				return result;
			}
			return RemotingConstants.ProtocolVersion;
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x060016C0 RID: 5824 RVA: 0x000904A5 File Offset: 0x0008E6A5
		// (set) Token: 0x060016C1 RID: 5825 RVA: 0x000904AD File Offset: 0x0008E6AD
		internal Guid EngineActivityId
		{
			get
			{
				return this._engineActivityId;
			}
			set
			{
				this._engineActivityId = value;
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x060016C2 RID: 5826 RVA: 0x000904B8 File Offset: 0x0008E6B8
		internal static ReadOnlyDictionary<int, WeakReference<Runspace>> RunspaceDictionary
		{
			get
			{
				ReadOnlyDictionary<int, WeakReference<Runspace>> result;
				lock (Runspace.s_syncObject)
				{
					result = new ReadOnlyDictionary<int, WeakReference<Runspace>>(new Dictionary<int, WeakReference<Runspace>>(Runspace.s_runspaceDictionary));
				}
				return result;
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x060016C3 RID: 5827 RVA: 0x00090504 File Offset: 0x0008E704
		internal static IReadOnlyList<Runspace> RunspaceList
		{
			get
			{
				List<Runspace> list = new List<Runspace>();
				lock (Runspace.s_syncObject)
				{
					foreach (WeakReference<Runspace> weakReference in Runspace.s_runspaceDictionary.Values)
					{
						Runspace item;
						if (weakReference.TryGetTarget(out item))
						{
							list.Add(item);
						}
					}
				}
				return new ReadOnlyCollection<Runspace>(list);
			}
		}

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x060016C4 RID: 5828
		// (remove) Token: 0x060016C5 RID: 5829
		public abstract event EventHandler<RunspaceStateEventArgs> StateChanged;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x060016C6 RID: 5830
		// (remove) Token: 0x060016C7 RID: 5831
		public abstract event EventHandler<RunspaceAvailabilityEventArgs> AvailabilityChanged;

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x060016C8 RID: 5832
		internal abstract bool HasAvailabilityChangedSubscribers { get; }

		// Token: 0x060016C9 RID: 5833
		protected abstract void OnAvailabilityChanged(RunspaceAvailabilityEventArgs e);

		// Token: 0x060016CA RID: 5834 RVA: 0x0009059C File Offset: 0x0008E79C
		internal void UpdateRunspaceAvailability(PipelineState pipelineState, bool raiseEvent, Guid? cmdInstanceId = null)
		{
			RunspaceAvailability runspaceAvailability = this.RunspaceAvailability;
			switch (runspaceAvailability)
			{
			case RunspaceAvailability.None:
				if (pipelineState == PipelineState.Running)
				{
					this.RunspaceAvailability = RunspaceAvailability.Busy;
				}
				break;
			case RunspaceAvailability.Available:
				if (pipelineState != PipelineState.Running)
				{
					if (pipelineState == PipelineState.Disconnected)
					{
						this.RunspaceAvailability = RunspaceAvailability.None;
					}
				}
				else
				{
					this.RunspaceAvailability = RunspaceAvailability.Busy;
				}
				break;
			case RunspaceAvailability.AvailableForNestedCommand:
				if (pipelineState != PipelineState.Running)
				{
					if (pipelineState == PipelineState.Completed)
					{
						this.RunspaceAvailability = ((this.InNestedPrompt || this._runningPowerShells.Count > 1) ? RunspaceAvailability.AvailableForNestedCommand : RunspaceAvailability.Available);
					}
				}
				else
				{
					this.RunspaceAvailability = RunspaceAvailability.Busy;
				}
				break;
			case RunspaceAvailability.Busy:
			case RunspaceAvailability.RemoteDebug:
				switch (pipelineState)
				{
				case PipelineState.Stopped:
				case PipelineState.Completed:
				case PipelineState.Failed:
					if (this.InNestedPrompt || (!(this is RemoteRunspace) && this.Debugger.InBreakpoint))
					{
						this.RunspaceAvailability = RunspaceAvailability.AvailableForNestedCommand;
					}
					else
					{
						RemoteRunspace remoteRunspace = this as RemoteRunspace;
						RemoteDebugger remoteDebugger = (remoteRunspace != null) ? (remoteRunspace.Debugger as RemoteDebugger) : null;
						ConnectCommandInfo connectCommandInfo = (remoteRunspace != null) ? remoteRunspace.RemoteCommand : null;
						if ((pipelineState == PipelineState.Completed || pipelineState == PipelineState.Failed || (pipelineState == PipelineState.Stopped && this.RunspaceStateInfo.State == RunspaceState.Opened)) && connectCommandInfo != null && cmdInstanceId != null && connectCommandInfo.CommandId == cmdInstanceId)
						{
							remoteRunspace.RunspacePool.RemoteRunspacePoolInternal.ConnectCommands = null;
							connectCommandInfo = null;
							if (remoteDebugger != null && pipelineState == PipelineState.Stopped)
							{
								remoteDebugger.OnCommandStopped();
							}
						}
						Pipeline currentlyRunningPipeline = this.GetCurrentlyRunningPipeline();
						RemotePipeline remotePipeline = currentlyRunningPipeline as RemotePipeline;
						Guid? guid = (remotePipeline != null && remotePipeline.PowerShell != null) ? new Guid?(remotePipeline.PowerShell.InstanceId) : null;
						if (currentlyRunningPipeline == null)
						{
							if (connectCommandInfo == null)
							{
								if (remoteRunspace != null)
								{
									if (remoteDebugger != null && pipelineState == PipelineState.Stopped)
									{
										remoteDebugger.OnCommandStopped();
									}
									if (remoteDebugger != null && remoteDebugger.IsRemoteDebug)
									{
										this.RunspaceAvailability = RunspaceAvailability.RemoteDebug;
									}
									else
									{
										this.RunspaceAvailability = (remoteRunspace.RunspacePool.RemoteRunspacePoolInternal.AvailableForConnection ? RunspaceAvailability.Available : RunspaceAvailability.Busy);
									}
								}
								else
								{
									this.RunspaceAvailability = RunspaceAvailability.Available;
								}
							}
						}
						else if (cmdInstanceId != null && guid != null && cmdInstanceId == guid)
						{
							if (remoteDebugger != null && pipelineState == PipelineState.Stopped)
							{
								remoteDebugger.OnCommandStopped();
							}
							this.RunspaceAvailability = RunspaceAvailability.Available;
						}
						else if (runspaceAvailability == RunspaceAvailability.RemoteDebug)
						{
							this.RunspaceAvailability = RunspaceAvailability.RemoteDebug;
						}
						else if (currentlyRunningPipeline.PipelineStateInfo.State == PipelineState.Running || this._runningPowerShells.Count > 1)
						{
							this.RunspaceAvailability = RunspaceAvailability.Busy;
						}
						else
						{
							this.RunspaceAvailability = RunspaceAvailability.Available;
						}
					}
					break;
				case PipelineState.Disconnected:
					if (runspaceAvailability == RunspaceAvailability.RemoteDebug)
					{
						this.RunspaceAvailability = RunspaceAvailability.RemoteDebug;
					}
					else
					{
						this.RunspaceAvailability = RunspaceAvailability.None;
					}
					break;
				}
				break;
			}
			if (raiseEvent && this.RunspaceAvailability != runspaceAvailability)
			{
				this.OnAvailabilityChanged(new RunspaceAvailabilityEventArgs(this.RunspaceAvailability));
			}
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x000908B0 File Offset: 0x0008EAB0
		internal void UpdateRunspaceAvailability(PSInvocationState invocationState, bool raiseEvent, Guid cmdInstanceId)
		{
			switch (invocationState)
			{
			case PSInvocationState.NotStarted:
				this.UpdateRunspaceAvailability(PipelineState.NotStarted, raiseEvent, new Guid?(cmdInstanceId));
				return;
			case PSInvocationState.Running:
				this.UpdateRunspaceAvailability(PipelineState.Running, raiseEvent, new Guid?(cmdInstanceId));
				return;
			case PSInvocationState.Stopping:
				this.UpdateRunspaceAvailability(PipelineState.Stopping, raiseEvent, new Guid?(cmdInstanceId));
				return;
			case PSInvocationState.Stopped:
				this.UpdateRunspaceAvailability(PipelineState.Stopped, raiseEvent, new Guid?(cmdInstanceId));
				return;
			case PSInvocationState.Completed:
				this.UpdateRunspaceAvailability(PipelineState.Completed, raiseEvent, new Guid?(cmdInstanceId));
				return;
			case PSInvocationState.Failed:
				this.UpdateRunspaceAvailability(PipelineState.Failed, raiseEvent, new Guid?(cmdInstanceId));
				return;
			case PSInvocationState.Disconnected:
				this.UpdateRunspaceAvailability(PipelineState.Disconnected, raiseEvent, new Guid?(cmdInstanceId));
				return;
			default:
				return;
			}
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x0009094C File Offset: 0x0008EB4C
		protected void UpdateRunspaceAvailability(RunspaceState runspaceState, bool raiseEvent)
		{
			RunspaceAvailability runspaceAvailability = this.RunspaceAvailability;
			RemoteRunspace remoteRunspace = this as RemoteRunspace;
			ConnectCommandInfo connectCommandInfo = null;
			bool flag = false;
			if (remoteRunspace != null)
			{
				connectCommandInfo = remoteRunspace.RemoteCommand;
				RemoteDebugger remoteDebugger = remoteRunspace.Debugger as RemoteDebugger;
				flag = (remoteDebugger != null && remoteDebugger.IsRemoteDebug);
			}
			switch (runspaceAvailability)
			{
			case RunspaceAvailability.None:
				if (runspaceState == RunspaceState.Opened)
				{
					if (flag)
					{
						this.RunspaceAvailability = RunspaceAvailability.RemoteDebug;
					}
					else
					{
						this.RunspaceAvailability = ((connectCommandInfo == null && this.GetCurrentlyRunningPipeline() == null) ? RunspaceAvailability.Available : RunspaceAvailability.Busy);
					}
				}
				break;
			case RunspaceAvailability.Available:
			case RunspaceAvailability.AvailableForNestedCommand:
			case RunspaceAvailability.Busy:
			case RunspaceAvailability.RemoteDebug:
				switch (runspaceState)
				{
				case RunspaceState.Closed:
				case RunspaceState.Closing:
				case RunspaceState.Broken:
				case RunspaceState.Disconnected:
					this.RunspaceAvailability = RunspaceAvailability.None;
					break;
				}
				break;
			}
			if (raiseEvent && this.RunspaceAvailability != runspaceAvailability)
			{
				this.OnAvailabilityChanged(new RunspaceAvailabilityEventArgs(this.RunspaceAvailability));
			}
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x00090A24 File Offset: 0x0008EC24
		internal void UpdateRunspaceAvailability(RunspaceAvailability availability, bool raiseEvent)
		{
			RunspaceAvailability runspaceAvailability = this.RunspaceAvailability;
			this.RunspaceAvailability = availability;
			if (raiseEvent && this.RunspaceAvailability != runspaceAvailability)
			{
				this.OnAvailabilityChanged(new RunspaceAvailabilityEventArgs(this.RunspaceAvailability));
			}
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x00090A5C File Offset: 0x0008EC5C
		internal void RaiseAvailabilityChangedEvent(RunspaceAvailability availability)
		{
			this.OnAvailabilityChanged(new RunspaceAvailabilityEventArgs(availability));
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x00090A6A File Offset: 0x0008EC6A
		public static Runspace[] GetRunspaces(RunspaceConnectionInfo connectionInfo)
		{
			return Runspace.GetRunspaces(connectionInfo, null, null);
		}

		// Token: 0x060016D0 RID: 5840 RVA: 0x00090A74 File Offset: 0x0008EC74
		public static Runspace[] GetRunspaces(RunspaceConnectionInfo connectionInfo, PSHost host)
		{
			return Runspace.GetRunspaces(connectionInfo, host, null);
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x00090A7E File Offset: 0x0008EC7E
		public static Runspace[] GetRunspaces(RunspaceConnectionInfo connectionInfo, PSHost host, TypeTable typeTable)
		{
			return RemoteRunspace.GetRemoteRunspaces(connectionInfo, host, typeTable);
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x00090A88 File Offset: 0x0008EC88
		public static Runspace GetRunspace(RunspaceConnectionInfo connectionInfo, Guid sessionId, Guid? commandId, PSHost host, TypeTable typeTable)
		{
			return RemoteRunspace.GetRemoteRunspace(connectionInfo, sessionId, commandId, host, typeTable);
		}

		// Token: 0x060016D3 RID: 5843
		public abstract void Disconnect();

		// Token: 0x060016D4 RID: 5844
		public abstract void DisconnectAsync();

		// Token: 0x060016D5 RID: 5845
		public abstract void Connect();

		// Token: 0x060016D6 RID: 5846
		public abstract void ConnectAsync();

		// Token: 0x060016D7 RID: 5847
		public abstract Pipeline CreateDisconnectedPipeline();

		// Token: 0x060016D8 RID: 5848
		public abstract PowerShell CreateDisconnectedPowerShell();

		// Token: 0x060016D9 RID: 5849
		public abstract RunspaceCapability GetCapabilities();

		// Token: 0x060016DA RID: 5850
		public abstract void Open();

		// Token: 0x060016DB RID: 5851
		public abstract void OpenAsync();

		// Token: 0x060016DC RID: 5852
		public abstract void Close();

		// Token: 0x060016DD RID: 5853
		public abstract void CloseAsync();

		// Token: 0x060016DE RID: 5854
		public abstract Pipeline CreatePipeline();

		// Token: 0x060016DF RID: 5855
		public abstract Pipeline CreatePipeline(string command);

		// Token: 0x060016E0 RID: 5856
		public abstract Pipeline CreatePipeline(string command, bool addToHistory);

		// Token: 0x060016E1 RID: 5857
		public abstract Pipeline CreateNestedPipeline();

		// Token: 0x060016E2 RID: 5858
		public abstract Pipeline CreateNestedPipeline(string command, bool addToHistory);

		// Token: 0x060016E3 RID: 5859
		internal abstract Pipeline GetCurrentlyRunningPipeline();

		// Token: 0x060016E4 RID: 5860
		public abstract PSPrimitiveDictionary GetApplicationPrivateData();

		// Token: 0x060016E5 RID: 5861
		internal abstract void SetApplicationPrivateData(PSPrimitiveDictionary applicationPrivateData);

		// Token: 0x060016E6 RID: 5862 RVA: 0x00090A98 File Offset: 0x0008EC98
		internal void PushRunningPowerShell(PowerShell ps)
		{
			lock (this._syncObject)
			{
				this._runningPowerShells.Push(ps);
				if (this._runningPowerShells.Count == 1)
				{
					this._baseRunningPowerShell = ps;
				}
			}
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x00090AF4 File Offset: 0x0008ECF4
		internal PowerShell PopRunningPowerShell()
		{
			lock (this._syncObject)
			{
				int count = this._runningPowerShells.Count;
				if (count > 0)
				{
					if (count == 1)
					{
						this._baseRunningPowerShell = null;
					}
					return this._runningPowerShells.Pop();
				}
			}
			return null;
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x00090B5C File Offset: 0x0008ED5C
		internal PowerShell GetCurrentBasePowerShell()
		{
			return this._baseRunningPowerShell;
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x060016E9 RID: 5865 RVA: 0x00090B64 File Offset: 0x0008ED64
		public SessionStateProxy SessionStateProxy
		{
			get
			{
				return this.GetSessionStateProxy();
			}
		}

		// Token: 0x060016EA RID: 5866
		internal abstract SessionStateProxy GetSessionStateProxy();

		// Token: 0x060016EB RID: 5867 RVA: 0x00090B6C File Offset: 0x0008ED6C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060016EC RID: 5868 RVA: 0x00090B7C File Offset: 0x0008ED7C
		protected virtual void Dispose(bool disposing)
		{
			lock (Runspace.s_syncObject)
			{
				Runspace.s_runspaceDictionary.Remove(this.Id);
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x060016ED RID: 5869
		internal abstract ExecutionContext GetExecutionContext { get; }

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x060016EE RID: 5870
		internal abstract bool InNestedPrompt { get; }

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x060016EF RID: 5871 RVA: 0x00090BC8 File Offset: 0x0008EDC8
		public virtual Debugger Debugger
		{
			get
			{
				ExecutionContext getExecutionContext = this.GetExecutionContext;
				if (getExecutionContext == null)
				{
					return null;
				}
				return getExecutionContext.Debugger;
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x060016F0 RID: 5872 RVA: 0x00090BE7 File Offset: 0x0008EDE7
		// (set) Token: 0x060016F1 RID: 5873 RVA: 0x00090BEF File Offset: 0x0008EDEF
		internal Debugger InternalDebugger { get; set; }

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x060016F2 RID: 5874
		public abstract PSEventManager Events { get; }

		// Token: 0x060016F3 RID: 5875 RVA: 0x00090BF8 File Offset: 0x0008EDF8
		public void SetBaseTransaction(CommittableTransaction transaction)
		{
			this.ExecutionContext.TransactionManager.SetBaseTransaction(transaction, RollbackSeverity.Error);
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x00090C0C File Offset: 0x0008EE0C
		public void SetBaseTransaction(CommittableTransaction transaction, RollbackSeverity severity)
		{
			this.ExecutionContext.TransactionManager.SetBaseTransaction(transaction, severity);
		}

		// Token: 0x060016F5 RID: 5877 RVA: 0x00090C20 File Offset: 0x0008EE20
		public void ClearBaseTransaction()
		{
			this.ExecutionContext.TransactionManager.ClearBaseTransaction();
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x00090C32 File Offset: 0x0008EE32
		public virtual void ResetRunspaceState()
		{
			throw new NotImplementedException("ResetRunspaceState");
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x00090C3E File Offset: 0x0008EE3E
		internal long GeneratePipelineId()
		{
			return Interlocked.Increment(ref this._pipelineIdSeed);
		}

		// Token: 0x040009C3 RID: 2499
		internal const ApartmentState DefaultApartmentState = ApartmentState.Unknown;

		// Token: 0x040009C4 RID: 2500
		private static int _globalId;

		// Token: 0x040009C5 RID: 2501
		private Stack<PowerShell> _runningPowerShells;

		// Token: 0x040009C6 RID: 2502
		private PowerShell _baseRunningPowerShell;

		// Token: 0x040009C7 RID: 2503
		private object _syncObject;

		// Token: 0x040009C8 RID: 2504
		[ThreadStatic]
		private static Runspace ThreadSpecificDefaultRunspace = null;

		// Token: 0x040009C9 RID: 2505
		private ApartmentState apartmentState = ApartmentState.Unknown;

		// Token: 0x040009CA RID: 2506
		private Guid _instanceId = Guid.NewGuid();

		// Token: 0x040009CB RID: 2507
		private bool _skipUserProfile;

		// Token: 0x040009CC RID: 2508
		private Guid _engineActivityId = Guid.Empty;

		// Token: 0x040009CD RID: 2509
		private static SortedDictionary<int, WeakReference<Runspace>> s_runspaceDictionary;

		// Token: 0x040009CE RID: 2510
		private static object s_syncObject = new object();

		// Token: 0x040009CF RID: 2511
		private long _pipelineIdSeed;
	}
}
