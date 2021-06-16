using System;
using System.Collections.Generic;
using System.IO;

namespace DiscUtils.Internal
{
	// Token: 0x02000074 RID: 116
	internal abstract class VirtualDiskFactory
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600043D RID: 1085
		public abstract string[] Variants { get; }

		// Token: 0x0600043E RID: 1086
		public abstract VirtualDiskTypeInfo GetDiskTypeInformation(string variant);

		// Token: 0x0600043F RID: 1087
		public abstract DiskImageBuilder GetImageBuilder(string variant);

		// Token: 0x06000440 RID: 1088
		public abstract VirtualDisk CreateDisk(FileLocator locator, string variant, string path, VirtualDiskParameters diskParameters);

		// Token: 0x06000441 RID: 1089
		public abstract VirtualDisk OpenDisk(string path, FileAccess access);

		// Token: 0x06000442 RID: 1090
		public abstract VirtualDisk OpenDisk(FileLocator locator, string path, FileAccess access);

		// Token: 0x06000443 RID: 1091 RVA: 0x0000CDDF File Offset: 0x0000AFDF
		public virtual VirtualDisk OpenDisk(FileLocator locator, string path, string extraInfo, Dictionary<string, string> parameters, FileAccess access)
		{
			return this.OpenDisk(locator, path, access);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000CDEB File Offset: 0x0000AFEB
		public VirtualDisk OpenDisk(DiscFileSystem fileSystem, string path, FileAccess access)
		{
			return this.OpenDisk(new DiscFileLocator(fileSystem, "\\"), path, access);
		}

		// Token: 0x06000445 RID: 1093
		public abstract VirtualDiskLayer OpenDiskLayer(FileLocator locator, string path, FileAccess access);
	}
}
