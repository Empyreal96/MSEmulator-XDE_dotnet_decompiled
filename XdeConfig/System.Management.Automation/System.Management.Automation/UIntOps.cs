using System;

namespace System.Management.Automation
{
	// Token: 0x0200063A RID: 1594
	internal static class UIntOps
	{
		// Token: 0x060044FF RID: 17663 RVA: 0x00172520 File Offset: 0x00170720
		internal static object Add(uint lhs, uint rhs)
		{
			ulong num = (ulong)lhs + (ulong)rhs;
			if (num <= (ulong)-1)
			{
				return (uint)num;
			}
			return num;
		}

		// Token: 0x06004500 RID: 17664 RVA: 0x00172548 File Offset: 0x00170748
		internal static object Sub(uint lhs, uint rhs)
		{
			long num = (long)((ulong)lhs - (ulong)rhs);
			if (num >= 0L)
			{
				return (uint)num;
			}
			return (double)num;
		}

		// Token: 0x06004501 RID: 17665 RVA: 0x00172570 File Offset: 0x00170770
		internal static object Multiply(uint lhs, uint rhs)
		{
			ulong num = (ulong)lhs * (ulong)rhs;
			if (num <= (ulong)-1)
			{
				return (uint)num;
			}
			return num;
		}

		// Token: 0x06004502 RID: 17666 RVA: 0x00172598 File Offset: 0x00170798
		internal static object Divide(uint lhs, uint rhs)
		{
			if (rhs == 0U)
			{
				DivideByZeroException ex = new DivideByZeroException();
				throw new RuntimeException(ex.Message, ex);
			}
			if (lhs % rhs == 0U)
			{
				return lhs / rhs;
			}
			return lhs / rhs;
		}

		// Token: 0x06004503 RID: 17667 RVA: 0x001725D8 File Offset: 0x001707D8
		internal static object Remainder(uint lhs, uint rhs)
		{
			if (rhs == 0U)
			{
				DivideByZeroException ex = new DivideByZeroException();
				throw new RuntimeException(ex.Message, ex);
			}
			return lhs % rhs;
		}

		// Token: 0x06004504 RID: 17668 RVA: 0x00172603 File Offset: 0x00170803
		internal static object CompareEq(uint lhs, uint rhs)
		{
			if (lhs != rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x06004505 RID: 17669 RVA: 0x00172614 File Offset: 0x00170814
		internal static object CompareNe(uint lhs, uint rhs)
		{
			if (lhs == rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x06004506 RID: 17670 RVA: 0x00172625 File Offset: 0x00170825
		internal static object CompareLt(uint lhs, uint rhs)
		{
			if (lhs >= rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x06004507 RID: 17671 RVA: 0x00172636 File Offset: 0x00170836
		internal static object CompareLe(uint lhs, uint rhs)
		{
			if (lhs > rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x06004508 RID: 17672 RVA: 0x00172647 File Offset: 0x00170847
		internal static object CompareGt(uint lhs, uint rhs)
		{
			if (lhs <= rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x06004509 RID: 17673 RVA: 0x00172658 File Offset: 0x00170858
		internal static object CompareGe(uint lhs, uint rhs)
		{
			if (lhs < rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}
	}
}
