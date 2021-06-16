using System;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting.Client;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020002D1 RID: 721
	public sealed class VMConnectionInfo : RunspaceConnectionInfo
	{
		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x0600227A RID: 8826 RVA: 0x000C2F0E File Offset: 0x000C110E
		// (set) Token: 0x0600227B RID: 8827 RVA: 0x000C2F16 File Offset: 0x000C1116
		public Guid VMGuid
		{
			get
			{
				return this._vmGuid;
			}
			set
			{
				this._vmGuid = value;
			}
		}

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x0600227C RID: 8828 RVA: 0x000C2F1F File Offset: 0x000C111F
		// (set) Token: 0x0600227D RID: 8829 RVA: 0x000C2F28 File Offset: 0x000C1128
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

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x0600227E RID: 8830 RVA: 0x000C2F6E File Offset: 0x000C116E
		// (set) Token: 0x0600227F RID: 8831 RVA: 0x000C2F75 File Offset: 0x000C1175
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

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06002280 RID: 8832 RVA: 0x000C2F7C File Offset: 0x000C117C
		// (set) Token: 0x06002281 RID: 8833 RVA: 0x000C2F84 File Offset: 0x000C1184
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

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06002282 RID: 8834 RVA: 0x000C2F94 File Offset: 0x000C1194
		// (set) Token: 0x06002283 RID: 8835 RVA: 0x000C2F9C File Offset: 0x000C119C
		public override string ComputerName
		{
			get
			{
				return this._vmName;
			}
			set
			{
				this._vmName = value;
			}
		}

		// Token: 0x06002284 RID: 8836 RVA: 0x000C2FA8 File Offset: 0x000C11A8
		internal override RunspaceConnectionInfo InternalCopy()
		{
			return new VMConnectionInfo(this.Credential, this.VMGuid, this.ComputerName);
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x000C2FCE File Offset: 0x000C11CE
		internal override BaseClientSessionTransportManager CreateClientSessionTransportManager(Guid instanceId, string sessionName, PSRemotingCryptoHelper cryptoHelper)
		{
			return new VMHyperVSocketClientSessionTransportManager(this, instanceId, cryptoHelper, this.VMGuid);
		}

		// Token: 0x06002286 RID: 8838 RVA: 0x000C2FDE File Offset: 0x000C11DE
		internal VMConnectionInfo(PSCredential credential, Guid vmGuid, string vmName)
		{
			this.Credential = credential;
			this.VMGuid = vmGuid;
			this.ComputerName = vmName;
			this.AuthenticationMechanism = AuthenticationMechanism.Default;
			base.OpenTimeout = 20000;
		}

		// Token: 0x0400106A RID: 4202
		private const int _defaultOpenTimeout = 20000;

		// Token: 0x0400106B RID: 4203
		private AuthenticationMechanism _authMechanism;

		// Token: 0x0400106C RID: 4204
		private PSCredential _credential;

		// Token: 0x0400106D RID: 4205
		private Guid _vmGuid;

		// Token: 0x0400106E RID: 4206
		private string _vmName;
	}
}
