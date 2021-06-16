using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils.Raw
{
	// Token: 0x02000046 RID: 70
	public sealed class Disk : VirtualDisk
	{
		// Token: 0x060002D9 RID: 729 RVA: 0x000064F2 File Offset: 0x000046F2
		public Disk(Stream stream, Ownership ownsStream) : this(stream, ownsStream, null)
		{
		}

		// Token: 0x060002DA RID: 730 RVA: 0x000064FD File Offset: 0x000046FD
		public Disk(Stream stream, Ownership ownsStream, Geometry geometry)
		{
			this._file = new DiskImageFile(stream, ownsStream, geometry);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00006513 File Offset: 0x00004713
		public Disk(string path) : this(path, FileAccess.ReadWrite)
		{
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00006520 File Offset: 0x00004720
		public Disk(string path, FileAccess access)
		{
			FileShare share = (access == FileAccess.Read) ? FileShare.Read : FileShare.None;
			LocalFileLocator localFileLocator = new LocalFileLocator(string.Empty);
			this._file = new DiskImageFile(localFileLocator.Open(path, FileMode.Open, access, share), Ownership.Dispose, null);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000655E File Offset: 0x0000475E
		private Disk(DiskImageFile file)
		{
			this._file = file;
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0000656D File Offset: 0x0000476D
		public override long Capacity
		{
			get
			{
				return this._file.Capacity;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0000657A File Offset: 0x0000477A
		public override SparseStream Content
		{
			get
			{
				return this._file.Content;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x00006587 File Offset: 0x00004787
		public override VirtualDiskClass DiskClass
		{
			get
			{
				return this._file.DiskType;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x00006594 File Offset: 0x00004794
		public override VirtualDiskTypeInfo DiskTypeInfo
		{
			get
			{
				return DiskFactory.MakeDiskTypeInfo();
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000659B File Offset: 0x0000479B
		public override Geometry Geometry
		{
			get
			{
				return this._file.Geometry;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x000065A8 File Offset: 0x000047A8
		public override IEnumerable<VirtualDiskLayer> Layers
		{
			get
			{
				yield return this._file;
				yield break;
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x000065B8 File Offset: 0x000047B8
		public static Disk Initialize(Stream stream, Ownership ownsStream, long capacity)
		{
			return Disk.Initialize(stream, ownsStream, capacity, null);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x000065C3 File Offset: 0x000047C3
		public static Disk Initialize(Stream stream, Ownership ownsStream, long capacity, Geometry geometry)
		{
			return new Disk(DiskImageFile.Initialize(stream, ownsStream, capacity, geometry));
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x000065D3 File Offset: 0x000047D3
		public static Disk Initialize(Stream stream, Ownership ownsStream, FloppyDiskType type)
		{
			return new Disk(DiskImageFile.Initialize(stream, ownsStream, type));
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x000065E2 File Offset: 0x000047E2
		public override VirtualDisk CreateDifferencingDisk(DiscFileSystem fileSystem, string path)
		{
			throw new NotSupportedException("Differencing disks not supported for raw disks");
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x000065EE File Offset: 0x000047EE
		public override VirtualDisk CreateDifferencingDisk(string path)
		{
			throw new NotSupportedException("Differencing disks not supported for raw disks");
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x000065FC File Offset: 0x000047FC
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					if (this._file != null)
					{
						this._file.Dispose();
					}
					this._file = null;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x040000A2 RID: 162
		private DiskImageFile _file;
	}
}
