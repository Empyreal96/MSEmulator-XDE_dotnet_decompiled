using System;
using System.IO;

namespace DiscUtils.Compression
{
	// Token: 0x0200007B RID: 123
	internal class BigEndianBitStream : BitStream
	{
		// Token: 0x06000462 RID: 1122 RVA: 0x0000CFC0 File Offset: 0x0000B1C0
		public BigEndianBitStream(Stream byteStream)
		{
			this._byteStream = byteStream;
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x0000CFDB File Offset: 0x0000B1DB
		public override int MaxReadAhead
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0000CFE0 File Offset: 0x0000B1E0
		public override uint Read(int count)
		{
			if (count > 16)
			{
				return this.Read(16) << count - 16 | this.Read(count - 16);
			}
			this.EnsureBufferFilled();
			this._bufferAvailable -= count;
			uint num = (1U << count) - 1U;
			return this._buffer >> this._bufferAvailable & num;
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0000D040 File Offset: 0x0000B240
		public override uint Peek(int count)
		{
			this.EnsureBufferFilled();
			uint num = (1U << count) - 1U;
			return this._buffer >> this._bufferAvailable - count & num;
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0000D070 File Offset: 0x0000B270
		public override void Consume(int count)
		{
			this.EnsureBufferFilled();
			this._bufferAvailable -= count;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0000D088 File Offset: 0x0000B288
		private void EnsureBufferFilled()
		{
			if (this._bufferAvailable < 16)
			{
				this._readBuffer[0] = 0;
				this._readBuffer[1] = 0;
				this._byteStream.Read(this._readBuffer, 0, 2);
				this._buffer = (this._buffer << 16 | (uint)((uint)this._readBuffer[0] << 8) | (uint)this._readBuffer[1]);
				this._bufferAvailable += 16;
			}
		}

		// Token: 0x0400018E RID: 398
		private uint _buffer;

		// Token: 0x0400018F RID: 399
		private int _bufferAvailable;

		// Token: 0x04000190 RID: 400
		private readonly Stream _byteStream;

		// Token: 0x04000191 RID: 401
		private readonly byte[] _readBuffer = new byte[2];
	}
}
