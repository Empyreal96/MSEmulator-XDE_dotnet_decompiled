using System;

namespace System.Management.Automation
{
	// Token: 0x0200063D RID: 1597
	internal static class DecimalOps
	{
		// Token: 0x06004520 RID: 17696 RVA: 0x00172A50 File Offset: 0x00170C50
		internal static object Add(decimal lhs, decimal rhs)
		{
			object result;
			try
			{
				result = lhs + rhs;
			}
			catch (OverflowException ex)
			{
				throw new RuntimeException(ex.Message, ex);
			}
			return result;
		}

		// Token: 0x06004521 RID: 17697 RVA: 0x00172A8C File Offset: 0x00170C8C
		internal static object Sub(decimal lhs, decimal rhs)
		{
			object result;
			try
			{
				result = lhs - rhs;
			}
			catch (OverflowException ex)
			{
				throw new RuntimeException(ex.Message, ex);
			}
			return result;
		}

		// Token: 0x06004522 RID: 17698 RVA: 0x00172AC8 File Offset: 0x00170CC8
		internal static object Multiply(decimal lhs, decimal rhs)
		{
			object result;
			try
			{
				result = lhs * rhs;
			}
			catch (OverflowException ex)
			{
				throw new RuntimeException(ex.Message, ex);
			}
			return result;
		}

		// Token: 0x06004523 RID: 17699 RVA: 0x00172B04 File Offset: 0x00170D04
		internal static object Divide(decimal lhs, decimal rhs)
		{
			object result;
			try
			{
				result = lhs / rhs;
			}
			catch (OverflowException ex)
			{
				throw new RuntimeException(ex.Message, ex);
			}
			catch (DivideByZeroException ex2)
			{
				throw new RuntimeException(ex2.Message, ex2);
			}
			return result;
		}

		// Token: 0x06004524 RID: 17700 RVA: 0x00172B5C File Offset: 0x00170D5C
		internal static object Remainder(decimal lhs, decimal rhs)
		{
			object result;
			try
			{
				result = lhs % rhs;
			}
			catch (OverflowException ex)
			{
				throw new RuntimeException(ex.Message, ex);
			}
			catch (DivideByZeroException ex2)
			{
				throw new RuntimeException(ex2.Message, ex2);
			}
			return result;
		}

		// Token: 0x06004525 RID: 17701 RVA: 0x00172BB4 File Offset: 0x00170DB4
		internal static object BNot(decimal val)
		{
			if (val <= 2147483647m && val >= -2147483648m)
			{
				return ~LanguagePrimitives.ConvertTo<int>(val);
			}
			if (val <= 4294967295m && val >= 0m)
			{
				return ~LanguagePrimitives.ConvertTo<uint>(val);
			}
			if (val <= 9223372036854775807m && val >= -9223372036854775808m)
			{
				return ~LanguagePrimitives.ConvertTo<long>(val);
			}
			if (val <= 18446744073709551615m && val >= 0m)
			{
				return ~LanguagePrimitives.ConvertTo<ulong>(val);
			}
			LanguagePrimitives.ThrowInvalidCastException(val, typeof(int));
			return null;
		}

		// Token: 0x06004526 RID: 17702 RVA: 0x00172CB0 File Offset: 0x00170EB0
		internal static object BOr(decimal lhs, decimal rhs)
		{
			ulong num = DecimalOps.ConvertToUlong(lhs);
			ulong num2 = DecimalOps.ConvertToUlong(rhs);
			if (lhs < 0m || rhs < 0m)
			{
				return (long)(num | num2);
			}
			return num | num2;
		}

		// Token: 0x06004527 RID: 17703 RVA: 0x00172CF8 File Offset: 0x00170EF8
		internal static object BXor(decimal lhs, decimal rhs)
		{
			ulong num = DecimalOps.ConvertToUlong(lhs);
			ulong num2 = DecimalOps.ConvertToUlong(rhs);
			if (lhs < 0m || rhs < 0m)
			{
				return (long)(num ^ num2);
			}
			return num ^ num2;
		}

		// Token: 0x06004528 RID: 17704 RVA: 0x00172D40 File Offset: 0x00170F40
		internal static object BAnd(decimal lhs, decimal rhs)
		{
			ulong num = DecimalOps.ConvertToUlong(lhs);
			ulong num2 = DecimalOps.ConvertToUlong(rhs);
			if (lhs < 0m || rhs < 0m)
			{
				return (long)(num & num2);
			}
			return num & num2;
		}

