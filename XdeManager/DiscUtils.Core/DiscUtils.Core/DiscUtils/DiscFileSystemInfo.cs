using System;
using System.IO;
using DiscUtils.Internal;

namespace DiscUtils
{
	// Token: 0x0200000B RID: 11
	public class DiscFileSystemInfo
	{
		// Token: 0x0600007A RID: 122 RVA: 0x0000281B File Offset: 0x00000A1B
		internal DiscFileSystemInfo(DiscFileSystem fileSystem, string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			this.FileSystem = fileSystem;
			this.Path = path.Trim(new char[]
			{
				'\\'
			});
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600007B RID: 123 RVA: 0x0000284F File Offset: 0x00000A4F
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00002862 File Offset: 0x00000A62
		public virtual FileAttributes Attributes
		{
			get
			{
				return this.FileSystem.GetAttributes(this.Path);
			}
			set
			{
				this.FileSystem.SetAttributes(this.Path, value);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00002878 File Offset: 0x00000A78
		// (set) Token: 0x0600007E RID: 126 RVA: 0x00002893 File Offset: 0x00000A93
		public virtual DateTime CreationTime
		{
			get
			{
				return this.CreationTimeUtc.ToLocalTime();
			}
			set
			{
				this.CreationTimeUtc = value.ToUniversalTime();
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600007F RID: 127 RVA: 0x000028A2 File Offset: 0x00000AA2
		// (set) Token: 0x06000080 RID: 128 RVA: 0x000028B5 File Offset: 0x00000AB5
		public virtual DateTime CreationTimeUtc
		{
			get
			{
				return this.FileSystem.GetCreationTimeUtc(this.Path);
			}
			set
			{
				this.FileSystem.SetCreationTimeUtc(this.Path, value);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000081 RID: 129 RVA: 0x000028C9 File Offset: 0x00000AC9
		public virtual bool Exists
		{
			get
			{
				return this.FileSystem.Exists(this.Path);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000082 RID: 130 RVA: 0x000028DC File Offset: 0x00000ADC
		public virtual string Extension
		{
			get
			{
				string name = this.Name;
				int num = name.LastIndexOf('.');
				if (num >= 0)
				{
					return name.Substring(num + 1);
				}
				return string.Empty;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000083 RID: 131 RVA: 0x0000290C File Offset: 0x00000B0C
		public DiscFileSystem FileSystem { get; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00002914 File Offset: 0x00000B14
		public virtual string FullName
		{
			get
			{
				return this.Path;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000085 RID: 133 RVA: 0x0000291C File Offset: 0x00000B1C
		// (set) Token: 0x06000086 RID: 134 RVA: 0x00002937 File Offset: 0x00000B37
		public virtual DateTime LastAccessTime
		{
			get
			{
				return this.LastAccessTimeUtc.ToLocalTime();
			}
			set
			{
				this.LastAccessTimeUtc = value.ToUniversalTime();
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00002946 File Offset: 0x00000B46
		// (set) Token: 0x06000088 RID: 136 RVA: 0x00002959 File Offset: 0x00000B59
		public virtual DateTime LastAccessTimeUtc
		{
			get
			{
				return this.FileSystem.GetLastAccessTimeUtc(this.Path);
			}
			set
			{
				this.FileSystem.SetLastAccessTimeUtc(this.Path, value);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00002970 File Offset: 0x00000B70
		// (set) Token: 0x0600008A RID: 138 RVA: 0x0000298B File Offset: 0x00000B8B
		public virtual DateTime LastWriteTime
		{
			get
			{
				return this.LastWriteTimeUtc.ToLocalTime();
			}
			set
			{
				this.LastWriteTimeUtc = value.ToUniversalTime();
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600008B RID: 139 RVA: 0x0000299A File Offset: 0x00000B9A
		// (set) Token: 0x0600008C RID: 140 RVA: 0x000029AD File Offset: 0x00000BAD
		public virtual DateTime LastWriteTimeUtc
		{
			get
			{
				return this.FileSystem.GetLastWriteTimeUtc(this.Path);
			}
			set
			{
				this.FileSystem.SetLastWriteTimeUtc(this.Path, value);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600008D RID: 141 RVA: 0x000029C1 File Offset: 0x00000BC1
		public virtual string Name
		{
			get
			{
				return Utilities.GetFileFromPath(this.Path);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600008E RID: 142 RVA: 0x000029CE File Offset: 0x00000BCE
		public virtual DiscDirectoryInfo Parent
		{
			get
			{
				if (string.IsNullOrEmpty(this.Path))
				{
					return null;
				}
				return new DiscDirectoryInfo(this.FileSystem, Utilities.GetDirectoryFromPath(this.Path));
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600008F RID: 143 RVA: 0x000029F5 File Offset: 0x00000BF5
		protected string Path { get; }

		// Token: 0x06000090 RID: 144 RVA: 0x000029FD File Offset: 0x00000BFD
		public virtual void Delete()
		{
			if ((this.Attributes & FileAttributes.Directory) != (FileAttributes)0)
			{
				this.FileSystem.DeleteDirectory(this.Path);
				return;
			}
			this.FileSystem.DeleteFile(this.Path);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00002A30 File Offset: 0x00000C30
		public override bool Equals(object obj)
		{
			DiscFileSystemInfo discFileSystemInfo = obj as DiscFileSystemInfo;
			return obj != null && string.Compare(this.Path, discFileSystemInfo.Path, StringComparison.Ordinal) == 0 && object.Equals(this.FileSystem, discFileSystemInfo.FileSystem);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00002A70 File Offset: 0x00000C70
		public override int GetHashCode()
		{
			return this.Path.GetHashCode() ^ this.FileSystem.GetHashCode();
		}
	}
}
