using System;

namespace System.Management.Automation
{
	// Token: 0x0200063E RID: 1598
	internal static class DoubleOps
	{
		// Token: 0x06004540 RID: 17728 RVA: 0x0017326C File Offset: 0x0017146C
		internal static object Add(double lhs, double rhs)
		{
			return lhs + rhs;
		}

		// Token: 0x06004541 RID: 17729 RVA: 0x00173276 File Offset: 0x00171476
		internal static object Sub(double lhs, double rhs)
		{
			return lhs - rhs;
		}

		// Token: 0x06004542 RID: 17730 RVA: 0x00173280 File Offset: 0x00171480
		internal static object Multiply(double lhs, double rhs)
		{
			return lhs * rhs;
		}

		// Token: 0x06004543 RID: 17731 RVA: 0x0017328A File Offset: 0x0017148A
		internal static object Divide(double lhs, double rhs)
		{
			return lhs / rhs;
		}

		// Token: 0x06004544 RID: 17732 RVA: 0x00173294 File Offset: 0x00171494
		internal static object Remainder(double lhs, double rhs)
		{
			return lhs % rhs;
		}

		// Token: 0x06004545 RID: 17733 RVA: 0x001732A0 File Offset: 0x001714A0
		internal static object BNot(double val)
		{
			try
			{
				if (val <= 2147483647.0 && val >= -2147483648.0)
				{
					return ~LanguagePrimitives.ConvertTo<int>(val);
				}
				if (val <= 4294967295.0 && val >= 0.0)
				{
					return ~LanguagePrimitives.ConvertTo<uint>(val);
				}
				if (val <= 9.223372036854776E+18 && val >= -9.223372036854776E+18)
				{
					return ~LanguagePrimitives.ConvertTo<long>(val);
				}
				if (val <= 1.8446744073709552E+19 && val >= 0.0)
				{
					return ~LanguagePrimitives.ConvertTo<ulong>(val);
				}
			}
			catch (OverflowException)
			{
			}
			LanguagePrimitives.ThrowInvalidCastException(val, typeof(ulong));
			return null;
		}

		// Token: 0x06004546 RID: 17734 RVA: 0x00173390 File Offset: 0x00171590
		internal static object BOr(double lhs, double rhs)
		{
			ulong num = DoubleOps.ConvertToUlong(lhs);
			ulong num2 = DoubleOps.ConvertToUlong(rhs);
			if (lhs < 0.0 || rhs < 0.0)
			{
				return (long)(num | num2);
			}
			return num | num2;
		}

		// Token: 0x06004547 RID: 17735 RVA: 0x001733D4 File Offset: 0x001715D4
		internal static object BXor(double lhs, double rhs)
		{
			ulong num = DoubleOps.ConvertToUlong(lhs);
			ulong num2 = DoubleOps.ConvertToUlong(rhs);
			if (lhs < 0.0 || rhs < 0.0)
			{
				return (long)(num ^ num2);
			}
			return num ^ num2;
		}

		// Token: 0x06004548 RID: 17736 RVA: 0x00173418 File Offset: 0x00171618
		internal static object BAnd(double lhs, double rhs)
		{
			ulong num = DoubleOps.ConvertToUlong(lhs);
			ulong num2 = DoubleOps.ConvertToUlong(rhs);
			if (lhs < 0.0 || rhs < 0.0)
			{
				return (long)(num & num2);
			}
			return num & num2;
		}

		// Token: 0x06004549 RID: 17737 RVA: 0x0017345C File Offset: 0x0017165C
		private static ulong ConvertToUlong(double val)
		{
			if (val < 0.0)
			{
				return (ulong)LanguagePrimitives.ConvertTo<long>(val);
			}
			return LanguagePrimitives.ConvertTo<ulong>(val);
		}

		// Token: 0x0600454A RID: 17738 RVA: 0x00173490 File Offset: 0x00171690
		internal static object LeftShift(double val, int count)
		{
			if (val <= 2147483647.0 && val >= -2147483648.0)
			{
				return LanguagePrimitives.ConvertTo<int>(val) << count;
			}
			if (val <= 4294967295.0 && val >= 0.0)
			{
				return LanguagePrimitives.ConvertTo<uint>(val) << count;
			}
			if (val <= 9.223372036854776E+18 && val >= -9.223372036854776E+18)
			{
				return LanguagePrimitives.ConvertTo<long>(val) << count;
			}
			if (val <= 1.8446744073709552E+19 && val >= 0.0)
			{
				return LanguagePrimitives.ConvertTo<ulong>(val) << count;
			}
			LanguagePrimitives.ThrowInvalidCastException(val, typeof(ulong));
			return null;
		}

		// Token: 0x0600454B RID: 17739 RVA: 0x0017356C File Offset: 0x0017176C
		internal static object RightShift(double val, int count)
		{
			if (val <= 2147483647.0 && val >= -2147483648.0)
			{
				return LanguagePrimitives.ConvertTo<int>(val) >> count;
			}
			if (val <= 4294967295.0 && val >= 0.0)
			{
				return LanguagePrimitives.ConvertTo<uint>(val) >> count;
			}
			if (val <= 9.223372036854776E+18 && val >= -9.223372036854776E+18)
			{
				return LanguagePrimitives.ConvertTo<long>(val) >> count;
			}
			if (val <= 1.8446744073709552E+19 && val >= 0.0)
			{
				return LanguagePrimitives.ConvertTo<ulong>(val) >> count;
			}
			LanguagePrimitives.ThrowInvalidCastException(val, typeof(ulong));
			return null;
		}

		// Token: 0x0600454C RID: 17740 RVA: 0x00173648 File Offset: 0x00171848
		internal static object CompareEq(double lhs, double rhs)
		{
			if (lhs != rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x0600454D RID: 17741 RVA: 0x00173659 File Offset: 0x00171859
		internal static object CompareNe(double lhs, double rhs)
		{
			if (lhs == rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x0600454E RID: 17742 RVA: 0x0017366A File Offset: 0x0017186A
		internal static object CompareLt(double lhs, double rhs)
		{
			if (lhs >= rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x0600454F RID: 17743 RVA: 0x0017367B File Offset: 0x0017187B
		internal static object CompareLe(double lhs, double rhs)
		{
			if (lhs > rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x06004550 RID: 17744 RVA: 0x0017368C File Offset: 0x0017188C
		internal static object CompareGt(double lhs, double rhs)
		{
			if (lhs <= rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x06004551 RID: 17745 RVA: 0x0017369D File Offset: 0x0017189D
		internal static object CompareGe(double lhs, double rhs)
		{
			if (lhs < rhs)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}
	}
}
