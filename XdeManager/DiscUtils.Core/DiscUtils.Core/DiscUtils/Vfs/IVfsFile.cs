using System;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Vfs
{
	// Token: 0x02000037 RID: 55
	public interface IVfsFile
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000233 RID: 563
		// (set) Token: 0x06000234 RID: 564
		DateTime CreationTimeUtc { get; set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000235 RID: 565
		// (set) Token: 0x06000236 RID: 566
		FileAttributes FileAttributes { get; set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000237 RID: 567
		IBuffer FileContent { get; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000238 RID: 568
		long FileLength { get; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000239 RID: 569
		// (set) Token: 0x0600023A RID: 570
		DateTime LastAccessTimeUtc { get; set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600023B RID: 571
		// (set) Token: 0x0600023C RID: 572
		DateTime LastWriteTimeUtc { get; set; }
	}
}
