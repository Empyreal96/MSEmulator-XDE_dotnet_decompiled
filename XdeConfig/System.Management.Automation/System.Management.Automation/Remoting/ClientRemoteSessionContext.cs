using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000269 RID: 617
	internal class ClientRemoteSessionContext
	{
		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06001D2F RID: 7471 RVA: 0x000A976D File Offset: 0x000A796D
		// (set) Token: 0x06001D30 RID: 7472 RVA: 0x000A9775 File Offset: 0x000A7975
		internal Uri RemoteAddress
		{
			get
			{
				return this._remoteAddress;
			}
			set
			{
				this._remoteAddress = value;
			}
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06001D31 RID: 7473 RVA: 0x000A977E File Offset: 0x000A797E
		// (set) Token: 0x06001D32 RID: 7474 RVA: 0x000A9786 File Offset: 0x000A7986
		internal PSCredential UserCredential
		{
			get
			{
				return this._userCredential;
			}
			set
			{
				this._userCredential = value;
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06001D33 RID: 7475 RVA: 0x000A978F File Offset: 0x000A798F
		// (set) Token: 0x06001D34 RID: 7476 RVA: 0x000A9797 File Offset: 0x000A7997
		internal RemoteSessionCapability ClientCapability
		{
			get
			{
				return this._clientCapability;
			}
			set
			{
				this._clientCapability = value;
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06001D35 RID: 7477 RVA: 0x000A97A0 File Offset: 0x000A79A0
		// (set) Token: 0x06001D36 RID: 7478 RVA: 0x000A97A8 File Offset: 0x000A79A8
		internal RemoteSessionCapability ServerCapability
		{
			get
			{
				return this._serverCapability;
			}
			set
			{
				this._serverCapability = value;
			}
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x06001D37 RID: 7479 RVA: 0x000A97B1 File Offset: 0x000A79B1
		// (set) Token: 0x06001D38 RID: 7480 RVA: 0x000A97B9 File Offset: 0x000A79B9
		internal string ShellName
		{
			get
			{
				return this._shellName;
			}
			set
			{
				this._shellName = value;
			}
		}

		// Token: 0x04000CF2 RID: 3314
		private Uri _remoteAddress;

		// Token: 0x04000CF3 RID: 3315
		private PSCredential _userCredential;

		// Token: 0x04000CF4 RID: 3316
		private RemoteSessionCapability _clientCapability;

		// Token: 0x04000CF5 RID: 3317
		private RemoteSessionCapability _serverCapability;

		// Token: 0x04000CF6 RID: 3318
		private string _shellName;
	}
}
