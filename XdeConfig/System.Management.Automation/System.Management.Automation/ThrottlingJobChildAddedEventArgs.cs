using System;

namespace System.Management.Automation
{
	// Token: 0x02000293 RID: 659
	internal class ThrottlingJobChildAddedEventArgs : EventArgs
	{
		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06001F9D RID: 8093 RVA: 0x000B799C File Offset: 0x000B5B9C
		internal Job AddedChildJob
		{
			get
			{
				return this._addedChildJob;
			}
		}

		// Token: 0x06001F9E RID: 8094 RVA: 0x000B79A4 File Offset: 0x000B5BA4
		internal ThrottlingJobChildAddedEventArgs(Job addedChildJob)
		{
			this._addedChildJob = addedChildJob;
		}

		// Token: 0x04000E02 RID: 3586
		private readonly Job _addedChildJob;
	}
}
