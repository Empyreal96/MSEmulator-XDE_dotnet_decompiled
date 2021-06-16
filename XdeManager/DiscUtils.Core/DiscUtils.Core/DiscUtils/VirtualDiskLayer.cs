using System;
using System.Collections.Generic;
using DiscUtils.Streams;

namespace DiscUtils
{
	// Token: 0x0200002F RID: 47
	public abstract class VirtualDiskLayer : IDisposable
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001DC RID: 476
		internal abstract long Capacity { get; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001DD RID: 477 RVA: 0x00004D0A File Offset: 0x00002F0A
		public virtual IList<VirtualDiskExtent> Extents
		{
			get
			{
				return new List<VirtualDiskExtent>();
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001DE RID: 478 RVA: 0x00004D11 File Offset: 0x00002F11
		public virtual string FullPath
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001DF RID: 479
		public abstract Geometry Geometry { get; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001E0 RID: 480
		public abstract bool IsSparse { get; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001E1 RID: 481
		public abstract bool NeedsParent { get; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001E2 RID: 482
		internal abstract FileLocator RelativeFileLocator { get; }

		// Token: 0x060001E3 RID: 483 RVA: 0x00004D18 File Offset: 0x00002F18
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00004D28 File Offset: 0x00002F28
		~VirtualDiskLayer()
		{
			this.Dispose(false);
		}

		// Token: 0x060001E5 RID: 485
		public abstract SparseStream OpenContent(SparseStream parent, Ownership ownsParent);

		// Token: 0x060001E6 RID: 486
		public abstract string[] GetParentLocations();

		// Token: 0x060001E7 RID: 487 RVA: 0x00004D58 File Offset: 0x00002F58
		protected virtual void Dispose(bool disposing)
		{
		}
	}
}
