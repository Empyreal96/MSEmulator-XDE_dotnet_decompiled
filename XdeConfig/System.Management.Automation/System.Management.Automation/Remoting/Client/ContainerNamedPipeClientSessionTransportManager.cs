using System;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation.Remoting.Client
{
	// Token: 0x02000384 RID: 900
	internal sealed class ContainerNamedPipeClientSessionTransportManager : NamedPipeClientSessionTransportManagerBase
	{
		// Token: 0x06002BC3 RID: 11203 RVA: 0x000F21A4 File Offset: 0x000F03A4
		internal ContainerNamedPipeClientSessionTransportManager(ContainerConnectionInfo connectionInfo, Guid runspaceId, PSRemotingCryptoHelper cryptoHelper) : base(connectionInfo, runspaceId, cryptoHelper, "ContainerNamedPipeTransport Reader Thread")
		{
			if (connectionInfo == null)
			{
				throw new PSArgumentNullException("connectionInfo");
			}
			this._connectionInfo = connectionInfo;
		}

		// Token: 0x06002BC4 RID: 11204 RVA: 0x000F21CC File Offset: 0x000F03CC
		internal override void CreateAsync()
		{
			this._clientPipe = new ContainerSessionNamedPipeClient(this._connectionInfo.ContainerProc.ProcessId, string.Empty, this._connectionInfo.ContainerProc.ContainerId);
			this._clientPipe.Connect(this._connectionInfo.OpenTimeout);
			this.stdInWriter = new OutOfProcessTextWriter(this._clientPipe.TextWriter);
			base.StartReaderThread(this._clientPipe.TextReader);
		}

		// Token: 0x06002BC5 RID: 11205 RVA: 0x000F2246 File Offset: 0x000F0446
		protected override void CleanupConnection()
		{
			this._clientPipe.Close();
			if (!this._connectionInfo.TerminateContainerProcess())
			{
				this._tracer.WriteMessage("ContainerNamedPipeClientSessionTransportManager", "CleanupConnection", Guid.Empty, "Failed to terminate PowerShell process inside container", new string[0]);
			}
		}

		// Token: 0x040015FB RID: 5627
		private const string _threadName = "ContainerNamedPipeTransport Reader Thread";

		// Token: 0x040015FC RID: 5628
		private ContainerConnectionInfo _connectionInfo;
	}
}
