using System;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Vfs
{
	// Token: 0x0200003D RID: 61
	public abstract class VfsFileSystemFacade : DiscFileSystem
	{
		// Token: 0x06000275 RID: 629 RVA: 0x00005F70 File Offset: 0x00004170
		protected VfsFileSystemFacade(DiscFileSystem toWrap)
		{
			this._wrapped = toWrap;
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000276 RID: 630 RVA: 0x00005F7F File Offset: 0x0000417F
		public override bool CanWrite
		{
			get
			{
				return this._wrapped.CanWrite;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000277 RID: 631 RVA: 0x00005F8C File Offset: 0x0000418C
		public override string FriendlyName
		{
			get
			{
				return this._wrapped.FriendlyName;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000278 RID: 632 RVA: 0x00005F99 File Offset: 0x00004199
		public override bool IsThreadSafe
		{
			get
			{
				return this._wrapped.IsThreadSafe;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000279 RID: 633 RVA: 0x00005FA6 File Offset: 0x000041A6
		public override DiscFileSystemOptions Options
		{
			get
			{
				return this._wrapped.Options;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600027A RID: 634 RVA: 0x00005FB3 File Offset: 0x000041B3
		public override DiscDirectoryInfo Root
		{
			get
			{
				return new DiscDirectoryInfo(this, string.Empty);
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600027B RID: 635 RVA: 0x00005FC0 File Offset: 0x000041C0
		public override string VolumeLabel
		{
			get
			{
				return this._wrapped.VolumeLabel;
			}
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00005FCD File Offset: 0x000041CD
		public override void CopyFile(string sourceFile, string destinationFile)
		{
			this._wrapped.CopyFile(sourceFile, destinationFile);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00005FDC File Offset: 0x000041DC
		public override void CopyFile(string sourceFile, string destinationFile, bool overwrite)
		{
			this._wrapped.CopyFile(sourceFile, destinationFile, overwrite);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00005FEC File Offset: 0x000041EC
		public override void CreateDirectory(string path)
		{
			this._wrapped.CreateDirectory(path);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00005FFA File Offset: 0x000041FA
		public override void DeleteDirectory(string path)
		{
			this._wrapped.DeleteDirectory(path);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00006008 File Offset: 0x00004208
		public override void DeleteDirectory(string path, bool recursive)
		{
			this._wrapped.DeleteDirectory(path, recursive);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00006017 File Offset: 0x00004217
		public override void DeleteFile(string path)
		{
			this._wrapped.DeleteFile(path);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00006025 File Offset: 0x00004225
		public override bool DirectoryExists(string path)
		{
			return this._wrapped.DirectoryExists(path);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00006033 File Offset: 0x00004233
		public override bool FileExists(string path)
		{
			return this._wrapped.FileExists(path);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00006041 File Offset: 0x00004241
		public override bool Exists(string path)
		{
			return this._wrapped.Exists(path);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000604F File Offset: 0x0000424F
		public override string[] GetDirectories(string path)
		{
			return this._wrapped.GetDirectories(path);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000605D File Offset: 0x0000425D
		public override string[] GetDirectories(string path, string searchPattern)
		{
			return this._wrapped.GetDirectories(path, searchPattern);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000606C File Offset: 0x0000426C
		public override string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
		{
			return this._wrapped.GetDirectories(path, searchPattern, searchOption);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000607C File Offset: 0x0000427C
		public override string[] GetFiles(string path)
		{
			return this._wrapped.GetFiles(path);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000608A File Offset: 0x0000428A
		public override string[] GetFiles(string path, string searchPattern)
		{
			return this._wrapped.GetFiles(path, searchPattern);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00006099 File Offset: 0x00004299
		public override string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
		{
			return this._wrapped.GetFiles(path, searchPattern, searchOption);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x000060A9 File Offset: 0x000042A9
		public override string[] GetFileSystemEntries(string path)
		{
			return this._wrapped.GetFileSystemEntries(path);
		}

		// Token: 0x0600028C RID: 652 RVA: 0x000060B7 File Offset: 0x000042B7
		public override string[] GetFileSystemEntries(string path, string searchPattern)
		{
			return this._wrapped.GetFileSystemEntries(path, searchPattern);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x000060C6 File Offset: 0x000042C6
		public override void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName)
		{
			this._wrapped.MoveDirectory(sourceDirectoryName, destinationDirectoryName);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x000060D5 File Offset: 0x000042D5
		public override void MoveFile(string sourceName, string destinationName)
		{
			this._wrapped.MoveFile(sourceName, destinationName);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x000060E4 File Offset: 0x000042E4
		public override void MoveFile(string sourceName, string destinationName, bool overwrite)
		{
			this._wrapped.MoveFile(sourceName, destinationName, overwrite);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x000060F4 File Offset: 0x000042F4
		public override SparseStream OpenFile(string path, FileMode mode)
		{
			return this._wrapped.OpenFile(path, mode);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00006103 File Offset: 0x00004303
		public override SparseStream OpenFile(string path, FileMode mode, FileAccess access)
		{
			return this._wrapped.OpenFile(path, mode, access);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00006113 File Offset: 0x00004313
		public override FileAttributes GetAttributes(string path)
		{
			return this._wrapped.GetAttributes(path);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00006121 File Offset: 0x00004321
		public override void SetAttributes(string path, FileAttributes newValue)
		{
			this._wrapped.SetAttributes(path, newValue);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00006130 File Offset: 0x00004330
		public override DateTime GetCreationTime(string path)
		{
			return this._wrapped.GetCreationTime(path);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000613E File Offset: 0x0000433E
		public override void SetCreationTime(string path, DateTime newTime)
		{
			this._wrapped.SetCreationTime(path, newTime);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000614D File Offset: 0x0000434D
		public override DateTime GetCreationTimeUtc(string path)
		{
			return this._wrapped.GetCreationTimeUtc(path);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000615B File Offset: 0x0000435B
		public override void SetCreationTimeUtc(string path, DateTime newTime)
		{
			this._wrapped.SetCreationTimeUtc(path, newTime);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000616A File Offset: 0x0000436A
		public override DateTime GetLastAccessTime(string path)
		{
			return this._wrapped.GetLastAccessTime(path);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00006178 File Offset: 0x00004378
		public override void SetLastAccessTime(string path, DateTime newTime)
		{
			this._wrapped.SetLastAccessTime(path, newTime);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00006187 File Offset: 0x00004387
		public override DateTime GetLastAccessTimeUtc(string path)
		{
			return this._wrapped.GetLastAccessTimeUtc(path);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00006195 File Offset: 0x00004395
		public override void SetLastAccessTimeUtc(string path, DateTime newTime)
		{
			this._wrapped.SetLastAccessTimeUtc(path, newTime);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x000061A4 File Offset: 0x000043A4
		public override DateTime GetLastWriteTime(string path)
		{
			return this._wrapped.GetLastWriteTime(path);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x000061B2 File Offset: 0x000043B2
		public override void SetLastWriteTime(string path, DateTime newTime)
		{
			this._wrapped.SetLastWriteTime(path, newTime);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x000061C1 File Offset: 0x000043C1
		public override DateTime GetLastWriteTimeUtc(string path)
		{
			return this._wrapped.GetLastWriteTimeUtc(path);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x000061CF File Offset: 0x000043CF
		public override void SetLastWriteTimeUtc(string path, DateTime newTime)
		{
			this._wrapped.SetLastWriteTimeUtc(path, newTime);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x000061DE File Offset: 0x000043DE
		public override long GetFileLength(string path)
		{
			return this._wrapped.GetFileLength(path);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x000061EC File Offset: 0x000043EC
		public override DiscFileInfo GetFileInfo(string path)
		{
			return new DiscFileInfo(this, path);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x000061F5 File Offset: 0x000043F5
		public override DiscDirectoryInfo GetDirectoryInfo(string path)
		{
			return new DiscDirectoryInfo(this, path);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x000061FE File Offset: 0x000043FE
		public override DiscFileSystemInfo GetFileSystemInfo(string path)
		{
			return new DiscFileSystemInfo(this, path);
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x00006207 File Offset: 0x00004407
		public override long Size
		{
			get
			{
				return this._wrapped.Size;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x00006214 File Offset: 0x00004414
		public override long UsedSpace
		{
			get
			{
				return this._wrapped.UsedSpace;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x00006221 File Offset: 0x00004421
		public override long AvailableSpace
		{
			get
			{
				return this._wrapped.AvailableSpace;
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000622E File Offset: 0x0000442E
		protected VfsFileSystem<TDirEntry, TFile, TDirectory, TContext> GetRealFileSystem<TDirEntry, TFile, TDirectory, TContext>() where TDirEntry : VfsDirEntry where TFile : IVfsFile where TDirectory : class, IVfsDirectory<TDirEntry, TFile>, TFile where TContext : VfsContext
		{
			return (VfsFileSystem<TDirEntry, TFile, TDirectory, TContext>)this._wrapped;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000623B File Offset: 0x0000443B
		protected T GetRealFileSystem<T>() where T : DiscFileSystem
		{
			return (T)((object)this._wrapped);
		}

		// Token: 0x04000096 RID: 150
		private readonly DiscFileSystem _wrapped;
	}
}
