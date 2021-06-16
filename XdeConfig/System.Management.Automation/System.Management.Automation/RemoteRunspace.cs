using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Remoting.Client;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Runspaces.Internal;
using System.Management.Automation.Tracing;
using System.Threading;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x02000265 RID: 613
	internal class RemoteRunspace : Runspace, IDisposable
	{
		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06001CA9 RID: 7337 RVA: 0x000A6F0C File Offset: 0x000A510C
		// (set) Token: 0x06001CAA RID: 7338 RVA: 0x000A6F14 File Offset: 0x000A5114
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

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06001CAB RID: 7339 RVA: 0x000A6F1D File Offset: 0x000A511D
		// (set) Token: 0x06001CAC RID: 7340 RVA: 0x000A6F25 File Offset: 0x000A5125
		internal bool ShouldCloseOnPop
		{
			get
			{
				return this._shouldCloseOnPop;
			}
			set
			{
				this._shouldCloseOnPop = value;
			}
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x000A6F30 File Offset: 0x000A5130
		internal RemoteRunspace(TypeTable typeTable, RunspaceConnectionInfo connectionInfo, PSHost host, PSPrimitiveDictionary applicationArguments, string name = null, int id = -1)
		{
			PSEtwLog.SetActivityIdForCurrentThread(base.InstanceId);
			PSEtwLog.LogOperationalVerbose(PSEventId.RunspaceConstructor, PSOpcode.Constructor, PSTask.CreateRunspace, PSKeyword.UseAlwaysOperational, new object[]
			{
				base.InstanceId.ToString()
			});
			this._connectionInfo = connectionInfo.InternalCopy();
			this._originalConnectionInfo = connectionInfo.InternalCopy();
			this._runspacePool = new RunspacePool(1, 1, typeTable, host, applicationArguments, connectionInfo, name);
			this.PSSessionId = id;
			this.SetEventHandlers();
		}

		// Token: 0x06001CAE RID: 7342 RVA: 0x000A6FFC File Offset: 0x000A51FC
		internal RemoteRunspace(RunspacePool runspacePool)
		{
			if (runspacePool.RunspacePoolStateInfo.State != RunspacePoolState.Disconnected || !(runspacePool.ConnectionInfo is WSManConnectionInfo))
			{
				throw PSTraceSource.NewInvalidOperationException(RunspaceStrings.InvalidRunspacePool, new object[0]);
			}
			this._runspacePool = runspacePool;
			this._runspacePool.RemoteRunspacePoolInternal.SetMinRunspaces(1);
			this._runspacePool.RemoteRunspacePoolInternal.SetMaxRunspaces(1);
			this._connectionInfo = runspacePool.ConnectionInfo.InternalCopy();
			this.UpdateDisconnectExpiresOn();
			this.SetRunspaceState(RunspaceState.Disconnected, null);
			this._runspaceAvailability = (this._runspacePool.RemoteRunspacePoolInternal.AvailableForConnection ? RunspaceAvailability.None : RunspaceAvailability.Busy);
			this.SetEventHandlers();
			PSEtwLog.SetActivityIdForCurrentThread(base.InstanceId);
			PSEtwLog.LogOperationalVerbose(PSEventId.RunspaceConstructor, PSOpcode.Constructor, PSTask.CreateRunspace, PSKeyword.UseAlwaysOperational, new object[]
			{
				base.InstanceId.ToString()
			});
		}

		// Token: 0x06001CAF RID: 7343 RVA: 0x000A7128 File Offset: 0x000A5328
		private void SetEventHandlers()
		{
			base.InstanceId = this._runspacePool.InstanceId;
			this._eventManager = new PSRemoteEventManager(this._connectionInfo.ComputerName, base.InstanceId);
			this._runspacePool.StateChanged += this.HandleRunspacePoolStateChanged;
			this._runspacePool.RemoteRunspacePoolInternal.HostCallReceived += this.HandleHostCallReceived;
			this._runspacePool.RemoteRunspacePoolInternal.URIRedirectionReported += this.HandleURIDirectionReported;
			this._runspacePool.ForwardEvent += this.HandleRunspacePoolForwardEvent;
			this._runspacePool.RemoteRunspacePoolInternal.SessionCreateCompleted += this.HandleSessionCreateCompleted;
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06001CB0 RID: 7344 RVA: 0x000A71E4 File Offset: 0x000A53E4
		public override RunspaceConfiguration RunspaceConfiguration
		{
			get
			{
				throw PSTraceSource.NewNotImplementedException();
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06001CB1 RID: 7345 RVA: 0x000A71EB File Offset: 0x000A53EB
		public override InitialSessionState InitialSessionState
		{
			get
			{
				throw PSTraceSource.NewNotImplementedException();
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06001CB2 RID: 7346 RVA: 0x000A71F2 File Offset: 0x000A53F2
		public override JobManager JobManager
		{
			get
			{
				throw PSTraceSource.NewNotImplementedException();
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06001CB3 RID: 7347 RVA: 0x000A71F9 File Offset: 0x000A53F9
		public override Version Version
		{
			get
			{
				return this._version;
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06001CB4 RID: 7348 RVA: 0x000A7204 File Offset: 0x000A5404
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

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06001CB5 RID: 7349 RVA: 0x000A724C File Offset: 0x000A544C
		// (set) Token: 0x06001CB6 RID: 7350 RVA: 0x000A7254 File Offset: 0x000A5454
		public override PSThreadOptions ThreadOptions
		{
			get
			{
				return this.createThreadOptions;
			}
			set
			{
				lock (this._syncRoot)
				{
					if (value != this.createThreadOptions)
					{
						if (this.RunspaceStateInfo.State != RunspaceState.BeforeOpen)
						{
							throw new InvalidRunspaceStateException(StringUtil.Format(RunspaceStrings.ChangePropertyAfterOpen, new object[0]));
						}
						this.createThreadOptions = value;
					}
				}
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06001CB7 RID: 7351 RVA: 0x000A72C4 File Offset: 0x000A54C4
		// (set) Token: 0x06001CB8 RID: 7352 RVA: 0x000A72CC File Offset: 0x000A54CC
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

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x06001CB9 RID: 7353 RVA: 0x000A72D8 File Offset: 0x000A54D8
		// (remove) Token: 0x06001CBA RID: 7354 RVA: 0x000A7310 File Offset: 0x000A5510
		public override event EventHandler<RunspaceStateEventArgs> StateChanged;

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x06001CBB RID: 7355 RVA: 0x000A7348 File Offset: 0x000A5548
		// (remove) Token: 0x06001CBC RID: 7356 RVA: 0x000A7380 File Offset: 0x000A5580
		public override event EventHandler<RunspaceAvailabilityEventArgs> AvailabilityChanged;

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06001CBD RID: 7357 RVA: 0x000A73B5 File Offset: 0x000A55B5
		internal override bool HasAvailabilityChangedSubscribers
		{
			get
			{
				return this.AvailabilityChanged != null;
			}
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x000A73C4 File Offset: 0x000A55C4
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

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06001CBF RID: 7359 RVA: 0x000A7400 File Offset: 0x000A5600
		public override RunspaceConnectionInfo ConnectionInfo
		{
			get
			{
				return this._connectionInfo;
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06001CC0 RID: 7360 RVA: 0x000A7408 File Offset: 0x000A5608
		public override RunspaceConnectionInfo OriginalConnectionInfo
		{
			get
			{
				return this._originalConnectionInfo;
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06001CC1 RID: 7361 RVA: 0x000A7410 File Offset: 0x000A5610
		public override PSEventManager Events
		{
			get
			{
				return this._eventManager;
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06001CC2 RID: 7362 RVA: 0x000A7418 File Offset: 0x000A5618
		internal override ExecutionContext GetExecutionContext
		{
			get
			{
				throw PSTraceSource.NewNotImplementedException();
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06001CC3 RID: 7363 RVA: 0x000A741F File Offset: 0x000A561F
		internal override bool InNestedPrompt
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06001CC4 RID: 7364 RVA: 0x000A7424 File Offset: 0x000A5624
		internal ClientRemoteSession ClientRemoteSession
		{
			get
			{
				ClientRemoteSession remoteSession;
				try
				{
					remoteSession = this._runspacePool.RemoteRunspacePoolInternal.DataStructureHandler.RemoteSession;
				}
				catch (InvalidRunspacePoolStateException ex)
				{
					throw ex.ToInvalidRunspaceStateException();
				}
				return remoteSession;
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06001CC5 RID: 7365 RVA: 0x000A7464 File Offset: 0x000A5664
		internal ConnectCommandInfo RemoteCommand
		{
			get
			{
				if (this._runspacePool.RemoteRunspacePoolInternal.ConnectCommands == null)
				{
					return null;
				}
				if (this._runspacePool.RemoteRunspacePoolInternal.ConnectCommands.Length > 0)
				{
					return this._runspacePool.RemoteRunspacePoolInternal.ConnectCommands[0];
				}
				return null;
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06001CC6 RID: 7366 RVA: 0x000A74A3 File Offset: 0x000A56A3
		// (set) Token: 0x06001CC7 RID: 7367 RVA: 0x000A74B5 File Offset: 0x000A56B5
		internal string PSSessionName
		{
			get
			{
				return this._runspacePool.RemoteRunspacePoolInternal.Name;
			}
			set
			{
				this._runspacePool.RemoteRunspacePoolInternal.Name = value;
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06001CC8 RID: 7368 RVA: 0x000A74C8 File Offset: 0x000A56C8
		// (set) Token: 0x06001CC9 RID: 7369 RVA: 0x000A74D0 File Offset: 0x000A56D0
		internal int PSSessionId
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06001CCA RID: 7370 RVA: 0x000A74D9 File Offset: 0x000A56D9
		internal bool CanDisconnect
		{
			get
			{
				return this._runspacePool.RemoteRunspacePoolInternal.CanDisconnect;
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06001CCB RID: 7371 RVA: 0x000A74EB File Offset: 0x000A56EB
		internal bool CanConnect
		{
			get
			{
				return this._runspacePool.RemoteRunspacePoolInternal.AvailableForConnection;
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06001CCC RID: 7372 RVA: 0x000A74FD File Offset: 0x000A56FD
		public override Debugger Debugger
		{
			get
			{
				return this._remoteDebugger;
			}
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x000A7508 File Offset: 0x000A5708
		public override void OpenAsync()
		{
			this.AssertIfStateIsBeforeOpen();
			try
			{
				this._runspacePool.BeginOpen(null, null);
			}
			catch (InvalidRunspacePoolStateException ex)
			{
				throw ex.ToInvalidRunspaceStateException();
			}
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x000A7544 File Offset: 0x000A5744
		public override void Open()
		{
			this.AssertIfStateIsBeforeOpen();
			try
			{
				this._runspacePool.ThreadOptions = this.ThreadOptions;
				this._runspacePool.ApartmentState = base.ApartmentState;
				this._runspacePool.Open();
			}
			catch (InvalidRunspacePoolStateException ex)
			{
				throw ex.ToInvalidRunspaceStateException();
			}
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x000A75A0 File Offset: 0x000A57A0
		public override void CloseAsync()
		{
			try
			{
				this._runspacePool.BeginClose(null, null);
			}
			catch (InvalidRunspacePoolStateException ex)
			{
				throw ex.ToInvalidRunspaceStateException();
			}
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x000A75D8 File Offset: 0x000A57D8
		public override void Close()
		{
			try
			{
				IAsyncResult asyncResult = this._runspacePool.BeginClose(null, null);
				this.WaitForFinishofPipelines();
				if (asyncResult != null)
				{
					this._runspacePool.EndClose(asyncResult);
				}
			}
			catch (InvalidRunspacePoolStateException ex)
			{
				throw ex.ToInvalidRunspaceStateException();
			}
		}

		// Token: 0x06001CD1 RID: 7377 RVA: 0x000A7624 File Offset: 0x000A5824
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
						try
						{
							this.Close();
						}
						catch (PSRemotingTransportException)
						{
						}
						if (this._remoteDebugger != null)
						{
							this._remoteDebugger.Dispose();
						}
						try
						{
							this._runspacePool.StateChanged -= this.HandleRunspacePoolStateChanged;
							this._runspacePool.RemoteRunspacePoolInternal.HostCallReceived -= this.HandleHostCallReceived;
							this._runspacePool.RemoteRunspacePoolInternal.URIRedirectionReported -= this.HandleURIDirectionReported;
							this._runspacePool.ForwardEvent -= this.HandleRunspacePoolForwardEvent;
							this._runspacePool.RemoteRunspacePoolInternal.SessionCreateCompleted -= this.HandleSessionCreateCompleted;
							this._eventManager = null;
							this._runspacePool.Dispose();
						}
						catch (InvalidRunspacePoolStateException ex)
						{
							throw ex.ToInvalidRunspaceStateException();
						}
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06001CD2 RID: 7378 RVA: 0x000A77A0 File Offset: 0x000A59A0
		public override void ResetRunspaceState()
		{
			PSInvalidOperationException ex = null;
			if (this.RunspaceStateInfo.State != RunspaceState.Opened)
			{
				ex = PSTraceSource.NewInvalidOperationException(RunspaceStrings.RunspaceNotInOpenedState, new object[]
				{
					this.RunspaceStateInfo.State
				});
			}
			else if (this.RunspaceAvailability != RunspaceAvailability.Available)
			{
				ex = PSTraceSource.NewInvalidOperationException(RunspaceStrings.ConcurrentInvokeNotAllowed, new object[0]);
			}
			else if (!this._runspacePool.RemoteRunspacePoolInternal.ResetRunspaceState())
			{
				ex = PSTraceSource.NewInvalidOperationException();
			}
			if (ex != null)
			{
				ex.Source = "ResetRunspaceState";
				throw ex;
			}
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x000A782C File Offset: 0x000A5A2C
		internal static Runspace[] GetRemoteRunspaces(RunspaceConnectionInfo connectionInfo, PSHost host, TypeTable typeTable)
		{
			List<Runspace> list = new List<Runspace>();
			RunspacePool[] remoteRunspacePools = RemoteRunspacePoolInternal.GetRemoteRunspacePools(connectionInfo, host, typeTable);
			foreach (RunspacePool runspacePool in remoteRunspacePools)
			{
				if (runspacePool.RemoteRunspacePoolInternal.ConnectCommands.Length < 2)
				{
					list.Add(new RemoteRunspace(runspacePool));
				}
			}
			return list.ToArray();
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x000A7884 File Offset: 0x000A5A84
		internal static Runspace GetRemoteRunspace(RunspaceConnectionInfo connectionInfo, Guid sessionId, Guid? commandId, PSHost host, TypeTable typeTable)
		{
			RunspacePool remoteRunspacePool = RemoteRunspacePoolInternal.GetRemoteRunspacePool(connectionInfo, sessionId, commandId, host, typeTable);
			return new RemoteRunspace(remoteRunspacePool);
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x000A78A4 File Offset: 0x000A5AA4
		public override void Disconnect()
		{
			if (!this.CanDisconnect)
			{
				throw PSTraceSource.NewInvalidOperationException(RunspaceStrings.DisconnectNotSupportedOnServer, new object[0]);
			}
			this.UpdatePoolDisconnectOptions();
			try
			{
				this._runspacePool.Disconnect();
			}
			catch (InvalidRunspacePoolStateException ex)
			{
				throw ex.ToInvalidRunspaceStateException();
			}
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x000A78F8 File Offset: 0x000A5AF8
		public override void DisconnectAsync()
		{
			if (!this.CanDisconnect)
			{
				throw PSTraceSource.NewInvalidOperationException(RunspaceStrings.DisconnectNotSupportedOnServer, new object[0]);
			}
			this.UpdatePoolDisconnectOptions();
			try
			{
				this._runspacePool.BeginDisconnect(null, null);
			}
			catch (InvalidRunspacePoolStateException ex)
			{
				throw ex.ToInvalidRunspaceStateException();
			}
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x000A794C File Offset: 0x000A5B4C
		public override void Connect()
		{
			if (!this.CanConnect)
			{
				throw PSTraceSource.NewInvalidOperationException(RunspaceStrings.CannotConnect, new object[0]);
			}
			this.UpdatePoolDisconnectOptions();
			try
			{
				this._runspacePool.Connect();
			}
			catch (InvalidRunspacePoolStateException ex)
			{
				throw ex.ToInvalidRunspaceStateException();
			}
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x000A79A0 File Offset: 0x000A5BA0
		public override void ConnectAsync()
		{
			if (!this.CanConnect)
			{
				throw PSTraceSource.NewInvalidOperationException(RunspaceStrings.CannotConnect, new object[0]);
			}
			this.UpdatePoolDisconnectOptions();
			try
			{
				this._runspacePool.BeginConnect(null, null);
			}
			catch (InvalidRunspacePoolStateException ex)
			{
				throw ex.ToInvalidRunspaceStateException();
			}
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x000A79F4 File Offset: 0x000A5BF4
		public override Pipeline CreateDisconnectedPipeline()
		{
			if (this.RemoteCommand == null)
			{
				throw PSTraceSource.NewInvalidOperationException(RunspaceStrings.NoDisconnectedCommand, new object[0]);
			}
			return new RemotePipeline(this);
		}

		// Token: 0x06001CDA RID: 7386 RVA: 0x000A7A15 File Offset: 0x000A5C15
		public override PowerShell CreateDisconnectedPowerShell()
		{
			if (this.RemoteCommand == null)
			{
				throw PSTraceSource.NewInvalidOperationException(RunspaceStrings.NoDisconnectedCommand, new object[0]);
			}
			return new PowerShell(this.RemoteCommand, this);
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x000A7A3C File Offset: 0x000A5C3C
		public override RunspaceCapability GetCapabilities()
		{
			RunspaceCapability runspaceCapability = RunspaceCapability.Default;
			if (this.CanDisconnect)
			{
				runspaceCapability |= RunspaceCapability.SupportsDisconnect;
			}
			if (this._connectionInfo is NamedPipeConnectionInfo)
			{
				runspaceCapability |= RunspaceCapability.NamedPipeTransport;
			}
			else
			{
				ContainerConnectionInfo containerConnectionInfo = this._connectionInfo as ContainerConnectionInfo;
				if (containerConnectionInfo != null && containerConnectionInfo.ContainerProc.RuntimeId == Guid.Empty)
				{
					runspaceCapability |= RunspaceCapability.NamedPipeTransport;
				}
			}
			return runspaceCapability;
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x000A7A98 File Offset: 0x000A5C98
		private void UpdatePoolDisconnectOptions()
		{
			WSManConnectionInfo wsmanConnectionInfo = this._runspacePool.ConnectionInfo as WSManConnectionInfo;
			WSManConnectionInfo wsmanConnectionInfo2 = this.ConnectionInfo as WSManConnectionInfo;
			wsmanConnectionInfo.IdleTimeout = wsmanConnectionInfo2.IdleTimeout;
			wsmanConnectionInfo.OutputBufferingMode = wsmanConnectionInfo2.OutputBufferingMode;
		}

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x06001CDD RID: 7389 RVA: 0x000A7ADC File Offset: 0x000A5CDC
		// (remove) Token: 0x06001CDE RID: 7390 RVA: 0x000A7B14 File Offset: 0x000A5D14
		internal event EventHandler<PSEventArgs> RemoteDebuggerStop;

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x06001CDF RID: 7391 RVA: 0x000A7B4C File Offset: 0x000A5D4C
		// (remove) Token: 0x06001CE0 RID: 7392 RVA: 0x000A7B84 File Offset: 0x000A5D84
		internal event EventHandler<PSEventArgs> RemoteDebuggerBreakpointUpdated;

		// Token: 0x06001CE1 RID: 7393 RVA: 0x000A7BB9 File Offset: 0x000A5DB9
		public override Pipeline CreatePipeline()
		{
			return this.CoreCreatePipeline(null, false, false);
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x000A7BC4 File Offset: 0x000A5DC4
		public override Pipeline CreatePipeline(string command)
		{
			if (command == null)
			{
				throw PSTraceSource.NewArgumentNullException("command");
			}
			return this.CoreCreatePipeline(command, false, false);
		}

		// Token: 0x06001CE3 RID: 7395 RVA: 0x000A7BDD File Offset: 0x000A5DDD
		public override Pipeline CreatePipeline(string command, bool addToHistory)
		{
			if (command == null)
			{
				throw PSTraceSource.NewArgumentNullException("command");
			}
			return this.CoreCreatePipeline(command, addToHistory, false);
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x000A7BF6 File Offset: 0x000A5DF6
		public override Pipeline CreateNestedPipeline()
		{
			return this.CoreCreatePipeline(null, false, true);
		}

		// Token: 0x06001CE5 RID: 7397 RVA: 0x000A7C01 File Offset: 0x000A5E01
		public override Pipeline CreateNestedPipeline(string command, bool addToHistory)
		{
			if (command == null)
			{
				throw PSTraceSource.NewArgumentNullException("command");
			}
			return this.CoreCreatePipeline(command, addToHistory, true);
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x000A7C1C File Offset: 0x000A5E1C
		internal void AddToRunningPipelineList(RemotePipeline pipeline)
		{
			lock (this._syncRoot)
			{
				if (!this._bypassRunspaceStateCheck && this._runspaceStateInfo.State != RunspaceState.Opened && this._runspaceStateInfo.State != RunspaceState.Disconnected)
				{
					InvalidRunspaceStateException ex = new InvalidRunspaceStateException(StringUtil.Format(RunspaceStrings.RunspaceNotOpenForPipeline, this._runspaceStateInfo.State.ToString()), this._runspaceStateInfo.State, RunspaceState.Opened);
					if (this.ConnectionInfo != null)
					{
						ex.Source = this.ConnectionInfo.ComputerName;
					}
					throw ex;
				}
				this._runningPipelines.Add(pipeline);
			}
		}

		// Token: 0x06001CE7 RID: 7399 RVA: 0x000A7CD4 File Offset: 0x000A5ED4
		internal void RemoveFromRunningPipelineList(RemotePipeline pipeline)
		{
			lock (this._syncRoot)
			{
				this._runningPipelines.Remove(pipeline);
				pipeline.PipelineFinishedEvent.Set();
			}
		}

		// Token: 0x06001CE8 RID: 7400 RVA: 0x000A7D28 File Offset: 0x000A5F28
		internal void DoConcurrentCheckAndAddToRunningPipelines(RemotePipeline pipeline, bool syncCall)
		{
			lock (this._syncRoot)
			{
				if (this._bSessionStateProxyCallInProgress)
				{
					throw PSTraceSource.NewInvalidOperationException(RunspaceStrings.NoPipelineWhenSessionStateProxyInProgress, new object[0]);
				}
				pipeline.DoConcurrentCheck(syncCall);
				this.AddToRunningPipelineList(pipeline);
			}
		}

		// Token: 0x06001CE9 RID: 7401 RVA: 0x000A7D8C File Offset: 0x000A5F8C
		internal override SessionStateProxy GetSessionStateProxy()
		{
			if (this.sessionStateProxy == null)
			{
				this.sessionStateProxy = new RemoteSessionStateProxy(this);
			}
			return this.sessionStateProxy;
		}

		// Token: 0x06001CEA RID: 7402 RVA: 0x000A7DA8 File Offset: 0x000A5FA8
		private void HandleRunspacePoolStateChanged(object sender, RunspacePoolStateChangedEventArgs e)
		{
			RunspaceState state = (RunspaceState)e.RunspacePoolStateInfo.State;
			RunspaceState runspaceState = this.SetRunspaceState(state, e.RunspacePoolStateInfo.Reason);
			RunspaceState runspaceState2 = state;
			if (runspaceState2 != RunspaceState.Opened)
			{
				if (runspaceState2 == RunspaceState.Disconnected)
				{
					this.UpdateDisconnectExpiresOn();
				}
			}
			else
			{
				RunspaceState runspaceState3 = runspaceState;
				if (runspaceState3 != RunspaceState.Opening)
				{
					if (runspaceState3 == RunspaceState.Connecting)
					{
						this.UpdateDisconnectExpiresOn();
						if (this._applicationPrivateData == null)
						{
							this._applicationPrivateData = this.GetApplicationPrivateData();
							this.SetDebugInfo(this._applicationPrivateData);
						}
					}
				}
				else
				{
					this.SetDebugModeOnOpen();
				}
			}
			this.RaiseRunspaceStateEvents();
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x000A7E2C File Offset: 0x000A602C
		private void SetDebugModeOnOpen()
		{
			this._applicationPrivateData = this.GetApplicationPrivateData();
			if (!this.SetDebugInfo(this._applicationPrivateData))
			{
				return;
			}
			DebugModes debugModes = DebugModes.Default;
			try
			{
				IHostSupportsInteractiveSession hostSupportsInteractiveSession = this._runspacePool.RemoteRunspacePoolInternal.Host as IHostSupportsInteractiveSession;
				if (hostSupportsInteractiveSession != null && hostSupportsInteractiveSession.Runspace != null && hostSupportsInteractiveSession.Runspace.Debugger != null)
				{
					debugModes = hostSupportsInteractiveSession.Runspace.Debugger.DebugMode;
				}
			}
			catch (PSNotImplementedException)
			{
			}
			if ((debugModes & DebugModes.RemoteScript) == DebugModes.RemoteScript)
			{
				try
				{
					this._remoteDebugger.SetDebugMode(debugModes);
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
				}
			}
		}

		// Token: 0x06001CEC RID: 7404 RVA: 0x000A7ED8 File Offset: 0x000A60D8
		private bool SetDebugInfo(PSPrimitiveDictionary psApplicationPrivateData)
		{
			DebugModes? debugMode = null;
			bool inBreakpoint = false;
			int breakpointCount = 0;
			bool breakAll = false;
			UnhandledBreakpointProcessingMode unhandledBreakpointMode = UnhandledBreakpointProcessingMode.Ignore;
			Version serverPSVersion = null;
			if (psApplicationPrivateData != null)
			{
				if (psApplicationPrivateData.ContainsKey("DebugMode"))
				{
					debugMode = new DebugModes?((DebugModes)((int)psApplicationPrivateData["DebugMode"]));
				}
				if (psApplicationPrivateData.ContainsKey("DebugStop"))
				{
					inBreakpoint = (bool)psApplicationPrivateData["DebugStop"];
				}
				if (psApplicationPrivateData.ContainsKey("DebugBreakpointCount"))
				{
					breakpointCount = (int)psApplicationPrivateData["DebugBreakpointCount"];
				}
				if (psApplicationPrivateData.ContainsKey("BreakAll"))
				{
					breakAll = (bool)psApplicationPrivateData["BreakAll"];
				}
				if (psApplicationPrivateData.ContainsKey("UnhandledBreakpointMode"))
				{
					unhandledBreakpointMode = (UnhandledBreakpointProcessingMode)((int)psApplicationPrivateData["UnhandledBreakpointMode"]);
				}
				if (psApplicationPrivateData.ContainsKey("PSVersionTable"))
				{
					PSPrimitiveDictionary psprimitiveDictionary = psApplicationPrivateData["PSVersionTable"] as PSPrimitiveDictionary;
					if (psprimitiveDictionary.ContainsKey("PSVersion"))
					{
						serverPSVersion = (psprimitiveDictionary["PSVersion"] as Version);
					}
				}
			}
			if (debugMode != null)
			{
				this._remoteDebugger = new RemoteDebugger(this);
				this._remoteDebugger.SetClientDebugInfo(debugMode, inBreakpoint, breakpointCount, breakAll, unhandledBreakpointMode, serverPSVersion);
				return true;
			}
			return false;
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x000A8008 File Offset: 0x000A6208
		private void AssertIfStateIsBeforeOpen()
		{
			lock (this._syncRoot)
			{
				if (this._runspaceStateInfo.State != RunspaceState.BeforeOpen)
				{
					InvalidRunspaceStateException ex = new InvalidRunspaceStateException(StringUtil.Format(RunspaceStrings.CannotOpenAgain, new object[]
					{
						this._runspaceStateInfo.State.ToString()
					}), this._runspaceStateInfo.State, RunspaceState.BeforeOpen);
					throw ex;
				}
			}
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x000A8090 File Offset: 0x000A6290
		private RunspaceState SetRunspaceState(RunspaceState state, Exception reason)
		{
			RunspaceState state2;
			lock (this._syncRoot)
			{
				state2 = this._runspaceStateInfo.State;
				if (state != state2)
				{
					this._runspaceStateInfo = new RunspaceStateInfo(state, reason);
					RunspaceAvailability runspaceAvailability = this._runspaceAvailability;
					base.UpdateRunspaceAvailability(this._runspaceStateInfo.State, false);
					this._runspaceEventQueue.Enqueue(new RemoteRunspace.RunspaceEventQueueItem(this._runspaceStateInfo.Clone(), runspaceAvailability, this._runspaceAvailability));
					PSEtwLog.LogOperationalVerbose(PSEventId.RunspaceStateChange, PSOpcode.Open, PSTask.CreateRunspace, PSKeyword.UseAlwaysOperational, new object[]
					{
						state.ToString()
					});
				}
			}
			return state2;
		}

		// Token: 0x06001CEF RID: 7407 RVA: 0x000A8154 File Offset: 0x000A6354
		private void RaiseRunspaceStateEvents()
		{
			Queue<RemoteRunspace.RunspaceEventQueueItem> queue = null;
			EventHandler<RunspaceStateEventArgs> eventHandler = null;
			bool flag = false;
			lock (this._syncRoot)
			{
				eventHandler = this.StateChanged;
				flag = this.HasAvailabilityChangedSubscribers;
				if (eventHandler != null || flag)
				{
					queue = this._runspaceEventQueue;
					this._runspaceEventQueue = new Queue<RemoteRunspace.RunspaceEventQueueItem>();
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
					RemoteRunspace.RunspaceEventQueueItem runspaceEventQueueItem = queue.Dequeue();
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

		// Token: 0x06001CF0 RID: 7408 RVA: 0x000A8230 File Offset: 0x000A6430
		private Pipeline CoreCreatePipeline(string command, bool addToHistory, bool isNested)
		{
			return new RemotePipeline(this, command, addToHistory, isNested);
		}

		// Token: 0x06001CF1 RID: 7409 RVA: 0x000A823C File Offset: 0x000A643C
		private bool WaitForFinishofPipelines()
		{
			RemotePipeline[] array;
			lock (this._syncRoot)
			{
				array = (RemotePipeline[])this._runningPipelines.ToArray(typeof(RemotePipeline));
			}
			if (array.Length > 0)
			{
				WaitHandle[] array2 = new WaitHandle[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = array[i].PipelineFinishedEvent;
				}
				return WaitHandle.WaitAll(array2);
			}
			return true;
		}

		// Token: 0x06001CF2 RID: 7410 RVA: 0x000A82C4 File Offset: 0x000A64C4
		internal override Pipeline GetCurrentlyRunningPipeline()
		{
			Pipeline result;
			lock (this._syncRoot)
			{
				if (this._runningPipelines.Count != 0)
				{
					result = (Pipeline)this._runningPipelines[this._runningPipelines.Count - 1];
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x06001CF3 RID: 7411 RVA: 0x000A8330 File Offset: 0x000A6530
		private void HandleHostCallReceived(object sender, RemoteDataEventArgs<RemoteHostCall> eventArgs)
		{
			ClientMethodExecutor.Dispatch(this._runspacePool.RemoteRunspacePoolInternal.DataStructureHandler.TransportManager, this._runspacePool.RemoteRunspacePoolInternal.Host, null, null, false, this._runspacePool.RemoteRunspacePoolInternal, Guid.Empty, eventArgs.Data);
		}

		// Token: 0x06001CF4 RID: 7412 RVA: 0x000A8380 File Offset: 0x000A6580
		private void HandleURIDirectionReported(object sender, RemoteDataEventArgs<Uri> eventArgs)
		{
			WSManConnectionInfo wsmanConnectionInfo = this._connectionInfo as WSManConnectionInfo;
			if (wsmanConnectionInfo != null)
			{
				wsmanConnectionInfo.ConnectionUri = eventArgs.Data;
				this.URIRedirectionReported.SafeInvoke(this, eventArgs);
			}
		}

		// Token: 0x06001CF5 RID: 7413 RVA: 0x000A83B8 File Offset: 0x000A65B8
		private void HandleRunspacePoolForwardEvent(object sender, PSEventArgs e)
		{
			if (e.SourceIdentifier.Equals("PSInternalRemoteDebuggerStopEvent"))
			{
				this.RemoteDebuggerStop.SafeInvoke(this, e);
				return;
			}
			if (e.SourceIdentifier.Equals("PSInternalRemoteDebuggerBreakpointUpdatedEvent"))
			{
				this.RemoteDebuggerBreakpointUpdated.SafeInvoke(this, e);
				return;
			}
			this._eventManager.AddForwardedEvent(e);
		}

		// Token: 0x06001CF6 RID: 7414 RVA: 0x000A8414 File Offset: 0x000A6614
		private void HandleSessionCreateCompleted(object sender, CreateCompleteEventArgs eventArgs)
		{
			if (eventArgs != null)
			{
				this._connectionInfo.IdleTimeout = eventArgs.ConnectionInfo.IdleTimeout;
				this._connectionInfo.MaxIdleTimeout = eventArgs.ConnectionInfo.MaxIdleTimeout;
				WSManConnectionInfo wsmanConnectionInfo = this._connectionInfo as WSManConnectionInfo;
				if (wsmanConnectionInfo != null)
				{
					wsmanConnectionInfo.OutputBufferingMode = ((WSManConnectionInfo)eventArgs.ConnectionInfo).OutputBufferingMode;
				}
			}
		}

		// Token: 0x06001CF7 RID: 7415 RVA: 0x000A8478 File Offset: 0x000A6678
		private void UpdateDisconnectExpiresOn()
		{
			WSManConnectionInfo wsmanConnectionInfo = this._runspacePool.RemoteRunspacePoolInternal.ConnectionInfo as WSManConnectionInfo;
			if (wsmanConnectionInfo != null)
			{
				base.DisconnectedOn = wsmanConnectionInfo.DisconnectedOn;
				base.ExpiresOn = wsmanConnectionInfo.ExpiresOn;
			}
		}

		// Token: 0x06001CF8 RID: 7416 RVA: 0x000A84B6 File Offset: 0x000A66B6
		internal bool IsAnotherInvokeCommandExecuting(InvokeCommandCommand invokeCommand, long localPipelineId)
		{
			if (this.currentLocalPipelineId != localPipelineId && this.currentLocalPipelineId != 0L)
			{
				return false;
			}
			if (this.currentInvokeCommand == null)
			{
				this.SetCurrentInvokeCommand(invokeCommand, localPipelineId);
				return false;
			}
			return !this.currentInvokeCommand.Equals(invokeCommand);
		}

		// Token: 0x06001CF9 RID: 7417 RVA: 0x000A84F0 File Offset: 0x000A66F0
		internal void SetCurrentInvokeCommand(InvokeCommandCommand invokeCommand, long localPipelineId)
		{
			this.currentInvokeCommand = invokeCommand;
			this.currentLocalPipelineId = localPipelineId;
		}

		// Token: 0x06001CFA RID: 7418 RVA: 0x000A8500 File Offset: 0x000A6700
		internal void ClearInvokeCommand()
		{
			this.currentLocalPipelineId = 0L;
			this.currentInvokeCommand = null;
		}

		// Token: 0x06001CFB RID: 7419 RVA: 0x000A8514 File Offset: 0x000A6714
		internal void AbortOpen()
		{
			NamedPipeClientSessionTransportManager namedPipeClientSessionTransportManager = this._runspacePool.RemoteRunspacePoolInternal.DataStructureHandler.TransportManager as NamedPipeClientSessionTransportManager;
			if (namedPipeClientSessionTransportManager != null)
			{
				namedPipeClientSessionTransportManager.AbortConnect();
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06001CFC RID: 7420 RVA: 0x000A8545 File Offset: 0x000A6745
		internal RunspacePool RunspacePool
		{
			get
			{
				return this._runspacePool;
			}
		}

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x06001CFD RID: 7421 RVA: 0x000A8550 File Offset: 0x000A6750
		// (remove) Token: 0x06001CFE RID: 7422 RVA: 0x000A8588 File Offset: 0x000A6788
		internal event EventHandler<RemoteDataEventArgs<Uri>> URIRedirectionReported;

		// Token: 0x06001CFF RID: 7423 RVA: 0x000A85C0 File Offset: 0x000A67C0
		public override PSPrimitiveDictionary GetApplicationPrivateData()
		{
			PSPrimitiveDictionary applicationPrivateData;
			try
			{
				applicationPrivateData = this._runspacePool.GetApplicationPrivateData();
			}
			catch (InvalidRunspacePoolStateException ex)
			{
				throw ex.ToInvalidRunspaceStateException();
			}
			return applicationPrivateData;
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x000A85F4 File Offset: 0x000A67F4
		internal override void SetApplicationPrivateData(PSPrimitiveDictionary applicationPrivateData)
		{
		}

		// Token: 0x04000CBD RID: 3261
		private RunspacePool _runspacePool;

		// Token: 0x04000CBE RID: 3262
		private ArrayList _runningPipelines = new ArrayList();

		// Token: 0x04000CBF RID: 3263
		private object _syncRoot = new object();

		// Token: 0x04000CC0 RID: 3264
		private RunspaceStateInfo _runspaceStateInfo = new RunspaceStateInfo(RunspaceState.BeforeOpen);

		// Token: 0x04000CC1 RID: 3265
		private Version _version = PSVersionInfo.PSVersion;

		// Token: 0x04000CC2 RID: 3266
		private bool _bSessionStateProxyCallInProgress;

		// Token: 0x04000CC3 RID: 3267
		private RunspaceConnectionInfo _connectionInfo;

		// Token: 0x04000CC4 RID: 3268
		private RunspaceConnectionInfo _originalConnectionInfo;

		// Token: 0x04000CC5 RID: 3269
		private RemoteDebugger _remoteDebugger;

		// Token: 0x04000CC6 RID: 3270
		private PSPrimitiveDictionary _applicationPrivateData;

		// Token: 0x04000CC7 RID: 3271
		private bool _disposed;

		// Token: 0x04000CC8 RID: 3272
		private InvokeCommandCommand currentInvokeCommand;

		// Token: 0x04000CC9 RID: 3273
		private long currentLocalPipelineId;

		// Token: 0x04000CCA RID: 3274
		private Queue<RemoteRunspace.RunspaceEventQueueItem> _runspaceEventQueue = new Queue<RemoteRunspace.RunspaceEventQueueItem>();

		// Token: 0x04000CCB RID: 3275
		private bool _bypassRunspaceStateCheck;

		// Token: 0x04000CCC RID: 3276
		private bool _shouldCloseOnPop;

		// Token: 0x04000CCD RID: 3277
		private PSThreadOptions createThreadOptions;

		// Token: 0x04000CCE RID: 3278
		private RunspaceAvailability _runspaceAvailability;

		// Token: 0x04000CD1 RID: 3281
		private PSRemoteEventManager _eventManager;

		// Token: 0x04000CD2 RID: 3282
		private int id = -1;

		// Token: 0x04000CD5 RID: 3285
		private RemoteSessionStateProxy sessionStateProxy;

		// Token: 0x02000266 RID: 614
		protected class RunspaceEventQueueItem
		{
			// Token: 0x06001D01 RID: 7425 RVA: 0x000A85F6 File Offset: 0x000A67F6
			public RunspaceEventQueueItem(RunspaceStateInfo runspaceStateInfo, RunspaceAvailability currentAvailability, RunspaceAvailability newAvailability)
			{
				this.RunspaceStateInfo = runspaceStateInfo;
				this.CurrentRunspaceAvailability = currentAvailability;
				this.NewRunspaceAvailability = newAvailability;
			}

			// Token: 0x04000CD7 RID: 3287
			public RunspaceStateInfo RunspaceStateInfo;

			// Token: 0x04000CD8 RID: 3288
			public RunspaceAvailability CurrentRunspaceAvailability;

			// Token: 0x04000CD9 RID: 3289
			public RunspaceAvailability NewRunspaceAvailability;
		}
	}
}
