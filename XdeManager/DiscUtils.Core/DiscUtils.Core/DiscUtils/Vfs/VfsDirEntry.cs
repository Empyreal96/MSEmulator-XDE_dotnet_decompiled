using System;
using System.IO;

namespace DiscUtils.Vfs
{
	// Token: 0x0200003B RID: 59
	public abstract class VfsDirEntry
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000241 RID: 577
		public abstract DateTime CreationTimeUtc { get; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000242 RID: 578
		public abstract FileAttributes FileAttributes { get; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000243 RID: 579
		public abstract string FileName { get; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000244 RID: 580
		public abstract bool HasVfsFileAttributes { get; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000245 RID: 581
		public abstract bool HasVfsTimeInfo { get; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000246 RID: 582
		public abstract bool IsDirectory { get; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000247 RID: 583
		public abstract bool IsSymlink { get; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000248 RID: 584
		public abstract DateTime LastAccessTimeUtc { get; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000249 RID: 585
		public abstract DateTime LastWriteTimeUtc { get; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600024A RID: 586 RVA: 0x0000548C File Offset: 0x0000368C
		public virtual string SearchName
		{
			get
			{
				string fileName = this.FileName;
				if (fileName.IndexOf('.') == -1)
				{
					return fileName + ".";
				}
				return fileName;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600024B RID: 587
		public abstract long UniqueCacheId { get; }
	}
}
