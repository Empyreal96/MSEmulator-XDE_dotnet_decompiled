using System;

namespace DiscUtils.Streams
{
	// Token: 0x02000018 RID: 24
	public sealed class CircularStream : WrappingStream
	{
		// Token: 0x060000B8 RID: 184 RVA: 0x000039BF File Offset: 0x00001BBF
		public CircularStream(SparseStream toWrap, Ownership ownership) : base(toWrap, ownership)
		{
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000039C9 File Offset: 0x00001BC9
		public override int Read(byte[] buffer, int offset, int count)
		{
			this.WrapPosition();
			int result = base.Read(buffer, offset, (int)Math.Min(this.Length - this.Position, (long)count));
			this.WrapPosition();
			return result;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000039F4 File Offset: 0x00001BF4
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.WrapPosition();
			int num;
			for (int i = 0; i < count; i += num)
			{
				num = (int)Math.Min((long)(count - i), this.Length - this.Position);
				base.Write(buffer, offset + i, num);
				this.WrapPosition();
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00003A3C File Offset: 0x00001C3C
		private void WrapPosition()
		{
			long position = this.Position;
			long length = this.Length;
			if (position >= length)
			{
				this.Position = position % length;
			}
		}
	}
}
