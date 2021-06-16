using System;
using System.Collections.Generic;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x0200001C RID: 28
	public abstract class MappedStream : SparseStream
	{
		// Token: 0x060000D2 RID: 210 RVA: 0x00003E86 File Offset: 0x00002086
		public new static MappedStream FromStream(Stream stream, Ownership takeOwnership)
		{
			return new WrappingMappedStream<Stream>(stream, takeOwnership, null);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00003E90 File Offset: 0x00002090
		public new static MappedStream FromStream(Stream stream, Ownership takeOwnership, IEnumerable<StreamExtent> extents)
		{
			return new WrappingMappedStream<Stream>(stream, takeOwnership, extents);
		}

		// Token: 0x060000D4 RID: 212
		public abstract IEnumerable<StreamExtent> MapContent(long start, long length);
	}
}
