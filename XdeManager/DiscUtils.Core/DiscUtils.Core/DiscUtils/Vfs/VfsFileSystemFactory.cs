using System;
using System.IO;

namespace DiscUtils.Vfs
{
	// Token: 0x0200003E RID: 62
	public abstract class VfsFileSystemFactory
	{
		// Token: 0x060002A9 RID: 681 RVA: 0x00006248 File Offset: 0x00004448
		public FileSystemInfo[] Detect(Stream stream)
		{
			return this.Detect(stream, null);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00006254 File Offset: 0x00004454
		public FileSystemInfo[] Detect(VolumeInfo volume)
		{
			FileSystemInfo[] result;
			using (Stream stream = volume.Open())
			{
				result = this.Detect(stream, volume);
			}
			return result;
		}

		// Token: 0x060002AB RID: 683
		public abstract FileSystemInfo[] Detect(Stream stream, VolumeInfo volumeInfo);
	}
}
