using System;

namespace DiscUtils.Vfs
{
	// Token: 0x02000039 RID: 57
	public interface IVfsSymlink<TDirEntry, TFile> : IVfsFile where TDirEntry : VfsDirEntry where TFile : IVfsFile
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600023F RID: 575
		string TargetPath { get; }
	}
}
