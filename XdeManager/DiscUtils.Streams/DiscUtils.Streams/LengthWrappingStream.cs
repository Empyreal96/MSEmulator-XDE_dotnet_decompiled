using System;

namespace DiscUtils.Streams
{
	// Token: 0x0200001B RID: 27
	public class LengthWrappingStream : WrappingStream
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x00003E6D File Offset: 0x0000206D
		public LengthWrappingStream(SparseStream toWrap, long length, Ownership ownership) : base(toWrap, ownership)
		{
			this._length = length;
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00003E7E File Offset: 0x0000207E
		public override long Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x0400003F RID: 63
		private readonly long _length;
	}
}
