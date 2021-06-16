using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002FD RID: 765
	internal class ServerRemoteSessionContext
	{
		// Token: 0x06002408 RID: 9224 RVA: 0x000CA0F9 File Offset: 0x000C82F9
		internal ServerRemoteSessionContext()
		{
			this._serverCapability = RemoteSessionCapability.CreateServerCapability();
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06002409 RID: 9225 RVA: 0x000CA10C File Offset: 0x000C830C
		// (set) Token: 0x0600240A RID: 9226 RVA: 0x000CA114 File Offset: 0x000C8314
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

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x0600240B RID: 9227 RVA: 0x000CA11D File Offset: 0x000C831D
		// (set) Token: 0x0600240C RID: 9228 RVA: 0x000CA125 File Offset: 0x000C8325
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

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x0600240D RID: 9229 RVA: 0x000CA12E File Offset: 0x000C832E
		// (set) Token: 0x0600240E RID: 9230 RVA: 0x000CA136 File Offset: 0x000C8336
		internal bool IsNegotiationSucceeded
		{
			get
			{
				return this.isNegotiationSucceeded;
			}
			set
			{
				this.isNegotiationSucceeded = value;
			}
		}

		// Token: 0x040011C0 RID: 4544
		private RemoteSessionCapability _clientCapability;

		// Token: 0x040011C1 RID: 4545
		private RemoteSessionCapability _serverCapability;

		// Token: 0x040011C2 RID: 4546
		private bool isNegotiationSucceeded;
	}
}
