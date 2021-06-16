using System;
using System.IO;

namespace DiscUtils
{
	// Token: 0x02000010 RID: 16
	public abstract class FileSystemInfo
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000B3 RID: 179
		public abstract string Description { get; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000B4 RID: 180
		public abstract string Name { get; }

		// Token: 0x060000B5 RID: 181 RVA: 0x00002C8F File Offset: 0x00000E8F
		public DiscFileSystem Open(VolumeInfo volume)
		{
			return this.Open(volume, null);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00002C99 File Offset: 0x00000E99
		public DiscFileSystem Open(Stream stream)
		{
			return this.Open(stream, null);
		}

		// Token: 0x060000B7 RID: 183
		public abstract DiscFileSystem Open(VolumeInfo volume, FileSystemParameters parameters);

		// Token: 0x060000B8 RID: 184
		public abstract DiscFileSystem Open(Stream stream, FileSystemParameters parameters);

		// Token: 0x060000B9 RID: 185 RVA: 0x00002CA3 File Offset: 0x00000EA3
		public override string ToString()
		{
			return this.Name;
		}
	}
}
