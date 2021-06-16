using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Tracing;
using System.Threading;
using System.Xml;

namespace System.Management.Automation.Runspaces.Internal
{
	// Token: 0x0200029F RID: 671
	internal class RemoteRunspacePoolInternal : RunspacePoolInternal, IDisposable
	{
		// Token: 0x0600200B RID: 8203 RVA: 0x000B990C File Offset: 0x000B7B0C
		internal RemoteRunspacePoolInternal(int minRunspaces, int maxRunspaces, TypeTable typeTable, PSHost host, PSPrimitiveDictionary applicationArguments, RunspaceConnectionInfo connectionInfo, string name = null) : base(minRunspaces, maxRunspaces)
		{
			if (connectionInfo == null)
			{
				throw PSTraceSource.NewArgumentNullException("WSManConnectionInfo");
			}
			PSEtwLog.LogOperationalVerbose(PSEventId.RunspacePoolConstructor, PSOpcode.Constructor, PSTask.CreateRunspace, PSKeyword.UseAlwaysOperational, new object[]
			{
				this.instanceId.ToString(),
				this.minPoolSz.ToString(CultureInfo.InvariantCulture),
				this.maxPoolSz.ToString(CultureInfo.InvariantCulture)
			});
			this.connectionInfo = connectionInfo.InternalCopy();
			this.host = host;
			this.applicationArguments = applicationArguments;
			this.availableForConnection = false;
			this.dispatchTable = new DispatchTable<object>();
			this._runningPowerShells = new ConcurrentStack<PowerShell>();
			if (!string.IsNullOrEmpty(name))
			{
				this.Name = name;
			}
			this.CreateDSHandler(typeTable);
		}

