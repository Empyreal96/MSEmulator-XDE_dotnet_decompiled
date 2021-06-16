using System;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Registry
{
	// Token: 0x02000003 RID: 3
	internal sealed class BinHeader : IByteArraySerializable
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000025F8 File Offset: 0x000007F8
		public int Size
		{
			get
			{
				return 32;
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000025FC File Offset: 0x000007FC
		public int ReadFrom(byte[] buffer, int offset)
		{
			if (EndianUtilities.ToUInt32LittleEndian(buffer, offset) != 1852400232U)
			{
				throw new IOException("Invalid signature for registry bin");
			}
			this.FileOffset = EndianUtilities.ToInt32LittleEndian(buffer, offset + 4);
			this.BinSize = EndianUtilities.ToInt32LittleEndian(buffer, offset + 8);
			EndianUtilities.ToInt64LittleEndian(buffer, offset + 12);
			EndianUtilities.ToInt64LittleEndian(buffer, offset + 20);
			EndianUtilities.ToInt32LittleEndian(buffer, offset + 28);
			return 32;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002663 File Offset: 0x00000863
		public void WriteTo(byte[] buffer, int offset)
		{
			EndianUtilities.WriteBytesLittleEndian(1852400232U, buffer, offset);
			EndianUtilities.WriteBytesLittleEndian(this.FileOffset, buffer, offset + 4);
			EndianUtilities.WriteBytesLittleEndian(this.BinSize, buffer, offset + 8);
		}

		// Token: 0x04000007 RID: 7
		public const int HeaderSize = 32;

		// Token: 0x04000008 RID: 8
		private const uint Signature = 1852400232U;

		// Token: 0x04000009 RID: 9
		public int BinSize;

		// Token: 0x0400000A RID: 10
		public int FileOffset;
	}
}
