using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000062 RID: 98
	internal struct StringBuffer
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x00019179 File Offset: 0x00017379
		// (set) Token: 0x060005B9 RID: 1465 RVA: 0x00019181 File Offset: 0x00017381
		public int Position
		{
			get
			{
				return this._position;
			}
			set
			{
				this._position = value;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x0001918A File Offset: 0x0001738A
		public bool IsEmpty
		{
			get
			{
				return this._buffer == null;
			}
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00019195 File Offset: 0x00017395
		public StringBuffer(IArrayPool<char> bufferPool, int initalSize)
		{
			this = new StringBuffer(BufferUtils.RentBuffer(bufferPool, initalSize));
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x000191A4 File Offset: 0x000173A4
		private StringBuffer(char[] buffer)
		{
			this._buffer = buffer;
			this._position = 0;
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x000191B4 File Offset: 0x000173B4
		public void Append(IArrayPool<char> bufferPool, char value)
		{
			if (this._position == this._buffer.Length)
			{
				this.EnsureSize(bufferPool, 1);
			}
			char[] buffer = this._buffer;
			int position = this._position;
			this._position = position + 1;
			buffer[position] = value;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x000191F4 File Offset: 0x000173F4
		public void Append(IArrayPool<char> bufferPool, char[] buffer, int startIndex, int count)
		{
			if (this._position + count >= this._buffer.Length)
			{
				this.EnsureSize(bufferPool, count);
			}
			Array.Copy(buffer, startIndex, this._buffer, this._position, count);
			this._position += count;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00019241 File Offset: 0x00017441
		public void Clear(IArrayPool<char> bufferPool)
		{
			if (this._buffer != null)
			{
				BufferUtils.ReturnBuffer(bufferPool, this._buffer);
				this._buffer = null;
			}
			this._position = 0;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00019268 File Offset: 0x00017468
		private void EnsureSize(IArrayPool<char> bufferPool, int appendLength)
		{
			char[] array = BufferUtils.RentBuffer(bufferPool, (this._position + appendLength) * 2);
			if (this._buffer != null)
			{
				Array.Copy(this._buffer, array, this._position);
				BufferUtils.ReturnBuffer(bufferPool, this._buffer);
			}
			this._buffer = array;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x000192B3 File Offset: 0x000174B3
		public override string ToString()
		{
			return this.ToString(0, this._position);
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x000192C2 File Offset: 0x000174C2
		public string ToString(int start, int length)
		{
			return new string(this._buffer, start, length);
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x000192D1 File Offset: 0x000174D1
		public char[] InternalBuffer
		{
			get
			{
				return this._buffer;
			}
		}

		// Token: 0x040001FC RID: 508
		private char[] _buffer;

		// Token: 0x040001FD RID: 509
		private int _position;
	}
}
