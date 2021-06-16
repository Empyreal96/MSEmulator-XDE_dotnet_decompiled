using System;

namespace DiscUtils.Streams
{
	// Token: 0x02000030 RID: 48
	public static class BitCounter
	{
		// Token: 0x060001B7 RID: 439 RVA: 0x00006240 File Offset: 0x00004440
		static BitCounter()
		{
			for (int i = 0; i < 256; i++)
			{
				byte b = 0;
				for (int num = i; num != 0; num &= (int)((byte)(num - 1)))
				{
					b += 1;
				}
				BitCounter._lookupTable[i] = b;
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00006289 File Offset: 0x00004489
		public static byte Count(byte value)
		{
			return BitCounter._lookupTable[(int)value];
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00006294 File Offset: 0x00004494
		public static long Count(byte[] values, int offset, int count)
		{
			int num = offset + count;
			if (num > values.Length)
			{
				throw new ArgumentOutOfRangeException("count", "can't count after end of values");
			}
			long num2 = 0L;
			for (int i = offset; i < num; i++)
			{
				byte b = values[i];
				num2 += (long)((ulong)BitCounter._lookupTable[(int)b]);
			}
			return num2;
		}

		// Token: 0x04000076 RID: 118
		private static readonly byte[] _lookupTable = new byte[256];
	}
}
