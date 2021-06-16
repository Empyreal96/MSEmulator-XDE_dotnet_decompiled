using System;
using System.IO;
using DiscUtils.Vfs;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000020 RID: 32
	[VfsFileSystemFactory]
	internal class FileSystemFactory : VfsFileSystemFactory
	{
		// Token: 0x0600013F RID: 319 RVA: 0x00007FBC File Offset: 0x000061BC
		public override FileSystemInfo[] Detect(Stream stream, VolumeInfo volume)
		{
			if (NtfsFileSystem.Detect(stream))
			{
				return new FileSystemInfo[]
				{
					new VfsFileSystemInfo("NTFS", "Microsoft NTFS", new VfsFileSystemOpener(this.Open))
				};
			}
			return new FileSystemInfo[0];
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00007FF1 File Offset: 0x000061F1
		private DiscFileSystem Open(Stream stream, VolumeInfo volumeInfo, FileSystemParameters parameters)
		{
			return new NtfsFileSystem(stream);
		}
	}
}
