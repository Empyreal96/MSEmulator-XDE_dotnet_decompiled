using System;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Net;

namespace System.Management.Automation.Remoting.Client
{
	// Token: 0x02000380 RID: 896
	internal sealed class VMHyperVSocketClientSessionTransportManager : HyperVSocketClientSessionTransportManagerBase
	{
		// Token: 0x06002BB7 RID: 11191 RVA: 0x000F1D40 File Offset: 0x000EFF40
		internal VMHyperVSocketClientSessionTransportManager(VMConnectionInfo connectionInfo, Guid runspaceId, PSRemotingCryptoHelper cryptoHelper, Guid vmGuid) : base(runspaceId, cryptoHelper)
		{
			if (connectionInfo == null)
			{
				throw new PSArgumentNullException("connectionInfo");
			}
			this._connectionInfo = connectionInfo;
			this._vmGuid = vmGuid;
			if (connectionInfo.Credential == null)
			{
				this._networkCredential = CredentialCache.DefaultNetworkCredentials;
				return;
			}
			this._networkCredential = connectionInfo.Credential.GetNetworkCredential();
		}

		// Token: 0x06002BB8 RID: 11192 RVA: 0x000F1D98 File Offset: 0x000EFF98
		internal override void CreateAsync()
		{
			this._client = new RemoteSessionHyperVSocketClient(this._vmGuid, true, false);
			if (!this._client.Connect(this._networkCredential, true))
			{
				this._client.Dispose();
				throw new PSInvalidOperationException(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.VMSessionConnectFailed, new object[0]), null, PSRemotingErrorId.VMSessionConnectFailed.ToString(), ErrorCategory.InvalidOperation, null);
			}
			this._client.Dispose();
			this._client = new RemoteSessionHyperVSocketClient(this._vmGuid, false, false);
			if (!this._client.Connect(this._networkCredential, false))
			{
				this._client.Dispose();
				throw new PSInvalidOperationException(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.VMSessionConnectFailed, new object[0]), null, PSRemotingErrorId.VMSessionConnectFailed.ToString(), ErrorCategory.InvalidOperation, null);
			}
			this.stdInWriter = new OutOfProcessTextWriter(this._client.TextWriter);
			base.StartReaderThread(this._client.TextReader);
		}

		// Token: 0x040015F1 RID: 5617
		private Guid _vmGuid;

		// Token: 0x040015F2 RID: 5618
		private VMConnectionInfo _connectionInfo;

		// Token: 0x040015F3 RID: 5619
		private NetworkCredential _networkCredential;
	}
}
