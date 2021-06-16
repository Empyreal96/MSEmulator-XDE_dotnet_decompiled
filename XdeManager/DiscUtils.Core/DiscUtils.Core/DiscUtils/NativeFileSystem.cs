using System;
using System.IO;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils
{
	// Token: 0x02000021 RID: 33
	public class NativeFileSystem : DiscFileSystem
	{
		// Token: 0x06000140 RID: 320 RVA: 0x000035F7 File Offset: 0x000017F7
		public NativeFileSystem(string basePath, bool readOnly)
		{
			this.BasePath = basePath;
			if (!this.BasePath.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				this.BasePath += "\\";
			}
			this._readOnly = readOnly;
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00003636 File Offset: 0x00001836
		public string BasePath { get; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000142 RID: 322 RVA: 0x0000363E File Offset: 0x0000183E
		public override bool CanWrite
		{
			get
			{
				return !this._readOnly;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00003649 File Offset: 0x00001849
		public override string FriendlyName
		{
			get
			{
				return "Native";
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00003650 File Offset: 0x00001850
		public override bool IsThreadSafe
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00003653 File Offset: 0x00001853
		public override DiscDirectoryInfo Root
		{
			get
			{
				return new DiscDirectoryInfo(this, string.Empty);
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00003660 File Offset: 0x00001860
		public override string VolumeLabel
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00003667 File Offset: 0x00001867
		public override void CopyFile(string sourceFile, string destinationFile)
		{
			this.CopyFile(sourceFile, destinationFile, true);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00003674 File Offset: 0x00001874
		public override void CopyFile(string sourceFile, string destinationFile, bool overwrite)
		{
			if (this._readOnly)
			{
				throw new UnauthorizedAccessException();
			}
			if (sourceFile.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				sourceFile = sourceFile.Substring(1);
			}
			if (destinationFile.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				destinationFile = destinationFile.Substring(1);
			}
			File.Copy(Path.Combine(this.BasePath, sourceFile), Path.Combine(this.BasePath, destinationFile), true);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000036DB File Offset: 0x000018DB
		public override void CreateDirectory(string path)
		{
			if (this._readOnly)
			{
				throw new UnauthorizedAccessException();
			}
			if (path.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				path = path.Substring(1);
			}
			Directory.CreateDirectory(Path.Combine(this.BasePath, path));
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00003714 File Offset: 0x00001914
		public override void DeleteDirectory(string path)
		{
			if (this._readOnly)
			{
				throw new UnauthorizedAccessException();
			}
			if (path.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				path = path.Substring(1);
			}
			Directory.Delete(Path.Combine(this.BasePath, path));
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000374C File Offset: 0x0000194C
		public override void DeleteDirectory(string path, bool recursive)
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

		// Token: 0x0600014C RID: 332 RVA: 0x000037A6 File Offset: 0x000019A6
		public override void DeleteFile(string path)
		{
			if (this._readOnly)
			{
				throw new UnauthorizedAccessException();
			}
			if (path.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				path = path.Substring(1);
			}
			File.Delete(Path.Combine(this.BasePath, path));
		}

		// Token: 0x0600014D RID: 333 RVA: 0x000037DE File Offset: 0x000019DE
		public override bool DirectoryExists(string path)
		{
			if (path.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				path = path.Substring(1);
			}
			return Directory.Exists(Path.Combine(this.BasePath, path));
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00003808 File Offset: 0x00001A08
		public override bool FileExists(string path)
		{
			if (path.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				path = path.Substring(1);
			}
			return File.Exists(Path.Combine(this.BasePath, path));
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00003832 File Offset: 0x00001A32
		public override bool Exists(string path)
		{
			return this.FileExists(path) || this.DirectoryExists(path);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00003846 File Offset: 0x00001A46
		public override string[] GetDirectories(string path)
		{
			return this.GetDirectories(path, "*.*", SearchOption.TopDirectoryOnly);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00003855 File Offset: 0x00001A55
		public override string[] GetDirectories(string path, string searchPattern)
		{
			return this.GetDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00003860 File Offset: 0x00001A60
		public override string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
		{
			if (path.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				path = path.Substring(1);
			}
			string[] result;
			try
			{
				result = this.CleanItems(Directory.GetDirectories(Path.Combine(this.BasePath, path), searchPattern, searchOption));
			}
			catch (IOException)
			{
				result = new string[0];
			}
			catch (UnauthorizedAccessException)
			{
				result = new string[0];
			}
			return result;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x000038D4 File Offset: 0x00001AD4
		public override string[] GetFiles(string path)
		{
			return this.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000038E3 File Offset: 0x00001AE3
		public override string[] GetFiles(string path, string searchPattern)
		{
			return this.GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000038F0 File Offset: 0x00001AF0
		public override string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
		{
			if (path.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				path = path.Substring(1);
			}
			string[] result;
			try
			{
				result = this.CleanItems(Directory.GetFiles(Path.Combine(this.BasePath, path), searchPattern, searchOption));
			}
			catch (IOException)
			{
				result = new string[0];
			}
			catch (UnauthorizedAccessException)
			{
				result = new string[0];
			}
			return result;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00003964 File Offset: 0x00001B64
		public override string[] GetFileSystemEntries(string path)
		{
			return this.GetFileSystemEntries(path, "*.*");
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00003974 File Offset: 0x00001B74
		public override string[] GetFileSystemEntries(string path, string searchPattern)
		{
			if (path.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				path = path.Substring(1);
			}
			string[] result;
			try
			{
				result = this.CleanItems(Directory.GetFileSystemEntries(Path.Combine(this.BasePath, path), searchPattern));
			}
			catch (IOException)
			{
				result = new string[0];
			}
			catch (UnauthorizedAccessException)
			{
				result = new string[0];
			}
			return result;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000039E8 File Offset: 0x00001BE8
		public override void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName)
		{
			if (this._readOnly)
			{
				throw new UnauthorizedAccessException();
			}
			if (sourceDirectoryName.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				sourceDirectoryName = sourceDirectoryName.Substring(1);
			}
			if (destinationDirectoryName.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				destinationDirectoryName = destinationDirectoryName.Substring(1);
			}
			Directory.Move(Path.Combine(this.BasePath, sourceDirectoryName), Path.Combine(this.BasePath, destinationDirectoryName));
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00003A4E File Offset: 0x00001C4E
		public override void MoveFile(string sourceName, string destinationName)
		{
			this.MoveFile(sourceName, destinationName, false);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00003A5C File Offset: 0x00001C5C
		public override void MoveFile(string sourceName, string destinationName, bool overwrite)
		{
			if (this._readOnly)
			{
				throw new UnauthorizedAccessException();
			}
			if (destinationName.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				destinationName = destinationName.Substring(1);
			}
			if (this.FileExists(Path.Combine(this.BasePath, destinationName)))
			{
				if (!overwrite)
				{
					throw new IOException("File already exists");
				}
				this.DeleteFile(Path.Combine(this.BasePath, destinationName));
			}
			if (sourceName.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				sourceName = sourceName.Substring(1);
			}
			File.Move(Path.Combine(this.BasePath, sourceName), Path.Combine(this.BasePath, destinationName));
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00003AF8 File Offset: 0x00001CF8
		public override SparseStream OpenFile(string path, FileMode mode)
		{
			return this.OpenFile(path, mode, FileAccess.ReadWrite);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00003B04 File Offset: 0x00001D04
		public override SparseStream OpenFile(string path, FileMode mode, FileAccess access)
		{
			if (this._readOnly && access != FileAccess.Read)
			{
				throw new UnauthorizedAccessException();
			}
			if (path.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				path = path.Substring(1);
			}
			FileShare share = FileShare.None;
			if (access == FileAccess.Read)
			{
				share = FileShare.Read;
			}
			return SparseStream.FromStream(new LocalFileLocator(this.BasePath).Open(path, mode, access, share), Ownership.Dispose);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00003B5C File Offset: 0x00001D5C
		public override FileAttributes GetAttributes(string path)
		{
			if (path.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				path = path.Substring(1);
			}
			return File.GetAttributes(Path.Combine(this.BasePath, path));
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00003B86 File Offset: 0x00001D86
		public override void SetAttributes(string path, FileAttributes newValue)
		{
			if (this._readOnly)
			{
				throw new UnauthorizedAccessException();
			}
			if (path.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				path = path.Substring(1);
			}
			File.SetAttributes(Path.Combine(this.BasePath, path), newValue);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00003BC0 File Offset: 0x00001DC0
		public override DateTime GetCreationTime(string path)
		{
			return this.GetCreationTimeUtc(path).ToLocalTime();
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00003BDC File Offset: 0x00001DDC
		public override void SetCreationTime(string path, DateTime newTime)
		{
			this.SetCreationTimeUtc(path, newTime.ToUniversalTime());
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00003BEC File Offset: 0x00001DEC
		public override DateTime GetCreationTimeUtc(string path)
		{
			if (path.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				path = path.Substring(1);
			}
			return File.GetCreationTimeUtc(Path.Combine(this.BasePath, path));
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00003C16 File Offset: 0x00001E16
		public override void SetCreationTimeUtc(string path, DateTime newTime)
		{
			if (this._readOnly)
			{
				throw new UnauthorizedAccessException();
			}
			if (path.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				path = path.Substring(1);
			}
			File.SetCreationTimeUtc(Path.Combine(this.BasePath, path), newTime);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00003C50 File Offset: 0x00001E50
		public override DateTime GetLastAccessTime(string path)
		{
			return this.GetLastAccessTimeUtc(path).ToLocalTime();
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00003C6C File Offset: 0x00001E6C
		public override void SetLastAccessTime(string path, DateTime newTime)
		{
			this.SetLastAccessTimeUtc(path, newTime.ToUniversalTime());
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00003C7C File Offset: 0x00001E7C
		public override DateTime GetLastAccessTimeUtc(string path)
		{
			if (path.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				path = path.Substring(1);
			}
			return File.GetLastAccessTimeUtc(Path.Combine(this.BasePath, path));
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00003CA6 File Offset: 0x00001EA6
		public override void SetLastAccessTimeUtc(string path, DateTime newTime)
		{
			if (this._readOnly)
			{
				throw new UnauthorizedAccessException();
			}
			if (path.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				path = path.Substring(1);
			}
			File.SetLastAccessTimeUtc(Path.Combine(this.BasePath, path), newTime);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00003CE0 File Offset: 0x00001EE0
		public override DateTime GetLastWriteTime(string path)
		{
			return this.GetLastWriteTimeUtc(path).ToLocalTime();
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00003CFC File Offset: 0x00001EFC
		public override void SetLastWriteTime(string path, DateTime newTime)
		{
			this.SetLastWriteTimeUtc(path, newTime.ToUniversalTime());
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00003D0C File Offset: 0x00001F0C
		public override DateTime GetLastWriteTimeUtc(string path)
		{
			if (path.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				path = path.Substring(1);
			}
			return File.GetLastWriteTimeUtc(Path.Combine(this.BasePath, path));
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00003D36 File Offset: 0x00001F36
		public override void SetLastWriteTimeUtc(string path, DateTime newTime)
		{
			if (this._readOnly)
			{
				throw new UnauthorizedAccessException();
			}
			if (path.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				path = path.Substring(1);
			}
			File.SetLastWriteTimeUtc(Path.Combine(this.BasePath, path), newTime);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00003D6F File Offset: 0x00001F6F
		public override long GetFileLength(string path)
		{
			if (path.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				path = path.Substring(1);
			}
			return new FileInfo(Path.Combine(this.BasePath, path)).Length;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00003D9E File Offset: 0x00001F9E
		public override DiscFileInfo GetFileInfo(string path)
		{
			return new DiscFileInfo(this, path);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00003DA7 File Offset: 0x00001FA7
		public override DiscDirectoryInfo GetDirectoryInfo(string path)
		{
			return new DiscDirectoryInfo(this, path);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00003DB0 File Offset: 0x00001FB0
		public override DiscFileSystemInfo GetFileSystemInfo(string path)
		{
			return new DiscFileSystemInfo(this, path);
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00003DB9 File Offset: 0x00001FB9
		public override long Size
		{
			get
			{
				return new DriveInfo(this.BasePath).TotalSize;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00003DCB File Offset: 0x00001FCB
		public override long UsedSpace
		{
			get
			{
				return this.Size - this.AvailableSpace;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00003DDA File Offset: 0x00001FDA
		public override long AvailableSpace
		{
			get
			{
				return new DriveInfo(this.BasePath).AvailableFreeSpace;
			}
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00003DEC File Offset: 0x00001FEC
		private string[] CleanItems(string[] dirtyItems)
		{
			string[] array = new string[dirtyItems.Length];
			for (int i = 0; i < dirtyItems.Length; i++)
			{
				array[i] = dirtyItems[i].Substring(this.BasePath.Length - 1);
			}
			return array;
		}

		// Token: 0x0400003D RID: 61
		private readonly bool _readOnly;
	}
}
