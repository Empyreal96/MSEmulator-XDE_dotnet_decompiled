using System;

namespace DiscUtils
{
	// Token: 0x0200001D RID: 29
	public interface IUnixFileSystem : IFileSystem
	{
		// Token: 0x06000129 RID: 297
		UnixFileSystemInfo GetUnixFileInfo(string path);
	}
}
