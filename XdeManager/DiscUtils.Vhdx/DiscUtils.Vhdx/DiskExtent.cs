using System;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x02000008 RID: 8
	internal sealed class DiskExtent : VirtualDiskExtent
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00003466 File Offset: 0x00001666
		public DiskExtent(DiskImageFile file)
		{
			this._file = file;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00003475 File Offset: 0x00001675
		public override long Capacity
		{
			get
			{
				return this._file.Capacity;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00003482 File Offset: 0x00001682
		public override bool IsSparse
		{
			get
			{
				return this._file.IsSparse;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000051 RID: 81 RVA: 0x0000348F File Offset: 0x0000168F
		public override long StoredSize
		{
			get
			{
				return this._file.StoredSize;
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x0000349C File Offset: 0x0000169C
		public override MappedStream OpenContent(SparseStream parent, Ownership ownsParent)
		{
			return this._file.DoOpenContent(parent, ownsParent);
		}

		// Token: 0x0400001E RID: 30
		private readonly DiskImageFile _file;
	}
}
