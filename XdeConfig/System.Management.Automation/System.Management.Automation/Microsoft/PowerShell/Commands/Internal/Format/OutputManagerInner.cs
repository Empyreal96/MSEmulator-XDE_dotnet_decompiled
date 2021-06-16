using System;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000528 RID: 1320
	internal sealed class OutputManagerInner : ImplementationCommandBase
	{
		// Token: 0x17000C34 RID: 3124
		// (set) Token: 0x06003732 RID: 14130 RVA: 0x001297AC File Offset: 0x001279AC
		internal LineOutput LineOutput
		{
			set
			{
				lock (this.syncRoot)
				{
					this.lo = value;
					if (this.isStopped)
					{
						this.lo.StopProcessing();
					}
				}
			}
		}

		// Token: 0x06003733 RID: 14131 RVA: 0x00129800 File Offset: 0x00127A00
		internal override void ProcessRecord()
		{
			PSObject psobject = this.ReadObject();
			if (psobject == null || psobject == AutomationNull.Value)
			{
				return;
			}
			if (this.mgr == null)
			{
				this.mgr = new SubPipelineManager();
				this.mgr.Initialize(this.lo, this.OuterCmdlet().Context);
			}
			this.mgr.Process(psobject);
		}

		// Token: 0x06003734 RID: 14132 RVA: 0x0012985B File Offset: 0x00127A5B
		internal override void EndProcessing()
		{
			if (this.mgr != null)
			{
				this.mgr.ShutDown();
			}
		}

		// Token: 0x06003735 RID: 14133 RVA: 0x00129870 File Offset: 0x00127A70
		internal override void StopProcessing()
		{
			lock (this.syncRoot)
			{
				if (this.lo != null)
				{
					this.lo.StopProcessing();
				}
				this.isStopped = true;
			}
		}

		// Token: 0x06003736 RID: 14134 RVA: 0x001298C4 File Offset: 0x00127AC4
		protected override void InternalDispose()
		{
			base.InternalDispose();
			if (this.mgr != null)
			{
				this.mgr.Dispose();
				this.mgr = null;
			}
		}

		// Token: 0x04001C3F RID: 7231
		[TraceSource("format_out_OutputManagerInner", "OutputManagerInner")]
		internal static PSTraceSource tracer = PSTraceSource.GetTracer("format_out_OutputManagerInner", "OutputManagerInner");

		// Token: 0x04001C40 RID: 7232
		private LineOutput lo;

		// Token: 0x04001C41 RID: 7233
		private SubPipelineManager mgr;

		// Token: 0x04001C42 RID: 7234
		private bool isStopped;

		// Token: 0x04001C43 RID: 7235
		private object syncRoot = new object();
	}
}
