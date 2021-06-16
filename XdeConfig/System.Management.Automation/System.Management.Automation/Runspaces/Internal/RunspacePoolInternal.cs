using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Security;
using System.Management.Automation.Tracing;
using System.Threading;

namespace System.Management.Automation.Runspaces.Internal
{
	// Token: 0x0200024C RID: 588
	internal class RunspacePoolInternal
	{
		// Token: 0x06001BD1 RID: 7121 RVA: 0x000A2184 File Offset: 0x000A0384
		public RunspacePoolInternal(int minRunspaces, int maxRunspaces, RunspaceConfiguration runspaceConfiguration, PSHost host) : this(minRunspaces, maxRunspaces)
		{
			if (runspaceConfiguration == null)
			{
				throw PSTraceSource.NewArgumentNullException("runspaceConfiguration");
			}
			if (host == null)
			{
				throw PSTraceSource.NewArgumentNullException("host");
			}
			this.rsConfig = runspaceConfiguration;
			this.host = host;
			this.pool = new Stack<Runspace>();
			this.runspaceRequestQueue = new Queue<GetRunspaceAsyncResult>();
			this.ultimateRequestQueue = new Queue<GetRunspaceAsyncResult>();
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x000A21E8 File Offset: 0x000A03E8
		public RunspacePoolInternal(int minRunspaces, int maxRunspaces, InitialSessionState initialSessionState, PSHost host) : this(minRunspaces, maxRunspaces)
		{
			if (initialSessionState == null)
			{
				throw PSTraceSource.NewArgumentNullException("initialSessionState");
			}
			if (host == null)
			{
				throw PSTraceSource.NewArgumentNullException("host");
			}
			this._initialSessionState = initialSessionState.Clone();
			this.host = host;
			this.threadOptions = initialSessionState.ThreadOptions;
			this.apartmentState = initialSessionState.ApartmentState;
			this.pool = new Stack<Runspace>();
			this.runspaceRequestQueue = new Queue<GetRunspaceAsyncResult>();
			this.ultimateRequestQueue = new Queue<GetRunspaceAsyncResult>();
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x000A2268 File Offset: 0x000A0468
		protected RunspacePoolInternal(int minRunspaces, int maxRunspaces)
		{
			this.runspaceList = new List<Runspace>();
			this.syncObject = new object();
			this.apartmentState = ApartmentState.Unknown;
			base..ctor();
			if (maxRunspaces < 1)
			{
				throw PSTraceSource.NewArgumentException("maxRunspaces", RunspacePoolStrings.MaxPoolLessThan1, new object[0]);
			}
			if (minRunspaces < 1)
			{
				throw PSTraceSource.NewArgumentException("minRunspaces", RunspacePoolStrings.MinPoolLessThan1, new object[0]);
			}
			if (minRunspaces > maxRunspaces)
			{
				throw PSTraceSource.NewArgumentException("minRunspaces", RunspacePoolStrings.MinPoolGreaterThanMaxPool, new object[0]);
			}
			this.maxPoolSz = maxRunspaces;
			this.minPoolSz = minRunspaces;
			this.stateInfo = new RunspacePoolStateInfo(RunspacePoolState.BeforeOpen, null);
			this.instanceId = Guid.NewGuid();
			PSEtwLog.SetActivityIdForCurrentThread(this.instanceId);
			this.cleanupInterval = RunspacePoolInternal.DefaultCleanupPeriod;
			this.cleanupTimer = new Timer(new TimerCallback(this.CleanupCallback), null, -1, -1);
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x000A233C File Offset: 0x000A053C
		internal RunspacePoolInternal()
		{
			this.runspaceList = new List<Runspace>();
			this.syncObject = new object();
			this.apartmentState = ApartmentState.Unknown;
			base..ctor();
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06001BD5 RID: 7125 RVA: 0x000A2361 File Offset: 0x000A0561
		public Guid InstanceId
		{
			get
			{
				return this.instanceId;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06001BD6 RID: 7126 RVA: 0x000A2369 File Offset: 0x000A0569
		public bool IsDisposed
		{
			get
			{
				return this.isDisposed;
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06001BD7 RID: 7127 RVA: 0x000A2371 File Offset: 0x000A0571
		public RunspacePoolStateInfo RunspacePoolStateInfo
		{
			get
			{
				return this.stateInfo;
			}
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x000A237C File Offset: 0x000A057C
		internal virtual PSPrimitiveDictionary GetApplicationPrivateData()
		{
			if (this.applicationPrivateData == null)
			{
				lock (this.syncObject)
				{
					if (this.applicationPrivateData == null)
					{
						this.applicationPrivateData = new PSPrimitiveDictionary();
					}
				}
			}
			return this.applicationPrivateData;
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x000A23D8 File Offset: 0x000A05D8
		internal virtual void PropagateApplicationPrivateData(Runspace runspace)
		{
			runspace.SetApplicationPrivateData(this.GetApplicationPrivateData());
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06001BDA RID: 7130 RVA: 0x000A23E6 File Offset: 0x000A05E6
		public RunspaceConfiguration RunspaceConfiguration
		{
			get
			{
				return this.rsConfig;
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06001BDB RID: 7131 RVA: 0x000A23EE File Offset: 0x000A05EE
		public InitialSessionState InitialSessionState
		{
			get
			{
				return this._initialSessionState;
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06001BDC RID: 7132 RVA: 0x000A23F6 File Offset: 0x000A05F6
		public virtual RunspaceConnectionInfo ConnectionInfo
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06001BDD RID: 7133 RVA: 0x000A23F9 File Offset: 0x000A05F9
		// (set) Token: 0x06001BDE RID: 7134 RVA: 0x000A2404 File Offset: 0x000A0604
		public TimeSpan CleanupInterval
		{
			get
			{
				return this.cleanupInterval;
			}
			set
			{
				lock (this.syncObject)
				{
					this.cleanupInterval = value;
				}
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06001BDF RID: 7135 RVA: 0x000A2448 File Offset: 0x000A0648
		public virtual RunspacePoolAvailability RunspacePoolAvailability
		{
			get
			{
				if (this.stateInfo.State != RunspacePoolState.Opened)
				{
					return RunspacePoolAvailability.None;
				}
				return RunspacePoolAvailability.Available;
			}
		}

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06001BE0 RID: 7136 RVA: 0x000A245C File Offset: 0x000A065C
		// (remove) Token: 0x06001BE1 RID: 7137 RVA: 0x000A2494 File Offset: 0x000A0694
		public event EventHandler<RunspacePoolStateChangedEventArgs> StateChanged;

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06001BE2 RID: 7138 RVA: 0x000A24CC File Offset: 0x000A06CC
		// (remove) Token: 0x06001BE3 RID: 7139 RVA: 0x000A2504 File Offset: 0x000A0704
		public event EventHandler<PSEventArgs> ForwardEvent;

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x06001BE4 RID: 7140 RVA: 0x000A253C File Offset: 0x000A073C
		// (remove) Token: 0x06001BE5 RID: 7141 RVA: 0x000A2574 File Offset: 0x000A0774
		internal event EventHandler<RunspaceCreatedEventArgs> RunspaceCreated;

		// Token: 0x06001BE6 RID: 7142 RVA: 0x000A25A9 File Offset: 0x000A07A9
		public virtual void Disconnect()
		{
			throw PSTraceSource.NewInvalidOperationException(RunspacePoolStrings.RunspaceDisconnectConnectNotSupported, new object[0]);
		}

		// Token: 0x06001BE7 RID: 7143 RVA: 0x000A25BB File Offset: 0x000A07BB
		public virtual IAsyncResult BeginDisconnect(AsyncCallback callback, object state)
		{
			throw PSTraceSource.NewInvalidOperationException(RunspacePoolStrings.RunspaceDisconnectConnectNotSupported, new object[0]);
		}

		// Token: 0x06001BE8 RID: 7144 RVA: 0x000A25CD File Offset: 0x000A07CD
		public virtual void EndDisconnect(IAsyncResult asyncResult)
		{
			throw PSTraceSource.NewInvalidOperationException(RunspacePoolStrings.RunspaceDisconnectConnectNotSupported, new object[0]);
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x000A25DF File Offset: 0x000A07DF
		public virtual void Connect()
		{
			throw PSTraceSource.NewInvalidOperationException(RunspacePoolStrings.RunspaceDisconnectConnectNotSupported, new object[0]);
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x000A25F1 File Offset: 0x000A07F1
		public virtual IAsyncResult BeginConnect(AsyncCallback callback, object state)
		{
			throw PSTraceSource.NewInvalidOperationException(RunspacePoolStrings.RunspaceDisconnectConnectNotSupported, new object[0]);
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x000A2603 File Offset: 0x000A0803
		public virtual void EndConnect(IAsyncResult asyncResult)
		{
			throw PSTraceSource.NewInvalidOperationException(RunspacePoolStrings.RunspaceDisconnectConnectNotSupported, new object[0]);
		}

		// Token: 0x06001BEC RID: 7148 RVA: 0x000A2615 File Offset: 0x000A0815
		public virtual Collection<PowerShell> CreateDisconnectedPowerShells(RunspacePool runspacePool)
		{
			throw PSTraceSource.NewInvalidOperationException(RunspacePoolStrings.RunspaceDisconnectConnectNotSupported, new object[0]);
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x000A2627 File Offset: 0x000A0827
		public virtual RunspacePoolCapability GetCapabilities()
		{
			return RunspacePoolCapability.Default;
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x000A262A File Offset: 0x000A082A
		internal virtual bool ResetRunspaceState()
		{
			throw new PSNotSupportedException();
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x000A2634 File Offset: 0x000A0834
		internal virtual bool SetMaxRunspaces(int maxRunspaces)
		{
			bool flag = false;
			lock (this.pool)
			{
				if (maxRunspaces < this.minPoolSz)
				{
					return false;
				}
				if (maxRunspaces > this.maxPoolSz)
				{
					flag = true;
				}
				else
				{
					while (this.pool.Count > maxRunspaces)
					{
						Runspace runspace = this.pool.Pop();
						this.DestroyRunspace(runspace);
					}
				}
				this.maxPoolSz = maxRunspaces;
			}
			if (flag)
			{
				this.EnqueueCheckAndStartRequestServicingThread(null, false);
			}
			return true;
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x000A26C4 File Offset: 0x000A08C4
		public int GetMaxRunspaces()
		{
			return this.maxPoolSz;
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x000A26CC File Offset: 0x000A08CC
		internal virtual bool SetMinRunspaces(int minRunspaces)
		{
			lock (this.pool)
			{
				if (minRunspaces < 1 || minRunspaces > this.maxPoolSz)
				{
					return false;
				}
				this.minPoolSz = minRunspaces;
			}
			return true;
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x000A2724 File Offset: 0x000A0924
		public int GetMinRunspaces()
		{
			return this.minPoolSz;
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x000A272C File Offset: 0x000A092C
		internal virtual int GetAvailableRunspaces()
		{
			int result;
			lock (this.syncObject)
			{
				if (this.stateInfo.State == RunspacePoolState.Opened)
				{
					int num = (this.maxPoolSz - this.totalRunspaces < 0) ? 0 : (this.maxPoolSz - this.totalRunspaces);
					result = this.pool.Count + num;
				}
				else
				{
					if (this.stateInfo.State != RunspacePoolState.BeforeOpen && this.stateInfo.State != RunspacePoolState.Opening)
					{
						throw new InvalidOperationException(HostInterfaceExceptionsStrings.RunspacePoolNotOpened);
					}
					if (this.stateInfo.State == RunspacePoolState.Disconnected)
					{
						throw new InvalidOperationException(RunspacePoolStrings.CannotWhileDisconnected);
					}
					result = this.maxPoolSz;
				}
			}
			return result;
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x000A27EC File Offset: 0x000A09EC
		public virtual void Open()
		{
			this.CoreOpen(false, null, null);
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x000A27F8 File Offset: 0x000A09F8
		public IAsyncResult BeginOpen(AsyncCallback callback, object state)
		{
			return this.CoreOpen(true, callback, state);
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x000A2804 File Offset: 0x000A0A04
		public void EndOpen(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw PSTraceSource.NewArgumentNullException("asyncResult");
			}
			RunspacePoolAsyncResult runspacePoolAsyncResult = asyncResult as RunspacePoolAsyncResult;
			if (runspacePoolAsyncResult == null || runspacePoolAsyncResult.OwnerId != this.instanceId || !runspacePoolAsyncResult.IsAssociatedWithAsyncOpen)
			{
				throw PSTraceSource.NewArgumentException("asyncResult", RunspacePoolStrings.AsyncResultNotOwned, new object[]
				{
					"IAsyncResult",
					"BeginOpen"
				});
			}
			runspacePoolAsyncResult.EndInvoke();
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x000A2872 File Offset: 0x000A0A72
		public virtual void Close()
		{
			this.CoreClose(false, null, null);
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x000A287E File Offset: 0x000A0A7E
		public virtual IAsyncResult BeginClose(AsyncCallback callback, object state)
		{
			return this.CoreClose(true, callback, state);
		}

		// Token: 0x06001BF9 RID: 7161 RVA: 0x000A288C File Offset: 0x000A0A8C
		public virtual void EndClose(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw PSTraceSource.NewArgumentNullException("asyncResult");
			}
			RunspacePoolAsyncResult runspacePoolAsyncResult = asyncResult as RunspacePoolAsyncResult;
			if (runspacePoolAsyncResult == null || runspacePoolAsyncResult.OwnerId != this.instanceId || runspacePoolAsyncResult.IsAssociatedWithAsyncOpen)
			{
				throw PSTraceSource.NewArgumentException("asyncResult", RunspacePoolStrings.AsyncResultNotOwned, new object[]
				{
					"IAsyncResult",
					"BeginClose"
				});
			}
			runspacePoolAsyncResult.EndInvoke();
		}

		// Token: 0x06001BFA RID: 7162 RVA: 0x000A28FC File Offset: 0x000A0AFC
		public Runspace GetRunspace()
		{
			this.AssertPoolIsOpen();
			GetRunspaceAsyncResult getRunspaceAsyncResult = (GetRunspaceAsyncResult)this.BeginGetRunspace(null, null);
			getRunspaceAsyncResult.AsyncWaitHandle.WaitOne();
			if (getRunspaceAsyncResult.Exception != null)
			{
				throw getRunspaceAsyncResult.Exception;
			}
			return getRunspaceAsyncResult.Runspace;
		}

		// Token: 0x06001BFB RID: 7163 RVA: 0x000A2940 File Offset: 0x000A0B40
		public void ReleaseRunspace(Runspace runspace)
		{
			if (runspace == null)
			{
				throw PSTraceSource.NewArgumentNullException("runspace");
			}
			this.AssertPoolIsOpen();
			bool flag = false;
			bool flag2 = false;
			lock (this.runspaceList)
			{
				if (!this.runspaceList.Contains(runspace))
				{
					throw PSTraceSource.NewInvalidOperationException(RunspacePoolStrings.RunspaceNotBelongsToPool, new object[0]);
				}
			}
			if (runspace.RunspaceStateInfo.State == RunspaceState.Opened)
			{
				lock (this.pool)
				{
					if (this.pool.Count < this.maxPoolSz)
					{
						flag = true;
						this.pool.Push(runspace);
					}
					else
					{
						flag = true;
						flag2 = true;
					}
					goto IL_AE;
				}
			}
			flag2 = true;
			flag = true;
			IL_AE:
			if (flag2)
			{
				this.DestroyRunspace(runspace);
			}
			if (flag)
			{
				this.EnqueueCheckAndStartRequestServicingThread(null, false);
			}
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x000A2A2C File Offset: 0x000A0C2C
		public virtual void Dispose(bool disposing)
		{
			if (!this.isDisposed)
			{
				if (disposing)
				{
					this.Close();
					this.cleanupTimer.Dispose();
					this._initialSessionState = null;
					this.host = null;
				}
				this.isDisposed = true;
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06001BFD RID: 7165 RVA: 0x000A2A5F File Offset: 0x000A0C5F
		// (set) Token: 0x06001BFE RID: 7166 RVA: 0x000A2A67 File Offset: 0x000A0C67
		internal PSThreadOptions ThreadOptions
		{
			get
			{
				return this.threadOptions;
			}
			set
			{
				this.threadOptions = value;
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06001BFF RID: 7167 RVA: 0x000A2A70 File Offset: 0x000A0C70
		// (set) Token: 0x06001C00 RID: 7168 RVA: 0x000A2A78 File Offset: 0x000A0C78
		internal ApartmentState ApartmentState
		{
			get
			{
				return this.apartmentState;
			}
			set
			{
				this.apartmentState = value;
			}
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x000A2A84 File Offset: 0x000A0C84
		internal IAsyncResult BeginGetRunspace(AsyncCallback callback, object state)
		{
			this.AssertPoolIsOpen();
			GetRunspaceAsyncResult getRunspaceAsyncResult = new GetRunspaceAsyncResult(this.InstanceId, callback, state);
			this.EnqueueCheckAndStartRequestServicingThread(getRunspaceAsyncResult, true);
			return getRunspaceAsyncResult;
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x000A2AB0 File Offset: 0x000A0CB0
		internal void CancelGetRunspace(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw PSTraceSource.NewArgumentNullException("asyncResult");
			}
			GetRunspaceAsyncResult getRunspaceAsyncResult = asyncResult as GetRunspaceAsyncResult;
			if (getRunspaceAsyncResult == null || getRunspaceAsyncResult.OwnerId != this.instanceId)
			{
				throw PSTraceSource.NewArgumentException("asyncResult", RunspacePoolStrings.AsyncResultNotOwned, new object[]
				{
					"IAsyncResult",
					"BeginGetRunspace"
				});
			}
			getRunspaceAsyncResult.IsActive = false;
		}

		// Token: 0x06001C03 RID: 7171 RVA: 0x000A2B18 File Offset: 0x000A0D18
		internal Runspace EndGetRunspace(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw PSTraceSource.NewArgumentNullException("asyncResult");
			}
			GetRunspaceAsyncResult getRunspaceAsyncResult = asyncResult as GetRunspaceAsyncResult;
			if (getRunspaceAsyncResult == null || getRunspaceAsyncResult.OwnerId != this.instanceId)
			{
				throw PSTraceSource.NewArgumentException("asyncResult", RunspacePoolStrings.AsyncResultNotOwned, new object[]
				{
					"IAsyncResult",
					"BeginGetRunspace"
				});
			}
			getRunspaceAsyncResult.EndInvoke();
			return getRunspaceAsyncResult.Runspace;
		}

		// Token: 0x06001C04 RID: 7172 RVA: 0x000A2B84 File Offset: 0x000A0D84
		protected virtual IAsyncResult CoreOpen(bool isAsync, AsyncCallback callback, object asyncState)
		{
			lock (this.syncObject)
			{
				this.AssertIfStateIsBeforeOpen();
				this.stateInfo = new RunspacePoolStateInfo(RunspacePoolState.Opening, null);
			}
			this.RaiseStateChangeEvent(this.stateInfo);
			if (isAsync)
			{
				AsyncResult asyncResult = new RunspacePoolAsyncResult(this.instanceId, callback, asyncState, true);
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.OpenThreadProc), asyncResult);
				return asyncResult;
			}
			this.OpenHelper();
			return null;
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x000A2C0C File Offset: 0x000A0E0C
		protected void OpenHelper()
		{
			try
			{
				PSEtwLog.SetActivityIdForCurrentThread(this.InstanceId);
				Runspace item = this.CreateRunspace();
				this.pool.Push(item);
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				this.SetStateToBroken(ex);
				throw;
			}
			bool flag = false;
			lock (this.syncObject)
			{
				if (this.stateInfo.State == RunspacePoolState.Opening)
				{
					this.stateInfo = new RunspacePoolStateInfo(RunspacePoolState.Opened, null);
					flag = true;
				}
			}
			if (flag)
			{
				this.RaiseStateChangeEvent(this.stateInfo);
			}
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x000A2CB4 File Offset: 0x000A0EB4
		private void SetStateToBroken(Exception reason)
		{
			bool flag = false;
			lock (this.syncObject)
			{
				if (this.stateInfo.State == RunspacePoolState.Opening || this.stateInfo.State == RunspacePoolState.Opened || this.stateInfo.State == RunspacePoolState.Disconnecting || this.stateInfo.State == RunspacePoolState.Disconnected || this.stateInfo.State == RunspacePoolState.Connecting)
				{
					this.stateInfo = new RunspacePoolStateInfo(RunspacePoolState.Broken, null);
					flag = true;
				}
			}
			if (flag)
			{
				RunspacePoolStateInfo runspacePoolStateInfo = new RunspacePoolStateInfo(this.stateInfo.State, reason);
				this.RaiseStateChangeEvent(runspacePoolStateInfo);
			}
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x000A2D64 File Offset: 0x000A0F64
		protected void OpenThreadProc(object o)
		{
			AsyncResult asyncResult = (AsyncResult)o;
			Exception asCompleted = null;
			try
			{
				this.OpenHelper();
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				asCompleted = ex;
			}
			finally
			{
				asyncResult.SetAsCompleted(asCompleted);
			}
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x000A2DB4 File Offset: 0x000A0FB4
		private IAsyncResult CoreClose(bool isAsync, AsyncCallback callback, object asyncState)
		{
			lock (this.syncObject)
			{
				if (this.stateInfo.State == RunspacePoolState.Closed || this.stateInfo.State == RunspacePoolState.Broken || this.stateInfo.State == RunspacePoolState.Closing || this.stateInfo.State == RunspacePoolState.Disconnecting || this.stateInfo.State == RunspacePoolState.Disconnected)
				{
					if (isAsync)
					{
						RunspacePoolAsyncResult runspacePoolAsyncResult = new RunspacePoolAsyncResult(this.instanceId, callback, asyncState, false);
						runspacePoolAsyncResult.SetAsCompleted(null);
						return runspacePoolAsyncResult;
					}
					return null;
				}
				else
				{
					this.stateInfo = new RunspacePoolStateInfo(RunspacePoolState.Closing, null);
				}
			}
			this.RaiseStateChangeEvent(this.stateInfo);
			if (isAsync)
			{
				RunspacePoolAsyncResult runspacePoolAsyncResult2 = new RunspacePoolAsyncResult(this.instanceId, callback, asyncState, false);
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.CloseThreadProc), runspacePoolAsyncResult2);
				return runspacePoolAsyncResult2;
			}
			this.CloseHelper();
			return null;
		}

		// Token: 0x06001C09 RID: 7177 RVA: 0x000A2EA0 File Offset: 0x000A10A0
		private void CloseHelper()
		{
			try
			{
				this.InternalClearAllResources();
			}
			finally
			{
				this.stateInfo = new RunspacePoolStateInfo(RunspacePoolState.Closed, null);
				this.RaiseStateChangeEvent(this.stateInfo);
			}
		}

		// Token: 0x06001C0A RID: 7178 RVA: 0x000A2EE0 File Offset: 0x000A10E0
		private void CloseThreadProc(object o)
		{
			AsyncResult asyncResult = (AsyncResult)o;
			Exception asCompleted = null;
			try
			{
				this.CloseHelper();
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				asCompleted = ex;
			}
			finally
			{
				asyncResult.SetAsCompleted(asCompleted);
			}
		}

		// Token: 0x06001C0B RID: 7179 RVA: 0x000A2F30 File Offset: 0x000A1130
		protected void RaiseStateChangeEvent(RunspacePoolStateInfo stateInfo)
		{
			this.StateChanged.SafeInvoke(this, new RunspacePoolStateChangedEventArgs(stateInfo));
		}

		// Token: 0x06001C0C RID: 7180 RVA: 0x000A2F44 File Offset: 0x000A1144
		internal void AssertPoolIsOpen()
		{
			lock (this.syncObject)
			{
				if (this.stateInfo.State != RunspacePoolState.Opened)
				{
					string message = StringUtil.Format(RunspacePoolStrings.InvalidRunspacePoolState, RunspacePoolState.Opened, this.stateInfo.State);
					throw new InvalidRunspacePoolStateException(message, this.stateInfo.State, RunspacePoolState.Opened);
				}
			}
		}

		// Token: 0x06001C0D RID: 7181 RVA: 0x000A2FC0 File Offset: 0x000A11C0
		protected Runspace CreateRunspace()
		{
			Runspace runspace = null;
			if (this.rsConfig != null)
			{
				runspace = RunspaceFactory.CreateRunspace(this.host, this.rsConfig);
			}
			else
			{
				runspace = RunspaceFactory.CreateRunspaceFromSessionStateNoClone(this.host, this._initialSessionState);
			}
			runspace.ThreadOptions = ((this.ThreadOptions == PSThreadOptions.Default) ? PSThreadOptions.ReuseThread : this.ThreadOptions);
			runspace.ApartmentState = this.ApartmentState;
			this.PropagateApplicationPrivateData(runspace);
			runspace.Open();
			if (SystemPolicy.GetSystemLockdownPolicy() == SystemEnforcementMode.Enforce)
			{
				runspace.ExecutionContext.LanguageMode = PSLanguageMode.ConstrainedLanguage;
			}
			runspace.Events.ForwardEvent += this.OnRunspaceForwardEvent;
			lock (this.runspaceList)
			{
				this.runspaceList.Add(runspace);
				this.totalRunspaces = this.runspaceList.Count;
			}
			lock (this.syncObject)
			{
				this.cleanupTimer.Change(this.CleanupInterval, this.CleanupInterval);
			}
			this.RunspaceCreated.SafeInvoke(this, new RunspaceCreatedEventArgs(runspace));
			return runspace;
		}

		// Token: 0x06001C0E RID: 7182 RVA: 0x000A30F8 File Offset: 0x000A12F8
		protected void DestroyRunspace(Runspace runspace)
		{
			runspace.Events.ForwardEvent -= this.OnRunspaceForwardEvent;
			runspace.Close();
			runspace.Dispose();
			lock (this.runspaceList)
			{
				this.runspaceList.Remove(runspace);
				this.totalRunspaces = this.runspaceList.Count;
			}
		}

		// Token: 0x06001C0F RID: 7183 RVA: 0x000A3174 File Offset: 0x000A1374
		protected void CleanupCallback(object state)
		{
			bool flag = false;
			while (this.totalRunspaces > this.minPoolSz)
			{
				if (this.stateInfo.State == RunspacePoolState.Closing)
				{
					return;
				}
				Runspace runspace = null;
				lock (this.pool)
				{
					if (this.pool.Count <= 0)
					{
						break;
					}
					runspace = this.pool.Pop();
				}
				if (!flag)
				{
					lock (this.syncObject)
					{
						this.cleanupTimer.Change(-1, -1);
						flag = true;
					}
				}
				this.DestroyRunspace(runspace);
			}
		}

		// Token: 0x06001C10 RID: 7184 RVA: 0x000A323C File Offset: 0x000A143C
		private void InternalClearAllResources()
		{
			string message = StringUtil.Format(RunspacePoolStrings.InvalidRunspacePoolState, RunspacePoolState.Opened, this.stateInfo.State);
			Exception asCompleted = new InvalidRunspacePoolStateException(message, this.stateInfo.State, RunspacePoolState.Opened);
			lock (this.runspaceRequestQueue)
			{
				while (this.runspaceRequestQueue.Count > 0)
				{
					GetRunspaceAsyncResult getRunspaceAsyncResult = this.runspaceRequestQueue.Dequeue();
					getRunspaceAsyncResult.SetAsCompleted(asCompleted);
				}
			}
			lock (this.ultimateRequestQueue)
			{
				while (this.ultimateRequestQueue.Count > 0)
				{
					GetRunspaceAsyncResult getRunspaceAsyncResult = this.ultimateRequestQueue.Dequeue();
					getRunspaceAsyncResult.SetAsCompleted(asCompleted);
				}
			}
			List<Runspace> list = new List<Runspace>();
			lock (this.runspaceList)
			{
				list.AddRange(this.runspaceList);
				this.runspaceList.Clear();
			}
			for (int i = list.Count - 1; i >= 0; i--)
			{
				try
				{
					list[i].Close();
					list[i].Dispose();
				}
				catch (InvalidRunspaceStateException e)
				{
					CommandProcessorBase.CheckForSevereException(e);
				}
			}
			lock (this.pool)
			{
				this.pool.Clear();
			}
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x000A33F0 File Offset: 0x000A15F0
		protected void EnqueueCheckAndStartRequestServicingThread(GetRunspaceAsyncResult requestToEnqueue, bool useCallingThread)
		{
			bool flag = false;
			lock (this.runspaceRequestQueue)
			{
				if (requestToEnqueue != null)
				{
					this.runspaceRequestQueue.Enqueue(requestToEnqueue);
				}
				if (this.isServicingRequests)
				{
					return;
				}
				if (this.runspaceRequestQueue.Count + this.ultimateRequestQueue.Count > 0)
				{
					lock (this.pool)
					{
						if (this.pool.Count > 0 || this.totalRunspaces < this.maxPoolSz)
						{
							this.isServicingRequests = true;
							if (useCallingThread && this.ultimateRequestQueue.Count == 0)
							{
								flag = true;
							}
							else
							{
								ThreadPool.QueueUserWorkItem(new WaitCallback(this.ServicePendingRequests), false);
							}
						}
					}
				}
			}
			if (flag)
			{
				this.ServicePendingRequests(true);
			}
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x000A34EC File Offset: 0x000A16EC
		protected void ServicePendingRequests(object useCallingThreadState)
		{
			if (this.stateInfo.State == RunspacePoolState.Closed || this.stateInfo.State == RunspacePoolState.Closing)
			{
				return;
			}
			bool flag = (bool)useCallingThreadState;
			GetRunspaceAsyncResult getRunspaceAsyncResult = null;
			try
			{
				for (;;)
				{
					lock (this.ultimateRequestQueue)
					{
						while (this.ultimateRequestQueue.Count > 0)
						{
							if (this.stateInfo.State == RunspacePoolState.Closing)
							{
								return;
							}
							Runspace runspace;
							lock (this.pool)
							{
								if (this.pool.Count > 0)
								{
									runspace = this.pool.Pop();
								}
								else
								{
									if (this.totalRunspaces >= this.maxPoolSz)
									{
										return;
									}
									runspace = this.CreateRunspace();
								}
							}
							getRunspaceAsyncResult = this.ultimateRequestQueue.Dequeue();
							if (!getRunspaceAsyncResult.IsActive)
							{
								lock (this.pool)
								{
									this.pool.Push(runspace);
								}
								getRunspaceAsyncResult.Release();
							}
							else
							{
								getRunspaceAsyncResult.Runspace = runspace;
								if (flag)
								{
									goto IL_186;
								}
								ThreadPool.QueueUserWorkItem(new WaitCallback(getRunspaceAsyncResult.DoComplete));
							}
						}
					}
					lock (this.runspaceRequestQueue)
					{
						if (this.runspaceRequestQueue.Count != 0)
						{
							while (this.runspaceRequestQueue.Count > 0)
							{
								this.ultimateRequestQueue.Enqueue(this.runspaceRequestQueue.Dequeue());
							}
							continue;
						}
					}
					break;
				}
				IL_186:;
			}
			finally
			{
				lock (this.runspaceRequestQueue)
				{
					this.isServicingRequests = false;
					this.EnqueueCheckAndStartRequestServicingThread(null, false);
				}
			}
			if (flag && getRunspaceAsyncResult != null)
			{
				getRunspaceAsyncResult.DoComplete(null);
			}
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x000A3754 File Offset: 0x000A1954
		protected void AssertIfStateIsBeforeOpen()
		{
			if (this.stateInfo.State != RunspacePoolState.BeforeOpen)
			{
				InvalidRunspacePoolStateException ex = new InvalidRunspacePoolStateException(StringUtil.Format(RunspacePoolStrings.CannotOpenAgain, new object[]
				{
					this.stateInfo.State.ToString()
				}), this.stateInfo.State, RunspacePoolState.BeforeOpen);
				throw ex;
			}
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x000A37AC File Offset: 0x000A19AC
		protected virtual void OnForwardEvent(PSEventArgs e)
		{
			EventHandler<PSEventArgs> forwardEvent = this.ForwardEvent;
			if (forwardEvent != null)
			{
				forwardEvent(this, e);
			}
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x000A37CB File Offset: 0x000A19CB
		private void OnRunspaceForwardEvent(object sender, PSEventArgs e)
		{
			if (e.ForwardEvent)
			{
				this.OnForwardEvent(e);
			}
		}

		// Token: 0x04000B5E RID: 2910
		protected int maxPoolSz;

		// Token: 0x04000B5F RID: 2911
		protected int minPoolSz;

		// Token: 0x04000B60 RID: 2912
		protected int totalRunspaces;

		// Token: 0x04000B61 RID: 2913
		protected List<Runspace> runspaceList;

		// Token: 0x04000B62 RID: 2914
		protected Stack<Runspace> pool;

		// Token: 0x04000B63 RID: 2915
		protected Queue<GetRunspaceAsyncResult> runspaceRequestQueue;

		// Token: 0x04000B64 RID: 2916
		protected Queue<GetRunspaceAsyncResult> ultimateRequestQueue;

		// Token: 0x04000B65 RID: 2917
		protected RunspacePoolStateInfo stateInfo;

		// Token: 0x04000B66 RID: 2918
		protected RunspaceConfiguration rsConfig;

		// Token: 0x04000B67 RID: 2919
		protected InitialSessionState _initialSessionState;

		// Token: 0x04000B68 RID: 2920
		protected PSHost host;

		// Token: 0x04000B69 RID: 2921
		protected Guid instanceId;

		// Token: 0x04000B6A RID: 2922
		private bool isDisposed;

		// Token: 0x04000B6B RID: 2923
		protected bool isServicingRequests;

		// Token: 0x04000B6C RID: 2924
		protected object syncObject;

		// Token: 0x04000B6D RID: 2925
		private static readonly TimeSpan DefaultCleanupPeriod = new TimeSpan(0, 15, 0);

		// Token: 0x04000B6E RID: 2926
		private TimeSpan cleanupInterval;

		// Token: 0x04000B6F RID: 2927
		private Timer cleanupTimer;

		// Token: 0x04000B70 RID: 2928
		private PSPrimitiveDictionary applicationPrivateData;

		// Token: 0x04000B74 RID: 2932
		private PSThreadOptions threadOptions;

		// Token: 0x04000B75 RID: 2933
		private ApartmentState apartmentState;
	}
}
