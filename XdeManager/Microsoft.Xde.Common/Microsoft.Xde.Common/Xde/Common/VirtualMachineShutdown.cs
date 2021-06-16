using System;
using System.Threading;
using Microsoft.Xde.Interface;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000074 RID: 116
	public sealed class VirtualMachineShutdown : IVirtualMachineShutdown, IDisposable
	{
		// Token: 0x060002BD RID: 701 RVA: 0x00007790 File Offset: 0x00005990
		public void ShutdownVirtualMachine(IXdeVirtualMachine virtualMachine, IXdeAutomationSimpleCommandsPipe simplePipe, int timeout)
		{
			ValidationUtilities.CheckNotNull(virtualMachine, "virtualMachine");
			if (virtualMachine.EnabledState != VirtualMachineEnabledState.Enabled)
			{
				return;
			}
			bool flag = false;
			if (simplePipe != null && simplePipe.IsConnected)
			{
				this.doneEvent.Reset();
				if (timeout != 0)
				{
					virtualMachine.EnableStateChanged += this.VirtualMachine_EnableStateChanged;
					virtualMachine.IntentionalShutdownComing();
					simplePipe.InitiateSystemShutdown();
					flag = this.doneEvent.WaitOne(timeout);
					virtualMachine.EnableStateChanged -= this.VirtualMachine_EnableStateChanged;
				}
			}
			if (!flag && virtualMachine.EnabledState == VirtualMachineEnabledState.Enabled)
			{
				virtualMachine.Stop();
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000781D File Offset: 0x00005A1D
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			this.doneEvent.Dispose();
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00007830 File Offset: 0x00005A30
		private void VirtualMachine_EnableStateChanged(object sender, EnabledStateChangedEventArgs e)
		{
			if (e.EnabledState == VirtualMachineEnabledState.Disabled)
			{
				this.doneEvent.Set();
			}
		}

		// Token: 0x040001B0 RID: 432
		private ManualResetEvent doneEvent = new ManualResetEvent(false);
	}
}
