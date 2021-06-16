using System;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils
{
	// Token: 0x02000009 RID: 9
	public abstract class DiscFileSystem : MarshalByRefObject, IFileSystem, IDisposable
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00002629 File Offset: 0x00000829
		protected DiscFileSystem()
		{
			this.Options = new DiscFileSystemOptions();
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000263C File Offset: 0x0000083C
		protected DiscFileSystem(DiscFileSystemOptions defaultOptions)
		{
			this.Options = defaultOptions;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000264C File Offset: 0x0000084C
		~DiscFileSystem()
		{
			this.Dispose(false);
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000044 RID: 68 RVA: 0x0000267C File Offset: 0x0000087C
		public virtual DiscFileSystemOptions Options { get; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000045 RID: 69
		public abstract string FriendlyName { get; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000046 RID: 70
		public abstract bool CanWrite { get; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002684 File Offset: 0x00000884
		public virtual DiscDirectoryInfo Root
		{
			get
			{
				return new DiscDirectoryInfo(this, string.Empty);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002691 File Offset: 0x00000891
		public virtual string VolumeLabel
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002698 File Offset: 0x00000898
		public virtual bool IsThreadSafe
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000269B File Offset: 0x0000089B
		public virtual void CopyFile(string sourceFile, string destinationFile)
		{
			this.CopyFile(sourceFile, destinationFile, false);
		}

		// Token: 0x0600004B RID: 75
		public abstract void CopyFile(string sourceFile, string destinationFile, bool overwrite);

		// Token: 0x0600004C RID: 76
		public abstract void CreateDirectory(string path);

		// Token: 0x0600004D RID: 77
		public abstract void DeleteDirectory(string path);

		// Token: 0x0600004E RID: 78 RVA: 0x000026A8 File Offset: 0x000008A8
		public virtual void DeleteDirectory(string path, bool recursive)
		{
			if (recursive)
			{
				foreach (string path2 in this.GetDirectories(path))
				{
					this.DeleteDirectory(path2, true);
				}
				foreach (string path3 in this.GetFiles(path))
				{
					this.DeleteFile(path3);
				}
			}
			this.DeleteDirectory(path);
		}

		// Token: 0x0600004F RID: 79
		public abstract void DeleteFile(string path);

		// Token: 0x06000050 RID: 80
		public abstract bool DirectoryExists(string path);

		// Token: 0x06000051 RID: 81
		public abstract bool FileExists(string path);

		// Token: 0x06000052 RID: 82 RVA: 0x00002702 File Offset: 0x00000902
		public virtual bool Exists(string path)
		{
			return this.FileExists(path) || this.DirectoryExists(path);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002716 File Offset: 0x00000916
		public virtual string[] GetDirectories(string path)
		{
			return this.GetDirectories(path, "*.*", SearchOption.TopDirectoryOnly);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002725 File Offset: 0x00000925
		public virtual string[] GetDirectories(string path, string searchPattern)
		{
			return this.GetDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);
		}

		// Token: 0x06000055 RID: 85
		public abstract string[] GetDirectories(string path, string searchPattern, SearchOption searchOption);

		// Token: 0x06000056 RID: 86 RVA: 0x00002730 File Offset: 0x00000930
		public virtual string[] GetFiles(string path)
		{
			return this.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000273F File Offset: 0x0000093F
		public virtual string[] GetFiles(string path, string searchPattern)
		{
			return this.GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
		}

		// Token: 0x06000058 RID: 88
		public abstract string[] GetFiles(string path, string searchPattern, SearchOption searchOption);

		// Token: 0x06000059 RID: 89
		public abstract string[] GetFileSystemEntries(string path);

		// Token: 0x0600005A RID: 90
		public abstract string[] GetFileSystemEntries(string path, string searchPattern);

		// Token: 0x0600005B RID: 91
		public abstract void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName);

		// Token: 0x0600005C RID: 92 RVA: 0x0000274A File Offset: 0x0000094A
		public virtual void MoveFile(string sourceName, string destinationName)
		{
			this.MoveFile(sourceName, destinationName, false);
		}

		// Token: 0x0600005D RID: 93
		public abstract void MoveFile(string sourceName, string destinationName, bool overwrite);

		// Token: 0x0600005E RID: 94 RVA: 0x00002755 File Offset: 0x00000955
		public virtual SparseStream OpenFile(string path, FileMode mode)
		{
			return this.OpenFile(path, mode, FileAccess.ReadWrite);
		}

		// Token: 0x0600005F RID: 95
		public abstract SparseStream OpenFile(string path, FileMode mode, FileAccess access);

		// Token: 0x06000060 RID: 96
		public abstract FileAttributes GetAttributes(string path);

		// Token: 0x06000061 RID: 97
		public abstract void SetAttributes(string path, FileAttributes newValue);

		// Token: 0x06000062 RID: 98 RVA: 0x00002760 File Offset: 0x00000960
		public virtual DateTime GetCreationTime(string path)
		{
			return this.GetCreationTimeUtc(path).ToLocalTime();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000277C File Offset: 0x0000097C
		public virtual void SetCreationTime(string path, DateTime newTime)
		{
			this.SetCreationTimeUtc(path, newTime.ToUniversalTime());
		}

		// Token: 0x06000064 RID: 100
		public abstract DateTime GetCreationTimeUtc(string path);

		// Token: 0x06000065 RID: 101
		public abstract void SetCreationTimeUtc(string path, DateTime newTime);

		// Token: 0x06000066 RID: 102 RVA: 0x0000278C File Offset: 0x0000098C
		public virtual DateTime GetLastAccessTime(string path)
		{
			return this.GetLastAccessTimeUtc(path).ToLocalTime();
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000027A8 File Offset: 0x000009A8
		public virtual void SetLastAccessTime(string path, DateTime newTime)
		{
			this.SetLastAccessTimeUtc(path, newTime.ToUniversalTime());
		}

		// Token: 0x06000068 RID: 104
		public abstract DateTime GetLastAccessTimeUtc(string path);

		// Token: 0x06000069 RID: 105
		public abstract void SetLastAccessTimeUtc(string path, DateTime newTime);

		// Token: 0x0600006A RID: 106 RVA: 0x000027B8 File Offset: 0x000009B8
		public virtual DateTime GetLastWriteTime(string path)
		{
			return this.GetLastWriteTimeUtc(path).ToLocalTime();
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000027D4 File Offset: 0x000009D4
		public virtual void SetLastWriteTime(string path, DateTime newTime)
		{
			this.SetLastWriteTimeUtc(path, newTime.ToUniversalTime());
		}

		// Token: 0x0600006C RID: 108
		public abstract DateTime GetLastWriteTimeUtc(string path);

		// Token: 0x0600006D RID: 109
		public abstract void SetLastWriteTimeUtc(string path, DateTime newTime);

		// Token: 0x0600006E RID: 110
		public abstract long GetFileLength(string path);

		// Token: 0x0600006F RID: 111 RVA: 0x000027E4 File Offset: 0x000009E4
		public virtual DiscFileInfo GetFileInfo(string path)
		{
			return new DiscFileInfo(this, path);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000027ED File Offset: 0x000009ED
		public virtual DiscDirectoryInfo GetDirectoryInfo(string path)
		{
			return new DiscDirectoryInfo(this, path);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000027F6 File Offset: 0x000009F6
		public virtual DiscFileSystemInfo GetFileSystemInfo(string path)
		{
			return new DiscFileSystemInfo(this, path);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000027FF File Offset: 0x000009FF
		public virtual byte[] ReadBootCode()
		{
			return null;
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000073 RID: 115
		public abstract long Size { get; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000074 RID: 116
		public abstract long UsedSpace { get; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000075 RID: 117
		public abstract long AvailableSpace { get; }

		// Token: 0x06000076 RID: 118 RVA: 0x00002802 File Offset: 0x00000A02
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002811 File Offset: 0x00000A11
		protected virtual void Dispose(bool disposing)
		{
		}
	}
}
