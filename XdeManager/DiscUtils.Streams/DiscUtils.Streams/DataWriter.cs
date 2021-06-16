using System;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x02000023 RID: 35
	public abstract class DataWriter
	{
		// Token: 0x0600010C RID: 268 RVA: 0x000044E6 File Offset: 0x000026E6
		public DataWriter(Stream stream)
		{
			this._stream = stream;
		}

		// Token: 0x0600010D RID: 269
		public abstract void Write(ushort value);

		// Token: 0x0600010E RID: 270
		public abstract void Write(int value);

		// Token: 0x0600010F RID: 271
		public abstract void Write(uint value);

		// Token: 0x06000110 RID: 272
		public abstract void Write(long value);

		// Token: 0x06000111 RID: 273
		public abstract void Write(ulong value);

		// Token: 0x06000112 RID: 274 RVA: 0x000044F5 File Offset: 0x000026F5
		public virtual void WriteBytes(byte[] value, int offset, int count)
		{
			this._stream.Write(value, offset, count);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00004505 File Offset: 0x00002705
		public virtual void WriteBytes(byte[] value)
		{
			this._stream.Write(value, 0, value.Length);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00004517 File Offset: 0x00002717
		public virtual void Flush()
		{
			this._stream.Flush();
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00004524 File Offset: 0x00002724
		protected void EnsureBuffer()
		{
			if (this._buffer == null)
			{
				this._buffer = new byte[8];
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000453A File Offset: 0x0000273A
		protected void FlushBuffer(int count)
		{
			this._stream.Write(this._buffer, 0, count);
		}

		// Token: 0x0400004E RID: 78
		private const int _bufferSize = 8;

		// Token: 0x0400004F RID: 79
		protected readonly Stream _stream;

		// Token: 0x04000050 RID: 80
		protected byte[] _buffer;
	}
}
