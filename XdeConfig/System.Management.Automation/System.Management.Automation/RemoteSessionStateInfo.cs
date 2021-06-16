using System;

namespace System.Management.Automation
{
	// Token: 0x020002BA RID: 698
	internal class RemoteSessionStateInfo
	{
		// Token: 0x0600218E RID: 8590 RVA: 0x000C0DD3 File Offset: 0x000BEFD3
		internal RemoteSessionStateInfo(RemoteSessionState state) : this(state, null)
		{
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x000C0DDD File Offset: 0x000BEFDD
		internal RemoteSessionStateInfo(RemoteSessionState state, Exception reason)
		{
			this._state = state;
			this._reason = reason;
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x000C0DF3 File Offset: 0x000BEFF3
		internal RemoteSessionStateInfo(RemoteSessionStateInfo sessionStateInfo)
		{
			this._state = sessionStateInfo.State;
			this._reason = sessionStateInfo.Reason;
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x06002191 RID: 8593 RVA: 0x000C0E13 File Offset: 0x000BF013
		internal RemoteSessionState State
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06002192 RID: 8594 RVA: 0x000C0E1B File Offset: 0x000BF01B
		internal Exception Reason
		{
			get
			{
				return this._reason;
			}
		}

		// Token: 0x04000F1D RID: 3869
		private RemoteSessionState _state;

		// Token: 0x04000F1E RID: 3870
		private Exception _reason;
	}
}
