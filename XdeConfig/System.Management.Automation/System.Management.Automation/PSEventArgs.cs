using System;

namespace System.Management.Automation
{
	// Token: 0x020000D2 RID: 210
	internal class PSEventArgs<T> : EventArgs
	{
		// Token: 0x06000BD4 RID: 3028 RVA: 0x00043CEC File Offset: 0x00041EEC
		public PSEventArgs(T args)
		{
			this.Args = args;
		}

		// Token: 0x04000545 RID: 1349
		internal T Args;
	}
}
