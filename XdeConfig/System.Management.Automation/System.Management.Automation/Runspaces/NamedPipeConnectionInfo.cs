using System;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting.Client;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020002D0 RID: 720
	public sealed class NamedPipeConnectionInfo : RunspaceConnectionInfo
	{
		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06002268 RID: 8808 RVA: 0x000C2D9E File Offset: 0x000C0F9E
		// (set) Token: 0x06002269 RID: 8809 RVA: 0x000C2DA6 File Offset: 0x000C0FA6
		public int ProcessId { get; set; }

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x0600226A RID: 8810 RVA: 0x000C2DAF File Offset: 0x000C0FAF
		// (set) Token: 0x0600226B RID: 8811 RVA: 0x000C2DB7 File Offset: 0x000C0FB7
		public string AppDomainName
		{
			get
			{
				return this._appDomainName;
			}
			set
			{
				this._appDomainName = (value ?? string.Empty);
			}
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x000C2DC9 File Offset: 0x000C0FC9
		public NamedPipeConnectionInfo()
		{
			base.OpenTimeout = 60000;
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x000C2DE7 File Offset: 0x000C0FE7
		public NamedPipeConnectionInfo(int processId) : this(processId, string.Empty, 60000)
		{
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x000C2DFA File Offset: 0x000C0FFA
		public NamedPipeConnectionInfo(int processId, string appDomainName) : this(processId, appDomainName, 60000)
		{
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x000C2E09 File Offset: 0x000C1009
		public NamedPipeConnectionInfo(int processId, string appDomainName, int openTimeout)
		{
			this.ProcessId = processId;
			this.AppDomainName = appDomainName;
			base.OpenTimeout = openTimeout;
		}

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x06002270 RID: 8816 RVA: 0x000C2E31 File Offset: 0x000C1031
		// (set) Token: 0x06002271 RID: 8817 RVA: 0x000C2E38 File Offset: 0x000C1038
		public override string ComputerName
		{
			get
			{
				return "localhost";
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06002272 RID: 8818 RVA: 0x000C2E3F File Offset: 0x000C103F
		// (set) Token: 0x06002273 RID: 8819 RVA: 0x000C2E47 File Offset: 0x000C1047
		public override PSCredential Credential
		{
			get
			{
				return this._credential;
			}
			set
			{
				this._credential = value;
				this._authMechanism = AuthenticationMechanism.Default;
			}
		}

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06002274 RID: 8820 RVA: 0x000C2E57 File Offset: 0x000C1057
		// (set) Token: 0x06002275 RID: 8821 RVA: 0x000C2E60 File Offset: 0x000C1060
		public override AuthenticationMechanism AuthenticationMechanism
		{
			get
			{
				return this._authMechanism;
			}
			set
			{
				if (value != AuthenticationMechanism.Default)
				{
					throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.IPCSupportsOnlyDefaultAuth, new object[]
					{
						value.ToString(),
						AuthenticationMechanism.Default.ToString()
					});
				}
				this._authMechanism = value;
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06002276 RID: 8822 RVA: 0x000C2EA6 File Offset: 0x000C10A6
		// (set) Token: 0x06002277 RID: 8823 RVA: 0x000C2EAD File Offset: 0x000C10AD
		public override string CertificateThumbprint
		{
			get
			{
				return string.Empty;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x000C2EB4 File Offset: 0x000C10B4
		internal override RunspaceConnectionInfo InternalCopy()
		{
			return new NamedPipeConnectionInfo
			{
				_authMechanism = this.AuthenticationMechanism,
				_credential = this.Credential,
				ProcessId = this.ProcessId,
				_appDomainName = this._appDomainName,
				OpenTimeout = base.OpenTimeout
			};
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x000C2F04 File Offset: 0x000C1104
		internal override BaseClientSessionTransportManager CreateClientSessionTransportManager(Guid instanceId, string sessionName, PSRemotingCryptoHelper cryptoHelper)
		{
			return new NamedPipeClientSessionTransportManager(this, instanceId, cryptoHelper);
		}

		// Token: 0x04001065 RID: 4197
		private const int _defaultOpenTimeout = 60000;

		// Token: 0x04001066 RID: 4198
		private PSCredential _credential;

		// Token: 0x04001067 RID: 4199
		private AuthenticationMechanism _authMechanism;

		// Token: 0x04001068 RID: 4200
		private string _appDomainName = string.Empty;
	}
}
