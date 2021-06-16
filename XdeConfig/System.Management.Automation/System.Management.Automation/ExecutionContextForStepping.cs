using System;
using System.Management.Automation.Host;

namespace System.Management.Automation
{
	// Token: 0x02000301 RID: 769
	internal class ExecutionContextForStepping : IDisposable
	{
		// Token: 0x06002453 RID: 9299 RVA: 0x000CC437 File Offset: 0x000CA637
		private ExecutionContextForStepping(ExecutionContext ctxt)
		{
			this.executionContext = ctxt;
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x000CC448 File Offset: 0x000CA648
		internal static ExecutionContextForStepping PrepareExecutionContext(ExecutionContext ctxt, PSInformationalBuffers newBuffers, PSHost newHost)
		{
			ExecutionContextForStepping executionContextForStepping = new ExecutionContextForStepping(ctxt);
			executionContextForStepping.originalInformationalBuffers = ctxt.InternalHost.InternalUI.GetInformationalMessageBuffers();
			executionContextForStepping.originalHost = ctxt.InternalHost.ExternalHost;
			ctxt.InternalHost.InternalUI.SetInformationalMessageBuffers(newBuffers);
			ctxt.InternalHost.SetHostRef(newHost);
			return executionContextForStepping;
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x000CC4A1 File Offset: 0x000CA6A1
		void IDisposable.Dispose()
		{
			this.executionContext.InternalHost.InternalUI.SetInformationalMessageBuffers(this.originalInformationalBuffers);
			this.executionContext.InternalHost.SetHostRef(this.originalHost);
			GC.SuppressFinalize(this);
		}

		// Token: 0x040011D8 RID: 4568
		private ExecutionContext executionContext;

		// Token: 0x040011D9 RID: 4569
		private PSInformationalBuffers originalInformationalBuffers;

		// Token: 0x040011DA RID: 4570
		private PSHost originalHost;
	}
}
