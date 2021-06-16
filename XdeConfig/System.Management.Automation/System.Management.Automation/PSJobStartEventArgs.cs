using System;

namespace System.Management.Automation
{
	// Token: 0x020000E7 RID: 231
	public sealed class PSJobStartEventArgs : EventArgs
	{
		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x000464B3 File Offset: 0x000446B3
		// (set) Token: 0x06000CC4 RID: 3268 RVA: 0x000464BB File Offset: 0x000446BB
		public Job Job { get; private set; }

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x000464C4 File Offset: 0x000446C4
		// (set) Token: 0x06000CC6 RID: 3270 RVA: 0x000464CC File Offset: 0x000446CC
		public Debugger Debugger { get; private set; }

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x000464D5 File Offset: 0x000446D5
		// (set) Token: 0x06000CC8 RID: 3272 RVA: 0x000464DD File Offset: 0x000446DD
		public bool IsAsync { get; private set; }

		// Token: 0x06000CC9 RID: 3273 RVA: 0x000464E6 File Offset: 0x000446E6
		public PSJobStartEventArgs(Job job, Debugger debugger, bool isAsync)
		{
			this.Job = job;
			this.Debugger = debugger;
			this.IsAsync = isAsync;
		}
	}
}
