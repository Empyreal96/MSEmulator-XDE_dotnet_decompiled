using System;

namespace DiscUtils.Streams
{
	// Token: 0x0200001F RID: 31
	public class PumpProgressEventArgs : EventArgs
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00004329 File Offset: 0x00002529
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00004331 File Offset: 0x00002531
		public long BytesRead { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000EE RID: 238 RVA: 0x0000433A File Offset: 0x0000253A
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00004342 File Offset: 0x00002542
		public long BytesWritten { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x0000434B File Offset: 0x0000254B
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00004353 File Offset: 0x00002553
		public long DestinationPosition { get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x0000435C File Offset: 0x0000255C
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00004364 File Offset: 0x00002564
		public long SourcePosition { get; set; }
	}
}
