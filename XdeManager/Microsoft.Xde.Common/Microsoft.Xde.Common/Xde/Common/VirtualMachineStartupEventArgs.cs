using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000075 RID: 117
	public class VirtualMachineStartupEventArgs : EventArgs
	{
		// Token: 0x060002C1 RID: 705 RVA: 0x0000785B File Offset: 0x00005A5B
		public VirtualMachineStartupEventArgs(bool isUsingSnapshot)
		{
			this.IsUsingSnapshot = isUsingSnapshot;
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x0000786A File Offset: 0x00005A6A
		// (set) Token: 0x060002C3 RID: 707 RVA: 0x00007872 File Offset: 0x00005A72
		public bool IsUsingSnapshot { get; private set; }
	}
}
