using System;
using DiscUtils.Streams;

namespace DiscUtils
{
	// Token: 0x0200002E RID: 46
	public abstract class VirtualDiskExtent : IDisposable
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001D5 RID: 469
		public abstract long Capacity { get; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001D6 RID: 470
		public abstract bool IsSparse { get; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001D7 RID: 471
		public abstract long StoredSize { get; }

		// Token: 0x060001D8 RID: 472 RVA: 0x00004CF1 File Offset: 0x00002EF1
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060001D9 RID: 473
		public abstract MappedStream OpenContent(SparseStream parent, Ownership ownsParent);

		// Token: 0x060001DA RID: 474 RVA: 0x00004D00 File Offset: 0x00002F00
		protected virtual void Dispose(bool disposing)
		{
		}
	}
}
