using System;

namespace System.Management.Automation
{
	// Token: 0x02000639 RID: 1593
	internal static class IntOps
	{
		// Token: 0x060044F3 RID: 17651 RVA: 0x00172314 File Offset: 0x00170514
		internal static object Add(int lhs, int rhs)
		{
			long num = (long)lhs + (long)rhs;
			if (num <= 2147483647L && num >= -2147483648L)
			{
				return (int)num;
			}
			return (double)num;
		}

		// Token: 0x060044F4 RID: 17652 RVA: 0x00172348 File Offset: 0x00170548
		internal static object Sub(int lhs, int rhs)
		{
			long num = (long)lhs - (long)rhs;
			if (num <= 2147483647L && num >= -2147483648L)
			{
				return (int)num;
			}
			return (double)num;
		}

		// Token: 0x060044F5 RID: 17653 RVA: 0x0017237C File Offset: 0x0017057C
		internal static object Multiply(int lhs, int rhs)
		{
			long num = (long)lhs * (long)rhs;
			if (num <= 2147483647L && num >= -2147483648L)
			{
				return (int)num;
			}
			return (double)num;
		}

		// Token: 0x060044F6 RID: 17654 RVA: 0x001723B0 File Offset: 0x001705B0
		internal static object Divide(int lhs, int rhs)
		{
			if (rhs == 0)
			{
				DivideByZeroException ex = new DivideByZeroException();
				throw new RuntimeException(ex.Message, ex);
			}
			if (lhs == -2147483648 && rhs == -1)
			{
				return (double)lhs / (double)rhs;
			}
			if (lhs % rhs == 0)
			{
				return lhs / rhs;
			}
			return (double)lhs / (double)rhs;
		}

		// Token: 0x060044F7 RID: 17655 RVA: 0x00172404 File Offset: 0x00170604
		internal static object Remainder(int lhs, int rhs)
		{
			if (rhs == 0)
			{
				DivideByZeroException ex = new DivideByZeroException();
				throw new RuntimeException(ex.Message, ex);
			}
			if (lhs == -2147483648 && rhs == -1)
			{
				return 0;
			}
			return lhs % rhs;
		}

		// Token: 0x060044F8 RID: 17656 RVA: 0x00172442 File Offset: 0x00170642
		internal static object CompareEq(int lhs, int rhs)
		{
			if (lhs != rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x060044F9 RID: 17657 RVA: 0x00172453 File Offset: 0x00170653
		internal static object CompareNe(int lhs, int rhs)
		{
			if (lhs == rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x060044FA RID: 17658 RVA: 0x00172464 File Offset: 0x00170664
		internal static object CompareLt(int lhs, int rhs)
		{
			if (lhs >= rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x060044FB RID: 17659 RVA: 0x00172475 File Offset: 0x00170675
		internal static object CompareLe(int lhs, int rhs)
		{
			if (lhs > rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x060044FC RID: 17660 RVA: 0x00172486 File Offset: 0x00170686
		internal static object CompareGt(int lhs, int rhs)
		{
			if (lhs <= rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x060044FD RID: 17661 RVA: 0x00172497 File Offset: 0x00170697
		internal static object CompareGe(int lhs, int rhs)
		{
			if (lhs < rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x060044FE RID: 17662 RVA: 0x001724A8 File Offset: 0x001706A8
		internal static object[] Range(int lower, int upper)
		{
			if (lower == upper)
			{
				return new object[]
				{
					lower
				};
			}
			int num = Math.Abs(checked(upper - lower));
			object[] array = new object[num + 1];
			if (lower > upper)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = lower--;
				}
			}
			else
			{
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = lower++;
				}
			}
			return array;
		}
	}
}
