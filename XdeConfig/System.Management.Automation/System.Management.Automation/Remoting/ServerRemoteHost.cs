using System;
using System.Globalization;
using System.Management.Automation.Host;
using System.Management.Automation.Remoting.Server;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000306 RID: 774
	internal class ServerRemoteHost : PSHost, IHostSupportsInteractiveSession
	{
		// Token: 0x06002490 RID: 9360 RVA: 0x000CD634 File Offset: 0x000CB834
		internal ServerRemoteHost(Guid clientRunspacePoolId, Guid clientPowerShellId, HostInfo hostInfo, AbstractServerTransportManager transportManager, Runspace runspace, ServerDriverRemoteHost serverDriverRemoteHost)
		{
			this._clientRunspacePoolId = clientRunspacePoolId;
			this._clientPowerShellId = clientPowerShellId;
			this._hostInfo = hostInfo;
			this._transportManager = transportManager;
			this._serverDriverRemoteHost = serverDriverRemoteHost;
			this._serverMethodExecutor = new ServerMethodExecutor(clientRunspacePoolId, clientPowerShellId, this._transportManager);
			this._remoteHostUserInterface = (hostInfo.IsHostUINull ? null : new ServerRemoteHostUserInterface(this));
			this._runspace = runspace;
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x06002491 RID: 9361 RVA: 0x000CD6A9 File Offset: 0x000CB8A9
		internal ServerMethodExecutor ServerMethodExecutor
		{
			get
			{
				return this._serverMethodExecutor;
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06002492 RID: 9362 RVA: 0x000CD6B1 File Offset: 0x000CB8B1
		public override PSHostUserInterface UI
		{
			get
			{
				return this._remoteHostUserInterface;
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x06002493 RID: 9363 RVA: 0x000CD6B9 File Offset: 0x000CB8B9
		public override string Name
		{
			get
			{
				return "ServerRemoteHost";
			}
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06002494 RID: 9364 RVA: 0x000CD6C0 File Offset: 0x000CB8C0
		public override Version Version
		{
			get
			{
				return RemotingConstants.HostVersion;
			}
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06002495 RID: 9365 RVA: 0x000CD6C7 File Offset: 0x000CB8C7
		public override Guid InstanceId
		{
			get
			{
				return this._instanceId;
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06002496 RID: 9366 RVA: 0x000CD6CF File Offset: 0x000CB8CF
		public virtual bool IsRunspacePushed
		{
			get
			{
				if (this._serverDriverRemoteHost != null)
				{
					return this._serverDriverRemoteHost.IsRunspacePushed;
				}
				throw RemoteHostExceptions.NewNotImplementedException(RemoteHostMethodId.GetIsRunspacePushed);
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06002497 RID: 9367 RVA: 0x000CD6EC File Offset: 0x000CB8EC
		// (set) Token: 0x06002498 RID: 9368 RVA: 0x000CD6F4 File Offset: 0x000CB8F4
		public Runspace Runspace
		{
			get
			{
				return this._runspace;
			}
			internal set
			{
				this._runspace = value;
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06002499 RID: 9369 RVA: 0x000CD6FD File Offset: 0x000CB8FD
		internal HostInfo HostInfo
		{
			get
			{
				return this._hostInfo;
			}
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x000CD708 File Offset: 0x000CB908
		public override void SetShouldExit(int exitCode)
		{
			this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.SetShouldExit, new object[]
			{
				exitCode
			});
		}

		// Token: 0x0600249B RID: 9371 RVA: 0x000CD732 File Offset: 0x000CB932
		public override void EnterNestedPrompt()
		{
			throw RemoteHostExceptions.NewNotImplementedException(RemoteHostMethodId.EnterNestedPrompt);
		}

		// Token: 0x0600249C RID: 9372 RVA: 0x000CD73A File Offset: 0x000CB93A
		public override void ExitNestedPrompt()
		{
			throw RemoteHostExceptions.NewNotImplementedException(RemoteHostMethodId.ExitNestedPrompt);
		}

		// Token: 0x0600249D RID: 9373 RVA: 0x000CD742 File Offset: 0x000CB942
		public override void NotifyBeginApplication()
		{
		}

		// Token: 0x0600249E RID: 9374 RVA: 0x000CD744 File Offset: 0x000CB944
		public override void NotifyEndApplication()
		{
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x0600249F RID: 9375 RVA: 0x000CD746 File Offset: 0x000CB946
		public override CultureInfo CurrentCulture
		{
			get
			{
				return CultureInfo.CurrentCulture;
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x060024A0 RID: 9376 RVA: 0x000CD74D File Offset: 0x000CB94D
		public override CultureInfo CurrentUICulture
		{
			get
			{
				return CultureInfo.CurrentUICulture;
			}
		}

		// Token: 0x060024A1 RID: 9377 RVA: 0x000CD754 File Offset: 0x000CB954
		public virtual void PushRunspace(Runspace runspace)
		{
			if (this._serverDriverRemoteHost != null)
			{
				this._serverDriverRemoteHost.PushRunspace(runspace);
				return;
			}
			throw RemoteHostExceptions.NewNotImplementedException(RemoteHostMethodId.PushRunspace);
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x000CD772 File Offset: 0x000CB972
		public virtual void PopRunspace()
		{
			if (this._serverDriverRemoteHost != null && this._serverDriverRemoteHost.IsRunspacePushed)
			{
				this._serverDriverRemoteHost.PopRunspace();
				return;
			}
			this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.PopRunspace);
		}

		// Token: 0x040011FD RID: 4605
		private Guid _instanceId = Guid.NewGuid();

		// Token: 0x040011FE RID: 4606
		private ServerRemoteHostUserInterface _remoteHostUserInterface;

		// Token: 0x040011FF RID: 4607
		private ServerMethodExecutor _serverMethodExecutor;

		// Token: 0x04001200 RID: 4608
		private Guid _clientRunspacePoolId;

		// Token: 0x04001201 RID: 4609
		private Guid _clientPowerShellId;

		// Token: 0x04001202 RID: 4610
		private HostInfo _hostInfo;

		// Token: 0x04001203 RID: 4611
		protected AbstractServerTransportManager _transportManager;

		// Token: 0x04001204 RID: 4612
		private Runspace _runspace;

		// Token: 0x04001205 RID: 4613
		private ServerDriverRemoteHost _serverDriverRemoteHost;
	}
}
