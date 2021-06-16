using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils.Vhd
{
	// Token: 0x02000002 RID: 2
	public sealed class Disk : VirtualDisk
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public Disk(Stream stream, Ownership ownsStream)
		{
			this._files = new List<Tuple<DiskImageFile, Ownership>>();
			this._files.Add(new Tuple<DiskImageFile, Ownership>(new DiskImageFile(stream, ownsStream), Ownership.Dispose));
			if (this._files[0].Item1.NeedsParent)
			{
				throw new NotSupportedException("Differencing disks cannot be opened from a stream");
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020AC File Offset: 0x000002AC
		public Disk(string path)
		{
			DiskImageFile item = new DiskImageFile(path, FileAccess.ReadWrite);
			this._files = new List<Tuple<DiskImageFile, Ownership>>();
			this._files.Add(new Tuple<DiskImageFile, Ownership>(item, Ownership.Dispose));
			this.ResolveFileChain();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020EC File Offset: 0x000002EC
		public Disk(string path, FileAccess access)
		{
			DiskImageFile item = new DiskImageFile(path, access);
			this._files = new List<Tuple<DiskImageFile, Ownership>>();
			this._files.Add(new Tuple<DiskImageFile, Ownership>(item, Ownership.Dispose));
			this.ResolveFileChain();
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000212C File Offset: 0x0000032C
		public Disk(DiscFileSystem fileSystem, string path, FileAccess access)
		{
			DiskImageFile item = new DiskImageFile(new DiscFileLocator(fileSystem, Utilities.GetDirectoryFromPath(path)), Utilities.GetFileFromPath(path), access);
			this._files = new List<Tuple<DiskImageFile, Ownership>>();
			this._files.Add(new Tuple<DiskImageFile, Ownership>(item, Ownership.Dispose));
			this.ResolveFileChain();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000217C File Offset: 0x0000037C
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
				if (files[i].Information.DynamicParentUniqueId != files[i + 1].UniqueId)
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

		// Token: 0x06000006 RID: 6 RVA: 0x000022A8 File Offset: 0x000004A8
		internal Disk(FileLocator locator, string path, FileAccess access)
		{
			DiskImageFile item = new DiskImageFile(locator, path, access);
			this._files = new List<Tuple<DiskImageFile, Ownership>>();
			this._files.Add(new Tuple<DiskImageFile, Ownership>(item, Ownership.Dispose));
			this.ResolveFileChain();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000022E7 File Offset: 0x000004E7
		private Disk(DiskImageFile file, Ownership ownsFile)
		{
			this._files = new List<Tuple<DiskImageFile, Ownership>>();
			this._files.Add(new Tuple<DiskImageFile, Ownership>(file, ownsFile));
			this.ResolveFileChain();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002314 File Offset: 0x00000514
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

		// Token: 0x06000009 RID: 9 RVA: 0x0000236C File Offset: 0x0000056C
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

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000023CC File Offset: 0x000005CC
		// (set) Token: 0x0600000B RID: 11 RVA: 0x000023F0 File Offset: 0x000005F0
		public bool AutoCommitFooter
		{
			get
			{
				DynamicStream dynamicStream = this.Content as DynamicStream;
				return dynamicStream == null || dynamicStream.AutoCommitFooter;
			}
			set
			{
				DynamicStream dynamicStream = this.Content as DynamicStream;
				if (dynamicStream != null)
				{
					dynamicStream.AutoCommitFooter = value;
				}
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002413 File Offset: 0x00000613
		public override long Capacity
		{
			get
			{
				return this._files[0].Item1.Capacity;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000D RID: 13 RVA: 0x0000242C File Offset: 0x0000062C
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

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002481 File Offset: 0x00000681
		public override VirtualDiskClass DiskClass
		{
			get
			{
				return VirtualDiskClass.HardDisk;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002484 File Offset: 0x00000684
		public override VirtualDiskTypeInfo DiskTypeInfo
		{
			get
			{
				return DiskFactory.MakeDiskTypeInfo(this._files[this._files.Count - 1].Item1.IsSparse ? "dynamic" : "fixed");
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000024BB File Offset: 0x000006BB
		public override Geometry Geometry
		{
			get
			{
				return this._files[0].Item1.Geometry;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000024D3 File Offset: 0x000006D3
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

		// Token: 0x06000012 RID: 18 RVA: 0x000024E3 File Offset: 0x000006E3
		public static Disk InitializeFixed(Stream stream, Ownership ownsStream, long capacity)
		{
			return Disk.InitializeFixed(stream, ownsStream, capacity, null);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000024EE File Offset: 0x000006EE
		public static Disk InitializeFixed(Stream stream, Ownership ownsStream, long capacity, Geometry geometry)
		{
			return new Disk(DiskImageFile.InitializeFixed(stream, ownsStream, capacity, geometry), Ownership.Dispose);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000024FF File Offset: 0x000006FF
		public static Disk InitializeDynamic(Stream stream, Ownership ownsStream, long capacity)
		{
			return Disk.InitializeDynamic(stream, ownsStream, capacity, null);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000250A File Offset: 0x0000070A
		public static Disk InitializeDynamic(Stream stream, Ownership ownsStream, long capacity, Geometry geometry)
		{
			return new Disk(DiskImageFile.InitializeDynamic(stream, ownsStream, capacity, geometry), Ownership.Dispose);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000251B File Offset: 0x0000071B
		public static Disk InitializeDynamic(Stream stream, Ownership ownsStream, long capacity, long blockSize)
		{
			return new Disk(DiskImageFile.InitializeDynamic(stream, ownsStream, capacity, blockSize), Ownership.Dispose);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000252C File Offset: 0x0000072C
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

		// Token: 0x06000018 RID: 24 RVA: 0x00002598 File Offset: 0x00000798
		public static Disk InitializeDifferencing(Stream stream, Ownership ownsStream, DiskImageFile parent, Ownership ownsParent, string parentAbsolutePath, string parentRelativePath, DateTime parentModificationTime)
		{
			return new Disk(DiskImageFile.InitializeDifferencing(stream, ownsStream, parent, parentAbsolutePath, parentRelativePath, parentModificationTime), Ownership.Dispose, parent, ownsParent);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000025B0 File Offset: 0x000007B0
		public override VirtualDisk CreateDifferencingDisk(DiscFileSystem fileSystem, string path)
		{
			FileLocator fileLocator = new DiscFileLocator(fileSystem, Utilities.GetDirectoryFromPath(path));
			return new Disk(this._files[0].Item1.CreateDifferencing(fileLocator, Utilities.GetFileFromPath(path)), Ownership.Dispose);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000025F0 File Offset: 0x000007F0
		public override VirtualDisk CreateDifferencingDisk(string path)
		{
			FileLocator fileLocator = new LocalFileLocator(Path.GetDirectoryName(path));
			return new Disk(this._files[0].Item1.CreateDifferencing(fileLocator, Path.GetFileName(path)), Ownership.Dispose);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000262C File Offset: 0x0000082C
		internal static Disk InitializeFixed(FileLocator fileLocator, string path, long capacity, Geometry geometry)
		{
			return new Disk(DiskImageFile.InitializeFixed(fileLocator, path, capacity, geometry), Ownership.Dispose);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000263D File Offset: 0x0000083D
		internal static Disk InitializeDynamic(FileLocator fileLocator, string path, long capacity, Geometry geometry, long blockSize)
		{
			return new Disk(DiskImageFile.InitializeDynamic(fileLocator, path, capacity, geometry, blockSize), Ownership.Dispose);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002650 File Offset: 0x00000850
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

		// Token: 0x0600001E RID: 30 RVA: 0x000026F4 File Offset: 0x000008F4
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

		// Token: 0x04000001 RID: 1
		private SparseStream _content;

		// Token: 0x04000002 RID: 2
		private List<Tuple<DiskImageFile, Ownership>> _files;
	}
}
