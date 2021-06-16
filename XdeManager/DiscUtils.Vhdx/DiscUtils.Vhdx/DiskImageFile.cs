using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x0200000A RID: 10
	public sealed class DiskImageFile : VirtualDiskLayer
	{
		// Token: 0x0600005C RID: 92 RVA: 0x00003636 File Offset: 0x00001836
		public DiskImageFile(Stream stream)
		{
			this._fileStream = stream;
			this.Initialize();
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000364B File Offset: 0x0000184B
		public DiskImageFile(Stream stream, Ownership ownsStream)
		{
			this._fileStream = stream;
			this._ownsStream = ownsStream;
			this.Initialize();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003667 File Offset: 0x00001867
		public DiskImageFile(string path, FileAccess access) : this(new LocalFileLocator(Path.GetDirectoryName(path)), Path.GetFileName(path), access)
		{
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003681 File Offset: 0x00001881
		internal DiskImageFile(FileLocator locator, string path, Stream stream, Ownership ownsStream) : this(stream, ownsStream)
		{
			this._fileLocator = locator.GetRelativeLocator(locator.GetDirectoryFromPath(path));
			this._fileName = locator.GetFileFromPath(path);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000036AC File Offset: 0x000018AC
		internal DiskImageFile(FileLocator locator, string path, FileAccess access)
		{
			FileShare share = (access == FileAccess.Read) ? FileShare.Read : FileShare.None;
			this._fileStream = locator.Open(path, FileMode.Open, access, share);
			this._ownsStream = Ownership.Dispose;
			try
			{
				this._fileLocator = locator.GetRelativeLocator(locator.GetDirectoryFromPath(path));
				this._fileName = locator.GetFileFromPath(path);
				this.Initialize();
			}
			catch
			{
				this._fileStream.Dispose();
				throw;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00003728 File Offset: 0x00001928
		internal override long Capacity
		{
			get
			{
				return (long)this._metadata.DiskSize;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00003735 File Offset: 0x00001935
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

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00003748 File Offset: 0x00001948
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

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00003771 File Offset: 0x00001971
		public override Geometry Geometry
		{
			get
			{
				return Geometry.FromCapacity(this.Capacity, (int)this._metadata.LogicalSectorSize);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000065 RID: 101 RVA: 0x0000378C File Offset: 0x0000198C
		public DiskImageFileInfo Information
		{
			get
			{
				this._fileStream.Position = 0L;
				FileHeader fileHeader = StreamUtilities.ReadStruct<FileHeader>(this._fileStream);
				this._fileStream.Position = 65536L;
				VhdxHeader vhdxHeader = StreamUtilities.ReadStruct<VhdxHeader>(this._fileStream);
				this._fileStream.Position = 131072L;
				VhdxHeader vhdxHeader2 = StreamUtilities.ReadStruct<VhdxHeader>(this._fileStream);
				LogSequence activeLogSequence = this.FindActiveLogSequence();
				return new DiskImageFileInfo(fileHeader, vhdxHeader, vhdxHeader2, this._regionTable, this._metadata, activeLogSequence);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00003806 File Offset: 0x00001A06
		public override bool IsSparse
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00003809 File Offset: 0x00001A09
		public long LogicalSectorSize
		{
			get
			{
				return (long)((ulong)this._metadata.LogicalSectorSize);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00003817 File Offset: 0x00001A17
		public override bool NeedsParent
		{
			get
			{
				return (this._metadata.FileParameters.Flags & FileParametersFlags.HasParent) > FileParametersFlags.None;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00003830 File Offset: 0x00001A30
		public Guid ParentUniqueId
		{
			get
			{
				if ((this._metadata.FileParameters.Flags & FileParametersFlags.HasParent) == FileParametersFlags.None)
				{
					return Guid.Empty;
				}
				string g;
				if (this._metadata.ParentLocator.Entries.TryGetValue("parent_linkage", out g))
				{
					return new Guid(g);
				}
				return Guid.Empty;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00003881 File Offset: 0x00001A81
		internal override FileLocator RelativeFileLocator
		{
			get
			{
				return this._fileLocator;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00003889 File Offset: 0x00001A89
		internal long StoredSize
		{
			get
			{
				return this._fileStream.Length;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00003896 File Offset: 0x00001A96
		public Guid UniqueId
		{
			get
			{
				return this._header.DataWriteGuid;
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000038A3 File Offset: 0x00001AA3
		public static DiskImageFile InitializeFixed(Stream stream, Ownership ownsStream, long capacity)
		{
			return DiskImageFile.InitializeFixed(stream, ownsStream, capacity, null);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000038AE File Offset: 0x00001AAE
		public static DiskImageFile InitializeFixed(Stream stream, Ownership ownsStream, long capacity, Geometry geometry)
		{
			DiskImageFile.InitializeFixedInternal(stream, capacity, geometry);
			return new DiskImageFile(stream, ownsStream);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000038BF File Offset: 0x00001ABF
		public static DiskImageFile InitializeDynamic(Stream stream, Ownership ownsStream, long capacity)
		{
			DiskImageFile.InitializeDynamicInternal(stream, capacity, 33554432L);
			return new DiskImageFile(stream, ownsStream);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000038D5 File Offset: 0x00001AD5
		public static DiskImageFile InitializeDynamic(Stream stream, Ownership ownsStream, long capacity, long blockSize)
		{
			DiskImageFile.InitializeDynamicInternal(stream, capacity, blockSize);
			return new DiskImageFile(stream, ownsStream);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000038E6 File Offset: 0x00001AE6
		public static DiskImageFile InitializeDifferencing(Stream stream, Ownership ownsStream, DiskImageFile parent, string parentAbsolutePath, string parentRelativePath, DateTime parentModificationTimeUtc)
		{
			DiskImageFile.InitializeDifferencingInternal(stream, parent, parentAbsolutePath, parentRelativePath, parentModificationTimeUtc);
			return new DiskImageFile(stream, ownsStream);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000038FC File Offset: 0x00001AFC
		public Stream OpenRegion(Guid region)
		{
			RegionEntry regionEntry = this._regionTable.Regions[region];
			return new SubStream(this._logicalStream, regionEntry.FileOffset, (long)((ulong)regionEntry.Length));
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003933 File Offset: 0x00001B33
		public override SparseStream OpenContent(SparseStream parent, Ownership ownsParent)
		{
			return this.DoOpenContent(parent, ownsParent);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000393D File Offset: 0x00001B3D
		public override string[] GetParentLocations()
		{
			return this.GetParentLocations(this._fileLocator);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000394B File Offset: 0x00001B4B
		[Obsolete("Use GetParentLocations() by preference")]
		public string[] GetParentLocations(string basePath)
		{
			return this.GetParentLocations(new LocalFileLocator(basePath));
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000395C File Offset: 0x00001B5C
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

		// Token: 0x06000077 RID: 119 RVA: 0x000039A8 File Offset: 0x00001BA8
		internal static DiskImageFile InitializeDynamic(FileLocator locator, string path, long capacity, long blockSize)
		{
			DiskImageFile result = null;
			Stream stream = locator.Open(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
			try
			{
				DiskImageFile.InitializeDynamicInternal(stream, capacity, blockSize);
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

		// Token: 0x06000078 RID: 120 RVA: 0x000039F4 File Offset: 0x00001BF4
		internal DiskImageFile CreateDifferencing(FileLocator fileLocator, string path)
		{
			Stream stream = fileLocator.Open(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
			string fullPath = this._fileLocator.GetFullPath(this._fileName);
			string parentRelativePath = fileLocator.MakeRelativePath(this._fileLocator, this._fileName);
			DateTime lastWriteTimeUtc = this._fileLocator.GetLastWriteTimeUtc(this._fileName);
			DiskImageFile.InitializeDifferencingInternal(stream, this, fullPath, parentRelativePath, lastWriteTimeUtc);
			return new DiskImageFile(fileLocator, path, stream, Ownership.Dispose);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003A58 File Offset: 0x00001C58
		internal MappedStream DoOpenContent(SparseStream parent, Ownership ownsParent)
		{
			SparseStream parentStream = parent;
			Ownership ownsParent2 = ownsParent;
			if (parent == null)
			{
				parentStream = new ZeroStream(this.Capacity);
				ownsParent2 = Ownership.Dispose;
			}
			return new AligningStream(new ContentStream(SparseStream.FromStream(this._logicalStream, Ownership.None), new bool?(this._fileStream.CanWrite), this._batStream, this._freeSpace, this._metadata, this.Capacity, parentStream, ownsParent2), Ownership.Dispose, (int)this._metadata.LogicalSectorSize);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003AC8 File Offset: 0x00001CC8
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					if (this._logicalStream != this._fileStream && this._logicalStream != null)
					{
						this._logicalStream.Dispose();
					}
					this._logicalStream = null;
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

		// Token: 0x0600007B RID: 123 RVA: 0x00003B40 File Offset: 0x00001D40
		private static void InitializeFixedInternal(Stream stream, long capacity, Geometry geometry)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003B48 File Offset: 0x00001D48
		private static void InitializeDynamicInternal(Stream stream, long capacity, long blockSize)
		{
			if (blockSize < 1048576L || blockSize > 268435456L || !Utilities.IsPowerOfTwo(blockSize))
			{
				throw new ArgumentOutOfRangeException("blockSize", blockSize, "BlockSize must be a power of 2 between 1MB and 256MB");
			}
			int num = 512;
			int physicalSectorSize = 4096;
			long num2 = 8388608L * (long)num / blockSize;
			long num3 = MathUtilities.Ceil(capacity, blockSize);
			MathUtilities.Ceil(num3, num2);
			long num4 = num3 + (num3 - 1L) / num2;
			FileHeader obj = new FileHeader
			{
				Creator = ".NET DiscUtils"
			};
			long num5 = 1048576L;
			VhdxHeader vhdxHeader = new VhdxHeader();
			vhdxHeader.SequenceNumber = 0UL;
			vhdxHeader.FileWriteGuid = Guid.NewGuid();
			vhdxHeader.DataWriteGuid = Guid.NewGuid();
			vhdxHeader.LogGuid = Guid.Empty;
			vhdxHeader.LogVersion = 0;
			vhdxHeader.Version = 1;
			vhdxHeader.LogLength = 1048576U;
			vhdxHeader.LogOffset = (ulong)num5;
			vhdxHeader.CalcChecksum();
			num5 += (long)((ulong)vhdxHeader.LogLength);
			VhdxHeader vhdxHeader2 = new VhdxHeader(vhdxHeader);
			vhdxHeader2.SequenceNumber = 1UL;
			vhdxHeader2.CalcChecksum();
			RegionTable regionTable = new RegionTable();
			RegionEntry regionEntry = new RegionEntry();
			regionEntry.Guid = RegionEntry.MetadataRegionGuid;
			regionEntry.FileOffset = num5;
			regionEntry.Length = 1048576U;
			regionEntry.Flags = RegionFlags.Required;
			regionTable.Regions.Add(regionEntry.Guid, regionEntry);
			num5 += (long)((ulong)regionEntry.Length);
			RegionEntry regionEntry2 = new RegionEntry();
			regionEntry2.Guid = RegionEntry.BatGuid;
			regionEntry2.FileOffset = 3145728L;
			regionEntry2.Length = (uint)MathUtilities.RoundUp(num4 * 8L, 1048576L);
			regionEntry2.Flags = RegionFlags.Required;
			regionTable.Regions.Add(regionEntry2.Guid, regionEntry2);
			num5 += (long)((ulong)regionEntry2.Length);
			stream.Position = 0L;
			StreamUtilities.WriteStruct<FileHeader>(stream, obj);
			stream.Position = 65536L;
			StreamUtilities.WriteStruct<VhdxHeader>(stream, vhdxHeader);
			stream.Position = 131072L;
			StreamUtilities.WriteStruct<VhdxHeader>(stream, vhdxHeader2);
			stream.Position = 196608L;
			StreamUtilities.WriteStruct<RegionTable>(stream, regionTable);
			stream.Position = 262144L;
			StreamUtilities.WriteStruct<RegionTable>(stream, regionTable);
			stream.Position = num5 - 1L;
			stream.WriteByte(0);
			FileParameters fileParameters = new FileParameters
			{
				BlockSize = (uint)blockSize,
				Flags = FileParametersFlags.None
			};
			new ParentLocator();
			Metadata.Initialize(new SubStream(stream, regionEntry.FileOffset, (long)((ulong)regionEntry.Length)), fileParameters, (ulong)capacity, (uint)num, (uint)physicalSectorSize, null);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003DBD File Offset: 0x00001FBD
		private static void InitializeDifferencingInternal(Stream stream, DiskImageFile parent, string parentAbsolutePath, string parentRelativePath, DateTime parentModificationTimeUtc)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003DC4 File Offset: 0x00001FC4
		private void Initialize()
		{
			this._fileStream.Position = 0L;
			if (!StreamUtilities.ReadStruct<FileHeader>(this._fileStream).IsValid)
			{
				throw new IOException("Invalid VHDX file - file signature mismatch");
			}
			this._freeSpace = new FreeSpaceTable(this._fileStream.Length);
			this.ReadHeaders();
			this.ReplayLog();
			this.ReadRegionTable();
			this.ReadMetadata();
			this._batStream = this.OpenRegion(RegionTable.BatGuid);
			this._freeSpace.Reserve(this.BatControlledFileExtents());
			if (this._fileStream.CanWrite)
			{
				this._header.FileWriteGuid = Guid.NewGuid();
				this.WriteHeader();
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003E70 File Offset: 0x00002070
		private IEnumerable<StreamExtent> BatControlledFileExtents()
		{
			this._batStream.Position = 0L;
			byte[] array = StreamUtilities.ReadExact(this._batStream, (int)this._batStream.Length);
			uint blockSize = this._metadata.FileParameters.BlockSize;
			int num = (int)(8388608UL * (ulong)this._metadata.LogicalSectorSize / (ulong)this._metadata.FileParameters.BlockSize);
			List<StreamExtent> list = new List<StreamExtent>();
			for (int i = 0; i < array.Length; i += 8)
			{
				long num2 = (long)((EndianUtilities.ToUInt64LittleEndian(array, i) >> 20 & 17592186044415UL) * 1048576UL);
				if (num2 != 0L)
				{
					if (i % ((num + 1) * 8) == num * 8)
					{
						list.Add(new StreamExtent(num2, 1048576L));
					}
					else
					{
						list.Add(new StreamExtent(num2, (long)((ulong)blockSize)));
					}
				}
			}
			list.Sort();
			return list;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003F4C File Offset: 0x0000214C
		private void ReadMetadata()
		{
			Stream regionStream = this.OpenRegion(RegionTable.MetadataRegionGuid);
			this._metadata = new Metadata(regionStream);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003F74 File Offset: 0x00002174
		private void ReplayLog()
		{
			this._freeSpace.Reserve((long)this._header.LogOffset, (long)((ulong)this._header.LogLength));
			this._logicalStream = this._fileStream;
			if (this._header.LogGuid == Guid.Empty)
			{
				return;
			}
			LogSequence logSequence = this.FindActiveLogSequence();
			if (logSequence == null || logSequence.Count == 0)
			{
				throw new IOException("Unable to replay VHDX log, suspected corrupt VHDX file");
			}
			if (logSequence.Head.FlushedFileOffset > (ulong)this._logicalStream.Length)
			{
				throw new IOException("truncated VHDX file found while replaying log");
			}
			if (logSequence.Count > 1 || !logSequence.Head.IsEmpty)
			{
				if (!this._fileStream.CanWrite)
				{
					SnapshotStream snapshotStream = new SnapshotStream(this._fileStream, Ownership.None);
					snapshotStream.Snapshot();
					this._logicalStream = snapshotStream;
				}
				foreach (LogEntry logEntry in logSequence)
				{
					if (logEntry.LogGuid != this._header.LogGuid)
					{
						throw new IOException("Invalid log entry in VHDX log, suspected currupt VHDX file");
					}
					if (!logEntry.IsEmpty)
					{
						logEntry.Replay(this._logicalStream);
					}
				}
				this._logicalStream.Seek((long)logSequence.Head.LastFileOffset, SeekOrigin.Begin);
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000040D0 File Offset: 0x000022D0
		private LogSequence FindActiveLogSequence()
		{
			LogSequence result;
			using (Stream stream = new CircularStream(new SubStream(this._fileStream, (long)this._header.LogOffset, (long)((ulong)this._header.LogLength)), Ownership.Dispose))
			{
				LogSequence logSequence = new LogSequence();
				LogEntry logEntry = null;
				long num = 0L;
				long num2;
				do
				{
					num2 = num;
					stream.Position = num;
					LogSequence logSequence2 = new LogSequence();
					while (LogEntry.TryRead(stream, out logEntry) && logEntry.LogGuid == this._header.LogGuid && (logSequence2.Count == 0 || logEntry.SequenceNumber == logSequence2.Head.SequenceNumber + 1UL))
					{
						logSequence2.Add(logEntry);
						logEntry = null;
					}
					if (logSequence2.Count > 0 && logSequence2.Contains((long)((ulong)logSequence2.Head.Tail)) && logSequence2.HigherSequenceThan(logSequence))
					{
						logSequence = logSequence2;
					}
					if (logSequence2.Count == 0)
					{
						num += 4096L;
					}
					else
					{
						num = logSequence2.Head.Position + 4096L;
					}
					num %= stream.Length;
				}
				while (num > num2);
				result = logSequence;
			}
			return result;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004200 File Offset: 0x00002400
		private void ReadRegionTable()
		{
			this._fileStream.Position = 196608L;
			this._regionTable = StreamUtilities.ReadStruct<RegionTable>(this._fileStream);
			foreach (RegionEntry regionEntry in this._regionTable.Regions.Values)
			{
				if ((regionEntry.Flags & RegionFlags.Required) != RegionFlags.None && regionEntry.Guid != RegionTable.BatGuid && regionEntry.Guid != RegionTable.MetadataRegionGuid)
				{
					throw new IOException("Invalid VHDX file - unrecognised required region");
				}
				this._freeSpace.Reserve(regionEntry.FileOffset, (long)((ulong)regionEntry.Length));
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000042C4 File Offset: 0x000024C4
		private void ReadHeaders()
		{
			this._freeSpace.Reserve(0L, 1048576L);
			this._activeHeader = 0;
			this._fileStream.Position = 65536L;
			VhdxHeader vhdxHeader = StreamUtilities.ReadStruct<VhdxHeader>(this._fileStream);
			if (vhdxHeader.IsValid)
			{
				this._header = vhdxHeader;
				this._activeHeader = 1;
			}
			this._fileStream.Position = 131072L;
			VhdxHeader vhdxHeader2 = StreamUtilities.ReadStruct<VhdxHeader>(this._fileStream);
			if (vhdxHeader2.IsValid && (this._activeHeader == 0 || this._header.SequenceNumber < vhdxHeader2.SequenceNumber))
			{
				this._header = vhdxHeader2;
				this._activeHeader = 2;
			}
			if (this._activeHeader == 0)
			{
				throw new IOException("Invalid VHDX file - no valid VHDX headers found");
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004380 File Offset: 0x00002580
		private void WriteHeader()
		{
			this._header.SequenceNumber += 1UL;
			this._header.CalcChecksum();
			long position;
			if (this._activeHeader == 1)
			{
				this._fileStream.Position = 131072L;
				position = 65536L;
			}
			else
			{
				this._fileStream.Position = 65536L;
				position = 131072L;
			}
			StreamUtilities.WriteStruct<VhdxHeader>(this._fileStream, this._header);
			this._fileStream.Flush();
			this._header.SequenceNumber += 1UL;
			this._header.CalcChecksum();
			this._fileStream.Position = position;
			StreamUtilities.WriteStruct<VhdxHeader>(this._fileStream, this._header);
			this._fileStream.Flush();
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000444C File Offset: 0x0000264C
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
			List<string> list = new List<string>();
			ParentLocator parentLocator = this._metadata.ParentLocator;
			string text;
			if (parentLocator.Entries.TryGetValue("relative_path", out text))
			{
				list.Add(fileLocator.ResolveRelativePath(text));
			}
			if (parentLocator.Entries.TryGetValue("volume_path", out text))
			{
				list.Add(text);
			}
			if (parentLocator.Entries.TryGetValue("absolute_win32_path", out text))
			{
				list.Add(text);
			}
			return list.ToArray();
		}

		// Token: 0x0400001F RID: 31
		private int _activeHeader;

		// Token: 0x04000020 RID: 32
		private Stream _batStream;

		// Token: 0x04000021 RID: 33
		private readonly FileLocator _fileLocator;

		// Token: 0x04000022 RID: 34
		private readonly string _fileName;

		// Token: 0x04000023 RID: 35
		private Stream _fileStream;

		// Token: 0x04000024 RID: 36
		private FreeSpaceTable _freeSpace;

		// Token: 0x04000025 RID: 37
		private VhdxHeader _header;

		// Token: 0x04000026 RID: 38
		private Stream _logicalStream;

		// Token: 0x04000027 RID: 39
		private Metadata _metadata;

		// Token: 0x04000028 RID: 40
		private readonly Ownership _ownsStream;

		// Token: 0x04000029 RID: 41
		private RegionTable _regionTable;
	}
}
