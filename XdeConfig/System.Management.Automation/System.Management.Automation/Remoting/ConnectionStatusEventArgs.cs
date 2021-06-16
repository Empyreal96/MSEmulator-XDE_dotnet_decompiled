using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x0200035F RID: 863
	internal class ConnectionStatusEventArgs : EventArgs
	{
		// Token: 0x06002AAD RID: 10925 RVA: 0x000EB489 File Offset: 0x000E9689
		internal ConnectionStatusEventArgs(ConnectionStatus notification)
		{
			this._notification = notification;
		}

		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x06002AAE RID: 10926 RVA: 0x000EB498 File Offset: 0x000E9698
		internal ConnectionStatus Notification
		{
			get
			{
				return this._notification;
			}
		}

		// Token: 0x0400152E RID: 5422
		private ConnectionStatus _notification;
	}
}
