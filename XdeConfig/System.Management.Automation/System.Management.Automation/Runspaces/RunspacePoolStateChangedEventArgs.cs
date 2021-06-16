using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000245 RID: 581
	public sealed class RunspacePoolStateChangedEventArgs : EventArgs
	{
		// Token: 0x06001B85 RID: 7045 RVA: 0x000A18F6 File Offset: 0x0009FAF6
		internal RunspacePoolStateChangedEventArgs(RunspacePoolState state)
		{
			this.stateInfo = new RunspacePoolStateInfo(state, null);
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x000A190B File Offset: 0x0009FB0B
		internal RunspacePoolStateChangedEventArgs(RunspacePoolStateInfo stateInfo)
		{
			this.stateInfo = stateInfo;
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06001B87 RID: 7047 RVA: 0x000A191A File Offset: 0x0009FB1A
		public RunspacePoolStateInfo RunspacePoolStateInfo
		{
			get
			{
				return this.stateInfo;
			}
		}

		// Token: 0x04000B4C RID: 2892
		private RunspacePoolStateInfo stateInfo;
	}
}
