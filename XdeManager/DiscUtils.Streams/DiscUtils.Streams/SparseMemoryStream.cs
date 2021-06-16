using System;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x02000027 RID: 39
	public sealed class SparseMemoryStream : BufferStream
	{
		// Token: 0x0600013E RID: 318 RVA: 0x00004D93 File Offset: 0x00002F93
		public SparseMemoryStream() : base(new SparseMemoryBuffer(16384), FileAccess.ReadWrite)
		{
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00004DA6 File Offset: 0x00002FA6
		public SparseMemoryStream(SparseMemoryBuffer buffer, FileAccess access) : base(buffer, access)
		{
		}
	}
}
