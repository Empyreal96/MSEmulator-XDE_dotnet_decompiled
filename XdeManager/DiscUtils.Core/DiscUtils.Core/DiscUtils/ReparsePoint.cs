using System;

namespace DiscUtils
{
	// Token: 0x02000026 RID: 38
	public sealed class ReparsePoint
	{
		// Token: 0x06000199 RID: 409 RVA: 0x000044F8 File Offset: 0x000026F8
		public ReparsePoint(int tag, byte[] content)
		{
			this.Tag = tag;
			this.Content = content;
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600019A RID: 410 RVA: 0x0000450E File Offset: 0x0000270E
		// (set) Token: 0x0600019B RID: 411 RVA: 0x00004516 File Offset: 0x00002716
		public byte[] Content { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600019C RID: 412 RVA: 0x0000451F File Offset: 0x0000271F
		// (set) Token: 0x0600019D RID: 413 RVA: 0x00004527 File Offset: 0x00002727
		public int Tag { get; set; }
	}
}
