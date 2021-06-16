using System;

namespace DiscUtils.Compression
{
	// Token: 0x0200007D RID: 125
	public abstract class BlockCompressor
	{
		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x0000D0FF File Offset: 0x0000B2FF
		// (set) Token: 0x0600046E RID: 1134 RVA: 0x0000D107 File Offset: 0x0000B307
		public int BlockSize { get; set; }

		// Token: 0x0600046F RID: 1135
		public abstract CompressionResult Compress(byte[] source, int sourceOffset, int sourceLength, byte[] compressed, int compressedOffset, ref int compressedLength);

		// Token: 0x06000470 RID: 1136
		public abstract int Decompress(byte[] source, int sourceOffset, int sourceLength, byte[] decompressed, int decompressedOffset);
	}
}
