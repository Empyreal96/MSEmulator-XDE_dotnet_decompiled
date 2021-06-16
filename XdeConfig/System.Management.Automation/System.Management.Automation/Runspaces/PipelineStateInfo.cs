using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200022D RID: 557
	public sealed class PipelineStateInfo
	{
		// Token: 0x06001A13 RID: 6675 RVA: 0x0009B635 File Offset: 0x00099835
		internal PipelineStateInfo(PipelineState state) : this(state, null)
		{
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x0009B63F File Offset: 0x0009983F
		internal PipelineStateInfo(PipelineState state, Exception reason)
		{
			this._state = state;
			this._reason = reason;
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x0009B655 File Offset: 0x00099855
		internal PipelineStateInfo(PipelineStateInfo pipelineStateInfo)
		{
			this._state = pipelineStateInfo.State;
			this._reason = pipelineStateInfo.Reason;
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06001A16 RID: 6678 RVA: 0x0009B675 File Offset: 0x00099875
		public PipelineState State
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06001A17 RID: 6679 RVA: 0x0009B67D File Offset: 0x0009987D
		public Exception Reason
		{
			get
			{
				return this._reason;
			}
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x0009B685 File Offset: 0x00099885
		internal PipelineStateInfo Clone()
		{
			return new PipelineStateInfo(this);
		}

		// Token: 0x04000AC0 RID: 2752
		private PipelineState _state;

		// Token: 0x04000AC1 RID: 2753
		private Exception _reason;
	}
}
