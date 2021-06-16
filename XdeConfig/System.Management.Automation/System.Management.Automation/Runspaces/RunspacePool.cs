using System;
using System.Collections.ObjectModel;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces.Internal;
using System.Threading;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200024B RID: 587
	public sealed class RunspacePool : IDisposable
	{
		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06001B92 RID: 7058 RVA: 0x000A1A0C File Offset: 0x0009FC0C
		// (remove) Token: 0x06001B93 RID: 7059 RVA: 0x000A1A44 File Offset: 0x0009FC44
		private event EventHandler<RunspacePoolStateChangedEventArgs> InternalStateChanged;

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06001B94 RID: 7060 RVA: 0x000A1A7C File Offset: 0x0009FC7C
		// (remove) Token: 0x06001B95 RID: 7061 RVA: 0x000A1AB4 File Offset: 0x0009FCB4
		private event EventHandler<PSEventArgs> InternalForwardEvent;

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06001B96 RID: 7062 RVA: 0x000A1AEC File Offset: 0x0009FCEC
		// (remove) Token: 0x06001B97 RID: 7063 RVA: 0x000A1B24 File Offset: 0x0009FD24
		private event EventHandler<RunspaceCreatedEventArgs> InternalRunspaceCreated;

		// Token: 0x06001B98 RID: 7064 RVA: 0x000A1B59 File Offset: 0x0009FD59
		internal RunspacePool(int minRunspaces, int maxRunspaces, RunspaceConfiguration runspaceConfiguration, PSHost host)
		{
			this.internalPool = new RunspacePoolInternal(minRunspaces, maxRunspaces, runspaceConfiguration, host);
		}

		// Token: 0x06001B99 RID: 7065 RVA: 0x000A1B7C File Offset: 0x0009FD7C
		internal RunspacePool(int minRunspaces, int maxRunspaces, InitialSessionState initialSessionState, PSHost host)
		{
			this.internalPool = new RunspacePoolInternal(minRunspaces, maxRunspaces, initialSessionState, host);
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x000A1B9F File Offset: 0x0009FD9F
		internal RunspacePool(int minRunspaces, int maxRunspaces, TypeTable typeTable, PSHost host, PSPrimitiveDictionary applicationArguments, RunspaceConnectionInfo connectionInfo, string name = null)
		{
			this.internalPool = new RemoteRunspacePoolInternal(minRunspaces, maxRunspaces, typeTable, host, applicationArguments, connectionInfo, name);
			this.isRemote = true;
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x000A1BCF File Offset: 0x0009FDCF
		internal RunspacePool(bool isDisconnected, Guid instanceId, string name, ConnectCommandInfo[] connectCommands, RunspaceConnectionInfo connectionInfo, PSHost host, TypeTable typeTable)
		{
			if (!(connectionInfo is WSManConnectionInfo))
			{
				throw new NotSupportedException();
			}
			this.internalPool = new RemoteRunspacePoolInternal(instanceId, name, isDisconnected, connectCommands, connectionInfo, host, typeTable);
			this.isRemote = true;
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06001B9C RID: 7068 RVA: 0x000A1C0E File Offset: 0x0009FE0E
		public Guid InstanceId
		{
			get
			{
				return this.internalPool.InstanceId;
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06001B9D RID: 7069 RVA: 0x000A1C1B File Offset: 0x0009FE1B
		public bool IsDisposed
		{
			get
			{
				return this.internalPool.IsDisposed;
			}
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06001B9E RID: 7070 RVA: 0x000A1C28 File Offset: 0x0009FE28
		public RunspacePoolStateInfo RunspacePoolStateInfo
		{
			get
			{
				return this.internalPool.RunspacePoolStateInfo;
			}
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06001B9F RID: 7071 RVA: 0x000A1C35 File Offset: 0x0009FE35
		public InitialSessionState InitialSessionState
		{
			get
			{
				return this.internalPool.InitialSessionState;
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06001BA0 RID: 7072 RVA: 0x000A1C42 File Offset: 0x0009FE42
		public RunspaceConnectionInfo ConnectionInfo
		{
			get
			{
				return this.internalPool.ConnectionInfo;
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06001BA1 RID: 7073 RVA: 0x000A1C4F File Offset: 0x0009FE4F
		// (set) Token: 0x06001BA2 RID: 7074 RVA: 0x000A1C5C File Offset: 0x0009FE5C
		public TimeSpan CleanupInterval
		{
			get
			{
				return this.internalPool.CleanupInterval;
			}
			set
			{
				this.internalPool.CleanupInterval = value;
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06001BA3 RID: 7075 RVA: 0x000A1C6A File Offset: 0x0009FE6A
		public RunspacePoolAvailability RunspacePoolAvailability
		{
			get
			{
				return this.internalPool.RunspacePoolAvailability;
			}
		}

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06001BA4 RID: 7076 RVA: 0x000A1C78 File Offset: 0x0009FE78
		// (remove) Token: 0x06001BA5 RID: 7077 RVA: 0x000A1CE0 File Offset: 0x0009FEE0
		public event EventHandler<RunspacePoolStateChangedEventArgs> StateChanged
		{
			add
			{
				lock (this.syncObject)
				{
					bool flag2 = null == this.InternalStateChanged;
					this.InternalStateChanged += value;
					if (flag2)
					{
						this.internalPool.StateChanged += this.OnStateChanged;
					}
				}
			}
			remove
			{
				lock (this.syncObject)
				{
					this.InternalStateChanged -= value;
					if (this.InternalStateChanged == null)
					{
						this.internalPool.StateChanged -= this.OnStateChanged;
					}
				}
			}
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x000A1D40 File Offset: 0x0009FF40
		private void OnStateChanged(object source, RunspacePoolStateChangedEventArgs args)
		{
			if (this.ConnectionInfo is NewProcessConnectionInfo)
			{
				NewProcessConnectionInfo newProcessConnectionInfo = this.ConnectionInfo as NewProcessConnectionInfo;
				if (newProcessConnectionInfo.Process != null && (args.RunspacePoolStateInfo.State == RunspacePoolState.Opened || args.RunspacePoolStateInfo.State == RunspacePoolState.Broken))
				{
					newProcessConnectionInfo.Process.RunspacePool = this;
				}
			}
			this.InternalStateChanged.SafeInvoke(this, args);
		}

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06001BA7 RID: 7079 RVA: 0x000A1DA4 File Offset: 0x0009FFA4
		// (remove) Token: 0x06001BA8 RID: 7080 RVA: 0x000A1E0C File Offset: 0x000A000C
		internal event EventHandler<PSEventArgs> ForwardEvent
		{
			add
			{
				lock (this.syncObject)
				{
					bool flag2 = this.InternalForwardEvent == null;
					this.InternalForwardEvent += value;
					if (flag2)
					{
						this.internalPool.ForwardEvent += this.OnInternalPoolForwardEvent;
					}
				}
			}
			remove
			{
				lock (this.syncObject)
				{
					this.InternalForwardEvent -= value;
					if (this.InternalForwardEvent == null)
					{
						this.internalPool.ForwardEvent -= this.OnInternalPoolForwardEvent;
					}
				}
			}
		}

		// Token: 0x06001BA9 RID: 7081 RVA: 0x000A1E6C File Offset: 0x000A006C
		private void OnInternalPoolForwardEvent(object sender, PSEventArgs e)
		{
			this.OnEventForwarded(e);
		}

		// Token: 0x06001BAA RID: 7082 RVA: 0x000A1E78 File Offset: 0x000A0078
		private void OnEventForwarded(PSEventArgs e)
		{
			EventHandler<PSEventArgs> internalForwardEvent = this.InternalForwardEvent;
			if (internalForwardEvent != null)
			{
				internalForwardEvent(this, e);
			}
		}

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06001BAB RID: 7083 RVA: 0x000A1E98 File Offset: 0x000A0098
		// (remove) Token: 0x06001BAC RID: 7084 RVA: 0x000A1F00 File Offset: 0x000A0100
		internal event EventHandler<RunspaceCreatedEventArgs> RunspaceCreated
		{
			add
			{
				lock (this.syncObject)
				{
					bool flag2 = null == this.InternalRunspaceCreated;
					this.InternalRunspaceCreated += value;
					if (flag2)
					{
						this.internalPool.RunspaceCreated += this.OnRunspaceCreated;
					}
				}
			}
			remove
			{
				lock (this.syncObject)
				{
					this.InternalRunspaceCreated -= value;
					if (this.InternalRunspaceCreated == null)
					{
						this.internalPool.RunspaceCreated -= this.OnRunspaceCreated;
					}
				}
			}
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x000A1F60 File Offset: 0x000A0160
		private void OnRunspaceCreated(object source, RunspaceCreatedEventArgs args)
		{
			this.InternalRunspaceCreated.SafeInvoke(this, args);
		}

		// Token: 0x06001BAE RID: 7086 RVA: 0x000A1F6F File Offset: 0x000A016F
		public static RunspacePool[] GetRunspacePools(RunspaceConnectionInfo connectionInfo)
		{
			return RunspacePool.GetRunspacePools(connectionInfo, null, null);
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x000A1F79 File Offset: 0x000A0179
		public static RunspacePool[] GetRunspacePools(RunspaceConnectionInfo connectionInfo, PSHost host)
		{
			return RunspacePool.GetRunspacePools(connectionInfo, host, null);
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x000A1F83 File Offset: 0x000A0183
		public static RunspacePool[] GetRunspacePools(RunspaceConnectionInfo connectionInfo, PSHost host, TypeTable typeTable)
		{
			return RemoteRunspacePoolInternal.GetRemoteRunspacePools(connectionInfo, host, typeTable);
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x000A1F8D File Offset: 0x000A018D
		public void Disconnect()
		{
			this.internalPool.Disconnect();
		}

		// Token: 0x06001BB2 RID: 7090 RVA: 0x000A1F9A File Offset: 0x000A019A
		public IAsyncResult BeginDisconnect(AsyncCallback callback, object state)
		{
			return this.internalPool.BeginDisconnect(callback, state);
		}

		// Token: 0x06001BB3 RID: 7091 RVA: 0x000A1FA9 File Offset: 0x000A01A9
		public void EndDisconnect(IAsyncResult asyncResult)
		{
			this.internalPool.EndDisconnect(asyncResult);
		}

		// Token: 0x06001BB4 RID: 7092 RVA: 0x000A1FB7 File Offset: 0x000A01B7
		public void Connect()
		{
			this.internalPool.Connect();
		}

		// Token: 0x06001BB5 RID: 7093 RVA: 0x000A1FC4 File Offset: 0x000A01C4
		public IAsyncResult BeginConnect(AsyncCallback callback, object state)
		{
			return this.internalPool.BeginConnect(callback, state);
		}

		// Token: 0x06001BB6 RID: 7094 RVA: 0x000A1FD3 File Offset: 0x000A01D3
		public void EndConnect(IAsyncResult asyncResult)
		{
			this.internalPool.EndConnect(asyncResult);
		}

		// Token: 0x06001BB7 RID: 7095 RVA: 0x000A1FE1 File Offset: 0x000A01E1
		public Collection<PowerShell> CreateDisconnectedPowerShells()
		{
			return this.internalPool.CreateDisconnectedPowerShells(this);
		}

		// Token: 0x06001BB8 RID: 7096 RVA: 0x000A1FEF File Offset: 0x000A01EF
		public RunspacePoolCapability GetCapabilities()
		{
			return this.internalPool.GetCapabilities();
		}

		// Token: 0x06001BB9 RID: 7097 RVA: 0x000A1FFC File Offset: 0x000A01FC
		public bool SetMaxRunspaces(int maxRunspaces)
		{
			return this.internalPool.SetMaxRunspaces(maxRunspaces);
		}

		// Token: 0x06001BBA RID: 7098 RVA: 0x000A200A File Offset: 0x000A020A
		public int GetMaxRunspaces()
		{
			return this.internalPool.GetMaxRunspaces();
		}

		// Token: 0x06001BBB RID: 7099 RVA: 0x000A2017 File Offset: 0x000A0217
		public bool SetMinRunspaces(int minRunspaces)
		{
			return this.internalPool.SetMinRunspaces(minRunspaces);
		}

		// Token: 0x06001BBC RID: 7100 RVA: 0x000A2025 File Offset: 0x000A0225
		public int GetMinRunspaces()
		{
			return this.internalPool.GetMinRunspaces();
		}

		// Token: 0x06001BBD RID: 7101 RVA: 0x000A2032 File Offset: 0x000A0232
		public int GetAvailableRunspaces()
		{
			return this.internalPool.GetAvailableRunspaces();
		}

		// Token: 0x06001BBE RID: 7102 RVA: 0x000A203F File Offset: 0x000A023F
		public void Open()
		{
			this.internalPool.Open();
		}

		// Token: 0x06001BBF RID: 7103 RVA: 0x000A204C File Offset: 0x000A024C
		public IAsyncResult BeginOpen(AsyncCallback callback, object state)
		{
			return this.internalPool.BeginOpen(callback, state);
		}

		// Token: 0x06001BC0 RID: 7104 RVA: 0x000A205B File Offset: 0x000A025B
		public void EndOpen(IAsyncResult asyncResult)
		{
			this.internalPool.EndOpen(asyncResult);
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x000A2069 File Offset: 0x000A0269
		public void Close()
		{
			this.internalPool.Close();
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x000A2076 File Offset: 0x000A0276
		public IAsyncResult BeginClose(AsyncCallback callback, object state)
		{
			return this.internalPool.BeginClose(callback, state);
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x000A2085 File Offset: 0x000A0285
		public void EndClose(IAsyncResult asyncResult)
		{
			this.internalPool.EndClose(asyncResult);
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x000A2093 File Offset: 0x000A0293
		public void Dispose()
		{
			this.internalPool.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x000A20A7 File Offset: 0x000A02A7
		public PSPrimitiveDictionary GetApplicationPrivateData()
		{
			return this.internalPool.GetApplicationPrivateData();
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06001BC6 RID: 7110 RVA: 0x000A20B4 File Offset: 0x000A02B4
		// (set) Token: 0x06001BC7 RID: 7111 RVA: 0x000A20C1 File Offset: 0x000A02C1
		public PSThreadOptions ThreadOptions
		{
			get
			{
				return this.internalPool.ThreadOptions;
			}
			set
			{
				if (this.RunspacePoolStateInfo.State != RunspacePoolState.BeforeOpen)
				{
					throw new InvalidRunspacePoolStateException(RunspacePoolStrings.ChangePropertyAfterOpen);
				}
				this.internalPool.ThreadOptions = value;
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06001BC8 RID: 7112 RVA: 0x000A20E7 File Offset: 0x000A02E7
		// (set) Token: 0x06001BC9 RID: 7113 RVA: 0x000A20F4 File Offset: 0x000A02F4
		public ApartmentState ApartmentState
		{
			get
			{
				return this.internalPool.ApartmentState;
			}
			set
			{
				if (this.RunspacePoolStateInfo.State != RunspacePoolState.BeforeOpen)
				{
					throw new InvalidRunspacePoolStateException(RunspacePoolStrings.ChangePropertyAfterOpen);
				}
				this.internalPool.ApartmentState = value;
			}
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x000A211A File Offset: 0x000A031A
		internal IAsyncResult BeginGetRunspace(AsyncCallback callback, object state)
		{
			return this.internalPool.BeginGetRunspace(callback, state);
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x000A2129 File Offset: 0x000A0329
		internal void CancelGetRunspace(IAsyncResult asyncResult)
		{
			this.internalPool.CancelGetRunspace(asyncResult);
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x000A2137 File Offset: 0x000A0337
		internal Runspace EndGetRunspace(IAsyncResult asyncResult)
		{
			return this.internalPool.EndGetRunspace(asyncResult);
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x000A2145 File Offset: 0x000A0345
		internal void ReleaseRunspace(Runspace runspace)
		{
			this.internalPool.ReleaseRunspace(runspace);
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06001BCE RID: 7118 RVA: 0x000A2153 File Offset: 0x000A0353
		internal bool IsRemote
		{
			get
			{
				return this.isRemote;
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06001BCF RID: 7119 RVA: 0x000A215B File Offset: 0x000A035B
		internal RemoteRunspacePoolInternal RemoteRunspacePoolInternal
		{
			get
			{
				if (this.internalPool is RemoteRunspacePoolInternal)
				{
					return (RemoteRunspacePoolInternal)this.internalPool;
				}
				return null;
			}
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x000A2177 File Offset: 0x000A0377
		internal void AssertPoolIsOpen()
		{
			this.internalPool.AssertPoolIsOpen();
		}

		// Token: 0x04000B58 RID: 2904
		private RunspacePoolInternal internalPool;

		// Token: 0x04000B59 RID: 2905
		private object syncObject = new object();

		// Token: 0x04000B5D RID: 2909
		private bool isRemote;
	}
}
