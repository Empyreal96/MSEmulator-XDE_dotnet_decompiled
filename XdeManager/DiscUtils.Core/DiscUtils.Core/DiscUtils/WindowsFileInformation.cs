using System;
using System.IO;

namespace DiscUtils
{
	// Token: 0x02000035 RID: 53
	public class WindowsFileInformation
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00005426 File Offset: 0x00003626
		// (set) Token: 0x06000225 RID: 549 RVA: 0x0000542E File Offset: 0x0000362E
		public DateTime ChangeTime { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000226 RID: 550 RVA: 0x00005437 File Offset: 0x00003637
		// (set) Token: 0x06000227 RID: 551 RVA: 0x0000543F File Offset: 0x0000363F
		public DateTime CreationTime { get; set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000228 RID: 552 RVA: 0x00005448 File Offset: 0x00003648
		// (set) Token: 0x06000229 RID: 553 RVA: 0x00005450 File Offset: 0x00003650
		public FileAttributes FileAttributes { get; set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600022A RID: 554 RVA: 0x00005459 File Offset: 0x00003659
		// (set) Token: 0x0600022B RID: 555 RVA: 0x00005461 File Offset: 0x00003661
		public DateTime LastAccessTime { get; set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000546A File Offset: 0x0000366A
		// (set) Token: 0x0600022D RID: 557 RVA: 0x00005472 File Offset: 0x00003672
		public DateTime LastWriteTime { get; set; }
	}
}
