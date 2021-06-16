using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020001F4 RID: 500
	public sealed class RunspaceAvailabilityEventArgs : EventArgs
	{
		// Token: 0x0600169D RID: 5789 RVA: 0x00090244 File Offset: 0x0008E444
		internal RunspaceAvailabilityEventArgs(RunspaceAvailability runspaceAvailability)
		{
			this._runspaceAvailability = runspaceAvailability;
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x0600169E RID: 5790 RVA: 0x00090253 File Offset: 0x0008E453
		public RunspaceAvailability RunspaceAvailability
		{
			get
			{
				return this._runspaceAvailability;
			}
		}

		// Token: 0x040009BE RID: 2494
		private RunspaceAvailability _runspaceAvailability;
	}
}