		// Token: 0x06004529 RID: 17705 RVA: 0x00172D88 File Offset: 0x00170F88
		private static ulong ConvertToUlong(decimal val)
		{
			if (val < 0m)
			{
				return (ulong)LanguagePrimitives.ConvertTo<long>(val);
			}
			return LanguagePrimitives.ConvertTo<ulong>(val);
		}

		// Token: 0x0600452A RID: 17706 RVA: 0x00172DBC File Offset: 0x00170FBC
		internal static object LeftShift(decimal val, int count)
		{
			if (val <= 2147483647m && val >= -2147483648m)
			{
				return LanguagePrimitives.ConvertTo<int>(val) << count;
			}
			if (val <= 4294967295m && val >= 0m)
			{
				return LanguagePrimitives.ConvertTo<uint>(val) << count;
			}
			if (val <= 9223372036854775807m && val >= -9223372036854775808m)
			{
				return LanguagePrimitives.ConvertTo<long>(val) << count;
			}
			if (val <= 18446744073709551615m && val >= 0m)
			{
				return LanguagePrimitives.ConvertTo<ulong>(val) << count;
			}
			LanguagePrimitives.ThrowInvalidCastException(val, typeof(int));
			return null;
		}

		// Token: 0x0600452B RID: 17707 RVA: 0x00172EC8 File Offset: 0x001710C8
		internal static object RightShift(decimal val, int count)
		{
			if (val <= 2147483647m && val >= -2147483648m)
			{
				return LanguagePrimitives.ConvertTo<int>(val) >> count;
			}
			if (val <= 4294967295m && val >= 0m)
			{
				return LanguagePrimitives.ConvertTo<uint>(val) >> count;
			}
			if (val <= 9223372036854775807m && val >= -9223372036854775808m)
			{
				return LanguagePrimitives.ConvertTo<long>(val) >> count;
			}
			if (val <= 18446744073709551615m && val >= 0m)
			{
				return LanguagePrimitives.ConvertTo<ulong>(val) >> count;
			}
			LanguagePrimitives.ThrowInvalidCastException(val, typeof(int));
			return null;
		}

		// Token: 0x0600452C RID: 17708 RVA: 0x00172FD1 File Offset: 0x001711D1
		internal static object CompareEq(decimal lhs, decimal rhs)
		{
			if (!(lhs == rhs))
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x0600452D RID: 17709 RVA: 0x00172FE7 File Offset: 0x001711E7
		internal static object CompareNe(decimal lhs, decimal rhs)
		{
			if (!(lhs != rhs))
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x0600452E RID: 17710 RVA: 0x00172FFD File Offset: 0x001711FD
		internal static object CompareLt(decimal lhs, decimal rhs)
		{
			if (!(lhs < rhs))
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x0600452F RID: 17711 RVA: 0x00173013 File Offset: 0x00171213
		internal static object CompareLe(decimal lhs, decimal rhs)
		{
			if (!(lhs <= rhs))
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x06004530 RID: 17712 RVA: 0x00173029 File Offset: 0x00171229
		internal static object CompareGt(decimal lhs, decimal rhs)
		{
			if (!(lhs > rhs))
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x06004531 RID: 17713 RVA: 0x0017303F File Offset: 0x0017123F
		internal static object CompareGe(decimal lhs, decimal rhs)
		{
			if (!(lhs >= rhs))
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x06004532 RID: 17714 RVA: 0x00173058 File Offset: 0x00171258
		private static object CompareWithDouble(decimal left, double right, Func<double, double, object> doubleComparer, Func<decimal, decimal, object> decimalComparer)
		{
			decimal arg;
			try
			{
				arg = (decimal)right;
			}
			catch (OverflowException)
			{
				return doubleComparer((double)left, right);
			}
			return decimalComparer(left, arg);
		}

		// Token: 0x06004533 RID: 17715 RVA: 0x0017309C File Offset: 0x0017129C
		private static object CompareWithDouble(double left, decimal right, Func<double, double, object> doubleComparer, Func<decimal, decimal, object> decimalComparer)
		{
			decimal arg;
			try
			{
				arg = (decimal)left;
			}
			catch (OverflowException)
			{
				return doubleComparer(left, (double)right);
			}
			return decimalComparer(arg, right);
		}

