using System;

namespace System.Management.Automation
{
	// Token: 0x02000233 RID: 563
	public sealed class PSInvocationStateChangedEventArgs : EventArgs
	{
		// Token: 0x06001A26 RID: 6694 RVA: 0x0009B759 File Offset: 0x00099959
		internal PSInvocationStateChangedEventArgs(PSInvocationStateInfo psStateInfo)
		{
			this.executionStateInfo = psStateInfo;
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06001A27 RID: 6695 RVA: 0x0009B768 File Offset: 0x00099968
		public PSInvocationStateInfo InvocationStateInfo
		{
			get
			{
				return this.executionStateInfo;
			}
		}

		// Token: 0x04000AD1 RID: 2769
		private PSInvocationStateInfo executionStateInfo;
	}
}
