using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000246 RID: 582
	internal sealed class RunspaceCreatedEventArgs : EventArgs
	{
		// Token: 0x06001B88 RID: 7048 RVA: 0x000A1922 File Offset: 0x0009FB22
		internal RunspaceCreatedEventArgs(Runspace runspace)
		{
			this.runspace = runspace;
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06001B89 RID: 7049 RVA: 0x000A1931 File Offset: 0x0009FB31
		internal Runspace Runspace
		{
			get
			{
				return this.runspace;
			}
		}

		// Token: 0x04000B4D RID: 2893
		private Runspace runspace;
	}
}