		// Token: 0x06004534 RID: 17716 RVA: 0x001730E0 File Offset: 0x001712E0
		internal static object CompareEq1(double lhs, decimal rhs)
		{
			return DecimalOps.CompareWithDouble(lhs, rhs, new Func<double, double, object>(DoubleOps.CompareEq), new Func<decimal, decimal, object>(DecimalOps.CompareEq));
		}

		// Token: 0x06004535 RID: 17717 RVA: 0x00173101 File Offset: 0x00171301
		internal static object CompareNe1(double lhs, decimal rhs)
		{
			return DecimalOps.CompareWithDouble(lhs, rhs, new Func<double, double, object>(DoubleOps.CompareNe), new Func<decimal, decimal, object>(DecimalOps.CompareNe));
		}

		// Token: 0x06004536 RID: 17718 RVA: 0x00173122 File Offset: 0x00171322
		internal static object CompareLt1(double lhs, decimal rhs)
		{
			return DecimalOps.CompareWithDouble(lhs, rhs, new Func<double, double, object>(DoubleOps.CompareLt), new Func<decimal, decimal, object>(DecimalOps.CompareLt));
		}

		// Token: 0x06004537 RID: 17719 RVA: 0x00173143 File Offset: 0x00171343
		internal static object CompareLe1(double lhs, decimal rhs)
		{
			return DecimalOps.CompareWithDouble(lhs, rhs, new Func<double, double, object>(DoubleOps.CompareLe), new Func<decimal, decimal, object>(DecimalOps.CompareLe));
		}

		// Token: 0x06004538 RID: 17720 RVA: 0x00173164 File Offset: 0x00171364
		internal static object CompareGt1(double lhs, decimal rhs)
		{
			return DecimalOps.CompareWithDouble(lhs, rhs, new Func<double, double, object>(DoubleOps.CompareGt), new Func<decimal, decimal, object>(DecimalOps.CompareGt));
		}

		// Token: 0x06004539 RID: 17721 RVA: 0x00173185 File Offset: 0x00171385
		internal static object CompareGe1(double lhs, decimal rhs)
		{
			return DecimalOps.CompareWithDouble(lhs, rhs, new Func<double, double, object>(DoubleOps.CompareGe), new Func<decimal, decimal, object>(DecimalOps.CompareGe));
		}

		// Token: 0x0600453A RID: 17722 RVA: 0x001731A6 File Offset: 0x001713A6
		internal static object CompareEq2(decimal lhs, double rhs)
		{
			return DecimalOps.CompareWithDouble(lhs, rhs, new Func<double, double, object>(DoubleOps.CompareEq), new Func<decimal, decimal, object>(DecimalOps.CompareEq));
		}

		// Token: 0x0600453B RID: 17723 RVA: 0x001731C7 File Offset: 0x001713C7
		internal static object CompareNe2(decimal lhs, double rhs)
		{
			return DecimalOps.CompareWithDouble(lhs, rhs, new Func<double, double, object>(DoubleOps.CompareNe), new Func<decimal, decimal, object>(DecimalOps.CompareNe));
		}

		// Token: 0x0600453C RID: 17724 RVA: 0x001731E8 File Offset: 0x001713E8
		internal static object CompareLt2(decimal lhs, double rhs)
		{
			return DecimalOps.CompareWithDouble(lhs, rhs, new Func<double, double, object>(DoubleOps.CompareLt), new Func<decimal, decimal, object>(DecimalOps.CompareLt));
		}

		// Token: 0x0600453D RID: 17725 RVA: 0x00173209 File Offset: 0x00171409
		internal static object CompareLe2(decimal lhs, double rhs)
		{
			return DecimalOps.CompareWithDouble(lhs, rhs, new Func<double, double, object>(DoubleOps.CompareLe), new Func<decimal, decimal, object>(DecimalOps.CompareLe));
		}

		// Token: 0x0600453E RID: 17726 RVA: 0x0017322A File Offset: 0x0017142A
		internal static object CompareGt2(decimal lhs, double rhs)
		{
			return DecimalOps.CompareWithDouble(lhs, rhs, new Func<double, double, object>(DoubleOps.CompareGt), new Func<decimal, decimal, object>(DecimalOps.CompareGt));
		}

		// Token: 0x0600453F RID: 17727 RVA: 0x0017324B File Offset: 0x0017144B
		internal static object CompareGe2(decimal lhs, double rhs)
		{
			return DecimalOps.CompareWithDouble(lhs, rhs, new Func<double, double, object>(DoubleOps.CompareGe), new Func<decimal, decimal, object>(DecimalOps.CompareGe));
		}
	}
}
