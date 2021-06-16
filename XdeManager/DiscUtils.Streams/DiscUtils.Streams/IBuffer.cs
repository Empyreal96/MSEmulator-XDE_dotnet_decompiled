using System;
using System.Collections.Generic;

namespace DiscUtils.Streams
{
	// Token: 0x0200000A RID: 10
	public interface IBuffer
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000064 RID: 100
		bool CanRead { get; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000065 RID: 101
		bool CanWrite { get; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000066 RID: 102
		long Capacity { get; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000067 RID: 103
		IEnumerable<StreamExtent> Extents { get; }

		// Token: 0x06000068 RID: 104
		int Read(long pos, byte[] buffer, int offset, int count);

		// Token: 0x06000069 RID: 105
		void Write(long pos, byte[] buffer, int offset, int count);

		// Token: 0x0600006A RID: 106
		void Clear(long pos, int count);

		// Token: 0x0600006B RID: 107
		void Flush();

		// Token: 0x0600006C RID: 108
		void SetCapacity(long value);

		// Token: 0x0600006D RID: 109
		IEnumerable<StreamExtent> GetExtentsInRange(long start, long count);
	}
}
