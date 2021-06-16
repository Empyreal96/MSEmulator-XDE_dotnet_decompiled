using System;
using System.Globalization;
using DiscUtils.Partitions;
using DiscUtils.Streams;

namespace DiscUtils
{
	// Token: 0x02000022 RID: 34
	public sealed class PhysicalVolumeInfo : VolumeInfo
	{
		// Token: 0x06000173 RID: 371 RVA: 0x00003E29 File Offset: 0x00002029
		internal PhysicalVolumeInfo(string diskId, VirtualDisk disk, PartitionInfo partitionInfo)
		{
			this._diskId = diskId;
			this._disk = disk;
			this._streamOpener = new SparseStreamOpenDelegate(partitionInfo.Open);
			this.VolumeType = partitionInfo.VolumeType;
			this.Partition = partitionInfo;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00003E68 File Offset: 0x00002068
		internal PhysicalVolumeInfo(string diskId, VirtualDisk disk)
		{
			this._diskId = diskId;
			this._disk = disk;
			this._streamOpener = (() => new SubStream(disk.Content, Ownership.None, 0L, disk.Capacity));
			this.VolumeType = 1;
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00003EB4 File Offset: 0x000020B4
		public override Geometry BiosGeometry
		{
			get
			{
				return this._disk.BiosGeometry;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00003EC1 File Offset: 0x000020C1
		public override byte BiosType
		{
			get
			{
				if (this.Partition != null)
				{
					return this.Partition.BiosType;
				}
				return 0;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00003ED8 File Offset: 0x000020D8
		public Guid DiskIdentity
		{
			get
			{
				if (this.VolumeType == PhysicalVolumeType.EntireDisk)
				{
					return Guid.Empty;
				}
				return this._disk.Partitions.DiskGuid;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00003EF9 File Offset: 0x000020F9
		public int DiskSignature
		{
			get
			{
				if (this.VolumeType == PhysicalVolumeType.EntireDisk)
				{
					return 0;
				}
				return this._disk.Signature;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00003F14 File Offset: 0x00002114
		public override string Identity
		{
			get
			{
				if (this.VolumeType == PhysicalVolumeType.GptPartition)
				{
					return "VPG" + this.PartitionIdentity.ToString("B");
				}
				string str;
				switch (this.VolumeType)
				{
				case PhysicalVolumeType.EntireDisk:
					str = "PD";
					goto IL_8D;
				case PhysicalVolumeType.BiosPartition:
				case PhysicalVolumeType.ApplePartition:
					str = "PO" + (this.Partition.FirstSector * (long)this._disk.SectorSize).ToString("X", CultureInfo.InvariantCulture);
					goto IL_8D;
				}
				str = "P*";
				IL_8D:
				return "VPD:" + this._diskId + ":" + str;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00003FC4 File Offset: 0x000021C4
		public override long Length
		{
			get
			{
				if (this.Partition != null)
				{
					return this.Partition.SectorCount * (long)this._disk.SectorSize;
				}
				return this._disk.Capacity;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00003FF2 File Offset: 0x000021F2
		public PartitionInfo Partition { get; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00003FFC File Offset: 0x000021FC
		public Guid PartitionIdentity
		{
			get
			{
				GuidPartitionInfo guidPartitionInfo = this.Partition as GuidPartitionInfo;
				if (guidPartitionInfo != null)
				{
					return guidPartitionInfo.Identity;
				}
				return Guid.Empty;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00004024 File Offset: 0x00002224
		public override Geometry PhysicalGeometry
		{
			get
			{
				return this._disk.Geometry;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00004031 File Offset: 0x00002231
		public override long PhysicalStartSector
		{
			get
			{
				if (this.VolumeType != PhysicalVolumeType.EntireDisk)
				{
					return this.Partition.FirstSector;
				}
				return 0L;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600017F RID: 383 RVA: 0x0000404A File Offset: 0x0000224A
		public PhysicalVolumeType VolumeType { get; }

		// Token: 0x06000180 RID: 384 RVA: 0x00004052 File Offset: 0x00002252
		public override SparseStream Open()
		{
			return this._streamOpener();
		}

		// Token: 0x0400003F RID: 63
		private readonly VirtualDisk _disk;

		// Token: 0x04000040 RID: 64
		private readonly string _diskId;

		// Token: 0x04000041 RID: 65
		private readonly SparseStreamOpenDelegate _streamOpener;
	}
}
