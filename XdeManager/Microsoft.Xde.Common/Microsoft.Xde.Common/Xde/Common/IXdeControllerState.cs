using System;
using System.ComponentModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000029 RID: 41
	public interface IXdeControllerState : INotifyPropertyChanged
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000111 RID: 273
		// (remove) Token: 0x06000112 RID: 274
		event EventHandler SkinChanged;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000113 RID: 275
		// (remove) Token: 0x06000114 RID: 276
		event EventHandler<VirtualMachineStartupEventArgs> VirtualMachineStarting;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000115 RID: 277
		// (remove) Token: 0x06000116 RID: 278
		event EventHandler VirtualMachineShuttingDown;

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000117 RID: 279
		bool IsShuttingDown { get; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000118 RID: 280
		IXdeVirtualMachine CurrentVirtualMachine { get; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000119 RID: 281
		IXdeSkin Skin { get; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600011A RID: 282
		// (set) Token: 0x0600011B RID: 283
		bool DeleteCheckpointsAfterReboot { get; set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600011C RID: 284
		bool IsGuestOSViewDisplayed { get; }
	}
}
