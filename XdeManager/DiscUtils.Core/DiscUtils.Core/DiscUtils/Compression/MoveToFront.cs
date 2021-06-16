using System;

namespace DiscUtils.Compression
{
	// Token: 0x02000087 RID: 135
	internal class MoveToFront
	{
		// Token: 0x060004B0 RID: 1200 RVA: 0x0000DE2C File Offset: 0x0000C02C
		public MoveToFront() : this(256, false)
		{
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0000DE3C File Offset: 0x0000C03C
		public MoveToFront(int size, bool autoInit)
		{
			this._buffer = new byte[size];
			if (autoInit)
			{
				byte b = 0;
				while ((int)b < size)
				{
					this._buffer[(int)b] = b;
					b += 1;
				}
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x0000DE74 File Offset: 0x0000C074
		public byte Head
		{
			get
			{
				return this._buffer[0];
			}
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0000DE7E File Offset: 0x0000C07E
		public void Set(int pos, byte val)
		{
			this._buffer[pos] = val;
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0000DE8C File Offset: 0x0000C08C
		public byte GetAndMove(int pos)
		{
			byte b = this._buffer[pos];
			for (int i = pos; i > 0; i--)
			{
				this._buffer[i] = this._buffer[i - 1];
			}
			this._buffer[0] = b;
			return b;
		}

		// Token: 0x040001BA RID: 442
		private readonly byte[] _buffer;
	}
}
