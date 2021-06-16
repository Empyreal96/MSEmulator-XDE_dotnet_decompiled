using System;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils
{
	// Token: 0x02000025 RID: 37
	public abstract class ReadOnlyDiscFileSystem : DiscFileSystem
	{
		// Token: 0x0600018B RID: 395 RVA: 0x00004493 File Offset: 0x00002693
		protected ReadOnlyDiscFileSystem()
		{
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000449B File Offset: 0x0000269B
		protected ReadOnlyDiscFileSystem(DiscFileSystemOptions defaultOptions) : base(defaultOptions)
		{
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600018D RID: 397 RVA: 0x000044A4 File Offset: 0x000026A4
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000044A7 File Offset: 0x000026A7
		public override void CopyFile(string sourceFile, string destinationFile, bool overwrite)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000044AE File Offset: 0x000026AE
		public override void CreateDirectory(string path)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000044B5 File Offset: 0x000026B5
		public override void DeleteDirectory(string path)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000044BC File Offset: 0x000026BC
		public override void DeleteFile(string path)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000192 RID: 402 RVA: 0x000044C3 File Offset: 0x000026C3
		public override void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000193 RID: 403 RVA: 0x000044CA File Offset: 0x000026CA
		public override void MoveFile(string sourceName, string destinationName, bool overwrite)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000194 RID: 404 RVA: 0x000044D1 File Offset: 0x000026D1
		public override SparseStream OpenFile(string path, FileMode mode)
		{
			return this.OpenFile(path, mode, FileAccess.Read);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x000044DC File Offset: 0x000026DC
		public override void SetAttributes(string path, FileAttributes newValue)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000044E3 File Offset: 0x000026E3
		public override void SetCreationTimeUtc(string path, DateTime newTime)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000197 RID: 407 RVA: 0x000044EA File Offset: 0x000026EA
		public override void SetLastAccessTimeUtc(string path, DateTime newTime)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000044F1 File Offset: 0x000026F1
		public override void SetLastWriteTimeUtc(string path, DateTime newTime)
		{
			throw new NotSupportedException();
		}
	}
}
