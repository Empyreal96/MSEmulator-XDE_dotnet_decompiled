using System;

namespace System.Management.Automation.Tracing
{
	// Token: 0x020008FA RID: 2298
	internal class EtwActivityReverter : IEtwActivityReverter, IDisposable
	{
		// Token: 0x06005616 RID: 22038 RVA: 0x001C3AB7 File Offset: 0x001C1CB7
		public EtwActivityReverter(IEtwEventCorrelator correlator, Guid oldActivityId)
		{
			this._correlator = correlator;
			this._oldActivityId = oldActivityId;
		}

		// Token: 0x06005617 RID: 22039 RVA: 0x001C3ACD File Offset: 0x001C1CCD
		public void RevertCurrentActivityId()
		{
			this.Dispose();
		}

		// Token: 0x06005618 RID: 22040 RVA: 0x001C3AD5 File Offset: 0x001C1CD5
		public void Dispose()
		{
			if (!this._isDisposed)
			{
				this._correlator.CurrentActivityId = this._oldActivityId;
				this._isDisposed = true;
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x04002DCF RID: 11727
		private readonly IEtwEventCorrelator _correlator;

		// Token: 0x04002DD0 RID: 11728
		private readonly Guid _oldActivityId;

		// Token: 0x04002DD1 RID: 11729
		private bool _isDisposed;
	}
}
