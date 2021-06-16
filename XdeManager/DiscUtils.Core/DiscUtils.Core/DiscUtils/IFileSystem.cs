using System;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils
{
	// Token: 0x0200001B RID: 27
	public interface IFileSystem
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000F6 RID: 246
		bool CanWrite { get; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000F7 RID: 247
		bool IsThreadSafe { get; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000F8 RID: 248
		DiscDirectoryInfo Root { get; }

		// Token: 0x060000F9 RID: 249
		void CopyFile(string sourceFile, string destinationFile);

		// Token: 0x060000FA RID: 250
		void CopyFile(string sourceFile, string destinationFile, bool overwrite);

		// Token: 0x060000FB RID: 251
		void CreateDirectory(string path);

		// Token: 0x060000FC RID: 252
		void DeleteDirectory(string path);

		// Token: 0x060000FD RID: 253
		void DeleteDirectory(string path, bool recursive);

		// Token: 0x060000FE RID: 254
		void DeleteFile(string path);

		// Token: 0x060000FF RID: 255
		bool DirectoryExists(string path);

		// Token: 0x06000100 RID: 256
		bool FileExists(string path);

		// Token: 0x06000101 RID: 257
		bool Exists(string path);

		// Token: 0x06000102 RID: 258
		string[] GetDirectories(string path);

		// Token: 0x06000103 RID: 259
		string[] GetDirectories(string path, string searchPattern);

		// Token: 0x06000104 RID: 260
		string[] GetDirectories(string path, string searchPattern, SearchOption searchOption);

		// Token: 0x06000105 RID: 261
		string[] GetFiles(string path);

		// Token: 0x06000106 RID: 262
		string[] GetFiles(string path, string searchPattern);

		// Token: 0x06000107 RID: 263
		string[] GetFiles(string path, string searchPattern, SearchOption searchOption);

		// Token: 0x06000108 RID: 264
		string[] GetFileSystemEntries(string path);

		// Token: 0x06000109 RID: 265
		string[] GetFileSystemEntries(string path, string searchPattern);

		// Token: 0x0600010A RID: 266
		void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName);

		// Token: 0x0600010B RID: 267
		void MoveFile(string sourceName, string destinationName);

		// Token: 0x0600010C RID: 268
		void MoveFile(string sourceName, string destinationName, bool overwrite);

		// Token: 0x0600010D RID: 269
		SparseStream OpenFile(string path, FileMode mode);

		// Token: 0x0600010E RID: 270
		SparseStream OpenFile(string path, FileMode mode, FileAccess access);

		// Token: 0x0600010F RID: 271
		FileAttributes GetAttributes(string path);

		// Token: 0x06000110 RID: 272
		void SetAttributes(string path, FileAttributes newValue);

		// Token: 0x06000111 RID: 273
		DateTime GetCreationTime(string path);

		// Token: 0x06000112 RID: 274
		void SetCreationTime(string path, DateTime newTime);

		// Token: 0x06000113 RID: 275
		DateTime GetCreationTimeUtc(string path);

		// Token: 0x06000114 RID: 276
		void SetCreationTimeUtc(string path, DateTime newTime);

		// Token: 0x06000115 RID: 277
		DateTime GetLastAccessTime(string path);

		// Token: 0x06000116 RID: 278
		void SetLastAccessTime(string path, DateTime newTime);

		// Token: 0x06000117 RID: 279
		DateTime GetLastAccessTimeUtc(string path);

		// Token: 0x06000118 RID: 280
		void SetLastAccessTimeUtc(string path, DateTime newTime);

		// Token: 0x06000119 RID: 281
		DateTime GetLastWriteTime(string path);

		// Token: 0x0600011A RID: 282
		void SetLastWriteTime(string path, DateTime newTime);

		// Token: 0x0600011B RID: 283
		DateTime GetLastWriteTimeUtc(string path);

		// Token: 0x0600011C RID: 284
		void SetLastWriteTimeUtc(string path, DateTime newTime);

		// Token: 0x0600011D RID: 285
		long GetFileLength(string path);

		// Token: 0x0600011E RID: 286
		DiscFileInfo GetFileInfo(string path);

		// Token: 0x0600011F RID: 287
		DiscDirectoryInfo GetDirectoryInfo(string path);

		// Token: 0x06000120 RID: 288
		DiscFileSystemInfo GetFileSystemInfo(string path);

		// Token: 0x06000121 RID: 289
		byte[] ReadBootCode();

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000122 RID: 290
		long Size { get; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000123 RID: 291
		long UsedSpace { get; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000124 RID: 292
		long AvailableSpace { get; }
	}
}
