using System;
using System.IO;

namespace DiscUtils.Internal
{
	// Token: 0x0200006F RID: 111
	internal sealed class LocalFileLocator : FileLocator
	{
		// Token: 0x06000410 RID: 1040 RVA: 0x0000C38F File Offset: 0x0000A58F
		public LocalFileLocator(string dir)
		{
			this._dir = dir;
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000C39E File Offset: 0x0000A59E
		public override bool Exists(string fileName)
		{
			return File.Exists(Path.Combine(this._dir, fileName));
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000C3B1 File Offset: 0x0000A5B1
		protected override Stream OpenFile(string fileName, FileMode mode, FileAccess access, FileShare share)
		{
			return new FileStream(Path.Combine(this._dir, fileName), mode, access, share);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000C3C8 File Offset: 0x0000A5C8
		public override FileLocator GetRelativeLocator(string path)
		{
			return new LocalFileLocator(Path.Combine(this._dir, path));
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000C3DC File Offset: 0x0000A5DC
		public override string GetFullPath(string path)
		{
			string text = Path.Combine(this._dir, path);
			if (string.IsNullOrEmpty(text))
			{
				return Environment.CurrentDirectory;
			}
			return Path.GetFullPath(text);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0000C40A File Offset: 0x0000A60A
		public override string GetDirectoryFromPath(string path)
		{
			return Path.GetDirectoryName(path);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000C412 File Offset: 0x0000A612
		public override string GetFileFromPath(string path)
		{
			return Path.GetFileName(path);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0000C41A File Offset: 0x0000A61A
		public override DateTime GetLastWriteTimeUtc(string path)
		{
			return File.GetLastWriteTimeUtc(Path.Combine(this._dir, path));
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0000C430 File Offset: 0x0000A630
		public override bool HasCommonRoot(FileLocator other)
		{
			LocalFileLocator localFileLocator = other as LocalFileLocator;
			if (localFileLocator == null)
			{
				return false;
			}
			string dir = localFileLocator._dir;
			return dir.Length < 2 || this._dir.Length < 2 || dir[1] != ':' || this._dir[1] != ':' || char.ToUpperInvariant(dir[0]) == char.ToUpperInvariant(this._dir[0]);
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000C4A3 File Offset: 0x0000A6A3
		public override string ResolveRelativePath(string path)
		{
			return Utilities.ResolveRelativePath(this._dir, path);
		}

		// Token: 0x04000182 RID: 386
		private readonly string _dir;
	}
}
