using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000068 RID: 104
	public class UpdateSyncProgressEventArgs : EventArgs
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0000553C File Offset: 0x0000373C
		// (set) Token: 0x06000263 RID: 611 RVA: 0x00005544 File Offset: 0x00003744
		public double CurrentProgressValue { get; set; }
	}
}
