using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils.Vfs
{
	// Token: 0x0200003C RID: 60
	public abstract class VfsFileSystem<TDirEntry, TFile, TDirectory, TContext> : DiscFileSystem where TDirEntry : VfsDirEntry where TFile : IVfsFile where TDirectory : class, IVfsDirectory<TDirEntry, TFile>, TFile where TContext : VfsContext
	{
		// Token: 0x0600024D RID: 589 RVA: 0x000054C0 File Offset: 0x000036C0
		protected VfsFileSystem(DiscFileSystemOptions defaultOptions) : base(defaultOptions)
		{
			this._fileCache = new ObjectCache<long, TFile>();
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600024E RID: 590 RVA: 0x000054D4 File Offset: 0x000036D4
		// (set) Token: 0x0600024F RID: 591 RVA: 0x000054DC File Offset: 0x000036DC
		protected TContext Context { get; set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000250 RID: 592 RVA: 0x000054E5 File Offset: 0x000036E5
		// (set) Token: 0x06000251 RID: 593 RVA: 0x000054ED File Offset: 0x000036ED
		protected TDirectory RootDirectory { get; set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000252 RID: 594
		public abstract override string VolumeLabel { get; }

		// Token: 0x06000253 RID: 595 RVA: 0x000054F6 File Offset: 0x000036F6
		public override void CopyFile(string sourceFile, string destinationFile, bool overwrite)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000254 RID: 596 RVA: 0x000054FD File Offset: 0x000036FD
		public override void CreateDirectory(string path)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00005504 File Offset: 0x00003704
		public override void DeleteDirectory(string path)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000550B File Offset: 0x0000370B
		public override void DeleteFile(string path)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00005514 File Offset: 0x00003714
		public override bool DirectoryExists(string path)
		{
			if (VfsFileSystem<TDirEntry, TFile, TDirectory, TContext>.IsRoot(path))
			{
				return true;
			}
			TDirEntry directoryEntry = this.GetDirectoryEntry(path);
			return directoryEntry != null && directoryEntry.IsDirectory;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00005548 File Offset: 0x00003748
		public override bool FileExists(string path)
		{
			TDirEntry directoryEntry = this.GetDirectoryEntry(path);
			return directoryEntry != null && !directoryEntry.IsDirectory;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00005578 File Offset: 0x00003778
		public override string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
		{
			Regex regex = Utilities.ConvertWildcardsToRegEx(searchPattern);
			List<string> list = new List<string>();
			this.DoSearch(list, path, regex, searchOption == SearchOption.AllDirectories, true, false);
			return list.ToArray();
		}

		// Token: 0x0600025A RID: 602 RVA: 0x000055A8 File Offset: 0x000037A8
		public override string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
		{
			Regex regex = Utilities.ConvertWildcardsToRegEx(searchPattern);
			List<string> list = new List<string>();
			this.DoSearch(list, path, regex, searchOption == SearchOption.AllDirectories, false, true);
			return list.ToArray();
		}

		// Token: 0x0600025B RID: 603 RVA: 0x000055D8 File Offset: 0x000037D8
		public override string[] GetFileSystemEntries(string path)
		{
			if (!path.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				path = "\\" + path;
			}
			return Utilities.Map<TDirEntry, string>(this.GetDirectory(path).AllEntries, (TDirEntry m) => Utilities.CombinePaths(path, this.FormatFileName(m.FileName)));
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000564C File Offset: 0x0000384C
		public override string[] GetFileSystemEntries(string path, string searchPattern)
		{
			Regex regex = Utilities.ConvertWildcardsToRegEx(searchPattern);
			TDirectory directory = this.GetDirectory(path);
			List<string> list = new List<string>();
			foreach (TDirEntry tdirEntry in directory.AllEntries)
			{
				if (regex.IsMatch(tdirEntry.SearchName))
				{
					list.Add(Utilities.CombinePaths(path, tdirEntry.FileName));
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600025D RID: 605 RVA: 0x000056DC File Offset: 0x000038DC
		public override void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600025E RID: 606 RVA: 0x000056E3 File Offset: 0x000038E3
		public override void MoveFile(string sourceName, string destinationName, bool overwrite)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600025F RID: 607 RVA: 0x000056EC File Offset: 0x000038EC
		public override SparseStream OpenFile(string path, FileMode mode, FileAccess access)
		{
			if (!this.CanWrite)
			{
				if (mode != FileMode.Open)
				{
					throw new NotSupportedException("Only existing files can be opened");
				}
				if (access != FileAccess.Read)
				{
					throw new NotSupportedException("Files cannot be opened for write");
				}
			}
			string fileFromPath = Utilities.GetFileFromPath(path);
			string text = null;
			int num = fileFromPath.IndexOf(':');
			if (num >= 0)
			{
				text = fileFromPath.Substring(num + 1);
			}
			string directoryFromPath;
			try
			{
				directoryFromPath = Utilities.GetDirectoryFromPath(path);
			}
			catch (ArgumentException)
			{
				throw new IOException("Invalid path: " + path);
			}
			string path2 = Utilities.CombinePaths(directoryFromPath, fileFromPath);
			TDirEntry tdirEntry = this.GetDirectoryEntry(path2);
			if (tdirEntry == null)
			{
				if (mode == FileMode.Open)
				{
					throw new FileNotFoundException("No such file", path);
				}
				tdirEntry = this.GetDirectory(Utilities.GetDirectoryFromPath(path)).CreateNewFile(Utilities.GetFileFromPath(path));
			}
			else if (mode == FileMode.CreateNew)
			{
				throw new IOException("File already exists");
			}
			if (tdirEntry.IsSymlink)
			{
				tdirEntry = this.ResolveSymlink(tdirEntry, path2);
			}
			if (tdirEntry.IsDirectory)
			{
				throw new IOException("Attempt to open directory as a file");
			}
			TFile file = this.GetFile(tdirEntry);
			SparseStream sparseStream;
			if (string.IsNullOrEmpty(text))
			{
				sparseStream = new BufferStream(file.FileContent, access);
			}
			else
			{
				IVfsFileWithStreams vfsFileWithStreams = file as IVfsFileWithStreams;
				if (vfsFileWithStreams == null)
				{
					throw new NotSupportedException("Attempt to open a file stream on a file system that doesn't support them");
				}
				sparseStream = vfsFileWithStreams.OpenExistingStream(text);
				if (sparseStream == null)
				{
					if (mode != FileMode.Create && mode != FileMode.OpenOrCreate)
					{
						throw new FileNotFoundException("No such attribute on file", path);
					}
					sparseStream = vfsFileWithStreams.CreateStream(text);
				}
			}
			if (mode == FileMode.Create || mode == FileMode.Truncate)
			{
				sparseStream.SetLength(0L);
			}
			return sparseStream;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00005884 File Offset: 0x00003A84
		public override FileAttributes GetAttributes(string path)
		{
			if (VfsFileSystem<TDirEntry, TFile, TDirectory, TContext>.IsRoot(path))
			{
				return this.RootDirectory.FileAttributes;
			}
			TDirEntry directoryEntry = this.GetDirectoryEntry(path);
			if (directoryEntry == null)
			{
				throw new FileNotFoundException("File not found", path);
			}
			if (directoryEntry.HasVfsFileAttributes)
			{
				return directoryEntry.FileAttributes;
			}
			TFile file = this.GetFile(directoryEntry);
			return file.FileAttributes;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x000058F4 File Offset: 0x00003AF4
		public override void SetAttributes(string path, FileAttributes newValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000262 RID: 610 RVA: 0x000058FC File Offset: 0x00003AFC
		public override DateTime GetCreationTimeUtc(string path)
		{
			if (VfsFileSystem<TDirEntry, TFile, TDirectory, TContext>.IsRoot(path))
			{
				return this.RootDirectory.CreationTimeUtc;
			}
			TDirEntry directoryEntry = this.GetDirectoryEntry(path);
			if (directoryEntry == null)
			{
				throw new FileNotFoundException("No such file or directory", path);
			}
			if (directoryEntry.HasVfsTimeInfo)
			{
				return directoryEntry.CreationTimeUtc;
			}
			TFile file = this.GetFile(directoryEntry);
			return file.CreationTimeUtc;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000596C File Offset: 0x00003B6C
		public override void SetCreationTimeUtc(string path, DateTime newTime)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00005974 File Offset: 0x00003B74
		public override DateTime GetLastAccessTimeUtc(string path)
		{
			if (VfsFileSystem<TDirEntry, TFile, TDirectory, TContext>.IsRoot(path))
			{
				return this.RootDirectory.LastAccessTimeUtc;
			}
			TDirEntry directoryEntry = this.GetDirectoryEntry(path);
			if (directoryEntry == null)
			{
				throw new FileNotFoundException("No such file or directory", path);
			}
			if (directoryEntry.HasVfsTimeInfo)
			{
				return directoryEntry.LastAccessTimeUtc;
			}
			TFile file = this.GetFile(directoryEntry);
			return file.LastAccessTimeUtc;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x000059E4 File Offset: 0x00003BE4
		public override void SetLastAccessTimeUtc(string path, DateTime newTime)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000266 RID: 614 RVA: 0x000059EC File Offset: 0x00003BEC
		public override DateTime GetLastWriteTimeUtc(string path)
		{
			if (VfsFileSystem<TDirEntry, TFile, TDirectory, TContext>.IsRoot(path))
			{
				return this.RootDirectory.LastWriteTimeUtc;
			}
			TDirEntry directoryEntry = this.GetDirectoryEntry(path);
			if (directoryEntry == null)
			{
				throw new FileNotFoundException("No such file or directory", path);
			}
			if (directoryEntry.HasVfsTimeInfo)
			{
				return directoryEntry.LastWriteTimeUtc;
			}
			TFile file = this.GetFile(directoryEntry);
			return file.LastWriteTimeUtc;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00005A5C File Offset: 0x00003C5C
		public override void SetLastWriteTimeUtc(string path, DateTime newTime)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00005A64 File Offset: 0x00003C64
		public override long GetFileLength(string path)
		{
			TFile file = this.GetFile(path);
			if (file == null || (file.FileAttributes & FileAttributes.Directory) != (FileAttributes)0)
			{
				throw new FileNotFoundException("No such file", path);
			}
			return file.FileLength;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00005AAC File Offset: 0x00003CAC
		internal TFile GetFile(TDirEntry dirEntry)
		{
			long uniqueCacheId = dirEntry.UniqueCacheId;
			TFile tfile = this._fileCache[uniqueCacheId];
			if (tfile == null)
			{
				tfile = this.ConvertDirEntryToFile(dirEntry);
				this._fileCache[uniqueCacheId] = tfile;
			}
			return tfile;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00005AF0 File Offset: 0x00003CF0
		internal TDirectory GetDirectory(string path)
		{
			if (VfsFileSystem<TDirEntry, TFile, TDirectory, TContext>.IsRoot(path))
			{
				return this.RootDirectory;
			}
			TDirEntry tdirEntry = this.GetDirectoryEntry(path);
			if (tdirEntry != null && tdirEntry.IsSymlink)
			{
				tdirEntry = this.ResolveSymlink(tdirEntry, path);
			}
			if (tdirEntry == null || !tdirEntry.IsDirectory)
			{
				throw new DirectoryNotFoundException("No such directory: " + path);
			}
			return (TDirectory)((object)this.GetFile(tdirEntry));
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00005B69 File Offset: 0x00003D69
		internal TDirEntry GetDirectoryEntry(string path)
		{
			return this.GetDirectoryEntry(this.RootDirectory, path);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00005B78 File Offset: 0x00003D78
		protected void ForAllDirEntries(string path, VfsFileSystem<TDirEntry, TFile, TDirectory, TContext>.DirEntryHandler handler)
		{
			TDirectory tdirectory = default(TDirectory);
			TDirEntry directoryEntry = this.GetDirectoryEntry(path);
			if (directoryEntry != null)
			{
				handler(path, directoryEntry);
				if (directoryEntry.IsDirectory)
				{
					tdirectory = (this.GetFile(directoryEntry) as TDirectory);
				}
			}
			else
			{
				tdirectory = (this.GetFile(path) as TDirectory);
			}
			if (tdirectory != null)
			{
				foreach (TDirEntry tdirEntry in tdirectory.AllEntries)
				{
					this.ForAllDirEntries(Utilities.CombinePaths(path, tdirEntry.FileName), handler);
				}
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00005C40 File Offset: 0x00003E40
		protected TFile GetFile(string path)
		{
			if (VfsFileSystem<TDirEntry, TFile, TDirectory, TContext>.IsRoot(path))
			{
				return (TFile)((object)this.RootDirectory);
			}
			if (path == null)
			{
				return default(TFile);
			}
			TDirEntry directoryEntry = this.GetDirectoryEntry(path);
			if (directoryEntry == null)
			{
				throw new FileNotFoundException("No such file or directory", path);
			}
			return this.GetFile(directoryEntry);
		}

		// Token: 0x0600026E RID: 622
		protected abstract TFile ConvertDirEntryToFile(TDirEntry dirEntry);

		// Token: 0x0600026F RID: 623 RVA: 0x00005C96 File Offset: 0x00003E96
		protected virtual string FormatFileName(string name)
		{
			return name;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00005C99 File Offset: 0x00003E99
		private static bool IsRoot(string path)
		{
			return string.IsNullOrEmpty(path) || path == "\\";
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00005CB0 File Offset: 0x00003EB0
		private TDirEntry GetDirectoryEntry(TDirectory dir, string path)
		{
			string[] pathEntries = path.Split(new char[]
			{
				'\\'
			}, StringSplitOptions.RemoveEmptyEntries);
			return this.GetDirectoryEntry(dir, pathEntries, 0);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00005CDC File Offset: 0x00003EDC
		private TDirEntry GetDirectoryEntry(TDirectory dir, string[] pathEntries, int pathOffset)
		{
			if (pathEntries.Length == 0)
			{
				return dir.Self;
			}
			TDirEntry entryByName = dir.GetEntryByName(pathEntries[pathOffset]);
			if (entryByName == null)
			{
				return default(TDirEntry);
			}
			if (pathOffset == pathEntries.Length - 1)
			{
				return entryByName;
			}
			if (entryByName.IsDirectory)
			{
				return this.GetDirectoryEntry((TDirectory)((object)this.ConvertDirEntryToFile(entryByName)), pathEntries, pathOffset + 1);
			}
			throw new IOException(string.Format(CultureInfo.InvariantCulture, "{0} is a file, not a directory", new object[]
			{
				pathEntries[pathOffset]
			}));
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00005D70 File Offset: 0x00003F70
		private void DoSearch(List<string> results, string path, Regex regex, bool subFolders, bool dirs, bool files)
		{
			TDirectory directory = this.GetDirectory(path);
			if (directory == null)
			{
				throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, "The directory '{0}' was not found", new object[]
				{
					path
				}));
			}
			string a = path;
			if (VfsFileSystem<TDirEntry, TFile, TDirectory, TContext>.IsRoot(path))
			{
				a = "\\";
			}
			foreach (TDirEntry tdirEntry in directory.AllEntries)
			{
				TDirEntry tdirEntry2 = tdirEntry;
				if (tdirEntry2.IsSymlink)
				{
					tdirEntry2 = this.ResolveSymlink(tdirEntry2, path + "\\" + tdirEntry2.FileName);
				}
				bool isDirectory = tdirEntry2.IsDirectory;
				if (((isDirectory && dirs) || (!isDirectory && files)) && regex.IsMatch(tdirEntry.SearchName))
				{
					results.Add(Utilities.CombinePaths(a, this.FormatFileName(tdirEntry2.FileName)));
				}
				if (subFolders && isDirectory)
				{
					this.DoSearch(results, Utilities.CombinePaths(a, this.FormatFileName(tdirEntry2.FileName)), regex, subFolders, dirs, files);
				}
			}
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00005EB0 File Offset: 0x000040B0
		private TDirEntry ResolveSymlink(TDirEntry entry, string path)
		{
			TDirEntry tdirEntry = entry;
			if (path.Length > 0 && path[0] != '\\')
			{
				path = "\\" + path;
			}
			string text = path;
			int num = 20;
			while (tdirEntry.IsSymlink && num > 0)
			{
				IVfsSymlink<TDirEntry, TFile> vfsSymlink = this.GetFile(tdirEntry) as IVfsSymlink<TDirEntry, TFile>;
				if (vfsSymlink == null)
				{
					throw new FileNotFoundException("Unable to resolve symlink", path);
				}
				text = Utilities.ResolvePath(text.TrimEnd(new char[]
				{
					'\\'
				}), vfsSymlink.TargetPath);
				tdirEntry = this.GetDirectoryEntry(text);
				if (tdirEntry == null)
				{
					throw new FileNotFoundException("Unable to resolve symlink", path);
				}
				num--;
			}
			if (tdirEntry.IsSymlink)
			{
				throw new FileNotFoundException("Unable to resolve symlink - too many links", path);
			}
			return tdirEntry;
		}

		// Token: 0x04000093 RID: 147
		private readonly ObjectCache<long, TFile> _fileCache;

		// Token: 0x02000099 RID: 153
		// (Invoke) Token: 0x06000520 RID: 1312
		protected delegate void DirEntryHandler(string path, TDirEntry dirEntry);
	}
}
