using System;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation.Internal
{
	// Token: 0x020000FC RID: 252
	public abstract class PSMonitorRunspaceInfo
	{
		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000DE5 RID: 3557 RVA: 0x0004BAF6 File Offset: 0x00049CF6
		// (set) Token: 0x06000DE6 RID: 3558 RVA: 0x0004BAFE File Offset: 0x00049CFE
		public Runspace Runspace { get; private set; }

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000DE7 RID: 3559 RVA: 0x0004BB07 File Offset: 0x00049D07
		// (set) Token: 0x06000DE8 RID: 3560 RVA: 0x0004BB0F File Offset: 0x00049D0F
		public PSMonitorRunspaceType RunspaceType { get; private set; }

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000DE9 RID: 3561 RVA: 0x0004BB18 File Offset: 0x00049D18
		// (set) Token: 0x06000DEA RID: 3562 RVA: 0x0004BB20 File Offset: 0x00049D20
		internal NestedRunspaceDebugger NestedDebugger { get; set; }

		// Token: 0x06000DEB RID: 3563 RVA: 0x0004BB29 File Offset: 0x00049D29
		private PSMonitorRunspaceInfo()
		{
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x0004BB31 File Offset: 0x00049D31
		protected PSMonitorRunspaceInfo(Runspace runspace, PSMonitorRunspaceType runspaceType)
		{
			if (runspace == null)
			{
				throw new PSArgumentNullException("runspace");
			}
			this.Runspace = runspace;
			this.RunspaceType = runspaceType;
		}

		// Token: 0x06000DED RID: 3565
		internal abstract PSMonitorRunspaceInfo Copy();

		// Token: 0x06000DEE RID: 3566
		internal abstract NestedRunspaceDebugger CreateDebugger(Debugger rootDebugger);
	}
}
