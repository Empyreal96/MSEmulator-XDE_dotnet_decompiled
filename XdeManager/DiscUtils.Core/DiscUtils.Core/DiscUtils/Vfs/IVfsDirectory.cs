using System;
using System.Collections.Generic;

namespace DiscUtils.Vfs
{
	// Token: 0x02000036 RID: 54
	public interface IVfsDirectory<TDirEntry, TFile> : IVfsFile where TDirEntry : VfsDirEntry where TFile : IVfsFile
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600022F RID: 559
		ICollection<TDirEntry> AllEntries { get; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000230 RID: 560
		TDirEntry Self { get; }

		// Token: 0x06000231 RID: 561
		TDirEntry GetEntryByName(string name);

		// Token: 0x06000232 RID: 562
		TDirEntry CreateNewFile(string name);
	}
}
