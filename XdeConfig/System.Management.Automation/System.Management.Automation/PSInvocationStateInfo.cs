using System;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x02000232 RID: 562
	public sealed class PSInvocationStateInfo
	{
		// Token: 0x06001A21 RID: 6689 RVA: 0x0009B700 File Offset: 0x00099900
		internal PSInvocationStateInfo(PSInvocationState state, Exception reason)
		{
			this.executionState = state;
			this.exceptionReason = reason;
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x0009B716 File Offset: 0x00099916
		internal PSInvocationStateInfo(PipelineStateInfo pipelineStateInfo)
		{
			this.executionState = (PSInvocationState)pipelineStateInfo.State;
			this.exceptionReason = pipelineStateInfo.Reason;
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06001A23 RID: 6691 RVA: 0x0009B736 File Offset: 0x00099936
		public PSInvocationState State
		{
			get
			{
				return this.executionState;
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06001A24 RID: 6692 RVA: 0x0009B73E File Offset: 0x0009993E
		public Exception Reason
		{
			get
			{
				return this.exceptionReason;
			}
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x0009B746 File Offset: 0x00099946
		internal PSInvocationStateInfo Clone()
		{
			return new PSInvocationStateInfo(this.executionState, this.exceptionReason);
		}

		// Token: 0x04000ACF RID: 2767
		private PSInvocationState executionState;

		// Token: 0x04000AD0 RID: 2768
		private Exception exceptionReason;
	}
}
