using System;

namespace DiscUtils.Compression
{
	// Token: 0x0200007A RID: 122
	public class Adler32
	{
		// Token: 0x0600045F RID: 1119 RVA: 0x0000CEE5 File Offset: 0x0000B0E5
		public Adler32()
		{
			this._a = 1U;
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x0000CEF4 File Offset: 0x0000B0F4
		public int Value
		{
			get
			{
				return (int)(this._b << 16 | this._a);
			}
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0000CF08 File Offset: 0x0000B108
		public void Process(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentException("Offset outside of array bounds", "offset");
			}
			if (count < 0 || offset + count > buffer.Length)
			{
				throw new ArgumentException("Array index out of bounds", "count");
			}
			int i = 0;
			while (i < count)
			{
				int num = Math.Min(count, i + 2000);
				while (i < num)
				{
					this._a += (uint)buffer[i++];
					this._b += this._a;
				}
				this._a %= 65521U;
				this._b %= 65521U;
			}
		}

		// Token: 0x0400018C RID: 396
		private uint _a;

		// Token: 0x0400018D RID: 397
		private uint _b;
	}
}
