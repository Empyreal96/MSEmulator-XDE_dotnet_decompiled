using System;
using System.IO;
using DiscUtils.Internal;

namespace DiscUtils
{
	// Token: 0x02000008 RID: 8
	internal sealed class DiscFileLocator : FileLocator
	{
		// Token: 0x06000037 RID: 55 RVA: 0x0000255A File Offset: 0x0000075A
		public DiscFileLocator(DiscFileSystem fileSystem, string basePath)
		{
			this._fileSystem = fileSystem;
			this._basePath = basePath;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002570 File Offset: 0x00000770
		public override bool Exists(string fileName)
		{
			return this._fileSystem.FileExists(Utilities.CombinePaths(this._basePath, fileName));
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002589 File Offset: 0x00000789
		protected override Stream OpenFile(string fileName, FileMode mode, FileAccess access, FileShare share)
		{
			return this._fileSystem.OpenFile(Utilities.CombinePaths(this._basePath, fileName), mode, access);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000025A4 File Offset: 0x000007A4
		public override FileLocator GetRelativeLocator(string path)
		{
			return new DiscFileLocator(this._fileSystem, Utilities.CombinePaths(this._basePath, path));
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000025BD File Offset: 0x000007BD
		public override string GetFullPath(string path)
		{
			return Utilities.CombinePaths(this._basePath, path);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000025CB File Offset: 0x000007CB
		public override string GetDirectoryFromPath(string path)
		{
			return Utilities.GetDirectoryFromPath(path);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000025D3 File Offset: 0x000007D3
		public override string GetFileFromPath(string path)
		{
			return Utilities.GetFileFromPath(path);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000025DB File Offset: 0x000007DB
		public override DateTime GetLastWriteTimeUtc(string path)
		{
			return this._fileSystem.GetLastWriteTimeUtc(Utilities.CombinePaths(this._basePath, path));
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000025F4 File Offset: 0x000007F4
		public override bool HasCommonRoot(FileLocator other)
		{
			DiscFileLocator discFileLocator = other as DiscFileLocator;
			return discFileLocator != null && discFileLocator._fileSystem == this._fileSystem;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000261B File Offset: 0x0000081B
		public override string ResolveRelativePath(string path)
		{
			return Utilities.ResolveRelativePath(this._basePath, path);
		}

		// Token: 0x04000011 RID: 17
		private readonly string _basePath;

		// Token: 0x04000012 RID: 18
		private readonly DiscFileSystem _fileSystem;
	}
}
