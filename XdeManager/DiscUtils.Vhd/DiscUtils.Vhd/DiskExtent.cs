using System;
using DiscUtils.Streams;

namespace DiscUtils.Vhd
{
	// Token: 0x02000004 RID: 4
	internal sealed class DiskExtent : VirtualDiskExtent
	{
		// Token: 0x06000023 RID: 35 RVA: 0x00002948 File Offset: 0x00000B48
		public DiskExtent(DiskImageFile file)
		{
			this._file = file;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002957 File Offset: 0x00000B57
		public override long Capacity
		{
			get
			{
				return this._file.Capacity;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002964 File Offset: 0x00000B64
		public override bool IsSparse
		{
			get
			{
				return this._file.IsSparse;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002971 File Offset: 0x00000B71
		public override long StoredSize
		{
			get
			{
				return this._file.StoredSize;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x0000297E File Offset: 0x00000B7E
		public override MappedStream OpenContent(SparseStream parent, Ownership ownsParent)
		{
			return this._file.DoOpenContent(parent, ownsParent);
		}

		// Token: 0x04000004 RID: 4
		private readonly DiskImageFile _file;
	}
}
