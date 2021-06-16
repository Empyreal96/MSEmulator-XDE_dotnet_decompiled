using System;
using DiscUtils.Compression;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000032 RID: 50
	internal sealed class LZNT1 : BlockCompressor
	{
		// Token: 0x060001F2 RID: 498 RVA: 0x0000A37C File Offset: 0x0000857C
		public LZNT1()
		{
			base.BlockSize = 4096;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000A390 File Offset: 0x00008590
		public override CompressionResult Compress(byte[] source, int sourceOffset, int sourceLength, byte[] compressed, int compressedOffset, ref int compressedLength)
		{
			uint num = 0U;
			uint num2 = 0U;
			LzWindowDictionary lzWindowDictionary = new LzWindowDictionary();
			bool flag = false;
			for (int i = 0; i < sourceLength; i += base.BlockSize)
			{
				lzWindowDictionary.MinMatchAmount = 3;
				uint num3 = num;
				uint num4 = (uint)Math.Min(sourceLength - i, base.BlockSize);
				uint num5 = 0U;
				uint num6 = num2;
				checked
				{
					compressed[(int)((IntPtr)(unchecked((long)compressedOffset + (long)((ulong)num2))))] = (compressed[(int)((IntPtr)(unchecked((long)compressedOffset + (long)((ulong)num2) + 1L)))] = 0);
				}
				num2 += 2U;
				while ((ulong)num - (ulong)((long)i) < (ulong)num4)
				{
					if ((ulong)(num2 + 1U) >= (ulong)((long)compressedLength))
					{
						return CompressionResult.Incompressible;
					}
					byte b = 0;
					uint num7 = num2;
					compressed[(int)(checked((IntPtr)(unchecked((long)compressedOffset + (long)((ulong)num2)))))] = b;
					num5 += 1U;
					num2 += 1U;
					for (int j = 0; j < 8; j++)
					{
						int num8 = (int)(16 - LZNT1._compressionBits[(int)(checked((IntPtr)(unchecked((ulong)num - (ulong)((long)i)))))]);
						ushort num9 = (ushort)((1 << (int)LZNT1._compressionBits[(int)(checked((IntPtr)(unchecked((ulong)num - (ulong)((long)i)))))]) - 1);
						lzWindowDictionary.MaxMatchAmount = Math.Min(1 << num8, base.BlockSize - 1);
						int[] array = lzWindowDictionary.Search(source, sourceOffset + i, (uint)((ulong)num - (ulong)((long)i)), num4);
						if (array[1] > 0)
						{
							if ((ulong)(num2 + 2U) >= (ulong)((long)compressedLength))
							{
								return CompressionResult.Incompressible;
							}
							b |= (byte)(1 << j);
							int num10 = array[0];
							int num11 = array[1];
							int num12 = num10 - 1 << num8;
							int num13 = num11 - 3 & (1 << (int)num9) - 1;
							EndianUtilities.WriteBytesLittleEndian((ushort)(num12 | num13), compressed, compressedOffset + (int)num2);
							lzWindowDictionary.AddEntryRange(source, sourceOffset + i, (int)((ulong)num - (ulong)((long)i)), array[1]);
							num += (uint)array[1];
							num2 += 2U;
							num5 += 2U;
						}
						else
						{
							if ((ulong)(num2 + 1U) >= (ulong)((long)compressedLength))
							{
								return CompressionResult.Incompressible;
							}
							b |= (byte)(0 << j);
							checked
							{
								if (source[(int)((IntPtr)(unchecked((long)sourceOffset + (long)((ulong)num))))] != 0)
								{
									flag = true;
								}
								compressed[(int)((IntPtr)(unchecked((long)compressedOffset + (long)((ulong)num2))))] = source[(int)((IntPtr)(unchecked((long)sourceOffset + (long)((ulong)num))))];
							}
							lzWindowDictionary.AddEntry(source, sourceOffset + i, (int)((ulong)num - (ulong)((long)i)));
							num += 1U;
							num2 += 1U;
							num5 += 1U;
						}
						if ((ulong)num - (ulong)((long)i) >= (ulong)num4)
						{
							break;
						}
					}
					compressed[(int)(checked((IntPtr)(unchecked((long)compressedOffset + (long)((ulong)num7)))))] = b;
				}
				if ((ulong)num5 >= (ulong)((long)base.BlockSize))
				{
					EndianUtilities.WriteBytesLittleEndian((ushort)(12288 | base.BlockSize - 1), compressed, compressedOffset + (int)num6);
					Array.Copy(source, (int)((long)sourceOffset + (long)((ulong)num3)), compressed, (int)((long)compressedOffset + (long)((ulong)num6) + 2L), base.BlockSize);
					num2 = (uint)((ulong)(num6 + 2U) + (ulong)((long)base.BlockSize));
					compressed[(int)num2] = 0;
					compressed[(int)(num2 + 1U)] = 0;
				}
				else
				{
					EndianUtilities.WriteBytesLittleEndian((ushort)(45056U | num5 - 1U), compressed, compressedOffset + (int)num6);
				}
				lzWindowDictionary.Reset();
			}
			if ((ulong)num2 >= (ulong)((long)sourceLength))
			{
				compressedLength = 0;
				return CompressionResult.Incompressible;
			}
			if (flag)
			{
				compressedLength = (int)num2;
				return CompressionResult.Compressed;
			}
			compressedLength = 0;
			return CompressionResult.AllZeros;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000A63C File Offset: 0x0000883C
		public override int Decompress(byte[] source, int sourceOffset, int sourceLength, byte[] decompressed, int decompressedOffset)
		{
			int i = 0;
			int num = 0;
			while (i < sourceLength)
			{
				ushort num2 = EndianUtilities.ToUInt16LittleEndian(source, sourceOffset + i);
				i += 2;
				if (num2 == 0)
				{
					break;
				}
				if ((num2 & 32768) == 0)
				{
					int num3 = (int)((num2 & 4095) + 1);
					Array.Copy(source, sourceOffset + i, decompressed, decompressedOffset + num, num3);
					i += num3;
					num += num3;
				}
				else
				{
					int num4 = num;
					int num5 = i + (int)(num2 & 4095) + 1;
					while (i < num5)
					{
						byte b = source[sourceOffset + i];
						i++;
						int num6 = 0;
						while (num6 < 8 && i < num5)
						{
							if ((b & 1) == 0)
							{
								if (decompressedOffset + num >= decompressed.Length)
								{
									return num;
								}
								decompressed[decompressedOffset + num] = source[sourceOffset + i];
								num++;
								i++;
							}
							else
							{
								ushort num7 = (ushort)(16 - LZNT1._compressionBits[num - num4]);
								ushort num8 = (ushort)((1 << (int)num7) - 1);
								ushort num9 = EndianUtilities.ToUInt16LittleEndian(source, sourceOffset + i);
								i += 2;
								int num10 = num - (num9 >> (int)num7) - 1;
								int num11 = (int)((num9 & num8) + 3);
								for (int j = 0; j < num11; j++)
								{
									decompressed[decompressedOffset + num++] = decompressed[decompressedOffset + num10++];
								}
							}
							b = (byte)(b >> 1);
							num6++;
						}
					}
					if (decompressedOffset + num + 4096 > decompressed.Length)
					{
						return num;
					}
					if (num < num4 + 4096)
					{
						int num12 = num4 + 4096 - num;
						Array.Clear(decompressed, decompressedOffset + num, num12);
						num += num12;
					}
				}
			}
			return num;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000A7BC File Offset: 0x000089BC
		private static byte[] CalcCompressionBits()
		{
			byte[] array = new byte[4096];
			byte b = 0;
			int num = 16;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 4 + b;
				if (i == num)
				{
					num <<= 1;
					b += 1;
				}
			}
			return array;
		}

		// Token: 0x040000E7 RID: 231
		private const ushort SubBlockIsCompressedFlag = 32768;

		// Token: 0x040000E8 RID: 232
		private const ushort SubBlockSizeMask = 4095;

		// Token: 0x040000E9 RID: 233
		private const int FixedBlockSize = 4096;

		// Token: 0x040000EA RID: 234
		private static readonly byte[] _compressionBits = LZNT1.CalcCompressionBits();
	}
}
