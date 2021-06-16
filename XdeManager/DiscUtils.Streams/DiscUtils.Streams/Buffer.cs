using System;
using System.Collections.Generic;

namespace DiscUtils.Streams
{
	// Token: 0x02000008 RID: 8
	public abstract class Buffer : MarshalByRefObject, IBuffer
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600004A RID: 74
		public abstract bool CanRead { get; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600004B RID: 75
		public abstract bool CanWrite { get; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600004C RID: 76
		public abstract long Capacity { get; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002F3D File Offset: 0x0000113D
		public virtual IEnumerable<StreamExtent> Extents
		{
			get
			{
				return this.GetExtentsInRange(0L, this.Capacity);
			}
		}

		// Token: 0x0600004E RID: 78
		public abstract int Read(long pos, byte[] buffer, int offset, int count);

		// Token: 0x0600004F RID: 79
		public abstract void Write(long pos, byte[] buffer, int offset, int count);

		// Token: 0x06000050 RID: 80 RVA: 0x00002F4D File Offset: 0x0000114D
		public virtual void Clear(long pos, int count)
		{
			this.Write(pos, new byte[count], 0, count);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002F5E File Offset: 0x0000115E
		public virtual void Flush()
		{
		}

		// Token: 0x06000052 RID: 82
		public abstract void SetCapacity(long value);

		// Token: 0x06000053 RID: 83
		public abstract IEnumerable<StreamExtent> GetExtentsInRange(long start, long count);
	}
}
