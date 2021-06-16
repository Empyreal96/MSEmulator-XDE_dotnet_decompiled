using System;
using System.IO;

namespace DiscUtils.Setup
{
	// Token: 0x02000044 RID: 68
	public class FileOpenEventArgs : EventArgs
	{
		// Token: 0x060002C8 RID: 712 RVA: 0x0000634F File Offset: 0x0000454F
		internal FileOpenEventArgs(string fileName, FileMode mode, FileAccess access, FileShare share, FileOpenDelegate opener)
		{
			this.FileName = fileName;
			this.FileMode = mode;
			this.FileAccess = access;
			this.FileShare = share;
			this._opener = opener;
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000637C File Offset: 0x0000457C
		// (set) Token: 0x060002CA RID: 714 RVA: 0x00006384 File Offset: 0x00004584
		public string FileName { get; set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000638D File Offset: 0x0000458D
		// (set) Token: 0x060002CC RID: 716 RVA: 0x00006395 File Offset: 0x00004595
		public FileMode FileMode { get; set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000639E File Offset: 0x0000459E
		// (set) Token: 0x060002CE RID: 718 RVA: 0x000063A6 File Offset: 0x000045A6
		public FileAccess FileAccess { get; set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002CF RID: 719 RVA: 0x000063AF File Offset: 0x000045AF
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x000063B7 File Offset: 0x000045B7
		public FileShare FileShare { get; set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x000063C0 File Offset: 0x000045C0
		// (set) Token: 0x060002D2 RID: 722 RVA: 0x000063C8 File Offset: 0x000045C8
		public Stream Result { get; set; }

		// Token: 0x060002D3 RID: 723 RVA: 0x000063D1 File Offset: 0x000045D1
		public Stream GetFileStream()
		{
			return this._opener(this.FileName, this.FileMode, this.FileAccess, this.FileShare);
		}

		// Token: 0x0400009A RID: 154
		private FileOpenDelegate _opener;
	}
}
