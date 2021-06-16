using System;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.LogicalDiskManager
{
	// Token: 0x02000064 RID: 100
	internal class DynamicVolume
	{
		// Token: 0x060003F2 RID: 1010 RVA: 0x0000BBA0 File Offset: 0x00009DA0
		internal DynamicVolume(DynamicDiskGroup group, Guid volumeId)
		{
			this._group = group;
			this.Identity = volumeId;
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x0000BBB6 File Offset: 0x00009DB6
		public byte BiosType
		{
			get
			{
				return this.Record.BiosType;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0000BBC3 File Offset: 0x00009DC3
		public Guid Identity { get; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x0000BBCB File Offset: 0x00009DCB
		public long Length
		{
			get
			{
				return this.Record.Size * 512L;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x0000BBDF File Offset: 0x00009DDF
		private VolumeRecord Record
		{
			get
			{
				return this._group.GetVolume(this.Identity);
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x0000BBF2 File Offset: 0x00009DF2
		public LogicalVolumeStatus Status
		{
			get
			{
				return this._group.GetVolumeStatus(this.Record.Id);
			}
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000BC0A File Offset: 0x00009E0A
		public SparseStream Open()
		{
			if (this.Status == LogicalVolumeStatus.Failed)
			{
				throw new IOException("Attempt to open 'failed' volume");
			}
			return this._group.OpenVolume(this.Record.Id);
		}

		// Token: 0x0400012E RID: 302
		private readonly DynamicDiskGroup _group;
	}
}
