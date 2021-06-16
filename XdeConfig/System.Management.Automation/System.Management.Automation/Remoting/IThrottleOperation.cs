using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000214 RID: 532
	internal abstract class IThrottleOperation
	{
		// Token: 0x06001905 RID: 6405
		internal abstract void StartOperation();

		// Token: 0x06001906 RID: 6406
		internal abstract void StopOperation();

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06001907 RID: 6407
		// (remove) Token: 0x06001908 RID: 6408
		internal abstract event EventHandler<OperationStateEventArgs> OperationComplete;

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06001909 RID: 6409 RVA: 0x0009810A File Offset: 0x0009630A
		// (set) Token: 0x0600190A RID: 6410 RVA: 0x00098112 File Offset: 0x00096312
		internal bool IgnoreStop
		{
			get
			{
				return this._ignoreStop;
			}
			set
			{
				this._ignoreStop = true;
			}
		}

		// Token: 0x04000A51 RID: 2641
		private bool _ignoreStop;
	}
}
