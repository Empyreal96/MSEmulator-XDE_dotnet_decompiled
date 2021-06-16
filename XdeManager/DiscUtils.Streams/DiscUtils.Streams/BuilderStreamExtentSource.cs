using System;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x02000014 RID: 20
	public class BuilderStreamExtentSource : BuilderExtentSource
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x00003521 File Offset: 0x00001721
		public BuilderStreamExtentSource(Stream stream)
		{
			this._stream = stream;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003530 File Offset: 0x00001730
		public override BuilderExtent Fix(long pos)
		{
			return new BuilderStreamExtent(pos, this._stream);
		}

		// Token: 0x04000034 RID: 52
		private readonly Stream _stream;
	}
}
