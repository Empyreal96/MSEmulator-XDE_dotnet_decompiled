using System;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation.Internal
{
	// Token: 0x020000FD RID: 253
	public sealed class PSStandaloneMonitorRunspaceInfo : PSMonitorRunspaceInfo
	{
		// Token: 0x06000DEF RID: 3567 RVA: 0x0004BB55 File Offset: 0x00049D55
		public PSStandaloneMonitorRunspaceInfo(Runspace runspace) : base(runspace, PSMonitorRunspaceType.Standalone)
		{
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x0004BB5F File Offset: 0x00049D5F
		internal override PSMonitorRunspaceInfo Copy()
		{
			return new PSStandaloneMonitorRunspaceInfo(base.Runspace);
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x0004BB6C File Offset: 0x00049D6C
		internal override NestedRunspaceDebugger CreateDebugger(Debugger rootDebugger)
		{
			return new StandaloneRunspaceDebugger(base.Runspace);
		}
	}
}
