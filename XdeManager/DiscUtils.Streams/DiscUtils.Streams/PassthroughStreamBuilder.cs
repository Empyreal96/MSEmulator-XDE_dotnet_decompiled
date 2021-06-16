using System;
using System.Collections.Generic;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x02000015 RID: 21
	public class PassthroughStreamBuilder : StreamBuilder
	{
		// Token: 0x060000A2 RID: 162 RVA: 0x0000353E File Offset: 0x0000173E
		public PassthroughStreamBuilder(Stream stream)
		{
			this._stream = stream;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000354D File Offset: 0x0000174D
		protected override List<BuilderExtent> FixExtents(out long totalLength)
		{
			this._stream.Position = 0L;
			List<BuilderExtent> list = new List<BuilderExtent>();
			list.Add(new BuilderStreamExtent(0L, this._stream));
			totalLength = this._stream.Length;
			return list;
		}

		// Token: 0x04000035 RID: 53
		private readonly Stream _stream;
	}
}
