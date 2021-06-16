using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000056 RID: 86
	internal static class BufferUtils
	{
		// Token: 0x0600053D RID: 1341 RVA: 0x00016DFB File Offset: 0x00014FFB
		public static char[] RentBuffer(IArrayPool<char> bufferPool, int minSize)
		{
			if (bufferPool == null)
			{
				return new char[minSize];
			}
			return bufferPool.Rent(minSize);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00016E0E File Offset: 0x0001500E
		public static void ReturnBuffer(IArrayPool<char> bufferPool, char[] buffer)
		{
			if (bufferPool != null)
			{
				bufferPool.Return(buffer);
			}
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00016E1A File Offset: 0x0001501A
		public static char[] EnsureBufferSize(IArrayPool<char> bufferPool, int size, char[] buffer)
		{
			if (bufferPool == null)
			{
				return new char[size];
			}
			if (buffer != null)
			{
				bufferPool.Return(buffer);
			}
			return bufferPool.Rent(size);
		}
	}
}
