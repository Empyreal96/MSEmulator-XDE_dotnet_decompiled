using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020001F2 RID: 498
	public sealed class RunspaceStateEventArgs : EventArgs
	{
		// Token: 0x0600169B RID: 5787 RVA: 0x0009021F File Offset: 0x0008E41F
		internal RunspaceStateEventArgs(RunspaceStateInfo runspaceStateInfo)
		{
			if (runspaceStateInfo == null)
			{
				throw PSTraceSource.NewArgumentNullException("runspaceStateInfo");
			}
			this._runspaceStateInfo = runspaceStateInfo;
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x0600169C RID: 5788 RVA: 0x0009023C File Offset: 0x0008E43C
		public RunspaceStateInfo RunspaceStateInfo
		{
			get
			{
				return this._runspaceStateInfo;
			}
		}

		// Token: 0x040009B7 RID: 2487
		private RunspaceStateInfo _runspaceStateInfo;
	}
}