		// Token: 0x0600200C RID: 8204 RVA: 0x000B99F4 File Offset: 0x000B7BF4
		internal RemoteRunspacePoolInternal(Guid instanceId, string name, bool isDisconnected, ConnectCommandInfo[] connectCommands, RunspaceConnectionInfo connectionInfo, PSHost host, TypeTable typeTable) : base(1, 1)
		{
			if (connectCommands == null)
			{
				throw PSTraceSource.NewArgumentNullException("ConnectCommandInfo[]");
			}
			if (connectionInfo == null)
			{
				throw PSTraceSource.NewArgumentNullException("WSManConnectionInfo");
			}
			if (connectionInfo is WSManConnectionInfo)
			{
				this.connectionInfo = connectionInfo.InternalCopy();
			}
			this.instanceId = instanceId;
			this.minPoolSz = -1;
			this.maxPoolSz = -1;
			PSEtwLog.LogOperationalVerbose(PSEventId.RunspacePoolConstructor, PSOpcode.Constructor, PSTask.CreateRunspace, PSKeyword.UseAlwaysOperational, new object[]
			{
				instanceId.ToString(),
				this.minPoolSz.ToString(CultureInfo.InvariantCulture),
				this.maxPoolSz.ToString(CultureInfo.InvariantCulture)
			});
			this.connectCommands = connectCommands;
			this.Name = name;
			this.host = host;
			this.dispatchTable = new DispatchTable<object>();
			this._runningPowerShells = new ConcurrentStack<PowerShell>();
			this.SetRunspacePoolState(new RunspacePoolStateInfo(RunspacePoolState.Disconnected, null));
			this.CreateDSHandler(typeTable);
			this.availableForConnection = isDisconnected;
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x000B9B08 File Offset: 0x000B7D08
		private void CreateDSHandler(TypeTable typeTable)
		{
			this.dataStructureHandler = new ClientRunspacePoolDataStructureHandler(this, typeTable);
			this.dataStructureHandler.RemoteHostCallReceived += this.HandleRemoteHostCalls;
			this.dataStructureHandler.StateInfoReceived += this.HandleStateInfoReceived;
			this.dataStructureHandler.RSPoolInitInfoReceived += this.HandleInitInfoReceived;
			this.dataStructureHandler.ApplicationPrivateDataReceived += this.HandleApplicationPrivateDataReceived;
			this.dataStructureHandler.SessionClosing += this.HandleSessionClosing;
			this.dataStructureHandler.SessionClosed += this.HandleSessionClosed;
			this.dataStructureHandler.SetMaxMinRunspacesResponseRecieved += this.HandleResponseReceived;
			this.dataStructureHandler.URIRedirectionReported += this.HandleURIDirectionReported;
			this.dataStructureHandler.PSEventArgsReceived += this.HandlePSEventArgsReceived;
			this.dataStructureHandler.SessionDisconnected += this.HandleSessionDisconnected;
			this.dataStructureHandler.SessionReconnected += this.HandleSessionReconnected;
			this.dataStructureHandler.SessionRCDisconnecting += this.HandleSessionRCDisconnecting;
			this.dataStructureHandler.SessionCreateCompleted += this.HandleSessionCreateCompleted;
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x0600200E RID: 8206 RVA: 0x000B9C4D File Offset: 0x000B7E4D
		public override RunspaceConnectionInfo ConnectionInfo
		{
			get
			{
				return this.connectionInfo;
			}
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x0600200F RID: 8207 RVA: 0x000B9C55 File Offset: 0x000B7E55
		internal ClientRunspacePoolDataStructureHandler DataStructureHandler
		{
			get
			{
				return this.dataStructureHandler;
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06002010 RID: 8208 RVA: 0x000B9C5D File Offset: 0x000B7E5D
		// (set) Token: 0x06002011 RID: 8209 RVA: 0x000B9C65 File Offset: 0x000B7E65
		internal ConnectCommandInfo[] ConnectCommands
		{
			get
			{
				return this.connectCommands;
			}
			set
			{
				this.connectCommands = value;
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06002012 RID: 8210 RVA: 0x000B9C6E File Offset: 0x000B7E6E
		// (set) Token: 0x06002013 RID: 8211 RVA: 0x000B9C76 File Offset: 0x000B7E76
		internal string Name
		{
			get
			{
				return this.friendlyName;
			}
			set
			{
				if (value == null)
				{
					this.friendlyName = string.Empty;
					return;
				}
				this.friendlyName = value;
			}
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06002014 RID: 8212 RVA: 0x000B9C8E File Offset: 0x000B7E8E
		internal bool AvailableForConnection
		{
			get
			{
				return this.availableForConnection;
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06002015 RID: 8213 RVA: 0x000B9C96 File Offset: 0x000B7E96
		internal int MaxRetryConnectionTime
		{
			get
			{
				if (this.dataStructureHandler == null)
				{
					return 0;
				}
				return this.dataStructureHandler.MaxRetryConnectionTime;
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06002016 RID: 8214 RVA: 0x000B9CB0 File Offset: 0x000B7EB0
		public override RunspacePoolAvailability RunspacePoolAvailability
		{
			get
			{
				RunspacePoolAvailability result;
				if (this.stateInfo.State == RunspacePoolState.Disconnected)
				{
					result = (this.AvailableForConnection ? RunspacePoolAvailability.None : RunspacePoolAvailability.Busy);
				}
				else
				{
					result = base.RunspacePoolAvailability;
				}
				return result;
			}
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x06002018 RID: 8216 RVA: 0x000B9CEB File Offset: 0x000B7EEB
		// (set) Token: 0x06002017 RID: 8215 RVA: 0x000B9CE2 File Offset: 0x000B7EE2
		internal bool IsRemoteDebugStop { get; set; }

		// Token: 0x06002019 RID: 8217 RVA: 0x000B9CF4 File Offset: 0x000B7EF4
		internal override bool ResetRunspaceState()
		{
			Version psremotingProtocolVersion = this.PSRemotingProtocolVersion;
			if (psremotingProtocolVersion == null || psremotingProtocolVersion < RemotingConstants.ProtocolVersionWin10RTM)
			{
				throw PSTraceSource.NewInvalidOperationException(RunspacePoolStrings.ResetRunspaceStateNotSupportedOnServer, new object[0]);
			}
			long callId = 0L;
			lock (this.syncObject)
			{
				callId = this.DispatchTable.CreateNewCallId();
				this.dataStructureHandler.SendResetRunspaceStateToServer(callId);
			}
			object response = this.DispatchTable.GetResponse(callId, false);
			return (bool)response;
		}

		// Token: 0x0600201A RID: 8218 RVA: 0x000B9D94 File Offset: 0x000B7F94
		internal override bool SetMaxRunspaces(int maxRunspaces)
		{
			bool flag = false;
			long callId = 0L;
			lock (this.syncObject)
			{
				if (maxRunspaces < this.minPoolSz || maxRunspaces == this.maxPoolSz || this.stateInfo.State == RunspacePoolState.Closed || this.stateInfo.State == RunspacePoolState.Closing || this.stateInfo.State == RunspacePoolState.Broken)
				{
					return false;
				}
				if (this.stateInfo.State == RunspacePoolState.BeforeOpen || this.stateInfo.State == RunspacePoolState.Disconnected)
				{
					this.maxPoolSz = maxRunspaces;
					return true;
				}
				callId = this.DispatchTable.CreateNewCallId();
				this.dataStructureHandler.SendSetMaxRunspacesToServer(maxRunspaces, callId);
			}
			object response = this.DispatchTable.GetResponse(callId, false);
			flag = (bool)response;
			if (flag)
			{
				lock (this.syncObject)
				{
					this.maxPoolSz = maxRunspaces;
				}
			}
			return flag;
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x000B9EB0 File Offset: 0x000B80B0
		internal override bool SetMinRunspaces(int minRunspaces)
		{
			bool flag = false;
			long callId = 0L;
			lock (this.syncObject)
			{
				if (minRunspaces < 1 || minRunspaces > this.maxPoolSz || minRunspaces == this.minPoolSz || this.stateInfo.State == RunspacePoolState.Closed || this.stateInfo.State == RunspacePoolState.Closing || this.stateInfo.State == RunspacePoolState.Broken)
				{
					return false;
				}
				if (this.stateInfo.State == RunspacePoolState.BeforeOpen || this.stateInfo.State == RunspacePoolState.Disconnected)
				{
					this.minPoolSz = minRunspaces;
					return true;
				}
				callId = this.DispatchTable.CreateNewCallId();
				this.dataStructureHandler.SendSetMinRunspacesToServer(minRunspaces, callId);
			}
			object response = this.DispatchTable.GetResponse(callId, false);
			flag = (bool)response;
			if (flag)
			{
				lock (this.syncObject)
				{
					this.minPoolSz = minRunspaces;
				}
			}
			return flag;
		}

		// Token: 0x0600201C RID: 8220 RVA: 0x000B9FD0 File Offset: 0x000B81D0
		internal override int GetAvailableRunspaces()
		{
			long callId = 0L;
			lock (this.syncObject)
			{
				if (this.stateInfo.State == RunspacePoolState.Opened)
				{
					callId = this.DispatchTable.CreateNewCallId();
					this.dataStructureHandler.SendGetAvailableRunspacesToServer(callId);
				}
				else
				{
					if (this.stateInfo.State != RunspacePoolState.BeforeOpen && this.stateInfo.State != RunspacePoolState.Opening)
					{
						throw new InvalidOperationException(HostInterfaceExceptionsStrings.RunspacePoolNotOpened);
					}
					return this.maxPoolSz;
				}
			}
			object response = this.dispatchTable.GetResponse(callId, 0);
			return (int)response;
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x000BA088 File Offset: 0x000B8288
		internal void HandleApplicationPrivateDataReceived(object sender, RemoteDataEventArgs<PSPrimitiveDictionary> eventArgs)
		{
			this.SetApplicationPrivateData(eventArgs.Data);
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x000BA098 File Offset: 0x000B8298
		internal void HandleInitInfoReceived(object sender, RemoteDataEventArgs<RunspacePoolInitInfo> eventArgs)
		{
			RunspacePoolStateInfo runspacePoolStateInfo = new RunspacePoolStateInfo(RunspacePoolState.Opened, null);
			bool flag = false;
			lock (this.syncObject)
			{
				this.minPoolSz = eventArgs.Data.MinRunspaces;
				this.maxPoolSz = eventArgs.Data.MaxRunspaces;
				if (this.stateInfo.State == RunspacePoolState.Connecting)
				{
					this.ResetDisconnectedOnExpiresOn();
					flag = true;
					this.SetRunspacePoolState(runspacePoolStateInfo);
				}
			}
			if (flag)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.WaitAndRaiseConnectEventsProc), runspacePoolStateInfo);
			}
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x000BA134 File Offset: 0x000B8334
		internal void HandleStateInfoReceived(object sender, RemoteDataEventArgs<RunspacePoolStateInfo> eventArgs)
		{
			RunspacePoolStateInfo data = eventArgs.Data;
			bool flag = false;
			if (data.State == RunspacePoolState.Opened)
			{
				lock (this.syncObject)
				{
					if (this.stateInfo.State == RunspacePoolState.Opening)
					{
						this.SetRunspacePoolState(data);
						flag = true;
					}
				}
				if (flag)
				{
					base.RaiseStateChangeEvent(this.stateInfo);
					this.SetOpenAsCompleted();
					return;
				}
			}
			else if (data.State == RunspacePoolState.Closed || data.State == RunspacePoolState.Broken)
			{
				bool flag3 = false;
				lock (this.syncObject)
				{
					if (this.stateInfo.State == RunspacePoolState.Closed || this.stateInfo.State == RunspacePoolState.Broken)
					{
						return;
					}
					if (this.stateInfo.State == RunspacePoolState.Opening || this.stateInfo.State == RunspacePoolState.Opened || this.stateInfo.State == RunspacePoolState.Closing)
					{
						flag3 = true;
						this.SetRunspacePoolState(data);
					}
				}
				if (flag3 && this.closeAsyncResult == null)
				{
					this.dataStructureHandler.CloseRunspacePoolAsync();
				}
			}
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x000BA260 File Offset: 0x000B8460
		internal void HandleRemoteHostCalls(object sender, RemoteDataEventArgs<RemoteHostCall> eventArgs)
		{
			if (this.HostCallReceived != null)
			{
				this.HostCallReceived.SafeInvoke(sender, eventArgs);
				return;
			}
			RemoteHostCall data = eventArgs.Data;
			if (data.IsVoidMethod)
			{
				data.ExecuteVoidMethod(this.host);
				return;
			}
			RemoteHostResponse hostResponse = data.ExecuteNonVoidMethod(this.host);
			this.dataStructureHandler.SendHostResponseToServer(hostResponse);
		}

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06002021 RID: 8225 RVA: 0x000BA2B8 File Offset: 0x000B84B8
		internal PSHost Host
		{
			get
			{
				return this.host;
			}
		}

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06002022 RID: 8226 RVA: 0x000BA2C0 File Offset: 0x000B84C0
		internal PSPrimitiveDictionary ApplicationArguments
		{
			get
			{
				return this.applicationArguments;
			}
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x000BA2C8 File Offset: 0x000B84C8
		internal override PSPrimitiveDictionary GetApplicationPrivateData()
		{
			if (base.RunspacePoolStateInfo.State == RunspacePoolState.Disconnected && !this.applicationPrivateDataReceived.WaitOne(0))
			{
				return null;
			}
			return this.applicationPrivateData;
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x000BA2F0 File Offset: 0x000B84F0
		internal void SetApplicationPrivateData(PSPrimitiveDictionary applicationPrivateData)
		{
			lock (this.syncObject)
			{
				if (!this.applicationPrivateDataReceived.WaitOne(0))
				{
					this.applicationPrivateData = applicationPrivateData;
					this.applicationPrivateDataReceived.Set();
					foreach (Runspace runspace in this.runspaceList)
					{
						runspace.SetApplicationPrivateData(applicationPrivateData);
					}
				}
			}
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x000BA390 File Offset: 0x000B8590
		internal override void PropagateApplicationPrivateData(Runspace runspace)
		{
			if (this.applicationPrivateDataReceived.WaitOne(0))
			{
				runspace.SetApplicationPrivateData(this.GetApplicationPrivateData());
			}
		}

		// Token: 0x14000048 RID: 72
		// (add) Token: 0x06002026 RID: 8230 RVA: 0x000BA3AC File Offset: 0x000B85AC
		// (remove) Token: 0x06002027 RID: 8231 RVA: 0x000BA3E4 File Offset: 0x000B85E4
		internal event EventHandler<RemoteDataEventArgs<RemoteHostCall>> HostCallReceived;

		// Token: 0x14000049 RID: 73
		// (add) Token: 0x06002028 RID: 8232 RVA: 0x000BA41C File Offset: 0x000B861C
		// (remove) Token: 0x06002029 RID: 8233 RVA: 0x000BA454 File Offset: 0x000B8654
		internal event EventHandler<RemoteDataEventArgs<Uri>> URIRedirectionReported;

		// Token: 0x1400004A RID: 74
		// (add) Token: 0x0600202A RID: 8234 RVA: 0x000BA48C File Offset: 0x000B868C
		// (remove) Token: 0x0600202B RID: 8235 RVA: 0x000BA4C4 File Offset: 0x000B86C4
		internal event EventHandler<CreateCompleteEventArgs> SessionCreateCompleted;

		// Token: 0x0600202C RID: 8236 RVA: 0x000BA4F9 File Offset: 0x000B86F9
		internal void CreatePowerShellOnServerAndInvoke(ClientRemotePowerShell shell)
		{
			this.dataStructureHandler.CreatePowerShellOnServerAndInvoke(shell);
			if (!shell.NoInput)
			{
				shell.SendInput();
			}
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x000BA515 File Offset: 0x000B8715
		internal void AddRemotePowerShellDSHandler(Guid psShellInstanceId, ClientPowerShellDataStructureHandler psDSHandler)
		{
			this.dataStructureHandler.AddRemotePowerShellDSHandler(psShellInstanceId, psDSHandler);
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x0600202E RID: 8238 RVA: 0x000BA524 File Offset: 0x000B8724
		internal bool CanDisconnect
		{
			get
			{
				Version psremotingProtocolVersion = this.PSRemotingProtocolVersion;
				return psremotingProtocolVersion != null && this.dataStructureHandler != null && psremotingProtocolVersion >= RemotingConstants.ProtocolVersionWin8RTM && this.dataStructureHandler.EndpointSupportsDisconnect;
			}
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x0600202F RID: 8239 RVA: 0x000BA568 File Offset: 0x000B8768
		internal Version PSRemotingProtocolVersion
		{
			get
			{
				Version result = null;
				PSPrimitiveDictionary psprimitiveDictionary = this.GetApplicationPrivateData();
				if (psprimitiveDictionary != null)
				{
					PSPrimitiveDictionary.TryPathGet<Version>(psprimitiveDictionary, out result, new string[]
					{
						"PSVersionTable",
						"PSRemotingProtocolVersion"
					});
				}
				return result;
			}
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x000BA5A3 File Offset: 0x000B87A3
		internal void PushRunningPowerShell(PowerShell ps)
		{
			this._runningPowerShells.Push(ps);
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x000BA5B4 File Offset: 0x000B87B4
		internal PowerShell PopRunningPowerShell()
		{
			PowerShell result;
			if (this._runningPowerShells.TryPop(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x000BA5D4 File Offset: 0x000B87D4
		internal PowerShell GetCurrentRunningPowerShell()
		{
			PowerShell result;
			if (this._runningPowerShells.TryPeek(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x000BA5F4 File Offset: 0x000B87F4
		protected override IAsyncResult CoreOpen(bool isAsync, AsyncCallback callback, object asyncState)
		{
			PSEtwLog.SetActivityIdForCurrentThread(base.InstanceId);
			PSEtwLog.LogOperationalVerbose(PSEventId.RunspacePoolOpen, PSOpcode.Open, PSTask.CreateRunspace, PSKeyword.UseAlwaysOperational, new object[0]);
			lock (this.syncObject)
			{
				base.AssertIfStateIsBeforeOpen();
				this.stateInfo = new RunspacePoolStateInfo(RunspacePoolState.Opening, null);
			}
			base.RaiseStateChangeEvent(this.stateInfo);
			RunspacePoolAsyncResult result = new RunspacePoolAsyncResult(this.instanceId, callback, asyncState, true);
			this.openAsyncResult = result;
			this.dataStructureHandler.CreateRunspacePoolAndOpenAsync();
			return result;
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x000BA698 File Offset: 0x000B8898
		public override void Open()
		{
			IAsyncResult asyncResult = base.BeginOpen(null, null);
			base.EndOpen(asyncResult);
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x000BA6B8 File Offset: 0x000B88B8
		public override void Close()
		{
			IAsyncResult asyncResult = this.BeginClose(null, null);
			this.EndClose(asyncResult);
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x000BA6D8 File Offset: 0x000B88D8
		public override IAsyncResult BeginClose(AsyncCallback callback, object asyncState)
		{
			bool flag = false;
			bool flag2 = false;
			RunspacePoolStateInfo stateInfo = new RunspacePoolStateInfo(RunspacePoolState.BeforeOpen, null);
			RunspacePoolAsyncResult runspacePoolAsyncResult = null;
			lock (this.syncObject)
			{
				if (this.stateInfo.State == RunspacePoolState.Closed || this.stateInfo.State == RunspacePoolState.Broken)
				{
					flag2 = true;
					runspacePoolAsyncResult = new RunspacePoolAsyncResult(this.instanceId, callback, asyncState, false);
				}
				else if (this.stateInfo.State == RunspacePoolState.BeforeOpen)
				{
					stateInfo = (this.stateInfo = new RunspacePoolStateInfo(RunspacePoolState.Closed, null));
					flag = true;
					flag2 = true;
					this.closeAsyncResult = null;
					runspacePoolAsyncResult = new RunspacePoolAsyncResult(this.instanceId, callback, asyncState, false);
				}
				else if (this.stateInfo.State == RunspacePoolState.Opened || this.stateInfo.State == RunspacePoolState.Opening)
				{
					stateInfo = (this.stateInfo = new RunspacePoolStateInfo(RunspacePoolState.Closing, null));
					this.closeAsyncResult = new RunspacePoolAsyncResult(this.instanceId, callback, asyncState, false);
					runspacePoolAsyncResult = this.closeAsyncResult;
					flag = true;
				}
				else if (this.stateInfo.State == RunspacePoolState.Disconnected || this.stateInfo.State == RunspacePoolState.Disconnecting || this.stateInfo.State == RunspacePoolState.Connecting)
				{
					this.closeAsyncResult = new RunspacePoolAsyncResult(this.instanceId, callback, asyncState, false);
					runspacePoolAsyncResult = this.closeAsyncResult;
				}
				else if (this.stateInfo.State == RunspacePoolState.Closing)
				{
					return this.closeAsyncResult;
				}
			}
			if (flag)
			{
				base.RaiseStateChangeEvent(stateInfo);
			}
			if (!flag2)
			{
				this.dataStructureHandler.CloseRunspacePoolAsync();
			}
			else
			{
				runspacePoolAsyncResult.SetAsCompleted(null);
			}
			return runspacePoolAsyncResult;
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x000BA874 File Offset: 0x000B8A74
		public override void Disconnect()
		{
			IAsyncResult asyncResult = this.BeginDisconnect(null, null);
			this.EndDisconnect(asyncResult);
		}

		// Token: 0x06002038 RID: 8248 RVA: 0x000BA894 File Offset: 0x000B8A94
		public override IAsyncResult BeginDisconnect(AsyncCallback callback, object state)
		{
			if (!this.CanDisconnect)
			{
				throw PSTraceSource.NewInvalidOperationException(RunspacePoolStrings.DisconnectNotSupportedOnServer, new object[0]);
			}
			bool flag = false;
			RunspacePoolState state2;
			lock (this.syncObject)
			{
				state2 = this.stateInfo.State;
				if (state2 == RunspacePoolState.Opened)
				{
					RunspacePoolStateInfo runspacePoolState = new RunspacePoolStateInfo(RunspacePoolState.Disconnecting, null);
					this.SetRunspacePoolState(runspacePoolState);
					flag = true;
				}
			}
			if (flag)
			{
				base.RaiseStateChangeEvent(this.stateInfo);
			}
			if (state2 == RunspacePoolState.Opened)
			{
				RunspacePoolAsyncResult result = new RunspacePoolAsyncResult(this.instanceId, callback, state, false);
				this.disconnectAsyncResult = result;
				this.dataStructureHandler.DisconnectPoolAsync();
				return result;
			}
			string message = StringUtil.Format(RunspacePoolStrings.InvalidRunspacePoolState, RunspacePoolState.Opened, this.stateInfo.State);
			InvalidRunspacePoolStateException ex = new InvalidRunspacePoolStateException(message, this.stateInfo.State, RunspacePoolState.Opened);
			throw ex;
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x000BA980 File Offset: 0x000B8B80
		public override void EndDisconnect(IAsyncResult asyncResult)
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
					"BeginOpen"
				});
			}
			runspacePoolAsyncResult.EndInvoke();
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x000BA9F0 File Offset: 0x000B8BF0
		public override void Connect()
		{
			IAsyncResult asyncResult = this.BeginConnect(null, null);
			this.EndConnect(asyncResult);
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x000BAA10 File Offset: 0x000B8C10
		public override IAsyncResult BeginConnect(AsyncCallback callback, object state)
		{
			if (!this.AvailableForConnection)
			{
				throw PSTraceSource.NewInvalidOperationException(RunspacePoolStrings.CannotConnect, new object[0]);
			}
			bool flag = false;
			RunspacePoolState state2;
			lock (this.syncObject)
			{
				state2 = this.stateInfo.State;
				if (state2 == RunspacePoolState.Disconnected)
				{
					RunspacePoolStateInfo runspacePoolState = new RunspacePoolStateInfo(RunspacePoolState.Connecting, null);
					this.SetRunspacePoolState(runspacePoolState);
					flag = true;
				}
			}
			if (flag)
			{
				base.RaiseStateChangeEvent(this.stateInfo);
			}
			flag = false;
			if (state2 == RunspacePoolState.Disconnected)
			{
				RunspacePoolAsyncResult result = new RunspacePoolAsyncResult(this.instanceId, callback, state, false);
				if (this.canReconnect)
				{
					this.reconnectAsyncResult = result;
					this.dataStructureHandler.ReconnectPoolAsync();
				}
				else
				{
					this.openAsyncResult = result;
					this.dataStructureHandler.ConnectPoolAsync();
				}
				if (flag)
				{
					base.RaiseStateChangeEvent(this.stateInfo);
				}
				return result;
			}
			string message = StringUtil.Format(RunspacePoolStrings.InvalidRunspacePoolState, RunspacePoolState.Disconnected, this.stateInfo.State);
			InvalidRunspacePoolStateException ex = new InvalidRunspacePoolStateException(message, this.stateInfo.State, RunspacePoolState.Disconnected);
			throw ex;
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x000BAB28 File Offset: 0x000B8D28
		public override void EndConnect(IAsyncResult asyncResult)
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
					"BeginOpen"
				});
			}
			runspacePoolAsyncResult.EndInvoke();
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x000BAB98 File Offset: 0x000B8D98
		public override Collection<PowerShell> CreateDisconnectedPowerShells(RunspacePool runspacePool)
		{
			Collection<PowerShell> collection = new Collection<PowerShell>();
			if (this.ConnectCommands == null)
			{
				string message = StringUtil.Format(RunspacePoolStrings.CannotReconstructCommands, this.Name);
				throw new InvalidRunspacePoolStateException(message);
			}
			foreach (ConnectCommandInfo connectCmdInfo in this.ConnectCommands)
			{
				collection.Add(new PowerShell(connectCmdInfo, runspacePool));
			}
			return collection;
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x000BABF8 File Offset: 0x000B8DF8
		public override RunspacePoolCapability GetCapabilities()
		{
			RunspacePoolCapability runspacePoolCapability = RunspacePoolCapability.Default;
			if (this.CanDisconnect)
			{
				runspacePoolCapability |= RunspacePoolCapability.SupportsDisconnect;
			}
			return runspacePoolCapability;
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x000BAC14 File Offset: 0x000B8E14
		internal static RunspacePool[] GetRemoteRunspacePools(RunspaceConnectionInfo connectionInfo, PSHost host, TypeTable typeTable)
		{
			WSManConnectionInfo wsmanConnectionInfo = connectionInfo as WSManConnectionInfo;
			if (wsmanConnectionInfo == null)
			{
				throw new NotSupportedException();
			}
			List<RunspacePool> list = new List<RunspacePool>();
			Collection<PSObject> remotePools = RemoteRunspacePoolEnumeration.GetRemotePools(wsmanConnectionInfo);
			foreach (PSObject psobject in remotePools)
			{
				WSManConnectionInfo wsmanConnectionInfo2 = wsmanConnectionInfo.Copy();
				PSPropertyInfo pspropertyInfo = psobject.Properties["ShellId"];
				PSPropertyInfo pspropertyInfo2 = psobject.Properties["State"];
				PSPropertyInfo pspropertyInfo3 = psobject.Properties["Name"];
				PSPropertyInfo pspropertyInfo4 = psobject.Properties["ResourceUri"];
				if (pspropertyInfo != null && pspropertyInfo2 != null && pspropertyInfo3 != null && pspropertyInfo4 != null)
				{
					string name = pspropertyInfo3.Value.ToString();
					string text = pspropertyInfo4.Value.ToString();
					bool flag = pspropertyInfo2.Value.ToString().Equals("Disconnected", StringComparison.OrdinalIgnoreCase);
					Guid guid = Guid.Parse(pspropertyInfo.Value.ToString());
					if (text.StartsWith("http://schemas.microsoft.com/powershell/", StringComparison.OrdinalIgnoreCase))
					{
						RemoteRunspacePoolInternal.UpdateWSManConnectionInfo(wsmanConnectionInfo2, psobject);
						wsmanConnectionInfo2.EnableNetworkAccess = true;
						if (flag)
						{
							DateTime? disconnectedOn;
							DateTime? expiresOn;
							RemoteRunspacePoolInternal.ComputeDisconnectedOnExpiresOn(psobject, out disconnectedOn, out expiresOn);
							wsmanConnectionInfo2.DisconnectedOn = disconnectedOn;
							wsmanConnectionInfo2.ExpiresOn = expiresOn;
						}
						List<ConnectCommandInfo> list2 = new List<ConnectCommandInfo>();
						Collection<PSObject> remoteCommands;
						try
						{
							remoteCommands = RemoteRunspacePoolEnumeration.GetRemoteCommands(guid, wsmanConnectionInfo2);
						}
						catch (CmdletInvocationException ex)
						{
							if (ex.InnerException != null && ex.InnerException is InvalidOperationException)
							{
								continue;
							}
							throw;
						}
						foreach (PSObject psobject2 in remoteCommands)
						{
							PSPropertyInfo pspropertyInfo5 = psobject2.Properties["CommandId"];
							PSPropertyInfo pspropertyInfo6 = psobject2.Properties["CommandLine"];
							if (pspropertyInfo5 != null)
							{
								string cmdStr = (pspropertyInfo6 != null) ? pspropertyInfo6.Value.ToString() : string.Empty;
								Guid cmdId = Guid.Parse(pspropertyInfo5.Value.ToString());
								list2.Add(new ConnectCommandInfo(cmdId, cmdStr));
							}
						}
						RunspacePool item = new RunspacePool(flag, guid, name, list2.ToArray(), wsmanConnectionInfo2, host, typeTable);
						list.Add(item);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x000BAE9C File Offset: 0x000B909C
		internal static RunspacePool GetRemoteRunspacePool(RunspaceConnectionInfo connectionInfo, Guid sessionId, Guid? commandId, PSHost host, TypeTable typeTable)
		{
			List<ConnectCommandInfo> list = new List<ConnectCommandInfo>();
			if (commandId != null)
			{
				list.Add(new ConnectCommandInfo(commandId.Value, string.Empty));
			}
			return new RunspacePool(true, sessionId, string.Empty, list.ToArray(), connectionInfo, host, typeTable);
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x000BAEE8 File Offset: 0x000B90E8
		private static void UpdateWSManConnectionInfo(WSManConnectionInfo wsmanConnectionInfo, PSObject rsInfoObject)
		{
			PSPropertyInfo pspropertyInfo = rsInfoObject.Properties["IdleTimeOut"];
			PSPropertyInfo pspropertyInfo2 = rsInfoObject.Properties["BufferMode"];
			PSPropertyInfo pspropertyInfo3 = rsInfoObject.Properties["ResourceUri"];
			PSPropertyInfo pspropertyInfo4 = rsInfoObject.Properties["Locale"];
			PSPropertyInfo pspropertyInfo5 = rsInfoObject.Properties["DataLocale"];
			PSPropertyInfo pspropertyInfo6 = rsInfoObject.Properties["CompressionMode"];
			PSPropertyInfo pspropertyInfo7 = rsInfoObject.Properties["Encoding"];
			PSPropertyInfo pspropertyInfo8 = rsInfoObject.Properties["ProfileLoaded"];
			PSPropertyInfo pspropertyInfo9 = rsInfoObject.Properties["MaxIdleTimeout"];
			int idleTimeout;
			if (pspropertyInfo != null && RemoteRunspacePoolInternal.GetTimeIntValue(pspropertyInfo.Value as string, out idleTimeout))
			{
				wsmanConnectionInfo.IdleTimeout = idleTimeout;
			}
			if (pspropertyInfo2 != null)
			{
				string text = pspropertyInfo2.Value as string;
				OutputBufferingMode outputBufferingMode;
				if (text != null && Enum.TryParse<OutputBufferingMode>(text, out outputBufferingMode))
				{
					wsmanConnectionInfo.OutputBufferingMode = outputBufferingMode;
				}
			}
			if (pspropertyInfo3 != null)
			{
				string text2 = pspropertyInfo3.Value as string;
				if (text2 != null)
				{
					wsmanConnectionInfo.ShellUri = text2;
				}
			}
			if (pspropertyInfo4 != null)
			{
				string text3 = pspropertyInfo4.Value as string;
				if (text3 != null)
				{
					try
					{
						wsmanConnectionInfo.UICulture = new CultureInfo(text3);
					}
					catch (ArgumentException)
					{
					}
				}
			}
			if (pspropertyInfo5 != null)
			{
				string text4 = pspropertyInfo5.Value as string;
				if (text4 != null)
				{
					try
					{
						wsmanConnectionInfo.Culture = new CultureInfo(text4);
					}
					catch (ArgumentException)
					{
					}
				}
			}
			if (pspropertyInfo6 != null)
			{
				string text5 = pspropertyInfo6.Value as string;
				if (text5 != null)
				{
					wsmanConnectionInfo.UseCompression = !text5.Equals("NoCompression", StringComparison.OrdinalIgnoreCase);
				}
			}
			if (pspropertyInfo7 != null)
			{
				string text6 = pspropertyInfo7.Value as string;
				if (text6 != null)
				{
					wsmanConnectionInfo.UseUTF16 = text6.Equals("UTF16", StringComparison.OrdinalIgnoreCase);
				}
			}
			if (pspropertyInfo8 != null)
			{
				string text7 = pspropertyInfo8.Value as string;
				if (text7 != null)
				{
					wsmanConnectionInfo.NoMachineProfile = !text7.Equals("Yes", StringComparison.OrdinalIgnoreCase);
				}
			}
			int maxIdleTimeout;
			if (pspropertyInfo9 != null && RemoteRunspacePoolInternal.GetTimeIntValue(pspropertyInfo9.Value as string, out maxIdleTimeout))
			{
				wsmanConnectionInfo.MaxIdleTimeout = maxIdleTimeout;
			}
		}

		// Token: 0x06002042 RID: 8258 RVA: 0x000BB110 File Offset: 0x000B9310
		private static void ComputeDisconnectedOnExpiresOn(PSObject rsInfoObject, out DateTime? disconnectedOn, out DateTime? expiresOn)
		{
			PSPropertyInfo pspropertyInfo = rsInfoObject.Properties["IdleTimeOut"];
			PSPropertyInfo pspropertyInfo2 = rsInfoObject.Properties["ShellInactivity"];
			if (pspropertyInfo != null && pspropertyInfo2 != null)
			{
				string text = pspropertyInfo2.Value as string;
				int num;
				if (text != null && RemoteRunspacePoolInternal.GetTimeIntValue(pspropertyInfo.Value as string, out num))
				{
					try
					{
						TimeSpan timeSpan = XmlConvert.ToTimeSpan(text);
						TimeSpan timeSpan2 = TimeSpan.FromSeconds((double)(num / 1000));
						if (timeSpan2 > timeSpan)
						{
							disconnectedOn = new DateTime?(DateTime.Now.Subtract(timeSpan));
							expiresOn = new DateTime?(disconnectedOn.Value.Add(timeSpan2));
							return;
						}
					}
					catch (FormatException)
					{
					}
					catch (ArgumentOutOfRangeException)
					{
					}
					catch (OverflowException)
					{
					}
				}
			}
			disconnectedOn = null;
			expiresOn = null;
		}

		// Token: 0x06002043 RID: 8259 RVA: 0x000BB20C File Offset: 0x000B940C
		private static bool GetTimeIntValue(string timeString, out int value)
		{
			if (timeString != null)
			{
				string value2 = timeString.Replace("PT", "").Replace("S", "");
				try
				{
					int num = (int)(Convert.ToDouble(value2, CultureInfo.InvariantCulture) * 1000.0);
					if (num > 0)
					{
						value = num;
						return true;
					}
				}
				catch (FormatException)
				{
				}
				catch (OverflowException)
				{
				}
			}
			value = 0;
			return false;
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x000BB288 File Offset: 0x000B9488
		private void SetRunspacePoolState(RunspacePoolStateInfo newStateInfo)
		{
			this.SetRunspacePoolState(newStateInfo, false);
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x000BB292 File Offset: 0x000B9492
		private void SetRunspacePoolState(RunspacePoolStateInfo newStateInfo, bool raiseEvents)
		{
			this.stateInfo = newStateInfo;
			this.availableForConnection = (this.stateInfo.State == RunspacePoolState.Disconnected || this.stateInfo.State == RunspacePoolState.Opened);
			if (raiseEvents)
			{
				base.RaiseStateChangeEvent(newStateInfo);
			}
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x000BB2CC File Offset: 0x000B94CC
		private void HandleSessionDisconnected(object sender, RemoteDataEventArgs<Exception> eventArgs)
		{
			bool flag = false;
			lock (this.syncObject)
			{
				if (this.stateInfo.State == RunspacePoolState.Disconnecting)
				{
					this.UpdateDisconnectedExpiresOn();
					this.SetRunspacePoolState(new RunspacePoolStateInfo(RunspacePoolState.Disconnected, eventArgs.Data));
					flag = true;
				}
				this.canReconnect = true;
			}
			if (flag)
			{
				base.RaiseStateChangeEvent(this.stateInfo);
				this.SetDisconnectAsCompleted();
			}
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x000BB34C File Offset: 0x000B954C
		private void SetDisconnectAsCompleted()
		{
			if (this.disconnectAsyncResult != null && !this.disconnectAsyncResult.IsCompleted)
			{
				this.disconnectAsyncResult.SetAsCompleted(this.stateInfo.Reason);
				this.disconnectAsyncResult = null;
			}
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x000BB380 File Offset: 0x000B9580
		private void HandleSessionReconnected(object sender, RemoteDataEventArgs<Exception> eventArgs)
		{
			bool flag = false;
			lock (this.syncObject)
			{
				if (this.stateInfo.State == RunspacePoolState.Connecting)
				{
					this.ResetDisconnectedOnExpiresOn();
					this.SetRunspacePoolState(new RunspacePoolStateInfo(RunspacePoolState.Opened, null));
					flag = true;
				}
			}
			if (flag)
			{
				base.RaiseStateChangeEvent(this.stateInfo);
				this.SetReconnectAsCompleted();
			}
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x000BB3F4 File Offset: 0x000B95F4
		private void SetReconnectAsCompleted()
		{
			if (this.reconnectAsyncResult != null && !this.reconnectAsyncResult.IsCompleted)
			{
				this.reconnectAsyncResult.SetAsCompleted(this.stateInfo.Reason);
				this.reconnectAsyncResult = null;
			}
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x000BB428 File Offset: 0x000B9628
		private void HandleSessionClosing(object sender, RemoteDataEventArgs<Exception> eventArgs)
		{
			this.closingReason = eventArgs.Data;
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x000BB438 File Offset: 0x000B9638
		private void HandleSessionClosed(object sender, RemoteDataEventArgs<Exception> eventArgs)
		{
			if (eventArgs.Data != null)
			{
				this.closingReason = eventArgs.Data;
			}
			RunspacePoolStateInfo stateInfo;
			lock (this.syncObject)
			{
				switch (this.stateInfo.State)
				{
				case RunspacePoolState.Opening:
				case RunspacePoolState.Opened:
				case RunspacePoolState.Disconnecting:
				case RunspacePoolState.Disconnected:
				case RunspacePoolState.Connecting:
					this.SetRunspacePoolState(new RunspacePoolStateInfo(RunspacePoolState.Broken, this.closingReason));
					break;
				case RunspacePoolState.Closing:
					this.SetRunspacePoolState(new RunspacePoolStateInfo(RunspacePoolState.Closed, this.closingReason));
					break;
				}
				stateInfo = new RunspacePoolStateInfo(this.stateInfo.State, this.stateInfo.Reason);
			}
			try
			{
				base.RaiseStateChangeEvent(stateInfo);
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			this.SetDisconnectAsCompleted();
			this.SetReconnectAsCompleted();
			this.SetCloseAsCompleted();
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x000BB534 File Offset: 0x000B9734
		private void SetOpenAsCompleted()
		{
			RunspacePoolAsyncResult runspacePoolAsyncResult = this.openAsyncResult;
			this.openAsyncResult = null;
			if (runspacePoolAsyncResult != null && !runspacePoolAsyncResult.IsCompleted)
			{
				runspacePoolAsyncResult.SetAsCompleted(this.stateInfo.Reason);
			}
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x000BB56C File Offset: 0x000B976C
		private void SetCloseAsCompleted()
		{
			this.DispatchTable.AbortAllCalls();
			if (this.closeAsyncResult != null)
			{
				this.closeAsyncResult.SetAsCompleted(this.stateInfo.Reason);
				this.closeAsyncResult = null;
			}
			this.SetOpenAsCompleted();
			try
			{
				this.applicationPrivateDataReceived.Set();
			}
			catch (ObjectDisposedException)
			{
			}
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x000BB5D0 File Offset: 0x000B97D0
		private void HandleResponseReceived(object sender, RemoteDataEventArgs<PSObject> eventArgs)
		{
			PSObject data = eventArgs.Data;
			object propertyValue = RemotingDecoder.GetPropertyValue<object>(data, "SetMinMaxRunspacesResponse");
			long propertyValue2 = RemotingDecoder.GetPropertyValue<long>(data, "ci");
			this.dispatchTable.SetResponse(propertyValue2, propertyValue);
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x000BB60C File Offset: 0x000B980C
		private void HandleURIDirectionReported(object sender, RemoteDataEventArgs<Uri> eventArgs)
		{
			WSManConnectionInfo wsmanConnectionInfo = this.connectionInfo as WSManConnectionInfo;
			if (wsmanConnectionInfo != null)
			{
				wsmanConnectionInfo.ConnectionUri = eventArgs.Data;
				this.URIRedirectionReported.SafeInvoke(this, eventArgs);
			}
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x000BB641 File Offset: 0x000B9841
		private void HandlePSEventArgsReceived(object sender, RemoteDataEventArgs<PSEventArgs> e)
		{
			this.OnForwardEvent(e.Data);
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x000BB650 File Offset: 0x000B9850
		private void HandleSessionRCDisconnecting(object sender, RemoteDataEventArgs<Exception> e)
		{
			lock (this.syncObject)
			{
				this.SetRunspacePoolState(new RunspacePoolStateInfo(RunspacePoolState.Disconnecting, e.Data));
			}
			base.RaiseStateChangeEvent(this.stateInfo);
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x000BB6A8 File Offset: 0x000B98A8
		private void HandleSessionCreateCompleted(object sender, CreateCompleteEventArgs eventArgs)
		{
			if (eventArgs != null)
			{
				this.connectionInfo.IdleTimeout = eventArgs.ConnectionInfo.IdleTimeout;
				this.connectionInfo.MaxIdleTimeout = eventArgs.ConnectionInfo.MaxIdleTimeout;
				WSManConnectionInfo wsmanConnectionInfo = this.connectionInfo as WSManConnectionInfo;
				if (wsmanConnectionInfo != null)
				{
					wsmanConnectionInfo.OutputBufferingMode = ((WSManConnectionInfo)eventArgs.ConnectionInfo).OutputBufferingMode;
				}
			}
			this.SessionCreateCompleted.SafeInvoke(this, eventArgs);
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x000BB718 File Offset: 0x000B9918
		private void ResetDisconnectedOnExpiresOn()
		{
			WSManConnectionInfo wsmanConnectionInfo = this.connectionInfo as WSManConnectionInfo;
			if (wsmanConnectionInfo != null)
			{
				wsmanConnectionInfo.NullDisconnectedExpiresOn();
			}
		}

		// Token: 0x06002054 RID: 8276 RVA: 0x000BB73C File Offset: 0x000B993C
		private void UpdateDisconnectedExpiresOn()
		{
			WSManConnectionInfo wsmanConnectionInfo = this.connectionInfo as WSManConnectionInfo;
			if (wsmanConnectionInfo != null)
			{
				wsmanConnectionInfo.SetDisconnectedExpiresOnToNow();
			}
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x000BB760 File Offset: 0x000B9960
		private void WaitAndRaiseConnectEventsProc(object state)
		{
			RunspacePoolStateInfo stateInfo = state as RunspacePoolStateInfo;
			try
			{
				this.applicationPrivateDataReceived.WaitOne();
			}
			catch (ObjectDisposedException)
			{
			}
			try
			{
				base.RaiseStateChangeEvent(stateInfo);
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			this.SetOpenAsCompleted();
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06002056 RID: 8278 RVA: 0x000BB7BC File Offset: 0x000B99BC
		private DispatchTable<object> DispatchTable
		{
			get
			{
				return this.dispatchTable;
			}
		}

		// Token: 0x06002057 RID: 8279 RVA: 0x000BB7C4 File Offset: 0x000B99C4
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x000BB7D3 File Offset: 0x000B99D3
		public override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (!this.isDisposed)
			{
				this.isDisposed = true;
				this.dataStructureHandler.Dispose(disposing);
				this.applicationPrivateDataReceived.Dispose();
			}
		}

		// Token: 0x04000E28 RID: 3624
		private PSPrimitiveDictionary applicationArguments;

		// Token: 0x04000E29 RID: 3625
		private PSPrimitiveDictionary applicationPrivateData;

		// Token: 0x04000E2A RID: 3626
		private ManualResetEvent applicationPrivateDataReceived = new ManualResetEvent(false);

		// Token: 0x04000E2E RID: 3630
		private RunspaceConnectionInfo connectionInfo;

		// Token: 0x04000E2F RID: 3631
		private ClientRunspacePoolDataStructureHandler dataStructureHandler;

		// Token: 0x04000E30 RID: 3632
		private RunspacePoolAsyncResult openAsyncResult;

		// Token: 0x04000E31 RID: 3633
		private RunspacePoolAsyncResult closeAsyncResult;

		// Token: 0x04000E32 RID: 3634
		private Exception closingReason;

		// Token: 0x04000E33 RID: 3635
		private RunspacePoolAsyncResult disconnectAsyncResult;

		// Token: 0x04000E34 RID: 3636
		private RunspacePoolAsyncResult reconnectAsyncResult;

		// Token: 0x04000E35 RID: 3637
		private bool isDisposed;

		// Token: 0x04000E36 RID: 3638
		private DispatchTable<object> dispatchTable;

		// Token: 0x04000E37 RID: 3639
		private ConnectCommandInfo[] connectCommands;

		// Token: 0x04000E38 RID: 3640
		private bool canReconnect;

		// Token: 0x04000E39 RID: 3641
		private string friendlyName = string.Empty;

		// Token: 0x04000E3A RID: 3642
		private bool availableForConnection;

		// Token: 0x04000E3B RID: 3643
		private ConcurrentStack<PowerShell> _runningPowerShells;
	}
}
