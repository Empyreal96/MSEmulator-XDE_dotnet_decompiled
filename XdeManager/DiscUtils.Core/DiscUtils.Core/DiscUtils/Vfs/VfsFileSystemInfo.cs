using System;
using System.IO;

namespace DiscUtils.Vfs
{
	// Token: 0x02000040 RID: 64
	public sealed class VfsFileSystemInfo : FileSystemInfo
	{
		// Token: 0x060002AE RID: 686 RVA: 0x000062A0 File Offset: 0x000044A0
		public VfsFileSystemInfo(string name, string description, VfsFileSystemOpener openDelegate)
		{
			this.Name = name;
			this.Description = description;
			this._openDelegate = openDelegate;
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002AF RID: 687 RVA: 0x000062BD File Offset: 0x000044BD
		public override string Description { get; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x000062C5 File Offset: 0x000044C5
		public override string Name { get; }

		// Token: 0x060002B1 RID: 689 RVA: 0x000062CD File Offset: 0x000044CD
		public override DiscFileSystem Open(VolumeInfo volume, FileSystemParameters parameters)
		{
			return this._openDelegate(volume.Open(), volume, parameters);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x000062E2 File Offset: 0x000044E2
		public override DiscFileSystem Open(Stream stream, FileSystemParameters parameters)
		{
			return this._openDelegate(stream, null, parameters);
		}

		// Token: 0x04000097 RID: 151
		private readonly VfsFileSystemOpener _openDelegate;
	}
}
