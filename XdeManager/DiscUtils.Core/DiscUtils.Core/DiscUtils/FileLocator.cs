using System;
using System.IO;
using DiscUtils.Internal;
using DiscUtils.Setup;

namespace DiscUtils
{
	// Token: 0x0200000F RID: 15
	internal abstract class FileLocator
	{
		// Token: 0x060000A7 RID: 167
		public abstract bool Exists(string fileName);

		// Token: 0x060000A8 RID: 168 RVA: 0x00002BF0 File Offset: 0x00000DF0
		public Stream Open(string fileName, FileMode mode, FileAccess access, FileShare share)
		{
			FileOpenEventArgs fileOpenEventArgs = new FileOpenEventArgs(fileName, mode, access, share, new FileOpenDelegate(this.OpenFile));
			SetupHelper.OnOpeningFile(this, fileOpenEventArgs);
			if (fileOpenEventArgs.Result != null)
			{
				return fileOpenEventArgs.Result;
			}
			return this.OpenFile(fileOpenEventArgs.FileName, fileOpenEventArgs.FileMode, fileOpenEventArgs.FileAccess, fileOpenEventArgs.FileShare);
		}

		// Token: 0x060000A9 RID: 169
		protected abstract Stream OpenFile(string fileName, FileMode mode, FileAccess access, FileShare share);

		// Token: 0x060000AA RID: 170
		public abstract FileLocator GetRelativeLocator(string path);

		// Token: 0x060000AB RID: 171
		public abstract string GetFullPath(string path);

		// Token: 0x060000AC RID: 172
		public abstract string GetDirectoryFromPath(string path);

		// Token: 0x060000AD RID: 173
		public abstract string GetFileFromPath(string path);

		// Token: 0x060000AE RID: 174
		public abstract DateTime GetLastWriteTimeUtc(string path);

		// Token: 0x060000AF RID: 175
		public abstract bool HasCommonRoot(FileLocator other);

		// Token: 0x060000B0 RID: 176
		public abstract string ResolveRelativePath(string path);

		// Token: 0x060000B1 RID: 177 RVA: 0x00002C4C File Offset: 0x00000E4C
		internal string MakeRelativePath(FileLocator fileLocator, string path)
		{
			if (!this.HasCommonRoot(fileLocator))
			{
				return null;
			}
			string basePath = this.GetFullPath(string.Empty) + "\\";
			return Utilities.MakeRelativePath(fileLocator.GetFullPath(path), basePath);
		}
	}
}
