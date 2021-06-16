using System;
using System.Diagnostics;
using System.Diagnostics.Eventing;

namespace System.Management.Automation.Tracing
{
	// Token: 0x020008FE RID: 2302
	public class EtwEventCorrelator : IEtwEventCorrelator
	{
		// Token: 0x06005623 RID: 22051 RVA: 0x001C3BA8 File Offset: 0x001C1DA8
		public EtwEventCorrelator(EventProvider transferProvider, EventDescriptor transferEvent)
		{
			if (transferProvider == null)
			{
				throw new ArgumentNullException("transferProvider");
			}
			this._transferProvider = transferProvider;
			this._transferEvent = transferEvent;
		}

		// Token: 0x17001193 RID: 4499
		// (get) Token: 0x06005624 RID: 22052 RVA: 0x001C3BCC File Offset: 0x001C1DCC
		// (set) Token: 0x06005625 RID: 22053 RVA: 0x001C3BD8 File Offset: 0x001C1DD8
		public Guid CurrentActivityId
		{
			get
			{
				return Trace.CorrelationManager.ActivityId;
			}
			set
			{
				EventProvider.SetActivityId(ref value);
			}
		}

		// Token: 0x06005626 RID: 22054 RVA: 0x001C3BE4 File Offset: 0x001C1DE4
		public IEtwActivityReverter StartActivity(Guid relatedActivityId)
		{
			EtwActivityReverter result = new EtwActivityReverter(this, this.CurrentActivityId);
			this.CurrentActivityId = EventProvider.CreateActivityId();
			if (relatedActivityId != Guid.Empty)
			{
				EventDescriptor transferEvent = this._transferEvent;
				this._transferProvider.WriteTransferEvent(ref transferEvent, relatedActivityId, new object[0]);
			}
			return result;
		}

		// Token: 0x06005627 RID: 22055 RVA: 0x001C3C33 File Offset: 0x001C1E33
		public IEtwActivityReverter StartActivity()
		{
			return this.StartActivity(this.CurrentActivityId);
		}

		// Token: 0x04002DD4 RID: 11732
		private readonly EventProvider _transferProvider;

		// Token: 0x04002DD5 RID: 11733
		private readonly EventDescriptor _transferEvent;
	}
}
