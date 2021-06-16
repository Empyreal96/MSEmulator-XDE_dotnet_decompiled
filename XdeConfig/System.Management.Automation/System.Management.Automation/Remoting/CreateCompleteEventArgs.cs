using System;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000360 RID: 864
	internal class CreateCompleteEventArgs : EventArgs
	{
		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x06002AAF RID: 10927 RVA: 0x000EB4A0 File Offset: 0x000E96A0
		internal RunspaceConnectionInfo ConnectionInfo
		{
			get
			{
				return this._connectionInfo;
			}
		}

		// Token: 0x06002AB0 RID: 10928 RVA: 0x000EB4A8 File Offset: 0x000E96A8
		internal CreateCompleteEventArgs(RunspaceConnectionInfo connectionInfo)
		{
			this._connectionInfo = connectionInfo;
		}

		// Token: 0x0400152F RID: 5423
		private readonly RunspaceConnectionInfo _connectionInfo;
	}
}
