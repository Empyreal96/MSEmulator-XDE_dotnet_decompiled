using System;

namespace DiscUtils.Streams
{
	// Token: 0x02000032 RID: 50
	public static class MathUtilities
	{
		// Token: 0x060001DB RID: 475 RVA: 0x000067E4 File Offset: 0x000049E4
		public static long RoundUp(long value, long unit)
		{
			return (value + (unit - 1L)) / unit * unit;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x000067F0 File Offset: 0x000049F0
		public static int RoundUp(int value, int unit)
		{
			return (value + (unit - 1)) / unit * unit;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x000067FB File Offset: 0x000049FB
		public static long RoundDown(long value, long unit)
		{
			return value / unit * unit;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00006802 File Offset: 0x00004A02
		public static int Ceil(int numerator, int denominator)
		{
			return (numerator + (denominator - 1)) / denominator;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000680B File Offset: 0x00004A0B
		public static uint Ceil(uint numerator, uint denominator)
		{
			return (numerator + (denominator - 1U)) / denominator;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00006814 File Offset: 0x00004A14
		public static long Ceil(long numerator, long denominator)
		{
			return (numerator + (denominator - 1L)) / denominator;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00006820 File Offset: 0x00004A20
		public static int Log2(uint val)
		{
			if (val == 0U)
			{
				throw new ArgumentException("Cannot calculate log of Zero", "val");
			}
			int num = 0;
			while ((val & 1U) != 1U)
			{
				val >>= 1;
				num++;
			}
			if (val == 1U)
			{
				return num;
			}
			throw new ArgumentException("Input is not a power of Two", "val");
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00006868 File Offset: 0x00004A68
		public static int Log2(int val)
		{
			if (val == 0)
			{
				throw new ArgumentException("Cannot calculate log of Zero", "val");
			}
			int num = 0;
			while ((val & 1) != 1)
			{
				val >>= 1;
				num++;
			}
			if (val == 1)
			{
				return num;
			}
			throw new ArgumentException("Input is not a power of Two", "val");
		}
	}
}
