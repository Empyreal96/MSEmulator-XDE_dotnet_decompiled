using System;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation.Internal
{
	// Token: 0x020000FE RID: 254
	public sealed class PSEmbeddedMonitorRunspaceInfo : PSMonitorRunspaceInfo
	{
		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000DF2 RID: 3570 RVA: 0x0004BB79 File Offset: 0x00049D79
		// (set) Token: 0x06000DF3 RID: 3571 RVA: 0x0004BB81 File Offset: 0x00049D81
		public PowerShell Command { get; private set; }

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000DF4 RID: 3572 RVA: 0x0004BB8A File Offset: 0x00049D8A
		// (set) Token: 0x06000DF5 RID: 3573 RVA: 0x0004BB92 File Offset: 0x00049D92
		public Guid ParentDebuggerId { get; private set; }

		// Token: 0x06000DF6 RID: 3574 RVA: 0x0004BB9B File Offset: 0x00049D9B
		public PSEmbeddedMonitorRunspaceInfo(Runspace runspace, PSMonitorRunspaceType runspaceType, PowerShell command, Guid parentDebuggerId) : base(runspace, runspaceType)
		{
			this.Command = command;
			this.ParentDebuggerId = parentDebuggerId;
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x0004BBB4 File Offset: 0x00049DB4
		internal override PSMonitorRunspaceInfo Copy()
		{
			return new PSEmbeddedMonitorRunspaceInfo(base.Runspace, base.RunspaceType, this.Command, this.ParentDebuggerId);
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0004BBD3 File Offset: 0x00049DD3
		internal override NestedRunspaceDebugger CreateDebugger(Debugger rootDebugger)
		{
			return new EmbeddedRunspaceDebugger(base.Runspace, this.Command, rootDebugger, base.RunspaceType, this.ParentDebuggerId);
		}
	}
}
