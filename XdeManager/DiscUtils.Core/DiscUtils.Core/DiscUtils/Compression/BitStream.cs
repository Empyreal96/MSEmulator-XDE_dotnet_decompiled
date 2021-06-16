using System;

namespace DiscUtils.Compression
{
	// Token: 0x0200007C RID: 124
	internal abstract class BitStream
	{
		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000468 RID: 1128
		public abstract int MaxReadAhead { get; }

		// Token: 0x06000469 RID: 1129
		public abstract uint Read(int count);

		// Token: 0x0600046A RID: 1130
		public abstract uint Peek(int count);

		// Token: 0x0600046B RID: 1131
		public abstract void Consume(int count);
	}
}
