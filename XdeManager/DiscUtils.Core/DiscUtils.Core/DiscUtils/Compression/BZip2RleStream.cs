using System;
using System.IO;

namespace DiscUtils.Compression
{
	// Token: 0x02000082 RID: 130
	internal class BZip2RleStream : Stream
	{
		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x0000D96C File Offset: 0x0000BB6C
		public bool AtEof
		{
			get
			{
				return this._runBytesOutstanding == 0 && this._blockRemaining == 0;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x0000D981 File Offset: 0x0000BB81
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x0000D984 File Offset: 0x0000BB84
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x0000D987 File Offset: 0x0000BB87
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x0000D98A File Offset: 0x0000BB8A
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x0000D991 File Offset: 0x0000BB91
		// (set) Token: 0x06000497 RID: 1175 RVA: 0x0000D999 File Offset: 0x0000BB99
		public override long Position
		{
			get
			{
				return this._position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0000D9A0 File Offset: 0x0000BBA0
		public void Reset(byte[] buffer, int offset, int count)
		{
			this._position = 0L;
			this._blockBuffer = buffer;
			this._blockOffset = offset;
			this._blockRemaining = count;
			this._numSame = -1;
			this._lastByte = 0;
			this._runBytesOutstanding = 0;
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0000D9D4 File Offset: 0x0000BBD4
		public override void Flush()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0000D9DC File Offset: 0x0000BBDC
		public override int Read(byte[] buffer, int offset, int count)
		{
			int i;
			int num;
			for (i = 0; i < count; i += num)
			{
				if (this._runBytesOutstanding <= 0)
				{
					break;
				}
				num = Math.Min(this._runBytesOutstanding, count);
				for (int j = 0; j < num; j++)
				{
					buffer[offset + i] = this._lastByte;
				}
				this._runBytesOutstanding -= num;
			}
			while (i < count && this._blockRemaining > 0)
			{
				byte b = this._blockBuffer[this._blockOffset];
				this._blockOffset++;
				this._blockRemaining--;
				if (this._numSame == 4)
				{
					int num2 = Math.Min((int)b, count - i);
					for (int k = 0; k < num2; k++)
					{
						buffer[offset + i] = this._lastByte;
						i++;
					}
					this._runBytesOutstanding = (int)b - num2;
					this._numSame = 0;
				}
				else
				{
					if (b != this._lastByte || this._numSame <= 0)
					{
						this._lastByte = b;
						this._numSame = 0;
					}
					buffer[offset + i] = b;
					i++;
					this._numSame++;
				}
			}
			this._position += (long)i;
			return i;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0000DB01 File Offset: 0x0000BD01
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0000DB08 File Offset: 0x0000BD08
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0000DB0F File Offset: 0x0000BD0F
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x040001A8 RID: 424
		private byte[] _blockBuffer;

		// Token: 0x040001A9 RID: 425
		private int _blockOffset;

		// Token: 0x040001AA RID: 426
		private int _blockRemaining;

		// Token: 0x040001AB RID: 427
		private byte _lastByte;

		// Token: 0x040001AC RID: 428
		private int _numSame;

		// Token: 0x040001AD RID: 429
		private long _position;

		// Token: 0x040001AE RID: 430
		private int _runBytesOutstanding;
	}
}
