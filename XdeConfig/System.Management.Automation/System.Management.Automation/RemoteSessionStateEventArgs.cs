using System;

namespace System.Management.Automation
{
	// Token: 0x020002BB RID: 699
	internal class RemoteSessionStateEventArgs : EventArgs
	{
		// Token: 0x06002193 RID: 8595 RVA: 0x000C0E23 File Offset: 0x000BF023
		internal RemoteSessionStateEventArgs(RemoteSessionStateInfo remoteSessionStateInfo)
		{
			if (remoteSessionStateInfo == null)
			{
				PSTraceSource.NewArgumentNullException("remoteSessionStateInfo");
			}
			this._remoteSessionStateInfo = remoteSessionStateInfo;
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06002194 RID: 8596 RVA: 0x000C0E40 File Offset: 0x000BF040
		public RemoteSessionStateInfo SessionStateInfo
		{
			get
			{
				return this._remoteSessionStateInfo;
			}
		}

		// Token: 0x04000F1F RID: 3871
		private RemoteSessionStateInfo _remoteSessionStateInfo;
	}
}
