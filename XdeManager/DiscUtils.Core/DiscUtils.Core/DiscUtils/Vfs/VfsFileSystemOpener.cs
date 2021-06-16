using System;
using System.IO;

namespace DiscUtils.Vfs
{
	// Token: 0x02000041 RID: 65
	// (Invoke) Token: 0x060002B4 RID: 692
	public delegate DiscFileSystem VfsFileSystemOpener(Stream stream, VolumeInfo volumeInfo, FileSystemParameters parameters);
}
