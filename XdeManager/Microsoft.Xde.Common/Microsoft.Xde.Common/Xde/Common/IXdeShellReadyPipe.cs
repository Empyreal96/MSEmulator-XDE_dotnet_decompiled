using System;
using System.ComponentModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000057 RID: 87
	public interface IXdeShellReadyPipe : IXdePipe, IXdeAutomationPipe, INotifyPropertyChanged, IXdeConnectionController, IDisposable
	{
		// Token: 0x14000015 RID: 21
		// (add) Token: 0x060001B1 RID: 433
		// (remove) Token: 0x060001B2 RID: 434
		event EventHandler ShellReadyEvent;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060001B3 RID: 435
		// (remove) Token: 0x060001B4 RID: 436
		event EventHandler<ExEventArgs> ErrorEncountered;

		// Token: 0x060001B5 RID: 437
		void StartListening();

		// Token: 0x060001B6 RID: 438
		void StopListening();
	}
}
