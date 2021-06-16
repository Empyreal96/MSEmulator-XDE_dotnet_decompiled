using System;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x02000013 RID: 19
	public class BuilderStreamExtent : BuilderExtent
	{
		// Token: 0x0600009A RID: 154 RVA: 0x000034AC File Offset: 0x000016AC
		public BuilderStreamExtent(long start, Stream source) : this(start, source, Ownership.None)
		{
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000034B7 File Offset: 0x000016B7
		public BuilderStreamExtent(long start, Stream source, Ownership ownership) : base(start, source.Length)
		{
			this._source = source;
			this._ownership = ownership;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000034D4 File Offset: 0x000016D4
		public override void Dispose()
		{
			if (this._source != null && this._ownership == Ownership.Dispose)
			{
				this._source.Dispose();
				this._source = null;
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000034F9 File Offset: 0x000016F9
		public override void PrepareForRead()
		{
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000034FB File Offset: 0x000016FB
		public override int Read(long diskOffset, byte[] block, int offset, int count)
		{
			this._source.Position = diskOffset - base.Start;
			return this._source.Read(block, offset, count);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000351F File Offset: 0x0000171F
		public override void DisposeReadState()
		{
		}

		// Token: 0x04000032 RID: 50
		private readonly Ownership _ownership;

		// Token: 0x04000033 RID: 51
		private Stream _source;
	}
}
