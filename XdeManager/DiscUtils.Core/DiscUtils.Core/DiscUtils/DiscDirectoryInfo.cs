using System;
using System.IO;
using DiscUtils.Internal;

namespace DiscUtils
{
	// Token: 0x02000006 RID: 6
	public sealed class DiscDirectoryInfo : DiscFileSystemInfo
	{
		// Token: 0x0600000F RID: 15 RVA: 0x0000226D File Offset: 0x0000046D
		internal DiscDirectoryInfo(DiscFileSystem fileSystem, string path) : base(fileSystem, path)
		{
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002277 File Offset: 0x00000477
		public override bool Exists
		{
			get
			{
				return base.FileSystem.DirectoryExists(base.Path);
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000228A File Offset: 0x0000048A
		public override string FullName
		{
			get
			{
				return base.FullName + "\\";
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000229C File Offset: 0x0000049C
		public void Create()
		{
			base.FileSystem.CreateDirectory(base.Path);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000022AF File Offset: 0x000004AF
		public override void Delete()
		{
			base.FileSystem.DeleteDirectory(base.Path, false);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000022C3 File Offset: 0x000004C3
		public void Delete(bool recursive)
		{
			base.FileSystem.DeleteDirectory(base.Path, recursive);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000022D7 File Offset: 0x000004D7
		public void MoveTo(string destinationDirName)
		{
			base.FileSystem.MoveDirectory(base.Path, destinationDirName);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000022EB File Offset: 0x000004EB
		public DiscDirectoryInfo[] GetDirectories()
		{
			return Utilities.Map<string, DiscDirectoryInfo>(base.FileSystem.GetDirectories(base.Path), (string p) => new DiscDirectoryInfo(base.FileSystem, p));
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000230F File Offset: 0x0000050F
		public DiscDirectoryInfo[] GetDirectories(string pattern)
		{
			return this.GetDirectories(pattern, SearchOption.TopDirectoryOnly);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002319 File Offset: 0x00000519
		public DiscDirectoryInfo[] GetDirectories(string pattern, SearchOption searchOption)
		{
			return Utilities.Map<string, DiscDirectoryInfo>(base.FileSystem.GetDirectories(base.Path, pattern, searchOption), (string p) => new DiscDirectoryInfo(base.FileSystem, p));
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000233F File Offset: 0x0000053F
		public DiscFileInfo[] GetFiles()
		{
			return Utilities.Map<string, DiscFileInfo>(base.FileSystem.GetFiles(base.Path), (string p) => new DiscFileInfo(base.FileSystem, p));
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002363 File Offset: 0x00000563
		public DiscFileInfo[] GetFiles(string pattern)
		{
			return this.GetFiles(pattern, SearchOption.TopDirectoryOnly);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000236D File Offset: 0x0000056D
		public DiscFileInfo[] GetFiles(string pattern, SearchOption searchOption)
		{
			return Utilities.Map<string, DiscFileInfo>(base.FileSystem.GetFiles(base.Path, pattern, searchOption), (string p) => new DiscFileInfo(base.FileSystem, p));
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002393 File Offset: 0x00000593
		public DiscFileSystemInfo[] GetFileSystemInfos()
		{
			return Utilities.Map<string, DiscFileSystemInfo>(base.FileSystem.GetFileSystemEntries(base.Path), (string p) => new DiscFileSystemInfo(base.FileSystem, p));
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000023B7 File Offset: 0x000005B7
		public DiscFileSystemInfo[] GetFileSystemInfos(string pattern)
		{
			return Utilities.Map<string, DiscFileSystemInfo>(base.FileSystem.GetFileSystemEntries(base.Path, pattern), (string p) => new DiscFileSystemInfo(base.FileSystem, p));
		}
	}
}
