using System;

namespace DiscUtils.Internal
{
	// Token: 0x0200006D RID: 109
	internal sealed class Crc32BigEndian : Crc32
	{
		// Token: 0x06000404 RID: 1028 RVA: 0x0000C148 File Offset: 0x0000A348
		static Crc32BigEndian()
		{
			Crc32BigEndian.Tables[0] = Crc32BigEndian.CalcTable(79764919U);
			Crc32BigEndian.Tables[1] = Crc32BigEndian.CalcTable(517762881U);
			Crc32BigEndian.Tables[2] = Crc32BigEndian.CalcTable(1947962583U);
			Crc32BigEndian.Tables[3] = Crc32BigEndian.CalcTable(2168537515U);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000C1A4 File Offset: 0x0000A3A4
		public Crc32BigEndian(Crc32Algorithm algorithm) : base(Crc32BigEndian.Tables[(int)algorithm])
		{
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000C1B3 File Offset: 0x0000A3B3
		public static uint Compute(Crc32Algorithm algorithm, byte[] buffer, int offset, int count)
		{
			return Crc32BigEndian.Process(Crc32BigEndian.Tables[(int)algorithm], uint.MaxValue, buffer, offset, count) ^ uint.MaxValue;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000C1C7 File Offset: 0x0000A3C7
		public override void Process(byte[] buffer, int offset, int count)
		{
			this._value = Crc32BigEndian.Process(this.Table, this._value, buffer, offset, count);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000C1E4 File Offset: 0x0000A3E4
		private static uint[] CalcTable(uint polynomial)
		{
			uint[] array = new uint[256];
			for (uint num = 0U; num < 256U; num += 1U)
			{
				uint num2 = num << 24;
				for (int i = 8; i > 0; i--)
				{
					if ((num2 & 2147483648U) != 0U)
					{
						num2 = (num2 << 1 ^ polynomial);
					}
					else
					{
						num2 <<= 1;
					}
				}
				array[(int)num] = num2;
			}
			return array;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000C238 File Offset: 0x0000A438
		private static uint Process(uint[] table, uint accumulator, byte[] buffer, int offset, int count)
		{
			uint num = accumulator;
			for (int i = 0; i < count; i++)
			{
				byte b = buffer[offset + i];
				num = (table[(int)(num >> 24 ^ (uint)b)] ^ num << 8);
			}
			return num;
		}

		// Token: 0x04000180 RID: 384
		private static readonly uint[][] Tables = new uint[4][];
	}
}
