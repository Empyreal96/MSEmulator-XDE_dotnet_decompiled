using System;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x0200001E RID: 30
	public class PositionWrappingStream : WrappingStream
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x000041FC File Offset: 0x000023FC
		public PositionWrappingStream(SparseStream toWrap, long currentPosition, Ownership ownership) : base(toWrap, ownership)
		{
			this._position = currentPosition;
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x0000420D File Offset: 0x0000240D
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00004215 File Offset: 0x00002415
		public override long Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (this._position == value)
				{
					return;
				}
				this.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000422C File Offset: 0x0000242C
		public override long Seek(long offset, SeekOrigin origin)
		{
			if (base.CanSeek)
			{
				return base.Seek(offset, SeekOrigin.Current);
			}
			switch (origin)
			{
			case SeekOrigin.Begin:
				offset -= this._position;
				break;
			case SeekOrigin.Current:
				offset += this._position;
				break;
			case SeekOrigin.End:
				offset = this.Length - offset;
				break;
			default:
				throw new ArgumentOutOfRangeException("origin", origin, null);
			}
			if (offset == 0L)
			{
				return this._position;
			}
			if (offset < 0L)
			{
				throw new NotSupportedException("backward seeking is not supported");
			}
			byte[] array = new byte[1024L];
			while (offset > 0L)
			{
				int num = base.Read(array, 0, (int)Math.Min((long)array.Length, offset));
				offset -= (long)num;
			}
			return this._position;
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x000042E2 File Offset: 0x000024E2
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000042E8 File Offset: 0x000024E8
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = base.Read(buffer, offset, count);
			this._position += (long)num;
			return num;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000430F File Offset: 0x0000250F
		public override void Write(byte[] buffer, int offset, int count)
		{
			base.Write(buffer, offset, count);
			this._position += (long)count;
		}

		// Token: 0x04000046 RID: 70
		private long _position;
	}
}
