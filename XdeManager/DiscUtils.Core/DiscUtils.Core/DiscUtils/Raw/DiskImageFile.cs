using System;
using System.IO;
using DiscUtils.Partitions;
using DiscUtils.Streams;

namespace DiscUtils.Raw
{
	// Token: 0x02000048 RID: 72
	public sealed class DiskImageFile : VirtualDiskLayer
	{
		// Token: 0x060002F3 RID: 755 RVA: 0x00006716 File Offset: 0x00004916
		public DiskImageFile(Stream stream) : this(stream, Ownership.None, null)
		{
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00006724 File Offset: 0x00004924
		public DiskImageFile(Stream stream, Ownership ownsStream, Geometry geometry)
		{
			this.Content = (stream as SparseStream);
			this._ownsContent = ownsStream;
			if (this.Content == null)
			{
				this.Content = SparseStream.FromStream(stream, ownsStream);
				this._ownsContent = Ownership.Dispose;
			}
			this.Geometry = (geometry ?? DiskImageFile.DetectGeometry(this.Content));
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x0000677C File Offset: 0x0000497C
		internal override long Capacity
		{
			get
			{
				return this.Content.Length;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x00006789 File Offset: 0x00004989
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x00006791 File Offset: 0x00004991
		internal SparseStream Content { get; private set; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x0000679A File Offset: 0x0000499A
		public VirtualDiskClass DiskType
		{
			get
			{
				return DiskImageFile.DetectDiskType(this.Capacity);
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x000067A7 File Offset: 0x000049A7
		public override Geometry Geometry { get; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060002FA RID: 762 RVA: 0x000067AF File Offset: 0x000049AF
		public override bool IsSparse
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060002FB RID: 763 RVA: 0x000067B2 File Offset: 0x000049B2
		public override bool NeedsParent
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060002FC RID: 764 RVA: 0x000067B5 File Offset: 0x000049B5
		internal override FileLocator RelativeFileLocator
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060002FD RID: 765 RVA: 0x000067B8 File Offset: 0x000049B8
		public static DiskImageFile Initialize(Stream stream, Ownership ownsStream, long capacity, Geometry geometry)
		{
			stream.SetLength(MathUtilities.RoundUp(capacity, 512L));
			stream.Position = 0L;
			stream.Write(new byte[512], 0, 512);
			stream.Position = 0L;
			return new DiskImageFile(stream, ownsStream, geometry);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00006805 File Offset: 0x00004A05
		public static DiskImageFile Initialize(Stream stream, Ownership ownsStream, FloppyDiskType type)
		{
			return DiskImageFile.Initialize(stream, ownsStream, DiskImageFile.FloppyCapacity(type), null);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00006815 File Offset: 0x00004A15
		public override SparseStream OpenContent(SparseStream parent, Ownership ownsParent)
		{
			if (ownsParent == Ownership.Dispose && parent != null)
			{
				parent.Dispose();
			}
			return SparseStream.FromStream(this.Content, Ownership.None);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00006830 File Offset: 0x00004A30
		public override string[] GetParentLocations()
		{
			return new string[0];
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00006838 File Offset: 0x00004A38
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					if (this._ownsContent == Ownership.Dispose && this.Content != null)
					{
						this.Content.Dispose();
					}
					this.Content = null;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00006888 File Offset: 0x00004A88
		private static Geometry DetectGeometry(Stream disk)
		{
			long length = disk.Length;
			if (length == 737280L)
			{
				return new Geometry(80, 2, 9);
			}
			if (length == 1474560L)
			{
				return new Geometry(80, 2, 18);
			}
			if (length == 2949120L)
			{
				return new Geometry(80, 2, 36);
			}
			return BiosPartitionTable.DetectGeometry(disk);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x000068DE File Offset: 0x00004ADE
		private static VirtualDiskClass DetectDiskType(long capacity)
		{
			if (capacity == 737280L || capacity == 1474560L || capacity == 2949120L)
			{
				return VirtualDiskClass.FloppyDisk;
			}
			return VirtualDiskClass.HardDisk;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x000068FE File Offset: 0x00004AFE
		private static long FloppyCapacity(FloppyDiskType type)
		{
			switch (type)
			{
			case FloppyDiskType.DoubleDensity:
				return 737280L;
			case FloppyDiskType.HighDensity:
				return 1474560L;
			case FloppyDiskType.Extended:
				return 2949120L;
			default:
				throw new ArgumentException("Invalid floppy disk type", "type");
			}
		}

		// Token: 0x040000A3 RID: 163
		private readonly Ownership _ownsContent;
	}
}
