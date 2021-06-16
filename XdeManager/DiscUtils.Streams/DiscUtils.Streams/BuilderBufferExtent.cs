using System;

namespace DiscUtils.Streams
{
	// Token: 0x0200000D RID: 13
	public class BuilderBufferExtent : BuilderExtent
	{
		// Token: 0x0600007A RID: 122 RVA: 0x000032B4 File Offset: 0x000014B4
		public BuilderBufferExtent(long start, long length) : base(start, length)
		{
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000032BE File Offset: 0x000014BE
		public BuilderBufferExtent(long start, byte[] buffer) : base(start, (long)buffer.Length)
		{
			this._fixedBuffer = true;
			this._buffer = buffer;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000032D9 File Offset: 0x000014D9
		public override void Dispose()
		{
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000032DB File Offset: 0x000014DB
		public override void PrepareForRead()
		{
			if (!this._fixedBuffer)
			{
				this._buffer = this.GetBuffer();
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000032F4 File Offset: 0x000014F4
		public override int Read(long diskOffset, byte[] block, int offset, int count)
		{
			int num = (int)(diskOffset - base.Start);
			int num2 = (int)Math.Min(base.Length - (long)num, (long)count);
			Array.Copy(this._buffer, num, block, offset, num2);
			return num2;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000332E File Offset: 0x0000152E
		public override void DisposeReadState()
		{
			if (!this._fixedBuffer)
			{
				this._buffer = null;
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000333F File Offset: 0x0000153F
		protected virtual byte[] GetBuffer()
		{
			throw new NotSupportedException("Derived class should implement");
		}

		// Token: 0x0400002A RID: 42
		private byte[] _buffer;

		// Token: 0x0400002B RID: 43
		private readonly bool _fixedBuffer;
	}
}
