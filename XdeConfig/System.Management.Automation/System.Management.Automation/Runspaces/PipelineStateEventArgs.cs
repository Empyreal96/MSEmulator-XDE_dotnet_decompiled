using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200022E RID: 558
	public sealed class PipelineStateEventArgs : EventArgs
	{
		// Token: 0x06001A19 RID: 6681 RVA: 0x0009B68D File Offset: 0x0009988D
		internal PipelineStateEventArgs(PipelineStateInfo pipelineStateInfo)
		{
			this._pipelineStateInfo = pipelineStateInfo;
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06001A1A RID: 6682 RVA: 0x0009B69C File Offset: 0x0009989C
		public PipelineStateInfo PipelineStateInfo
		{
			get
			{
				return this._pipelineStateInfo;
			}
		}

		// Token: 0x04000AC2 RID: 2754
		private PipelineStateInfo _pipelineStateInfo;
	}
}
