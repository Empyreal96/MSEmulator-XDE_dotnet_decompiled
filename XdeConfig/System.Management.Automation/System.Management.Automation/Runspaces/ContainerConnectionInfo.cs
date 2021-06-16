using System;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting.Client;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020002D2 RID: 722
	public sealed class ContainerConnectionInfo : RunspaceConnectionInfo
	{
		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06002287 RID: 8839 RVA: 0x000C300D File Offset: 0x000C120D
		// (set) Token: 0x06002288 RID: 8840 RVA: 0x000C3015 File Offset: 0x000C1215
		internal ContainerProcess ContainerProc
		{
			get
			{
				return this._containerProc;
			}
			set
			{
				this._containerProc = value;
			}
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06002289 RID: 8841 RVA: 0x000C301E File Offset: 0x000C121E
		// (set) Token: 0x0600228A RID: 8842 RVA: 0x000C3028 File Offset: 0x000C1228
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

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x0600228B RID: 8843 RVA: 0x000C306E File Offset: 0x000C126E
		// (set) Token: 0x0600228C RID: 8844 RVA: 0x000C3075 File Offset: 0x000C1275
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

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x0600228D RID: 8845 RVA: 0x000C307C File Offset: 0x000C127C
		// (set) Token: 0x0600228E RID: 8846 RVA: 0x000C3084 File Offset: 0x000C1284
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

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x0600228F RID: 8847 RVA: 0x000C3094 File Offset: 0x000C1294
		// (set) Token: 0x06002290 RID: 8848 RVA: 0x000C30A1 File Offset: 0x000C12A1
		public override string ComputerName
		{
			get
			{
				return this._containerProc.ContainerName;
			}
			set
			{
				throw new PSNotSupportedException();
			}
		}

		// Token: 0x06002291 RID: 8849 RVA: 0x000C30A8 File Offset: 0x000C12A8
		internal override RunspaceConnectionInfo InternalCopy()
		{
			return new ContainerConnectionInfo(this.ContainerProc);
		}

		// Token: 0x06002292 RID: 8850 RVA: 0x000C30C2 File Offset: 0x000C12C2
		internal override BaseClientSessionTransportManager CreateClientSessionTransportManager(Guid instanceId, string sessionName, PSRemotingCryptoHelper cryptoHelper)
		{
			if (this.ContainerProc.RuntimeId != Guid.Empty)
			{
				return new ContainerHyperVSocketClientSessionTransportManager(this, instanceId, cryptoHelper, this.ContainerProc.RuntimeId);
			}
			return new ContainerNamedPipeClientSessionTransportManager(this, instanceId, cryptoHelper);
		}

		// Token: 0x06002293 RID: 8851 RVA: 0x000C30F7 File Offset: 0x000C12F7
		internal ContainerConnectionInfo(ContainerProcess containerProc)
		{
			this.ContainerProc = containerProc;
			this.AuthenticationMechanism = AuthenticationMechanism.Default;
			this.Credential = null;
			base.OpenTimeout = 20000;
		}

		// Token: 0x06002294 RID: 8852 RVA: 0x000C3120 File Offset: 0x000C1320
		public static ContainerConnectionInfo CreateContainerConnectionInfoById(string containerId, bool runAsAdmin)
		{
			ContainerProcess containerProc = new ContainerProcess(containerId, null, 0, runAsAdmin);
			return new ContainerConnectionInfo(containerProc);
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x000C3140 File Offset: 0x000C1340
		public static ContainerConnectionInfo CreateContainerConnectionInfoByName(string containerName, bool runAsAdmin)
		{
			ContainerProcess containerProc = new ContainerProcess(null, containerName, 0, runAsAdmin);
			return new ContainerConnectionInfo(containerProc);
		}

		// Token: 0x06002296 RID: 8854 RVA: 0x000C315D File Offset: 0x000C135D
		public void CreateContainerProcess()
		{
			this.ContainerProc.CreateContainerProcess();
		}

		// Token: 0x06002297 RID: 8855 RVA: 0x000C316A File Offset: 0x000C136A
		public bool TerminateContainerProcess()
		{
			return this.ContainerProc.TerminateContainerProcess();
		}

		// Token: 0x0400106F RID: 4207
		private const int _defaultOpenTimeout = 20000;

		// Token: 0x04001070 RID: 4208
		private AuthenticationMechanism _authMechanism;

		// Token: 0x04001071 RID: 4209
		private ContainerProcess _containerProc;

		// Token: 0x04001072 RID: 4210
		private PSCredential _credential;
	}
}
