using System;
using System.Management.Automation.Runspaces.Internal;

namespace System.Management.Automation.Remoting
{
	// Token: 0x0200026B RID: 619
	internal abstract class ClientRemoteSession : RemoteSession
	{
		// Token: 0x06001D41 RID: 7489
		public abstract void CreateAsync();

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06001D42 RID: 7490
		// (remove) Token: 0x06001D43 RID: 7491
		public abstract event EventHandler<RemoteSessionStateEventArgs> StateChanged;

		// Token: 0x06001D44 RID: 7492
		public abstract void CloseAsync();

		// Token: 0x06001D45 RID: 7493
		public abstract void DisconnectAsync();

		// Token: 0x06001D46 RID: 7494
		public abstract void ReconnectAsync();

		// Token: 0x06001D47 RID: 7495
		public abstract void ConnectAsync();

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06001D48 RID: 7496 RVA: 0x000A97F7 File Offset: 0x000A79F7
		internal ClientRemoteSessionContext Context
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06001D49 RID: 7497 RVA: 0x000A97FF File Offset: 0x000A79FF
		// (set) Token: 0x06001D4A RID: 7498 RVA: 0x000A9807 File Offset: 0x000A7A07
		internal ClientRemoteSessionDataStructureHandler SessionDataStructureHandler
		{
			get
			{
				return this._sessionDSHandler;
			}
			set
			{
				this._sessionDSHandler = value;
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06001D4B RID: 7499 RVA: 0x000A9810 File Offset: 0x000A7A10
		internal Version ServerProtocolVersion
		{
			get
			{
				return this._serverProtocolVersion;
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06001D4C RID: 7500 RVA: 0x000A9818 File Offset: 0x000A7A18
		// (set) Token: 0x06001D4D RID: 7501 RVA: 0x000A9820 File Offset: 0x000A7A20
		internal RemoteRunspacePoolInternal RemoteRunspacePoolInternal
		{
			get
			{
				return this.remoteRunspacePool;
			}
			set
			{
				this.remoteRunspacePool = value;
			}
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x000A982C File Offset: 0x000A7A2C
		internal RemoteRunspacePoolInternal GetRunspacePool(Guid clientRunspacePoolId)
		{
			if (this.remoteRunspacePool != null && this.remoteRunspacePool.InstanceId.Equals(clientRunspacePoolId))
			{
				return this.remoteRunspacePool;
			}
			return null;
		}

		// Token: 0x04000CF9 RID: 3321
		[TraceSource("CRSession", "ClientRemoteSession")]
		private static PSTraceSource _trace = PSTraceSource.GetTracer("CRSession", "ClientRemoteSession");

		// Token: 0x04000CFA RID: 3322
		private ClientRemoteSessionContext _context = new ClientRemoteSessionContext();

		// Token: 0x04000CFB RID: 3323
		private ClientRemoteSessionDataStructureHandler _sessionDSHandler;

		// Token: 0x04000CFC RID: 3324
		protected Version _serverProtocolVersion;

		// Token: 0x04000CFD RID: 3325
		private RemoteRunspacePoolInternal remoteRunspacePool;

		// Token: 0x0200026C RID: 620
		// (Invoke) Token: 0x06001D52 RID: 7506
		internal delegate void URIDirectionReported(Uri newURI);
	}
}
