using System;
using System.IO;

namespace DiscUtils.Compression
{
	// Token: 0x0200007E RID: 126
	internal class BZip2BlockDecoder
	{
		// Token: 0x06000472 RID: 1138 RVA: 0x0000D118 File Offset: 0x0000B318
		public BZip2BlockDecoder(int blockSize)
		{
			this._inverseBurrowsWheeler = new InverseBurrowsWheeler(blockSize);
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x0000D12C File Offset: 0x0000B32C
		// (set) Token: 0x06000474 RID: 1140 RVA: 0x0000D134 File Offset: 0x0000B334
		public uint Crc { get; private set; }

		// Token: 0x06000475 RID: 1141 RVA: 0x0000D140 File Offset: 0x0000B340
		public int Process(BitStream bitstream, byte[] outputBuffer, int outputBufferOffset)
		{
			this.Crc = 0U;
			for (int i = 0; i < 4; i++)
			{
				this.Crc = (this.Crc << 8 | bitstream.Read(8));
			}
			bool flag = bitstream.Read(1) > 0U;
			int originalIndex = (int)bitstream.Read(24);
			int num = BZip2BlockDecoder.ReadBuffer(bitstream, outputBuffer, outputBufferOffset);
			this._inverseBurrowsWheeler.OriginalIndex = originalIndex;
			this._inverseBurrowsWheeler.Process(outputBuffer, outputBufferOffset, num, outputBuffer, outputBufferOffset);
			if (flag)
			{
				new BZip2Randomizer().Process(outputBuffer, outputBufferOffset, num, outputBuffer, outputBufferOffset);
			}
			return num;
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0000D1C4 File Offset: 0x0000B3C4
		private static int ReadBuffer(BitStream bitstream, byte[] buffer, int offset)
		{
			int num = 0;
			MoveToFront moveToFront = new MoveToFront();
			bool[] array = new bool[16];
			for (int i = 0; i < 16; i++)
			{
				array[i] = (bitstream.Read(1) > 0U);
			}
			for (int j = 0; j < 256; j++)
			{
				if (array[j / 16] && bitstream.Read(1) != 0U)
				{
					moveToFront.Set(num, (byte)j);
					num++;
				}
			}
			BZip2CombinedHuffmanTrees bzip2CombinedHuffmanTrees = new BZip2CombinedHuffmanTrees(bitstream, num + 2);
			int num2 = 0;
			uint num3;
			for (;;)
			{
				num3 = bzip2CombinedHuffmanTrees.NextSymbol();
				if (num3 < 2U)
				{
					uint num4 = 0U;
					int num5 = 0;
					while (num3 < 2U)
					{
						num4 += num3 + 1U << num5;
						num5++;
						num3 = bzip2CombinedHuffmanTrees.NextSymbol();
					}
					byte head = moveToFront.Head;
					while (num4 > 0U)
					{
						buffer[offset + num2] = head;
						num2++;
						num4 -= 1U;
					}
				}
				if ((ulong)num3 > (ulong)((long)num))
				{
					break;
				}
				byte andMove = moveToFront.GetAndMove((int)(num3 - 1U));
				buffer[offset + num2] = andMove;
				num2++;
			}
			if ((ulong)num3 == (ulong)((long)(num + 1)))
			{
				return num2;
			}
			throw new InvalidDataException("Invalid symbol from Huffman table");
		}

		// Token: 0x04000193 RID: 403
		private readonly InverseBurrowsWheeler _inverseBurrowsWheeler;
	}
}
