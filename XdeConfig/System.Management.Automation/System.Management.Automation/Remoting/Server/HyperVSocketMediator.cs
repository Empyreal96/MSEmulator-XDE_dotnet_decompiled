using System;

namespace System.Management.Automation.Remoting.Server
{
	// Token: 0x020002FA RID: 762
	internal sealed class HyperVSocketMediator : OutOfProcessMediatorBase
	{
		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x060023FB RID: 9211 RVA: 0x000C9E9A File Offset: 0x000C809A
		internal bool IsDisposed
		{
			get
			{
				return this._hypervSocketServer.IsDisposed;
			}
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x000C9EA8 File Offset: 0x000C80A8
		private HyperVSocketMediator() : base(false)
		{
			this._hypervSocketServer = new RemoteSessionHyperVSocketServer(false);
			this.originalStdIn = this._hypervSocketServer.TextReader;
			this.originalStdOut = new OutOfProcessTextWriter(this._hypervSocketServer.TextWriter);
			this.originalStdErr = new HyperVSocketErrorTextWriter(this._hypervSocketServer.TextWriter);
		}

		// Token: 0x060023FD RID: 9213 RVA: 0x000C9F08 File Offset: 0x000C8108
		internal static void Run(string initialCommand)
		{
			lock (OutOfProcessMediatorBase.SyncObject)
			{
				HyperVSocketMediator.Instance = new HyperVSocketMediator();
			}
			AppDomain.CurrentDomain.UnhandledException += OutOfProcessMediatorBase.AppDomainUnhandledException;
			HyperVSocketMediator.Instance.Start(initialCommand);
		}

		// Token: 0x040011B7 RID: 4535
		private static HyperVSocketMediator Instance;

		// Token: 0x040011B8 RID: 4536
		private readonly RemoteSessionHyperVSocketServer _hypervSocketServer;
	}
}
