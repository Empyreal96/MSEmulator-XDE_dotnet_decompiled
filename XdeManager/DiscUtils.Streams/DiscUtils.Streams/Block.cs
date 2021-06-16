using System;

namespace DiscUtils.Streams
{
	// Token: 0x02000003 RID: 3
	public class Block
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002430 File Offset: 0x00000630
		// (set) Token: 0x0600000A RID: 10 RVA: 0x00002438 File Offset: 0x00000638
		public int Available { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002441 File Offset: 0x00000641
		// (set) Token: 0x0600000C RID: 12 RVA: 0x00002449 File Offset: 0x00000649
		public byte[] Data { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002452 File Offset: 0x00000652
		// (set) Token: 0x0600000E RID: 14 RVA: 0x0000245A File Offset: 0x0000065A
		public long Position { get; set; }

		// Token: 0x0600000F RID: 15 RVA: 0x00002463 File Offset: 0x00000663
		public bool Equals(Block other)
		{
			return this.Position == other.Position;
		}
	}
}
