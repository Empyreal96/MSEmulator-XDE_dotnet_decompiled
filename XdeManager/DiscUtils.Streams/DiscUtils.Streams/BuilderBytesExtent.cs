using System;

namespace DiscUtils.Streams
{
	// Token: 0x0200000F RID: 15
	public class BuilderBytesExtent : BuilderExtent
	{
		// Token: 0x06000083 RID: 131 RVA: 0x00003368 File Offset: 0x00001568
		public BuilderBytesExtent(long start, byte[] data) : base(start, (long)data.Length)
		{
			this._data = data;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000337C File Offset: 0x0000157C
		protected BuilderBytesExtent(long start, long length) : base(start, length)
		{
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003386 File Offset: 0x00001586
		public override void Dispose()
		{
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003388 File Offset: 0x00001588
		public override void PrepareForRead()
		{
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000338C File Offset: 0x0000158C
		public override int Read(long diskOffset, byte[] block, int offset, int count)
		{
			int num = (int)Math.Min(diskOffset - base.Start, (long)this._data.Length);
			int num2 = Math.Min(count, this._data.Length - num);
			Array.Copy(this._data, num, block, offset, num2);
			return num2;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000033D3 File Offset: 0x000015D3
		public override void DisposeReadState()
		{
		}

		// Token: 0x0400002D RID: 45
		protected byte[] _data;
	}
}
