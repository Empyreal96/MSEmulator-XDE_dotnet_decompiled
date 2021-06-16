using System;
using System.Management.Automation.Remoting;

namespace System.Management.Automation
{
	// Token: 0x020002B5 RID: 693
	internal sealed class RemoteSessionNegotiationEventArgs : EventArgs
	{
		// Token: 0x06002186 RID: 8582 RVA: 0x000C0D5C File Offset: 0x000BEF5C
		internal RemoteSessionNegotiationEventArgs(RemoteSessionCapability remoteSessionCapability)
		{
			if (remoteSessionCapability == null)
			{
				throw PSTraceSource.NewArgumentNullException("remoteSessionCapability");
			}
			this._remoteSessionCapability = remoteSessionCapability;
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06002187 RID: 8583 RVA: 0x000C0D79 File Offset: 0x000BEF79
		internal RemoteSessionCapability RemoteSessionCapability
		{
			get
			{
				return this._remoteSessionCapability;
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06002188 RID: 8584 RVA: 0x000C0D81 File Offset: 0x000BEF81
		// (set) Token: 0x06002189 RID: 8585 RVA: 0x000C0D89 File Offset: 0x000BEF89
		internal RemoteDataObject<PSObject> RemoteData
		{
			get
			{
				return this._remoteObject;
			}
			set
			{
				this._remoteObject = value;
			}
		}

		// Token: 0x04000EE1 RID: 3809
		private RemoteSessionCapability _remoteSessionCapability;

		// Token: 0x04000EE2 RID: 3810
		private RemoteDataObject<PSObject> _remoteObject;
	}
}
