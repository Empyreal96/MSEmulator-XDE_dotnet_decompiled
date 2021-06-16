using System;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation.Remoting.Client
{
	// Token: 0x02000381 RID: 897
	internal sealed class ContainerHyperVSocketClientSessionTransportManager : HyperVSocketClientSessionTransportManagerBase
	{
		// Token: 0x06002BB9 RID: 11193 RVA: 0x000F1E8B File Offset: 0x000F008B
		internal ContainerHyperVSocketClientSessionTransportManager(ContainerConnectionInfo connectionInfo, Guid runspaceId, PSRemotingCryptoHelper cryptoHelper, Guid targetGuid) : base(runspaceId, cryptoHelper)
		{
			if (connectionInfo == null)
			{
				throw new PSArgumentNullException("connectionInfo");
			}
			this._connectionInfo = connectionInfo;
			this._targetGuid = targetGuid;
		}

		// Token: 0x06002BBA RID: 11194 RVA: 0x000F1EB4 File Offset: 0x000F00B4
		internal override void CreateAsync()
		{
			this._client = new RemoteSessionHyperVSocketClient(this._targetGuid, false, true);
			if (!this._client.Connect(null, false))
			{
				this._client.Dispose();
				throw new PSInvalidOperationException(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.ContainerSessionConnectFailed, new object[0]), null, PSRemotingErrorId.ContainerSessionConnectFailed.ToString(), ErrorCategory.InvalidOperation, null);
			}
			this.stdInWriter = new OutOfProcessTextWriter(this._client.TextWriter);
			base.StartReaderThread(this._client.TextReader);
		}

		// Token: 0x040015F4 RID: 5620
		private Guid _targetGuid;

		// Token: 0x040015F5 RID: 5621
		private ContainerConnectionInfo _connectionInfo;
	}
}
