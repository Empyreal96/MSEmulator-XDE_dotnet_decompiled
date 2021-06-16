using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils.Vhd
{
	// Token: 0x02000006 RID: 6
	public sealed class DiskImageFile : VirtualDiskLayer
	{
		// Token: 0x06000031 RID: 49 RVA: 0x00002B22 File Offset: 0x00000D22
		public DiskImageFile(Stream stream)
		{
			this._fileStream = stream;
			this.ReadFooter(true);
			this.ReadHeaders();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002B3E File Offset: 0x00000D3E
		public DiskImageFile(Stream stream, Ownership ownsStream)
		{
			this._fileStream = stream;
			this._ownsStream = ownsStream;
			this.ReadFooter(true);
			this.ReadHeaders();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002B61 File Offset: 0x00000D61
		public DiskImageFile(string path, FileAccess access) : this(new LocalFileLocator(Path.GetDirectoryName(path)), Path.GetFileName(path), access)
		{
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002B7B File Offset: 0x00000D7B
		internal DiskImageFile(FileLocator locator, string path, Stream stream, Ownership ownsStream) : this(stream, ownsStream)
		{
			this._fileLocator = locator.GetRelativeLocator(locator.GetDirectoryFromPath(path));
			this._fileName = locator.GetFileFromPath(path);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002BA8 File Offset: 0x00000DA8
		internal DiskImageFile(FileLocator locator, string path, FileAccess access)
		{
			FileShare share = (access == FileAccess.Read) ? FileShare.Read : FileShare.None;
			this._fileStream = locator.Open(path, FileMode.Open, access, share);
			this._ownsStream = Ownership.Dispose;
			try
			{
				this._fileLocator = locator.GetRelativeLocator(locator.GetDirectoryFromPath(path));
				this._fileName = locator.GetFileFromPath(path);
				this.ReadFooter(true);
				this.ReadHeaders();
			}
			catch
			{
				this._fileStream.Dispose();
				throw;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002C28 File Offset: 0x00000E28
		internal override long Capacity
		{
			get
			{
				return this._footer.CurrentSize;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002C35 File Offset: 0x00000E35
		public DateTime CreationTimestamp
		{
			get
			{
				return this._footer.Timestamp;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002C42 File Offset: 0x00000E42
		public override IList<VirtualDiskExtent> Extents
		{
			get
			{
				return new List<VirtualDiskExtent>
				{
					new DiskExtent(this)
				};
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002C55 File Offset: 0x00000E55
		public override string FullPath
		{
			get
			{
				if (this._fileLocator != null && this._fileName != null)
				{
					return this._fileLocator.GetFullPath(this._fileName);
				}
				return string.Empty;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002C7E File Offset: 0x00000E7E
		public override Geometry Geometry
		{
			get
			{
				return this._footer.Geometry;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002C8B File Offset: 0x00000E8B
		public DiskImageFileInfo Information
		{
			get
			{
				return new DiskImageFileInfo(this._footer, this._dynamicHeader, this._fileStream);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002CA4 File Offset: 0x00000EA4
		public override bool IsSparse
		{
			get
			{
				return this._footer.DiskType != FileType.Fixed;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002CB7 File Offset: 0x00000EB7
		public override bool NeedsParent
		{
			get
			{
				return this._footer.DiskType == FileType.Differencing;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002CC7 File Offset: 0x00000EC7
		public Guid ParentUniqueId
		{
			get
			{
				if (this._dynamicHeader != null)
				{
					return this._dynamicHeader.ParentUniqueId;
				}
				return Guid.Empty;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002CE2 File Offset: 0x00000EE2
		internal override FileLocator RelativeFileLocator
		{
			get
			{
				return this._fileLocator;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002CEA File Offset: 0x00000EEA
		internal long StoredSize
		{
			get
			{
				return this._fileStream.Length;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002CF7 File Offset: 0x00000EF7
		public Guid UniqueId
		{
			get
			{
				return this._footer.UniqueId;
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002D04 File Offset: 0x00000F04
		public static DiskImageFile InitializeFixed(Stream stream, Ownership ownsStream, long capacity)
		{
			return DiskImageFile.InitializeFixed(stream, ownsStream, capacity, null);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002D0F File Offset: 0x00000F0F
		public static DiskImageFile InitializeFixed(Stream stream, Ownership ownsStream, long capacity, Geometry geometry)
		{
			DiskImageFile.InitializeFixedInternal(stream, capacity, geometry);
			return new DiskImageFile(stream, ownsStream);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002D20 File Offset: 0x00000F20
		public static DiskImageFile InitializeDynamic(Stream stream, Ownership ownsStream, long capacity)
		{
			return DiskImageFile.InitializeDynamic(stream, ownsStream, capacity, null, 2097152L);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002D31 File Offset: 0x00000F31
		public static DiskImageFile InitializeDynamic(Stream stream, Ownership ownsStream, long capacity, Geometry geometry)
		{
			return DiskImageFile.InitializeDynamic(stream, ownsStream, capacity, geometry, 2097152L);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002D42 File Offset: 0x00000F42
		public static DiskImageFile InitializeDynamic(Stream stream, Ownership ownsStream, long capacity, long blockSize)
		{
			return DiskImageFile.InitializeDynamic(stream, ownsStream, capacity, null, blockSize);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002D4E File Offset: 0x00000F4E
		public static DiskImageFile InitializeDynamic(Stream stream, Ownership ownsStream, long capacity, Geometry geometry, long blockSize)
		{
			DiskImageFile.InitializeDynamicInternal(stream, capacity, geometry, blockSize);
			return new DiskImageFile(stream, ownsStream);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002D61 File Offset: 0x00000F61
		public static DiskImageFile InitializeDifferencing(Stream stream, Ownership ownsStream, DiskImageFile parent, string parentAbsolutePath, string parentRelativePath, DateTime parentModificationTimeUtc)
		{
			DiskImageFile.InitializeDifferencingInternal(stream, parent, parentAbsolutePath, parentRelativePath, parentModificationTimeUtc);
			return new DiskImageFile(stream, ownsStream);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002D76 File Offset: 0x00000F76
		public override SparseStream OpenContent(SparseStream parent, Ownership ownsParent)
		{
			return this.DoOpenContent(parent, ownsParent);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002D80 File Offset: 0x00000F80
		public override string[] GetParentLocations()
		{
			return this.GetParentLocations(this._fileLocator);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002D8E File Offset: 0x00000F8E
		[Obsolete("Use GetParentLocations() by preference")]
		public string[] GetParentLocations(string basePath)
		{
			return this.GetParentLocations(new LocalFileLocator(basePath));
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002D9C File Offset: 0x00000F9C
		internal static DiskImageFile InitializeFixed(FileLocator locator, string path, long capacity, Geometry geometry)
		{
			DiskImageFile result = null;
			Stream stream = locator.Open(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
			try
			{
				DiskImageFile.InitializeFixedInternal(stream, capacity, geometry);
				result = new DiskImageFile(locator, path, stream, Ownership.Dispose);
				stream = null;
			}
			finally
			{
				if (stream != null)
				{
					stream.Dispose();
				}
			}
			return result;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002DE8 File Offset: 0x00000FE8
		internal static DiskImageFile InitializeDynamic(FileLocator locator, string path, long capacity, Geometry geometry, long blockSize)
		{
			DiskImageFile result = null;
			Stream stream = locator.Open(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
			try
			{
				DiskImageFile.InitializeDynamicInternal(stream, capacity, geometry, blockSize);
				result = new DiskImageFile(locator, path, stream, Ownership.Dispose);
				stream = null;
			}
			finally
			{
				if (stream != null)
				{
					stream.Dispose();
				}
			}
			return result;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002E38 File Offset: 0x00001038
		internal DiskImageFile CreateDifferencing(FileLocator fileLocator, string path)
		{
			Stream stream = fileLocator.Open(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
			string fullPath = this._fileLocator.GetFullPath(this._fileName);
			string parentRelativePath = fileLocator.MakeRelativePath(this._fileLocator, this._fileName);
			DateTime lastWriteTimeUtc = this._fileLocator.GetLastWriteTimeUtc(this._fileName);
			DiskImageFile.InitializeDifferencingInternal(stream, this, fullPath, parentRelativePath, lastWriteTimeUtc);
			return new DiskImageFile(fileLocator, path, stream, Ownership.Dispose);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002E9C File Offset: 0x0000109C
		internal MappedStream DoOpenContent(SparseStream parent, Ownership ownsParent)
		{
			if (this._footer.DiskType == FileType.Fixed)
			{
				if (parent != null && ownsParent == Ownership.Dispose)
				{
					parent.Dispose();
				}
				return new SubStream(this._fileStream, 0L, this._fileStream.Length - 512L);
			}
			if (this._footer.DiskType == FileType.Dynamic)
			{
				if (parent != null && ownsParent == Ownership.Dispose)
				{
					parent.Dispose();
				}
				return new DynamicStream(this._fileStream, this._dynamicHeader, this._footer.CurrentSize, new ZeroStream(this._footer.CurrentSize), Ownership.Dispose);
			}
			if (parent == null)
			{
				parent = new ZeroStream(this._footer.CurrentSize);
				ownsParent = Ownership.Dispose;
			}
			return new DynamicStream(this._fileStream, this._dynamicHeader, this._footer.CurrentSize, parent, ownsParent);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002F64 File Offset: 0x00001164
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					if (this._ownsStream == Ownership.Dispose && this._fileStream != null)
					{
						this._fileStream.Dispose();
					}
					this._fileStream = null;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002FB4 File Offset: 0x000011B4
		private static void InitializeFixedInternal(Stream stream, long capacity, Geometry geometry)
		{
			if (geometry == null)
			{
				geometry = Geometry.FromCapacity(capacity);
			}
			Footer footer = new Footer(geometry, capacity, FileType.Fixed);
			footer.UpdateChecksum();
			byte[] array = new byte[512];
			footer.ToBytes(array, 0);
			stream.Position = MathUtilities.RoundUp(capacity, 512L);
			stream.Write(array, 0, array.Length);
			stream.SetLength(stream.Position);
			stream.Position = 0L;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003020 File Offset: 0x00001220
		private static void InitializeDynamicInternal(Stream stream, long capacity, Geometry geometry, long blockSize)
		{
			if (blockSize > (long)((ulong)-1) || blockSize < 0L)
			{
				throw new ArgumentOutOfRangeException("blockSize", "Must be in the range 0 to uint.MaxValue");
			}
			if (geometry == null)
			{
				geometry = Geometry.FromCapacity(capacity);
			}
			Footer footer = new Footer(geometry, capacity, FileType.Dynamic);
			footer.DataOffset = 512L;
			footer.UpdateChecksum();
			byte[] buffer = new byte[512];
			footer.ToBytes(buffer, 0);
			DynamicHeader dynamicHeader = new DynamicHeader(-1L, 1536L, (uint)blockSize, capacity);
			dynamicHeader.UpdateChecksum();
			byte[] array = new byte[1024];
			dynamicHeader.ToBytes(array, 0);
			int num = (dynamicHeader.MaxTableEntries * 4 + 512 - 1) / 512 * 512;
			byte[] array2 = new byte[num];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = byte.MaxValue;
			}
			stream.Position = 0L;
			stream.Write(buffer, 0, 512);
			stream.Write(array, 0, 1024);
			stream.Write(array2, 0, num);
			stream.Write(buffer, 0, 512);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003120 File Offset: 0x00001320
		private static void InitializeDifferencingInternal(Stream stream, DiskImageFile parent, string parentAbsolutePath, string parentRelativePath, DateTime parentModificationTimeUtc)
		{
			Footer footer = new Footer(parent.Geometry, parent._footer.CurrentSize, FileType.Differencing);
			footer.DataOffset = 512L;
			footer.OriginalSize = parent._footer.OriginalSize;
			footer.UpdateChecksum();
			byte[] buffer = new byte[512];
			footer.ToBytes(buffer, 0);
			long num = 1536L;
			uint blockSize = (parent._dynamicHeader == null) ? 2097152U : parent._dynamicHeader.BlockSize;
			DynamicHeader dynamicHeader = new DynamicHeader(-1L, num, blockSize, footer.CurrentSize);
			int num2 = (dynamicHeader.MaxTableEntries * 4 + 512 - 1) / 512 * 512;
			dynamicHeader.ParentUniqueId = parent.UniqueId;
			dynamicHeader.ParentTimestamp = parentModificationTimeUtc;
			dynamicHeader.ParentUnicodeName = Utilities.GetFileFromPath(parentAbsolutePath);
			dynamicHeader.ParentLocators[7].PlatformCode = "W2ku";
			dynamicHeader.ParentLocators[7].PlatformDataSpace = 512;
			dynamicHeader.ParentLocators[7].PlatformDataLength = parentAbsolutePath.Length * 2;
			dynamicHeader.ParentLocators[7].PlatformDataOffset = num + (long)num2;
			dynamicHeader.ParentLocators[6].PlatformCode = "W2ru";
			dynamicHeader.ParentLocators[6].PlatformDataSpace = 512;
			dynamicHeader.ParentLocators[6].PlatformDataLength = parentRelativePath.Length * 2;
			dynamicHeader.ParentLocators[6].PlatformDataOffset = num + (long)num2 + 512L;
			dynamicHeader.UpdateChecksum();
			byte[] array = new byte[1024];
			dynamicHeader.ToBytes(array, 0);
			byte[] array2 = new byte[512];
			Encoding.Unicode.GetBytes(parentAbsolutePath, 0, parentAbsolutePath.Length, array2, 0);
			byte[] array3 = new byte[512];
			Encoding.Unicode.GetBytes(parentRelativePath, 0, parentRelativePath.Length, array3, 0);
			byte[] array4 = new byte[num2];
			for (int i = 0; i < array4.Length; i++)
			{
				array4[i] = byte.MaxValue;
			}
			stream.Position = 0L;
			stream.Write(buffer, 0, 512);
			stream.Write(array, 0, 1024);
			stream.Write(array4, 0, num2);
			stream.Write(array2, 0, 512);
			stream.Write(array3, 0, 512);
			stream.Write(buffer, 0, 512);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003364 File Offset: 0x00001564
		private string[] GetParentLocations(FileLocator fileLocator)
		{
			if (!this.NeedsParent)
			{
				throw new InvalidOperationException("Only differencing disks contain parent locations");
			}
			if (fileLocator == null)
			{
				fileLocator = new LocalFileLocator(string.Empty);
			}
			List<string> list = new List<string>(8);
			List<string> list2 = new List<string>(8);
			foreach (ParentLocator parentLocator in this._dynamicHeader.ParentLocators)
			{
				if (parentLocator.PlatformCode == "W2ku" || parentLocator.PlatformCode == "W2ru")
				{
					this._fileStream.Position = parentLocator.PlatformDataOffset;
					byte[] bytes = StreamUtilities.ReadExact(this._fileStream, parentLocator.PlatformDataLength);
					string @string = Encoding.Unicode.GetString(bytes);
					if (parentLocator.PlatformCode == "W2ku")
					{
						list.Add(@string);
					}
					else
					{
						list2.Add(fileLocator.ResolveRelativePath(@string));
					}
				}
			}
			List<string> list3 = new List<string>(list.Count + list2.Count + 1);
			list3.AddRange(list);
			list3.AddRange(list2);
			if (list3.Count == 0)
			{
				list3.Add(fileLocator.ResolveRelativePath(this._dynamicHeader.ParentUnicodeName));
			}
			return list3.ToArray();
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003498 File Offset: 0x00001698
		private void ReadFooter(bool fallbackToFront)
		{
			this._fileStream.Position = this._fileStream.Length - 512L;
			byte[] buffer = StreamUtilities.ReadExact(this._fileStream, 512);
			this._footer = Footer.FromBytes(buffer, 0);
			if (!this._footer.IsValid())
			{
				if (!fallbackToFront)
				{
					throw new IOException("Corrupt VHD file - invalid footer at end (did not check front of file)");
				}
				this._fileStream.Position = 0L;
				StreamUtilities.ReadExact(this._fileStream, buffer, 0, 512);
				this._footer = Footer.FromBytes(buffer, 0);
				if (!this._footer.IsValid())
				{
					throw new IOException("Failed to find a valid VHD footer at start or end of file - VHD file is corrupt");
				}
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003540 File Offset: 0x00001740
		private void ReadHeaders()
		{
			Header header;
			for (long dataOffset = this._footer.DataOffset; dataOffset != -1L; dataOffset = header.DataOffset)
			{
				this._fileStream.Position = dataOffset;
				header = Header.FromStream(this._fileStream);
				if (header.Cookie == "cxsparse")
				{
					this._fileStream.Position = dataOffset;
					this._dynamicHeader = DynamicHeader.FromStream(this._fileStream);
					if (!this._dynamicHeader.IsValid())
					{
						throw new IOException("Invalid Dynamic Disc Header");
					}
				}
			}
		}

		// Token: 0x04000005 RID: 5
		private DynamicHeader _dynamicHeader;

		// Token: 0x04000006 RID: 6
		private readonly FileLocator _fileLocator;

		// Token: 0x04000007 RID: 7
		private readonly string _fileName;

		// Token: 0x04000008 RID: 8
		private Stream _fileStream;

		// Token: 0x04000009 RID: 9
		private Footer _footer;

		// Token: 0x0400000A RID: 10
		private readonly Ownership _ownsStream;
	}
}
