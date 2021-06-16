using System;

namespace System.Management.Automation
{
	// Token: 0x02000270 RID: 624
	public sealed class JobStateInfo
	{
		// Token: 0x06001D6F RID: 7535 RVA: 0x000AA05E File Offset: 0x000A825E
		public JobStateInfo(JobState state) : this(state, null)
		{
		}

		// Token: 0x06001D70 RID: 7536 RVA: 0x000AA068 File Offset: 0x000A8268
		public JobStateInfo(JobState state, Exception reason)
		{
			this._state = state;
			this._reason = reason;
		}

		// Token: 0x06001D71 RID: 7537 RVA: 0x000AA07E File Offset: 0x000A827E
		internal JobStateInfo(JobStateInfo jobStateInfo)
		{
			this._state = jobStateInfo.State;
			this._reason = jobStateInfo.Reason;
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06001D72 RID: 7538 RVA: 0x000AA09E File Offset: 0x000A829E
		public JobState State
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06001D73 RID: 7539 RVA: 0x000AA0A6 File Offset: 0x000A82A6
		public Exception Reason
		{
			get
			{
				return this._reason;
			}
		}

		// Token: 0x06001D74 RID: 7540 RVA: 0x000AA0AE File Offset: 0x000A82AE
		public override string ToString()
		{
			return this._state.ToString();
		}

		// Token: 0x06001D75 RID: 7541 RVA: 0x000AA0C0 File Offset: 0x000A82C0
		internal JobStateInfo Clone()
		{
			return new JobStateInfo(this);
		}

		// Token: 0x04000D10 RID: 3344
		private JobState _state;

		// Token: 0x04000D11 RID: 3345
		private Exception _reason;
	}
}
