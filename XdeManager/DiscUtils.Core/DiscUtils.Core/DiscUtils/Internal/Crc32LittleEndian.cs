using System;

namespace DiscUtils.Internal
{
	// Token: 0x0200006E RID: 110
	internal sealed class Crc32LittleEndian : Crc32
	{
		// Token: 0x0600040A RID: 1034 RVA: 0x0000C268 File Offset: 0x0000A468
		static Crc32LittleEndian()
		{
			Crc32LittleEndian.Tables[0] = Crc32LittleEndian.CalcTable(3988292384U);
			Crc32LittleEndian.Tables[1] = Crc32LittleEndian.CalcTable(2197175160U);
			Crc32LittleEndian.Tables[2] = Crc32LittleEndian.CalcTable(3945912366U);
			Crc32LittleEndian.Tables[3] = Crc32LittleEndian.CalcTable(3582100097U);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0000C2C4 File Offset: 0x0000A4C4
		public Crc32LittleEndian(Crc32Algorithm algorithm) : base(Crc32LittleEndian.Tables[(int)algorithm])
		{
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0000C2D3 File Offset: 0x0000A4D3
		public static uint Compute(Crc32Algorithm algorithm, byte[] buffer, int offset, int count)
		{
			return Crc32LittleEndian.Process(Crc32LittleEndian.Tables[(int)algorithm], uint.MaxValue, buffer, offset, count) ^ uint.MaxValue;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000C2E7 File Offset: 0x0000A4E7
		public override void Process(byte[] buffer, int offset, int count)
		{
			this._value = Crc32LittleEndian.Process(this.Table, this._value, buffer, offset, count);
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0000C304 File Offset: 0x0000A504
		private static uint[] CalcTable(uint polynomial)
		{
			uint[] array = new uint[256];
			array[0] = 0U;
			for (uint num = 0U; num <= 255U; num += 1U)
			{
				uint num2 = num;
				for (int i = 8; i > 0; i--)
				{
					if ((num2 & 1U) != 0U)
					{
						num2 = (num2 >> 1 ^ polynomial);
					}
					else
					{
						num2 >>= 1;
					}
				}
				array[(int)num] = num2;
			}
			return array;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0000C354 File Offset: 0x0000A554
		private static uint Process(uint[] table, uint accumulator, byte[] buffer, int offset, int count)
		{
			uint num = accumulator;
			for (int i = 0; i < count; i++)
			{
				byte b = buffer[offset + i];
				uint num2 = num >> 8 & 16777215U;
				uint num3 = table[(int)((num ^ (uint)b) & 255U)];
				num = (num2 ^ num3);
			}
			return num;
		}

		// Token: 0x04000181 RID: 385
		private static readonly uint[][] Tables = new uint[4][];
	}
}
