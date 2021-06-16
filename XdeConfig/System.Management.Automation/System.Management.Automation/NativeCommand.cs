using System;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x02000030 RID: 48
	internal sealed class NativeCommand : InternalCommand
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0000934D File Offset: 0x0000754D
		// (set) Token: 0x0600024E RID: 590 RVA: 0x00009355 File Offset: 0x00007555
		internal NativeCommandProcessor MyCommandProcessor
		{
			get
			{
				return this.myCommandProcessor;
			}
			set
			{
				this.myCommandProcessor = value;
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00009360 File Offset: 0x00007560
		internal override void DoStopProcessing()
		{
			try
			{
				if (this.myCommandProcessor != null)
				{
					this.myCommandProcessor.StopProcessing();
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
		}

		// Token: 0x040000CF RID: 207
		private NativeCommandProcessor myCommandProcessor;
	}
}
