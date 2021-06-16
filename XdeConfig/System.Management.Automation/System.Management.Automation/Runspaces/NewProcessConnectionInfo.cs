using System;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting.Client;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020002CF RID: 719
	internal sealed class NewProcessConnectionInfo : RunspaceConnectionInfo
	{
		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06002254 RID: 8788 RVA: 0x000C2C5A File Offset: 0x000C0E5A
		// (set) Token: 0x06002255 RID: 8789 RVA: 0x000C2C62 File Offset: 0x000C0E62
		public ScriptBlock InitializationScript
		{
			get
			{
				return this.initScript;
			}
			set
			{
				this.initScript = value;
			}
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06002256 RID: 8790 RVA: 0x000C2C6B File Offset: 0x000C0E6B
		// (set) Token: 0x06002257 RID: 8791 RVA: 0x000C2C73 File Offset: 0x000C0E73
		public bool RunAs32
		{
			get
			{
				return this.shouldRunsAs32;
			}
			set
			{
				this.shouldRunsAs32 = value;
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06002258 RID: 8792 RVA: 0x000C2C7C File Offset: 0x000C0E7C
		// (set) Token: 0x06002259 RID: 8793 RVA: 0x000C2C84 File Offset: 0x000C0E84
		public Version PSVersion
		{
			get
			{
				return this.psVersion;
			}
			set
			{
				this.psVersion = value;
			}
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x0600225A RID: 8794 RVA: 0x000C2C8D File Offset: 0x000C0E8D
		// (set) Token: 0x0600225B RID: 8795 RVA: 0x000C2C95 File Offset: 0x000C0E95
		internal PowerShellProcessInstance Process
		{
			get
			{
				return this.process;
			}
			set
			{
				this.process = value;
			}
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x0600225C RID: 8796 RVA: 0x000C2C9E File Offset: 0x000C0E9E
		// (set) Token: 0x0600225D RID: 8797 RVA: 0x000C2CA5 File Offset: 0x000C0EA5
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

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x0600225E RID: 8798 RVA: 0x000C2CAC File Offset: 0x000C0EAC
		// (set) Token: 0x0600225F RID: 8799 RVA: 0x000C2CB4 File Offset: 0x000C0EB4
		public override PSCredential Credential
		{
			get
			{
				return this.credential;
			}
			set
			{
				this.credential = value;
				this.authMechanism = AuthenticationMechanism.Default;
			}
		}

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06002260 RID: 8800 RVA: 0x000C2CC4 File Offset: 0x000C0EC4
		// (set) Token: 0x06002261 RID: 8801 RVA: 0x000C2CCC File Offset: 0x000C0ECC
		public override AuthenticationMechanism AuthenticationMechanism
		{
			get
			{
				return this.authMechanism;
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
				this.authMechanism = value;
			}
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06002262 RID: 8802 RVA: 0x000C2D12 File Offset: 0x000C0F12
		// (set) Token: 0x06002263 RID: 8803 RVA: 0x000C2D19 File Offset: 0x000C0F19
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

		// Token: 0x06002264 RID: 8804 RVA: 0x000C2D20 File Offset: 0x000C0F20
		public NewProcessConnectionInfo Copy()
		{
			return new NewProcessConnectionInfo(this.credential)
			{
				AuthenticationMechanism = this.AuthenticationMechanism,
				InitializationScript = this.InitializationScript,
				RunAs32 = this.RunAs32,
				PSVersion = this.PSVersion,
				Process = this.Process
			};
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x000C2D76 File Offset: 0x000C0F76
		internal override RunspaceConnectionInfo InternalCopy()
		{
			return this.Copy();
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x000C2D7E File Offset: 0x000C0F7E
		internal override BaseClientSessionTransportManager CreateClientSessionTransportManager(Guid instanceId, string sessionName, PSRemotingCryptoHelper cryptoHelper)
		{
			return new OutOfProcessClientSessionTransportManager(instanceId, this, cryptoHelper);
		}

		// Token: 0x06002267 RID: 8807 RVA: 0x000C2D88 File Offset: 0x000C0F88
		internal NewProcessConnectionInfo(PSCredential credential)
		{
			this.credential = credential;
			this.authMechanism = AuthenticationMechanism.Default;
		}

		// Token: 0x0400105F RID: 4191
		private PSCredential credential;

		// Token: 0x04001060 RID: 4192
		private AuthenticationMechanism authMechanism;

		// Token: 0x04001061 RID: 4193
		private ScriptBlock initScript;

		// Token: 0x04001062 RID: 4194
		private bool shouldRunsAs32;

		// Token: 0x04001063 RID: 4195
		private Version psVersion;

		// Token: 0x04001064 RID: 4196
		private PowerShellProcessInstance process;
	}
}
