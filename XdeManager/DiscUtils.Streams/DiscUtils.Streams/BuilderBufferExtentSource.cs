using System;

namespace DiscUtils.Streams
{
	// Token: 0x0200000E RID: 14
	public class BuilderBufferExtentSource : BuilderExtentSource
	{
		// Token: 0x06000081 RID: 129 RVA: 0x0000334B File Offset: 0x0000154B
		public BuilderBufferExtentSource(byte[] buffer)
		{
			this._buffer = buffer;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000335A File Offset: 0x0000155A
		public override BuilderExtent Fix(long pos)
		{
			return new BuilderBufferExtent(pos, this._buffer);
		}

		// Token: 0x0400002C RID: 44
		private readonly byte[] _buffer;
	}
}
