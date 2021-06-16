using System;
using System.Numerics;

namespace System.Management.Automation
{
	// Token: 0x0200063C RID: 1596
	internal static class ULongOps
	{
		// Token: 0x06004515 RID: 17685 RVA: 0x0017289C File Offset: 0x00170A9C
		internal static object Add(ulong lhs, ulong rhs)
		{
			decimal num = lhs + rhs;
			if (num <= 18446744073709551615m)
			{
				return (ulong)num;
			}
			return (double)num;
		}

		// Token: 0x06004516 RID: 17686 RVA: 0x001728E8 File Offset: 0x00170AE8
		internal static object Sub(ulong lhs, ulong rhs)
		{
			decimal num = lhs - rhs;
			if (num >= 0m)
			{
				return (ulong)num;
			}
			return (double)num;
		}

		// Token: 0x06004517 RID: 17687 RVA: 0x00172930 File Offset: 0x00170B30
		internal static object Multiply(ulong lhs, ulong rhs)
		{
			BigInteger left = lhs;
			BigInteger right = rhs;
			BigInteger bigInteger = left * right;
			if (bigInteger <= 18446744073709551615UL)
			{
				return (ulong)bigInteger;
			}
			return (double)bigInteger;
		}

		// Token: 0x06004518 RID: 17688 RVA: 0x00172978 File Offset: 0x00170B78
		internal static object Divide(ulong lhs, ulong rhs)
		{
			if (rhs == 0UL)
			{
				DivideByZeroException ex = new DivideByZeroException();
				throw new RuntimeException(ex.Message, ex);
			}
			if (lhs % rhs == 0UL)
			{
				return lhs / rhs;
			}
			return lhs / rhs;
		}

		// Token: 0x06004519 RID: 17689 RVA: 0x001729BC File Offset: 0x00170BBC
		internal static object Remainder(ulong lhs, ulong rhs)
		{
			if (rhs == 0UL)
			{
				DivideByZeroException ex = new DivideByZeroException();
				throw new RuntimeException(ex.Message, ex);
			}
			return lhs % rhs;
		}

		// Token: 0x0600451A RID: 17690 RVA: 0x001729E9 File Offset: 0x00170BE9
		internal static object CompareEq(ulong lhs, ulong rhs)
		{
			if (lhs != rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x0600451B RID: 17691 RVA: 0x001729FA File Offset: 0x00170BFA
		internal static object CompareNe(ulong lhs, ulong rhs)
		{
			if (lhs == rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x0600451C RID: 17692 RVA: 0x00172A0B File Offset: 0x00170C0B
		internal static object CompareLt(ulong lhs, ulong rhs)
		{
			if (lhs >= rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x0600451D RID: 17693 RVA: 0x00172A1C File Offset: 0x00170C1C
		internal static object CompareLe(ulong lhs, ulong rhs)
		{
			if (lhs > rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x0600451E RID: 17694 RVA: 0x00172A2D File Offset: 0x00170C2D
		internal static object CompareGt(ulong lhs, ulong rhs)
		{
			if (lhs <= rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x0600451F RID: 17695 RVA: 0x00172A3E File Offset: 0x00170C3E
		internal static object CompareGe(ulong lhs, ulong rhs)
		{
			if (lhs < rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}
	}
}
