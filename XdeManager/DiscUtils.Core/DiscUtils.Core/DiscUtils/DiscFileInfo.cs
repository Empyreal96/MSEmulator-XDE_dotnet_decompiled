using System;
using System.IO;

namespace DiscUtils
{
	// Token: 0x02000007 RID: 7
	public sealed class DiscFileInfo : DiscFileSystemInfo
	{
		// Token: 0x06000024 RID: 36 RVA: 0x00002430 File Offset: 0x00000630
		internal DiscFileInfo(DiscFileSystem fileSystem, string path) : base(fileSystem, path)
		{
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000025 RID: 37 RVA: 0x0000243A File Offset: 0x0000063A
		public DiscDirectoryInfo Directory
		{
			get
			{
				return this.Parent;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002442 File Offset: 0x00000642
		public string DirectoryName
		{
			get
			{
				return this.Directory.FullName;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000027 RID: 39 RVA: 0x0000244F File Offset: 0x0000064F
		public override bool Exists
		{
			get
			{
				return base.FileSystem.FileExists(base.Path);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002462 File Offset: 0x00000662
		// (set) Token: 0x06000029 RID: 41 RVA: 0x0000246F File Offset: 0x0000066F
		public bool IsReadOnly
		{
			get
			{
				return (this.Attributes & FileAttributes.ReadOnly) > (FileAttributes)0;
			}
			set
			{
				if (value)
				{
					this.Attributes |= FileAttributes.ReadOnly;
					return;
				}
				this.Attributes &= ~FileAttributes.ReadOnly;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002492 File Offset: 0x00000692
		public long Length
		{
			get
			{
				return base.FileSystem.GetFileLength(base.Path);
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000024A5 File Offset: 0x000006A5
		public override void Delete()
		{
			base.FileSystem.DeleteFile(base.Path);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000024B8 File Offset: 0x000006B8
		public StreamWriter AppendText()
		{
			return new StreamWriter(this.Open(FileMode.Append));
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000024C6 File Offset: 0x000006C6
		public void CopyTo(string destinationFileName)
		{
			this.CopyTo(destinationFileName, false);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000024D0 File Offset: 0x000006D0
		public void CopyTo(string destinationFileName, bool overwrite)
		{
			base.FileSystem.CopyFile(base.Path, destinationFileName, overwrite);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000024E5 File Offset: 0x000006E5
		public Stream Create()
		{
			return this.Open(FileMode.Create);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000024EE File Offset: 0x000006EE
		public StreamWriter CreateText()
		{
			return new StreamWriter(this.Open(FileMode.Create));
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000024FC File Offset: 0x000006FC
		public void MoveTo(string destinationFileName)
		{
			base.FileSystem.MoveFile(base.Path, destinationFileName);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002510 File Offset: 0x00000710
		public Stream Open(FileMode mode)
		{
			return base.FileSystem.OpenFile(base.Path, mode);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002524 File Offset: 0x00000724
		public Stream Open(FileMode mode, FileAccess access)
		{
			return base.FileSystem.OpenFile(base.Path, mode, access);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002539 File Offset: 0x00000739
		public Stream OpenRead()
		{
			return this.Open(FileMode.Open, FileAccess.Read);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002543 File Offset: 0x00000743
		public StreamReader OpenText()
		{
			return new StreamReader(this.OpenRead());
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002550 File Offset: 0x00000750
		public Stream OpenWrite()
		{
			return this.Open(FileMode.Open, FileAccess.Write);
		}
	}
}
