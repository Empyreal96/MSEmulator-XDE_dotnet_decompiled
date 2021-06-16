using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000021 RID: 33
	public class InsertEjectCompletedEventArgs : EventArgs
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00004D2C File Offset: 0x00002F2C
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x00004D34 File Offset: 0x00002F34
		public bool IsSDCardInserted { get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00004D3D File Offset: 0x00002F3D
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00004D45 File Offset: 0x00002F45
		public string SDCardException { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00004D4E File Offset: 0x00002F4E
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00004D56 File Offset: 0x00002F56
		public string SDCardExceptionStackTrace { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00004D5F File Offset: 0x00002F5F
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00004D67 File Offset: 0x00002F67
		public string HostFolderPath { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00004D70 File Offset: 0x00002F70
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00004D78 File Offset: 0x00002F78
		public bool ShouldSyncOnEject { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00004D81 File Offset: 0x00002F81
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00004D89 File Offset: 0x00002F89
		public InsertEjectCompletedEventArgs.EventCompletionStatus CompletionStatus { get; set; }

		// Token: 0x02000080 RID: 128
		public enum EventCompletionStatus
		{
			// Token: 0x040001CC RID: 460
			CompletedSuccessfully,
			// Token: 0x040001CD RID: 461
			CompletedWithException,
			// Token: 0x040001CE RID: 462
			Cancelled
		}
	}
}
