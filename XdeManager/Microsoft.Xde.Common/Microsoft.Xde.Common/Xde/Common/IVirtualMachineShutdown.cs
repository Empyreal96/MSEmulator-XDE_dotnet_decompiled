using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000048 RID: 72
	public interface IVirtualMachineShutdown
	{
		// Token: 0x06000192 RID: 402
		void ShutdownVirtualMachine(IXdeVirtualMachine virtualMachine, IXdeAutomationSimpleCommandsPipe simplePipe, int timeout);
	}
}
