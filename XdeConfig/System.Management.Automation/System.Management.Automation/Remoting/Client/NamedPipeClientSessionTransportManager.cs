using System;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation.Remoting.Client
{
	// Token: 0x02000383 RID: 899
	internal sealed class NamedPipeClientSessionTransportManager : NamedPipeClientSessionTransportManagerBase
	{
		// Token: 0x06002BC0 RID: 11200 RVA: 0x000F20FC File Offset: 0x000F02FC
		internal NamedPipeClientSessionTransportManager(NamedPipeConnectionInfo connectionInfo, Guid runspaceId, PSRemotingCryptoHelper cryptoHelper) : base(connectionInfo, runspaceId, cryptoHelper, "NamedPipeTransport Reader Thread")
		{
			if (connectionInfo == null)
			{
				throw new PSArgumentNullException("connectionInfo");
			}
			this._connectionInfo = connectionInfo;
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x000F2124 File Offset: 0x000F0324
		internal override void CreateAsync()
		{
			this._clientPipe = new RemoteSessionNamedPipeClient(this._connectionInfo.ProcessId, this._connectionInfo.AppDomainName);
			this._clientPipe.Connect(this._connectionInfo.OpenTimeout);
			this.stdInWriter = new OutOfProcessTextWriter(this._clientPipe.TextWriter);
			base.StartReaderThread(this._clientPipe.TextReader);
		}

		// Token: 0x06002BC2 RID: 11202 RVA: 0x000F218F File Offset: 0x000F038F
		public void AbortConnect()
		{
			if (this._clientPipe != null)
			{
				this._clientPipe.AbortConnect();
			}
		}

		// Token: 0x040015F9 RID: 5625
		private const string _threadName = "NamedPipeTransport Reader Thread";

		// Token: 0x040015FA RID: 5626
		private NamedPipeConnectionInfo _connectionInfo;
	}
}
