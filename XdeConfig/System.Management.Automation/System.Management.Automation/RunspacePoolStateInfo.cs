using System;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x02000257 RID: 599
	public sealed class RunspacePoolStateInfo
	{
		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06001C5D RID: 7261 RVA: 0x000A506F File Offset: 0x000A326F
		public RunspacePoolState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06001C5E RID: 7262 RVA: 0x000A5077 File Offset: 0x000A3277
		public Exception Reason
		{
			get
			{
				return this.reason;
			}
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x000A507F File Offset: 0x000A327F
		public RunspacePoolStateInfo(RunspacePoolState state, Exception reason)
		{
			this.state = state;
			this.reason = reason;
		}

		// Token: 0x04000BB8 RID: 3000
		private RunspacePoolState state;

		// Token: 0x04000BB9 RID: 3001
		private Exception reason;
	}
}
