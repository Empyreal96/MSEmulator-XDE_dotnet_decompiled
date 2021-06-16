using System;

namespace System.Management.Automation
{
	// Token: 0x02000271 RID: 625
	public sealed class JobStateEventArgs : EventArgs
	{
		// Token: 0x06001D76 RID: 7542 RVA: 0x000AA0C8 File Offset: 0x000A82C8
		public JobStateEventArgs(JobStateInfo jobStateInfo) : this(jobStateInfo, null)
		{
		}

		// Token: 0x06001D77 RID: 7543 RVA: 0x000AA0D2 File Offset: 0x000A82D2
		public JobStateEventArgs(JobStateInfo jobStateInfo, JobStateInfo previousJobStateInfo)
		{
			if (jobStateInfo == null)
			{
				throw PSTraceSource.NewArgumentNullException("jobStateInfo");
			}
			this._jobStateInfo = jobStateInfo;
			this._previousJobStateInfo = previousJobStateInfo;
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06001D78 RID: 7544 RVA: 0x000AA0F6 File Offset: 0x000A82F6
		public JobStateInfo JobStateInfo
		{
			get
			{
				return this._jobStateInfo;
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06001D79 RID: 7545 RVA: 0x000AA0FE File Offset: 0x000A82FE
		public JobStateInfo PreviousJobStateInfo
		{
			get
			{
				return this._previousJobStateInfo;
			}
		}

		// Token: 0x04000D12 RID: 3346
		private readonly JobStateInfo _jobStateInfo;

		// Token: 0x04000D13 RID: 3347
		private readonly JobStateInfo _previousJobStateInfo;
	}
}
