using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x02000006 RID: 6
	public sealed class Disk : VirtualDisk
	{
		// Token: 0x0600002C RID: 44 RVA: 0x00002C34 File Offset: 0x00000E34
		public Disk(Stream stream, Ownership ownsStream)
		{
			this._files = new List<Tuple<DiskImageFile, Ownership>>();
			this._files.Add(new Tuple<DiskImageFile, Ownership>(new DiskImageFile(stream, ownsStream), Ownership.Dispose));
			if (this._files[0].Item1.NeedsParent)
			{
				throw new NotSupportedException("Differencing disks cannot be opened from a stream");
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002C90 File Offset: 0x00000E90
		public Disk(string path)
		{
			DiskImageFile item = new DiskImageFile(path, FileAccess.ReadWrite);
			this._files = new List<Tuple<DiskImageFile, Ownership>>();
			this._files.Add(new Tuple<DiskImageFile, Ownership>(item, Ownership.Dispose));
			this.ResolveFileChain();
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002CD0 File Offset: 0x00000ED0
		public Disk(string path, FileAccess access)
		{
			DiskImageFile item = new DiskImageFile(path, access);
			this._files = new List<Tuple<DiskImageFile, Ownership>>();
			this._files.Add(new Tuple<DiskImageFile, Ownership>(item, Ownership.Dispose));
			this.ResolveFileChain();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002D10 File Offset: 0x00000F10
		public Disk(DiscFileSystem fileSystem, string path, FileAccess access)
		{
			DiskImageFile item = new DiskImageFile(new DiscFileLocator(fileSystem, Utilities.GetDirectoryFromPath(path)), Utilities.GetFileFromPath(path), access);
			this._files = new List<Tuple<DiskImageFile, Ownership>>();
			this._files.Add(new Tuple<DiskImageFile, Ownership>(item, Ownership.Dispose));
			this.ResolveFileChain();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002D60 File Offset: 0x00000F60
		public Disk(IList<DiskImageFile> files, Ownership ownsFiles)
		{
			if (files == null || files.Count == 0)
			{
				throw new ArgumentException("At least one file must be provided");
			}
			if (files[files.Count - 1].NeedsParent)
			{
				throw new ArgumentException("Final image file needs a parent");
			}
			List<Tuple<DiskImageFile, Ownership>> list = new List<Tuple<DiskImageFile, Ownership>>(files.Count);
			for (int i = 0; i < files.Count - 1; i++)
			{
				if (!files[i].NeedsParent)
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "File at index {0} does not have a parent disk", new object[]
					{
						i
					}));
				}
				if (files[i].ParentUniqueId != files[i + 1].UniqueId)
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "File at index {0} is not the parent of file at index {1} - Unique Ids don't match", new object[]
					{
						i + 1,
						i
					}));
				}
				list.Add(new Tuple<DiskImageFile, Ownership>(files[i], ownsFiles));
			}
			list.Add(new Tuple<DiskImageFile, Ownership>(files[files.Count - 1], ownsFiles));
			this._files = list;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002E84 File Offset: 0x00001084
		internal Disk(FileLocator locator, string path, FileAccess access)
		{
			DiskImageFile item = new DiskImageFile(locator, path, access);
			this._files = new List<Tuple<DiskImageFile, Ownership>>();
			this._files.Add(new Tuple<DiskImageFile, Ownership>(item, Ownership.Dispose));
			this.ResolveFileChain();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002EC3 File Offset: 0x000010C3
		private Disk(DiskImageFile file, Ownership ownsFile)
		{
			this._files = new List<Tuple<DiskImageFile, Ownership>>();
			this._files.Add(new Tuple<DiskImageFile, Ownership>(file, ownsFile));
			this.ResolveFileChain();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002EF0 File Offset: 0x000010F0
		private Disk(DiskImageFile file, Ownership ownsFile, FileLocator parentLocator, string parentPath)
		{
			this._files = new List<Tuple<DiskImageFile, Ownership>>();
			this._files.Add(new Tuple<DiskImageFile, Ownership>(file, ownsFile));
			if (file.NeedsParent)
			{
				this._files.Add(new Tuple<DiskImageFile, Ownership>(new DiskImageFile(parentLocator, parentPath, FileAccess.Read), Ownership.Dispose));
				this.ResolveFileChain();
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002F48 File Offset: 0x00001148
		private Disk(DiskImageFile file, Ownership ownsFile, DiskImageFile parentFile, Ownership ownsParent)
		{
			this._files = new List<Tuple<DiskImageFile, Ownership>>();
			this._files.Add(new Tuple<DiskImageFile, Ownership>(file, ownsFile));
			if (file.NeedsParent)
			{
				this._files.Add(new Tuple<DiskImageFile, Ownership>(parentFile, ownsParent));
				this.ResolveFileChain();
				return;
			}
			if (parentFile != null && ownsParent == Ownership.Dispose)
			{
				parentFile.Dispose();
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002FA8 File Offset: 0x000011A8
		public override int BlockSize
		{
			get
			{
				return (int)this._files[0].Item1.LogicalSectorSize;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002FC1 File Offset: 0x000011C1
		public override long Capacity
		{
			get
			{
				return this._files[0].Item1.Capacity;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002FDC File Offset: 0x000011DC
		public override SparseStream Content
		{
			get
			{
				if (this._content == null)
				{
					SparseStream sparseStream = null;
					for (int i = this._files.Count - 1; i >= 0; i--)
					{
						sparseStream = this._files[i].Item1.OpenContent(sparseStream, Ownership.Dispose);
					}
					this._content = sparseStream;
				}
				return this._content;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00003031 File Offset: 0x00001231
		public override VirtualDiskClass DiskClass
		{
			get
			{
				return VirtualDiskClass.HardDisk;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00003034 File Offset: 0x00001234
		public override VirtualDiskTypeInfo DiskTypeInfo
		{
			get
			{
				return DiskFactory.MakeDiskTypeInfo(this._files[this._files.Count - 1].Item1.IsSparse ? "dynamic" : "fixed");
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600003A RID: 58 RVA: 0x0000306B File Offset: 0x0000126B
		public override Geometry Geometry
		{
			get
			{
				return this._files[0].Item1.Geometry;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00003083 File Offset: 0x00001283
		public override IEnumerable<VirtualDiskLayer> Layers
		{
			get
			{
				foreach (Tuple<DiskImageFile, Ownership> tuple in this._files)
				{
					yield return tuple.Item1;
				}
				List<Tuple<DiskImageFile, Ownership>>.Enumerator enumerator = default(List<Tuple<DiskImageFile, Ownership>>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003093 File Offset: 0x00001293
		public static Disk InitializeFixed(Stream stream, Ownership ownsStream, long capacity)
		{
			return Disk.InitializeFixed(stream, ownsStream, capacity, null);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0000309E File Offset: 0x0000129E
		public static Disk InitializeFixed(Stream stream, Ownership ownsStream, long capacity, Geometry geometry)
		{
			return new Disk(DiskImageFile.InitializeFixed(stream, ownsStream, capacity, geometry), Ownership.Dispose);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000030AF File Offset: 0x000012AF
		public static Disk InitializeDynamic(Stream stream, Ownership ownsStream, long capacity)
		{
			return new Disk(DiskImageFile.InitializeDynamic(stream, ownsStream, capacity), Ownership.Dispose);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000030BF File Offset: 0x000012BF
		public static Disk InitializeDynamic(Stream stream, Ownership ownsStream, long capacity, long blockSize)
		{
			return new Disk(DiskImageFile.InitializeDynamic(stream, ownsStream, capacity, blockSize), Ownership.Dispose);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000030D0 File Offset: 0x000012D0
		public static Disk InitializeDifferencing(string path, string parentPath)
		{
			LocalFileLocator localFileLocator = new LocalFileLocator(Path.GetDirectoryName(parentPath));
			string fileName = Path.GetFileName(parentPath);
			DiskImageFile file;
			using (DiskImageFile diskImageFile = new DiskImageFile(localFileLocator, fileName, FileAccess.Read))
			{
				LocalFileLocator fileLocator = new LocalFileLocator(Path.GetDirectoryName(path));
				file = diskImageFile.CreateDifferencing(fileLocator, Path.GetFileName(path));
			}
			return new Disk(file, Ownership.Dispose, localFileLocator, fileName);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000313C File Offset: 0x0000133C
		public static Disk InitializeDifferencing(Stream stream, Ownership ownsStream, DiskImageFile parent, Ownership ownsParent, string parentAbsolutePath, string parentRelativePath, DateTime parentModificationTime)
		{
			return new Disk(DiskImageFile.InitializeDifferencing(stream, ownsStream, parent, parentAbsolutePath, parentRelativePath, parentModificationTime), Ownership.Dispose, parent, ownsParent);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003154 File Offset: 0x00001354
		public override VirtualDisk CreateDifferencingDisk(DiscFileSystem fileSystem, string path)
		{
			FileLocator fileLocator = new DiscFileLocator(fileSystem, Utilities.GetDirectoryFromPath(path));
			return new Disk(this._files[0].Item1.CreateDifferencing(fileLocator, Utilities.GetFileFromPath(path)), Ownership.Dispose);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003194 File Offset: 0x00001394
		public override VirtualDisk CreateDifferencingDisk(string path)
		{
			FileLocator fileLocator = new LocalFileLocator(Path.GetDirectoryName(path));
			return new Disk(this._files[0].Item1.CreateDifferencing(fileLocator, Path.GetFileName(path)), Ownership.Dispose);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000031D0 File Offset: 0x000013D0
		internal static Disk InitializeFixed(FileLocator fileLocator, string path, long capacity, Geometry geometry)
		{
			return new Disk(DiskImageFile.InitializeFixed(fileLocator, path, capacity, geometry), Ownership.Dispose);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000031E1 File Offset: 0x000013E1
		internal static Disk InitializeDynamic(FileLocator fileLocator, string path, long capacity, long blockSize)
		{
			return new Disk(DiskImageFile.InitializeDynamic(fileLocator, path, capacity, blockSize), Ownership.Dispose);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000031F4 File Offset: 0x000013F4
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					if (this._content != null)
					{
						this._content.Dispose();
						this._content = null;
					}
					if (this._files != null)
					{
						foreach (Tuple<DiskImageFile, Ownership> tuple in this._files)
						{
							if (tuple.Item2 == Ownership.Dispose)
							{
								tuple.Item1.Dispose();
							}
						}
						this._files = null;
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003298 File Offset: 0x00001498
		private void ResolveFileChain()
		{
			DiskImageFile diskImageFile = this._files[this._files.Count - 1].Item1;
			while (diskImageFile.NeedsParent)
			{
				FileLocator relativeFileLocator = diskImageFile.RelativeFileLocator;
				bool flag = false;
				string[] parentLocations = diskImageFile.GetParentLocations();
				int i = 0;
				while (i < parentLocations.Length)
				{
					string text = parentLocations[i];
					if (relativeFileLocator.Exists(text))
					{
						DiskImageFile diskImageFile2 = new DiskImageFile(relativeFileLocator, text, FileAccess.Read);
						if (diskImageFile2.UniqueId != diskImageFile.ParentUniqueId)
						{
							throw new IOException(string.Format(CultureInfo.CurrentUICulture, "Invalid disk chain found looking for parent with id {0}, found {1} with id {2}", new object[]
							{
								diskImageFile.ParentUniqueId,
								diskImageFile2.FullPath,
								diskImageFile2.UniqueId
							}));
						}
						diskImageFile = diskImageFile2;
						this._files.Add(new Tuple<DiskImageFile, Ownership>(diskImageFile, Ownership.Dispose));
						flag = true;
						break;
					}
					else
					{
						i++;
					}
				}
				if (!flag)
				{
					throw new IOException(string.Format(CultureInfo.InvariantCulture, "Failed to find parent for disk '{0}'", new object[]
					{
						diskImageFile.FullPath
					}));
				}
			}
		}

		// Token: 0x0400001A RID: 26
		private SparseStream _content;

		// Token: 0x0400001B RID: 27
		private List<Tuple<DiskImageFile, Ownership>> _files;
	}
}
