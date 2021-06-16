using System;
using Microsoft.Xde.Interface;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200001D RID: 29
	public class EnabledStateChangedEventArgs : EventArgs
	{
		// Token: 0x060000B1 RID: 177 RVA: 0x00004B4B File Offset: 0x00002D4B
		public EnabledStateChangedEventArgs(VirtualMachineEnabledState state)
		{
			this.EnabledState = state;
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00004B5A File Offset: 0x00002D5A
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x00004B62 File Offset: 0x00002D62
		public VirtualMachineEnabledState EnabledState { get; private set; }
	}
}
