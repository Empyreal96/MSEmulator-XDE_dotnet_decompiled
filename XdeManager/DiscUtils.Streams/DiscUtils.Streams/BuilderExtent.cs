using System;
using System.Collections.Generic;

namespace DiscUtils.Streams
{
	// Token: 0x02000010 RID: 16
	public abstract class BuilderExtent : IDisposable
	{
		// Token: 0x06000089 RID: 137 RVA: 0x000033D5 File Offset: 0x000015D5
		public BuilderExtent(long start, long length)
		{
			this.Start = start;
			this.Length = length;
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600008A RID: 138 RVA: 0x000033EB File Offset: 0x000015EB
		public long Length { get; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600008B RID: 139 RVA: 0x000033F3 File Offset: 0x000015F3
		public long Start { get; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600008C RID: 140 RVA: 0x000033FB File Offset: 0x000015FB
		public virtual IEnumerable<StreamExtent> StreamExtents
		{
			get
			{
				return new StreamExtent[]
				{
					new StreamExtent(this.Start, this.Length)
				};
			}
		}

		// Token: 0x0600008D RID: 141
		public abstract void Dispose();

		// Token: 0x0600008E RID: 142
		public abstract void PrepareForRead();

		// Token: 0x0600008F RID: 143
		public abstract int Read(long diskOffset, byte[] block, int offset, int count);

		// Token: 0x06000090 RID: 144
		public abstract void DisposeReadState();
	}
}
