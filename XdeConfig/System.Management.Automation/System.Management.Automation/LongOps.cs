using System;
using System.Numerics;

namespace System.Management.Automation
{
	// Token: 0x0200063B RID: 1595
	internal static class LongOps
	{
		// Token: 0x0600450A RID: 17674 RVA: 0x0017266C File Offset: 0x0017086C
		internal static object Add(long lhs, long rhs)
		{
			decimal num = lhs + rhs;
			if (num <= 9223372036854775807m && num >= -9223372036854775808m)
			{
				return (long)num;
			}
			return (double)num;
		}

		// Token: 0x0600450B RID: 17675 RVA: 0x001726D0 File Offset: 0x001708D0
		internal static object Sub(long lhs, long rhs)
		{
			decimal num = lhs - rhs;
			if (num <= 9223372036854775807m && num >= -9223372036854775808m)
			{
				return (long)num;
			}
			return (double)num;
		}

		// Token: 0x0600450C RID: 17676 RVA: 0x00172734 File Offset: 0x00170934
		internal static object Multiply(long lhs, long rhs)
		{
			BigInteger left = lhs;
			BigInteger right = rhs;
			BigInteger bigInteger = left * right;
			if (bigInteger <= 9223372036854775807L && bigInteger >= -9223372036854775808L)
			{
				return (long)bigInteger;
			}
			return (double)bigInteger;
		}

		// Token: 0x0600450D RID: 17677 RVA: 0x00172794 File Offset: 0x00170994
		internal static object Divide(long lhs, long rhs)
		{
			if (rhs == 0L)
			{
				DivideByZeroException ex = new DivideByZeroException();
				throw new RuntimeException(ex.Message, ex);
			}
			if (lhs == -9223372036854775808L && rhs == -1L)
			{
				return (double)lhs / (double)rhs;
			}
			if (lhs % rhs == 0L)
			{
				return lhs / rhs;
			}
			return (double)lhs / (double)rhs;
		}

		// Token: 0x0600450E RID: 17678 RVA: 0x001727F0 File Offset: 0x001709F0
		internal static object Remainder(long lhs, long rhs)
		{
			if (rhs == 0L)
			{
				DivideByZeroException ex = new DivideByZeroException();
				throw new RuntimeException(ex.Message, ex);
			}
			if (lhs == -9223372036854775808L && rhs == -1L)
			{
				return 0L;
			}
			return lhs % rhs;
		}

		// Token: 0x0600450F RID: 17679 RVA: 0x00172836 File Offset: 0x00170A36
		internal static object CompareEq(long lhs, long rhs)
		{
			if (lhs != rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x06004510 RID: 17680 RVA: 0x00172847 File Offset: 0x00170A47
		internal static object CompareNe(long lhs, long rhs)
		{
			if (lhs == rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x06004511 RID: 17681 RVA: 0x00172858 File Offset: 0x00170A58
		internal static object CompareLt(long lhs, long rhs)
		{
			if (lhs >= rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x06004512 RID: 17682 RVA: 0x00172869 File Offset: 0x00170A69
		internal static object CompareLe(long lhs, long rhs)
		{
			if (lhs > rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x06004513 RID: 17683 RVA: 0x0017287A File Offset: 0x00170A7A
		internal static object CompareGt(long lhs, long rhs)
		{
			if (lhs <= rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x06004514 RID: 17684 RVA: 0x0017288B File Offset: 0x00170A8B
		internal static object CompareGe(long lhs, long rhs)
		{
			if (lhs < rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}
	}
}
