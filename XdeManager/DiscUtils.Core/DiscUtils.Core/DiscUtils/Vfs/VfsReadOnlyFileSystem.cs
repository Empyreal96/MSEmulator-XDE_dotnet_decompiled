using System;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Vfs
{
	// Token: 0x02000042 RID: 66
	public abstract class VfsReadOnlyFileSystem<TDirEntry, TFile, TDirectory, TContext> : VfsFileSystem<TDirEntry, TFile, TDirectory, TContext> where TDirEntry : VfsDirEntry where TFile : IVfsFile where TDirectory : class, IVfsDirectory<TDirEntry, TFile>, TFile where TContext : VfsContext
	{
		// Token: 0x060002B7 RID: 695 RVA: 0x000062F2 File Offset: 0x000044F2
		protected VfsReadOnlyFileSystem(DiscFileSystemOptions defaultOptions) : base(defaultOptions)
		{
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x000062FB File Offset: 0x000044FB
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x000062FE File Offset: 0x000044FE
		public override void CopyFile(string sourceFile, string destinationFile, bool overwrite)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00006305 File Offset: 0x00004505
		public override void CreateDirectory(string path)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000630C File Offset: 0x0000450C
		public override void DeleteDirectory(string path)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00006313 File Offset: 0x00004513
		public override void DeleteFile(string path)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000631A File Offset: 0x0000451A
		public override void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00006321 File Offset: 0x00004521
		public override void MoveFile(string sourceName, string destinationName, bool overwrite)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00006328 File Offset: 0x00004528
		public override SparseStream OpenFile(string path, FileMode mode)
		{
			return this.OpenFile(path, mode, FileAccess.Read);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00006333 File Offset: 0x00004533
		public override void SetAttributes(string path, FileAttributes newValue)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000633A File Offset: 0x0000453A
		public override void SetCreationTimeUtc(string path, DateTime newTime)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00006341 File Offset: 0x00004541
		public override void SetLastAccessTimeUtc(string path, DateTime newTime)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00006348 File Offset: 0x00004548
		public override void SetLastWriteTimeUtc(string path, DateTime newTime)
		{
			throw new NotSupportedException();
		}
	}
}
