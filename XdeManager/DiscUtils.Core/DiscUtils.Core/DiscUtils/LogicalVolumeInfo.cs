using System;
using DiscUtils.Streams;

namespace DiscUtils
{
	// Token: 0x0200001F RID: 31
	public sealed class LogicalVolumeInfo : VolumeInfo
	{
		// Token: 0x06000136 RID: 310 RVA: 0x000034F5 File Offset: 0x000016F5
		internal LogicalVolumeInfo(Guid guid, PhysicalVolumeInfo physicalVolume, SparseStreamOpenDelegate opener, long length, byte biosType, LogicalVolumeStatus status)
		{
			this._guid = guid;
			this._physicalVol = physicalVolume;
			this._opener = opener;
			this.Length = length;
			this.BiosType = biosType;
			this.Status = status;
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000137 RID: 311 RVA: 0x0000352A File Offset: 0x0000172A
		public override Geometry BiosGeometry
		{
			get
			{
				if (this._physicalVol != null)
				{
					return this._physicalVol.BiosGeometry;
				}
				return Geometry.Null;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00003545 File Offset: 0x00001745
		public override byte BiosType { get; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00003550 File Offset: 0x00001750
		public override string Identity
		{
			get
			{
				if (this._guid != Guid.Empty)
				{
					return "VLG" + this._guid.ToString("B");
				}
				return "VLP:" + this._physicalVol.Identity;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600013A RID: 314 RVA: 0x0000359F File Offset: 0x0000179F
		public override long Length { get; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600013B RID: 315 RVA: 0x000035A7 File Offset: 0x000017A7
		public override Geometry PhysicalGeometry
		{
			get
			{
				if (this._physicalVol != null)
				{
					return this._physicalVol.PhysicalGeometry;
				}
				return Geometry.Null;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600013C RID: 316 RVA: 0x000035C2 File Offset: 0x000017C2
		public override long PhysicalStartSector
		{
			get
			{
				if (this._physicalVol != null)
				{
					return this._physicalVol.PhysicalStartSector;
				}
				return 0L;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600013D RID: 317 RVA: 0x000035DA File Offset: 0x000017DA
		public LogicalVolumeStatus Status { get; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600013E RID: 318 RVA: 0x000035E2 File Offset: 0x000017E2
		public PhysicalVolumeInfo PhysicalVolume
		{
			get
			{
				return this._physicalVol;
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000035EA File Offset: 0x000017EA
		public override SparseStream Open()
		{
			return this._opener();
		}

		// Token: 0x04000033 RID: 51
		private Guid _guid;

		// Token: 0x04000034 RID: 52
		private readonly SparseStreamOpenDelegate _opener;

		// Token: 0x04000035 RID: 53
		private readonly PhysicalVolumeInfo _physicalVol;
	}
}
