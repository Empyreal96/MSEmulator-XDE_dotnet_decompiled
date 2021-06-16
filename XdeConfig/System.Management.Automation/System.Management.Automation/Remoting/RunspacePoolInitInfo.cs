using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002D4 RID: 724
	internal class RunspacePoolInitInfo
	{
		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x060022B6 RID: 8886 RVA: 0x000C38CC File Offset: 0x000C1ACC
		internal int MinRunspaces
		{
			get
			{
				return this.minRunspaces;
			}
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x060022B7 RID: 8887 RVA: 0x000C38D4 File Offset: 0x000C1AD4
		internal int MaxRunspaces
		{
			get
			{
				return this.maxRunspaces;
			}
		}

		// Token: 0x060022B8 RID: 8888 RVA: 0x000C38DC File Offset: 0x000C1ADC
		internal RunspacePoolInitInfo(int minRS, int maxRS)
		{
			this.minRunspaces = minRS;
			this.maxRunspaces = maxRS;
		}

		// Token: 0x04001081 RID: 4225
		private int minRunspaces;

		// Token: 0x04001082 RID: 4226
		private int maxRunspaces;
	}
}
